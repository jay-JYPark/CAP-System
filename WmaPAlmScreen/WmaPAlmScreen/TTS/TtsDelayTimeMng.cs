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
    public class TtsDelayTimeMng
    {
        private static string ttsDelayTime = string.Empty;
        private static string filePath = NCasEnvironmentMng.NCasAppEnvPath + "\\WmaPAlmTtsDelayTime.xml";

        /// <summary>
        /// TTS Delay Time 프로퍼티
        /// </summary>
        public static string TtsDelayTime
        {
            get { return ttsDelayTime; }
            set { ttsDelayTime = value; }
        }

        /// <summary>
        /// TTS Delay Time 로드
        /// </summary>
        public static void LoadTtsDelayTime()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    ttsDelayTime = "5000";
                    SaveTtsDelayTime();
                    return;
                }

                using (Stream stream = new FileStream(filePath, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(String));
                    ttsDelayTime = (String)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILoggingException.WriteException("TtsDelayTimeMng", "TtsDelayTimeMng.LoadTtsDelayTime() Method", ex);
            }
        }

        /// <summary>
        /// TTS Delay Time 저장
        /// </summary>
        public static void SaveTtsDelayTime()
        {
            try
            {
                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(String));
                    ser.Serialize(stream, ttsDelayTime);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILoggingException.WriteException("TtsDelayTimeMng", "TtsDelayTimeMng.SaveTtsDelayTime() Method", ex);
            }
        }
    }
}