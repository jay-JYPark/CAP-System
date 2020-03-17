using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

using NCASFND.NCasLogging;
using NCASBIZ.NCasEnv;
using NCasAppCommon.Define;
using NCasAppCommon.Type;

namespace WmaPAlmScreen
{
    public class KeyDataMng
    {
        private static KeyDataContainer lstKeyData = new KeyDataContainer();

        /// <summary>
        /// 버튼 데이터 컨테이너 프로퍼티
        /// </summary>
        public static List<NCasKeyData> LstKeyData
        {
            get { return lstKeyData.LstKeyData; }
        }

        /// <summary>
        /// 버튼 데이터 로드
        /// </summary>
        public static void LoadKeyDatas()
        {
            try
            {
                string filePath = NCasEnvironmentMng.NCasAppEnvPath + "\\NCasPDAlmKeyData.xml";

                if (!File.Exists(filePath))
                    return;

                using (Stream stream = new FileStream(filePath, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(KeyDataContainer));
                    lstKeyData = (KeyDataContainer)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILoggingException.WriteException("KeyDataMng", "KeyDataMng.LoadKeyDatas() Method", ex);
            }
        }

        /// <summary>
        /// 버튼 데이터 저장
        /// </summary>
        public static void SaveKeyDatas()
        {
        }

        /// <summary>
        /// 버튼 데이터 저장
        /// </summary>
        /// <param name="keyData">저장할 버튼 데이터</param>
        public static void AddKeyData(NCasKeyData keyData)
        {
        }

        /// <summary>
        /// 단일 버튼 데이터 가져오기
        /// </summary>
        /// <param name="keyData"></param>
        /// <returns></returns>
        public static NCasKeyData GetKeyData(NCasKeyData keyData)
        {
            NCasKeyData rstKeyData = new NCasKeyData();

            foreach (NCasKeyData eachKeyData in lstKeyData.LstKeyData)
            {
                if (eachKeyData == keyData)
                {
                    rstKeyData = eachKeyData;
                    break;
                }
            }

            return rstKeyData;
        }

        /// <summary>
        /// 버튼 데이터 삭제
        /// </summary>
        /// <param name="keyData"></param>
        public static void RemoveKeyData(NCasKeyData keyData)
        {
        }
    }
}