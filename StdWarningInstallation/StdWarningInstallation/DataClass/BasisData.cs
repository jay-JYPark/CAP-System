using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CAPLib;

namespace StdWarningInstallation.DataClass
{
    public class BasisData
    {
        readonly public static string DEFAULT_LANGUAGECODE = "ko-KR";
        readonly public static string DEFAULT_CITYTYPECODE = "GENERAL";
        readonly public static string[] KEYWORD_TIMES = { "현재시각",
                                    "현재 시각",
                                    "현재시간",
                                    "현재 시간",
                                    "○○일 ○○시",
                                    "○○일○○시",
                                    };
        readonly public static string[] KEYWORD_REGIONS = { "이 지역",
                                        "이지역",
                                        "우리 지역",
                                        "우리지역",
                                        "○○지역",
                                        "○○ 지역",
                                      };

        #region FIXED_INFO
        private static Dictionary<StatusType, OrderMode> orderModeInfo = InitializeOrderMode();
        public static Dictionary<StatusType, OrderMode> OrderModeInfo
        {
            get { return orderModeInfo; }
            set { orderModeInfo = value; }
        }
        private static List<DisasterInfo> disasterInfo = new List<DisasterInfo>();
        public static List<DisasterInfo> Disasters
        {
            get { return disasterInfo; }
            set { disasterInfo = value; }
        }
        public static void SetDisasterInfo(List<DisasterInfo> info)
        {
            disasterInfo = info;
        }
        private static RegionInfo regionInfo = new RegionInfo();
        public static RegionInfo Regions
        {
            get { return regionInfo; }
            set { regionInfo = value; }
        }
        public static RegionDefinition topRegion = new RegionDefinition();
        public static RegionDefinition TopRegion
        {
            get { return topRegion; }
            set { topRegion = value; }
        }

        private static List<MsgTextDisplayLanguageKind> msgTextLanguageKind = new List<MsgTextDisplayLanguageKind>();
        public static List<MsgTextDisplayLanguageKind> MsgTextLanguageKind
        {
            get { return msgTextLanguageKind; }
            set { msgTextLanguageKind = value; }
        }
        private static List<MsgTextDisplayMediaType> msgTextDisplayMediaType = new List<MsgTextDisplayMediaType>();
        public static List<MsgTextDisplayMediaType> MsgTextDisplayMediaType
        {
            get { return msgTextDisplayMediaType; }
            set { msgTextDisplayMediaType = value; }
        }
        private static List<MsgTextCityType> msgTextCityType = new List<MsgTextCityType>();
        public static List<MsgTextCityType> MsgTextCityType
        {
            get { return msgTextCityType; }
            set { msgTextCityType = value; }
        }

        private static Dictionary<string, DisasterMsgText> basicMsgTextInfo = new Dictionary<string, DisasterMsgText>();
        public static Dictionary<string, DisasterMsgText> BasicMsgTextInfo
        {
            get { return BasisData.basicMsgTextInfo; }
            set { BasisData.basicMsgTextInfo = value; }
        }
        private static Dictionary<string, DisasterMsgText> transmitMsgTextInfo = new Dictionary<string, DisasterMsgText>();
        public static Dictionary<string, DisasterMsgText> TransmitMsgTextInfo
        {
            get { return BasisData.transmitMsgTextInfo; }
            set { BasisData.transmitMsgTextInfo = value; }
        }

        private static Dictionary<string, SASKind> stdAlertSystemKindInfo = new Dictionary<string,SASKind>();
        public static Dictionary<string, SASKind> SASKindInfo
        {
            get { return stdAlertSystemKindInfo; }
            set { stdAlertSystemKindInfo = value; }
        }

        #region 기상특보
        private static Dictionary<string, string> swrKindInfo = new Dictionary<string, string>();
        public static Dictionary<string, string> SwrKindInfo
        {
            get { return swrKindInfo; }
            set { swrKindInfo = value; }
        }

        private static Dictionary<string, string> swrStressInfo = new Dictionary<string, string>();
        public static Dictionary<string, string> SwrStressInfo
        {
            get { return swrStressInfo; }
            set { swrStressInfo = value; }
        }

        private static Dictionary<string, string> swrCommandInfo = new Dictionary<string, string>();
        public static Dictionary<string, string> SwrCommandInfo
        {
            get { return swrCommandInfo; }
            set { swrCommandInfo = value; }
        }

        private static Dictionary<string, SWRAnnounceArea> swrAreaInfo = new Dictionary<string, SWRAnnounceArea>();
        public static Dictionary<string, SWRAnnounceArea> SwrAreaInfo
        {
            get { return BasisData.swrAreaInfo; }
            set { BasisData.swrAreaInfo = value; }
        }

        private static List<SWRDisasterMatching> swrDisasterMatchingInfo = new List<SWRDisasterMatching>();
        public static List<SWRDisasterMatching> SwrDisasterMatchingInfo
        {
            get { return swrDisasterMatchingInfo; }
            set { swrDisasterMatchingInfo = value; }
        }
        #endregion

        private static Dictionary<ClearAlertState, AlertingClearState> alertingClearStateInfo = InitializeAlertingClearState();
        public static Dictionary<ClearAlertState, AlertingClearState> AlertingClearStateInfo
        {
            get { return alertingClearStateInfo; }
            set { alertingClearStateInfo = value; }
        }

        private static Dictionary<OrderLocationKind, OrderLocationKindInfo> orderLocationKindInfo = InitializeOrderLocationKind();
        public static Dictionary<OrderLocationKind, OrderLocationKindInfo> OrderLocationType
        {
            get { return orderLocationKindInfo; }
            set { orderLocationKindInfo = value; }
        }
        #endregion

        #region NONFIXED_DATA
        // 고정 데이터가 아니긴 하지만, 시스템 로딩 시점에 단 한 번 갱신되는 값

        /// <summary>
        /// 시스템에 로그인한 사용자 정보.
        /// 로그인에 실패한 경우, 디폴트 유저 정보 설정.
        /// </summary>
        private static UserAccount currentLoginUser = new UserAccount();
        public static UserAccount CurrentLoginUser
        {
            get { return currentLoginUser; }
            set { currentLoginUser = value; }
        }
        /// <summary>
        /// 발령자 정보.
        /// CAP 메시지에 담겨 전송되는 정보이다.
        /// </summary>
        private static ConfigSenderInfo systemSenderInfo = new ConfigSenderInfo();
        public static ConfigSenderInfo SystemSenderInfo
        {
            get { return BasisData.systemSenderInfo; }
            set { BasisData.systemSenderInfo = value; }
        }
        #endregion


        #region DataSet
        private static Dictionary<StatusType, OrderMode> InitializeOrderMode()
        {
            Dictionary<StatusType, OrderMode> result = new Dictionary<StatusType, OrderMode>();

            result.Add(StatusType.Actual, new OrderMode(StatusType.Actual, "실제"));
            result.Add(StatusType.Exercise, new OrderMode(StatusType.Exercise, "훈련"));
            result.Add(StatusType.Test, new OrderMode(StatusType.Test, "시험"));
            // 이하, 미사용

            return result;
        }
        private static Dictionary<ClearAlertState, AlertingClearState> InitializeAlertingClearState()
        {
            Dictionary<ClearAlertState, AlertingClearState> result = new Dictionary<ClearAlertState, AlertingClearState>();
            result.Add(ClearAlertState.Waiting, new AlertingClearState(ClearAlertState.Waiting, "해제대기"));
            result.Add(ClearAlertState.Clear, new AlertingClearState(ClearAlertState.Clear, "해제완료"));
            result.Add(ClearAlertState.Exclude, new AlertingClearState(ClearAlertState.Exclude, "해제제외"));

            return result;
        }
        private static Dictionary<OrderLocationKind, OrderLocationKindInfo> InitializeOrderLocationKind()
        {
            Dictionary<OrderLocationKind, OrderLocationKindInfo> result = new Dictionary<OrderLocationKind, OrderLocationKindInfo>();
            result.Add(OrderLocationKind.Local, new OrderLocationKindInfo(OrderLocationKind.Local, "자체발령"));
            result.Add(OrderLocationKind.Other, new OrderLocationKindInfo(OrderLocationKind.Other, "타지역발령"));

            return result;
        }
        #endregion

        #region 검색 기능
        public static bool IsRegionDataLoaded()
        {
            if (Regions == null || Regions.LstRegion == null || Regions.LstRegion.Count() < 1)
            {
                return false;
            }
            return true;
        }
        public static bool IsTransmitMsgTextLoaded()
        {
            if (transmitMsgTextInfo == null || transmitMsgTextInfo.Count < 1)
            {
                return false;
            }
            return true;
        }

        public static RegionProfile FindRegion(string regionCode)
        {
            if (string.IsNullOrEmpty(regionCode))
            {
                return null;
            }
            if (!IsRegionDataLoaded())
            {
                return null;
            }

            RegionProfile profile = null;
            try
            {
                if (Regions.LstRegion.ContainsKey(regionCode))
                {
                    profile = Regions.LstRegion[regionCode];
                }
                else
                {
                    bool isFound = false;
                    bool check2 = true;
                    foreach (RegionProfile region1 in Regions.LstRegion.Values)
                    {
                        if (region1.LstSubRegion == null || region1.LstSubRegion.Count <= 0)
                        {
                            continue;
                        }
                        if (check2)
                        {
                            check2 = false;
                            isFound = region1.LstSubRegion.ContainsKey(regionCode); // 시군구
                            if (isFound)
                            {
                                profile = region1.LstSubRegion[regionCode];
                                break;
                            }
                        }

                        foreach (RegionProfile region2 in region1.LstSubRegion.Values)
                        {
                            if (region2.LstSubRegion == null || region2.LstSubRegion.Count <= 0)
                            {
                                continue;
                            }
                            isFound = region2.LstSubRegion.ContainsKey(regionCode);
                            if (isFound)
                            {
                                profile = region2.LstSubRegion[regionCode];
                                break;
                            }
                        }

                        if (isFound)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[BasisData] FindRegion ( Exception: " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[BasisData] FindRegion ( Exception: " + ex.ToString() + " )");
            }

            return profile;
        }

        /// <summary>
        /// [문안]
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static List<MsgText> FindMsgTextInfoByDisasterCode(string disasterKindCode)
        {
            if (transmitMsgTextInfo == null || transmitMsgTextInfo.Values == null)
            {
                return null;
            }

            List<MsgText> result = new List<MsgText>();

            foreach (DisasterMsgText kind in transmitMsgTextInfo.Values)
            {
                if (kind == null || kind.Disaster == null || kind.Disaster.Kind == null)
                {
                    continue;
                }
                if (kind.Disaster.Kind.Code == disasterKindCode)
                {
                    MsgText copy = new MsgText();
                    copy.DeepCopyFrom(kind.MsgTxt);
                    result.Add(copy);
                }
            }
            return result;
        }
        /// <summary>
        /// [문안 언어 종류]
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static MsgTextDisplayLanguageKind FindMsgTextLanguageInfoByID(int identifier)
        {
            foreach (MsgTextDisplayLanguageKind kind in msgTextLanguageKind)
            {
                if (kind.ID == identifier)
                {
                    MsgTextDisplayLanguageKind copy = new MsgTextDisplayLanguageKind();
                    copy.DeepCopyFrom(kind);

                    return copy;
                }
            }
            return null;
        }
        /// <summary>
        /// [문안 언어 종류]
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static MsgTextDisplayLanguageKind FindMsgTextLanguageInfoByCode(string languageCode)
        {
            foreach (MsgTextDisplayLanguageKind kind in msgTextLanguageKind)
            {
                if (kind.LanguageCode == languageCode)
                {
                    MsgTextDisplayLanguageKind copy = new MsgTextDisplayLanguageKind();
                    copy.DeepCopyFrom(kind);

                    return copy;
                }
            }
            return null;
        }
        /// <summary>
        /// [문안 표출 유형]
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static MsgTextDisplayMediaType FindMediaTypeInfoByID(int identifier)
        {
            foreach (MsgTextDisplayMediaType media in msgTextDisplayMediaType)
            {
                if (media.ID == identifier)
                {
                    MsgTextDisplayMediaType copy = new MsgTextDisplayMediaType();
                    copy.DeepCopyFrom(media);
                    return copy;
                }
            }
            return null;
        }
        /// <summary>
        /// [도시 유형]
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static MsgTextCityType FindCityTypeInfoByCode(string code)
        {
            foreach (MsgTextCityType cityType in msgTextCityType)
            {
                if (cityType.TypeCode == code)
                {
                    MsgTextCityType copy = new MsgTextCityType();
                    copy.DeepCopyFrom(cityType);

                    return copy;
                }
            }
            return null;
        }
        /// <summary>
        /// [발령 모드]
        /// </summary>
        /// <param name="orderCode"></param>
        /// <returns></returns>
        public static OrderMode FindOrderModeInfoByCode(CAPLib.StatusType orderMode)
        {
            if (orderModeInfo.ContainsKey(orderMode))
            {
                OrderMode copy = new OrderMode();
                copy.DeepCopyFrom(orderModeInfo[orderMode]);
                return copy;
            }

            return null;
        }
        public static string GetDisplayStringOrderMode(CAPLib.StatusType orderMode)
        {
            if (!orderModeInfo.ContainsKey(orderMode))
            {
                return "Unknown(" + orderMode.ToString() + ")";
            }
            return orderModeInfo[orderMode].Name;
        }
        public static OrderLocationKindInfo FindOrderLocationKindByKindCode(OrderLocationKind locationKind)
        {
            if (!orderLocationKindInfo.ContainsKey(locationKind))
            {
                OrderLocationKindInfo unknown = new OrderLocationKindInfo();
                unknown.Code = OrderLocationKind.Unknown;
                unknown.Description = OrderLocationKind.Unknown.ToString();
                return unknown;
            }

            OrderLocationKindInfo copy = new OrderLocationKindInfo();
            copy.DeepCopyFrom(orderLocationKindInfo[locationKind]);
            return copy;
        }
        public static string GetDisplayStringLocationKindName(OrderLocationKind locationKind)
        {
            if (!orderLocationKindInfo.ContainsKey(locationKind))
            {
                return "Unknown(" + locationKind.ToString() + ")";
            }
            return orderLocationKindInfo[locationKind].Description;
        }

        /// <summary>
        /// [재난 종류]
        /// </summary>
        /// <param name="warnKindCode"></param>
        /// <returns></returns>
        public static DisasterKind FindDisasterKindByCode(string kindCode)
        {
            DisasterKind result = null;

            if (disasterInfo == null)
            {
                return null;
            }
            foreach (DisasterInfo info in disasterInfo)
            {
                if (info == null || info.KindList == null)
                {
                    continue;
                }
                foreach (DisasterKind kind in info.KindList)
                {
                    if (kind.Code == kindCode)
                    {
                        DisasterKind copy = new DisasterKind();
                        copy.DeepCopyFrom(kind);
                        return copy;
                    }
                }
            }

            return result;
        }
        public static string GetDisplayStringDisasterKindName(string disasterKindCode)
        {
            System.Diagnostics.Debug.Assert(disasterKindCode != null);

            DisasterKind kind = BasisData.FindDisasterKindByCode(disasterKindCode);
            if (kind == null || kind.Name == null)
            {
                return "Unknown(" + disasterKindCode + ")";
            }
            return kind.Name;
        }
        /// <summary>
        /// [재난 카테고리 종류]
        /// </summary>
        /// <param name="warnKindCode"></param>
        /// <returns></returns>
        public static DisasterCategory FindDisasterCategoryByCode(string categoryCode)
        {
            DisasterCategory result = null;

            if (disasterInfo == null)
            {
                return null;
            }
            foreach (DisasterInfo info in disasterInfo)
            {
                if (info == null || info.Category == null)
                {
                    continue;
                }
                if (info.Category.Code == categoryCode)
                {
                    DisasterCategory copy = new DisasterCategory();
                    copy.DeepCopyFrom(info.Category);
                    return copy;
                }
            }

            return result;
        }
        /// <summary>
        /// [재난 카테고리 종류]
        /// </summary>
        /// <param name="warnKindCode"></param>
        /// <returns></returns>
        public static DisasterCategory FindDisasterCategoryByID(int categoryID)
        {
            DisasterCategory result = null;

            if (disasterInfo == null)
            {
                return null;
            }
            foreach (DisasterInfo info in disasterInfo)
            {
                if (info == null || info.Category == null)
                {
                    continue;
                }
                if (info.Category.ID == categoryID)
                {
                    DisasterCategory copy = new DisasterCategory();
                    copy.DeepCopyFrom(info.Category);
                    return copy;
                }
            }

            return result;
        }
        /// <summary>
        /// [재난 카테고리] 입력 카테고리 코드로 해당 카테고리에 대한 정보(카테고리 + 종류 정보 목록)
        /// </summary>
        /// <param name="searchCategoryCode"></param>
        /// <returns></returns>
        public static DisasterInfo FindDisasterInfoByCategoryCode(string searchCategoryCode)
        {
            DisasterInfo result = null;

            if (searchCategoryCode == null)
            {
                return null;
            }

            // 재난 정보가 준비되어 있는지 체크.
            if (disasterInfo == null || disasterInfo.Count <= 0)
            {
                return null;
            }

            foreach (DisasterInfo info in disasterInfo)
            {
                if (info.KindList == null)
                {
                    continue;
                }
                if (info.Category == null || info.Category.Code != searchCategoryCode)
                {
                    continue;
                }

                DisasterInfo copy = new DisasterInfo();
                copy.DeepCopyFrom(info);
                return copy;
            }

            return result;
        }

        /// <summary>
        /// [표준경보시스템 종류] 
        /// </summary>
        /// <param name="warnKindCode"></param>
        /// <returns></returns>
        public static SASKind FindSASKindByCode(string kindCode)
        {
            System.Diagnostics.Debug.Assert(kindCode != null);

            SASKind result = null;

            if (stdAlertSystemKindInfo == null || stdAlertSystemKindInfo.Values == null)
            {
                return null;
            }
            foreach (SASKind info in stdAlertSystemKindInfo.Values)
            {
                if (info == null)
                {
                    continue;
                }

                if (info.Code.ToUpper() == kindCode.ToUpper())
                {
                    SASKind copy = new SASKind();
                    copy.DeepCopyFrom(info);
                    return copy;
                }
            }

            return result;
        }


        /// <summary>
        /// [재난 종류] 기상특보 정보로부터 관련 재난 종류 정보를 취득
        /// </summary>
        /// <param name="swrKindCode"></param>
        /// <param name="swrStressCode"></param>
        /// <returns></returns>
        public static DisasterKind FindDisasterKindBySWRInfo(string swrKindCode, string swrStressCode)
        {
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(swrKindCode));
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(swrStressCode));

            DisasterKind result = null;

            if (swrDisasterMatchingInfo == null)
            {
                return null;
            }
            foreach (SWRDisasterMatching info in swrDisasterMatchingInfo)
            {
                if (info == null)
                {
                    continue;
                }
                if (info.SwrKindCode == swrKindCode && info.SwrStressCode == swrStressCode)
                {
                    result = FindDisasterKindByCode(info.DisasterKindCode);
                    break;
                }
            }

            return result;
        }
        public static string FindSWRKindStringByKindCode(string kindCode)
        {
            System.Diagnostics.Debug.Assert(kindCode != null);

            if (swrKindInfo == null || swrKindInfo.Values == null)
            {
                return null;
            }

            if (swrKindInfo.ContainsKey(kindCode))
            {
                string copy = swrKindInfo[kindCode];
                return copy;
            }

            return null;
        }
        public static string FindSWRStressStringByStressCode(string stressCode)
        {
            System.Diagnostics.Debug.Assert(stressCode != null);

            if (swrStressInfo == null || swrStressInfo.Values == null)
            {
                return null;
            }

            if (swrStressInfo.ContainsKey(stressCode))
            {
                string copy = swrStressInfo[stressCode];
                return copy;
            }

            return null;
        }
        public static string FindSWRCommandStringByCommandCode(string commandCode)
        {
            System.Diagnostics.Debug.Assert(commandCode != null);

            if (swrCommandInfo == null || swrCommandInfo.Values == null)
            {
                return null;
            }

            if (swrCommandInfo.ContainsKey(commandCode))
            {
                string copy = swrCommandInfo[commandCode];
                return copy;
            }

            return null;
        }
        public static SWRAnnounceArea FindSWRAreaByAreaCode(string areaCode)
        {
            System.Diagnostics.Debug.Assert(areaCode != null);

            if (areaCode == null || swrAreaInfo == null)
            {
                return null;
            }

            if (swrAreaInfo.ContainsKey(areaCode))
            {
                SWRAnnounceArea copy = new SWRAnnounceArea();
                copy.DeepCopyFrom(swrAreaInfo[areaCode]);
                return copy;
            }

            return null;
        }
        #endregion

        #region 화면표시용_문자열생성
        public static string GetOrderRefTypeDescription(OrderReferenceType type)
        {
            string description = string.Empty;

            switch (type)
            {
                case OrderReferenceType.SWR:
                    {
                        description = "기상특보연계";
                    }
                    break;
                case OrderReferenceType.Cancel:
                    {
                        description = "발령취소";
                    }
                    break;
                case OrderReferenceType.Clear:
                    {
                        description = "해제경보";
                    }
                    break;
                case OrderReferenceType.None:
                default:
                    {
                    }
                    break;
            }

            return description;
        }
        #endregion
    }
}
