namespace StdWarningInstallation
{
    partial class MsgTextManageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MsgTextManageForm));
            this.btnClose = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.cmbboxDisasterCategory = new System.Windows.Forms.ComboBox();
            this.pnlSelectedOrderBack = new System.Windows.Forms.Panel();
            this.lstboxDisasterKind = new System.Windows.Forms.ListBox();
            this.lbDisaster = new System.Windows.Forms.Label();
            this.btnRestoreAll = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblDescription = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lstboxCityType = new System.Windows.Forms.ListBox();
            this.lblCityType = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lstboxLanguageKind = new System.Windows.Forms.ListBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.pnlConditions = new System.Windows.Forms.Panel();
            this.lblButtonDescription = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlTextTypeBack = new System.Windows.Forms.Panel();
            this.lblLimitLetterOfDMB = new System.Windows.Forms.Label();
            this.lblLimitLetterOfTTS = new System.Windows.Forms.Label();
            this.txtboxDMB = new System.Windows.Forms.TextBox();
            this.labelDMB = new System.Windows.Forms.Label();
            this.txtboxTTS = new System.Windows.Forms.TextBox();
            this.lblTTS = new System.Windows.Forms.Label();
            this.lblLimitLetterOfBoard = new System.Windows.Forms.Label();
            this.lblLimitLetterOfCBS = new System.Windows.Forms.Label();
            this.txtboxBoard = new System.Windows.Forms.TextBox();
            this.lblBoard = new System.Windows.Forms.Label();
            this.txtboxCBS = new System.Windows.Forms.TextBox();
            this.lblCBS = new System.Windows.Forms.Label();
            this.btnRestoreOne = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnSave = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlSelectedOrderBack.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.pnlConditions.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlTextTypeBack.SuspendLayout();
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
            this.btnClose.Location = new System.Drawing.Point(585, 738);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 36);
            this.btnClose.TabIndex = 17;
            this.btnClose.Text = "닫기";
            this.btnClose.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnClose.UseChecked = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cmbboxDisasterCategory
            // 
            this.cmbboxDisasterCategory.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbboxDisasterCategory.FormattingEnabled = true;
            this.cmbboxDisasterCategory.ItemHeight = 17;
            this.cmbboxDisasterCategory.Location = new System.Drawing.Point(8, 38);
            this.cmbboxDisasterCategory.Name = "cmbboxDisasterCategory";
            this.cmbboxDisasterCategory.Size = new System.Drawing.Size(214, 25);
            this.cmbboxDisasterCategory.TabIndex = 41;
            this.cmbboxDisasterCategory.SelectedIndexChanged += new System.EventHandler(this.cmbboxDisasterCategory_SelectedIndexChanged);
            this.cmbboxDisasterCategory.SelectionChangeCommitted += new System.EventHandler(this.cmbboxDisasterCategory_SelectionChangeCommitted);
            // 
            // pnlSelectedOrderBack
            // 
            this.pnlSelectedOrderBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.pnlSelectedOrderBack.Controls.Add(this.lstboxDisasterKind);
            this.pnlSelectedOrderBack.Controls.Add(this.cmbboxDisasterCategory);
            this.pnlSelectedOrderBack.Controls.Add(this.lbDisaster);
            this.pnlSelectedOrderBack.Location = new System.Drawing.Point(7, 8);
            this.pnlSelectedOrderBack.Name = "pnlSelectedOrderBack";
            this.pnlSelectedOrderBack.Size = new System.Drawing.Size(230, 404);
            this.pnlSelectedOrderBack.TabIndex = 16;
            // 
            // lstboxDisasterKind
            // 
            this.lstboxDisasterKind.AccessibleDescription = "";
            this.lstboxDisasterKind.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lstboxDisasterKind.FormattingEnabled = true;
            this.lstboxDisasterKind.ItemHeight = 17;
            this.lstboxDisasterKind.Location = new System.Drawing.Point(8, 67);
            this.lstboxDisasterKind.Name = "lstboxDisasterKind";
            this.lstboxDisasterKind.Size = new System.Drawing.Size(214, 327);
            this.lstboxDisasterKind.TabIndex = 42;
            this.lstboxDisasterKind.SelectedIndexChanged += new System.EventHandler(this.lstboxDisasterKind_SelectedIndexChanged);
            // 
            // lbDisaster
            // 
            this.lbDisaster.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbDisaster.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbDisaster.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lbDisaster.Location = new System.Drawing.Point(0, 0);
            this.lbDisaster.Margin = new System.Windows.Forms.Padding(0);
            this.lbDisaster.Name = "lbDisaster";
            this.lbDisaster.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lbDisaster.Size = new System.Drawing.Size(230, 35);
            this.lbDisaster.TabIndex = 0;
            this.lbDisaster.Text = "재난 종류";
            this.lbDisaster.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnRestoreAll
            // 
            this.btnRestoreAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRestoreAll.ChkValue = false;
            this.btnRestoreAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRestoreAll.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRestoreAll.ForeColor = System.Drawing.Color.White;
            this.btnRestoreAll.Image = ((System.Drawing.Image)(resources.GetObject("btnRestoreAll.Image")));
            this.btnRestoreAll.ImgDisable = null;
            this.btnRestoreAll.ImgHover = null;
            this.btnRestoreAll.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnRestoreAll.ImgSelect")));
            this.btnRestoreAll.ImgStatusEvent = null;
            this.btnRestoreAll.ImgStatusNormal = null;
            this.btnRestoreAll.ImgStatusOffsetX = 2;
            this.btnRestoreAll.ImgStatusOffsetY = 0;
            this.btnRestoreAll.ImgStretch = true;
            this.btnRestoreAll.IsImgStatusNormal = true;
            this.btnRestoreAll.Location = new System.Drawing.Point(7, 738);
            this.btnRestoreAll.Margin = new System.Windows.Forms.Padding(0);
            this.btnRestoreAll.Name = "btnRestoreAll";
            this.btnRestoreAll.Size = new System.Drawing.Size(100, 36);
            this.btnRestoreAll.TabIndex = 18;
            this.btnRestoreAll.Text = "전체 복원";
            this.btnRestoreAll.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnRestoreAll.UseChecked = false;
            this.btnRestoreAll.Click += new System.EventHandler(this.btnRestoreAll_Click);
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
            this.pnlTop.Size = new System.Drawing.Size(694, 40);
            this.pnlTop.TabIndex = 16;
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
            this.lblDescription.Size = new System.Drawing.Size(694, 40);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "발령 기본 문안을 편집하거나 복원할 수 있습니다.";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.panel3.Controls.Add(this.lstboxCityType);
            this.panel3.Controls.Add(this.lblCityType);
            this.panel3.Location = new System.Drawing.Point(7, 534);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(230, 135);
            this.panel3.TabIndex = 43;
            // 
            // lstboxCityType
            // 
            this.lstboxCityType.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lstboxCityType.FormattingEnabled = true;
            this.lstboxCityType.ItemHeight = 17;
            this.lstboxCityType.Items.AddRange(new object[] {
            "일반",
            "농촌/산촌",
            "어촌",
            "대도시"});
            this.lstboxCityType.Location = new System.Drawing.Point(8, 38);
            this.lstboxCityType.Name = "lstboxCityType";
            this.lstboxCityType.Size = new System.Drawing.Size(214, 89);
            this.lstboxCityType.TabIndex = 42;
            this.lstboxCityType.SelectedIndexChanged += new System.EventHandler(this.lstboxCityType_SelectedIndexChanged);
            // 
            // lblCityType
            // 
            this.lblCityType.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCityType.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCityType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblCityType.Location = new System.Drawing.Point(0, 0);
            this.lblCityType.Margin = new System.Windows.Forms.Padding(0);
            this.lblCityType.Name = "lblCityType";
            this.lblCityType.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblCityType.Size = new System.Drawing.Size(230, 35);
            this.lblCityType.TabIndex = 0;
            this.lblCityType.Text = "도시 형태 종류";
            this.lblCityType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.panel4.Controls.Add(this.lstboxLanguageKind);
            this.panel4.Controls.Add(this.lblLanguage);
            this.panel4.Location = new System.Drawing.Point(7, 421);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(230, 103);
            this.panel4.TabIndex = 44;
            // 
            // lstboxLanguageKind
            // 
            this.lstboxLanguageKind.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lstboxLanguageKind.FormattingEnabled = true;
            this.lstboxLanguageKind.ItemHeight = 17;
            this.lstboxLanguageKind.Items.AddRange(new object[] {
            "한국어",
            "영어"});
            this.lstboxLanguageKind.Location = new System.Drawing.Point(8, 38);
            this.lstboxLanguageKind.Name = "lstboxLanguageKind";
            this.lstboxLanguageKind.Size = new System.Drawing.Size(214, 55);
            this.lstboxLanguageKind.TabIndex = 42;
            this.lstboxLanguageKind.SelectedIndexChanged += new System.EventHandler(this.lstboxLanguageKind_SelectedIndexChanged);
            // 
            // lblLanguage
            // 
            this.lblLanguage.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLanguage.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLanguage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblLanguage.Location = new System.Drawing.Point(0, 0);
            this.lblLanguage.Margin = new System.Windows.Forms.Padding(0);
            this.lblLanguage.Name = "lblLanguage";
            this.lblLanguage.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblLanguage.Size = new System.Drawing.Size(230, 35);
            this.lblLanguage.TabIndex = 0;
            this.lblLanguage.Text = "언어 종류";
            this.lblLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlConditions
            // 
            this.pnlConditions.BackColor = System.Drawing.Color.White;
            this.pnlConditions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlConditions.Controls.Add(this.pnlSelectedOrderBack);
            this.pnlConditions.Controls.Add(this.panel3);
            this.pnlConditions.Controls.Add(this.panel4);
            this.pnlConditions.Location = new System.Drawing.Point(7, 48);
            this.pnlConditions.Name = "pnlConditions";
            this.pnlConditions.Size = new System.Drawing.Size(247, 680);
            this.pnlConditions.TabIndex = 45;
            // 
            // lblButtonDescription
            // 
            this.lblButtonDescription.AutoSize = true;
            this.lblButtonDescription.ForeColor = System.Drawing.Color.Gray;
            this.lblButtonDescription.Location = new System.Drawing.Point(112, 751);
            this.lblButtonDescription.Name = "lblButtonDescription";
            this.lblButtonDescription.Size = new System.Drawing.Size(273, 12);
            this.lblButtonDescription.TabIndex = 46;
            this.lblButtonDescription.Text = "모든 문안 정보를 시스템 초기 상태로 복원합니다.";
            this.lblButtonDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.pnlTextTypeBack);
            this.panel1.Controls.Add(this.btnRestoreOne);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Location = new System.Drawing.Point(260, 48);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(426, 680);
            this.panel1.TabIndex = 50;
            // 
            // pnlTextTypeBack
            // 
            this.pnlTextTypeBack.AutoScroll = true;
            this.pnlTextTypeBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.pnlTextTypeBack.Controls.Add(this.lblLimitLetterOfDMB);
            this.pnlTextTypeBack.Controls.Add(this.lblLimitLetterOfTTS);
            this.pnlTextTypeBack.Controls.Add(this.txtboxDMB);
            this.pnlTextTypeBack.Controls.Add(this.labelDMB);
            this.pnlTextTypeBack.Controls.Add(this.txtboxTTS);
            this.pnlTextTypeBack.Controls.Add(this.lblTTS);
            this.pnlTextTypeBack.Controls.Add(this.lblLimitLetterOfBoard);
            this.pnlTextTypeBack.Controls.Add(this.lblLimitLetterOfCBS);
            this.pnlTextTypeBack.Controls.Add(this.txtboxBoard);
            this.pnlTextTypeBack.Controls.Add(this.lblBoard);
            this.pnlTextTypeBack.Controls.Add(this.txtboxCBS);
            this.pnlTextTypeBack.Controls.Add(this.lblCBS);
            this.pnlTextTypeBack.Location = new System.Drawing.Point(8, 9);
            this.pnlTextTypeBack.Name = "pnlTextTypeBack";
            this.pnlTextTypeBack.Size = new System.Drawing.Size(408, 614);
            this.pnlTextTypeBack.TabIndex = 52;
            // 
            // lblLimitLetterOfDMB
            // 
            this.lblLimitLetterOfDMB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLimitLetterOfDMB.AutoSize = true;
            this.lblLimitLetterOfDMB.Location = new System.Drawing.Point(343, 479);
            this.lblLimitLetterOfDMB.Name = "lblLimitLetterOfDMB";
            this.lblLimitLetterOfDMB.Size = new System.Drawing.Size(57, 12);
            this.lblLimitLetterOfDMB.TabIndex = 5;
            this.lblLimitLetterOfDMB.Text = "(160/520)";
            this.lblLimitLetterOfDMB.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLimitLetterOfTTS
            // 
            this.lblLimitLetterOfTTS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLimitLetterOfTTS.AutoSize = true;
            this.lblLimitLetterOfTTS.Location = new System.Drawing.Point(343, 327);
            this.lblLimitLetterOfTTS.Name = "lblLimitLetterOfTTS";
            this.lblLimitLetterOfTTS.Size = new System.Drawing.Size(57, 12);
            this.lblLimitLetterOfTTS.TabIndex = 5;
            this.lblLimitLetterOfTTS.Text = "(160/520)";
            this.lblLimitLetterOfTTS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtboxDMB
            // 
            this.txtboxDMB.Font = new System.Drawing.Font("굴림", 10F);
            this.txtboxDMB.Location = new System.Drawing.Point(8, 494);
            this.txtboxDMB.MaxLength = 520;
            this.txtboxDMB.Multiline = true;
            this.txtboxDMB.Name = "txtboxDMB";
            this.txtboxDMB.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtboxDMB.Size = new System.Drawing.Size(392, 110);
            this.txtboxDMB.TabIndex = 4;
            this.txtboxDMB.TextChanged += new System.EventHandler(this.txtboxDMB_TextChanged);
            // 
            // labelDMB
            // 
            this.labelDMB.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelDMB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.labelDMB.Location = new System.Drawing.Point(7, 468);
            this.labelDMB.Margin = new System.Windows.Forms.Padding(0);
            this.labelDMB.Name = "labelDMB";
            this.labelDMB.Size = new System.Drawing.Size(50, 23);
            this.labelDMB.TabIndex = 3;
            this.labelDMB.Text = "DMB";
            this.labelDMB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtboxTTS
            // 
            this.txtboxTTS.Font = new System.Drawing.Font("굴림", 10F);
            this.txtboxTTS.Location = new System.Drawing.Point(8, 342);
            this.txtboxTTS.MaxLength = 520;
            this.txtboxTTS.Multiline = true;
            this.txtboxTTS.Name = "txtboxTTS";
            this.txtboxTTS.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtboxTTS.Size = new System.Drawing.Size(392, 110);
            this.txtboxTTS.TabIndex = 4;
            // 
            // lblTTS
            // 
            this.lblTTS.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTTS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblTTS.Location = new System.Drawing.Point(7, 316);
            this.lblTTS.Margin = new System.Windows.Forms.Padding(0);
            this.lblTTS.Name = "lblTTS";
            this.lblTTS.Size = new System.Drawing.Size(50, 23);
            this.lblTTS.TabIndex = 3;
            this.lblTTS.Text = "TTS";
            this.lblTTS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLimitLetterOfBoard
            // 
            this.lblLimitLetterOfBoard.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLimitLetterOfBoard.AutoSize = true;
            this.lblLimitLetterOfBoard.Location = new System.Drawing.Point(343, 175);
            this.lblLimitLetterOfBoard.Name = "lblLimitLetterOfBoard";
            this.lblLimitLetterOfBoard.Size = new System.Drawing.Size(57, 12);
            this.lblLimitLetterOfBoard.TabIndex = 2;
            this.lblLimitLetterOfBoard.Text = "(160/520)";
            this.lblLimitLetterOfBoard.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLimitLetterOfCBS
            // 
            this.lblLimitLetterOfCBS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLimitLetterOfCBS.AutoSize = true;
            this.lblLimitLetterOfCBS.Location = new System.Drawing.Point(349, 23);
            this.lblLimitLetterOfCBS.Name = "lblLimitLetterOfCBS";
            this.lblLimitLetterOfCBS.Size = new System.Drawing.Size(51, 12);
            this.lblLimitLetterOfCBS.TabIndex = 2;
            this.lblLimitLetterOfCBS.Text = "(160/90)";
            this.lblLimitLetterOfCBS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtboxBoard
            // 
            this.txtboxBoard.Font = new System.Drawing.Font("굴림", 10F);
            this.txtboxBoard.Location = new System.Drawing.Point(8, 190);
            this.txtboxBoard.MaxLength = 520;
            this.txtboxBoard.Multiline = true;
            this.txtboxBoard.Name = "txtboxBoard";
            this.txtboxBoard.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtboxBoard.Size = new System.Drawing.Size(392, 110);
            this.txtboxBoard.TabIndex = 1;
            this.txtboxBoard.TextChanged += new System.EventHandler(this.txtboxBoard_TextChanged);
            // 
            // lblBoard
            // 
            this.lblBoard.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblBoard.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblBoard.Location = new System.Drawing.Point(7, 164);
            this.lblBoard.Margin = new System.Windows.Forms.Padding(0);
            this.lblBoard.Name = "lblBoard";
            this.lblBoard.Size = new System.Drawing.Size(50, 23);
            this.lblBoard.TabIndex = 0;
            this.lblBoard.Text = "Board";
            this.lblBoard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtboxCBS
            // 
            this.txtboxCBS.Font = new System.Drawing.Font("굴림", 10F);
            this.txtboxCBS.Location = new System.Drawing.Point(8, 38);
            this.txtboxCBS.MaxLength = 90;
            this.txtboxCBS.Multiline = true;
            this.txtboxCBS.Name = "txtboxCBS";
            this.txtboxCBS.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtboxCBS.Size = new System.Drawing.Size(392, 110);
            this.txtboxCBS.TabIndex = 1;
            this.txtboxCBS.TextChanged += new System.EventHandler(this.txtboxCBS_TextChanged);
            // 
            // lblCBS
            // 
            this.lblCBS.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCBS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblCBS.Location = new System.Drawing.Point(7, 12);
            this.lblCBS.Margin = new System.Windows.Forms.Padding(0);
            this.lblCBS.Name = "lblCBS";
            this.lblCBS.Size = new System.Drawing.Size(50, 23);
            this.lblCBS.TabIndex = 0;
            this.lblCBS.Text = "CBS";
            this.lblCBS.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnRestoreOne
            // 
            this.btnRestoreOne.ChkValue = false;
            this.btnRestoreOne.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRestoreOne.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRestoreOne.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(47)))));
            this.btnRestoreOne.Image = ((System.Drawing.Image)(resources.GetObject("btnRestoreOne.Image")));
            this.btnRestoreOne.ImgDisable = null;
            this.btnRestoreOne.ImgHover = null;
            this.btnRestoreOne.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnRestoreOne.ImgSelect")));
            this.btnRestoreOne.ImgStatusEvent = null;
            this.btnRestoreOne.ImgStatusNormal = null;
            this.btnRestoreOne.ImgStatusOffsetX = 2;
            this.btnRestoreOne.ImgStatusOffsetY = 0;
            this.btnRestoreOne.ImgStretch = true;
            this.btnRestoreOne.IsImgStatusNormal = true;
            this.btnRestoreOne.Location = new System.Drawing.Point(210, 633);
            this.btnRestoreOne.Name = "btnRestoreOne";
            this.btnRestoreOne.Size = new System.Drawing.Size(100, 36);
            this.btnRestoreOne.TabIndex = 51;
            this.btnRestoreOne.Text = "문안 복원";
            this.btnRestoreOne.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnRestoreOne.UseChecked = true;
            this.btnRestoreOne.Click += new System.EventHandler(this.btnRestoreOne_Click);
            // 
            // btnSave
            // 
            this.btnSave.ChkValue = false;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(47)))));
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
            this.btnSave.Location = new System.Drawing.Point(316, 633);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 36);
            this.btnSave.TabIndex = 50;
            this.btnSave.Text = "변경 저장";
            this.btnSave.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnSave.UseChecked = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // MsgTextManageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 783);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblButtonDescription);
            this.Controls.Add(this.pnlConditions);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRestoreAll);
            this.Controls.Add(this.pnlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MsgTextManageForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "기본 문안 관리";
            this.pnlSelectedOrderBack.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.pnlConditions.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlTextTypeBack.ResumeLayout(false);
            this.pnlTextTypeBack.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Adeng.Framework.Ctrl.AdengImgButtonEx btnClose;
        private System.Windows.Forms.ComboBox cmbboxDisasterCategory;
        private System.Windows.Forms.Panel pnlSelectedOrderBack;
        private System.Windows.Forms.Label lbDisaster;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnRestoreAll;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.ListBox lstboxDisasterKind;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ListBox lstboxCityType;
        private System.Windows.Forms.Label lblCityType;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.ListBox lstboxLanguageKind;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.Panel pnlConditions;
        private System.Windows.Forms.Label lblButtonDescription;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlTextTypeBack;
        private System.Windows.Forms.Label lblLimitLetterOfDMB;
        private System.Windows.Forms.Label lblLimitLetterOfTTS;
        private System.Windows.Forms.TextBox txtboxDMB;
        private System.Windows.Forms.Label labelDMB;
        private System.Windows.Forms.TextBox txtboxTTS;
        private System.Windows.Forms.Label lblTTS;
        private System.Windows.Forms.Label lblLimitLetterOfBoard;
        private System.Windows.Forms.Label lblLimitLetterOfCBS;
        private System.Windows.Forms.TextBox txtboxBoard;
        private System.Windows.Forms.Label lblBoard;
        private System.Windows.Forms.TextBox txtboxCBS;
        private System.Windows.Forms.Label lblCBS;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnRestoreOne;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnSave;

    }
}