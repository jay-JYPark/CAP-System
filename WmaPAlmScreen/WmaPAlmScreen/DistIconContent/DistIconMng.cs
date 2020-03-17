using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

using NCASBIZ.NCasEnv;
using NCASBIZ.NCasDefine;
using NCASFND.NCasLogging;
using NCasAppCommon.Define;

namespace WmaPAlmScreen
{
    public static class DistIconMng
    {
        private static DistIconDataContainer lstDistIconData = new DistIconDataContainer();
        private static string filePath = NCasEnvironmentMng.NCasAppEnvPath + "\\WmaPAlmDistIconData.xml";

        /// <summary>
        /// 시군 아이콘 데이터 리스트 프로퍼티
        /// </summary>
        public static List<DistIconData> LstDistIconData
        {
            get { return lstDistIconData.LstDistIconData; }
            set { lstDistIconData.LstDistIconData = value; }
        }

        /// <summary>
        /// 시군 아이콘 데이터 로드
        /// </summary>
        public static void LoadDistIconDatas()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    #region 임시 시군 아이콘 정보 파일 생성
                    DistIconData tmp = new DistIconData();
                    tmp.Code = 2500;
                    tmp.X = 250;
                    tmp.Y = 300;
                    lstDistIconData.LstDistIconData.Add(tmp);

                    tmp = new DistIconData();
                    tmp.Code = 2501;
                    tmp.X = 550;
                    tmp.Y = 270;
                    lstDistIconData.LstDistIconData.Add(tmp);

                    tmp = new DistIconData();
                    tmp.Code = 2502;
                    tmp.X = 350;
                    tmp.Y = 120;
                    lstDistIconData.LstDistIconData.Add(tmp);

                    tmp = new DistIconData();
                    tmp.Code = 2503;
                    tmp.X = 450;
                    tmp.Y = 600;
                    lstDistIconData.LstDistIconData.Add(tmp);

                    tmp = new DistIconData();
                    tmp.Code = 2504;
                    tmp.X = 650;
                    tmp.Y = 490;
                    lstDistIconData.LstDistIconData.Add(tmp);
                    #endregion

                    SaveDistIconDatas();
                    return;
                }

                using (Stream stream = new FileStream(filePath, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(DistIconDataContainer));
                    lstDistIconData = (DistIconDataContainer)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILoggingException.WriteException("DistIconMng", "DistIconMng.LoadDistIconDatas() Method", ex);
            }
        }

        /// <summary>
        /// 시군 아이콘 데이터 저장
        /// </summary>
        public static void SaveDistIconDatas()
        {
            try
            {
                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(DistIconDataContainer));
                    ser.Serialize(stream, lstDistIconData);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILoggingException.WriteException("DistIconMng", "DistIconMng.SaveDistIconDatas() Method", ex);
            }
        }

        /// <summary>
        /// 시군의 Code를 받아 해당 시군의 정보를 리턴한다.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static DistIconData GetDistIconData(int code)
        {
            DistIconData rst = null;

            foreach(DistIconData distIcon in lstDistIconData.LstDistIconData)
            {
                if (distIcon.Code == code)
                {
                    rst = distIcon;
                    break;
                }
            }

            return rst;
        }
    }
}