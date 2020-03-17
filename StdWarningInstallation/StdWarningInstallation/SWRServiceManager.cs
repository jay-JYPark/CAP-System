using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml;

using StdWarningInstallation.DataClass;

namespace StdWarningInstallation
{
    class SWRServiceManager
    {
        enum RequestCode
        {
            /// <summary>
            /// 기상통보문조회
            /// </summary>
            WeatherWarningItem = 1,
            /// <summary>
            /// 기상특보코드조회
            /// </summary>
            SpecialNewsCode,
            /// <summary>
            /// 기상특보목록조회
            /// </summary>
            WeatherWarningListRequest,
        }

        public event EventHandler<SWREventArgs> NotifySWRReceived;

        private static SWRServiceManager thisObj = null;
        private static Mutex mutex = new Mutex();

        private string currentServiceLink = "http://newsky2.kma.go.kr/service/WetherSpcnwsInfoService/";
        private string currentServiceKey = string.Empty;
        //private string currentServiceKey = "6lIn11vBfAG7cH0zgE5Skyqh5bClzO%2Fhi%2Fub12cL6TvJdmp6QC4QQW0vbhzWUK%2B7OEVfqQ92SI9MNfHophhP4g%3D%3D";

        private Thread swrDataProcessingThread = null;                         // 기상특보 데이터 처리 쓰레드
        private bool isSWRDataProcessingContinue = false;
        private uint requestCycleBySec = 600;          // 기상특보 조회 주기(초 단위) - 디폴트는 10분
        private string currentLatestProfileID = string.Empty;
        private string newDetectedReportID = string.Empty;



        /// <summary>
        /// 생성자
        /// </summary>
        /// <returns></returns>
        private SWRServiceManager()
        {
            FileLogManager.GetInstance().WriteLog("[SWRServiceManager] 시작");
        }

        /// <summary>
        /// 싱글톤 인스턴스 취득
        /// </summary>
        /// <returns>기상특보 관리 클래스의 인스턴스</returns>
        public static SWRServiceManager GetInstance()
        {
            mutex.WaitOne();

            if (thisObj == null)
            {
                thisObj = new SWRServiceManager();
            }

            mutex.ReleaseMutex();
            return thisObj;
        }
        
        /// <summary>
        /// 파괴자
        /// 정상적이지 못한 종료의 경우, 이벤트 핸들러 제거
        /// </summary>
        ~SWRServiceManager()
        {
            FileLogManager.GetInstance().WriteLog("[SWRServiceManager] 종료");

            EndCheckingIssue();
        }


        /// <summary>
        /// 기상특보 웹서비스 접속 정보 설정
        /// </summary>
        /// <param name="info"></param>
        public void SetConnectionInfo(ConfigSWRData info)
        {
            System.Diagnostics.Debug.Assert(info != null);
            FileLogManager.GetInstance().WriteLog("[SWRServiceManager] SetConnectionInfo( )");

            this.currentServiceKey = info.ServiceKey;
            this.requestCycleBySec = (uint)(info.CycleTimeMinute * 60);
        }
        /// <summary>
        /// 기상특보 마지막으로 조회된 아이디 정보.
        /// </summary>
        /// <param name="profileID"></param>
        public void SetLatestProfileID(string profileID)
        {
            this.currentLatestProfileID = profileID;
        }


        /// <summary>
        /// 기상특보 데이터 감시 쓰레딩 시작.
        /// </summary>
        public void StartCheckingIssue(string latestReportID)
        {
            FileLogManager.GetInstance().WriteLog("[SWRServiceManager] StartCheckingIssue( latestReportID=[" + latestReportID + "] )");

            this.currentLatestProfileID = latestReportID;

            this.isSWRDataProcessingContinue = true;
            if (this.swrDataProcessingThread == null)
            {
                this.swrDataProcessingThread = new Thread(new ThreadStart(SWRDataProcessing));
                this.swrDataProcessingThread.IsBackground = true;
                this.swrDataProcessingThread.Name = "SWRDataProcessingThread";
                this.swrDataProcessingThread.Start();
            }
        }
        /// <summary>
        /// 기상특보 데이터 감시 쓰레딩 종료.
        /// </summary>
        public void EndCheckingIssue()
        {
            FileLogManager.GetInstance().WriteLog("[SWRServiceManager] EndCheckingIssue( )");

            if (this.swrDataProcessingThread != null && this.swrDataProcessingThread.IsAlive)
            {
                this.isSWRDataProcessingContinue = false;
                bool terminated = this.swrDataProcessingThread.Join(500);
                if (!terminated)
                {
                    this.swrDataProcessingThread.Abort();
                }
            }
            this.swrDataProcessingThread = null;
        }


        /// <summary>
        /// 입력 경로의 데이터를 요청하고, 응답 코드까지 체크.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string RequestData(string url)
        {
            //FileLogManager.GetInstance().WriteLog("[SWRServiceManager] RequestData( url=[" + url + "] )");

            string reportXml = string.Empty;

            // 데이터 요청이 서버 상태에 따라 실패하는 경우가 있으므로, 3회 정도 재시도.
            for (int retryIndex = 0; retryIndex < 3; retryIndex++)
            {
                try
                {
                    HttpWebResponse response = null;
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                    request.Method = "GET";
                    request.ServicePoint.Expect100Continue = false;

                    response = request.GetResponse() as HttpWebResponse;
                    if (response == null)
                    {
                        FileLogManager.GetInstance().WriteLog("[SWRServiceManager] RequestData( request failure[" + (retryIndex + 1) + "회] )");
                        continue;
                    }
                    Stream responseStream = response.GetResponseStream();
                    if (responseStream == null)
                    {
                        FileLogManager.GetInstance().WriteLog("[SWRServiceManager] RequestData( GetResponseStream failure[" + (retryIndex + 1) + "회] )");
                        continue;
                    }

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(responseStream);
                    // 응답 코드 체크
                    int resState = CheckResponseState(xmlDoc);
                    if (resState != 0)
                    {
                        FileLogManager.GetInstance().WriteLog("[SWRServiceManager] RequestData( GetResponseStream - result=[" + resState + "] )");
                        return null;
                    }
                    reportXml = xmlDoc.InnerXml;
                    if (string.IsNullOrEmpty(reportXml))
                    {
                        System.Console.WriteLine("[SWRManager] RequestData( 리포트 데이터 오류=[InnerXml is Null] )");
                        FileLogManager.GetInstance().WriteLog("[SWRServiceManager] RequestData( 리포트 데이터 오류=[InnerXml is Null] )");
                        return null;
                    }

                    break;
                }
                catch (WebException webEx)
                {
                    System.Console.WriteLine("[SWRManager] RequestData( WebException=[" + webEx.Response.ResponseUri + "] )");
                    FileLogManager.GetInstance().WriteLog("[SWRServiceManager] RequestData( WebException=[" + webEx.Response.ResponseUri + "] )");

                    Thread.Sleep(1000);
                    continue;
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("[SWRManager] RequestData( Exception=[" + ex.ToString() + "] )");
                    FileLogManager.GetInstance().WriteLog("[SWRServiceManager] RequestData( WebException=[" + ex.ToString() + "] )");

                    reportXml = null;
                    break;
                }
            }

            return reportXml;
        }
        /// <summary>
        /// 기상특보 웹서비스에 기상특보코드조회 데이터 요청.
        /// </summary>
        public bool RequestSpecialNewsCode(ref List<SWRProfile> reportList)
        {
            //FileLogManager.GetInstance().WriteLog("[SWRServiceManager] RequestSpecialNewsCode( start )");

            Dictionary<string, SWRProfile> swrList = new Dictionary<string, SWRProfile>();

            if (reportList == null)
            {
                reportList = new List<SWRProfile>();
            }
            foreach (SWRProfile swr in reportList)
            {
                swrList.Add(swr.ID, swr);
            }

            try
            {
                foreach (SWRAnnounceArea area in BasisData.SwrAreaInfo.Values)
                {
                    string uriStr = MakeURLForSpecialNewsCode(area.AreaCode); // 기상특보코드조회 URL
                    string reportXml = RequestData(uriStr);
                    if (string.IsNullOrEmpty(reportXml))
                    {
                        System.Console.WriteLine("[SWRManager] RequestSpecialNewsCode( 리포트 데이터 오류=[InnerXml is Null] )");
                        return false;
                    }
                    // 기상특보통신문 데이터 중 새로운 특보 검출
                    List<SWRNewsCodeProfile> newsCodeReportList = null;
                    int newDataCount = ParsingSpecialNewsCode(reportXml, out newsCodeReportList);
                    if (newDataCount <= 0)
                    {
                        System.Console.WriteLine("[SWRManager] RequestSpecialNewsCode( 새로운 기상특보코드조회 데이터 없음. 영역=[" + area.AreaCode + "(" + area.AreaName + ")] )");
                        continue;
                    }

                    // 딕셔너리에 추가
                    foreach (SWRNewsCodeProfile newsCodeProfile in newsCodeReportList)
                    {
                        if (string.IsNullOrEmpty(newsCodeProfile.AreaCode))
                        {
                            FileLogManager.GetInstance().WriteLog("[SWRServiceManager] RequestSpecialNewsCode( 지역 코드 오류 )");
                            continue;
                        }
                        if (!swrList.ContainsKey(newsCodeProfile.ReportID))
                        {
                            continue;
                        }

                        if (swrList[newsCodeProfile.ReportID].TargetAreas == null)
                        {
                            swrList[newsCodeProfile.ReportID].TargetAreas = string.Empty;
                        }
                        if (swrList[newsCodeProfile.ReportID].TargetAreas.Contains(newsCodeProfile.AreaCode))
                        {
                            continue;
                        }

                        System.Console.WriteLine("[SWRServiceManager] RequestSpecialNewsCode( 특보 추가=[" + newsCodeProfile.AreaCode + "(" + newsCodeProfile.AreaName + ")/" + newsCodeProfile.KindCode + "] )");

                        // 특보구역코드 리스트.
                        if (swrList[newsCodeProfile.ReportID].TargetAreas == string.Empty)
                        {
                            swrList[newsCodeProfile.ReportID].TargetAreas = newsCodeProfile.AreaCode;
                        }
                        else
                        {
                            swrList[newsCodeProfile.ReportID].TargetAreas = swrList[newsCodeProfile.ReportID].TargetAreas + "," + newsCodeProfile.AreaCode;
                        }
                        // 특보종류코드
                        if (!string.IsNullOrEmpty(swrList[newsCodeProfile.ReportID].WarnKindCode) && swrList[newsCodeProfile.ReportID].WarnKindCode != newsCodeProfile.KindCode)
                        {
                            System.Console.WriteLine("[SWRServiceManager] RequestSpecialNewsCode( 동일한 특보 아이디에 특보 종류 정보가 다름. )");
                            FileLogManager.GetInstance().WriteLog("[SWRServiceManager] RequestSpecialNewsCode( 동일한 특보 아이디에 특보 종류 정보가 다름. )");
                        }
                        swrList[newsCodeProfile.ReportID].WarnKindCode = newsCodeProfile.KindCode;
                        // 특보강도코드
                        if (!string.IsNullOrEmpty(swrList[newsCodeProfile.ReportID].WarnStressCode) && swrList[newsCodeProfile.ReportID].WarnStressCode != newsCodeProfile.StressCode)
                        {
                            System.Console.WriteLine("[SWRServiceManager] RequestSpecialNewsCode( 동일한 특보 아이디에 특보 강도 정보가 다름. )");
                            FileLogManager.GetInstance().WriteLog("[SWRServiceManager] RequestSpecialNewsCode( 동일한 특보 아이디에 특보 강도 정보가 다름. )");
                        }
                        swrList[newsCodeProfile.ReportID].WarnStressCode = newsCodeProfile.StressCode;
                        // 특보발표코드
                        swrList[newsCodeProfile.ReportID].CommandCode = newsCodeProfile.CommandCode;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[SWRServiceManager] RequestSpecialNewsCode( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[SWRServiceManager] RequestSpecialNewsCode( " + ex.ToString() + " )");

                reportList = null;
                return false;
            }
            finally
            {
                //FileLogManager.GetInstance().WriteLog("[SWRServiceManager] RequestSpecialNewsCode( end )");
            }

            return true;
        }
        /// <summary>
        /// 기상특보 웹서비스에 기상특보통보문 데이터 요청(유효 데이터 체크 처리 포함)
        /// </summary>
        public bool RequestWeatherWarningItem(ref List<SWRProfile> reportList)
        {
            //FileLogManager.GetInstance().WriteLog("[SWRServiceManager] RequestWeatherWarningItem( start )");

            try
            {
                string uriStr = MakeURLForWeatherWarningItem();
                string reportXml = RequestData(uriStr);
                if (string.IsNullOrEmpty(reportXml))
                {
                    System.Console.WriteLine("[SWRManager] RequestWeatherWarningItem( 리포트 데이터 오류=[InnerXml is Null] )");
                    FileLogManager.GetInstance().WriteLog("[SWRServiceManager] RequestWeatherWarningItem( 리포트 데이터 오류=[InnerXml is Null] )");

                    return false;
                }

                // 기상특보통신문 데이터 중 새로운 특보 검출
                List<SWRProfile> newReportList = null;
                int result = ParsingWeatherWarningItem(reportXml, out newReportList);
                if (result != 0)
                {
                    System.Console.WriteLine("[SWRManager] RequestWeatherWarningItem( 데이터 파싱 에러 )");
                    FileLogManager.GetInstance().WriteLog("[SWRServiceManager] RequestWeatherWarningItem( 데이터 파싱 에러 )");

                    return false;
                }
                if (newReportList == null)
                {
                    FileLogManager.GetInstance().WriteLog("[SWRServiceManager] RequestWeatherWarningItem( 신규 검출 리포트 없음 )");
                    return true;
                }

                reportList = newReportList;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[SWRServiceManager] RequestWeatherWarningItem( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[SWRServiceManager] RequestWeatherWarningItem( " + ex.ToString() + " )");

                return false;
            }
            finally
            {
                //FileLogManager.GetInstance().WriteLog("[SWRServiceManager] RequestWeatherWarningItem( end )");
            }

            return true;
        }

        /// <summary>
        /// 기상특보코드 조회 URL 생성.
        /// </summary>
        /// <param name="areaCode"></param>
        /// <returns></returns>
        public string MakeURLForSpecialNewsCode(string areaCode)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.currentServiceLink);
            if (!this.currentServiceLink.EndsWith("/"))
            {
                builder.Append("/");
            }
            builder.Append("SpecialNewsCode");
            builder.Append("?ServiceKey=" + currentServiceKey);
            builder.Append("&fromTmFc=" + DateTime.Today.ToString("yyyyMMdd") + "&toTmFc=" + DateTime.Today.ToString("yyyyMMdd"));
            builder.Append("&areaCode=" + areaCode);
            builder.Append("&warningType=0");           // 0: 모든 종류의 특보
            builder.Append("&numOfRows=1000&pageNo=1"); // 생략하면 디폴트로 10건씩만 조회됨.

            return builder.ToString();
        }
        /// <summary>
        /// 기상특보통보문 조회 URL 생성.
        /// </summary>
        /// <returns></returns>
        public string MakeURLForWeatherWarningItem()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.currentServiceLink);
            if (!this.currentServiceLink.EndsWith("/"))
            {
                builder.Append("/");
            }
            builder.Append("WeatherWarningItem");
            builder.Append("?ServiceKey=" + currentServiceKey);
            builder.Append("&fromTmFc=" + DateTime.Today.ToString("yyyyMMdd") + "&toTmFc=" + DateTime.Today.ToString("yyyyMMdd"));
            builder.Append("&numOfRows=1000&pageNo=1"); // 생략하면 디폴트로 10건씩만 조회됨.

            return builder.ToString();
        }
        /// <summary>
        /// 기상 특보 데이터의 응답코드를 체크한다.
        /// 정상인 경우, 응답코드는 "0000"이고 응답메시지는 "OK".
        /// </summary>
        private int CheckResponseState(XmlDocument xmlDoc)
        {
            System.Diagnostics.Debug.Assert(xmlDoc != null);
            FileLogManager.GetInstance().WriteLog("[SWRServiceManager] CheckResponseState( start )");

            if (xmlDoc == null)
            {
                FileLogManager.GetInstance().WriteLog("[SWRServiceManager] CheckResponseState( 입력 파리미터 오류 )");
                return -1;
            }

            try
            {
                XmlNodeList xNodes = xmlDoc.SelectNodes("/response/header");
                if (xNodes == null)
                {
                    FileLogManager.GetInstance().WriteLog("[SWRServiceManager] CheckResponseState( 응답 정보가 존재하지 않음. )");
                    return -2;
                }

                foreach (XmlNode node1 in xNodes)
                {
                    string resultCode = string.Empty;
                    string resultMsg = string.Empty;

                    foreach (XmlNode node2 in node1.ChildNodes)
                    {
                        if (node2.Name == "resultCode")
                        {
                            resultCode = node2.InnerText;
                        }
                        else if (node2.Name == "resultMsg")
                        {
                            resultMsg = node2.InnerText;
                        }
                        else
                        {
                            System.Console.WriteLine("node.Name[{0}] : InnerText{1}", node2.Name, node2.InnerText);
                        }
                    }

                    if (resultCode != "0000" || resultMsg != "OK")
                    {
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[SWRServiceManager] CheckResponseState( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[SWRServiceManager] CheckResponseState( " + ex.ToString() + " )");

                return -99;
            }
            finally
            {
                FileLogManager.GetInstance().WriteLog("[SWRServiceManager] CheckResponseState( end )");
            }

            return 0;
        }

        /// <summary>
        /// 기상특보코드조회 데이터를 파싱.
        /// (※기능 변경으로 인해 미사용).
        /// </summary>
        /// <param name="reportXml"></param>
        /// <returns></returns>
        private int ParsingSpecialNewsCode(string reportXml, out List<SWRNewsCodeProfile> reportList)
        {
            System.Diagnostics.Debug.Assert(reportXml != null);
            //FileLogManager.GetInstance().WriteLog("[SWRServiceManager] ParsingSpecialNewsCode( start )");

            reportList = null;
            int savedCount = 0;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(reportXml);

                XmlNodeList itemsList = xmlDoc.SelectNodes("/response/body/items");
                if (itemsList == null)
                {
                    FileLogManager.GetInstance().WriteLog("[SWRServiceManager] ParsingSpecialNewsCode( 아이템 정보를 찾을 수 없음. )");

                    return -1;
                }

                reportList = new List<SWRNewsCodeProfile>();

                foreach (XmlNode items in itemsList)
                {
                    foreach (XmlNode item in items.ChildNodes)
                    {
                        if (item.Name != "item")
                        {
                            //FileLogManager.GetInstance().WriteLog("[SWRServiceManager] ParsingSpecialNewsCode( item.Name=[" + item.Name + "], item.InnerText=[" + item.InnerText + "] )");
                            continue;
                        }

                        bool isError = false;
                        SWRNewsCodeProfile report = new SWRNewsCodeProfile();

                        // 모든 정보를 다 파싱할 필요는 없음.
                        foreach (XmlNode node in item)
                        {
                            if (node.Name == "tmFc")
                            {
                                DateTime temp = new DateTime();
                                if (DateTime.TryParse(node.InnerText, out temp))
                                {
                                    report.AnnounceTime = temp;
                                }
                                else
                                {
                                    isError = true;
                                    break;
                                }
                            }
                            else if (node.Name == "tmSeq")
                            {
                                int temp = 0;
                                if (int.TryParse(node.InnerText, out temp))
                                {
                                    report.SequenceNo = temp;
                                }
                            }
                            else if (node.Name == "areaCode")
                            {
                                report.AreaCode = node.InnerText;
                            }
                            else if (node.Name == "areaName")
                            {
                                report.AreaName = node.InnerText;
                            }
                            else if (node.Name == "warnVar")
                            {
                                report.KindCode = node.InnerText;
                            }
                            else if (node.Name == "warnStress")
                            {
                                report.StressCode = node.InnerText;
                            }
                            else if (node.Name == "command")
                            {
                                report.CommandCode = node.InnerText;
                            }
                            else if (node.Name == "startTime")
                            {
                                DateTime temp = new DateTime();
                                if (DateTime.TryParse(node.InnerText, out temp))
                                {
                                    report.EffectStart = temp;
                                }
                            }
                            else if (node.Name == "endTime")
                            {
                                DateTime temp = new DateTime();
                                if (DateTime.TryParse(node.InnerText, out temp))
                                {
                                    report.EffectEnd = temp;
                                }
                            }
                            else if (node.Name == "allEndTime")
                            {
                                DateTime temp = new DateTime();
                                if (DateTime.TryParse(node.InnerText, out temp))
                                {
                                    report.AllClearTime = temp;
                                }
                            }
                            else if (node.Name == "cancle")
                            {
                                report.IsCancel = false;
                                if (node.InnerText == "1")
                                {
                                    report.IsCancel = true;
                                }
                            }
                            else
                            {
                                // 일단 여기에서는 무시
                            }
                        }
                        if (isError || report.SequenceNo < 1)
                        {
                            continue;
                        }

                        string newReportID = report.AnnounceTime.Year.ToString("0000") +
                                                report.AnnounceTime.Month.ToString("00") +
                                                report.SequenceNo.ToString("000");
                        bool isNew = CheckSequenceNo(newReportID, this.currentLatestProfileID);
                        if (!isNew)
                        {
                            continue;
                        }

                        reportList.Add(report);

                        //if (CheckSequenceNo(newReportID, newDetectedReportID))
                        //{
                        //    newDetectedReportID = newReportID;
                        //}
                    }
                }

                savedCount = reportList.Count;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[SWRServiceManager] ParsingSpecialNewsCode( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[SWRServiceManager] ParsingSpecialNewsCode( " + ex.ToString() + " )");
            }
            finally
            {
                //FileLogManager.GetInstance().WriteLog("[SWRServiceManager] ParsingSpecialNewsCode( end )");
            }

            return savedCount;
        }
        /// <summary>
        /// 기상특보통보문 데이터를 파싱.
        /// </summary>
        /// <param name="reportXml"></param>
        /// <returns></returns>
        private int ParsingWeatherWarningItem(string reportXml, out List<SWRProfile> reportList)
        {
            System.Diagnostics.Debug.Assert(reportXml != null);
            //FileLogManager.GetInstance().WriteLog("[SWRServiceManager] ParsingWeatherWarningItem( start )");

            reportList = null;
            int result = 0;

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(reportXml);

                XmlNodeList itemsList = xmlDoc.SelectNodes("/response/body/items");
                if (itemsList == null)
                {
                    FileLogManager.GetInstance().WriteLog("[SWRServiceManager] ParsingWeatherWarningItem( 아이템 정보를 찾을 수 없음. )");

                    result = -1;
                    return result;
                }

                reportList = new List<SWRProfile>();

                foreach (XmlNode items in itemsList)
                {
                    foreach (XmlNode item in items.ChildNodes)
                    {
                        if (item.Name != "item")
                        {
                            continue;
                        }

                        bool isError = false;
                        DateTime announceTime = new DateTime();
                        int sequenceNo = 0;
                        string targetAreas = string.Empty;

                        foreach (XmlNode node in item)
                        {
                            switch (node.Name)
                            {
                                case "t2":
                                    {
                                        targetAreas = node.InnerText;
                                    }
                                    break;
                                case "tmFc":
                                    {
                                        bool isParsed = DateTime.TryParse(node.InnerText, out announceTime);
                                        if (!isParsed)
                                        {
                                            FileLogManager.GetInstance().WriteLog("[SWRServiceManager] ParsingWeatherWarningItem( 변환 에러. tmFc=[" + node.InnerText + "] )");

                                            isError = true;
                                            break;
                                        }
                                    }
                                    break;
                                case "tmSeq":
                                    {
                                        int temp = 0;
                                        if (int.TryParse(node.InnerText, out temp))
                                        {
                                            sequenceNo = temp;
                                        }
                                        else
                                        {
                                            FileLogManager.GetInstance().WriteLog("[SWRServiceManager] ParsingWeatherWarningItem( 변환 에러. tmSeq=[" + node.InnerText + "] )");
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        if (isError || sequenceNo < 1)
                        {
                            continue;
                        }

                        bool isContains = CheckAreaRange(targetAreas);
                        if (!isContains)
                        {
                            continue;
                        }
                        string newReportID = announceTime.Year.ToString("0000") + announceTime.Month.ToString("00") + sequenceNo.ToString("000");
                        bool isNew = CheckSequenceNo(newReportID, this.currentLatestProfileID);
                        if (!isNew)
                        {
                            continue;
                        }
                        if (CheckSequenceNo(newReportID, this.newDetectedReportID))
                        {
                            this.newDetectedReportID = newReportID;
                        }

                        SWRProfile report = new SWRProfile();
                        report.ID = newReportID;
                        report.TargetAreas = string.Empty;
                        report.WarnKindCode = string.Empty;
                        report.WarnStressCode = string.Empty;
                        report.CommandCode = string.Empty;
                        report.OriginalWarningItemReport = "<swr><item>" + item.InnerXml + "</item></swr>";
                        report.ReceivedTime = DateTime.Now;
                        report.AssociationState = SWRAssociationStateCode.Waiting;

                        reportList.Add(report);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[SWRServiceManager] ParsingWeatherWarningItem( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[SWRServiceManager] ParsingWeatherWarningItem( " + ex.ToString() + " )");

                result = -99;
            }
            finally
            {
                //FileLogManager.GetInstance().WriteLog("[SWRServiceManager] ParsingWeatherWarningItem( end )");
            }

            return result;
        }
        /// <summary>
        /// 지정 지역이 현 프로그램의 범위에 해당하는 지역인지 검사.
        /// </summary>
        /// <param name="targetAreas"></param>
        /// <returns></returns>
        private bool CheckAreaRange(string targetAreas)
        {
            bool isContain = false;
            foreach (SWRAnnounceArea area in BasisData.SwrAreaInfo.Values)
            {
                if (targetAreas.Contains(area.AreaName))
                {
                    isContain = true;
                    break;
                }
            }

            return isContain;
        }
        /// <summary>
        /// 이미 검색 완료한 특보 번호인지 신규 번호인지 검사.
        /// </summary>
        /// <param name="newSeqNo"></param>
        /// <returns></returns>
        private bool CheckSequenceNo(string newSeqNo, string currentSeqNo)
        {
            int currentValue = 0;
            int newValue = 0;

            if (!int.TryParse(newSeqNo, out newValue))
            {
                return false;
            }
            if (!int.TryParse(this.currentLatestProfileID, out currentValue))
            {
                return false;
            }

            if (newValue <= currentValue)
            {
                return false;
            }

            return true;
        }

        #region 쓰레드 함수
        /// <summary>
        /// 수신 데이터 처리 (쓰레드 호출용 함수)
        /// </summary>
        private void SWRDataProcessing()
        {
            System.Console.WriteLine("[SWRManager] SWRDataProcessing ( 기상특보 감시 쓰레드 시작 )");
            FileLogManager.GetInstance().WriteLog("[SWRServiceManager] SWRDataProcessing( start )");

            try
            {
                while (this.isSWRDataProcessingContinue)
                {
                    for (uint cnt = 0; cnt < this.requestCycleBySec; cnt++)
                    {
                        Thread.Sleep(1000);
                        if (!this.isSWRDataProcessingContinue)
                        {
                            System.Console.WriteLine("[SWRManager] SWRDataProcessing ( 기상특보 감시 종료 요청 감지 )");
                            FileLogManager.GetInstance().WriteLog("[SWRServiceManager] SWRDataProcessing( 기상특보 감시 종료 요청 감지 )");

                            return;
                        }
                    }

                    this.newDetectedReportID = this.currentLatestProfileID;

                    // 기상특보통신문 데이터 체크
                    List<SWRProfile> reportList = new List<SWRProfile>();
                    bool reqResult = RequestWeatherWarningItem(ref reportList);
                    if (!reqResult)
                    {
                        System.Console.WriteLine("[SWRManager] SWRDataProcessing( 기상특보통신문 데이터 체크 실패 )");
                        FileLogManager.GetInstance().WriteLog("[SWRServiceManager] SWRDataProcessing( 기상특보통신문 데이터 체크 실패 )");
                        continue;
                    }
                    // 기상특보통신문 데이터 중 새로운 특보 검출
                    if (reportList == null || reportList.Count <= 0)
                    {
                        System.Console.WriteLine("[SWRManager] SWRDataProcessing( 신규 특보(통신문) 없음 )");
                        FileLogManager.GetInstance().WriteLog("[SWRServiceManager] SWRDataProcessing( 신규 특보(통신문) 없음 )");
                        continue;
                    }

                    // 기상특보코드 조회로 데이터 생성.
                    reqResult = RequestSpecialNewsCode(ref reportList);
                    if (reqResult)
                    {
                        if (this.NotifySWRReceived != null)
                        {
                            FileLogManager.GetInstance().WriteLog("[SWRServiceManager] SWRDataProcessing( 신규 특보(통신문) 통보 )");
                            this.NotifySWRReceived(this, new SWREventArgs(reportList));
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("[SWRManager] SWRDataProcessing( 신규 특보(코드조회) 없음 )");
                        FileLogManager.GetInstance().WriteLog("[SWRServiceManager] SWRDataProcessing( 신규 특보(코드조회) 없음 )");
                    }

                    this.currentLatestProfileID = this.newDetectedReportID;
                }
            }
            catch (ThreadAbortException exAbort)
            {
                System.Console.WriteLine("[SWRServiceManager] SWRDataProcessing( ABORT 검출)");
                FileLogManager.GetInstance().WriteLog("[SWRServiceManager] SWRDataProcessing( ABORT 검출)");

                Thread.ResetAbort();    // 상위로 다시 익셉션을 Throw 하지 않게 하기 위해서 콜.
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[SWRServiceManager] SWRDataProcessing( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[SWRServiceManager] SWRDataProcessing( Exception=[" + ex.ToString() + "]");
            }
            finally
            {
                // 쓰레드 종료 전에 해야할 일이 있다면 이 타이밍에 처리.

                System.Console.WriteLine("[SWRManager] SWRDataProcessing ( 기상특보 감시 쓰레드 종료 )");
                FileLogManager.GetInstance().WriteLog("[SWRServiceManager] SWRDataProcessing( 기상특보 감시 쓰레드 종료)");
            }
        }
        #endregion
    }
}

/// <summary>
/// 기상특보 데이터 기록용 이벤트 아규먼트
/// </summary>
public class SWREventArgs : EventArgs
{
    private List<SWRProfile> reportList;
    public List<SWRProfile> ReportList
    {
        get { return reportList; }
        set { reportList = value; }
    }

    public SWREventArgs(List<SWRProfile> input)
    {
        this.reportList = input;
    }
}
