using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

using NCASBIZ.NCasEnv;
using NCASBIZ.NCasUtility;
using NCASFND.NCasLogging;
using NCasAppCommon.Define;

namespace WmaPAlmScreen
{
    public class DisasterBroadFlagMng
    {
        private static string disasterBroadFlag = string.Empty;
        private static string filePath = NCasEnvironmentMng.NCasAppEnvPath + "\\WmaPAlmDisasterBroadFlag.xml";

        /// <summary>
        /// 플래그 값 프로퍼티
        /// 111 - 마이크/TTS/STO 모두 사용함
        /// 000 - 마이크/TTS/STO 모두 사용안함
        /// 101 - 마이크/TTS/STO 중 마이크/STO만 사용함
        /// </summary>
        public static string DisasterBroadFlag
        {
            get { return disasterBroadFlag; }
            set { disasterBroadFlag = value; }
        }

        /// <summary>
        /// 플래그 값 로드
        /// </summary>
        public static void LoadDisasterBroadFlag()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    disasterBroadFlag = "111";
                    SaveDisasterBroadFlag();
                    return;
                }

                using (Stream stream = new FileStream(filePath, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(String));
                    disasterBroadFlag = (String)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILoggingException.WriteException("DisasterBroadFlagMng", "DisasterBroadFlagMng.LoadDisasterBroadFlag() Method", ex);
            }
        }

        /// <summary>
        /// 플래그 값 저장
        /// </summary>
        public static void SaveDisasterBroadFlag()
        {
            try
            {
                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(String));
                    ser.Serialize(stream, disasterBroadFlag);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILoggingException.WriteException("DisasterBroadFlagMng", "DisasterBroadFlagMng.SaveDisasterBroadFlag() Method", ex);
            }
        }
    }
}