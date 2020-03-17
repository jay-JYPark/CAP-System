using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using StdWarningInstallation.DataClass;

namespace StdWarningInstallation
{
    public partial class SystemConnectionSettingForm : Form
    {
        private const string IP_PATTERN = @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";
        private const string PORT_PATTERN = @"^([1-9]|[1-9][0-9]|[1-9][0-9]{2}|[1-9][0-9]{3}|[1-5][0-9]{4}|6[0-4][0-9]{3}|65[0-4][0-9]{2}|655[0-2][0-9]|6553[0-5])$";
        private const string AUTH_PATTERN = @"^[a-zA-Z0-9]{32,32}$";
        private const string SID_PATTERN = @"^[a-zA-Z0-9]{2,64}$";
        private const string ID_PATTERN = @"^[a-zA-Z0-9]{2,32}$";
        private const string PWD_PATTERN = @"^[a-zA-Z0-9]{4,32}$";
        private const string REGION_PATTERN = @"^[0-9]{10,10}$";

        public event EventHandler<IAGWConnectionTestEventArgs> NotifyIAGWConnectionTest;
        public event EventHandler<DBConnectionTestEventArgs> NotifyDBConnectionTest;
        public event EventHandler<ConfigSettingUpdateEventArgs> NotifyConfigSettingUpdate;

        private ConfigData currentConfigData = null;

        public SystemConnectionSettingForm(ConfigData configData)
        {
            InitializeComponent();

            this.currentConfigData = configData;
        }
        public void SetSystemSettingInfo(ConfigData configData)
        {
            this.currentConfigData = configData;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // 발령대 정보
            this.txtboxRegionCode.Text = this.currentConfigData.LOCAL.RegionCode;
            this.txtboxSenderID.Text = this.currentConfigData.LOCAL.SenderID;
            this.txtboxSenderName.Text = this.currentConfigData.LOCAL.SenderName;

            // 게이트웨이 접속 정보
            this.txtboxAuthCode.Text = this.currentConfigData.IAGW.AuthCode;
            this.txtboxIAGWIP.Text = this.currentConfigData.IAGW.ServerIP;
            this.txtboxIAGWPort.Text = this.currentConfigData.IAGW.ServerPort;
            this.chkboxAutoReConnection.Checked = this.currentConfigData.IAGW.UseAutoConnection;

            // 데이터베이스 접속 정보
            this.txtboxDBIP.Text = this.currentConfigData.DB.HostIP;
            this.txtboxDBServiceID.Text = this.currentConfigData.DB.ServiceID;
            this.txtboxDBID.Text = this.currentConfigData.DB.UserID;
            this.txtboxDBPassword.Text = this.currentConfigData.DB.UserPassword;

            //
        }

        /// <summary>
        /// 발령대 설정 정보 유효성 체크.
        /// </summary>
        /// <returns></returns>
        private bool CheckLocalSystemSetting()
        {
            // 발령대 정보
            if (!string.IsNullOrEmpty(this.txtboxRegionCode.Text))
            {
                bool result = Regex.IsMatch(this.txtboxRegionCode.Text, REGION_PATTERN);
                if (!result)
                {
                    MessageBox.Show("지역 코드 설정이 올바르지 않습니다.\n지역 코드 형식을 확인하세요", "설정 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            //if (!string.IsNullOrEmpty(this.txtboxSenderID.Text))
            //{
            //    bool result = Regex.IsMatch(this.txtboxSenderID.Text, SENDERID_PATTERN);
            //    if (!result)
            //    {
            //        MessageBox.Show("발령대 아이디 설정이 올바르지 않습니다.\n발령대 아이디 형식을 확인하세요", "설정 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return false;
            //    }
            //}

            //if (!string.IsNullOrEmpty(this.txtboxSenderName.Text))
            //{
            //    bool result = Regex.IsMatch(this.txtboxSenderName.Text, SENDERNAME_PATTERN);
            //    if (!result)
            //    {
            //        MessageBox.Show("발령자 정보가 올바르지 않습니다.\n발령자 정보 형식을 확인하세요", "설정 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return false;
            //    }
            //}

            return true;
        }
        /// <summary>
        /// 게이트웨이 접속 정보 유효성 체크.
        /// </summary>
        /// <returns></returns>
        private bool CheckIAGWConnectionSetting()
        {
            if (!string.IsNullOrEmpty(this.txtboxIAGWIP.Text))
            {
                bool result = Regex.IsMatch(this.txtboxIAGWIP.Text, IP_PATTERN);
                if (!result)
                {
                    MessageBox.Show("통합경보게이트웨이 접속 IP 주소가 올바르지 않습니다.\nIP 주소 형식을 확인하세요", "설정 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(this.txtboxIAGWPort.Text))
            {
                bool result = Regex.IsMatch(this.txtboxIAGWPort.Text, PORT_PATTERN);
                if (!result)
                {
                    MessageBox.Show("통합경보게이트웨이 접속 포트 번호가 올바르지 않습니다.\n포트 번호 형식을 확인하세요", "설정 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(this.txtboxAuthCode.Text))
            {
                bool result = Regex.IsMatch(this.txtboxAuthCode.Text, AUTH_PATTERN);
                if (!result)
                {
                    MessageBox.Show("통합경보게이트웨이 접속 인증 코드가 올바르지 않습니다.\n인증 코드 형식을 확인하세요", "설정 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// DB 접속 정보 유효성 체크.
        /// </summary>
        /// <returns></returns>
        private bool CheckDBConnectionSetting()
        {
            if (!string.IsNullOrEmpty(this.txtboxDBIP.Text))
            {
                // 데이터베이스 접속 정보
                bool result = Regex.IsMatch(this.txtboxDBIP.Text, IP_PATTERN);
                if (!result)
                {
                    MessageBox.Show("데이터베이스 접속 IP 주소가 올바르지 않습니다.\nIP 주소 형식을 확인하세요", "설정 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(this.txtboxDBID.Text))
            {
                bool result = Regex.IsMatch(this.txtboxDBServiceID.Text, SID_PATTERN);
                if (!result)
                {
                    MessageBox.Show("데이터베이스 접속 서비스 ID가 올바르지 않습니다.\n서비스 ID 형식을 확인하세요", "설정 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(this.txtboxDBID.Text))
            {
                bool result = Regex.IsMatch(this.txtboxDBID.Text, ID_PATTERN);
                if (!result)
                {
                    MessageBox.Show("데이터베이스 접속 ID가 올바르지 않습니다.\nID 형식을 확인하세요", "설정 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(this.txtboxDBPassword.Text))
            {
                bool result = Regex.IsMatch(this.txtboxDBPassword.Text, PWD_PATTERN);
                if (!result)
                {
                    MessageBox.Show("데이터베이스 접속 패스워드가 올바르지 않습니다.\n패스워드 형식을 확인하세요", "설정 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// 발령대 설정 정보 저장.
        /// </summary>
        /// <returns></returns>
        private bool SaveLocalSystemSetting()
        {
            // 발령대 정보
            if (!CheckLocalSystemSetting())
            {
                return false;
            }

            if (this.currentConfigData.LOCAL.RegionCode != this.txtboxRegionCode.Text ||
                this.currentConfigData.LOCAL.SenderID != this.txtboxSenderID.Text ||
                this.currentConfigData.LOCAL.SenderName != this.txtboxSenderName.Text)
            {
                // 발령대 정보
                this.currentConfigData.LOCAL.RegionCode = this.txtboxRegionCode.Text;
                this.currentConfigData.LOCAL.SenderID = this.txtboxSenderID.Text;
                this.currentConfigData.LOCAL.SenderName = this.txtboxSenderName.Text;

                if (this.NotifyConfigSettingUpdate != null)
                {
                    this.NotifyConfigSettingUpdate(this, new ConfigSettingUpdateEventArgs(this.currentConfigData));
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// 게이트웨이 접속 정보 저장.
        /// </summary>
        /// <returns></returns>
        private bool SaveIAGWConnectionSetting()
        {
            if (!CheckIAGWConnectionSetting())
            {
                return false;
            }

            if (this.currentConfigData.IAGW.AuthCode != this.txtboxAuthCode.Text ||
                this.currentConfigData.IAGW.ServerIP != this.txtboxIAGWIP.Text ||
                this.currentConfigData.IAGW.ServerPort != this.txtboxIAGWPort.Text ||
                this.currentConfigData.IAGW.UseAutoConnection != this.chkboxAutoReConnection.Checked)
            {
                if (this.NotifyConfigSettingUpdate != null)
                {
                    this.currentConfigData.IAGW.AuthCode = this.txtboxAuthCode.Text;
                    this.currentConfigData.IAGW.ServerIP = this.txtboxIAGWIP.Text;
                    this.currentConfigData.IAGW.ServerPort = this.txtboxIAGWPort.Text;
                    this.currentConfigData.IAGW.UseAutoConnection = this.chkboxAutoReConnection.Checked;

                    this.NotifyConfigSettingUpdate(this, new ConfigSettingUpdateEventArgs(this.currentConfigData));
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
        /// <summary>
        /// DB 접속 정보 저장.
        /// </summary>
        /// <returns></returns>
        private bool SaveDBConnectionSetting()
        {
            if (!CheckDBConnectionSetting())
            {
                return false;
            }

            if (this.currentConfigData.DB.HostIP != this.txtboxDBIP.Text ||
                this.currentConfigData.DB.ServiceID != this.txtboxDBServiceID.Text ||
                this.currentConfigData.DB.UserID != this.txtboxDBID.Text ||
                this.currentConfigData.DB.UserPassword != this.txtboxDBPassword.Text)
            {
                if (this.NotifyConfigSettingUpdate != null)
                {
                    this.currentConfigData.DB.HostIP = this.txtboxDBIP.Text;
                    this.currentConfigData.DB.ServiceID = this.txtboxDBServiceID.Text;
                    this.currentConfigData.DB.UserID = this.txtboxDBID.Text;
                    this.currentConfigData.DB.UserPassword = this.txtboxDBPassword.Text;

                    this.NotifyConfigSettingUpdate(this, new ConfigSettingUpdateEventArgs(this.currentConfigData));

                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// [통신 연결] 버튼 클릭 이벤트 핸들러.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIAGWConnectionTest_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtboxIAGWIP.Text) || string.IsNullOrEmpty(this.txtboxIAGWPort.Text))
                {
                    MessageBox.Show("입력 항목을 확인하세요", "통합경보게이트웨이 연결", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                bool rst = Regex.IsMatch(this.txtboxIAGWIP.Text, IP_PATTERN);
                if (!rst)
                {
                    MessageBox.Show("IP 형식을 확인하세요", "통합경보게이트웨이 연결", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                rst = Regex.IsMatch(this.txtboxIAGWPort.Text, PORT_PATTERN);
                if (!rst)
                {
                    MessageBox.Show("PORT 형식을 확인하세요", "통합경보게이트웨이 연결", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                bool isConnection = false;
                Button btn = sender as Button;
                if (btn.Text.Contains("연결"))
                {
                    isConnection = true;
                }
                if (this.NotifyIAGWConnectionTest != null)
                {
                    this.NotifyIAGWConnectionTest(this, new IAGWConnectionTestEventArgs(isConnection, this.txtboxIAGWIP.Text, this.txtboxIAGWPort.Text));
                }
            }
            catch (Exception ex)
            {
                FileLogManager.GetInstance().WriteLog("[SystemConnectionSettingForm] btnIAGWConnectionTest_Click( Exception=[" + ex.ToString() + "] )");

                MessageBox.Show("통신연결 처리 중에 예외가 발생하였습니다.\n\nError=\n" + ex.ToString()
                                , "통합경보게이트웨이 연결", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnDBConnectionTest_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.NotifyDBConnectionTest != null)
                {
                    ConfigDBData dbInfo = new ConfigDBData();
                    dbInfo.HostIP = this.txtboxDBIP.Text;
                    dbInfo.ServiceID = this.txtboxDBServiceID.Text;
                    dbInfo.UserID = this.txtboxDBID.Text;
                    dbInfo.UserPassword = this.txtboxDBPassword.Text;

                    this.NotifyDBConnectionTest(this, new DBConnectionTestEventArgs(dbInfo));
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[SystemConnectionSettingForm] btnDBConnectionTest_Click ( Exception Occured!!! " + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[SystemConnectionSettingForm] btnDBConnectionTest_Click( Exception=[" + ex.ToString() + "] )");

                MessageBox.Show("접속 테스트 처리 중에 예외가 발생 하였습니다. (2)", "데이터베이스 접속 시험", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /// <summary>
        /// [설정 저장] 버튼 클릭.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckLocalSystemSetting())
                {
                    return;
                }
                if (!CheckIAGWConnectionSetting())
                {
                    return;
                }
                if (!CheckDBConnectionSetting())
                {
                    return;
                }

                if (this.NotifyConfigSettingUpdate != null)
                {
                    // 발령대 정보
                    this.currentConfigData.LOCAL.RegionCode = this.txtboxRegionCode.Text;
                    this.currentConfigData.LOCAL.SenderID = this.txtboxSenderID.Text;
                    this.currentConfigData.LOCAL.SenderName = this.txtboxSenderName.Text;

                    // 통합경보게이트웨이 정보
                    this.currentConfigData.IAGW.AuthCode = this.txtboxAuthCode.Text;
                    this.currentConfigData.IAGW.ServerIP = this.txtboxIAGWIP.Text;
                    this.currentConfigData.IAGW.ServerPort = this.txtboxIAGWPort.Text;
                    this.currentConfigData.IAGW.UseAutoConnection = this.chkboxAutoReConnection.Checked;

                    // DB정보
                    this.currentConfigData.DB.HostIP = this.txtboxDBIP.Text;
                    this.currentConfigData.DB.ServiceID = this.txtboxDBServiceID.Text;
                    this.currentConfigData.DB.UserID = this.txtboxDBID.Text;
                    this.currentConfigData.DB.UserPassword = this.txtboxDBPassword.Text;

                    this.NotifyConfigSettingUpdate(this, new ConfigSettingUpdateEventArgs(this.currentConfigData));
                }
            }
            catch (Exception ex)
            {
                FileLogManager.GetInstance().WriteLog("[SystemConnectionSettingForm] btnSave_Click( Exception=[" + ex.ToString() + "] )");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabCtrlMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabCtrl = sender as TabControl;
        }

        private void tabCtrlMain_Selecting(object sender, TabControlCancelEventArgs e)
        {
            TabControl tabCtrl = sender as TabControl;

            if (this.txtboxRegionCode.Text != this.currentConfigData.LOCAL.RegionCode ||
                this.txtboxSenderID.Text != this.currentConfigData.LOCAL.SenderID ||
                this.txtboxSenderName.Text != this.currentConfigData.LOCAL.SenderName)
            {
                DialogResult answer = MessageBox.Show("발령대 정보에 변경된 설정이 있습니다. 변경 사항을 저장 하시겠습니까?", "시스템 설정 변경", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (answer == System.Windows.Forms.DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                else if (answer == System.Windows.Forms.DialogResult.Yes)
                {
                    bool isSaved = SaveLocalSystemSetting();
                    if (!isSaved)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    // 설정값을 원래대로 돌리고 탭 선택 변경 계속
                    this.txtboxRegionCode.Text = this.currentConfigData.LOCAL.RegionCode;
                    this.txtboxSenderID.Text = this.currentConfigData.LOCAL.SenderID;
                    this.txtboxSenderName.Text = this.currentConfigData.LOCAL.SenderName;
                }
            }
            else if (this.txtboxAuthCode.Text != this.currentConfigData.IAGW.AuthCode ||
                    this.txtboxIAGWIP.Text != this.currentConfigData.IAGW.ServerIP ||
                    this.txtboxIAGWPort.Text != this.currentConfigData.IAGW.ServerPort)
            {
                DialogResult answer = MessageBox.Show("통합경보게이트웨이 접속 정보에 변경된 설정이 있습니다. 변경 사항을 저장 하시겠습니까?", "시스템 설정 변경", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (answer == System.Windows.Forms.DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                else if (answer == System.Windows.Forms.DialogResult.Yes)
                {
                    bool isSaved = SaveIAGWConnectionSetting();
                    if (!isSaved)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    // 설정값을 원래대로 돌리고 탭 선택 변경 계속
                    this.txtboxAuthCode.Text = this.currentConfigData.IAGW.AuthCode;
                    this.txtboxIAGWIP.Text = this.currentConfigData.IAGW.ServerIP;
                    this.txtboxIAGWPort.Text = this.currentConfigData.IAGW.ServerPort;
                }
            }
            else if (this.txtboxDBIP.Text != this.currentConfigData.DB.HostIP ||
                this.txtboxDBServiceID.Text != this.currentConfigData.DB.ServiceID ||
                this.txtboxDBID.Text != this.currentConfigData.DB.UserID ||
                this.txtboxDBPassword.Text != this.currentConfigData.DB.UserPassword)
            {
                DialogResult answer = MessageBox.Show("데이터베이스 접속 정보에 변경된 설정이 있습니다. 변경 사항을 저장 하시겠습니까?", "시스템 설정 변경", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                if (answer == System.Windows.Forms.DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                else if (answer == System.Windows.Forms.DialogResult.Yes)
                {
                    bool isSaved = SaveDBConnectionSetting();
                    if (!isSaved)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
                else
                {
                    // 설정값을 원래대로 돌리고 탭 선택 변경 계속
                    this.txtboxDBIP.Text = this.currentConfigData.DB.HostIP;
                    this.txtboxDBServiceID.Text = this.currentConfigData.DB.ServiceID;
                    this.txtboxDBID.Text = this.currentConfigData.DB.UserID;
                    this.txtboxDBPassword.Text = this.currentConfigData.DB.UserPassword;
                }
            }
            else
            {
            }
        }

        private void tabCtrlMain_TabIndexChanged(object sender, EventArgs e)
        {
            TabControl tabCtrl = sender as TabControl;

        }
    }

    public class IAGWConnectionTestEventArgs : EventArgs
    {
        private bool isConnectState = false;
        public bool IsConnectState
        {
            get { return isConnectState; }
            set { isConnectState = value; }
        }
        private string ip = string.Empty;
        public string IP
        {
            get { return ip; }
            set { ip = value; }
        }
        private string port = string.Empty;
        public string Port
        {
            get { return port; }
            set { port = value; }
        }

        public IAGWConnectionTestEventArgs(bool connectState, string ipAddress, string portNo)
        {
            this.isConnectState = connectState;
            this.ip = ipAddress;
            this.port = portNo;
        }
    }
    public class DBConnectionTestEventArgs : EventArgs
    {
        private ConfigDBData dbInfo = new ConfigDBData();
        public ConfigDBData DbInfo
        {
            get { return dbInfo; }
            set { dbInfo = value; }
        }

        public DBConnectionTestEventArgs(ConfigDBData newInfo)
        {
            this.dbInfo.DeepCopyFrom(newInfo);
        }
    }

    public class ConfigSettingUpdateEventArgs : EventArgs
    {
        private ConfigData settingInfo = new ConfigData();
        public ConfigData SettingInfo
        {
            get { return settingInfo; }
            set { settingInfo = value; }
        }

        public ConfigSettingUpdateEventArgs(ConfigData newInfo)
        {
            this.settingInfo.DeepCopyFrom(newInfo);
        }
    }
}
