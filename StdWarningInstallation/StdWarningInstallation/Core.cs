using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

using CAPLib;
using StdWarningInstallation.DataClass;

namespace StdWarningInstallation
{
    public class Core
    {
        private ManualResetEvent manualEvtCAP = null;
        private ManualResetEvent manualEvtSYNC = null;
        private ManualResetEvent manualEvtSWR = null;
        private Thread processCAPThread = null;
        private Thread processDataSyncThread = null;
        private Thread processSWRThread = null;

        private Queue<ReceivedCAPInfo> dataCAPQueue = new Queue<ReceivedCAPInfo>();
        private Queue<SynchronizationReqData> dataSyncInfoQueue = new Queue<SynchronizationReqData>();
        private ProfileHashCheckReqData dataSyncSingleHashReqData= null;
        private ProfileUpdateReqData dataSyncProfileUpdateReqData = null;
        private Queue<SWRProfile> dataSWRQueue = new Queue<SWRProfile>();

        private bool isCAPDataProcessingContinue = false;
        private bool isDataSyncProcessingContinue = false;
        private bool isSWRDataProcessingContinue = false;

        public event EventHandler<IAGWConnectionEventArgs> NotifyIAGWConnectionState;
        public event EventHandler<OrderEventArgs> NotifyLatestOrderInfoChanged;             // 타지역발령 연계, 자체발령 시 사용
        public event EventHandler<OrderResponseEventArgs> NotifyOrderResponseUpdated;       // 발령 응답 갱신 통지
        public event EventHandler<SASProfileUpdateEventArgs> NotifySASProfileUpdated;       // 타지역발령 연계, 자체발령 시 사용
        public event EventHandler<EventArgs> NotifySWRProfileUpdated;                       // 기상특보연계

        private bool isConnectionStarted = false;
        private bool useAutoReconnectionWithIAGW = false;


        /// <summary>
        /// 생성자
        /// </summary>
        public Core()
        {
            FileLogManager.GetInstance().WriteLog("[Core] Start");

            ConfigData configInfo = ConfigManager.GetInstance().ConfigInfo;
            this.useAutoReconnectionWithIAGW = configInfo.IAGW.UseAutoConnection;

            DBManager.GetInstance().SetConnectionInfo(configInfo.DB);
            CommunicationManager.GetInstance().SetConnectionInfo(configInfo.IAGW);
            SWRServiceManager.GetInstance().SetConnectionInfo(configInfo.SWR);

            CommunicationManager.GetInstance().NotifyIAGWConnectionState += this.communicationManager_OnNotifyIAGWConnectionState;
            CommunicationManager.GetInstance().NotifyCAPReceived += this.communicationManager_OnCAPReceived;
            CommunicationManager.GetInstance().NotifyDataSyncRequested += this.communicationManager_OnNotifyDataSyncRequested;

            SWRServiceManager.GetInstance().NotifySWRReceived += this.swrServicemanager_OnNotifySWRReceived;

            Init();
        }

        ~Core()
        {
            UnInit();
 
            CommunicationManager.GetInstance().NotifyIAGWConnectionState -= this.communicationManager_OnNotifyIAGWConnectionState;
            CommunicationManager.GetInstance().NotifyCAPReceived -= this.communicationManager_OnCAPReceived;
            CommunicationManager.GetInstance().NotifyDataSyncRequested -= this.communicationManager_OnNotifyDataSyncRequested;

            SWRServiceManager.GetInstance().NotifySWRReceived -= this.swrServicemanager_OnNotifySWRReceived;

            FileLogManager.GetInstance().WriteLog("[Core] End");
        }

        /// <summary>
        /// 초기화
        /// </summary>
        public void Init()
        {
            FileLogManager.GetInstance().WriteLog("[Core] Init");

            bool chkRet = DBManager.GetInstance().CheckOpenDB();
            if (chkRet)
            {
                LoadBasisData();
            }
            else
            {
            }

            StartCAPDataProcessing();
            StartDataSynchronizationProcessing();

            CommunicationManager.GetInstance().PrepareConnectionSupervisory();
        }
        public void UnInit()
        {
            FileLogManager.GetInstance().WriteLog("[Core] UnInit");

            EndCAPDataProcessing();
            EndDataSynchronizationProcessing();
            EndSWRDataProcessing();
        }

        private void LoadBasisData()
        {
            ConfigData configInfo = ConfigManager.GetInstance().ConfigInfo;

            BasisData.SystemSenderInfo = new ConfigSenderInfo();
            if (configInfo != null && configInfo.LOCAL != null)
            {
                BasisData.SystemSenderInfo.ID = configInfo.LOCAL.SenderID;
                BasisData.SystemSenderInfo.Name = configInfo.LOCAL.SenderName;
            }

            BasisData.Disasters = DBManager.GetInstance().QueryDefinitionOfDisaster();

            if (configInfo != null && configInfo.LOCAL != null)
            {
                BasisData.Regions = DBManager.GetInstance().QueryRegionProfileInfo(configInfo.LOCAL.RegionCode);
            }
            if (BasisData.IsRegionDataLoaded())
            {
                RegionProfile profile = BasisData.Regions.LstRegion.Values.ElementAt(0);
                BasisData.TopRegion = new RegionDefinition(profile.Code, profile.Name);
            }

            BasisData.SASKindInfo = DBManager.GetInstance().QueryDefinitionOfSASKind();

            BasisData.MsgTextLanguageKind = DBManager.GetInstance().QueryDefinitionOfMsgTextDisplayLanguageKind();
            BasisData.MsgTextDisplayMediaType = DBManager.GetInstance().QueryDefinitionOfMsgTextDisplayMediaType();
            BasisData.MsgTextCityType = DBManager.GetInstance().QueryDefinitionOfMsgTextCityType();
            BasisData.BasicMsgTextInfo = DBManager.GetInstance().QueryBasicMsgTextInfo();
            BasisData.TransmitMsgTextInfo = DBManager.GetInstance().QueryTransmitMsgTextInfo(null);

            BasisData.SwrKindInfo = DBManager.GetInstance().QueryDefinitionOfSWRKind();
            BasisData.SwrStressInfo = DBManager.GetInstance().QueryDefinitionOfSWRStress();
            BasisData.SwrCommandInfo = DBManager.GetInstance().QueryDefinitionOfSWRCommand();
            BasisData.SwrAreaInfo = DBManager.GetInstance().QueryDefinitionOfSWRAnnounceArea();
            BasisData.SwrDisasterMatchingInfo = DBManager.GetInstance().QueryDefinitionOfSWRDisaster();
        }

        /// <summary>
        /// 프로그램 기동 처리
        /// </summary>
        public int StartUp()
        {
            FileLogManager.GetInstance().WriteLog("[Core] StartUp");

            int result = 0;

            bool dbResult = DBManager.GetInstance().CheckOpenDB();
            if (!dbResult)
            {
                result = - 1;
            }

            bool connResult = ConnectToGateway();
            if (!connResult)
            {
                result = - 2;
            }

            ConfigData configInfo = ConfigManager.GetInstance().ConfigInfo;
            if (configInfo.SWR.UseService)
            {
                string reportSeq = string.Empty;
                if (dbResult)
                {
                    reportSeq = QueryLatestSWRProfileID();
                }
                StartSWRDataProcessing(reportSeq);
            }

            CommunicationManager.GetInstance().StartConnectionSupervisory();

            return result;
        }

        #region 게이트웨이 접속 관련
        private bool ConnectToGateway()
        {
            System.Console.WriteLine("[Core] 게이트웨이에 접속 요청");

            isConnectionStarted = true;
            return CommunicationManager.GetInstance().ConnectToGateway();
        }
        public bool ConnectToGateway(string ip, string port)
        {
            CommunicationManager.GetInstance().SetConnectionInfo(ip, port);
            bool isSuccess = ConnectToGateway();
            if (isSuccess)
            {
                CommunicationManager.GetInstance().StartConnectionSupervisory();
            }

            return isSuccess;
        }
        public bool DisconnectFromGateway()
        {
            System.Console.WriteLine("[Core] 게이트웨이와 접속 해제");

            CommunicationManager.GetInstance().EndConnectionSupervisory();
            return CommunicationManager.GetInstance().DisconnectWithGateway();
        }
        #endregion

        #region 데이터베이스 접속 관련
        public bool TestOpenDB(ConfigDBData dbInfo)
        {
            return DBManager.GetInstance().TestOpenDB(dbInfo);
        }
        #endregion

        #region Config(설정파일)
        public ConfigData GetConfigInfo()
        {
            ConfigData data = ConfigManager.GetInstance().ConfigInfo;
            return data;
        }
        public void SetConfigInfo(ConfigData info)
        {
            // 변경 전 정보
            ConfigData oldSetting = ConfigManager.GetInstance().ConfigInfo;

            // 변경
            ConfigManager.GetInstance().SaveConfigInfo(info);

            // 새로 변경된 설정 정보를 현 시스템에 적용
            // 지역코드 및 데이터베이스 접속 정보가 변경된 경우는, 시스템 전반에 걸쳐 영향을 끼치므로, 재부팅이 필요.
            ConfigData newSetting = ConfigManager.GetInstance().ConfigInfo;

            // 1.통합경보게이트웨이 접속 정보
            this.useAutoReconnectionWithIAGW = newSetting.IAGW.UseAutoConnection;
            CommunicationManager.GetInstance().SetConnectionInfo(newSetting.IAGW);
            // 2.데이터베이스 접속 정보
            DBManager.GetInstance().SetConnectionInfo(newSetting.DB);
            // 3.발령대 정보(발령대 아이디 및 발령자 명칭)
            BasisData.SystemSenderInfo = new ConfigSenderInfo(newSetting.LOCAL.SenderID, newSetting.LOCAL.SenderName);
            // 4.기상특보
            if (oldSetting.SWR.UseService != newSetting.SWR.UseService)
            {
                UseSWRAssociation(newSetting.SWR.UseService);
            }
        }
        /// <summary>
        /// 기상특보연계 발령 기능 사용 유무 설정.
        /// </summary>
        /// <param name="isUse"></param>
        public void UseSWRAssociation(bool isUse)
        {
            if (isUse)
            {
                string reportSeq = QueryLatestSWRProfileID();
                StartSWRDataProcessing(reportSeq);
            }
            else
            {
                EndSWRDataProcessing();
            }
        }
        #endregion

        #region 사용자 정보 제공
        public List<UserAccount> GetUserAccountInfo()
        {
            return DBManager.GetInstance().QueryUserAccountInfo();
        }
        public int AddUserAccountInfo(UserAccount accountInfo)
        {
            return DBManager.GetInstance().RegistUserAccountInfo(accountInfo);
        }
        public int UpdateUserAccountInfo(UserAccount accountInfo)
        {
            return DBManager.GetInstance().UpdateUserAccountInfo(accountInfo);
        }
        public int DeleteUserAccount(string accountID)
        {
            int ret = 0;

            return ret;
        }
        #endregion

        #region 표준경보시스템 정보
        public int GetSASInfo(out Dictionary<string, SASInfo> dicSystemInfo)
        {
            dicSystemInfo = null;

            List<SASProfile> lst = DBManager.GetInstance().QuerySASInfo();
            if (lst == null)
            {
                return -1;
            }

            dicSystemInfo = new Dictionary<string, SASInfo>();
            foreach (SASProfile profile in lst)
            {
                SASInfo systemInfo = new SASInfo();
                systemInfo.Profile = new SASProfile();
                systemInfo.Profile.DeepCopyFrom(profile);

                dicSystemInfo.Add(profile.ID, systemInfo);
            }

            return 0;
        }
        #endregion

        #region 경보문안 기본언어
        public int UpdateLanguageSetting(Dictionary<int, MsgTextDisplayLanguageKind> settingList)
        {
            int result = DBManager.GetInstance().UpdateLanguageSetting(settingList);
            if (result == 0)
            {
                BasisData.MsgTextLanguageKind.Clear();
                BasisData.MsgTextLanguageKind = DBManager.GetInstance().QueryDefinitionOfMsgTextDisplayLanguageKind();
            }

            return result;
        }
        #endregion

        #region 발령 관련 기능
        /// <summary>
        /// 발령 정보를 CAP 메시지로 변환하여 발령을 전송하고, 이력을 기록한다.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int SendOrder(OrderProvisionInfo info)
        {
            FileLogManager.GetInstance().WriteLog("[Core] SendOrder");

            int result = 0;

            if (info == null)
            {
                System.Console.WriteLine("[Core] SendOrder( 입력 파라미터 에러 )");
                FileLogManager.GetInstance().WriteLog("[Core] SendOrder( 입력 파라미터 에러 )");

                return -1;
            }

            // info 를 CAP으로 변환
            info.CAPData = MakeCAP(info);
            if (info.CAPData == null)
            {
                EventLogManager.GetInstance().WriteLog(EventLogEntryType.Error, "경보 발령( 데이터 오류 )");
                FileLogManager.GetInstance().WriteLog("[Core] SendOrder( CAP 생성 에러 )");

                return -2;
            }

            OrderRecord recordInfo = info.ToOrderRecord();
            if (recordInfo == null)
            {
                System.Console.WriteLine("[Core] SendOrder( 데이터 변환 실패 )");
                FileLogManager.GetInstance().WriteLog("[Core] SendOrder( 데이터 변환 실패 )");

                return -3;
            }

            // 먼저 등록
            int dbResult = RegistOrder(recordInfo);
            if (dbResult != 0)
            {
                EventLogManager.GetInstance().WriteLog(EventLogEntryType.Error, "경보 발령( 등록 실패 )");
                System.Console.WriteLine("[Core] SendOrder( 발령 등록 실패 )");
                FileLogManager.GetInstance().WriteLog("[Core] SendOrder( 발령 정보 등록 실패. dbResult=[" + dbResult + "] )");
            }

            // 송신
            int sendResult = CommunicationManager.GetInstance().SendCAP(recordInfo.CapText);
            if (sendResult == 0)
            {
                EventLogManager.GetInstance().WriteLog(EventLogEntryType.SuccessAudit, "경보 발령( 전송 성공 )");
                FileLogManager.GetInstance().WriteLog("[Core] SendOrder( 경보 발령 전송 성공 )");

                result = dbResult;
            }
            else
            {
                EventLogManager.GetInstance().WriteLog(EventLogEntryType.Error, "경보 발령( 전송 실패 =[" + sendResult.ToString() + " )");
                System.Console.WriteLine("[Core] SendOrder( 발령 실패 )");
                FileLogManager.GetInstance().WriteLog("[Core] SendOrder( 발령 전송 실패. sendResult=[" + sendResult + " )");

                result = -20;
            }

            return result;
        }
        /// <summary>
        /// 발령 취소.
        /// </summary>
        /// <param name="orderCAPID">원발령 캡 아이디</param>
        /// <param name="cancelTarget">취소 정보</param>
        /// <returns></returns>
        public int CancelOrder(string orderCAPID, OrderRecord cancelTarget)
        {
            System.Diagnostics.Debug.Assert(orderCAPID != null);

            EventLogManager.GetInstance().WriteLog(EventLogEntryType.Information, "발령 취소");

            if (null == cancelTarget)
            {
                cancelTarget = DBManager.GetInstance().QueryOrderInfo(orderCAPID);
                if (cancelTarget == null)
                {
                    FileLogManager.GetInstance().WriteLog("[Core] CancelOrder( 입력 파라미터 오류(2) )");
                    return -1;
                }
            }

            int result = 0;

            // 발령 전송
            CAPLib.CAP capMsg = new CAP(cancelTarget.CapText);

            string refSender = capMsg.SenderID;
            string refID = capMsg.MessageID;
            string refSent = capMsg.SentDateTime.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz");

            CAPHelper helper = new CAPHelper();
            capMsg.MessageID = helper.MakeIdentifier(BasisData.SystemSenderInfo.Name);
            capMsg.SentDateTime = DateTime.Now;
            capMsg.MessageType = MsgType.Cancel;
            capMsg.ReferenceIDs = refSender + "," + refID + "," + refSent;


            // 발령 기록 등록
            OrderRecord newRecord = new OrderRecord();
            newRecord.DeepCopyFrom(cancelTarget);
            newRecord.CAPID = capMsg.MessageID;
            newRecord.OrderedTime = capMsg.SentDateTime;
            newRecord.RefType = OrderReferenceType.Cancel;
            newRecord.RefRecordID = cancelTarget.CAPID;
            newRecord.LocationKind = OrderLocationKind.Local;
            newRecord.ClearState = new AlertingClearState();
            newRecord.ClearState.DeepCopyFrom(BasisData.AlertingClearStateInfo[ClearAlertState.Exclude]);

            int dbResult = RegistOrder(newRecord);
            if (dbResult != 0)
            {
                System.Console.WriteLine("[Core] CancelOrder( 발령 등록 실패 )");
                FileLogManager.GetInstance().WriteLog("[Core] CancelOrder( 발령 등록 실패. dbResult=[ " + dbResult + "] )");
                //return result;
            }

            int sendResult = CommunicationManager.GetInstance().SendCAP(capMsg.WriteToXML());
            if (sendResult != 0)
            {
                EventLogManager.GetInstance().WriteLog(EventLogEntryType.Error, "발령 취소( 발령 전송 실패 )");
                System.Console.WriteLine("[Core] CancelOrder( 발령 실패 )");
                FileLogManager.GetInstance().WriteLog("[Core] CancelOrder( 발령 전송 실패. dbResult=[ " + sendResult + "] )");
            }

            if (sendResult == 0)
            {
                result = dbResult;
            }
            else
            {
                result = 11;
            }

            return result;
        }

        /// <summary>
        /// 발령 레코드의 경보 해제 상태 정보를 갱신한다.
        /// 해제상태 (0: 해제 대기, 1: 해제 완료, 2: 제외)
        /// 상태(2)의 해제란, 명시적인 해제가 불필요하다고 판단되는 항목에 대해 사용자가 항목 제외를 설정한 상태를 의미한다. 
        /// </summary>
        /// <param name="targetCAPID"></param>
        /// <param name="clearState"></param>
        /// <returns></returns>
        public int UpdateClearAlertingState(string targetCAPID, ClearAlertState clearState)
        {
            System.Diagnostics.Debug.Assert(targetCAPID != null);

            int result = DBManager.GetInstance().UpdateAlertingClearStateOfOrderRecord(targetCAPID, clearState);
            if (result != 0)
            {
                System.Console.WriteLine("[Core] UpdateClearAlertingState( 경보 해제 상태 정보 업데이트 실패 =["+result+"] )");
            }

            return result;
        }

        /// <summary>
        /// 입력 정보를 바탕으로 CAP 메시지 작성
        /// </summary>
        /// <param name="provisionInfo"></param>
        /// <returns></returns>
        public CAP MakeCAP(OrderProvisionInfo provisionInfo)
        {
            CAP capMsg = new CAP();
            CAPHelper helper = new CAPHelper();

            // MSG ID 생성
            capMsg.MessageID = helper.MakeIdentifier(BasisData.SystemSenderInfo.Name);
            if (string.IsNullOrEmpty(capMsg.MessageID))
            {
                System.Console.WriteLine("[Core] MakeCAP( CAP 아이디 생성 오류 )");
                FileLogManager.GetInstance().WriteLog("[Core] MakeCAP( 발령 정보 설정 중 오류가 발생하였습니다. 메시지 아이디 생성에 실패하였습니다. )");

                throw new Exception("발령 정보 설정 중 오류가 발생하였습니다. 메시지 아이디 생성에 실패하였습니다.");
            }

            capMsg.SenderID = BasisData.SystemSenderInfo.ID + "_통제소장";
            capMsg.SentDateTime = DateTime.Now;
            capMsg.MessageStatus = provisionInfo.Mode.Code;
            capMsg.MessageType = provisionInfo.MessageType;
            capMsg.Source = BasisData.SystemSenderInfo.Name + "_통제소장";
            capMsg.Scope = provisionInfo.Scope;
            capMsg.HandlingCode.Add("대한민국정부1.1");

            if (provisionInfo.Scope == ScopeType.Private)
            {
                capMsg.Addresses = helper.MakeAddress(provisionInfo.TargetSystems);
                if (string.IsNullOrEmpty(capMsg.Addresses))
                {
                    System.Console.WriteLine("[Core] MakeCAP( ScopeType.Private - 발령대상시스템 정보 오류)");
                    FileLogManager.GetInstance().WriteLog("[Core] MakeCAP( 발령 정보 설정 중 오류가 발생하였습니다. 발령 대상 시스템 정보가 올바르지 않습니다. )");

                    throw new Exception("발령 정보 설정 중 오류가 발생하였습니다. 발령 대상 시스템 정보가 올바르지 않습니다.");
                }
            }
            if (provisionInfo.Scope == ScopeType.Restricted)
            {
                capMsg.Restriction = helper.MakeRestriction(provisionInfo.TargetSystemsKinds);
                if (string.IsNullOrEmpty(capMsg.Restriction))
                {
                    System.Console.WriteLine("[Core] MakeCAP( ScopeType.Restricted - 발령대상시스템 종류 정보 오류)");
                    FileLogManager.GetInstance().WriteLog("[Core] MakeCAP( 발령 정보 설정 중 오류가 발생하였습니다. 발령 대상 시스템 종류 정보가 올바르지 않습니다. )");

                    throw new Exception("발령 정보 설정 중 오류가 발생하였습니다. 발령 대상 시스템 종류 정보가 올바르지 않습니다.");
                }
            }

            if (provisionInfo.MsgTextInfo == null || provisionInfo.MsgTextInfo.SelectedLanguages == null)
            {
                System.Console.WriteLine("[Core] MakeCAP( 발령 정보 문안 - 언어 설정 오류 )");
                throw new Exception("발령 정보 설정 중 오류가 발생하였습니다. 언어 정보가 올바르지 않습니다.");
            }

            if (provisionInfo.Disaster == null || 
                provisionInfo.Disaster.Category == null || provisionInfo.Disaster.Category.Code == null ||
                provisionInfo.Disaster.Kind == null || provisionInfo.Disaster.Kind.Code == null || provisionInfo.Disaster.Kind.Name == null)
            {
                System.Console.WriteLine("[Core] MakeCAP( 재난 종류 정보 오류 )");
                FileLogManager.GetInstance().WriteLog("[Core] MakeCAP( 발령 정보 설정 중 오류가 발생하였습니다. 재난 종류 정보가 올바르지 않습니다. )");

                throw new Exception("발령 정보 설정 중 오류가 발생하였습니다. 재난 종류 정보가 올바르지 않습니다.");
            }

            foreach (MsgTextDisplayLanguageKind languageKind in provisionInfo.MsgTextInfo.SelectedLanguages)
            {
                InfoType info = new InfoType();
                capMsg.Info.Add(info);

                info.Language = languageKind.LanguageCode;

                if (provisionInfo.Disaster.Category.Code != null)
                {
                    CategoryType result = CategoryType.Other;
                    if (Enum.TryParse<CategoryType>(provisionInfo.Disaster.Category.Code, out result))
                    {
                        info.Category.Add(result);
                    }
                }

                info.Event = provisionInfo.Disaster.Kind.Name;

                info.Urgency = UrgencyType.Unknown;
                info.Severity = SeverityType.Unknown;
                info.Certainty = CertaintyType.Unknown;


                NameValueType nameValueType = new NameValueType();
                nameValueType.Name = "KRDSTCode";
                nameValueType.Value = provisionInfo.Disaster.Kind.Code;

                info.EventCode.Add(nameValueType);
                info.SenderName = "";
                ConfigData config = GetConfigInfo();
                if (config != null && config.LOCAL != null)
                {
                    info.SenderName = config.LOCAL.SenderName;
                }

                // 헤드라인 정보에는 발령 대상 정보도 포함되어야 하므로, 하위에서 처리.

                info.Contact = BasisData.CurrentLoginUser.Departure + ", " + BasisData.CurrentLoginUser.Name + ", " + BasisData.CurrentLoginUser.Telephone;


                MsgTextCityType selectedCityType = provisionInfo.MsgTextInfo.SelectedCityType;

                foreach (MsgText msgTxt in provisionInfo.MsgTextInfo.CurrentTransmitMsgText)
                {
                    if (msgTxt.LanguageKindID != languageKind.ID)
                    {
                        continue;
                    }
                    if (msgTxt.CityTypeID != selectedCityType.ID)
                    {
                        continue;
                    }

                    MsgTextDisplayMediaType mediaType = BasisData.FindMediaTypeInfoByID(msgTxt.MediaTypeID);
                    if (mediaType != null)
                    {
                        NameValueType element = new NameValueType();
                        element.Name = mediaType.Code + "Text";
                        element.Value = msgTxt.Text;

                        info.Parameter.Add(element);
                    }
                }


                //ResourceType resource = new ResourceType();
                //resource.ResourceDesc = "사용안함";
                //resource.MimeType = "사용안함";
                //info.Resource.Add(resource);

                AreaType areaType = new AreaType();
                info.Area.Add(areaType);

                string areaDescName = string.Empty;
                StringBuilder builder = new StringBuilder();
                if (provisionInfo.Scope != ScopeType.Private && provisionInfo.TargetRegions != null)
                {
                    areaDescName = string.Join(" ", provisionInfo.TargetRegions);
                }
                else if (provisionInfo.TargetSystems != null)
                {
                    areaDescName = string.Join(" ", provisionInfo.TargetSystems);
                }
                else
                {
                }

                areaDescName = areaDescName.Trim();

                if (!string.IsNullOrEmpty(areaDescName))
                {
                    areaDescName = areaDescName.Replace("[", "");
                    areaDescName = areaDescName.Replace("]", "");
                    areaDescName = areaDescName.Replace("/", " ");
                }
                areaType.AreaDesc = areaDescName;

                if (provisionInfo.Scope != ScopeType.Private && provisionInfo.TargetRegions != null)
                {
                    foreach (RegionDefinition region in provisionInfo.TargetRegions)
                    {
                        if (region == null)
                        {
                            FileLogManager.GetInstance().WriteLog("[Core] MakeCAP( 발령 정보 설정 중 오류가 발생하였습니다. 발령 대상 지역 정보에 널인 데이터가 존재합니다. )");

                            throw new Exception("발령 정보 설정 중 오류가 발생하였습니다. 발령 대상 지역 정보에 널인 데이터가 존재합니다.");
                        }
                        NameValueType areaNameValueType = new NameValueType();
                        areaNameValueType.Name = "KRDSTGeocode";
                        areaNameValueType.Value = region.Code.ToString();
                        areaType.GeoCode.Add(areaNameValueType);
                    }
                }
                else if (provisionInfo.TargetSystems != null)
                {
                    foreach (SASProfile profile in provisionInfo.TargetSystems)
                    {
                        if (profile == null)
                        {
                            FileLogManager.GetInstance().WriteLog("[Core] MakeCAP( 발령 정보 설정 중 오류가 발생하였습니다. 발령 대상 시스템 정보에 널인 데이터가 존재합니다. )");

                            throw new Exception("발령 정보 설정 중 오류가 발생하였습니다. 발령 대상 시스템 정보에 널인 데이터가 존재합니다.");
                        }
                        NameValueType areaNameValueType = new NameValueType();
                        areaNameValueType.Name = "KRDSTGeocode";
                        areaNameValueType.Value = profile.IpAddress.Trim();
                        areaType.GeoCode.Add(areaNameValueType);
                    }
                }
                else
                {
                    Debug.Assert(false);
                }

                if (provisionInfo.Circle != null)
                {
                    foreach (AdengGE.CircleInfo circle in provisionInfo.Circle)
                    {
                        areaType.Circle.Add(circle.centerX + "," + circle.centerY + " " + (circle.radiusInMeter / 1000));
                    }
                }

                // 헤드라인 정보 설정.
                info.Headline = helper.MakeHeadline(capMsg);
                if (string.IsNullOrEmpty(info.Headline))
                {
                    System.Console.WriteLine("[Core] MakeCAP( 헤드라인 데이터 오류)");
                    FileLogManager.GetInstance().WriteLog("[Core] MakeCAP( 발령 정보 설정 중 오류가 발생하였습니다. 헤드라인 정보가 올바르지 않습니다. )");

                    throw new Exception("발령 정보 설정 중 오류가 발생하였습니다. 헤드라인 정보가 올바르지 않습니다.");
                }
                if (provisionInfo.RefType == OrderReferenceType.Cancel)
                {
                    info.Headline = info.Headline + " 취소";
                }
            }

            return capMsg;
        }
        /// <summary>
        /// 발령 정보 등록.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int RegistOrder(OrderRecord info)
        {
            System.Diagnostics.Debug.Assert(info != null);
            System.Diagnostics.Debug.Assert(info.CapText != null);

            int result = 0;

            result = DBManager.GetInstance().RegistOrderRecord(info);
            if (result != 0)
            {
                System.Console.WriteLine("[Core] RegistOrder(발령 기록 등록 실패)");
                FileLogManager.GetInstance().WriteLog("[Core] RegistOrder( 발령 기록 등록 실패. result=[ " + result + "] )");
            }
            else
            {
                if (info.RefType == OrderReferenceType.Clear)
                {
                    int updated = DBManager.GetInstance().UpdateAlertingClearStateOfOrderRecord(info.RefRecordID, ClearAlertState.Clear);
                    if (updated != 0)
                    {
                        System.Console.WriteLine("[Core] RegistOrder(경보해제 상태 갱신 실패)");
                        FileLogManager.GetInstance().WriteLog("[Core] RegistOrder( 경보해제 상태 갱신 실패. updated=[ " + updated + "] )");
                    }
                }
                else if (info.RefType == OrderReferenceType.SWR)
                {
                    int updated = DBManager.GetInstance().UpdateSWRAssociationState(info.RefRecordID, SWRAssociationStateCode.Order);
                    if (updated != 0)
                    {
                        System.Console.WriteLine("[Core] RegistOrder(기상특보연계 완료 갱신 실패)");
                        FileLogManager.GetInstance().WriteLog("[Core] RegistOrder( 기상특보 연계상태 갱신 실패. updated=[ " + updated + "] )");
                    }
                }
                else
                {
                }

                result = RegistOrderResponse(info);
                if (result != 0)
                {
                    System.Console.WriteLine("[Core] RegistOrder( 발령 예상 응답 등록 실패 )");
                    FileLogManager.GetInstance().WriteLog("[Core] RegistOrder( 발령 예상 응답 등록 실패. updated=[ " + result + "] )");
                }
            }

            // UI 통지
            if (this.NotifyLatestOrderInfoChanged != null)
            {
                string headline = info.GetHeadlineString();

                this.NotifyLatestOrderInfoChanged(this, new OrderEventArgs(info, headline));
            }

            return result;
        }
        /// <summary>
        /// 타 지역 발령 정보 저장(이력 관리용)
        /// </summary>
        /// <param name="data"></param>
        private int SaveOrderFromUpper(ReceivedCAPInfo data)
        {
            System.Diagnostics.Debug.Assert(data != null);

            int result = 0;

            if (data.Msg == null)
            {
                return -1;
            }

            // 발령 기록 등록
            OrderRecord newRecord = new OrderRecord();
            newRecord.CAPID = data.Msg.MessageID;
            newRecord.OrderMode = data.Msg.MessageStatus.Value;
            newRecord.OrderedTime = data.Msg.SentDateTime.ToLocalTime();
            newRecord.RefType = OrderReferenceType.None;
            newRecord.RefRecordID = data.Msg.ReferenceIDs;
            newRecord.LocationKind = OrderLocationKind.Other;

            string disasterKindCode = string.Empty;
            if (data.Msg != null && data.Msg.Info != null && data.Msg.Info.Count > 0)
            {
                InfoType infoType = data.Msg.Info[0];
                if (infoType.EventCode != null)
                {
                     disasterKindCode = infoType.EventCode[0].Value;
                }
            }
            newRecord.DisasterKindCode = disasterKindCode;
            newRecord.ClearState = new AlertingClearState();
            newRecord.ClearState.DeepCopyFrom(BasisData.AlertingClearStateInfo[ClearAlertState.Exclude]);
            newRecord.CapText = data.Msg.WriteToXML();

            result = RegistOrder(newRecord);
            if (result != 0)
            {
                System.Console.WriteLine("[Core] SaveOrderFromUpper( RegistOrder Eror(" + result + ") )");
            }

            return result;
        }
        private int RegistOrderResponse(OrderRecord info)
        {
            System.Diagnostics.Debug.Assert(info != null);

            if (info == null)
            {
                return -1;
            }

            CAP msg = new CAP(info.CapText);
            CAPHelper helper = new CAPHelper();
            List<RegionDefinition> targetRegions = null;
            List<SASKind> targetKinds = null;
            List<SASProfile> targetSystems = null;

            if (msg.Scope == ScopeType.Public || msg.Scope == ScopeType.Restricted)
            {
                targetRegions = helper.ExtractTargetRegionsFromCAP(msg);
            }
            if (msg.Scope == ScopeType.Restricted)
            {
                targetKinds = helper.ExtractTargetKindsFromCAP(msg);
            }
            if (msg.Scope == ScopeType.Private)
            {
                targetSystems = helper.ExtractTargetSystemsFromCAP(msg);
            }

            List<string> expectedTargetSystems = null;

            if (targetRegions != null)
            {
                int result = DBManager.GetInstance().ExpectedTargetSystemList(targetRegions, targetKinds, out expectedTargetSystems);
            }

            if (targetSystems != null)
            {
                if (expectedTargetSystems == null)
                {
                    expectedTargetSystems = new List<string>();
                }
                foreach (SASProfile system in targetSystems)
                {
                    expectedTargetSystems.Add(system.ID);
                }
            }

            if (expectedTargetSystems != null)
            {
                foreach (string systemID in expectedTargetSystems)
                {
                    OrderResponseProfile data = new OrderResponseProfile();
                    data.ID = null;
                    data.ReferenceCapID = info.CAPID;
                    //if (info.RefType != OrderReferenceType.SWR)
                    //{
                    //    data.ReferenceCapID = info.CAPData.MessageID;
                    //}
                    data.SenderID = systemID;
                    data.SenderType = SenderTypes.SAS;
                    data.CapMsg = null;
                    data.SystemManagerName = null;
                    data.SystemManagerDepartment = null;
                    data.SystemManagerPhone = null;

                    int result = DBManager.GetInstance().UpdateOrderResponse(data);
                    if (result != 0)
                    {
                        System.Console.WriteLine("[Core] RegistOrderResponse( UpdateOrderResponse Failure(" + result + "))");
                    }
                }
            }

            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void SaveOrderResponse(ReceivedCAPInfo data)
        {
            OrderResponseProfile response = new OrderResponseProfile();
            response.ID = data.Msg.MessageID;
            response.CapMsg = data.Msg.WriteToXML();

            string[] spliter = {",", };
            string[] divideStr = data.Msg.ReferenceIDs.Split(spliter, StringSplitOptions.RemoveEmptyEntries);
            if (divideStr != null && divideStr.Length > 1)
            {
                response.ReferenceCapID = divideStr[1];
            }
            response.SenderID = data.Msg.SenderID;
            response.SenderType = data.MsgSenderType;

            int result = DBManager.GetInstance().UpdateOrderResponse(response);
            if (result != 0)
            {
                System.Console.WriteLine("[Core] SaveOrderResponse Failure(" + result + ")");
            }

            if (this.NotifyOrderResponseUpdated != null)
            {
                List<OrderResponseProfile> responseInfo = null;
                result = DBManager.GetInstance().QueryOrderResponse(response.ReferenceCapID, out responseInfo);
                if (result != 0 || responseInfo == null)
                {
                    System.Console.WriteLine("[Core] SaveOrderResponse( QueryOrderResponse Failure=[" + result + "] )");
                }
                else
                {
                    this.NotifyOrderResponseUpdated(this, new OrderResponseEventArgs(response.ReferenceCapID, responseInfo));
                }
            }
        }
        /// <summary>
        /// 최근 발령 이력 30건 조회
        /// </summary>
        /// <returns></returns>
        public List<OrderRecord> GetRecentlyOrderList()
        {
            List<OrderRecord> history = null;
            int result = DBManager.GetInstance().QueryRecentlyOrderHistory(out history);
            if (result != 0)
            {
                System.Console.WriteLine("[Core] GetRecentlyOrderList( 최근 발령 이력 조회 실패 =[" + result + "] )");
            }

            return history;
        }

        /// <summary>
        /// [경보 해제] 경보 상황 해제 대기 목록 조회.
        /// </summary>
        /// <returns></returns>
        public List<OrderRecord> GetWaitToClearAlertList()
        {
            List<OrderRecord> list = null;
            int result = DBManager.GetInstance().QueryWaitToClearAlertList(out list);
            if (result != 0)
            {
                System.Console.WriteLine("[Core] GetWaitToClearAlertList( 경보 상황 해제 대기 목록 조회 실패 =[" + result + "] )");
            }

            return list;
        }
        #endregion

        #region 기상특보이력
        /// <summary>
        /// 기상특보 등록.
        /// </summary>
        /// <param name="reportInfo"></param>
        /// <returns></returns>
        public int RegistSWR(List<SWRProfile> reportInfo)
        {
            System.Diagnostics.Debug.Assert(reportInfo != null);

            int result = DBManager.GetInstance().RegistSWRInfo(reportInfo);
            if (result != 0)
            {
                System.Console.WriteLine("[Core] RegistSWR( 기상특보 등록 실패 =[" + result + "] )");
            }

            //// 최신 기상특보프로필 아이디 정보 갱신.
            //string latestProfileID = QueryLatestSWRProfileID();
            //if (!string.IsNullOrEmpty(latestProfileID))
            //{
            //    SWRServiceManager.GetInstance().SetLatestProfileID(latestProfileID);
            //}
            return result;
        }
        /// <summary>
        /// 연계발령 대기 중인 기상특보 목록을 얻는다.
        /// </summary>
        /// <returns></returns>
        public int GetWaitToOrderSWRList(out List<SWRProfile> profileList)
        {
            profileList = null;

            int result = DBManager.GetInstance().QueryWaitingToOrderSWRList(out profileList);
            if (result != 0)
            {
                System.Console.WriteLine("[Core] GetWaitToClearAlertList( 발령대기 중인 기상특보목록 조회 실패 =[" + result + "] )");
                return -1;
            }

            return result;
        }
        /// <summary>
        /// 등록된 기상특보 중 마지막 특보번호를 조회.
        /// </summary>
        /// <returns></returns>
        public string QueryLatestSWRProfileID()
        {
            string reportSeq = string.Empty;
            int result = DBManager.GetInstance().QueryLatestSWRProfileID(out reportSeq);
            if (result == 0)
            {
                if (string.IsNullOrEmpty(reportSeq))
                {
                    reportSeq = DateTime.Today.ToString("yyyyMM") + "000";
                }
            }
            else
            {
                reportSeq = string.Empty;
            }

            return reportSeq;
        }
        /// <summary>
        /// 기상특보 발령연계상태 갱신.
        /// </summary>
        /// <param name="recordID"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public int UpdateSWRAssociationState(string recordID, SWRAssociationStateCode state)
        {
            int ret = 0;
            ret = DBManager.GetInstance().UpdateSWRAssociationState(recordID, state);
            return ret;
        }
        #endregion

        #region 기상특보연계설정
        public List<SWRAssociationCondition> GetSWRAssociationCondition()
        {
            return DBManager.GetInstance().QuerySWRAssociationCondition();
        }
        public int UpdateSWRAssociationCondition(List<SWRAssociationCondition> conditions)
        {
            return DBManager.GetInstance().UpdateSWRAssociationCondition(conditions);
        }
        #endregion

        #region 발령 그룹 정보
        /// <summary>
        /// [지역 그룹 프로필] 조회.
        /// </summary>
        /// <returns></returns>
        public List<GroupProfile> QueryRegionGroupInfo()
        {
            return DBManager.GetInstance().QueryGroupInfo("R");
        }
        /// <summary>
        /// [시스템 그룹 프로필] 조회.
        /// </summary>
        /// <returns></returns>
        public List<GroupProfile> QuerySystemGroupInfo()
        {
            return DBManager.GetInstance().QueryGroupInfo("S");
        }
        /// <summary>
        /// 그룹 생성.
        /// </summary>
        /// <param name="groupInfo"></param>
        /// <returns></returns>
        public int CreateGroup(GroupProfile groupInfo)
        {
            return DBManager.GetInstance().RegistGroup(groupInfo);
        }
        /// <summary>
        /// 그룹 프로필 갱신.
        /// </summary>
        /// <param name="groupInfo"></param>
        /// <returns></returns>
        public int UpdateGroupInfo(GroupProfile groupInfo)
        {
            return DBManager.GetInstance().UpdateGroupInfo(groupInfo);
        }
        /// <summary>
        /// 그룹 프로필 삭제.
        /// </summary>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public int DeleteGroup(string groupID)
        {
            return DBManager.GetInstance().DeleteGroupInfo(groupID);
        }
        #endregion


        #region 송수신 데이터 처리
        /// <summary>
        /// CAP 수신데이터 처리 시작
        /// </summary>
        private void StartCAPDataProcessing()
        {
            System.Console.WriteLine("[Core] StartCAPDataProcessing (start)");

            try
            {
                this.isCAPDataProcessingContinue = true;

                if (this.processCAPThread == null)
                {
                    this.processCAPThread = new Thread(new ThreadStart(CAPDataProcessing));
                    this.processCAPThread.IsBackground = true;
                    this.processCAPThread.Name = "CAPProcessingThread";
                    this.processCAPThread.Start();
                }

                CommunicationManager.GetInstance().StartDataProcessing();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[Core] StartCAPDataProcessing( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[Core] StartCAPDataProcessing( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// CAP 데이터 처리 종료 (쓰레드 삭제 및 서비스 해제)
        /// </summary>
        private void EndCAPDataProcessing()
        {
            System.Console.WriteLine("[Core] EndCAPDataProcessing (start)");

            try
            {
                CommunicationManager.GetInstance().EndDataProcessing();

                this.isCAPDataProcessingContinue = false;
                if (this.manualEvtCAP != null)
                {
                    this.manualEvtCAP.Set();
                }
                if (this.processCAPThread != null && this.processCAPThread.IsAlive)
                {
                    bool isTerminated = this.processCAPThread.Join(500);
                    if (!isTerminated)
                    {
                        this.processCAPThread.Abort();
                    }
                }
                this.processCAPThread = null;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[Core] EndCAPDataProcessing (" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[Core] EndCAPDataProcessing( " + ex.ToString() + " )");
            }

            System.Console.WriteLine("[Core] EndCAPDataProcessing (end)");
        }

        /// <summary>
        /// 데이터동기화 수신데이터 처리 시작
        /// </summary>
        private void StartDataSynchronizationProcessing()
        {
            System.Console.WriteLine("[Core] StartDataSynchronizationProcessing (start)");

            try
            {
                this.isDataSyncProcessingContinue = true;

                if (this.processDataSyncThread == null)
                {
                    this.processDataSyncThread = new Thread(new ThreadStart(DataSyncProcessing));
                    this.processDataSyncThread.IsBackground = true;
                    this.processDataSyncThread.Name = "DataSyncProcessingThread";
                    this.processDataSyncThread.Start();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[Core] StartDataSynchronizationProcessing (" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[Core] StartDataSynchronizationProcessing( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// 데이터동기화 수신데이터 처리 종료 (쓰레드 삭제 및 서비스 해제)
        /// </summary>
        private void EndDataSynchronizationProcessing()
        {
            System.Console.WriteLine("[Core] EndDataSynchronizationProcessing (start)");

            try
            {
                CommunicationManager.GetInstance().EndDataProcessing();

                this.isDataSyncProcessingContinue = false;
                if (this.manualEvtSYNC != null)
                {
                    this.manualEvtSYNC.Set();
                }
                if (this.processDataSyncThread != null && this.processDataSyncThread.IsAlive)
                {
                    bool isTerminated = this.processDataSyncThread.Join(500);
                    if (!isTerminated)
                    {
                        this.processDataSyncThread.Abort();
                    }
                }
                this.processDataSyncThread = null;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[Core] EndDataSynchronizationProcessing (" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[Core] EndDataSynchronizationProcessing( " + ex.ToString() + " )");
            }

            System.Console.WriteLine("[Core] EndDataSynchronizationProcessing (end)");
        }

        /// <summary>
        /// 기상특보 데이터 처리 시작 (쓰레드 생성 및 서비스 접속)
        /// </summary>
        private void StartSWRDataProcessing(string latestReportID)
        {
            System.Console.WriteLine("[Core] StartSWRDataProcessing (start)");

            try
            {
                this.isSWRDataProcessingContinue = true;
                if (this.processSWRThread == null)
                {
                    this.processSWRThread = new Thread(new ThreadStart(SWRDataProcessing));
                    this.processSWRThread.IsBackground = true;
                    this.processSWRThread.Name = "SWRDataProcessingThread";
                    this.processSWRThread.Start();
                }

                SWRServiceManager.GetInstance().StartCheckingIssue(latestReportID);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[Core] StartSWRDataProcessing (" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[Core] StartSWRDataProcessing( " + ex.ToString() + " )");
            }

            System.Console.WriteLine("[Core] StartSWRDataProcessing (end)");
        }
        /// <summary>
        /// 기상특보 데이터 처리 종료 (쓰레드 삭제 및 서비스 해제)
        /// </summary>
        private void EndSWRDataProcessing()
        {
            System.Console.WriteLine("[Core] EndSWRDataProcessing (start)");

            try
            {
                // 기상특보데이터요청 쓰레드 종료
                SWRServiceManager.GetInstance().EndCheckingIssue();

                // 기상특보 데이터 수신 처리 쓰레드 종료(로컬)
                this.isSWRDataProcessingContinue = false;
                if (this.manualEvtSWR != null)
                {
                    this.manualEvtSWR.Set();
                }
                if (this.processSWRThread != null && this.processSWRThread.IsAlive)
                {
                    bool isTerminated = this.processSWRThread.Join(500);
                    if (!isTerminated)
                    {
                        this.processSWRThread.Abort();
                    }
                }
                this.processSWRThread = null;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[Core] EndSWRDataProcessing (" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[Core] EndSWRDataProcessing( " + ex.ToString() + " )");
            }

            System.Console.WriteLine("[Core] EndSWRDataProcessing (end)");
        }

        /// <summary>
        /// CAP 데이터 처리 쓰레드 함수
        /// </summary>
        private void CAPDataProcessing()
        {
            System.Console.WriteLine("[Core] CAPDataProcessing (start)");

            try
            {
                if (this.manualEvtCAP == null)
                {
                    this.manualEvtCAP = new ManualResetEvent(false);
                }

                while (this.isCAPDataProcessingContinue)
                {
                    int count = 0;

                    lock (this.dataCAPQueue)
                    {
                        count = this.dataCAPQueue.Count;
                    }

                    if (count <= 0)
                    {
                        this.manualEvtCAP.WaitOne();
                        this.manualEvtCAP.Reset();

                        continue;
                    }

                    // 수신 데이터 처리
                    ReceivedCAPInfo bufferdCap = null;
                    lock (this.dataCAPQueue)
                    {
                        bufferdCap = this.dataCAPQueue.Dequeue();
                    }

                    switch (bufferdCap.Msg.MessageType)
                    {
                        case CAPLib.MsgType.Alert:
                        case CAPLib.MsgType.Update:
                        case CAPLib.MsgType.Cancel:
                            {
                                // 통합경보게이트웨이를 통해서 표준발령대 쪽으로 발령 관련 메시지가 수신되는 것은,
                                // 상위에서 내린 발령과 동일 지역의 동일 레벨 발령대에서 내린 발령을 연계하는 경우 뿐이다.
                                if (bufferdCap.MsgSenderType == SenderTypes.SWI)
                                {
                                    SaveOrderFromUpper(bufferdCap);
                                }
                            }
                            break;
                        case CAPLib.MsgType.Ack:
                        case CAPLib.MsgType.Error:
                            {
                                SaveOrderResponse(bufferdCap);
                            }
                            break;
                        default:
                            // 데이터 무효: 버림
                            // 이런 경우가 있을리 없지만, 로그는 남기자
                            break;
                    }

                    bufferdCap = null;
                }
            }
            catch (ThreadAbortException ex)
            {
                System.Console.WriteLine("[Core] CAPDataProcessing( Exception=[ ThreadAbortException ] )");
                FileLogManager.GetInstance().WriteLog("[Core] CAPDataProcessing( Exception=[ ThreadAbortException ] )");

                Thread.ResetAbort();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[Core] CAPDataProcessing (" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[Core] CAPDataProcessing( " + ex.ToString() + " )");

                throw new Exception("[Core] CAP 데이터 처리 중에 예외가 발생하였습니다.");
            }
            finally
            {
                FileLogManager.GetInstance().WriteLog("[Core] CAPDataProcessing( end )");

                if (this.manualEvtCAP != null)
                {
                    this.manualEvtCAP.Close();
                    this.manualEvtCAP = null;
                }
            }
        }
        /// <summary>
        /// 데이터 동기화 데이터 처리 쓰레드 함수
        /// </summary>
        private void DataSyncProcessing()
        {
            System.Console.WriteLine("[Core] DataSyncProcessing( START )");
            FileLogManager.GetInstance().WriteLog("[Core] DataSyncProcessing( START )");

            try
            {
                if (this.manualEvtSYNC == null)
                {
                    this.manualEvtSYNC = new ManualResetEvent(false);
                }

                while (this.isDataSyncProcessingContinue)
                {
                    int count = this.dataSyncInfoQueue.Count;
                    System.Console.WriteLine("[Core] DataSyncProcessing (dataSyncInfoQueue.Count[" + count + "])");

                    if (count <= 0)
                    {
                        System.Console.WriteLine("Start WaitOne");

                        this.manualEvtSYNC.WaitOne();
                        this.manualEvtSYNC.Reset();

                        continue;
                    }

                    SynchronizationReqData bufferedSyncData = this.dataSyncInfoQueue.Dequeue();
                    if (bufferedSyncData == null)
                    {
                        System.Console.WriteLine("[Core] DataSyncProcessing( 큐 데이터가 NULL )");
                        FileLogManager.GetInstance().WriteLog("[Core] DataSyncProcessing START( 큐 데이터가 NULL )");
                        continue;
                    }

                    // 수신 데이터 유효성 체크, 결과 전송
                    DataValidationCheckResult validateResult = CheckReceivedDataValidationOfDataSync(bufferedSyncData.EventID, bufferedSyncData);
                    int sendValidateResult = CommunicationManager.GetInstance().SendResultOfCheckDataValidation(bufferedSyncData.EventID, validateResult);
                    if (validateResult != DataValidationCheckResult.Success || sendValidateResult != 0)
                    {
                        System.Console.WriteLine("[Error] 수신 데이터 유효성 체크 오류(validateResult=[" + validateResult.ToString() + "], sendResult=[" + sendValidateResult.ToString() + "])");
                        FileLogManager.GetInstance().WriteLog("[Core] CAPDataProcessing( 수신 데이터 유효성 체크 오류(validateResult=[" + validateResult.ToString() + "], sendResult=[" + sendValidateResult.ToString() + "])");
                        continue;
                    }

                    System.Console.WriteLine("bufferedSyncData.CommandCode=[" + bufferedSyncData.Command + ", " + bufferedSyncData.Command.ToString() + "])");
                    switch (bufferedSyncData.Command)
                    {
                        case DataSyncCommand.ChkEntireHashKey:
                            {
                                // 전체 해쉬키 체크
                                bool checkResult = false;
                                byte[] hashCode = null;
                                int result = DBManager.GetInstance().GetEntireHashKey(out hashCode);
                                if (result == 0)
                                {
                                    checkResult = CompareHashKey(bufferedSyncData.ProfileHashKey, hashCode);
                                }

                                // 전체 해쉬키 체크 결과 전송
                                bool sendResult = CommunicationManager.GetInstance().SendResultOfCheckEntireHashKey(bufferedSyncData.EventID, checkResult);
                                System.Console.WriteLine("[Core] CAPDataProcessing( 전체 해쉬키 체크 결과 전송 - checkResult=[" + checkResult + "], sendResult=[" + sendResult + "] )");
                                FileLogManager.GetInstance().WriteLog("[Core] CAPDataProcessing( 전체 해쉬키 체크 결과 전송 - checkResult=[" + checkResult + "], sendResult=[" + sendResult + "] )");
                            }
                            break;

                        case DataSyncCommand.ChkSingleHashKey:
                            {
                                uint currentReqEventID = bufferedSyncData.EventID;
                                uint totalPacketCount = bufferedSyncData.TotalPacketCnt;
                                uint currentPacketNo = bufferedSyncData.CurrentPacketNo;

                                // 기본 컨셉상 모든 재시도 횟수를 넘지 않는 한, 개별 체크 요청 중에 다른 요청이 들어올 일은 없으나, 
                                // 요청 이벤트 아이디가 다른 데이터가 수신된 경우, 이제까지 저장한 모든 데이터를 버린다.
                                if (this.dataSyncSingleHashReqData != null && currentReqEventID != this.dataSyncSingleHashReqData.ReqEventID)
                                {
                                    System.Console.WriteLine("[Core] DataSyncProcessing( 패킷 아이디 불일치: PreviousReqEventID=[" + this.dataSyncSingleHashReqData.ReqEventID + "] =/= CurrentReqEventID=[" + currentReqEventID + "]) )");
                                    FileLogManager.GetInstance().WriteLog("[Core] CAPDataProcessing( 패킷 아이디 불일치: PreviousReqEventID=[" + this.dataSyncSingleHashReqData.ReqEventID + "] =/= CurrentReqEventID=[" + currentReqEventID + "]) )");

                                    this.dataSyncSingleHashReqData = null;
                                }
                                if (this.dataSyncSingleHashReqData == null)
                                {
                                    this.dataSyncSingleHashReqData = new ProfileHashCheckReqData(currentReqEventID, totalPacketCount);
                                }

                                // 현재 수신된 패킷을 분할데이터 전용 메모리에 저장
                                SASProfileHash hashInfo = new SASProfileHash(bufferedSyncData.ProfileID, bufferedSyncData.ProfileHashKey);
                                bool insertRet = this.dataSyncSingleHashReqData.Insert((int)(currentPacketNo - 1), hashInfo);
                                if (!insertRet)
                                {
                                    System.Console.WriteLine("[Core] DataSyncProcessing( 데이터 저장 실패(currentPacketNo/totalPacketCnt=[" + currentPacketNo + "/" + totalPacketCount + "]) )");
                                    FileLogManager.GetInstance().WriteLog("[Core] CAPDataProcessing( 데이터 저장 실패(currentPacketNo/totalPacketCnt=[" + currentPacketNo + "/" + totalPacketCount + "]) )");

                                    continue;
                                }

                                // 분할 데이터가 전부(TotalCount만큼) 도착했는지 체크
                                if (!this.dataSyncSingleHashReqData.IsAllSet())
                                {
                                    List<uint> lstLossedPacketNo = new List<uint>();
                                    // 현재 수신한 분할패킷의 앞 번호까지만 패킷손실 체크를 수행
                                    for (int index = 0; index < currentPacketNo; index++)
                                    {
                                        SASProfileHash profile = this.dataSyncSingleHashReqData.LstProfileHash[index];
                                        if (string.IsNullOrEmpty(profile.ProfileID) || (null == profile.HashKey || profile.HashKey.Length <= 0))
                                        {
                                            lstLossedPacketNo.Add((uint)(index + 1));
                                        }
                                    }

                                    // 패킷 손실 통지
                                    if (lstLossedPacketNo.Count > 0)
                                    {
                                        int sendDataResult = CommunicationManager.GetInstance().SendResultOfCheckDataValidation(currentReqEventID, DataValidationCheckResult.PacketLoss, lstLossedPacketNo);
                                    }

                                    // 분할 데이터 수신 완료시까지
                                    continue;
                                }

                                System.Console.WriteLine("[Core] DataSyncProcessing( 분할 데이터 수신 완료 : 요청 처리 및 결과 전송 준비 시작 )");
                                FileLogManager.GetInstance().WriteLog("[Core] DataSyncProcessing( 분할 데이터 수신 완료 : 요청 처리 및 결과 전송 준비 시작 )");


                                List<SASProfileHash> lstSrcProfileHash = new List<SASProfileHash>();
                                foreach (SASProfileHash profileHash in this.dataSyncSingleHashReqData.LstProfileHash)
                                {
                                    SASProfileHash newProfileHash = new SASProfileHash();
                                    newProfileHash.ProfileID = profileHash.ProfileID;
                                    newProfileHash.HashKey = profileHash.HashKey;

                                    lstSrcProfileHash.Add(newProfileHash);
                                }
                                List<string> lstUnmatchedProfileID = null;
                                List<SASProfile> lstDelectedProfile = null;
                                int result = CheckSASProfileHashKey(lstSrcProfileHash, out lstUnmatchedProfileID, out lstDelectedProfile);
                                if (result != 0)
                                {
                                    System.Console.WriteLine("[Core] DataSyncProcessing( 개별 해쉬키 체크 실패: result=[" + result + "] )");
                                    FileLogManager.GetInstance().WriteLog("[Core] DataSyncProcessing( 개별 해쉬키 체크 실패: result=[" + result + "] )");
                                    continue;
                                }

                                bool sendResult = CommunicationManager.GetInstance().SendResultOfCheckSingleHashKey(this.dataSyncSingleHashReqData.ReqEventID, lstUnmatchedProfileID);
                                if (!sendResult)
                                {
                                    System.Console.WriteLine("[Core] DataSyncProcessing( 개별 해쉬키 체크 결과 전송 실패 )");
                                    FileLogManager.GetInstance().WriteLog("[Core] DataSyncProcessing( 개별 해쉬키 체크 결과 전송 실패 )");
                                }

                                if (lstDelectedProfile != null && lstDelectedProfile.Count > 0)
                                {
                                    ProfileUpdateReqData deletedDataInfo = new ProfileUpdateReqData(currentReqEventID, (uint)lstDelectedProfile.Count, ProfileUpdateMode.Delete);
                                    foreach (SASProfile profile in lstDelectedProfile)
                                    {
                                        SASProfile copyObj = new SASProfile();
                                        copyObj.DeepCopyFrom(profile);
                                        deletedDataInfo.LstSASProfile.Add(copyObj);

                                        if (this.NotifySASProfileUpdated != null)
                                        {
                                            this.NotifySASProfileUpdated(this, new SASProfileUpdateEventArgs(deletedDataInfo));
                                        }
                                    }
                                }
                                System.Console.WriteLine("[Core] DataSyncProcessing( 개별 해쉬키 처리 및 결과 전송 완료 )");
                                FileLogManager.GetInstance().WriteLog("[Core] DataSyncProcessing( 개별 해쉬키 체크 결과 전송 완료 )");
                            }
                            break;

                        case DataSyncCommand.UpdateProfileData:
                            {
                                System.Console.WriteLine("[Core] DataSyncProcessing(UpdateProfileData) - TotalPacketCnt:" + bufferedSyncData.TotalPacketCnt);
                                FileLogManager.GetInstance().WriteLog("[Core] DataSyncProcessing( UpdateProfileData - TotalPacketCnt:" + bufferedSyncData.TotalPacketCnt);

                                uint currentReqEventID = bufferedSyncData.EventID;
                                uint totalPacketCount = bufferedSyncData.TotalPacketCnt;
                                uint currentPacketNo = bufferedSyncData.CurrentPacketNo;

                                if (totalPacketCount < 1 || currentPacketNo < 1)
                                {
                                    System.Console.WriteLine("[Core] DataSyncProcessing( 패킷 번호 오류: currentPacketNo=[" + currentPacketNo + "] =/= totalPacketCount=[" + totalPacketCount + "]) )");
                                    FileLogManager.GetInstance().WriteLog("[Core] DataSyncProcessing( 패킷 번호 오류: currentPacketNo=[" + currentPacketNo + "] =/= totalPacketCount=[" + totalPacketCount + "]) )");
                                    continue;
                                }

                                // 기본 컨셉상 모든 재시도 횟수를 넘지 않는 한, 개별 체크 요청 중에 다른 요청이 들어올 일은 없으나, 
                                // 요청 이벤트 아이디가 다른 데이터가 수신된 경우, 이제까지 저장한 모든 데이터를 버린다.
                                if (this.dataSyncProfileUpdateReqData != null && (currentReqEventID != this.dataSyncProfileUpdateReqData.ReqEventID))
                                {
                                    System.Console.WriteLine("[Core] DataSyncProcessing( 패킷 아이디 불일치: PreviousReqEventID=[" + this.dataSyncProfileUpdateReqData.ReqEventID + "] =/= CurrentReqEventID=[" + currentReqEventID + "]) )");
                                    FileLogManager.GetInstance().WriteLog("[Core] DataSyncProcessing( 패킷 아이디 불일치: PreviousReqEventID=[" + this.dataSyncProfileUpdateReqData.ReqEventID + "] =/= CurrentReqEventID=[" + currentReqEventID + "]) )");

                                    this.dataSyncProfileUpdateReqData = null;
                                }
                                if (this.dataSyncProfileUpdateReqData == null)
                                {
                                    this.dataSyncProfileUpdateReqData = new ProfileUpdateReqData(currentReqEventID, totalPacketCount, bufferedSyncData.Mode);
                                }

                                // 현재 수신된 패킷을 분할데이터 전용 메모리에 저장
                                bool insertRet = this.dataSyncProfileUpdateReqData.Insert((int)(currentPacketNo - 1), bufferedSyncData.SystemProfile);
                                if (!insertRet)
                                {
                                    System.Console.WriteLine("[Core] DataSyncProcessing( 데이터 저장 실패(currentPacketNo/totalPacketCnt=[" + currentPacketNo + "/" + totalPacketCount + "]) )");
                                    FileLogManager.GetInstance().WriteLog("[Core] DataSyncProcessing( 데이터 저장 실패(currentPacketNo/totalPacketCnt=[" + currentPacketNo + "/" + totalPacketCount + "]) )");

                                    continue;
                                }

                                // 분할 데이터가 전부(TotalCount만큼) 도착했는지 체크
                                if (!this.dataSyncProfileUpdateReqData.IsAllSet())
                                {
                                    List<uint> lstLossedPacketNo = new List<uint>();
                                    // 패킷손실 체크는 현재 수신한 분할패킷의 앞 번호까지만 수행한다.
                                    for (int index = 0; index < currentPacketNo; index++)
                                    {
                                        SASProfile profile = this.dataSyncProfileUpdateReqData.LstSASProfile[index];
                                        if (string.IsNullOrEmpty(profile.ID) || string.IsNullOrEmpty(profile.Name))
                                        {
                                            lstLossedPacketNo.Add((uint)(index + 1));
                                        }
                                    }

                                    // 패킷 손실 통지
                                    if (lstLossedPacketNo.Count > 0)
                                    {
                                        int sendDataResult = CommunicationManager.GetInstance().SendResultOfCheckDataValidation(currentReqEventID, DataValidationCheckResult.PacketLoss, lstLossedPacketNo);
                                        if (sendDataResult != 0)
                                        {
                                            FileLogManager.GetInstance().WriteLog("[Core] DataSyncProcessing( 손실 패킷 재요청 실패 sendDataResult=[" + sendDataResult + "] )");
                                        }
                                    }

                                    // 분할 데이터 수신 완료시까지
                                    continue;
                                }

                                System.Console.WriteLine("[Core] DataSyncProcessing( 프로필 갱신 분할 데이터 수신 완료 : 요청 처리 및 결과 전송 준비 시작 )");

                                int updateResult = 0;
                                if (bufferedSyncData.Mode == ProfileUpdateMode.Regist || bufferedSyncData.Mode == ProfileUpdateMode.Modify)
                                {
                                    // 프로필 정보 갱신
                                    updateResult = DBManager.GetInstance().UpdateSASProfile(this.dataSyncProfileUpdateReqData.LstSASProfile);
                                }
                                else
                                {
                                    updateResult = DBManager.GetInstance().DeleteSASProfile(this.dataSyncProfileUpdateReqData.LstSASProfile);
                                }
                                if (updateResult == 0)
                                {
                                    if (this.NotifySASProfileUpdated != null)
                                    {
                                        this.NotifySASProfileUpdated(this, new SASProfileUpdateEventArgs(this.dataSyncProfileUpdateReqData));
                                    }
                                }

                                // 동기화 결과 통지
                                bool sendResult = CommunicationManager.GetInstance().SendResultOfSASProfileUpdate(currentReqEventID, updateResult);
                                if (!sendResult)
                                {
                                    System.Console.WriteLine("[Core] DataSyncProcessing( 프로필 갱신 결과 전송 실패 )");
                                    FileLogManager.GetInstance().WriteLog("[Core] DataSyncProcessing( 프로필 갱신 결과 전송 실패 )");
                                }

                                System.Console.WriteLine("[Core] DataSyncProcessing( 프로필 갱신 완료 )");
                                FileLogManager.GetInstance().WriteLog("[Core] DataSyncProcessing( 프로필 갱신 완료 )");
                            }
                            break;

                        default:
                            // 데이터 무효: 버림
                            // 이런 경우가 있을리 없지만, 로그는 남기자
                            break;
                    }

                    bufferedSyncData = null;
                }
            }
            catch (ThreadAbortException ex)
            {
                System.Console.WriteLine("[Core] DataSyncProcessing( Exception=[ ThreadAbortException ] )");
                FileLogManager.GetInstance().WriteLog("[Core] DataSyncProcessing( Exception=[ ThreadAbortException ] )");

                Thread.ResetAbort();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[Core] DataSyncProcessing ( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[Core] DataSyncProcessing( " + ex.ToString() + " )");

                throw new Exception("[Core] 동기화 데이터 처리 중에 예외가 발생하였습니다.");
            }
            finally
            {
                FileLogManager.GetInstance().WriteLog("[Core] DataSyncProcessing( end )");

                if (this.manualEvtSYNC != null)
                {
                    this.manualEvtSYNC.Close();
                    this.manualEvtSYNC = null;
                }
            }
        }
        /// <summary>
        /// 기상특보 데이터 처리 쓰레드 함수
        /// </summary>
        private void SWRDataProcessing()
        {
            System.Console.WriteLine("[Core] SWRDataProcessing ( 기상특보 데이터 수신 대기 쓰레드 시작 )");
            FileLogManager.GetInstance().WriteLog("[Core] SWRDataProcessing ( 기상특보 데이터 수신 대기 쓰레드 시작 )");

            try
            {
                if (this.manualEvtSWR == null)
                {
                    this.manualEvtSWR = new ManualResetEvent(false);
                }

                while (this.isSWRDataProcessingContinue)
                {
                    int count = 0;
                    List<SWRProfile> bufferedSWRList = new List<SWRProfile>();

                    lock (this.dataSWRQueue)
                    {
                        count = this.dataSWRQueue.Count;
                        for (int cnt = 0; cnt < count; cnt++)
                        {
                            SWRProfile bufferedSWR = this.dataSWRQueue.Dequeue();
                            bufferedSWRList.Add(bufferedSWR);
                        }
                    }
                    if (count <= 0)
                    {
                        this.manualEvtSWR.WaitOne();
                        this.manualEvtSWR.Reset();

                        continue;
                    }

                    // 연계설정정보 조회
                    List<SWRAssociationCondition> conditions = DBManager.GetInstance().QuerySWRAssociationCondition();
                    if (conditions == null)
                    {
                        // NULL인 경우에는 연계 안함으로 판단.
                        continue;
                    }

                    List<SWRProfile> validSWRList = new List<SWRProfile>();
                    foreach (SWRProfile profile in bufferedSWRList)
                    {
                        foreach (SWRAssociationCondition condition in conditions)
                        {
                            if (condition.IsUse &&
                                profile.WarnKindCode == condition.WarnKindCode.ToString() &&
                                profile.WarnStressCode == condition.WarnStressCode.ToString())
                            {
                                validSWRList.Add(profile);
                            }
                        }
                    }
                    if (validSWRList.Count < 1)
                    {
                        continue;
                    }

                    // 등록
                    int registResult = RegistSWR(validSWRList);

                    if (this.NotifySWRProfileUpdated != null)
                    {
                        this.NotifySWRProfileUpdated(this, new EventArgs());
                    }
                }
            }
            catch (ThreadAbortException ex)
            {
                System.Console.WriteLine("[Core] SWRDataProcessing( Exception=[ ThreadAbortException ] )");
                FileLogManager.GetInstance().WriteLog("[Core] SWRDataProcessing( Exception=[ ThreadAbortException ] )");

                Thread.ResetAbort();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[Core] SWRDataProcessing (" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[Core] SWRDataProcessing ( " + ex.ToString() + " )");

                throw new Exception("[Core] 기상특보 데이터 처리 중에 예외가 발생하였습니다.");
            }
            finally
            {
                if (this.manualEvtSWR != null)
                {
                    this.manualEvtSWR.Close();
                    this.manualEvtSWR = null;
                }

                System.Console.WriteLine("[Core] SWRDataProcessing ( 기상특보 데이터 수신 대기 쓰레드 종료 )");
                FileLogManager.GetInstance().WriteLog("[Core] SWRDataProcessing ( 기상특보 데이터 수신 대기 쓰레드 종료 )");
            }
        }

        /// <summary>
        /// [데이터 동기화] 수신 데이터의 CRC16 체크.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private DataValidationCheckResult CheckReceivedDataValidationOfDataSync(uint eventID, SynchronizationReqData data)
        {
            DataValidationCheckResult ret = DataValidationCheckResult.Success;

            // 3차년도 대응 검토.

            return ret;
        }
        /// <summary>
        /// [데이터동기화] 표준경보시스템 프로필 체크.
        /// </summary>
        /// <param name="lstSrcProfile"></param>
        /// <param name="lstUnmatchedProfileID"></param>
        /// <param name="lstDeletedProfile"></param>
        /// <returns></returns>
        private int CheckSASProfileHashKey(List<SASProfileHash> lstSrcProfile, out List<string> lstUnmatchedProfileID, out List<SASProfile> lstDeletedProfile)
        {
            int result = 0;
            lstUnmatchedProfileID = null;
            lstDeletedProfile = null;

            try
            {
                // 모든 표준경보시스템 아이디 목록 취득
                List<SASProfile> lstLocalProfiles = DBManager.GetInstance().QuerySASInfo();
                if (lstLocalProfiles == null)
                {
                    System.Console.WriteLine("[Core] CheckSASProfileHashKey( 내부 오류 )");
                    FileLogManager.GetInstance().WriteLog("[Core] CheckSASProfileHashKey ( 정보취득 오류 )");
                    return -1;
                }

                lstUnmatchedProfileID = new List<string>();

                // 로컬에만 존재하는 표준경보시스템 프로필 목록 추출
                lstDeletedProfile = new List<SASProfile>();
                for (int index = 0; index < lstLocalProfiles.Count; index++)
                {
                    string localProfileID = lstLocalProfiles[index].ID;

                    bool isFound = false;
                    foreach (SASProfileHash srcProfile in lstSrcProfile)
                    {
                        if (localProfileID == srcProfile.ProfileID)
                        {
                            isFound = true;
                            break;
                        }
                    }
                    if (isFound)
                    {
                        continue;
                    }

                    SASProfile deletedProfile = new SASProfile();
                    deletedProfile.DeepCopyFrom(lstLocalProfiles[index]);
                    lstDeletedProfile.Add(deletedProfile);

                    lstLocalProfiles.RemoveAt(index);
                    index--;
                }
                // 추출된 프로필 목록을 DB에서 삭제
                if (lstDeletedProfile.Count > 0)
                {
                    int deleteResult = DBManager.GetInstance().DeleteSASProfile(lstDeletedProfile);
                    if (deleteResult != 0)
                    {
                        FileLogManager.GetInstance().WriteLog("[Core] CheckSASProfileHashKey ( 불일치 프로필 삭제 오류 deleteResult=[" + deleteResult + "] )");
                    }
                }

                // 나머지를 비교해서 일치하지 않는 목록만 추출
                foreach (SASProfileHash srcProfile in lstSrcProfile)
                {
                    bool isMatched = false;
                    bool isFound = true;

                    foreach (SASProfile localProfile in lstLocalProfiles)
                    {
                        if (srcProfile.ProfileID == localProfile.ID)
                        {
                            isFound = true;
                            isMatched = CompareHashKey(srcProfile.HashKey, localProfile.ComputeHashKey());
                            break;
                        }
                    }
                    // 일치하는 프로필이 없거나, 프로필은 있으나 해쉬코드가 불일치의 경우
                    if (!isFound || !isMatched)
                    {
                        lstUnmatchedProfileID.Add(srcProfile.ProfileID);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[core] CheckSASProfileHashKey ( Exception Occured!!! \n" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[Core] CheckSASProfileHashKey ( " + ex.ToString() + " )");

                return -2;
            }

            return result;
        }
        /// <summary>
        /// 해쉬키 비교
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        private bool CompareHashKey(byte[] first, byte[] second)
        {
            if (first == null || second == null)
            {
                return false;
            }
            if (first.Length == 0 || first.Length != second.Length)
            {
                return false;
            }

            for (int index = 0; index < first.Length; index++)
            {
                if (first[index] != second[index])
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region 하위클래스_이벤트핸들러
        private void communicationManager_OnNotifyIAGWConnectionState(object sender, IAGWConnectionEventArgs e)
        {
            try
            {
                System.Console.WriteLine("[Core] : auth(" + e.IsAuthenticated.ToString() + "), conn(" + e.IsConnected.ToString() + ")");

                if (!e.IsConnected)
                {
                    e.IsAuthenticated = false;

                    // 재접속 처리
                    if (this.useAutoReconnectionWithIAGW && this.isConnectionStarted)
                    {
                        System.Console.WriteLine("[Core] communicationManager_OnNotifyIAGWConnectionState( 통합경보게이트웨이에 재접속 요청 )");
                        FileLogManager.GetInstance().WriteLog("[Core] communicationManager_OnNotifyIAGWConnectionState( 통합경보게이트웨이에 재접속 요청 )");

                        bool ret = ConnectToGateway();
                        if (!ret)
                        {
                            System.Console.WriteLine("[Core] communicationManager_OnNotifyIAGWConnectionState( 재접속 요청 실패 )");
                            FileLogManager.GetInstance().WriteLog("[Core] communicationManager_OnNotifyIAGWConnectionState( 재접속 요청 실패 )");
                        }
                    }
                }
                else if (!e.IsAuthenticated)
                {
                    System.Console.WriteLine("[Core] communicationManager_OnNotifyIAGWConnectionState( 통합경보게이트웨이에 인증 요청 )");
                    FileLogManager.GetInstance().WriteLog("[Core] communicationManager_OnNotifyIAGWConnectionState( 통합경보게이트웨이에 인증 요청 )");

                    // 인증 요청 해야하는데 세션에서 올라온 콜이 물려있다. 일단 이렇게 가고 1차 출시 후에 수정
                    bool ret = CommunicationManager.GetInstance().RequestAuth(ConfigManager.GetInstance().ConfigInfo.IAGW.AuthCode);
                    if (!ret)
                    {
                        System.Console.WriteLine("[Core] OnNotifyIAGWConnectionState( 인증 요청 실패 )");
                        FileLogManager.GetInstance().WriteLog("[Core] communicationManager_OnNotifyIAGWConnectionState( 인증 요청 실패 )");
                    }
                }
                else
                {
                    // 연결/인증 성공
                }

                if (this.NotifyIAGWConnectionState != null)
                {
                    this.NotifyIAGWConnectionState(this, e);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[core] communicationManager_OnNotifyIAGWConnectionState ( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[Core] communicationManager_OnNotifyIAGWConnectionState( " + ex.ToString() + " )");
            }
        }
        private void communicationManager_OnCAPReceived(object sender, CapEventArgs e)
        {
            System.Console.WriteLine("[Core] communicationManager_OnCAPReceived( CAP 데이터 수신 )");
            FileLogManager.GetInstance().WriteLog("[Core] communicationManager_OnCAPReceived( CAP 데이터 수신 )");

            try
            {

                System.Diagnostics.Debug.Assert(this.dataCAPQueue != null);
                System.Diagnostics.Debug.Assert(this.manualEvtCAP != null);

                if (this.dataCAPQueue != null)
                {
                    this.dataCAPQueue.Enqueue(e.Data);
                    if (this.manualEvtCAP != null)
                    {
                        this.manualEvtCAP.Set();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[Core] communicationManager_OnCAPReceived ( Exception Occured : " + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[Core] communicationManager_OnCAPReceived( " + ex.ToString() + " )");
            }
        }
        private void communicationManager_OnNotifyDataSyncRequested(object sender, DataSyncEvtArgs e)
        {
            System.Console.WriteLine("[Core] : communicationManager_OnNotifyDataSyncRequested( 데이터 동기화 요청 수신 )");
            FileLogManager.GetInstance().WriteLog("[Core] communicationManager_OnNotifyDataSyncRequested( 데이터 동기화 요청 수신 )");

            try
            {
                System.Diagnostics.Debug.Assert(this.dataSyncInfoQueue != null);
                System.Diagnostics.Debug.Assert(this.manualEvtSYNC != null);

                if (this.dataSyncInfoQueue != null)
                {
                    this.dataSyncInfoQueue.Enqueue(e.Data);
                    if (this.manualEvtSYNC != null)
                    {
                        this.manualEvtSYNC.Set();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[Core] communicationManager_OnNotifyDataSyncRequested ( Exception Occured : " + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[Core] communicationManager_OnNotifyDataSyncRequested( " + ex.ToString() + " )");
            }
        }
        private void swrServicemanager_OnNotifySWRReceived(object sender, SWREventArgs e)
        {
            System.Console.WriteLine("[Core] 기상특보 데이터 수신");
            FileLogManager.GetInstance().WriteLog("[Core] swrServicemanager_OnNotifySWRReceived( 기상 특보 데이터 수신 )");

            System.Diagnostics.Debug.Assert(e.ReportList != null);

            try
            {
                if (this.dataSWRQueue != null)
                {
                    foreach (SWRProfile info in e.ReportList)
                    {
                        if (info == null)
                        {
                            continue;
                        }

                        this.dataSWRQueue.Enqueue(info);
                    }
                    if (this.manualEvtSWR != null)
                    {
                        this.manualEvtSWR.Set();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[Core] swrServicemanager_OnNotifySWRReceived ( Exception Occured : " + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[Core] swrServicemanager_OnNotifySWRReceived( " + ex.ToString() + " )");
            }
        }
        #endregion
    }

}