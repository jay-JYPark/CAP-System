using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StdWarningInstallation.DataClass
{
    class ConfigInfo
    {
    }

    /// <summary>
    /// 설정 데이터 클래스
    /// </summary>
    public class ConfigData
    {
        private ConfigDBData database = null;
        private ConfigIAGWData gateway;
        private ConfigSWRData specialWeatherReport;
        private ConfigLocalSettingData localSetting;

        public ConfigDBData DB
        {
            get { return this.database; }
            set { this.database = value; }
        }

        public ConfigIAGWData IAGW
        {
            get { return this.gateway; }
            set { this.gateway = value; }
        }

        public ConfigSWRData SWR
        {
            get { return this.specialWeatherReport; }
            set { this.specialWeatherReport = value; }
        }

        public ConfigLocalSettingData LOCAL
        {
            get { return this.localSetting; }
            set { this.localSetting = value; }
        }

        public void DeepCopyFrom(ConfigData src)
        {
            System.Diagnostics.Debug.Assert(src != null);

            if (src == null)
            {
                return;
            }

            this.database.DeepCopyFrom(src.database);
            this.gateway.DeepCopyFrom(src.gateway);
            this.specialWeatherReport.DeepCopyFrom(src.specialWeatherReport);
            this.localSetting.DeepCopyFrom(src.localSetting);
        }

        public ConfigData()
        {
            this.database = new ConfigDBData();
            this.gateway = new ConfigIAGWData();
            this.specialWeatherReport = new ConfigSWRData();
            this.localSetting = new ConfigLocalSettingData();
        }
    }

    public class ConfigDBData
    {
        private string hostIP = string.Empty;
        public string HostIP
        {
            get { return hostIP; }
            set { hostIP = value; }
        }

        private string userID = string.Empty;
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        private string userPassword = string.Empty;
        public string UserPassword
        {
            get { return userPassword; }
            set { userPassword = value; }
        }

        private string serviceID = string.Empty;
        public string ServiceID
        {
            get { return serviceID; }
            set { serviceID = value; }
        }

        public void DeepCopyFrom(ConfigDBData src)
        {
            System.Diagnostics.Debug.Assert(src != null);

            if (src == null)
            {
                return;
            }

            this.hostIP = src.hostIP;
            this.userID = src.userID;
            this.userPassword = src.userPassword;
            this.serviceID = src.serviceID;
        }
    }
    public class ConfigIAGWData
    {
        private string authenticationCode = string.Empty;
        public string AuthCode
        {
            get { return authenticationCode; }
            set { authenticationCode = value; }
        }

        private string serverIP = string.Empty;
        public string ServerIP
        {
            get { return serverIP; }
            set { serverIP = value; }
        }

        private string serverPort = string.Empty;
        public string ServerPort
        {
            get { return serverPort; }
            set { serverPort = value; }
        }
        private bool useAutoConnection = false;
        public bool UseAutoConnection
        {
            get { return useAutoConnection; }
            set { useAutoConnection = value; }
        }

        public void DeepCopyFrom(ConfigIAGWData src)
        {
            System.Diagnostics.Debug.Assert(src != null);

            if (src == null)
            {
                return;
            }

            this.authenticationCode = src.authenticationCode;
            this.serverIP = src.serverIP;
            this.serverPort = src.serverPort;
            this.UseAutoConnection = src.useAutoConnection;
        }
    }

    public class ConfigSWRData
    {
        private bool useService = false;
        public bool UseService
        {
          get { return useService; }
          set { useService = value; }
        }

        private string serviceKey = string.Empty;
        public string ServiceKey
        {
            get { return serviceKey; }
            set { serviceKey = value; }
        }
        private int cycleTimeMinute = int.MinValue;
        public int CycleTimeMinute
        {
            get { return cycleTimeMinute; }
            set { cycleTimeMinute = value; }
        }

        public void DeepCopyFrom(ConfigSWRData src)
        {
            System.Diagnostics.Debug.Assert(src != null);

            if (src == null)
            {
                return;
            }

            this.useService = src.useService;
            this.serviceKey = src.serviceKey;
            this.cycleTimeMinute = src.cycleTimeMinute;
        }
    }

    public class ConfigLocalSettingData
    {
        private string regionCode = string.Empty;
        public string RegionCode
        {
            get { return regionCode; }
            set { regionCode = value; }
        }

        private string senderID = string.Empty;
        public string SenderID
        {
            get { return senderID; }
            set { senderID = value; }
        }
        private string senderName = string.Empty;
        public string SenderName
        {
            get { return senderName; }
            set { senderName = value; }
        }

        public void DeepCopyFrom(ConfigLocalSettingData src)
        {
            System.Diagnostics.Debug.Assert(src != null);

            if (src == null)
            {
                return;
            }

            this.regionCode = src.regionCode;
            this.senderID = src.senderID;
            this.senderName = src.senderName;
        }
    }

    public class ConfigSenderInfo
    {
        private string identifier = string.Empty;
        public string ID
        {
            get { return identifier; }
            set { identifier = value; }
        }
        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public ConfigSenderInfo()
        {
        }
        public ConfigSenderInfo(string identifier, string name)
        {
            this.identifier = identifier;
            this.name = name;
        }
    }
}
