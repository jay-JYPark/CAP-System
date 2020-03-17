using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CAPLib;

namespace StdWarningInstallation.DataClass
{
    public class CAPHelper
    {
        /// <summary>
        /// CAP 메시지 아이디 생성
        /// </summary>
        /// <param name="senderName"></param>
        /// <returns></returns>
        public string MakeIdentifier(string senderName)
        {
            if (string.IsNullOrEmpty(senderName))
            {
                return null;
            }

            Random rd = new Random();
            int randomID = rd.Next(1, 100);
            string identifier = senderName +
                DateTime.Now.Day.ToString().PadLeft(2, '0') +
                DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                DateTime.Now.Minute.ToString().PadLeft(2, '0') +
                DateTime.Now.Second.ToString().PadLeft(2, '0') +
                randomID.ToString().PadLeft(2, '0');

            return identifier;
        }
        /// <summary>
        /// CAP 메시지의 Address 데이터 생성.
        /// Scope가 Privatae 일 때만 유효함.
        /// </summary>
        /// <param name="systemList"></param>
        /// <returns></returns>
        public string MakeAddress(List<SASProfile> systemList)
        {
            System.Diagnostics.Debug.Assert(systemList != null);
            System.Diagnostics.Debug.Assert(systemList.Count > 0);

            if (systemList == null || systemList.Count < 0)
            {
                return null;
            }

            StringBuilder builder = new StringBuilder(systemList[0].IpAddress);
            for (int index = 1; index < systemList.Count; index++)
            {
                string address = systemList[index].IpAddress;
                builder.Append("," + address);
            }
            return builder.ToString();
        }
        /// <summary>
        /// CAP 메시지의 Restriction 데이터 생성.
        /// Scope가 Restricted 일 때만 유효함.
        /// </summary>
        /// <param name="sasKindList"></param>
        /// <returns></returns>
        public string MakeRestriction(List<SASKind> sasKindList)
        {
            System.Diagnostics.Debug.Assert(sasKindList != null);
            System.Diagnostics.Debug.Assert(sasKindList.Count > 0);

            StringBuilder builder = new StringBuilder(sasKindList[0].Code);
            for (int index = 1; index < sasKindList.Count; index++)
            {
                string systemKind = sasKindList[index].Code;
                builder.Append("," + systemKind);
            }
            return builder.ToString();
        }
        /// <summary>
        /// CAP 메시지의 Headline 데이터 생성.
        /// </summary>
        /// <param name="sentTime"></param>
        /// <param name="modeName"></param>
        /// <param name="targetRegions"></param>
        /// <param name="disasterKindName"></param>
        /// <returns></returns>
        public string MakeHeadline(CAPLib.MsgType msgType, DateTime sentTime, string modeName, List<RegionDefinition> targetRegions, string disasterKindName)
        {
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(modeName));
            System.Diagnostics.Debug.Assert(targetRegions != null);
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(disasterKindName));

            string mode = "[" + modeName + "]";

            StringBuilder targetName = new StringBuilder();
            for (int i = 0; i < targetRegions.Count; i++)
            {
                if (i < targetRegions.Count - 1)
                {
                    targetName.Append(targetRegions[i].Name.Trim() + "/");
                }
                else
                {
                    targetName.Append(targetRegions[i].Name.Trim());
                }
            }

            StringBuilder builder = new StringBuilder();
            builder.Append(sentTime.ToString());
            builder.Append(" [" + modeName + "]");
            builder.Append(" [" + targetName.ToString() + "] 대상으로");
            builder.Append(" [" + disasterKindName + "]");

            if (msgType == MsgType.Alert)
            {
                builder.Append(" 발령");
            }
            else if (msgType == MsgType.Cancel)
            {
                builder.Append(" 발령 취소");
            }
            else
            {
                // 현재 시점, 이 외는 미대응.
            }

            return builder.ToString();
        }
        /// <summary>
        /// CAP 메시지의 Headline 데이터 생성.
        /// </summary>
        /// <param name="sentTime"></param>
        /// <param name="modeName"></param>
        /// <param name="targetRegions"></param>
        /// <param name="disasterKindName"></param>
        /// <returns></returns>
        public string MakeHeadline(CAPLib.MsgType msgType, DateTime sentTime, string modeName, List<SASProfile> targetSystems, string disasterKindName)
        {
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(modeName));
            System.Diagnostics.Debug.Assert(targetSystems != null);
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(disasterKindName));

            string mode = "[" + modeName + "]";

            StringBuilder targetName = new StringBuilder();
            for (int i = 0; i < targetSystems.Count; i++)
            {
                if (i < targetSystems.Count - 1)
                {
                    targetName.Append(targetSystems[i].Name.Trim() + "/");
                }
                else
                {
                    targetName.Append(targetSystems[i].Name.Trim());
                }
            }

            StringBuilder builder = new StringBuilder();
            builder.Append(sentTime.ToString());
            builder.Append(" [" + modeName + "]");
            builder.Append(" [" + targetName.ToString() + "] 대상으로");
            builder.Append(" [" + disasterKindName + "]");

            if (msgType == MsgType.Alert)
            {
                builder.Append(" 발령");
            }
            else if (msgType == MsgType.Cancel)
            {
                builder.Append(" 발령 취소");
            }
            else
            {
                // 현재 시점, 이 외는 미대응.
            }

            return builder.ToString();
        }
        /// <summary>
        /// CAP 메시지를 기반으로 헤드라인 문자열 작성.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string MakeHeadline(CAP msg)
        {
            System.Diagnostics.Debug.Assert(msg != null);

            try
            {
                StringBuilder builder = new StringBuilder();
                foreach (InfoType msgInfo in msg.Info)
                {
                    if (msgInfo.Language.ToUpper() == BasisData.DEFAULT_LANGUAGECODE.ToUpper())
                    {
                        builder.Append(msgInfo.Headline);
                        break;
                    }
                }

                if (builder.Length < 1)
                {
                    OrderMode modeInfo = BasisData.FindOrderModeInfoByCode(msg.MessageStatus.Value);
                    if (modeInfo == null || string.IsNullOrEmpty(modeInfo.Name))
                    {
                        FileLogManager.GetInstance().WriteLog("[CAPInfo] MakeHeadline ( 헤드라인 정보 작성 실패. 모드 정보를 찾을 수 없습니다. )");
                        throw new Exception("[CAPInfo] 헤드라인 정보 작성 실패. 모드 정보를 찾을 수 없습니다.");
                    }

                    if (msg.Info == null || msg.Info.Count < 1 || string.IsNullOrEmpty(msg.Info[0].Event))
                    {
                        FileLogManager.GetInstance().WriteLog("[CAPInfo] MakeHeadline ( 헤드라인 정보 작성 실패. 이벤트 코드 정보를 찾을 수 없습니다. )");
                        throw new Exception("[CAPInfo] 헤드라인 정보 작성 실패. 이벤트 코드 정보를 찾을 수 없습니다.");
                    }
                    string disasterKindName = msg.Info[0].Event;

                    string targets = ExtractTargetNamesFromCAP(msg);
                    if (string.IsNullOrEmpty(targets))
                    {
                        FileLogManager.GetInstance().WriteLog("[CAPInfo] MakeHeadline ( 헤드라인 정보 작성 실패. 발령 대상 정보를 찾을 수 없습니다. )");
                        throw new Exception("[CAPInfo] 헤드라인 정보 작성 실패. 발령 대상 정보를 찾을 수 없습니다.");
                    }

                    builder.Append(msg.SentDateTime.ToString());
                    builder.Append(" [" + modeInfo.Name + "]");
                    builder.Append(" [" + targets.Replace(',', '/') + "] 대상으로");
                    builder.Append(" [" + disasterKindName + "]");
                    builder.Append(" 발령");
                }

                return builder.ToString();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CAPInfo] MakeHeadline( Exception=[ " + ex.ToString() + "] )");
                FileLogManager.GetInstance().WriteLog("[CAPInfo] MakeHeadline ( Exception=[" + ex.ToString() + "] )");

                throw new Exception("[CAPInfo] 헤드라인 정보 작성 실패. 처리 중 예외가 발생하였습니다.");
            }
        }

        /// <summary>
        /// CAP 메시지로부터 발령 대상 정보를 추출.
        /// 1)Private의 경우, 아이디어드레스의 연속 문자열(구분자 콤마)
        /// 2)Public/Restricted의 경우, 지역코드의 연속 문자열(구분자 콤마)
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public List<string> ExtractTargetCodesFromCAP(CAP msg)
        {
            System.Diagnostics.Debug.Assert(msg != null);

            List<string> targets = new List<string>();
            
            StringBuilder builder = new StringBuilder();
            if (msg.Scope == CAPLib.ScopeType.Private)
            {
                List<SASProfile> targetSystems = ExtractTargetSystemsFromCAP(msg);
                if (targetSystems == null)
                {
                    return null;
                }
                foreach (SASProfile system in targetSystems)
                {
                    targets.Add(system.IpAddress);
                }
            }
            else
            {
                List<RegionDefinition> targetRegions = ExtractTargetRegionsFromCAP(msg);
                if (targetRegions == null)
                {
                    return null;
                }
                foreach (RegionDefinition region in targetRegions)
                {
                    if (region.Code != null)
                    {
                        targets.Add(region.Code.ToString());
                    }
                }
            }

            return targets;
        }
        /// <summary>
        /// CAP 메시지로부터 발령 대상 정보를 추출.
        /// 1)Private의 경우, 표준경보시스템 명칭의 연속 문자열(구분자 콤마)
        /// 2)Public/Restricted의 경우, 지역 명칭의 연속 문자열(구분자 콤마)
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public string ExtractTargetNamesFromCAP(CAP msg)
        {
            System.Diagnostics.Debug.Assert(msg != null);

            if (msg == null || msg.Info == null || msg.Info.Count < 1)
            {
                return null;
            }

            string targets = string.Empty;
            StringBuilder builder = new StringBuilder();

            InfoType info = msg.Info[0];
            if (info == null)
            {
                return null;
            }
            if (info.Area == null || info.Area.Count < 1)
            {
                return null;
            }

            targets = info.Area[0].AreaDesc.Replace(' ', ',');

            return targets;
        }
        /// <summary>
        /// CAP 메시지로부터 발령 대상 지역 정보를 추출.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public List<RegionDefinition> ExtractTargetRegionsFromCAP(CAP msg)
        {
            System.Diagnostics.Debug.Assert(msg != null);

            if (msg.Scope == CAPLib.ScopeType.Private)
            {
                return null;
            }
            if (msg.Info == null || msg.Info.Count < 1)
            {
                return null;
            }

            List<RegionDefinition> targets = new List<RegionDefinition>();

            InfoType info = msg.Info[0];
            if (info == null)
            {
                return null;
            }
            if (info.Area == null || info.Area.Count < 1)
            {
                return null;
            }

            foreach (AreaType area in info.Area)
            {
                int areaIndex = -1;
                string[] stringSeperators = new string[] { " " };

                foreach (NameValueType geoCode in area.GeoCode)
                {
                    areaIndex ++;

                    if (geoCode.Name != "KRDSTGeocode")
                    {
                        continue;
                    }
                    string code = geoCode.Value;
                    RegionProfile profile = BasisData.FindRegion(code);
                    if (profile == null)
                    {
                        string areaDesc = area.AreaDesc;
                        if (areaDesc == null)
                        {
                            continue;
                        }
                        string[] areaNames = areaDesc.Split(stringSeperators, StringSplitOptions.RemoveEmptyEntries);
                        if (areaNames != null && areaNames.Count() <= areaIndex)
                        {
                            continue;
                        }

                        RegionDefinition region = new RegionDefinition(code, areaNames[areaIndex]);
                        targets.Add(region);
                    }
                    else
                    {
                        RegionDefinition region = new RegionDefinition(profile.Code, profile.Name);
                        targets.Add(region);
                    }
                }
            }

            return targets;
        }
        /// <summary>
        /// CAP 메시지로부터 발령 대상 시스템 정보를 추출.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public List<SASProfile> ExtractTargetSystemsFromCAP(CAP msg)
        {
            System.Diagnostics.Debug.Assert(msg != null);
            System.Diagnostics.Debug.Assert(msg.Info != null);

            if (msg.Scope != CAPLib.ScopeType.Private)
            {
                return null;
            }
            if (string.IsNullOrEmpty(msg.Addresses))
            {
                return null;
            }

            List<SASProfile> profileInfo = DBManager.GetInstance().QuerySASInfo();
            if (profileInfo == null || profileInfo.Count < 1)
            {
                return null;
            }

            string[] stringSeperators = new string[] { "," };
            string[] dividedArray = msg.Addresses.Split(stringSeperators, StringSplitOptions.RemoveEmptyEntries);
            if (dividedArray == null || dividedArray.Count() <= 0)
            {
                return null;
            }

            List<SASProfile> targets = new List<SASProfile>();
            foreach (string systemIP in dividedArray)
            {
                foreach (SASProfile profile in profileInfo)
                {
                    if (profile.IpAddress == systemIP)
                    {
                        SASProfile copy = new SASProfile();
                        copy.DeepCopyFrom(profile);
                        targets.Add(copy);
                        break;
                    }
                }
            }

            return targets;
        }
        /// <summary>
        /// CAP 메시지로부터 시스템 종류 정보를 추출.
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public List<SASKind> ExtractTargetKindsFromCAP(CAP msg)
        {
            System.Diagnostics.Debug.Assert(msg != null);

            if (msg.Scope != CAPLib.ScopeType.Restricted)
            {
                return null;
            }
            if (string.IsNullOrEmpty(msg.Restriction))
            {
                return null;
            }

            string[] stringSeperators = new string[] { "," };
            string[] dividedArray = msg.Restriction.Split(stringSeperators, StringSplitOptions.RemoveEmptyEntries);
            if (dividedArray == null || dividedArray.Count() <= 0)
            {
                return null;
            }

            List<SASKind> targets = new List<SASKind>();
            foreach (string systemKindCode in dividedArray)
            {
                SASKind kindInfo = BasisData.FindSASKindByCode(systemKindCode);
                if (kindInfo != null)
                {
                    targets.Add(kindInfo);
                }
            }

            return targets;
        }

        public string GetLanguageNamesFromCAP(CAP msg)
        {
            System.Diagnostics.Debug.Assert(msg != null);

            if (msg.Info == null || msg.Info.Count < 1)
            {
                return null;
            }

            bool isFirst = true;
            StringBuilder builder = new StringBuilder();
            foreach (InfoType info in msg.Info)
            {
                if (info == null || string.IsNullOrEmpty(info.Language))
                {
                    continue;
                }
                string name = string.Empty;
                MsgTextDisplayLanguageKind kindInfo = BasisData.FindMsgTextLanguageInfoByCode(info.Language);
                if (kindInfo == null)
                {
                    continue;
                }
                if (isFirst)
                {
                    isFirst = false;
                    builder.Append(kindInfo.LanguageName);
                }
                else
                {
                    builder.Append("," + kindInfo.LanguageName);
                }
            }

            return builder.ToString();
        }
        public List<CAPParameterMsgInfo> ExtractMsgTextFromCAP(CAP msg, string languageKindCode = null, string mediaTypeCode = null)
        {
            System.Diagnostics.Debug.Assert(msg != null);

            if (msg.Info == null || msg.Info.Count < 1 || languageKindCode == null)
            {
                return null;
            }

            List<CAPParameterMsgInfo> result = new List<CAPParameterMsgInfo>();
            foreach (InfoType info in msg.Info)
            {
                if (info == null)
                {
                    continue;
                }
                if (languageKindCode != null && info.Language != languageKindCode)
                {
                    continue;
                }
                foreach (NameValueType parameter in info.Parameter)
                {
                    if (parameter == null)
                    {
                        continue;
                    }
                    if (mediaTypeCode != null && mediaTypeCode != parameter.Name)
                    {
                        continue;
                    }
                    CAPParameterMsgInfo msgInfo = new CAPParameterMsgInfo();
                    msgInfo.ValueName = parameter.Name;
                    msgInfo.Value = parameter.Value;
                    msgInfo.LanguageCode = info.Language;

                    result.Add(msgInfo);
                }
            }

            return result;
        }

        public string ExtractAckNoteFromCAP(CAP msg)
        {
            System.Diagnostics.Debug.Assert(msg != null);

            if (msg == null || msg.MessageType.Value != MsgType.Ack)
            {
                return null;
            }

            return msg.Note;
        }
    }
}