using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

using Adeng.Framework.Util;
using StdWarningInstallation.DataClass;

namespace StdWarningInstallation
{
    public class ConfigManager
    {
        private static ConfigManager thisObj = null;
        private static Mutex mutex = new Mutex();
        private ConfigData configData = new ConfigData();

        private readonly string INI_PATH = Directory.GetCurrentDirectory() + "\\SWI.ini";
        private readonly string KEY_DB = "Database";
        private readonly string KEY_IAGW = "IAGW";
        private readonly string KEY_SWR = "SWR";
        private readonly string KEY_LOCAL = "LocalSetting";
        // [Database]
        private readonly string DB_HOST_IP = "HostIP";
        private readonly string DB_USER_ID = "UserID";
        private readonly string DB_USER_PWD = "UserPWD";
        private readonly string DB_SERVICE_ID = "ServiceID";
        // [IAGW]
        private readonly string IAGW_AUTHCODE = "AuthenticationCode";
        private readonly string IAGW_IP = "ServerIP";
        private readonly string IAGW_PORT = "ServerPort";
        private readonly string IAGW_AUTOCONN = "AutoReConnection";
        // [SWR]
        private readonly string SWR_USE = "UseService";
        private readonly string SWR_KEY = "ServiceKey";
        private readonly string SWR_CYCLE = "CycleTimeMinute";
        // [LocalSetting]
        private readonly string REGION_CODE = "RegionCode";
        private readonly string SENDER_ID = "SenderID";
        private readonly string SENDER_NAME = "SenderName";

        /// <summary>
        /// ConfigData 접근자
        /// </summary>
        public ConfigData ConfigInfo
        {
            get
            {
                ConfigData copy = new ConfigData();
                copy.DeepCopyFrom(this.configData);
                return copy;
            }
        }

        /// <summary>
        /// 싱글톤 처리
        /// 반드시 이 메소드를 호출하여 인스턴스를 사용해야 한다.
        /// </summary>
        /// <returns></returns>
        public static ConfigManager GetInstance()
        {
            mutex.WaitOne();

            if (thisObj == null)
            {
                thisObj = new ConfigManager();
            }

            mutex.ReleaseMutex();
            return thisObj;
        }

        /// <summary>
        /// 생성자
        /// 싱글톤 생성 메소드를 사용하여 인스턴스 생성할 것.
        /// </summary>
        private ConfigManager()
        {
            this.LoadSettingFile();
        }

        /// <summary>
        /// 설정 파일 LoadSettingFile
        /// </summary>
        private void LoadSettingFile()
        {
            if (!File.Exists(INI_PATH))
            {
                // Database
                WriteKeyValueToDatabase(this.DB_HOST_IP, "127.0.0.1");
                WriteKeyValueToDatabase(this.DB_USER_ID, "swidb");
                WriteKeyValueToDatabase(this.DB_USER_PWD, "swi2014");
                WriteKeyValueToDatabase(this.DB_SERVICE_ID, "orcl");
                // 통합경보게이트웨이
                WriteKeyValueToIAGW(this.IAGW_AUTHCODE, "");
                WriteKeyValueToIAGW(this.IAGW_IP, "127.0.0.1");
                WriteKeyValueToIAGW(this.IAGW_PORT, "26750");
                WriteKeyValueToIAGW(this.IAGW_AUTOCONN, "1");
                // 기상특보
                WriteKeyValueToSWR(this.SWR_USE, "0");
                WriteKeyValueToSWR(this.SWR_KEY, "");
                WriteKeyValueToSWR(this.SWR_CYCLE, "10");
                // 로컬 환경
                WriteKeyValueToLocalSetting(this.REGION_CODE, "0000000000");
                WriteKeyValueToLocalSetting(this.SENDER_ID, "표준발령대");
                WriteKeyValueToLocalSetting(this.SENDER_NAME, "표준발령대_관리자");
            }

            this.configData.DB.HostIP = ReadKeyValueFromDatabase(this.DB_HOST_IP);
            this.configData.DB.UserID = ReadKeyValueFromDatabase(this.DB_USER_ID);
            this.configData.DB.UserPassword = ReadKeyValueFromDatabase(this.DB_USER_PWD);
            this.configData.DB.ServiceID = ReadKeyValueFromDatabase(this.DB_SERVICE_ID);

            this.configData.IAGW.AuthCode = ReadKeyValueFromIAGW(this.IAGW_AUTHCODE);
            this.configData.IAGW.ServerIP = ReadKeyValueFromIAGW(this.IAGW_IP);
            this.configData.IAGW.ServerPort = ReadKeyValueFromIAGW(this.IAGW_PORT);
            this.configData.IAGW.UseAutoConnection = false;
            string isUseAutoConn = ReadKeyValueFromIAGW(this.IAGW_AUTOCONN);
            if (!string.IsNullOrEmpty(isUseAutoConn) && isUseAutoConn == "1")
            {
                this.configData.IAGW.UseAutoConnection = true;
            }

            this.configData.SWR.UseService = false;
            string isUse = ReadKeyValueFromSWR(this.SWR_USE);
            if (!string.IsNullOrEmpty(isUse) && isUse == "1")
            {
                this.configData.SWR.UseService = true;
            }
            this.configData.SWR.ServiceKey = ReadKeyValueFromSWR(this.SWR_KEY);
            string cycleTime = ReadKeyValueFromSWR(this.SWR_CYCLE);
            int temp = 1; // 최솟값
            if (int.TryParse(cycleTime, out temp))
            {
                this.configData.SWR.CycleTimeMinute = temp;
            }

            this.configData.LOCAL.RegionCode = ReadKeyValueFromLocalSetting(this.REGION_CODE);
            this.configData.LOCAL.SenderID = ReadKeyValueFromLocalSetting(this.SENDER_ID);
            this.configData.LOCAL.SenderName = ReadKeyValueFromLocalSetting(this.SENDER_NAME);
        }

        #region INI 값 읽기/쓰기
        /// <summary>
        /// [Database] 섹션의 키 값 읽기
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns>키 설정 값</returns>
        private string ReadKeyValueFromDatabase(string keyName)
        {
            return ReadKeyValue(KEY_DB, keyName);
        }
        /// <summary>
        /// [Database] 섹션의 키 값 쓰기
        /// </summary>
        /// <param name="keyName">키 이름</param>
        /// <param name="keyValue">키 값</param>
        /// <returns></returns>
        private void WriteKeyValueToDatabase(string keyName, string keyValue)
        {
            WriteKeyValue(KEY_DB, keyName, keyValue);
        }

        /// <summary>
        /// [IntegratedGateway] 섹션의 키 값 읽기
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns>키 설정 값</returns>
        private string ReadKeyValueFromIAGW(string keyName)
        {
            return ReadKeyValue(KEY_IAGW, keyName);
        }
        /// <summary>
        /// [IntegratedGateway] 섹션의 키 값 쓰기
        /// </summary>
        /// <param name="keyName">키 이름</param>
        /// <param name="keyValue">키 값</param>
        /// <returns></returns>
        private void WriteKeyValueToIAGW(string keyName, string keyValue)
        {
            WriteKeyValue(KEY_IAGW, keyName, keyValue);
        }

        /// <summary>
        /// [SWRProfile] 섹션의 키 값 읽기
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns>키 설정 값</returns>
        private string ReadKeyValueFromSWR(string keyName)
        {
            return ReadKeyValue(KEY_SWR, keyName);
        }
        /// <summary>
        /// [SWRProfile] 섹션의 키 값 쓰기
        /// </summary>
        /// <param name="keyName">키 이름</param>
        /// <param name="keyValue">키 값</param>
        /// <returns></returns>
        private void WriteKeyValueToSWR(string keyName, string keyValue)
        {
            WriteKeyValue(KEY_SWR, keyName, keyValue);
        }

        /// <summary>
        /// [LocalSetting] 섹션의 키 값 읽기
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns>키 설정 값</returns>
        private string ReadKeyValueFromLocalSetting(string keyName)
        {
            return ReadKeyValue(KEY_LOCAL, keyName);
        }
        /// <summary>
        /// [LocalSetting] 섹션의 키 값 쓰기
        /// </summary>
        /// <param name="keyName">키 이름</param>
        /// <param name="keyValue">키 값</param>
        /// <returns></returns>
        private void WriteKeyValueToLocalSetting(string keyName, string keyValue)
        {
            WriteKeyValue(KEY_LOCAL, keyName, keyValue);
        }

        /// <summary>
        /// 키 값 읽기
        /// </summary>
        /// <param name="keyName"></param>
        /// <returns>키 설정 값</returns>
        private string ReadKeyValue(string sectionName, string keyName)
        {
            return AdengUtil.INIReadValueString(sectionName, keyName, INI_PATH);
        }
        /// <summary>
        /// [LocalSetting] 섹션의 키 값 쓰기
        /// </summary>
        /// <param name="keyName">키 이름</param>
        /// <param name="keyValue">키 값</param>
        /// <returns></returns>
        private void WriteKeyValue(string sectionName, string keyName, string keyValue)
        {
            AdengUtil.INIWriteValueString(sectionName, keyName, keyValue, INI_PATH);
        }
        #endregion

        /// <summary>
        /// 데이터베이스 접속 정보 갱신
        /// </summary>
        /// <param name="newData">새 설정 정보</param>
        public void SaveConfigInfo(ConfigDBData newData)
        {
            WriteKeyValueToDatabase(this.DB_HOST_IP, newData.HostIP);
            WriteKeyValueToDatabase(this.DB_USER_ID, newData.UserID);
            WriteKeyValueToDatabase(this.DB_USER_PWD, newData.UserPassword);
            WriteKeyValueToDatabase(this.DB_SERVICE_ID, newData.ServiceID);

            this.configData.DB.HostIP = newData.HostIP;
            this.configData.DB.UserID = newData.UserID;
            this.configData.DB.UserPassword = newData.UserPassword;
            this.configData.DB.ServiceID = newData.ServiceID;
        }

        /// <summary>
        /// 통합경보게이트웨이 접속 정보 갱신
        /// </summary>
        /// <param name="newData">새 설정 정보</param>
        public void SaveConfigInfo(ConfigIAGWData newData)
        {
            WriteKeyValueToIAGW(this.IAGW_AUTHCODE, newData.AuthCode);
            WriteKeyValueToIAGW(this.IAGW_IP, newData.ServerIP);
            WriteKeyValueToIAGW(this.IAGW_PORT, newData.ServerPort);
            WriteKeyValueToIAGW(this.IAGW_AUTOCONN, string.Format("{0}", Convert.ToInt32(newData.UseAutoConnection)));

            this.configData.IAGW.AuthCode = newData.AuthCode;
            this.configData.IAGW.ServerIP = newData.ServerIP;
            this.configData.IAGW.ServerPort = newData.ServerPort;
            this.configData.IAGW.UseAutoConnection = newData.UseAutoConnection;
        }

        /// <summary>
        /// 기상 특보 웹서비스 접속 정보 갱신
        /// </summary>
        /// <param name="newData">새 설정 정보</param>
        public void SaveConfigInfo(ConfigSWRData newData)
        {
            WriteKeyValueToSWR(this.SWR_USE, string.Format("{0}", Convert.ToInt32(newData.UseService)));
            WriteKeyValueToSWR(this.SWR_KEY, newData.ServiceKey);
            WriteKeyValueToSWR(this.SWR_CYCLE, newData.CycleTimeMinute.ToString());

            this.configData.SWR.UseService = newData.UseService;
            this.configData.SWR.ServiceKey = newData.ServiceKey;
            this.configData.SWR.CycleTimeMinute = newData.CycleTimeMinute;
        }

        /// <summary>
        /// 지역 코드 정보 갱신
        /// </summary>
        /// <param name="newData">새 설정 정보</param>
        public void SaveConfigInfo(ConfigLocalSettingData newData)
        {
            WriteKeyValueToLocalSetting(this.REGION_CODE, newData.RegionCode);
            WriteKeyValueToLocalSetting(this.SENDER_ID, newData.SenderID);
            WriteKeyValueToLocalSetting(this.SENDER_NAME, newData.SenderName);

            this.configData.LOCAL.RegionCode = newData.RegionCode;
            this.configData.LOCAL.SenderID = newData.SenderID;
            this.configData.LOCAL.SenderName = newData.SenderName;
        }

        /// <summary>
        /// 환경 설정 정보 갱신
        /// </summary>
        /// <param name="newData">새 설정 정보</param>
        public void SaveConfigInfo(ConfigData newData)
        {
            SaveConfigInfo(newData.DB);
            SaveConfigInfo(newData.IAGW);
            SaveConfigInfo(newData.SWR);
            SaveConfigInfo(newData.LOCAL);
        }
    }

}