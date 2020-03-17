namespace StdWarningInstallation
{
    partial class SystemConnectionSettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemConnectionSettingForm));
            this.btnClose = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.tabCtrlMain = new System.Windows.Forms.TabControl();
            this.tpLocalSystemSetting = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtboxSenderName = new Adeng.Framework.Ctrl.TextBoxEx();
            this.txtboxSenderID = new Adeng.Framework.Ctrl.TextBoxEx();
            this.txtboxRegionCode = new Adeng.Framework.Ctrl.TextBoxEx();
            this.tpIAGWConnection = new System.Windows.Forms.TabPage();
            this.pnlIAGW = new System.Windows.Forms.Panel();
            this.chkboxAutoReConnection = new System.Windows.Forms.CheckBox();
            this.pboxAuthCheckState = new System.Windows.Forms.PictureBox();
            this.txtboxAuthCode = new Adeng.Framework.Ctrl.TextBoxEx();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDisconnection = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnConnection = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.txtboxIAGWIP = new Adeng.Framework.Ctrl.TextBoxEx();
            this.txtboxIAGWPort = new Adeng.Framework.Ctrl.TextBoxEx();
            this.tpDBConnection = new System.Windows.Forms.TabPage();
            this.pnlSoundTypeBack = new System.Windows.Forms.Panel();
            this.btnDBConnectionTest = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtboxDBPassword = new Adeng.Framework.Ctrl.TextBoxEx();
            this.txtboxDBID = new Adeng.Framework.Ctrl.TextBoxEx();
            this.txtboxDBServiceID = new Adeng.Framework.Ctrl.TextBoxEx();
            this.txtboxDBIP = new Adeng.Framework.Ctrl.TextBoxEx();
            this.lblDescription = new System.Windows.Forms.Label();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnSave = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.tabCtrlMain.SuspendLayout();
            this.tpLocalSystemSetting.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tpIAGWConnection.SuspendLayout();
            this.pnlIAGW.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pboxAuthCheckState)).BeginInit();
            this.tpDBConnection.SuspendLayout();
            this.pnlSoundTypeBack.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.ChkValue = false;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImgDisable = null;
            this.btnClose.ImgHover = null;
            this.btnClose.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnClose.ImgSelect")));
            this.btnClose.ImgStatusEvent = null;
            this.btnClose.ImgStatusNormal = null;
            this.btnClose.ImgStatusOffsetX = 2;
            this.btnClose.ImgStatusOffsetY = 0;
            this.btnClose.ImgStretch = true;
            this.btnClose.IsImgStatusNormal = true;
            this.btnClose.Location = new System.Drawing.Point(333, 276);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 36);
            this.btnClose.TabIndex = 48;
            this.btnClose.Text = "닫기";
            this.btnClose.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnClose.UseChecked = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tabCtrlMain
            // 
            this.tabCtrlMain.Controls.Add(this.tpLocalSystemSetting);
            this.tabCtrlMain.Controls.Add(this.tpIAGWConnection);
            this.tabCtrlMain.Controls.Add(this.tpDBConnection);
            this.tabCtrlMain.Location = new System.Drawing.Point(7, 48);
            this.tabCtrlMain.Name = "tabCtrlMain";
            this.tabCtrlMain.SelectedIndex = 0;
            this.tabCtrlMain.Size = new System.Drawing.Size(428, 220);
            this.tabCtrlMain.TabIndex = 50;
            this.tabCtrlMain.SelectedIndexChanged += new System.EventHandler(this.tabCtrlMain_SelectedIndexChanged);
            this.tabCtrlMain.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabCtrlMain_Selecting);
            this.tabCtrlMain.TabIndexChanged += new System.EventHandler(this.tabCtrlMain_TabIndexChanged);
            // 
            // tpLocalSystemSetting
            // 
            this.tpLocalSystemSetting.Controls.Add(this.panel1);
            this.tpLocalSystemSetting.Location = new System.Drawing.Point(4, 22);
            this.tpLocalSystemSetting.Name = "tpLocalSystemSetting";
            this.tpLocalSystemSetting.Size = new System.Drawing.Size(420, 194);
            this.tpLocalSystemSetting.TabIndex = 2;
            this.tpLocalSystemSetting.Text = "발령대 정보";
            this.tpLocalSystemSetting.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.txtboxSenderName);
            this.panel1.Controls.Add(this.txtboxSenderID);
            this.panel1.Controls.Add(this.txtboxRegionCode);
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(408, 182);
            this.panel1.TabIndex = 22;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label9.Location = new System.Drawing.Point(24, 91);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 23);
            this.label9.TabIndex = 36;
            this.label9.Text = "발령자 이름";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label10.Location = new System.Drawing.Point(24, 64);
            this.label10.Margin = new System.Windows.Forms.Padding(0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 23);
            this.label10.TabIndex = 36;
            this.label10.Text = "발령대 아이디";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label11.Location = new System.Drawing.Point(24, 26);
            this.label11.Margin = new System.Windows.Forms.Padding(0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(92, 23);
            this.label11.TabIndex = 36;
            this.label11.Text = "행정동 코드";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtboxSenderName
            // 
            this.txtboxSenderName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtboxSenderName.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxSenderName.Location = new System.Drawing.Point(119, 91);
            this.txtboxSenderName.MaxLength = 15;
            this.txtboxSenderName.Name = "txtboxSenderName";
            this.txtboxSenderName.Size = new System.Drawing.Size(205, 25);
            this.txtboxSenderName.TabIndex = 29;
            // 
            // txtboxSenderID
            // 
            this.txtboxSenderID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtboxSenderID.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxSenderID.Location = new System.Drawing.Point(119, 64);
            this.txtboxSenderID.MaxLength = 10;
            this.txtboxSenderID.Name = "txtboxSenderID";
            this.txtboxSenderID.Size = new System.Drawing.Size(205, 25);
            this.txtboxSenderID.TabIndex = 27;
            // 
            // txtboxRegionCode
            // 
            this.txtboxRegionCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtboxRegionCode.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxRegionCode.Location = new System.Drawing.Point(119, 25);
            this.txtboxRegionCode.MaxLength = 15;
            this.txtboxRegionCode.Name = "txtboxRegionCode";
            this.txtboxRegionCode.Size = new System.Drawing.Size(100, 25);
            this.txtboxRegionCode.TabIndex = 25;
            // 
            // tpIAGWConnection
            // 
            this.tpIAGWConnection.Controls.Add(this.pnlIAGW);
            this.tpIAGWConnection.Location = new System.Drawing.Point(4, 22);
            this.tpIAGWConnection.Name = "tpIAGWConnection";
            this.tpIAGWConnection.Padding = new System.Windows.Forms.Padding(3);
            this.tpIAGWConnection.Size = new System.Drawing.Size(420, 194);
            this.tpIAGWConnection.TabIndex = 0;
            this.tpIAGWConnection.Tag = "";
            this.tpIAGWConnection.Text = "통합경보게이트웨이 접속";
            this.tpIAGWConnection.UseVisualStyleBackColor = true;
            // 
            // pnlIAGW
            // 
            this.pnlIAGW.AutoScroll = true;
            this.pnlIAGW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.pnlIAGW.Controls.Add(this.chkboxAutoReConnection);
            this.pnlIAGW.Controls.Add(this.pboxAuthCheckState);
            this.pnlIAGW.Controls.Add(this.txtboxAuthCode);
            this.pnlIAGW.Controls.Add(this.label1);
            this.pnlIAGW.Controls.Add(this.label3);
            this.pnlIAGW.Controls.Add(this.label2);
            this.pnlIAGW.Controls.Add(this.btnDisconnection);
            this.pnlIAGW.Controls.Add(this.btnConnection);
            this.pnlIAGW.Controls.Add(this.txtboxIAGWIP);
            this.pnlIAGW.Controls.Add(this.txtboxIAGWPort);
            this.pnlIAGW.Location = new System.Drawing.Point(6, 6);
            this.pnlIAGW.Name = "pnlIAGW";
            this.pnlIAGW.Size = new System.Drawing.Size(408, 182);
            this.pnlIAGW.TabIndex = 20;
            // 
            // chkboxAutoReConnection
            // 
            this.chkboxAutoReConnection.AutoSize = true;
            this.chkboxAutoReConnection.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.chkboxAutoReConnection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.chkboxAutoReConnection.Location = new System.Drawing.Point(27, 137);
            this.chkboxAutoReConnection.Name = "chkboxAutoReConnection";
            this.chkboxAutoReConnection.Size = new System.Drawing.Size(118, 19);
            this.chkboxAutoReConnection.TabIndex = 45;
            this.chkboxAutoReConnection.Text = "자동 재접속 사용";
            this.chkboxAutoReConnection.UseVisualStyleBackColor = true;
            // 
            // pboxAuthCheckState
            // 
            this.pboxAuthCheckState.Image = ((System.Drawing.Image)(resources.GetObject("pboxAuthCheckState.Image")));
            this.pboxAuthCheckState.Location = new System.Drawing.Point(356, 21);
            this.pboxAuthCheckState.Name = "pboxAuthCheckState";
            this.pboxAuthCheckState.Size = new System.Drawing.Size(24, 24);
            this.pboxAuthCheckState.TabIndex = 44;
            this.pboxAuthCheckState.TabStop = false;
            this.pboxAuthCheckState.Visible = false;
            // 
            // txtboxAuthCode
            // 
            this.txtboxAuthCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtboxAuthCode.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxAuthCode.Location = new System.Drawing.Point(90, 21);
            this.txtboxAuthCode.MaxLength = 32;
            this.txtboxAuthCode.Name = "txtboxAuthCode";
            this.txtboxAuthCode.Size = new System.Drawing.Size(263, 25);
            this.txtboxAuthCode.TabIndex = 43;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label1.Location = new System.Drawing.Point(24, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 23);
            this.label1.TabIndex = 35;
            this.label1.Text = "인증 코드";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label3.Location = new System.Drawing.Point(24, 78);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 23);
            this.label3.TabIndex = 35;
            this.label3.Text = "Port";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label2.Location = new System.Drawing.Point(24, 49);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 23);
            this.label2.TabIndex = 35;
            this.label2.Text = "IP";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDisconnection
            // 
            this.btnDisconnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisconnection.ChkValue = false;
            this.btnDisconnection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDisconnection.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDisconnection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(47)))));
            this.btnDisconnection.Image = ((System.Drawing.Image)(resources.GetObject("btnDisconnection.Image")));
            this.btnDisconnection.ImgDisable = null;
            this.btnDisconnection.ImgHover = null;
            this.btnDisconnection.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnDisconnection.ImgSelect")));
            this.btnDisconnection.ImgStatusEvent = null;
            this.btnDisconnection.ImgStatusNormal = null;
            this.btnDisconnection.ImgStatusOffsetX = 2;
            this.btnDisconnection.ImgStatusOffsetY = 0;
            this.btnDisconnection.ImgStretch = true;
            this.btnDisconnection.IsImgStatusNormal = true;
            this.btnDisconnection.Location = new System.Drawing.Point(299, 137);
            this.btnDisconnection.Name = "btnDisconnection";
            this.btnDisconnection.Size = new System.Drawing.Size(100, 36);
            this.btnDisconnection.TabIndex = 19;
            this.btnDisconnection.Text = "통신 종료";
            this.btnDisconnection.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnDisconnection.UseChecked = false;
            this.btnDisconnection.Click += new System.EventHandler(this.btnIAGWConnectionTest_Click);
            // 
            // btnConnection
            // 
            this.btnConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnection.ChkValue = false;
            this.btnConnection.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnConnection.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnConnection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(47)))));
            this.btnConnection.Image = ((System.Drawing.Image)(resources.GetObject("btnConnection.Image")));
            this.btnConnection.ImgDisable = null;
            this.btnConnection.ImgHover = null;
            this.btnConnection.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnConnection.ImgSelect")));
            this.btnConnection.ImgStatusEvent = null;
            this.btnConnection.ImgStatusNormal = null;
            this.btnConnection.ImgStatusOffsetX = 2;
            this.btnConnection.ImgStatusOffsetY = 0;
            this.btnConnection.ImgStretch = true;
            this.btnConnection.IsImgStatusNormal = true;
            this.btnConnection.Location = new System.Drawing.Point(193, 137);
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Size = new System.Drawing.Size(100, 36);
            this.btnConnection.TabIndex = 19;
            this.btnConnection.Text = "통신 연결";
            this.btnConnection.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnConnection.UseChecked = false;
            this.btnConnection.Click += new System.EventHandler(this.btnIAGWConnectionTest_Click);
            // 
            // txtboxIAGWIP
            // 
            this.txtboxIAGWIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtboxIAGWIP.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxIAGWIP.Location = new System.Drawing.Point(90, 49);
            this.txtboxIAGWIP.MaxLength = 15;
            this.txtboxIAGWIP.Name = "txtboxIAGWIP";
            this.txtboxIAGWIP.Size = new System.Drawing.Size(134, 25);
            this.txtboxIAGWIP.TabIndex = 34;
            // 
            // txtboxIAGWPort
            // 
            this.txtboxIAGWPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtboxIAGWPort.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxIAGWPort.Location = new System.Drawing.Point(90, 78);
            this.txtboxIAGWPort.MaxLength = 5;
            this.txtboxIAGWPort.Name = "txtboxIAGWPort";
            this.txtboxIAGWPort.Size = new System.Drawing.Size(134, 25);
            this.txtboxIAGWPort.TabIndex = 29;
            // 
            // tpDBConnection
            // 
            this.tpDBConnection.Controls.Add(this.pnlSoundTypeBack);
            this.tpDBConnection.Location = new System.Drawing.Point(4, 22);
            this.tpDBConnection.Name = "tpDBConnection";
            this.tpDBConnection.Padding = new System.Windows.Forms.Padding(3);
            this.tpDBConnection.Size = new System.Drawing.Size(420, 194);
            this.tpDBConnection.TabIndex = 1;
            this.tpDBConnection.Tag = "";
            this.tpDBConnection.Text = "데이터베이스 접속";
            this.tpDBConnection.UseVisualStyleBackColor = true;
            // 
            // pnlSoundTypeBack
            // 
            this.pnlSoundTypeBack.AutoScroll = true;
            this.pnlSoundTypeBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.pnlSoundTypeBack.Controls.Add(this.btnDBConnectionTest);
            this.pnlSoundTypeBack.Controls.Add(this.label7);
            this.pnlSoundTypeBack.Controls.Add(this.label6);
            this.pnlSoundTypeBack.Controls.Add(this.label5);
            this.pnlSoundTypeBack.Controls.Add(this.label4);
            this.pnlSoundTypeBack.Controls.Add(this.txtboxDBPassword);
            this.pnlSoundTypeBack.Controls.Add(this.txtboxDBID);
            this.pnlSoundTypeBack.Controls.Add(this.txtboxDBServiceID);
            this.pnlSoundTypeBack.Controls.Add(this.txtboxDBIP);
            this.pnlSoundTypeBack.Location = new System.Drawing.Point(6, 6);
            this.pnlSoundTypeBack.Name = "pnlSoundTypeBack";
            this.pnlSoundTypeBack.Size = new System.Drawing.Size(408, 182);
            this.pnlSoundTypeBack.TabIndex = 21;
            // 
            // btnDBConnectionTest
            // 
            this.btnDBConnectionTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDBConnectionTest.ChkValue = false;
            this.btnDBConnectionTest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDBConnectionTest.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDBConnectionTest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(47)))));
            this.btnDBConnectionTest.Image = ((System.Drawing.Image)(resources.GetObject("btnDBConnectionTest.Image")));
            this.btnDBConnectionTest.ImgDisable = null;
            this.btnDBConnectionTest.ImgHover = null;
            this.btnDBConnectionTest.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnDBConnectionTest.ImgSelect")));
            this.btnDBConnectionTest.ImgStatusEvent = null;
            this.btnDBConnectionTest.ImgStatusNormal = null;
            this.btnDBConnectionTest.ImgStatusOffsetX = 2;
            this.btnDBConnectionTest.ImgStatusOffsetY = 0;
            this.btnDBConnectionTest.ImgStretch = true;
            this.btnDBConnectionTest.IsImgStatusNormal = true;
            this.btnDBConnectionTest.Location = new System.Drawing.Point(299, 137);
            this.btnDBConnectionTest.Name = "btnDBConnectionTest";
            this.btnDBConnectionTest.Size = new System.Drawing.Size(100, 36);
            this.btnDBConnectionTest.TabIndex = 37;
            this.btnDBConnectionTest.Text = "연결 테스트";
            this.btnDBConnectionTest.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnDBConnectionTest.UseChecked = false;
            this.btnDBConnectionTest.Click += new System.EventHandler(this.btnDBConnectionTest_Click);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label7.Location = new System.Drawing.Point(24, 105);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 23);
            this.label7.TabIndex = 36;
            this.label7.Text = "비밀번호";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label6.Location = new System.Drawing.Point(24, 79);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 23);
            this.label6.TabIndex = 36;
            this.label6.Text = "아이디";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label5.Location = new System.Drawing.Point(24, 52);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 23);
            this.label5.TabIndex = 36;
            this.label5.Text = "서비스 아이디";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label4.Location = new System.Drawing.Point(24, 26);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 23);
            this.label4.TabIndex = 36;
            this.label4.Text = "IP";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtboxDBPassword
            // 
            this.txtboxDBPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtboxDBPassword.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxDBPassword.Location = new System.Drawing.Point(119, 106);
            this.txtboxDBPassword.MaxLength = 15;
            this.txtboxDBPassword.Name = "txtboxDBPassword";
            this.txtboxDBPassword.Size = new System.Drawing.Size(100, 25);
            this.txtboxDBPassword.TabIndex = 31;
            // 
            // txtboxDBID
            // 
            this.txtboxDBID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtboxDBID.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxDBID.Location = new System.Drawing.Point(119, 79);
            this.txtboxDBID.MaxLength = 15;
            this.txtboxDBID.Name = "txtboxDBID";
            this.txtboxDBID.Size = new System.Drawing.Size(100, 25);
            this.txtboxDBID.TabIndex = 29;
            // 
            // txtboxDBServiceID
            // 
            this.txtboxDBServiceID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtboxDBServiceID.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxDBServiceID.Location = new System.Drawing.Point(119, 52);
            this.txtboxDBServiceID.MaxLength = 10;
            this.txtboxDBServiceID.Name = "txtboxDBServiceID";
            this.txtboxDBServiceID.Size = new System.Drawing.Size(100, 25);
            this.txtboxDBServiceID.TabIndex = 27;
            // 
            // txtboxDBIP
            // 
            this.txtboxDBIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtboxDBIP.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxDBIP.Location = new System.Drawing.Point(119, 25);
            this.txtboxDBIP.MaxLength = 15;
            this.txtboxDBIP.Name = "txtboxDBIP";
            this.txtboxDBIP.Size = new System.Drawing.Size(100, 25);
            this.txtboxDBIP.TabIndex = 25;
            // 
            // lblDescription
            // 
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblDescription.Image = ((System.Drawing.Image)(resources.GetObject("lblDescription.Image")));
            this.lblDescription.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDescription.Location = new System.Drawing.Point(0, 0);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Padding = new System.Windows.Forms.Padding(14, 0, 7, 0);
            this.lblDescription.Size = new System.Drawing.Size(442, 40);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "시스템 설정 정보를 확인/수정합니다.";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlTop
            // 
            this.pnlTop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlTop.BackgroundImage")));
            this.pnlTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlTop.Controls.Add(this.lblDescription);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(442, 40);
            this.pnlTop.TabIndex = 47;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.ChkValue = false;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImgDisable = null;
            this.btnSave.ImgHover = null;
            this.btnSave.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnSave.ImgSelect")));
            this.btnSave.ImgStatusEvent = null;
            this.btnSave.ImgStatusNormal = null;
            this.btnSave.ImgStatusOffsetX = 2;
            this.btnSave.ImgStatusOffsetY = 0;
            this.btnSave.ImgStretch = true;
            this.btnSave.IsImgStatusNormal = true;
            this.btnSave.Location = new System.Drawing.Point(226, 276);
            this.btnSave.Margin = new System.Windows.Forms.Padding(0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 36);
            this.btnSave.TabIndex = 48;
            this.btnSave.Text = "설정 저장";
            this.btnSave.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnSave.UseChecked = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // SystemConnectionSettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 320);
            this.ControlBox = false;
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabCtrlMain);
            this.Controls.Add(this.pnlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SystemConnectionSettingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "시스템 설정 관리";
            this.tabCtrlMain.ResumeLayout(false);
            this.tpLocalSystemSetting.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tpIAGWConnection.ResumeLayout(false);
            this.pnlIAGW.ResumeLayout(false);
            this.pnlIAGW.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pboxAuthCheckState)).EndInit();
            this.tpDBConnection.ResumeLayout(false);
            this.pnlSoundTypeBack.ResumeLayout(false);
            this.pnlSoundTypeBack.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Adeng.Framework.Ctrl.AdengImgButtonEx btnClose;
        private System.Windows.Forms.TabControl tabCtrlMain;
        private System.Windows.Forms.TabPage tpIAGWConnection;
        private System.Windows.Forms.Panel pnlIAGW;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnConnection;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnDisconnection;
        private System.Windows.Forms.TabPage tpDBConnection;
        private System.Windows.Forms.Panel pnlSoundTypeBack;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.PictureBox pboxAuthCheckState;
        private Adeng.Framework.Ctrl.TextBoxEx txtboxAuthCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private Adeng.Framework.Ctrl.TextBoxEx txtboxIAGWIP;
        private Adeng.Framework.Ctrl.TextBoxEx txtboxIAGWPort;
        private Adeng.Framework.Ctrl.TextBoxEx txtboxDBPassword;
        private Adeng.Framework.Ctrl.TextBoxEx txtboxDBID;
        private Adeng.Framework.Ctrl.TextBoxEx txtboxDBServiceID;
        private Adeng.Framework.Ctrl.TextBoxEx txtboxDBIP;
        private System.Windows.Forms.Label label4;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnDBConnectionTest;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabPage tpLocalSystemSetting;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private Adeng.Framework.Ctrl.TextBoxEx txtboxSenderName;
        private Adeng.Framework.Ctrl.TextBoxEx txtboxSenderID;
        private Adeng.Framework.Ctrl.TextBoxEx txtboxRegionCode;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnSave;
        private System.Windows.Forms.CheckBox chkboxAutoReConnection;
    }
}