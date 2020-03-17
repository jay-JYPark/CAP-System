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
    class HeightPointContentMng
    {
        private static HeightPointContentContainer lstHeightPointContent = new HeightPointContentContainer();
        private static string filePath = NCasEnvironmentMng.NCasAppEnvPath + "\\WmaPAlmHeightPointData.xml";

        /// <summary>
        /// 관측소 정보 컨테이너 프로퍼티
        /// </summary>
        public static List<HeightPointContent> LstHeightPointContent
        {
            get { return lstHeightPointContent.LstHeightPointContent; }
        }

        /// <summary>
        /// 관측소 정보 로드
        /// </summary>
        public static void LoadHeightPointContent()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    #region 관측소 정보생성
                    HeightPointData heightPointData = new HeightPointData();
                    HeightPointContent heightPointContent = new HeightPointContent();
                    heightPointContent.AddHeightPointData(heightPointData);
                    heightPointContent.Title = "관측소1";
                    heightPointContent.LastTime = new DateTime();
                    HeightPointContentMng.AddHeightPointContent(heightPointContent);

                    heightPointData = new HeightPointData();
                    heightPointContent = new HeightPointContent();
                    heightPointContent.AddHeightPointData(heightPointData);
                    heightPointContent.Title = "관측소2";
                    heightPointContent.LastTime = new DateTime();
                    HeightPointContentMng.AddHeightPointContent(heightPointContent);
                    #endregion

                    SaveHeightPointContent();
                    return;
                }

                using (Stream stream = new FileStream(filePath, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(HeightPointContentContainer));
                    lstHeightPointContent = (HeightPointContentContainer)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILoggingException.WriteException("HeightPointContentMng", "LoadHeightPointContent() Method", ex);
            }
        }

        /// <summary>
        /// 관측소 정보 저장
        /// </summary>
        public static void SaveHeightPointContent()
        {
            try
            {
                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(HeightPointContentContainer));
                    ser.Serialize(stream, lstHeightPointContent);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILoggingException.WriteException("HeightPointContentMng", "SaveHeightPointContent() Method", ex);
            }
        }

        /// <summary>
        /// 단일 관측소 정보 저장
        /// </summary>
        /// <param name="groupContent"></param>
        public static void AddHeightPointContent(HeightPointContent heightPointContent)
        {
            foreach (HeightPointContent Content in lstHeightPointContent.LstHeightPointContent)
            {
                if (Content.Title == heightPointContent.Title) //기존에 있는 관측소면..
                {
                    Content.LstHeightPointData.Clear();
                    Content.LstHeightPointData = heightPointContent.LstHeightPointData;
                    SaveHeightPointContent();
                    return;
                }
            }

            lstHeightPointContent.LstHeightPointContent.Add(heightPointContent);
            SaveHeightPointContent();
        }

        /// <summary>
        /// 단일 관측소 정보 가져오기
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static HeightPointContent GetHeightPointContent(string title)
        {
            HeightPointContent heightPointContent = new HeightPointContent();

            foreach (HeightPointContent eachHeightPointContent in lstHeightPointContent.LstHeightPointContent)
            {
                if (eachHeightPointContent.Title == title)
                {
                    heightPointContent = eachHeightPointContent;
                    break;
                }
            }

            return heightPointContent;
        }
    }
}