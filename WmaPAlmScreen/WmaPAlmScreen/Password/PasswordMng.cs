using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;

using NCASBIZ.NCasEnv;
using NCASBIZ.NCasUtility;
using NCASFND.NCasLogging;
using NCasAppCommon.Define;

namespace WmaPAlmScreen
{
    public class PasswordMng
    {
        private static string password = string.Empty;
        private static string filePath = NCasEnvironmentMng.NCasAppEnvPath + "\\WmaPAlmPassword.xml";
        private static readonly string passwordKey = "cas";

        /// <summary>
        /// 비밀번호 프로퍼티
        /// </summary>
        public static string Password
        {
            get { return password; }
            set { password = value; }
        }

        /// <summary>
        /// 비밀번호 로드
        /// </summary>
        public static void LoadPassword()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    SavePassword();
                    return;
                }

                using (Stream stream = new FileStream(filePath, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(String));
                    password = (String)serializer.Deserialize(stream);
                }

                //password = DecordingPassword(password, passwordKey);
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILoggingException.WriteException("PasswordMng", "PasswordMng.LoadPassword() Method", ex);
            }
        }

        /// <summary>
        /// 비밀번호 저장
        /// </summary>
        public static void SavePassword()
        {
            try
            {
                #region 임시 비밀번호 파일 생성
                password = "cas";
                #endregion

                //password = EncordingPassword(password, passwordKey);

                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(String));
                    ser.Serialize(stream, password);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILoggingException.WriteException("PasswordMng", "PasswordMng.SavePassword() Method", ex);
            }
        }

        /// <summary>
        /// 비밀번호 암호화
        /// </summary>
        /// <param name="password"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string EncordingPassword(string password, string key)
        {
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            byte[] passwordText = Encoding.Unicode.GetBytes(password);
            byte[] salt = Encoding.ASCII.GetBytes(key.Length.ToString());

            PasswordDeriveBytes createKey = new PasswordDeriveBytes(key, salt);
            ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(createKey.GetBytes(32), createKey.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            cryptoStream.Write(passwordText, 0, passwordText.Length);
            cryptoStream.FlushFinalBlock();
            byte[] cipherBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string EncryptedData = Convert.ToBase64String(cipherBytes);

            return EncryptedData;
        }

        /// <summary>
        /// 비밀번호 복호화
        /// </summary>
        /// <param name="password"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string DecordingPassword(string password, string key)
        {
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            byte[] encryptedData = Convert.FromBase64String(password);
            byte[] salt = Encoding.ASCII.GetBytes(key.Length.ToString());

            PasswordDeriveBytes createKey = new PasswordDeriveBytes(key, salt);
            ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(createKey.GetBytes(32), createKey.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream(encryptedData);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            byte[] passwordText = new byte[encryptedData.Length];
            int DecryptedCount = cryptoStream.Read(passwordText, 0, passwordText.Length);
            memoryStream.Close();
            cryptoStream.Close();
            string DecryptedData = Encoding.Unicode.GetString(passwordText, 0, DecryptedCount);

            return DecryptedData;
        }
    }
}