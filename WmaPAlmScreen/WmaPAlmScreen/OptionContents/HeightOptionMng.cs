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
    public class HeightOptionMng
    {
        private static HeightOptionDataContainer lstHeightOptionData = new HeightOptionDataContainer();
        private static string filePath = NCasEnvironmentMng.NCasAppEnvPath + "\\WmaPAlmHeightOption.xml";

        public static List<HeightOptionData> LstHeightOptionData
        {
            get { return lstHeightOptionData.LstHeightOptionData; }
            set { lstHeightOptionData.LstHeightOptionData = value; }
        }

        public static void LoadHeightOptionDatas()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    #region 임시저장
                    HeightOptionData od = new HeightOptionData();
                    od.UseTime = false;
                    od.FirstTime = 0;
                    od.SecondTime = 1;
                    od.UseAuto = false;
                    od.MaxValue = 100;
                    od.MaxValue2 = 200;
                    od.MaxValue3 = 300;
                    od.MaxValue4 = 400;
                    od.AutoTime = 3;
                    od.TestOrder = false;

                    StoredMessageText stoMsg = new StoredMessageText();
                    stoMsg.IsUse = true;
                    stoMsg.MsgNum = string.Empty;
                    stoMsg.PlayTime = 100;
                    stoMsg.RepeatCount = 1;
                    stoMsg.Text = string.Empty;
                    stoMsg.Title = string.Empty;
                    od.Msg = stoMsg;

                    stoMsg = new StoredMessageText();
                    stoMsg.IsUse = true;
                    stoMsg.MsgNum = string.Empty;
                    stoMsg.PlayTime = 100;
                    stoMsg.RepeatCount = 1;
                    stoMsg.Text = string.Empty;
                    stoMsg.Title = string.Empty;
                    od.Msg2 = stoMsg;

                    stoMsg = new StoredMessageText();
                    stoMsg.IsUse = true;
                    stoMsg.MsgNum = string.Empty;
                    stoMsg.PlayTime = 100;
                    stoMsg.RepeatCount = 1;
                    stoMsg.Text = string.Empty;
                    stoMsg.Title = string.Empty;
                    od.Msg3 = stoMsg;

                    stoMsg = new StoredMessageText();
                    stoMsg.IsUse = true;
                    stoMsg.MsgNum = string.Empty;
                    stoMsg.PlayTime = 100;
                    stoMsg.RepeatCount = 1;
                    stoMsg.Text = string.Empty;
                    stoMsg.Title = string.Empty;
                    od.Msg4 = stoMsg;

                    lstHeightOptionData.LstHeightOptionData.Add(od);
                    #endregion

                    SaveHeightOptionDatas();
                    return;
                }

                using (Stream stream = new FileStream(filePath, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(HeightOptionDataContainer));
                    lstHeightOptionData = (HeightOptionDataContainer)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILoggingException.WriteException("HeightOptionMng", "HeightOptionMng.LoadHeightOptionDatas() Method", ex);
            }
        }

        public static void SaveHeightOptionDatas()
        {
            try
            {
                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(HeightOptionDataContainer));
                    ser.Serialize(stream, lstHeightOptionData);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILoggingException.WriteException("HeightOptionMng", "HeightOptionMng.SaveHeightOptionDatas() Method", ex);
            }
        }
    }
}