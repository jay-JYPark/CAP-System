namespace StdWarningInstallation
{
    partial class InquiryHistoryForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InquiryHistoryForm));
            this.btnClose = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.lblDescription = new System.Windows.Forms.Label();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnClear = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.tabCtlMain = new System.Windows.Forms.TabControl();
            this.tpOrderHistory = new System.Windows.Forms.TabPage();
            this.lvOrderHistory = new Adeng.Framework.Ctrl.AdengListView();
            this.pnlSelectedOrderBack = new System.Windows.Forms.Panel();
            this.btnInquriyOrderHistory = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.cmbboxDisasterKind = new System.Windows.Forms.ComboBox();
            this.cmbboxOrderLocation = new System.Windows.Forms.ComboBox();
            this.cmbboxOrderMode = new System.Windows.Forms.ComboBox();
            this.cmbboxDisasterCategory = new System.Windows.Forms.ComboBox();
            this.lblOrderLocation = new System.Windows.Forms.Label();
            this.lblDisaster = new System.Windows.Forms.Label();
            this.dtmPickerOrderDurtionStart = new System.Windows.Forms.DateTimePicker();
            this.dtmPickerOrderDurtionEnd = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.lblOrderMode = new System.Windows.Forms.Label();
            this.lblInquiryDuration = new System.Windows.Forms.Label();
            this.tpProgramHistory = new System.Windows.Forms.TabPage();
            this.lvProgramHistoryList = new Adeng.Framework.Ctrl.AdengListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnInquiryProgramHistory = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.dtmPickerProgramDurtionStart = new System.Windows.Forms.DateTimePicker();
            this.dtmPickerProgramDurtionEnd = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.imglstEventLogIcons = new System.Windows.Forms.ImageList(this.components);
            this.pnlTop.SuspendLayout();
            this.tabCtlMain.SuspendLayout();
            this.tpOrderHistory.SuspendLayout();
            this.pnlSelectedOrderBack.SuspendLayout();
            this.tpProgramHistory.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.btnClose.Location = new System.Drawing.Point(711, 713);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 36);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "닫기";
            this.btnClose.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnClose.UseChecked = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
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
            this.lblDescription.Size = new System.Drawing.Size(820, 40);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "이력을 조회합니다.";
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
            this.pnlTop.Size = new System.Drawing.Size(820, 40);
            this.pnlTop.TabIndex = 12;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.ChkValue = false;
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.ImgDisable = null;
            this.btnClear.ImgHover = null;
            this.btnClear.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnClear.ImgSelect")));
            this.btnClear.ImgStatusEvent = null;
            this.btnClear.ImgStatusNormal = null;
            this.btnClear.ImgStatusOffsetX = 2;
            this.btnClear.ImgStatusOffsetY = 0;
            this.btnClear.ImgStretch = true;
            this.btnClear.IsImgStatusNormal = true;
            this.btnClear.Location = new System.Drawing.Point(9, 713);
            this.btnClear.Margin = new System.Windows.Forms.Padding(0);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 36);
            this.btnClear.TabIndex = 13;
            this.btnClear.Text = "초기화";
            this.btnClear.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnClear.UseChecked = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tabCtlMain
            // 
            this.tabCtlMain.Controls.Add(this.tpOrderHistory);
            this.tabCtlMain.Controls.Add(this.tpProgramHistory);
            this.tabCtlMain.Location = new System.Drawing.Point(7, 47);
            this.tabCtlMain.Name = "tabCtlMain";
            this.tabCtlMain.SelectedIndex = 0;
            this.tabCtlMain.Size = new System.Drawing.Size(809, 658);
            this.tabCtlMain.TabIndex = 15;
            // 
            // tpOrderHistory
            // 
            this.tpOrderHistory.Controls.Add(this.lvOrderHistory);
            this.tpOrderHistory.Controls.Add(this.pnlSelectedOrderBack);
            this.tpOrderHistory.Location = new System.Drawing.Point(4, 22);
            this.tpOrderHistory.Name = "tpOrderHistory";
            this.tpOrderHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tpOrderHistory.Size = new System.Drawing.Size(801, 632);
            this.tpOrderHistory.TabIndex = 0;
            this.tpOrderHistory.Tag = "OrderHistory";
            this.tpOrderHistory.Text = "발령 이력";
            this.tpOrderHistory.UseVisualStyleBackColor = true;
            // 
            // lvOrderHistory
            // 
            this.lvOrderHistory.AntiAlias = false;
            this.lvOrderHistory.AutoFit = false;
            this.lvOrderHistory.BackColor = System.Drawing.Color.White;
            this.lvOrderHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvOrderHistory.ColumnHeight = 24;
            this.lvOrderHistory.ColumnOffset = new System.Windows.Forms.Padding(2, 2, 0, 1);
            this.lvOrderHistory.ColumnStyle = Adeng.Framework.Ctrl.AdengListViewColumnStyle.DEF_STYLE;
            this.lvOrderHistory.FocusedItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(185)))), ((int)(((byte)(230)))));
            this.lvOrderHistory.FrozenColumnIndex = -1;
            this.lvOrderHistory.FullRowSelect = true;
            this.lvOrderHistory.GridDashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.lvOrderHistory.GridLineStyle = Adeng.Framework.Ctrl.AdengListViewGridLine.NONE;
            this.lvOrderHistory.HideSelection = false;
            this.lvOrderHistory.HoverSelection = false;
            this.lvOrderHistory.IconOffset = new System.Drawing.Point(1, 0);
            this.lvOrderHistory.InteraceColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.lvOrderHistory.InteraceColor2 = System.Drawing.Color.White;
            this.lvOrderHistory.ItemHeight = 18;
            this.lvOrderHistory.ItemOffset = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lvOrderHistory.Location = new System.Drawing.Point(7, 93);
            this.lvOrderHistory.MultiSelect = false;
            this.lvOrderHistory.Name = "lvOrderHistory";
            this.lvOrderHistory.ScrollType = Adeng.Framework.Ctrl.AdengListViewScrollType.BOTH;
            this.lvOrderHistory.SelectioinItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(232)))), ((int)(((byte)(250)))));
            this.lvOrderHistory.Size = new System.Drawing.Size(787, 533);
            this.lvOrderHistory.TabIndex = 17;
            this.lvOrderHistory.UseInteraceColor = true;
            this.lvOrderHistory.UseSelFocusedBar = false;
            this.lvOrderHistory.DoubleClick += new System.EventHandler(this.lvOrderHistory_DoubleClick);
            // 
            // pnlSelectedOrderBack
            // 
            this.pnlSelectedOrderBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.pnlSelectedOrderBack.Controls.Add(this.btnInquriyOrderHistory);
            this.pnlSelectedOrderBack.Controls.Add(this.cmbboxDisasterKind);
            this.pnlSelectedOrderBack.Controls.Add(this.cmbboxOrderLocation);
            this.pnlSelectedOrderBack.Controls.Add(this.cmbboxOrderMode);
            this.pnlSelectedOrderBack.Controls.Add(this.cmbboxDisasterCategory);
            this.pnlSelectedOrderBack.Controls.Add(this.lblOrderLocation);
            this.pnlSelectedOrderBack.Controls.Add(this.lblDisaster);
            this.pnlSelectedOrderBack.Controls.Add(this.dtmPickerOrderDurtionStart);
            this.pnlSelectedOrderBack.Controls.Add(this.dtmPickerOrderDurtionEnd);
            this.pnlSelectedOrderBack.Controls.Add(this.label3);
            this.pnlSelectedOrderBack.Controls.Add(this.lblOrderMode);
            this.pnlSelectedOrderBack.Controls.Add(this.lblInquiryDuration);
            this.pnlSelectedOrderBack.Location = new System.Drawing.Point(7, 7);
            this.pnlSelectedOrderBack.Name = "pnlSelectedOrderBack";
            this.pnlSelectedOrderBack.Size = new System.Drawing.Size(787, 79);
            this.pnlSelectedOrderBack.TabIndex = 16;
            // 
            // btnInquriyOrderHistory
            // 
            this.btnInquriyOrderHistory.ChkValue = false;
            this.btnInquriyOrderHistory.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInquriyOrderHistory.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInquriyOrderHistory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(47)))));
            this.btnInquriyOrderHistory.Image = ((System.Drawing.Image)(resources.GetObject("btnInquriyOrderHistory.Image")));
            this.btnInquriyOrderHistory.ImgDisable = null;
            this.btnInquriyOrderHistory.ImgHover = null;
            this.btnInquriyOrderHistory.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnInquriyOrderHistory.ImgSelect")));
            this.btnInquriyOrderHistory.ImgStatusEvent = null;
            this.btnInquriyOrderHistory.ImgStatusNormal = null;
            this.btnInquriyOrderHistory.ImgStatusOffsetX = 2;
            this.btnInquriyOrderHistory.ImgStatusOffsetY = 0;
            this.btnInquriyOrderHistory.ImgStretch = false;
            this.btnInquriyOrderHistory.IsImgStatusNormal = true;
            this.btnInquriyOrderHistory.Location = new System.Drawing.Point(690, 13);
            this.btnInquriyOrderHistory.Name = "btnInquriyOrderHistory";
            this.btnInquriyOrderHistory.Size = new System.Drawing.Size(70, 53);
            this.btnInquriyOrderHistory.TabIndex = 43;
            this.btnInquriyOrderHistory.Text = "조회";
            this.btnInquriyOrderHistory.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnInquriyOrderHistory.UseChecked = false;
            this.btnInquriyOrderHistory.Click += new System.EventHandler(this.btnInquriyOrderHistory_Click);
            // 
            // cmbboxDisasterKind
            // 
            this.cmbboxDisasterKind.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.cmbboxDisasterKind.FormattingEnabled = true;
            this.cmbboxDisasterKind.Location = new System.Drawing.Point(269, 43);
            this.cmbboxDisasterKind.Name = "cmbboxDisasterKind";
            this.cmbboxDisasterKind.Size = new System.Drawing.Size(173, 23);
            this.cmbboxDisasterKind.TabIndex = 42;
            this.cmbboxDisasterKind.SelectedIndexChanged += new System.EventHandler(this.cmbboxDisasterKind_SelectedIndexChanged);
            // 
            // cmbboxOrderLocation
            // 
            this.cmbboxOrderLocation.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbboxOrderLocation.FormattingEnabled = true;
            this.cmbboxOrderLocation.ItemHeight = 15;
            this.cmbboxOrderLocation.Location = new System.Drawing.Point(557, 43);
            this.cmbboxOrderLocation.Name = "cmbboxOrderLocation";
            this.cmbboxOrderLocation.Size = new System.Drawing.Size(110, 23);
            this.cmbboxOrderLocation.TabIndex = 41;
            this.cmbboxOrderLocation.SelectedIndexChanged += new System.EventHandler(this.cmbboxOrderLocation_SelectedIndexChanged);
            // 
            // cmbboxOrderMode
            // 
            this.cmbboxOrderMode.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbboxOrderMode.FormattingEnabled = true;
            this.cmbboxOrderMode.ItemHeight = 15;
            this.cmbboxOrderMode.Location = new System.Drawing.Point(557, 13);
            this.cmbboxOrderMode.Name = "cmbboxOrderMode";
            this.cmbboxOrderMode.Size = new System.Drawing.Size(110, 23);
            this.cmbboxOrderMode.TabIndex = 41;
            this.cmbboxOrderMode.SelectedIndexChanged += new System.EventHandler(this.cmbboxOrderMode_SelectedIndexChanged);
            // 
            // cmbboxDisasterCategory
            // 
            this.cmbboxDisasterCategory.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbboxDisasterCategory.FormattingEnabled = true;
            this.cmbboxDisasterCategory.ItemHeight = 15;
            this.cmbboxDisasterCategory.Location = new System.Drawing.Point(93, 43);
            this.cmbboxDisasterCategory.Name = "cmbboxDisasterCategory";
            this.cmbboxDisasterCategory.Size = new System.Drawing.Size(173, 23);
            this.cmbboxDisasterCategory.TabIndex = 41;
            this.cmbboxDisasterCategory.SelectedIndexChanged += new System.EventHandler(this.cmbboxDisasterCategory_SelectedIndexChanged);
            // 
            // lblOrderLocation
            // 
            this.lblOrderLocation.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblOrderLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblOrderLocation.Location = new System.Drawing.Point(472, 43);
            this.lblOrderLocation.Name = "lblOrderLocation";
            this.lblOrderLocation.Size = new System.Drawing.Size(82, 23);
            this.lblOrderLocation.TabIndex = 40;
            this.lblOrderLocation.Text = "발령지 구분";
            this.lblOrderLocation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDisaster
            // 
            this.lblDisaster.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold);
            this.lblDisaster.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblDisaster.Location = new System.Drawing.Point(18, 43);
            this.lblDisaster.Name = "lblDisaster";
            this.lblDisaster.Size = new System.Drawing.Size(64, 23);
            this.lblDisaster.TabIndex = 40;
            this.lblDisaster.Text = "재난 종류";
            this.lblDisaster.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dtmPickerOrderDurtionStart
            // 
            this.dtmPickerOrderDurtionStart.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtmPickerOrderDurtionStart.Location = new System.Drawing.Point(93, 13);
            this.dtmPickerOrderDurtionStart.Name = "dtmPickerOrderDurtionStart";
            this.dtmPickerOrderDurtionStart.Size = new System.Drawing.Size(163, 23);
            this.dtmPickerOrderDurtionStart.TabIndex = 38;
            this.dtmPickerOrderDurtionStart.ValueChanged += new System.EventHandler(this.dtmPickerOrderDurtionStart_ValueChanged);
            // 
            // dtmPickerOrderDurtionEnd
            // 
            this.dtmPickerOrderDurtionEnd.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.dtmPickerOrderDurtionEnd.Location = new System.Drawing.Point(279, 13);
            this.dtmPickerOrderDurtionEnd.Name = "dtmPickerOrderDurtionEnd";
            this.dtmPickerOrderDurtionEnd.Size = new System.Drawing.Size(163, 23);
            this.dtmPickerOrderDurtionEnd.TabIndex = 39;
            this.dtmPickerOrderDurtionEnd.ValueChanged += new System.EventHandler(this.dtmPickerOrderDurtionEnd_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label3.Location = new System.Drawing.Point(260, 17);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "~";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblOrderMode
            // 
            this.lblOrderMode.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblOrderMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblOrderMode.Location = new System.Drawing.Point(472, 13);
            this.lblOrderMode.Margin = new System.Windows.Forms.Padding(0);
            this.lblOrderMode.Name = "lblOrderMode";
            this.lblOrderMode.Size = new System.Drawing.Size(82, 23);
            this.lblOrderMode.TabIndex = 0;
            this.lblOrderMode.Text = "발령 모드";
            this.lblOrderMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblInquiryDuration
            // 
            this.lblInquiryDuration.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblInquiryDuration.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblInquiryDuration.Location = new System.Drawing.Point(18, 13);
            this.lblInquiryDuration.Margin = new System.Windows.Forms.Padding(0);
            this.lblInquiryDuration.Name = "lblInquiryDuration";
            this.lblInquiryDuration.Size = new System.Drawing.Size(64, 23);
            this.lblInquiryDuration.TabIndex = 0;
            this.lblInquiryDuration.Text = "조회 기간";
            this.lblInquiryDuration.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tpProgramHistory
            // 
            this.tpProgramHistory.Controls.Add(this.lvProgramHistoryList);
            this.tpProgramHistory.Controls.Add(this.panel1);
            this.tpProgramHistory.Location = new System.Drawing.Point(4, 22);
            this.tpProgramHistory.Name = "tpProgramHistory";
            this.tpProgramHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tpProgramHistory.Size = new System.Drawing.Size(801, 632);
            this.tpProgramHistory.TabIndex = 1;
            this.tpProgramHistory.Tag = "ProgramHistory";
            this.tpProgramHistory.Text = "프로그램 사용 이력";
            this.tpProgramHistory.UseVisualStyleBackColor = true;
            // 
            // lvProgramHistoryList
            // 
            this.lvProgramHistoryList.AntiAlias = false;
            this.lvProgramHistoryList.AutoFit = false;
            this.lvProgramHistoryList.BackColor = System.Drawing.Color.White;
            this.lvProgramHistoryList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvProgramHistoryList.ColumnHeight = 24;
            this.lvProgramHistoryList.ColumnOffset = new System.Windows.Forms.Padding(2, 2, 0, 1);
            this.lvProgramHistoryList.ColumnStyle = Adeng.Framework.Ctrl.AdengListViewColumnStyle.DEF_STYLE;
            this.lvProgramHistoryList.FocusedItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(185)))), ((int)(((byte)(230)))));
            this.lvProgramHistoryList.FrozenColumnIndex = -1;
            this.lvProgramHistoryList.FullRowSelect = true;
            this.lvProgramHistoryList.GridDashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.lvProgramHistoryList.GridLineStyle = Adeng.Framework.Ctrl.AdengListViewGridLine.NONE;
            this.lvProgramHistoryList.HideSelection = false;
            this.lvProgramHistoryList.HoverSelection = false;
            this.lvProgramHistoryList.IconOffset = new System.Drawing.Point(1, 0);
            this.lvProgramHistoryList.InteraceColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.lvProgramHistoryList.InteraceColor2 = System.Drawing.Color.White;
            this.lvProgramHistoryList.ItemHeight = 18;
            this.lvProgramHistoryList.ItemOffset = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lvProgramHistoryList.Location = new System.Drawing.Point(7, 93);
            this.lvProgramHistoryList.MultiSelect = false;
            this.lvProgramHistoryList.Name = "lvProgramHistoryList";
            this.lvProgramHistoryList.ScrollType = Adeng.Framework.Ctrl.AdengListViewScrollType.BOTH;
            this.lvProgramHistoryList.SelectioinItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(232)))), ((int)(((byte)(250)))));
            this.lvProgramHistoryList.Size = new System.Drawing.Size(787, 533);
            this.lvProgramHistoryList.TabIndex = 19;
            this.lvProgramHistoryList.UseInteraceColor = true;
            this.lvProgramHistoryList.UseSelFocusedBar = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.panel1.Controls.Add(this.btnInquiryProgramHistory);
            this.panel1.Controls.Add(this.dtmPickerProgramDurtionStart);
            this.panel1.Controls.Add(this.dtmPickerProgramDurtionEnd);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Location = new System.Drawing.Point(7, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(787, 79);
            this.panel1.TabIndex = 18;
            // 
            // btnInquiryProgramHistory
            // 
            this.btnInquiryProgramHistory.ChkValue = false;
            this.btnInquiryProgramHistory.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInquiryProgramHistory.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnInquiryProgramHistory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(47)))));
            this.btnInquiryProgramHistory.Image = ((System.Drawing.Image)(resources.GetObject("btnInquiryProgramHistory.Image")));
            this.btnInquiryProgramHistory.ImgDisable = null;
            this.btnInquiryProgramHistory.ImgHover = null;
            this.btnInquiryProgramHistory.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnInquiryProgramHistory.ImgSelect")));
            this.btnInquiryProgramHistory.ImgStatusEvent = null;
            this.btnInquiryProgramHistory.ImgStatusNormal = null;
            this.btnInquiryProgramHistory.ImgStatusOffsetX = 2;
            this.btnInquiryProgramHistory.ImgStatusOffsetY = 0;
            this.btnInquiryProgramHistory.ImgStretch = false;
            this.btnInquiryProgramHistory.IsImgStatusNormal = true;
            this.btnInquiryProgramHistory.Location = new System.Drawing.Point(690, 13);
            this.btnInquiryProgramHistory.Name = "btnInquiryProgramHistory";
            this.btnInquiryProgramHistory.Size = new System.Drawing.Size(70, 53);
            this.btnInquiryProgramHistory.TabIndex = 43;
            this.btnInquiryProgramHistory.Text = "조회";
            this.btnInquiryProgramHistory.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnInquiryProgramHistory.UseChecked = false;
            this.btnInquiryProgramHistory.Click += new System.EventHandler(this.btnInquiryProgramHistory_Click);
            // 
            // dtmPickerProgramDurtionStart
            // 
            this.dtmPickerProgramDurtionStart.CustomFormat = " yyyy년 MM월 dd일 HH시";
            this.dtmPickerProgramDurtionStart.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.dtmPickerProgramDurtionStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtmPickerProgramDurtionStart.Location = new System.Drawing.Point(93, 27);
            this.dtmPickerProgramDurtionStart.Name = "dtmPickerProgramDurtionStart";
            this.dtmPickerProgramDurtionStart.Size = new System.Drawing.Size(178, 23);
            this.dtmPickerProgramDurtionStart.TabIndex = 38;
            this.dtmPickerProgramDurtionStart.ValueChanged += new System.EventHandler(this.dtmPickerProgramDurtionStart_ValueChanged);
            // 
            // dtmPickerProgramDurtionEnd
            // 
            this.dtmPickerProgramDurtionEnd.CustomFormat = " yyyy년 MM월 dd일 HH시";
            this.dtmPickerProgramDurtionEnd.Font = new System.Drawing.Font("맑은 고딕", 9F);
            this.dtmPickerProgramDurtionEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtmPickerProgramDurtionEnd.Location = new System.Drawing.Point(302, 27);
            this.dtmPickerProgramDurtionEnd.Name = "dtmPickerProgramDurtionEnd";
            this.dtmPickerProgramDurtionEnd.Size = new System.Drawing.Size(178, 23);
            this.dtmPickerProgramDurtionEnd.TabIndex = 39;
            this.dtmPickerProgramDurtionEnd.ValueChanged += new System.EventHandler(this.dtmPickerProgramDurtionEnd_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label4.Location = new System.Drawing.Point(279, 31);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(15, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "~";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label6.Location = new System.Drawing.Point(18, 27);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 23);
            this.label6.TabIndex = 0;
            this.label6.Text = "조회 기간";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // imglstEventLogIcons
            // 
            this.imglstEventLogIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglstEventLogIcons.ImageStream")));
            this.imglstEventLogIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imglstEventLogIcons.Images.SetKeyName(0, "information.png");
            this.imglstEventLogIcons.Images.SetKeyName(1, "warningRed.png");
            // 
            // InquiryHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 758);
            this.ControlBox = false;
            this.Controls.Add(this.tabCtlMain);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InquiryHistoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "이력 조회";
            this.pnlTop.ResumeLayout(false);
            this.tabCtlMain.ResumeLayout(false);
            this.tpOrderHistory.ResumeLayout(false);
            this.pnlSelectedOrderBack.ResumeLayout(false);
            this.pnlSelectedOrderBack.PerformLayout();
            this.tpProgramHistory.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Adeng.Framework.Ctrl.AdengImgButtonEx btnClose;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Panel pnlTop;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnClear;
        private System.Windows.Forms.TabControl tabCtlMain;
        private System.Windows.Forms.TabPage tpOrderHistory;
        private System.Windows.Forms.Panel pnlSelectedOrderBack;
        private System.Windows.Forms.Label lblInquiryDuration;
        private System.Windows.Forms.TabPage tpProgramHistory;
        private System.Windows.Forms.Label lblDisaster;
        private System.Windows.Forms.DateTimePicker dtmPickerOrderDurtionStart;
        private System.Windows.Forms.DateTimePicker dtmPickerOrderDurtionEnd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbboxDisasterKind;
        private System.Windows.Forms.ComboBox cmbboxDisasterCategory;
        private System.Windows.Forms.Label lblOrderLocation;
        private System.Windows.Forms.Label lblOrderMode;
        private System.Windows.Forms.ComboBox cmbboxOrderMode;
        private System.Windows.Forms.ComboBox cmbboxOrderLocation;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnInquriyOrderHistory;
        private Adeng.Framework.Ctrl.AdengListView lvOrderHistory;
        private Adeng.Framework.Ctrl.AdengListView lvProgramHistoryList;
        private System.Windows.Forms.Panel panel1;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnInquiryProgramHistory;
        private System.Windows.Forms.DateTimePicker dtmPickerProgramDurtionStart;
        private System.Windows.Forms.DateTimePicker dtmPickerProgramDurtionEnd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ImageList imglstEventLogIcons;
    }
}