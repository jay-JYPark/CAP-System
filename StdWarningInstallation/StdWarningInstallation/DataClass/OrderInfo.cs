using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CAPLib;

namespace StdWarningInstallation.DataClass
{
    public class OrderProvisionInfo
    {
        private OrderMode mode;
        public OrderMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }
        private MsgType messageType;
        public MsgType MessageType
        {
            get { return messageType; }
            set { messageType = value; }
        }
        private ScopeType scope;
        public ScopeType Scope
        {
            get { return scope; }
            set { scope = value; }
        }

        private Disaster disaster;
        public Disaster Disaster
        {
            get { return disaster; }
            set { disaster = value; }
        }
        private OrderReferenceType refType;
        public OrderReferenceType RefType
        {
            get { return refType; }
            set { refType = value; }
        }
        private string refRecordID;
        public string RefRecordID
        {
            get { return refRecordID; }
            set { refRecordID = value; }
        }
        private SendingMsgTextInfo msgTextInfo;
        public SendingMsgTextInfo MsgTextInfo
        {
            get { return msgTextInfo; }
            set { msgTextInfo = value; }
        }

        private List<RegionDefinition> targetRegions;
        public List<RegionDefinition> TargetRegions
        {
            get { return targetRegions; }
            set { targetRegions = value; }
        }
        private List<SASProfile> targetSystems;
        public List<SASProfile> TargetSystems
        {
            get { return targetSystems; }
            set { targetSystems = value; }
        }
        private List<AdengGE.CircleInfo> circle;
        public List<AdengGE.CircleInfo> Circle
        {
            get { return circle; }
            set { circle = value; }
        }

        private OrderLocationKind orderLocationKind;
        public OrderLocationKind LocationKind
        {
            get { return orderLocationKind; }
            set { orderLocationKind = value; }
        }

        private List<SASKind> targetSystemsKinds;
        public List<SASKind> TargetSystemsKinds
        {
            get { return targetSystemsKinds; }
            set { targetSystemsKinds = value; }
        }
        private ClearAlertState clearAlertState;
        public ClearAlertState ClearAlertState
        {
            get { return clearAlertState; }
            set { clearAlertState = value; }
        }
        private CAP capInfo = null;
        public CAP CAPData
        {
            get { return capInfo; }
            set { capInfo = value; }
        }
        private object tag;
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        public OrderRecord ToOrderRecord()
        {
            OrderRecord recordInfo = new OrderRecord();

            try
            {
                recordInfo.CAPID = this.CAPData.MessageID;
                recordInfo.OrderedTime = this.CAPData.SentDateTime;
                recordInfo.RefType = this.RefType;
                recordInfo.RefRecordID = this.RefRecordID;
                recordInfo.LocationKind = this.LocationKind;

                if (this.Mode != null)
                {
                    recordInfo.OrderMode = this.Mode.Code;
                }
                else
                {
                    // [2016-03-31] 기본 발령 정보를 시험으로 생성 - by Gonzi
                    //recordInfo.OrderMode = StatusType.Actual;
                    recordInfo.OrderMode = StatusType.Test;
                }

                if (this.Disaster != null && this.Disaster.Kind != null && !string.IsNullOrEmpty(this.Disaster.Kind.Code))
                {
                    recordInfo.DisasterKindCode = this.Disaster.Kind.Code;
                }
                else
                {
                    recordInfo.DisasterKindCode = string.Empty;
                }

                recordInfo.ClearState = new AlertingClearState();
                recordInfo.ClearState.DeepCopyFrom(BasisData.AlertingClearStateInfo[this.clearAlertState]);

                if (this.CAPData != null)
                {
                    recordInfo.CapText = this.CAPData.WriteToXML();
                }
                else
                {
                    recordInfo.CapText = string.Empty;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[OrderInfo] ToOrderRecord( Exception=[" + ex.ToString() + "] )");
                FileLogManager.GetInstance().WriteLog("[OrderInfo] ToOrderRecord ( Exception=[" + ex.ToString() + "] )");

                return null;
            }

            return recordInfo;
        }

        public OrderProvisionInfo()
        {
            // [2016-03-31] 기본 발령 정보를 시험으로 생성 - by Gonzi
            //this.mode = BasisData.FindOrderModeInfoByCode(CAPLib.StatusType.Actual);
            this.mode = BasisData.FindOrderModeInfoByCode(CAPLib.StatusType.Test);
            this.messageType = MsgType.Alert;
            this.scope = ScopeType.Public;

            this.disaster = new Disaster();
            this.disaster.Category = new DisasterCategory();
            this.disaster.Kind = new DisasterKind();

            this.refType = new OrderReferenceType();
            this.refRecordID = string.Empty;
            this.msgTextInfo = new SendingMsgTextInfo();

            this.targetRegions = new List<RegionDefinition>();
            this.targetSystems = new List<SASProfile>();
            this.circle = new List<AdengGE.CircleInfo>();

            this.orderLocationKind = OrderLocationKind.Local;

            this.targetSystemsKinds = new List<SASKind>();

            this.tag = null;
        }
    }

    public class OrderRecord
    {
        private string capID;
        public string CAPID
        {
            get { return capID; }
            set { capID = value; }
        }
        private DateTime orderedTime;
        public DateTime OrderedTime
        {
            get { return orderedTime; }
            set { orderedTime = value; }
        }
        private StatusType orderMode;
        public StatusType OrderMode
        {
            get { return orderMode; }
            set { orderMode = value; }
        }
        private OrderLocationKind locationKind;
        public OrderLocationKind LocationKind
        {
            get { return locationKind; }
            set { locationKind = value; }
        }
        private OrderReferenceType refType;
        public OrderReferenceType RefType
        {
            get { return refType; }
            set { refType = value; }
        }
        private string refRecordID;
        public string RefRecordID
        {
            get { return refRecordID; }
            set { refRecordID = value; }
        }
        private string disasterKindCode;
        public string DisasterKindCode
        {
            get { return disasterKindCode; }
            set { disasterKindCode = value; }
        }
        private AlertingClearState clearState;
        public AlertingClearState ClearState
        {
            get { return clearState; }
            set { clearState = value; }
        }
        private string capText;
        public string CapText
        {
            get { return capText; }
            set { capText = value; }
        }

        public void DeepCopyFrom(OrderRecord src)
        {
            System.Diagnostics.Debug.Assert(src != null);

            this.capID = src.capID;
            this.orderedTime = src.orderedTime;
            this.orderMode = src.OrderMode;
            this.locationKind = src.locationKind;
            this.refType = src.refType;
            this.refRecordID = src.refRecordID;
            this.disasterKindCode = src.disasterKindCode;
            this.clearState = src.clearState;
            this.capText = src.capText;
        }
        public string GetHeadlineString()
        {
            if (this == null)
            {
                return string.Empty;
            }

            try
            {
                CAPHelper helper = new CAPHelper();
                StringBuilder headline = new StringBuilder();
                string association = string.Empty;

                if (this.LocationKind == OrderLocationKind.Other)
                {
                    association = "[연계] ";
                }

                CAP msg = new CAP(this.CapText);
                string headlineNormal = helper.MakeHeadline(msg);
                if (string.IsNullOrEmpty(headlineNormal))
                {
                    headline.Append(headlineNormal);
                }

                if (headline.Length < 1)
                {
                    OrderMode mode = BasisData.FindOrderModeInfoByCode(this.OrderMode);
                    if (mode == null)
                    {
                        headline.Append(this.OrderedTime + " [ Unknown ]");
                    }
                    else
                    {
                        headline.Append(this.OrderedTime + " [" + mode.Name + "]");
                    }

                    string targets = helper.ExtractTargetNamesFromCAP(msg);
                    if (!string.IsNullOrEmpty(targets))
                    {
                        headline.Append("[" + targets.Replace(',', '/') + "] 대상으로");
                    }
                    else
                    {
                        headline.Append("[ Unknown ] 대상으로");
                    }

                    string disasterName = "Unknown";
                    if (msg.Info != null && msg.Info.Count > 0)
                    {
                        disasterName = msg.Info[0].Event;
                    }
                    else
                    {
                        DisasterKind disasterKind = BasisData.FindDisasterKindByCode(this.DisasterKindCode);
                        if (disasterKind != null)
                        {
                            disasterName = disasterKind.Name;
                        }
                    }

                    headline.Append(" [" + disasterName + "]");
                    headline.Append(" 발령");

                    if (this.RefType == OrderReferenceType.Cancel)
                    {
                        headline.Append(" 취소");
                    }
                }

                return (association + headline.ToString());
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[OrderInfo] GetHeadlineString( Exception=[" + ex.ToString() + "] )");
                FileLogManager.GetInstance().WriteLog("[OrderInfo] GetHeadlineString ( Exception=[" + ex.ToString() + "] )");

                return string.Empty;
            }
        }

        public OrderRecord()
        {

        }
    }

    public class OrderMode
    {
        #region proerty
        private StatusType modeCode;
        public StatusType Code
        {
          get { return modeCode; }
          set { modeCode = value; }
        }

        private string modeString;
        public string Name
        {
            get { return modeString; }
            set { modeString = value; }
        }

        public void DeepCopyFrom(OrderMode src)
        {
            System.Diagnostics.Debug.Assert(src != null);

            this.modeCode = src.Code;
            this.modeString = src.Name;
        }
        public override string ToString()
        {
            return modeString;
        }
        public OrderMode()
        {
        }
        public OrderMode(StatusType code, string name)
        {
            this.modeCode = code;
            this.modeString = name;
        }
        #endregion
    }

    public class ReceivedCAPInfo
    {
        private CAP msg;
        public CAP Msg
        {
            get { return msg; }
        }
        private SenderTypes msgSenderType;
        public SenderTypes MsgSenderType
        {
            get { return msgSenderType; }
        }

        public ReceivedCAPInfo()
        {
        }
        public ReceivedCAPInfo(SenderTypes senderType, CAP msg)
        {
            this.msg = msg;
            this.msgSenderType = senderType;
        }
    }

    public class AlertingClearState
    {
        private ClearAlertState code;
        public ClearAlertState Code
        {
            get { return code; }
            set { code = value; }
        }
        private string description = string.Empty;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public void DeepCopyFrom(AlertingClearState src)
        {
            this.code = src.code;
            this.description = src.description;
        }

        public AlertingClearState()
        {
        }
        public AlertingClearState(ClearAlertState code, string description)
        {
            this.code = code;
            this.description = description;
        }
    }

    public class OrderLocationKindInfo
    {
        private OrderLocationKind code;
        public OrderLocationKind Code
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

        public void DeepCopyFrom(OrderLocationKindInfo src)
        {
            System.Diagnostics.Debug.Assert(src != null);

            this.code = src.Code;
            this.description = src.description;
        }
        public override string ToString()
        {
            return description;
        }
        public OrderLocationKindInfo()
        {
            this.code = OrderLocationKind.Unknown;
            this.description = OrderLocationKind.Unknown.ToString();
        }
        public OrderLocationKindInfo(OrderLocationKind code, string description)
        {
            this.code = code;
            this.description = description;
        }
    }

    public class OrderResponseProfile
    {
        /// <summary>
        /// 응답 캡 아이디(현재 캡)
        /// </summary>
        private string responseCapID;
        public string ID
        {
            get { return responseCapID; }
            set { responseCapID = value; }
        }
        /// <summary>
        /// 원발령 캡 아이디
        /// </summary>
        private string referenceCapID;
        public string ReferenceCapID
        {
            get { return referenceCapID; }
            set { referenceCapID = value; }
        }
        /// <summary>
        /// 송신자 아이디
        /// </summary>
        private string senderID;
        public string SenderID
        {
            get { return senderID; }
            set { senderID = value; }
        }
        /// <summary>
        /// 송신자 종류(통합경보게이트웨이, 표준경보시스템, 표준발령대)
        /// </summary>
        private SenderTypes senderType;
        public SenderTypes SenderType
        {
            get { return senderType; }
            set { senderType = value; }
        }
        /// <summary>
        /// 응답 캡 원문(현재 캡)
        /// </summary>
        private string responseCapMsg;
        public string CapMsg
        {
            get { return responseCapMsg; }
            set { responseCapMsg = value; }
        }
        /// <summary>
        /// 시스템 관리자 이름
        /// </summary>
        private string systemManagerName;
        public string SystemManagerName
        {
            get { return systemManagerName; }
            set { systemManagerName = value; }
        }
        /// <summary>
        /// 시스템 관리자 소속(부서 등)
        /// </summary>
        private string systemManagerDepartment;
        public string SystemManagerDepartment
        {
            get { return systemManagerDepartment; }
            set { systemManagerDepartment = value; }
        }
        /// <summary>
        /// 시스템 관리자 연락처
        /// </summary>
        private string systemManagerPhone;
        public string SystemManagerPhone
        {
            get { return systemManagerPhone; }
            set { systemManagerPhone = value; }
        }
        /// <summary>
        /// 응답 수신 시각.
        /// 초기값은 등록 시각.
        /// </summary>
        private DateTime recvTime;
        public DateTime RecvTime
        {
            get { return recvTime; }
            set { recvTime = value; }
        }

    }

    public class SystemManager
    {
        /// <summary>
        /// 시스템 관리자 이름
        /// </summary>
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// 시스템 관리자 소속(부서 등)
        /// </summary>
        private string department;
        public string Department
        {
            get { return department; }
            set { department = value; }
        }
        /// <summary>
        /// 시스템 관리자 연락처
        /// </summary>
        private string phone;
        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public void DeepCopyFrom(SystemManager src)
        {
            System.Diagnostics.Debug.Assert(src != null);

            this.name = src.name;
            this.department = src.department;
            this.phone = src.phone;
        }
    }

    /// <summary>
    /// 전송문안
    /// </summary>
    public class SendingMsgTextInfo
    {
        private List<MsgTextDisplayLanguageKind> selectedLanguages;
        public List<MsgTextDisplayLanguageKind> SelectedLanguages
        {
            get { return selectedLanguages; }
            set { selectedLanguages = value; }
        }
        private MsgTextCityType selectedCityType;
        public MsgTextCityType SelectedCityType
        {
            get { return selectedCityType; }
            set { selectedCityType = value; }
        }
        private List<MsgText> originalTransmitMsgText;
        public List<MsgText> OriginalTransmitMsgText
        {
            get { return originalTransmitMsgText; }
            set { originalTransmitMsgText = value; }
        }
        private List<MsgText> currentTransmitMsgText;
        public List<MsgText> CurrentTransmitMsgText
        {
            get { return currentTransmitMsgText; }
            set { currentTransmitMsgText = value; }
        }

        public void DeepCopyFrom(SendingMsgTextInfo src)
        {
            System.Diagnostics.Debug.Assert(src != null);

            // 다국어
            if (src.selectedLanguages == null)
            {
                this.selectedLanguages = null;
            }
            else
            {
                if (this.selectedLanguages == null)
                {
                    this.selectedLanguages = new List<MsgTextDisplayLanguageKind>();
                }
                this.selectedLanguages.Clear();

                foreach (MsgTextDisplayLanguageKind lang in src.selectedLanguages)
                {
                    MsgTextDisplayLanguageKind copy = new MsgTextDisplayLanguageKind();
                    copy.DeepCopyFrom(lang);
                    this.selectedLanguages.Add(copy);
                }
            }

            // 도시유형별
            if (src.selectedCityType == null)
            {
                this.selectedCityType = null;
            }
            else
            {
                if (this.selectedCityType == null)
                {
                    this.selectedCityType = new MsgTextCityType();
                }
                this.selectedCityType.DeepCopyFrom(src.selectedCityType);
            }

            // 원본 전송 문안
            if (src.originalTransmitMsgText == null)
            {
                this.originalTransmitMsgText = null;
            }
            else
            {
                if (this.originalTransmitMsgText == null)
                {
                    this.originalTransmitMsgText = new List<MsgText>();
                }
                this.originalTransmitMsgText.Clear();

                foreach (MsgText lang in src.originalTransmitMsgText)
                {
                    MsgText copy = new MsgText();
                    copy.DeepCopyFrom(lang);
                    this.originalTransmitMsgText.Add(copy);
                }
            }

            // 현재 전송 문안(런타임 편집/가공)
            if (src.currentTransmitMsgText == null)
            {
                this.currentTransmitMsgText = null;
            }
            else
            {
                if (this.currentTransmitMsgText == null)
                {
                    this.currentTransmitMsgText = new List<MsgText>();
                }
                this.currentTransmitMsgText.Clear();

                foreach (MsgText lang in src.currentTransmitMsgText)
                {
                    MsgText copy = new MsgText();
                    copy.DeepCopyFrom(lang);
                    this.currentTransmitMsgText.Add(copy);
                }
            }
        }

        public void Initialize()
        {
            this.selectedLanguages = new List<MsgTextDisplayLanguageKind>();
            this.selectedCityType = new MsgTextCityType();
            this.originalTransmitMsgText = new List<MsgText>();
            this.currentTransmitMsgText = new List<MsgText>();
        }

        public SendingMsgTextInfo()
        {
        }
    }
}
