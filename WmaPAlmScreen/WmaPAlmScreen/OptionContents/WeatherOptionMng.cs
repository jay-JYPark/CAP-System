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
using NCasMsgCommon.Std;
using NCasContentsModule.StoMsg;

namespace WmaPAlmScreen
{
    public class WeatherOptionMng
    {
        private static WeatherOptionDataContainer lstWeatherOptionData = new WeatherOptionDataContainer();
        private static string filePath = NCasEnvironmentMng.NCasAppEnvPath + "\\WmaPAlmWeatherOption.xml";

        public static List<WeatherOptionData> LstWeatherOptionData
        {
            get { return lstWeatherOptionData.LstWeatherOptionData; }
            set { lstWeatherOptionData.LstWeatherOptionData = value; }
        }

        public static void LoadWeatherOptionDatas()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    #region 임시저장
                    WeatherOptionData od = new WeatherOptionData();
                    od.UseTime = false;
                    od.FirstTime = 0;
                    od.SecondTime = 1;
                    od.UseAuto = false;
                    od.AutoTime = 3;

                    StoredMessageText stoMsg = new StoredMessageText();
                    stoMsg.IsUse = true;
                    stoMsg.MsgNum = string.Empty;
                    stoMsg.PlayTime = 100;
                    stoMsg.RepeatCount = 1;
                    stoMsg.Text = string.Empty;
                    stoMsg.Title = string.Empty;

                    WeatherKindData kind = new WeatherKindData();
                    kind.EWeatherKind = WeatherKindData.WeatherKind.galeWatch;
                    kind.StoMsg = stoMsg;
                    kind.LastTime = new DateTime();
                    od.WeatherKindMsg.Add(kind);

                    kind = new WeatherKindData();
                    kind.EWeatherKind = WeatherKindData.WeatherKind.galeAlarm;
                    kind.StoMsg = stoMsg;
                    kind.LastTime = new DateTime();
                    od.WeatherKindMsg.Add(kind);

                    kind = new WeatherKindData();
                    kind.EWeatherKind = WeatherKindData.WeatherKind.downpourWatch;
                    kind.StoMsg = stoMsg;
                    kind.LastTime = new DateTime();
                    od.WeatherKindMsg.Add(kind);

                    kind = new WeatherKindData();
                    kind.EWeatherKind = WeatherKindData.WeatherKind.downpourAlarm;
                    kind.StoMsg = stoMsg;
                    kind.LastTime = new DateTime();
                    od.WeatherKindMsg.Add(kind);

                    kind = new WeatherKindData();
                    kind.EWeatherKind = WeatherKindData.WeatherKind.coldwaveWatch;
                    kind.StoMsg = stoMsg;
                    kind.LastTime = new DateTime();
                    od.WeatherKindMsg.Add(kind);

                    kind = new WeatherKindData();
                    kind.EWeatherKind = WeatherKindData.WeatherKind.coldwaveAlarm;
                    kind.StoMsg = stoMsg;
                    kind.LastTime = new DateTime();
                    od.WeatherKindMsg.Add(kind);

                    kind = new WeatherKindData();
                    kind.EWeatherKind = WeatherKindData.WeatherKind.dryWatch;
                    kind.StoMsg = stoMsg;
                    kind.LastTime = new DateTime();
                    od.WeatherKindMsg.Add(kind);

                    kind = new WeatherKindData();
                    kind.EWeatherKind = WeatherKindData.WeatherKind.dryAlarm;
                    kind.StoMsg = stoMsg;
                    kind.LastTime = new DateTime();
                    od.WeatherKindMsg.Add(kind);

                    kind = new WeatherKindData();
                    kind.EWeatherKind = WeatherKindData.WeatherKind.tsunamiWatch;
                    kind.StoMsg = stoMsg;
                    kind.LastTime = new DateTime();
                    od.WeatherKindMsg.Add(kind);

                    kind = new WeatherKindData();
                    kind.EWeatherKind = WeatherKindData.WeatherKind.tsunamiAlarm;
                    kind.StoMsg = stoMsg;
                    kind.LastTime = new DateTime();
                    od.WeatherKindMsg.Add(kind);

                    kind = new WeatherKindData();
                    kind.EWeatherKind = WeatherKindData.WeatherKind.heavyseasWatch;
                    kind.StoMsg = stoMsg;
                    kind.LastTime = new DateTime();
                    od.WeatherKindMsg.Add(kind);

                    kind = new WeatherKindData();
                    kind.EWeatherKind = WeatherKindData.WeatherKind.heavyseasAlarm;
                    kind.StoMsg = stoMsg;
                    kind.LastTime = new DateTime();
                    od.WeatherKindMsg.Add(kind);

                    kind = new WeatherKindData();
                    kind.EWeatherKind = WeatherKindData.WeatherKind.stormWatch;
                    kind.StoMsg = stoMsg;
                    kind.LastTime = new DateTime();
                    od.WeatherKindMsg.Add(kind);

                    kind = new WeatherKindData();
                    kind.EWeatherKind = WeatherKindData.WeatherKind.stormAlarm;
                    kind.StoMsg = stoMsg;
                    kind.LastTime = new DateTime();
                    od.WeatherKindMsg.Add(kind);

                    kind = new WeatherKindData();
                    kind.EWeatherKind = WeatherKindData.WeatherKind.heavysnowWatch;
                    kind.StoMsg = stoMsg;
                    kind.LastTime = new DateTime();
                    od.WeatherKindMsg.Add(kind);

                    kind = new WeatherKindData();
                    kind.EWeatherKind = WeatherKindData.WeatherKind.heavysnowAlarm;
                    kind.StoMsg = stoMsg;
                    kind.LastTime = new DateTime();
                    od.WeatherKindMsg.Add(kind);

                    kind = new WeatherKindData();
                    kind.EWeatherKind = WeatherKindData.WeatherKind.sandstormWatch;
                    kind.StoMsg = stoMsg;
                    kind.LastTime = new DateTime();
                    od.WeatherKindMsg.Add(kind);

                    kind = new WeatherKindData();
                    kind.EWeatherKind = WeatherKindData.WeatherKind.sandstormAlarm;
                    kind.StoMsg = stoMsg;
                    kind.LastTime = new DateTime();
                    od.WeatherKindMsg.Add(kind);

                    od.LstAreaCode.Add("L1000000");
                    od.LstAreaCode.Add("L1010000");
                    od.LstAreaCode.Add("L1012500");
                    od.TestOrder = false;
                    
                    lstWeatherOptionData.LstWeatherOptionData.Add(od);
                    #endregion

                    SaveWeatherOptionDatas();
                    return;
                }

                using (Stream stream = new FileStream(filePath, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(WeatherOptionDataContainer));
                    lstWeatherOptionData = (WeatherOptionDataContainer)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILoggingException.WriteException("WeatherOptionMng", "WeatherOptionMng.LoadWeatherOptionDatas() Method", ex);
            }
        }

        public static void SaveWeatherOptionDatas()
        {
            try
            {
                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(WeatherOptionDataContainer));
                    ser.Serialize(stream, lstWeatherOptionData);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILoggingException.WriteException("WeatherOptionMng", "WeatherOptionMng.SaveWeatherOptionDatas() Method", ex);
            }
        }
    }
}