using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;

namespace StdWarningInstallation.DataClass
{
    class SpecialWeatherReportInfo
    {
    }

    public class ReceivedSWRInfo
    {
        /// <summary>
        /// 리포트 아이디(년월+특보발표번호(3자리)).
        /// </summary>
        private string reportID;
        public string ReportID
        {
            get { return reportID; }
            set { reportID = value; }
        }
        /// <summary>
        /// 수신 시각.
        /// </summary>
        private DateTime receivedTime;
        public DateTime ReceivedTime
        {
            get { return receivedTime; }
            set { receivedTime = value; }
        }
        /// <summary>
        /// 기상특보통보문(Xml 데이터).
        /// </summary>
        private string originalReport;
        public string OriginalReport
        {
            get { return originalReport; }
            set { originalReport = value; }
        }
        /// <summary>
        /// 발령연계상태(0-미연계,1-연계완료(=발령),2-제외)
        /// </summary>
        private SWRAssociationStateCode associationState;
        public SWRAssociationStateCode AssociationState
        {
            get { return associationState; }
            set { associationState = value; }
        }
        /// <summary>
        /// 특보종류코드 (1-강풍,2-호우,3-한파,4-건조,5-해일,6-풍랑,7-태풍,8-대설,9-황사,12-폭염).
        /// </summary>
        private string kindCode;
        public string KindCode
        {
            get { return kindCode; }
            set { kindCode = value; }
        }
        /// <summary>
        /// 특보강도코드(0-주의보, 1-경보).
        /// </summary>
        private string stressCode;
        public string StressCode
        {
            get { return stressCode; }
            set { stressCode = value; }
        }
        /// <summary>
        /// 대상 영역 코드 리스트(구분자 콤마).
        /// </summary>
        private string targetAreas;
        public string TargetAreas
        {
            get { return targetAreas; }
            set { targetAreas = value; }
        }

        public void DeepCopyFrom(ReceivedSWRInfo src)
        {
            System.Diagnostics.Debug.Assert(src != null);

            this.reportID = src.reportID;
            this.receivedTime = src.receivedTime;
            this.targetAreas = src.targetAreas;
            this.originalReport = src.originalReport;
            this.associationState = src.associationState;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SWRProfile
    {
        #region Properties
        /// <summary>
        /// 기상특보 프로필 아이디( YYYYMM + SequenceNo(3) ).
        /// </summary>
        private string identifire;
        public string ID
        {
            get { return identifire; }
            set { identifire = value; }
        }
        /// <summary>
        /// 특보 발표 구역 목록(구분자 콤마).
        /// </summary>
        private string targetAreas;
        public string TargetAreas
        {
            get { return targetAreas; }
            set { targetAreas = value; }
        }
        /// <summary>
        /// 특보 종류 (1-강풍,2-호우,3-한파,4-건조,5-해일,6-풍랑,7-태풍,8-대설,9-황사,12-폭염)
        /// </summary>
        private string warnKindCode;
        public string WarnKindCode
        {
            get { return warnKindCode; }
            set { warnKindCode = value; }
        }
        /// <summary>
        /// 특보 강도 (0-주의보,1-경보).
        /// </summary>
        private string warnStressCode;
        public string WarnStressCode
        {
            get { return warnStressCode; }
            set { warnStressCode = value; }
        }
        /// <summary>
        /// 특보 발표 코드 (1-발표,2-해제,3-연장,4-대치에 의한 해제,5-대치에 의한 발표,6-정정)
        /// </summary>
        private string commandCode;
        public string CommandCode
        {
            get { return commandCode; }
            set { commandCode = value; }
        }
        /// <summary>
        /// 기상특보통보문 조회 데이터(Xml).
        /// </summary>
        private string originalWarningItemReport;
        public string OriginalWarningItemReport
        {
            get { return originalWarningItemReport; }
            set { originalWarningItemReport = value; }
        }
        /// <summary>
        /// 수신시각.
        /// </summary>
        private DateTime receivedTime;
        public DateTime ReceivedTime
        {
            get { return receivedTime; }
            set { receivedTime = value; }
        }
        /// <summary>
        /// 연계 상태 코드 (대기/발령완료/발령제외)
        /// </summary>
        private SWRAssociationStateCode associationState;
        public SWRAssociationStateCode AssociationState
        {
            get { return associationState; }
            set { associationState = value; }
        }
        #endregion

        public bool DeepCopyFrom(SWRProfile src)
        {
            System.Diagnostics.Debug.Assert(src != null);

            if (src == null)
            {
                return false;
            }

            this.identifire = src.identifire;
            this.targetAreas = src.targetAreas;
            this.warnKindCode = src.warnKindCode;
            this.warnStressCode = src.warnStressCode;
            this.commandCode = src.commandCode;
            this.originalWarningItemReport = src.originalWarningItemReport;
            
            this.receivedTime = src.receivedTime;
            this.associationState = src.associationState;

            return true;
        }
        /// <summary>
        /// Xml 데이터에서 기상특보통보문 데이터를 클래스로 파싱.
        /// </summary>
        /// <returns></returns>
        public SWRWarningItemProfile GetWarningItemProfile()
        {
            System.Diagnostics.Debug.Assert(this != null);
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(this.originalWarningItemReport));

            SWRWarningItemProfile profile = new SWRWarningItemProfile();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(this.originalWarningItemReport);

                XmlNodeList xNodes = xmlDoc.SelectNodes("/swr/item");
                if (xNodes == null || xNodes.Count <= 0)
                {
                    return null;
                }

                foreach (XmlNode item in xNodes)
                {
                    foreach (XmlNode node in item.ChildNodes)
                    {
                        string value = node.InnerText;
                        switch (node.Name)
                        {
                            case "stnId":
                                {
                                    profile.StationID = value;
                                }
                                break;
                            case "tmFc":
                                {
                                    DateTime temp = new DateTime();
                                    if (DateTime.TryParse(value, out temp))
                                    {
                                        profile.AnnounceTime = temp;
                                    }
                                }
                                break;
                            case "tmSeq":
                                {
                                    int temp = 0;
                                    if (int.TryParse(value, out temp))
                                    {
                                        profile.SequenceNo = temp;
                                    }
                                }
                                break;
                            case "warFc":
                                {
                                    profile.CommandCode = value;
                                }
                                break;
                            case "t1":
                                {
                                    profile.Title = value;
                                }
                                break;
                            case "t2":
                                {
                                    profile.TargetAreas = value;
                                }
                                break;
                            case "t3":
                                {
                                    profile.EffectStartInfo = value;
                                }
                                break;
                            case "t4":
                                {
                                    profile.Contents = value;
                                }
                                break;
                            case "t5":
                                {
                                    DateTime temp = new DateTime();
                                    if (DateTime.TryParse(value, out temp))
                                    {
                                        profile.PresentConditionTime = temp;
                                    }
                                }
                                break;
                            case "t6":
                                {
                                    profile.PresentConditionContents = value;
                                }
                                break;
                            case "t7":
                                {
                                    profile.PreliminaryConditionContents = value;
                                }
                                break;
                            case "other":
                                {
                                    profile.Other = value;
                                }
                                break;
                            default:
                                {
                                    // 이외에 빠진 항목은 없는지 확인하자
                                    System.Console.WriteLine("[SpecialWeatherReportInfo] GetWarningItemProfile( Unknown Element: " + node.Name + " )");
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[SpecialWeatherReportInfo] GetWarningItemProfile( Exception Occured: " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[SpecialWeatherReportInfo] GetWarningItemProfile ( Exception=[" + ex.ToString() + "] )");

                return null;
            }

            return profile;
        }
        public string[] GetTargetAreaList()
        {
            if (string.IsNullOrEmpty(this.targetAreas))
            {
                return null;
            }
            string[] seperator = { ",", };
            string[] targetAreaList = this.targetAreas.Split(seperator, StringSplitOptions.RemoveEmptyEntries);

            return targetAreaList;
        }
        /// <summary>
        /// 특보 해당구역 코드 리스트를 구역명 문자열(콤마로 구분된 연속된 문자열)로 변환.
        /// </summary>
        /// <returns></returns>
        public string GetTargetAreaNames()
        {
            string[] targetAreaCodes = GetTargetAreaList();
            if (targetAreaCodes == null)
            {
                return string.Empty;
            }

            List<string> regionNames = new List<string>();
            foreach (string areaCode in targetAreaCodes)
            {
                SWRAnnounceArea areaInfo = BasisData.FindSWRAreaByAreaCode(areaCode);
                if (areaInfo == null || areaInfo.TargetRegions == null)
                {
                    continue;
                }
                regionNames.Add(areaInfo.AreaName);
            }

            return string.Join(",", regionNames);
        }
        /// <summary>
        /// 특보 해당구역 코드 리스트에 해당하는 행정구역 지역코드 문자열(콤마로 구분된 연속된 문자열)을 반환.
        /// </summary>
        /// <returns></returns>
        public string[] GetTargetRegionCodes()
        {
            string[] targetAreaCodes = GetTargetAreaList();
            if (targetAreaCodes == null)
            {
                return null;
            }

            List<string> targetRegionCodes = new List<string>();
            foreach (string areaCode in targetAreaCodes)
            {
                SWRAnnounceArea areaInfo = BasisData.FindSWRAreaByAreaCode(areaCode);
                if (areaInfo == null || areaInfo.TargetRegions == null)
                {
                    continue;
                }
                foreach (string regionCode in areaInfo.TargetRegions)
                {
                    if (targetRegionCodes.Contains(regionCode))
                    {
                        continue;
                    }
                    targetRegionCodes.Add(regionCode);
                }
            }

            return targetRegionCodes.ToArray();
        }
        /// <summary>
        /// 특보 해당구역 코드 리스트를 행정구역 지역명 문자열(콤마로 구분된 연속된 문자열)로 변환.
        /// </summary>
        /// <returns></returns>
        public string GetTargetRegionNames()
        {
            string[] targetRegionCodes = GetTargetRegionCodes();
            if (targetRegionCodes == null)
            {
                return string.Empty;
            }

            List<string> regionNames = new List<string>();
            foreach (string regionCode in targetRegionCodes)
            {
                RegionProfile profile = BasisData.FindRegion(regionCode);
                if (profile == null)
                {
                    continue;
                }
                if (regionNames.Contains(profile.Name))
                {
                    continue;
                }
                regionNames.Add(profile.Name);
            }

            return string.Join(",", regionNames);
        }
    }

    /// <summary>
    /// 기상특보코드 조회 데이터 프로필.
    /// </summary>
    public class SWRNewsCodeProfile
    {
        /// <summary>
        /// 특보프로필 아이디(로컬).
        /// </summary>
        public string ReportID
        {
            get
            {
                string profileID = announceTime.Year.ToString("0000") + announceTime.Month.ToString("00") + sequenceNo.ToString("000");
                return profileID;
            }
        }
        /// <summary>
        /// [지점번호] stnId (기상 관측 지점 코드).
        /// 항목크기(5), 샘플데이터("108").
        /// </summary>
        private string meteorologicalStationCode;
        public string StationID
        {
            get { return meteorologicalStationCode; }
            set { meteorologicalStationCode = value; }
        }
        /// <summary>
        /// [발표시각(년월일시분)] tmFc (YYYY-MM-DD HH24:MI:SS.FF1)
        /// 항목크기(21), 샘플데이터("2006-01-02 11:00:00.0")
        /// </summary>
        private DateTime announceTime;
        public DateTime AnnounceTime
        {
            get { return announceTime; }
            set { announceTime = value; }
        }
        /// <summary>
        /// [발표번호(월별)] tmSeq.
        /// 항목크기(4), 샘플데이터("1").
        /// </summary>
        private int sequenceNo;
        public int SequenceNo
        {
            get { return sequenceNo; }
            set { sequenceNo = value; }
        }
        /// <summary>
        /// [특보구역코드] areaCode.
        /// 항목크기(10), 샘플데이터("L1070100").
        /// </summary>
        private string areaCode;
        public string AreaCode
        {
            get { return areaCode; }
            set { areaCode = value; }
        }
        /// <summary>
        /// [구역명] areaName.
        /// 항목크기(50), 샘플데이터("대구광역시").
        /// </summary>
        private string areaName;
        public string AreaName
        {
            get { return areaName; }
            set { areaName = value; }
        }
        /// <summary>
        /// [특보종류] warnVar (1-강풍,2-호우,3-한파,4-건조,5-해일,6-풍랑,7-태풍,8-대설,9-황사,12-폭염).
        /// 항목크기(2), 샘플데이터("4").
        /// </summary>
        private string kindCode;
        public string KindCode
        {
            get { return kindCode; }
            set { kindCode = value; }
        }
        /// <summary>
        /// [특보강도] warnStress  (0-주의보,1-경보).
        /// 항목크기(1), 샘플데이터("0").
        /// </summary>
        private string stressCode;
        public string StressCode
        {
            get { return stressCode; }
            set { stressCode = value; }
        }
        /// <summary>
        /// [특보발표코드] command (1-발표,2-해제,3-연장,4-대치에 의한 해제,5-대치에 의한 발표,6-정정).
        /// 항목크기(1), 샘플데이터("2").
        /// </summary>
        private string commandCode;
        public string CommandCode
        {
            get { return commandCode; }
            set { commandCode = value; }
        }
        /// <summary>
        /// [발효시각] startTime (YYYY-MM-DD HH24:MI:SS.FF1).
        /// 항목크기(21), 샘플데이터("없음").
        /// </summary>
        private DateTime effectStart;
        public DateTime EffectStart
        {
            get { return effectStart; }
            set { effectStart = value; }
        }
        /// <summary>
        /// [발효해제시각] endTime (YYYY-MM-DD HH24:MI:SS.FF1).
        /// 항목크기(21), 샘플데이터("2006-01-02 11:00:00.0").
        /// </summary>
        private DateTime effectEnd;
        public DateTime EffectEnd
        {
            get { return effectEnd; }
            set { effectEnd = value; }
        }
        /// <summary>
        /// [전체특보해제시각] allEndTime (YYYY-MM-DD HH24:MI:SS.FF1).
        /// 항목크기(21), 샘플데이터("없음").
        /// </summary>
        private DateTime allClearTime;
        public DateTime AllClearTime
        {
            get { return allClearTime; }
            set { allClearTime = value; }
        }
        /// <summary>
        /// [취소구분] cancle (0: 해당사항없음, 1:취소)
        /// 항목크기(1), 샘플데이터("0").
        /// </summary>
        private bool isCancel;
        public bool IsCancel
        {
            get { return isCancel; }
            set { isCancel = value; }
        }

        public bool DeepCopyFrom(SWRNewsCodeProfile src)
        {
            System.Diagnostics.Debug.Assert(src != null);

            if (src == null)
            {
                return false;
            }

            this.meteorologicalStationCode = src.meteorologicalStationCode;
            this.announceTime = src.announceTime;
            this.sequenceNo = src.sequenceNo;
            this.areaCode = src.areaCode;
            this.areaName = src.areaName;
            this.kindCode = src.kindCode;
            this.stressCode = src.stressCode;
            this.commandCode = src.commandCode;
            this.effectStart = src.effectStart;
            this.effectEnd = src.effectEnd;
            this.allClearTime = src.allClearTime;
            this.isCancel = src.isCancel;

            return true;
        }
    }
    /// <summary>
    /// 기상특보통보문 조회 데이터 프로필.
    /// </summary>
    public class SWRWarningItemProfile
    {
        /// <summary>
        /// 특보프로필 아이디(로컬).
        /// </summary>
        public string ReportID
        {
            get
            {
                string profileID = announceTime.Year.ToString("0000") + announceTime.Month.ToString("00") + sequenceNo.ToString("000");
                return profileID;
            }
        }
        /// <summary>
        /// [지점 코드] stnId(기상 관측 지점 코드).
        /// 항목크기(5), 샘플데이터("108").
        /// </summary>
        private string meteorologicalStationCode;
        public string StationID
        {
            get { return meteorologicalStationCode; }
            set { meteorologicalStationCode = value; }
        }
        /// <summary>
        /// [발표시각] tmFc. 
        /// 형식: (YYYY-MM-DD HH24:MI:SS.FF1).
        /// 항목크기(21), 샘플데이터("2012-09-26 16:00:00.0").
        /// </summary>
        private DateTime announceTime;
        public DateTime AnnounceTime
        {
            get { return announceTime; }
            set { announceTime = value; }
        }
        /// <summary>
        /// [발표번호(월별)] tmSeq.
        /// 항목크기(4), 샘플데이터("82").
        /// </summary>
        private int sequenceNo;
        public int SequenceNo
        {
            get { return sequenceNo; }
            set { sequenceNo = value; }
        }
        /// <summary>
        /// [특보발표코드] warFc.
        /// 항목크기(2), 샘플데이터("1")
        /// 특보코드조회시: (1-발표,2-해제,3-연장,4-대치에 의한 해제,5-대치에 의한 발표,6-정정).
        /// 특보통보문조회시: (1-발표,2-대치,3-해제,4-해제예보연장).
        /// </summary>
        private string commandCode;
        public string CommandCode
        {
            get { return commandCode; }
            set { commandCode = value; }
        }
        /// <summary>
        /// [제목] t1.
        /// 항목크기(500), 샘플데이터("1").
        /// </summary>
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        /// <summary>
        /// [해당구역] t2.
        /// 항목크기(2000), 샘플데이터("(1) 풍랑주의보 해제 : 제주도남쪽먼바다").
        /// 여러개인 경우, (1)....(2)... 식으로 나열됨.
        /// </summary>
        private string targetAreas;
        public string TargetAreas
        {
            get { return targetAreas; }
            set { targetAreas = value; }
        }
        /// <summary>
        /// [발효시각] t3.
        /// 항목크기(2000), 샘플데이터("(1) 풍랑주의보 해제 : 2012년 09월 26일 20시 00분").
        /// 형식: (YYYY-MM-DD HH24:MI:SS.FF1).
        /// </summary>
        private string effectStartInfo;
        public string EffectStartInfo
        {
            get { return effectStartInfo; }
            set { effectStartInfo = value; }
        }
        /// <summary>
        /// [내용] t4.
        /// 항목크기(2000), 샘플데이터("(1) 풍랑주의보 해제 o 위 구역의 풍랑주의보를 해제함.").
        /// </summary>
        private string contents;
        public string Contents
        {
            get { return contents; }
            set { contents = value; }
        }
        /// <summary>
        /// [특보발효현황시각] t5.
        /// 항목크기(21), 샘플데이터("2012-09-26 20:00:00.0").
        /// 형식: (YYYY-MM-DD HH24:MI:SS.FF1).
        /// </summary>
        private DateTime presentConditionTime;
        public DateTime PresentConditionTime
        {
            get { return presentConditionTime; }
            set { presentConditionTime = value; }
        }
        /// <summary>
        /// [특보발효현황내용] t6.
        /// 항목크기(4000), 샘플데이터("없음").
        /// </summary>
        private string presentConditionContents;
        public string PresentConditionContents
        {
            get { return presentConditionContents; }
            set { presentConditionContents = value; }
        }
        /// <summary>
        /// [예비특보발효현황] t7.
        /// 항목크기(2000), 샘플데이터("없음").
        /// </summary>
        private string preliminaryConditionContents;
        public string PreliminaryConditionContents
        {
            get { return preliminaryConditionContents; }
            set { preliminaryConditionContents = value; }
        }
        /// <summary>
        /// [참고사항] other.
        /// 항목크기(2000), 샘플데이터("없음").
        /// </summary>
        private string other;
        public string Other
        {
            get { return other; }
            set { other = value; }
        }

        public bool DeepCopyFrom(SWRWarningItemProfile src)
        {
            System.Diagnostics.Debug.Assert(src != null);

            if (src == null)
            {
                return false;
            }

            this.meteorologicalStationCode = src.meteorologicalStationCode;
            this.announceTime = src.announceTime;
            this.sequenceNo = src.sequenceNo;
            this.commandCode = src.commandCode;
            this.title = src.title;
            this.targetAreas = src.targetAreas;
            this.effectStartInfo = src.effectStartInfo;
            this.contents = src.contents;
            this.presentConditionTime = src.presentConditionTime;
            this.presentConditionContents = src.presentConditionContents;
            this.preliminaryConditionContents = src.preliminaryConditionContents;
            this.other = src.other;

            return true;
        }
    }

    /// <summary>
    /// 특보 구역 코드.
    ///  예) L1010800, 인천광역시
    /// </summary>
    public class SWRAnnounceArea
    {
        private string areaCode;
        public string AreaCode
        {
            get { return areaCode; }
            set { areaCode = value; }
        }
        private string areaName;
        public string AreaName
        {
            get { return areaName; }
            set { areaName = value; }
        }
        private List<string> targetRegions;
        public List<string> TargetRegions
        {
            get { return targetRegions; }
            set { targetRegions = value; }
        }

        public void DeepCopyFrom(SWRAnnounceArea src)
        {
            System.Diagnostics.Debug.Assert(src != null);

            this.areaCode = src.areaCode;
            this.areaName = src.areaName;

            if (src.targetRegions == null)
            {
                this.targetRegions = null;
                return;
            }

            if (this.targetRegions == null)
            {
                this.targetRegions = new List<string>();
            }
            this.targetRegions.Clear();

            foreach (string regionCode in src.targetRegions)
            {
                string copy = regionCode;
                this.targetRegions.Add(copy);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class SWRAnnounceAreaInfo
    {
        private string areaCode;
        public string AreaCode
        {
            get { return areaCode; }
            set { areaCode = value; }
        }
        private string areaName;
        public string AreaName
        {
            get { return areaName; }
            set { areaName = value; }
        }
        private List<string> matchedRegions;
        public List<string> MatchedRegions
        {
            get { return matchedRegions; }
            set { matchedRegions = value; }
        }
    }

    /// <summary>
    /// 특보 발표 코드.
    /// (1-발표,2-해제,3-연장,4-대치에 의한 해제,5-대치에 의한 발표,6-정정)
    /// </summary>
    public class SWRCommand
    {
        private string code;
        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        private string cmdStr;
        public string Description
        {
            get { return cmdStr; }
            set { cmdStr = value; }
        }
    }

    /// <summary>
    /// 특보 강도.
    ///   (0-주의보,1-경보)
    /// </summary>
    public class SWRStress
    {
        private string code;
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SWRKind
    {
        private string code;
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }

    public class SWRAssociationState
    {
        private string code;
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
    }

    public class SWRAssociationCondition
    {
        /// <summary>
        /// [특보 종류 코드]
        /// </summary>
        private int warnKindCode;
        public int WarnKindCode
        {
            get { return warnKindCode; }
            set { warnKindCode = value; }
        }

        /// <summary>
        /// [특보 강도 코드]
        /// </summary>
        private int warnStressCode;
        public int WarnStressCode
        {
            get { return warnStressCode; }
            set { warnStressCode = value; }
        }

        private bool isUse;
        public bool IsUse
        {
            get { return isUse; }
            set { isUse = value; }
        }

        public void DeepCopyFrom(SWRAssociationCondition src)
        {
            System.Diagnostics.Debug.Assert(src != null);

            this.warnKindCode = src.warnKindCode;
            this.warnStressCode = src.warnStressCode;
            this.isUse = src.isUse;
        }
    }

    public class SWRAssociationConditionInfo
    {
        /// <summary>
        /// [연계 유무]
        /// </summary>
        private bool useAssociation;
        public bool UseAssociation
        {
            get { return useAssociation; }
            set { useAssociation = value; }
        }

        /// <summary>
        /// [강풍] 주의보/경보
        /// </summary>
        private StressCondition highWind;
        public StressCondition HighWind
        {
            get { return highWind; }
            set { highWind = value; }
        }

        /// <summary>
        /// [호우] 주의보/경보
        /// </summary>
        private StressCondition heavyRain;
        public StressCondition HeavyRain
        {
            get { return heavyRain; }
            set { heavyRain = value; }
        }

        /// <summary>
        /// [태풍] 주의보/경보
        /// </summary>
        private StressCondition hurricane;
        public StressCondition Hurricane
        {
            get { return hurricane; }
            set { hurricane = value; }
        }

        /// <summary>
        /// [한파] 주의보/경보
        /// </summary>
        private StressCondition coldWave;
        public StressCondition ColdWave
        {
            get { return coldWave; }
            set { coldWave = value; }
        }

        /// <summary>
        /// [건조] 주의보/경보
        /// </summary>
        private StressCondition heavyArid;
        public StressCondition HeavyArid
        {
            get { return heavyArid; }
            set { heavyArid = value; }
        }

        /// <summary>
        /// [대설] 주의보/경보
        /// </summary>
        private StressCondition heavySnow;
        public StressCondition HeavySnow
        {
            get { return heavySnow; }
            set { heavySnow = value; }
        }

        /// <summary>
        /// [황사] 주의보/경보
        /// </summary>
        private StressCondition yellowSand;
        public StressCondition YellowSand
        {
            get { return yellowSand; }
            set { yellowSand = value; }
        }

        /// <summary>
        /// [풍랑] 주의보/경보
        /// </summary>
        private StressCondition windAndWaves;
        public StressCondition WindAndWaves
        {
            get { return windAndWaves; }
            set { windAndWaves = value; }
        }

        /// <summary>
        /// [해일] 주의보/경보
        /// </summary>
        private StressCondition stormSurge;
        public StressCondition StormSurge
        {
            get { return stormSurge; }
            set { stormSurge = value; }
        }

        /// <summary>
        /// [폭염] 주의보/경보
        /// </summary>
        private StressCondition heatWaveSpecial;
        public StressCondition HeatWaveSpecial
        {
            get { return heatWaveSpecial; }
            set { heatWaveSpecial = value; }
        }

        public void DeepCopyFrom(SWRAssociationConditionInfo src)
        {
            System.Diagnostics.Debug.Assert(src != null);

            src.highWind.Watch = src.highWind.Watch;
            src.highWind.Warning = src.highWind.Warning;

            src.heavyRain.Watch = src.heavyRain.Watch;
            src.heavyRain.Warning = src.heavyRain.Warning;

            src.hurricane.Watch = src.hurricane.Watch;
            src.hurricane.Warning = src.hurricane.Warning;

            src.coldWave.Watch = src.coldWave.Watch;
            src.coldWave.Warning = src.coldWave.Warning;

            src.heavyArid.Watch = src.heavyArid.Watch;
            src.heavyArid.Warning = src.heavyArid.Warning;

            src.heavySnow.Watch = src.heavySnow.Watch;
            src.heavySnow.Warning = src.heavySnow.Warning;

            src.yellowSand.Watch = src.yellowSand.Watch;
            src.yellowSand.Warning = src.yellowSand.Warning;

            src.windAndWaves.Watch = src.windAndWaves.Watch;
            src.windAndWaves.Warning = src.windAndWaves.Warning;

            src.stormSurge.Watch = src.stormSurge.Watch;
            src.stormSurge.Warning = src.stormSurge.Warning;

            src.heatWaveSpecial.Watch = src.heatWaveSpecial.Watch;
            src.heatWaveSpecial.Warning = src.heatWaveSpecial.Warning;
        }
    }

    public class StressCondition
    {
        /// <summary>
        /// 주의보 사용 유무
        /// </summary>
        private bool watch;
        public bool Watch
        {
            get { return watch; }
            set { watch = value; }
        }
        /// <summary>
        /// 경보 사용 유무
        /// </summary>
        private bool warning;
        public bool Warning
        {
            get { return warning; }
            set { warning = value; }
        }
    }
    /// <summary>
    /// 기상특보 종류+강도에 해당하는 재난종류코드 정보를 담고 있다.
    /// </summary>
    public class SWRDisasterMatching
    {
        private string identifier;
        public string ID
        {
            get { return identifier; }
            set { identifier = value; }
        }
        private string swrKindCode;
        public string SwrKindCode
        {
            get { return swrKindCode; }
            set { swrKindCode = value; }
        }
        private string swrStressCode;
        public string SwrStressCode
        {
            get { return swrStressCode; }
            set { swrStressCode = value; }
        }
        private string disasterKindCode;
        public string DisasterKindCode
        {
            get { return disasterKindCode; }
            set { disasterKindCode = value; }
        }
    }

    /// <summary>
    /// 프로필 갱신 요청 이벤트 아규먼트 클래스
    /// </summary>
    public class SWRProfileUpdateEventArgs : EventArgs
    {
        private ProfileUpdateReqData updateData;
        public ProfileUpdateReqData UpdateData
        {
            get { return updateData; }
            set { updateData = value; }
        }

        public SWRProfileUpdateEventArgs(ProfileUpdateReqData data)
        {
            this.updateData = new ProfileUpdateReqData(data.ReqEventID, data.TotalCnt, data.Mode);
            this.updateData.DeepCopyFrom(data);
        }
    }

}
