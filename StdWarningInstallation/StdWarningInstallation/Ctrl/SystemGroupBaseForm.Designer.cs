namespace StdWarningInstallation
{
    partial class SystemGroupBaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemGroupBaseForm));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblDescription = new System.Windows.Forms.Label();
            this.pnlBodyBackground = new System.Windows.Forms.Panel();
            this.pnlTargetingBack = new System.Windows.Forms.Panel();
            this.lstboxInquiredSASList = new System.Windows.Forms.ListBox();
            this.lstboxTargetSASList = new System.Windows.Forms.ListBox();
            this.pnlFilteringBack = new System.Windows.Forms.Panel();
            this.cmbboxRegionLevel2 = new System.Windows.Forms.ComboBox();
            this.cmbboxSystemKind = new System.Windows.Forms.ComboBox();
            this.cmbboxRegionLevel1 = new System.Windows.Forms.ComboBox();
            this.lblSASKindSelectionTitle = new System.Windows.Forms.Label();
            this.lblRegionSelectionTitle = new System.Windows.Forms.Label();
            this.lblTargetingFilterTitle = new System.Windows.Forms.Label();
            this.btnFiltering = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.lblTargetListTitle = new System.Windows.Forms.Label();
            this.lblSystemListTitle = new System.Windows.Forms.Label();
            this.lblTargetSelectionTitle = new System.Windows.Forms.Label();
            this.btnRemoveFromTarget = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnToTarget = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlDisasterInfoBack = new System.Windows.Forms.Panel();
            this.cmbboxDisasterKind = new System.Windows.Forms.ComboBox();
            this.cmbboxDisasterCategory = new System.Windows.Forms.ComboBox();
            this.chkboxUseDisasterSet = new System.Windows.Forms.CheckBox();
            this.lblDisasterTitle = new System.Windows.Forms.Label();
            this.pnlGroupNameBack = new System.Windows.Forms.Panel();
            this.txtboxGroupName = new System.Windows.Forms.TextBox();
            this.lblGroupNameLimitCount = new System.Windows.Forms.Label();
            this.lblGroupNameTitle = new System.Windows.Forms.Label();
            this.btnOK = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnCancel = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnDeleteGroup = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlTop.SuspendLayout();
            this.pnlBodyBackground.SuspendLayout();
            this.pnlTargetingBack.SuspendLayout();
            this.pnlFilteringBack.SuspendLayout();
            this.pnlDisasterInfoBack.SuspendLayout();
            this.pnlGroupNameBack.SuspendLayout();
            this.SuspendLayout();
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
            this.pnlTop.Size = new System.Drawing.Size(566, 40);
            this.pnlTop.TabIndex = 3;
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
            this.lblDescription.Size = new System.Drawing.Size(566, 40);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "표준경보시스템을 선택하여 그룹을 생성합니다.";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBodyBackground
            // 
            this.pnlBodyBackground.BackColor = System.Drawing.Color.White;
            this.pnlBodyBackground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBodyBackground.Controls.Add(this.pnlTargetingBack);
            this.pnlBodyBackground.Controls.Add(this.pnlDisasterInfoBack);
            this.pnlBodyBackground.Controls.Add(this.pnlGroupNameBack);
            this.pnlBodyBackground.Location = new System.Drawing.Point(9, 49);
            this.pnlBodyBackground.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBodyBackground.Name = "pnlBodyBackground";
            this.pnlBodyBackground.Size = new System.Drawing.Size(548, 660);
            this.pnlBodyBackground.TabIndex = 5;
            // 
            // pnlTargetingBack
            // 
            this.pnlTargetingBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.pnlTargetingBack.Controls.Add(this.lstboxInquiredSASList);
            this.pnlTargetingBack.Controls.Add(this.lstboxTargetSASList);
            this.pnlTargetingBack.Controls.Add(this.pnlFilteringBack);
            this.pnlTargetingBack.Controls.Add(this.lblTargetListTitle);
            this.pnlTargetingBack.Controls.Add(this.lblSystemListTitle);
            this.pnlTargetingBack.Controls.Add(this.lblTargetSelectionTitle);
            this.pnlTargetingBack.Controls.Add(this.btnRemoveFromTarget);
            this.pnlTargetingBack.Controls.Add(this.btnToTarget);
            this.pnlTargetingBack.Location = new System.Drawing.Point(6, 140);
            this.pnlTargetingBack.Name = "pnlTargetingBack";
            this.pnlTargetingBack.Size = new System.Drawing.Size(534, 512);
            this.pnlTargetingBack.TabIndex = 1;
            // 
            // lstboxInquiredSASList
            // 
            this.lstboxInquiredSASList.BackColor = System.Drawing.Color.White;
            this.lstboxInquiredSASList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstboxInquiredSASList.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lstboxInquiredSASList.FormattingEnabled = true;
            this.lstboxInquiredSASList.ItemHeight = 17;
            this.lstboxInquiredSASList.Location = new System.Drawing.Point(26, 148);
            this.lstboxInquiredSASList.Margin = new System.Windows.Forms.Padding(0);
            this.lstboxInquiredSASList.Name = "lstboxInquiredSASList";
            this.lstboxInquiredSASList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstboxInquiredSASList.Size = new System.Drawing.Size(218, 342);
            this.lstboxInquiredSASList.TabIndex = 21;
            // 
            // lstboxTargetSASList
            // 
            this.lstboxTargetSASList.BackColor = System.Drawing.Color.White;
            this.lstboxTargetSASList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstboxTargetSASList.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lstboxTargetSASList.FormattingEnabled = true;
            this.lstboxTargetSASList.ItemHeight = 17;
            this.lstboxTargetSASList.Location = new System.Drawing.Point(290, 148);
            this.lstboxTargetSASList.Margin = new System.Windows.Forms.Padding(0);
            this.lstboxTargetSASList.Name = "lstboxTargetSASList";
            this.lstboxTargetSASList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstboxTargetSASList.Size = new System.Drawing.Size(218, 342);
            this.lstboxTargetSASList.TabIndex = 22;
            // 
            // pnlFilteringBack
            // 
            this.pnlFilteringBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.pnlFilteringBack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlFilteringBack.BackgroundImage")));
            this.pnlFilteringBack.Controls.Add(this.cmbboxRegionLevel2);
            this.pnlFilteringBack.Controls.Add(this.cmbboxSystemKind);
            this.pnlFilteringBack.Controls.Add(this.cmbboxRegionLevel1);
            this.pnlFilteringBack.Controls.Add(this.lblSASKindSelectionTitle);
            this.pnlFilteringBack.Controls.Add(this.lblRegionSelectionTitle);
            this.pnlFilteringBack.Controls.Add(this.lblTargetingFilterTitle);
            this.pnlFilteringBack.Controls.Add(this.btnFiltering);
            this.pnlFilteringBack.Location = new System.Drawing.Point(25, 34);
            this.pnlFilteringBack.Name = "pnlFilteringBack";
            this.pnlFilteringBack.Size = new System.Drawing.Size(484, 73);
            this.pnlFilteringBack.TabIndex = 1;
            // 
            // cmbboxRegionLevel2
            // 
            this.cmbboxRegionLevel2.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbboxRegionLevel2.FormattingEnabled = true;
            this.cmbboxRegionLevel2.ItemHeight = 17;
            this.cmbboxRegionLevel2.Location = new System.Drawing.Point(278, 10);
            this.cmbboxRegionLevel2.Name = "cmbboxRegionLevel2";
            this.cmbboxRegionLevel2.Size = new System.Drawing.Size(110, 25);
            this.cmbboxRegionLevel2.TabIndex = 1;
            this.cmbboxRegionLevel2.SelectedIndexChanged += new System.EventHandler(this.cmbboxRegionLevel2_SelectedIndexChanged);
            // 
            // cmbboxSystemKind
            // 
            this.cmbboxSystemKind.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbboxSystemKind.FormattingEnabled = true;
            this.cmbboxSystemKind.Location = new System.Drawing.Point(163, 41);
            this.cmbboxSystemKind.Name = "cmbboxSystemKind";
            this.cmbboxSystemKind.Size = new System.Drawing.Size(225, 23);
            this.cmbboxSystemKind.TabIndex = 2;
            this.cmbboxSystemKind.SelectedIndexChanged += new System.EventHandler(this.cmbboxSystemKind_SelectedIndexChanged);
            // 
            // cmbboxRegionLevel1
            // 
            this.cmbboxRegionLevel1.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbboxRegionLevel1.FormattingEnabled = true;
            this.cmbboxRegionLevel1.ItemHeight = 17;
            this.cmbboxRegionLevel1.Location = new System.Drawing.Point(163, 10);
            this.cmbboxRegionLevel1.Name = "cmbboxRegionLevel1";
            this.cmbboxRegionLevel1.Size = new System.Drawing.Size(110, 25);
            this.cmbboxRegionLevel1.TabIndex = 0;
            this.cmbboxRegionLevel1.SelectedIndexChanged += new System.EventHandler(this.cmbboxRegionLevel1_SelectedIndexChanged);
            // 
            // lblSASKindSelectionTitle
            // 
            this.lblSASKindSelectionTitle.AutoSize = true;
            this.lblSASKindSelectionTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSASKindSelectionTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSASKindSelectionTitle.ForeColor = System.Drawing.Color.Black;
            this.lblSASKindSelectionTitle.Location = new System.Drawing.Point(52, 42);
            this.lblSASKindSelectionTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblSASKindSelectionTitle.Name = "lblSASKindSelectionTitle";
            this.lblSASKindSelectionTitle.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblSASKindSelectionTitle.Size = new System.Drawing.Size(98, 15);
            this.lblSASKindSelectionTitle.TabIndex = 5;
            this.lblSASKindSelectionTitle.Text = "경보시스템 종류";
            // 
            // lblRegionSelectionTitle
            // 
            this.lblRegionSelectionTitle.AutoSize = true;
            this.lblRegionSelectionTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblRegionSelectionTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRegionSelectionTitle.ForeColor = System.Drawing.Color.Black;
            this.lblRegionSelectionTitle.Location = new System.Drawing.Point(52, 14);
            this.lblRegionSelectionTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblRegionSelectionTitle.Name = "lblRegionSelectionTitle";
            this.lblRegionSelectionTitle.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblRegionSelectionTitle.Size = new System.Drawing.Size(62, 15);
            this.lblRegionSelectionTitle.TabIndex = 6;
            this.lblRegionSelectionTitle.Text = "지역 선택";
            // 
            // lblTargetingFilterTitle
            // 
            this.lblTargetingFilterTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTargetingFilterTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTargetingFilterTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTargetingFilterTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblTargetingFilterTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTargetingFilterTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTargetingFilterTitle.Name = "lblTargetingFilterTitle";
            this.lblTargetingFilterTitle.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.lblTargetingFilterTitle.Size = new System.Drawing.Size(48, 73);
            this.lblTargetingFilterTitle.TabIndex = 0;
            this.lblTargetingFilterTitle.Text = "조건";
            this.lblTargetingFilterTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnFiltering
            // 
            this.btnFiltering.ChkValue = false;
            this.btnFiltering.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFiltering.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnFiltering.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(47)))));
            this.btnFiltering.Image = ((System.Drawing.Image)(resources.GetObject("btnFiltering.Image")));
            this.btnFiltering.ImgDisable = null;
            this.btnFiltering.ImgHover = null;
            this.btnFiltering.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnFiltering.ImgSelect")));
            this.btnFiltering.ImgStatusEvent = null;
            this.btnFiltering.ImgStatusNormal = null;
            this.btnFiltering.ImgStatusOffsetX = 2;
            this.btnFiltering.ImgStatusOffsetY = 0;
            this.btnFiltering.ImgStretch = false;
            this.btnFiltering.IsImgStatusNormal = true;
            this.btnFiltering.Location = new System.Drawing.Point(403, 10);
            this.btnFiltering.Name = "btnFiltering";
            this.btnFiltering.Size = new System.Drawing.Size(70, 53);
            this.btnFiltering.TabIndex = 4;
            this.btnFiltering.Text = "조회";
            this.btnFiltering.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnFiltering.UseChecked = false;
            this.btnFiltering.Click += new System.EventHandler(this.btnFiltering_Click);
            // 
            // lblTargetListTitle
            // 
            this.lblTargetListTitle.AutoSize = true;
            this.lblTargetListTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTargetListTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTargetListTitle.ForeColor = System.Drawing.Color.Black;
            this.lblTargetListTitle.Location = new System.Drawing.Point(289, 129);
            this.lblTargetListTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTargetListTitle.Name = "lblTargetListTitle";
            this.lblTargetListTitle.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblTargetListTitle.Size = new System.Drawing.Size(130, 15);
            this.lblTargetListTitle.TabIndex = 5;
            this.lblTargetListTitle.Text = "발령 대상 시스템 목록";
            // 
            // lblSystemListTitle
            // 
            this.lblSystemListTitle.AutoSize = true;
            this.lblSystemListTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSystemListTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSystemListTitle.ForeColor = System.Drawing.Color.Black;
            this.lblSystemListTitle.Location = new System.Drawing.Point(25, 129);
            this.lblSystemListTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblSystemListTitle.Name = "lblSystemListTitle";
            this.lblSystemListTitle.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblSystemListTitle.Size = new System.Drawing.Size(114, 15);
            this.lblSystemListTitle.TabIndex = 5;
            this.lblSystemListTitle.Text = "조회된 시스템 목록";
            // 
            // lblTargetSelectionTitle
            // 
            this.lblTargetSelectionTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTargetSelectionTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTargetSelectionTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblTargetSelectionTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTargetSelectionTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTargetSelectionTitle.Name = "lblTargetSelectionTitle";
            this.lblTargetSelectionTitle.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.lblTargetSelectionTitle.Size = new System.Drawing.Size(534, 33);
            this.lblTargetSelectionTitle.TabIndex = 0;
            this.lblTargetSelectionTitle.Text = "발령 대상 선택";
            this.lblTargetSelectionTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnRemoveFromTarget
            // 
            this.btnRemoveFromTarget.ChkValue = false;
            this.btnRemoveFromTarget.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRemoveFromTarget.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRemoveFromTarget.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(47)))));
            this.btnRemoveFromTarget.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveFromTarget.Image")));
            this.btnRemoveFromTarget.ImgDisable = null;
            this.btnRemoveFromTarget.ImgHover = null;
            this.btnRemoveFromTarget.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnRemoveFromTarget.ImgSelect")));
            this.btnRemoveFromTarget.ImgStatusEvent = null;
            this.btnRemoveFromTarget.ImgStatusNormal = null;
            this.btnRemoveFromTarget.ImgStatusOffsetX = 2;
            this.btnRemoveFromTarget.ImgStatusOffsetY = 0;
            this.btnRemoveFromTarget.ImgStretch = false;
            this.btnRemoveFromTarget.IsImgStatusNormal = true;
            this.btnRemoveFromTarget.Location = new System.Drawing.Point(249, 326);
            this.btnRemoveFromTarget.Name = "btnRemoveFromTarget";
            this.btnRemoveFromTarget.Size = new System.Drawing.Size(36, 53);
            this.btnRemoveFromTarget.TabIndex = 4;
            this.btnRemoveFromTarget.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnRemoveFromTarget.UseChecked = false;
            this.btnRemoveFromTarget.Click += new System.EventHandler(this.btnRemoveFromTarget_Click);
            // 
            // btnToTarget
            // 
            this.btnToTarget.ChkValue = false;
            this.btnToTarget.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToTarget.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnToTarget.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(47)))));
            this.btnToTarget.Image = ((System.Drawing.Image)(resources.GetObject("btnToTarget.Image")));
            this.btnToTarget.ImgDisable = null;
            this.btnToTarget.ImgHover = null;
            this.btnToTarget.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnToTarget.ImgSelect")));
            this.btnToTarget.ImgStatusEvent = null;
            this.btnToTarget.ImgStatusNormal = null;
            this.btnToTarget.ImgStatusOffsetX = 2;
            this.btnToTarget.ImgStatusOffsetY = 0;
            this.btnToTarget.ImgStretch = false;
            this.btnToTarget.IsImgStatusNormal = true;
            this.btnToTarget.Location = new System.Drawing.Point(249, 267);
            this.btnToTarget.Name = "btnToTarget";
            this.btnToTarget.Size = new System.Drawing.Size(36, 53);
            this.btnToTarget.TabIndex = 4;
            this.btnToTarget.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnToTarget.UseChecked = false;
            this.btnToTarget.Click += new System.EventHandler(this.btnToTarget_Click);
            // 
            // pnlDisasterInfoBack
            // 
            this.pnlDisasterInfoBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.pnlDisasterInfoBack.Controls.Add(this.cmbboxDisasterKind);
            this.pnlDisasterInfoBack.Controls.Add(this.cmbboxDisasterCategory);
            this.pnlDisasterInfoBack.Controls.Add(this.chkboxUseDisasterSet);
            this.pnlDisasterInfoBack.Controls.Add(this.lblDisasterTitle);
            this.pnlDisasterInfoBack.Location = new System.Drawing.Point(6, 59);
            this.pnlDisasterInfoBack.Name = "pnlDisasterInfoBack";
            this.pnlDisasterInfoBack.Size = new System.Drawing.Size(534, 75);
            this.pnlDisasterInfoBack.TabIndex = 0;
            // 
            // cmbboxDisasterKind
            // 
            this.cmbboxDisasterKind.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbboxDisasterKind.FormattingEnabled = true;
            this.cmbboxDisasterKind.Location = new System.Drawing.Point(275, 38);
            this.cmbboxDisasterKind.Name = "cmbboxDisasterKind";
            this.cmbboxDisasterKind.Size = new System.Drawing.Size(183, 25);
            this.cmbboxDisasterKind.TabIndex = 1;
            this.cmbboxDisasterKind.SelectedIndexChanged += new System.EventHandler(this.cmbboxDisasterKind_SelectedIndexChanged);
            // 
            // cmbboxDisasterCategory
            // 
            this.cmbboxDisasterCategory.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbboxDisasterCategory.FormattingEnabled = true;
            this.cmbboxDisasterCategory.ItemHeight = 17;
            this.cmbboxDisasterCategory.Location = new System.Drawing.Point(86, 38);
            this.cmbboxDisasterCategory.Name = "cmbboxDisasterCategory";
            this.cmbboxDisasterCategory.Size = new System.Drawing.Size(183, 25);
            this.cmbboxDisasterCategory.TabIndex = 0;
            this.cmbboxDisasterCategory.SelectedIndexChanged += new System.EventHandler(this.cmbboxDisasterCategory_SelectedIndexChanged);
            // 
            // chkboxUseDisasterSet
            // 
            this.chkboxUseDisasterSet.AutoSize = true;
            this.chkboxUseDisasterSet.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkboxUseDisasterSet.Location = new System.Drawing.Point(11, 11);
            this.chkboxUseDisasterSet.Name = "chkboxUseDisasterSet";
            this.chkboxUseDisasterSet.Size = new System.Drawing.Size(246, 19);
            this.chkboxUseDisasterSet.TabIndex = 1;
            this.chkboxUseDisasterSet.Text = "그룹의 기본 재난 종류를 지정(생략 가능)";
            this.chkboxUseDisasterSet.UseVisualStyleBackColor = true;
            this.chkboxUseDisasterSet.CheckedChanged += new System.EventHandler(this.chkboxUseDisasterSet_CheckedChanged);
            // 
            // lblDisasterTitle
            // 
            this.lblDisasterTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDisasterTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblDisasterTitle.Location = new System.Drawing.Point(0, 33);
            this.lblDisasterTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblDisasterTitle.Name = "lblDisasterTitle";
            this.lblDisasterTitle.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.lblDisasterTitle.Size = new System.Drawing.Size(83, 33);
            this.lblDisasterTitle.TabIndex = 0;
            this.lblDisasterTitle.Text = "재난 종류";
            this.lblDisasterTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlGroupNameBack
            // 
            this.pnlGroupNameBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.pnlGroupNameBack.Controls.Add(this.txtboxGroupName);
            this.pnlGroupNameBack.Controls.Add(this.lblGroupNameLimitCount);
            this.pnlGroupNameBack.Controls.Add(this.lblGroupNameTitle);
            this.pnlGroupNameBack.Location = new System.Drawing.Point(6, 6);
            this.pnlGroupNameBack.Name = "pnlGroupNameBack";
            this.pnlGroupNameBack.Size = new System.Drawing.Size(534, 45);
            this.pnlGroupNameBack.TabIndex = 0;
            // 
            // txtboxGroupName
            // 
            this.txtboxGroupName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtboxGroupName.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxGroupName.Location = new System.Drawing.Point(86, 11);
            this.txtboxGroupName.MaxLength = 20;
            this.txtboxGroupName.Name = "txtboxGroupName";
            this.txtboxGroupName.Size = new System.Drawing.Size(312, 25);
            this.txtboxGroupName.TabIndex = 1;
            this.txtboxGroupName.Text = "한둘셋넷닷예일여아열한둘셋넷닷예일여아열";
            this.txtboxGroupName.TextChanged += new System.EventHandler(this.txtboxGroupName_TextChanged);
            // 
            // lblGroupNameLimitCount
            // 
            this.lblGroupNameLimitCount.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblGroupNameLimitCount.Font = new System.Drawing.Font("맑은 고딕", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblGroupNameLimitCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblGroupNameLimitCount.Location = new System.Drawing.Point(401, 0);
            this.lblGroupNameLimitCount.Margin = new System.Windows.Forms.Padding(0);
            this.lblGroupNameLimitCount.Name = "lblGroupNameLimitCount";
            this.lblGroupNameLimitCount.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblGroupNameLimitCount.Size = new System.Drawing.Size(133, 45);
            this.lblGroupNameLimitCount.TabIndex = 2;
            this.lblGroupNameLimitCount.Text = "(최대20자)";
            this.lblGroupNameLimitCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblGroupNameTitle
            // 
            this.lblGroupNameTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblGroupNameTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblGroupNameTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblGroupNameTitle.Location = new System.Drawing.Point(0, 0);
            this.lblGroupNameTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblGroupNameTitle.Name = "lblGroupNameTitle";
            this.lblGroupNameTitle.Padding = new System.Windows.Forms.Padding(9, 0, 0, 0);
            this.lblGroupNameTitle.Size = new System.Drawing.Size(83, 45);
            this.lblGroupNameTitle.TabIndex = 0;
            this.lblGroupNameTitle.Text = "그룹 이름";
            this.lblGroupNameTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.ChkValue = false;
            this.btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOK.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.ImgDisable = null;
            this.btnOK.ImgHover = null;
            this.btnOK.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnOK.ImgSelect")));
            this.btnOK.ImgStatusEvent = null;
            this.btnOK.ImgStatusNormal = null;
            this.btnOK.ImgStatusOffsetX = 2;
            this.btnOK.ImgStatusOffsetY = 0;
            this.btnOK.ImgStretch = true;
            this.btnOK.IsImgStatusNormal = true;
            this.btnOK.Location = new System.Drawing.Point(351, 720);
            this.btnOK.Margin = new System.Windows.Forms.Padding(0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 36);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "그룹 등록";
            this.btnOK.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnOK.UseChecked = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ChkValue = false;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImgDisable = null;
            this.btnCancel.ImgHover = null;
            this.btnCancel.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImgSelect")));
            this.btnCancel.ImgStatusEvent = null;
            this.btnCancel.ImgStatusNormal = null;
            this.btnCancel.ImgStatusOffsetX = 2;
            this.btnCancel.ImgStatusOffsetY = 0;
            this.btnCancel.ImgStretch = true;
            this.btnCancel.IsImgStatusNormal = true;
            this.btnCancel.Location = new System.Drawing.Point(457, 720);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 36);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "닫기";
            this.btnCancel.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnCancel.UseChecked = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDeleteGroup
            // 
            this.btnDeleteGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteGroup.ChkValue = false;
            this.btnDeleteGroup.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDeleteGroup.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDeleteGroup.ForeColor = System.Drawing.Color.White;
            this.btnDeleteGroup.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteGroup.Image")));
            this.btnDeleteGroup.ImgDisable = null;
            this.btnDeleteGroup.ImgHover = null;
            this.btnDeleteGroup.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnDeleteGroup.ImgSelect")));
            this.btnDeleteGroup.ImgStatusEvent = null;
            this.btnDeleteGroup.ImgStatusNormal = null;
            this.btnDeleteGroup.ImgStatusOffsetX = 2;
            this.btnDeleteGroup.ImgStatusOffsetY = 0;
            this.btnDeleteGroup.ImgStretch = true;
            this.btnDeleteGroup.IsImgStatusNormal = true;
            this.btnDeleteGroup.Location = new System.Drawing.Point(244, 720);
            this.btnDeleteGroup.Margin = new System.Windows.Forms.Padding(0);
            this.btnDeleteGroup.Name = "btnDeleteGroup";
            this.btnDeleteGroup.Size = new System.Drawing.Size(100, 36);
            this.btnDeleteGroup.TabIndex = 4;
            this.btnDeleteGroup.Text = "그룹 삭제";
            this.btnDeleteGroup.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnDeleteGroup.UseChecked = false;
            this.btnDeleteGroup.Click += new System.EventHandler(this.btnDeleteGroup_Click);
            // 
            // SystemGroupBaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 765);
            this.ControlBox = false;
            this.Controls.Add(this.pnlBodyBackground);
            this.Controls.Add(this.btnDeleteGroup);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pnlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SystemGroupBaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "시스템 그룹";
            this.Load += new System.EventHandler(this.LoadForm);
            this.pnlTop.ResumeLayout(false);
            this.pnlBodyBackground.ResumeLayout(false);
            this.pnlTargetingBack.ResumeLayout(false);
            this.pnlTargetingBack.PerformLayout();
            this.pnlFilteringBack.ResumeLayout(false);
            this.pnlFilteringBack.PerformLayout();
            this.pnlDisasterInfoBack.ResumeLayout(false);
            this.pnlDisasterInfoBack.PerformLayout();
            this.pnlGroupNameBack.ResumeLayout(false);
            this.pnlGroupNameBack.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Panel pnlBodyBackground;
        private System.Windows.Forms.Panel pnlGroupNameBack;
        private System.Windows.Forms.Label lblGroupNameTitle;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnFiltering;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnOK;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnCancel;
        private System.Windows.Forms.Panel pnlDisasterInfoBack;
        private System.Windows.Forms.Label lblDisasterTitle;
        private System.Windows.Forms.TextBox txtboxGroupName;
        private System.Windows.Forms.CheckBox chkboxUseDisasterSet;
        private System.Windows.Forms.Label lblGroupNameLimitCount;
        private System.Windows.Forms.ComboBox cmbboxDisasterKind;
        private System.Windows.Forms.ComboBox cmbboxDisasterCategory;
        private System.Windows.Forms.Panel pnlTargetingBack;
        private System.Windows.Forms.Label lblTargetSelectionTitle;
        private System.Windows.Forms.Panel pnlFilteringBack;
        private System.Windows.Forms.Label lblTargetingFilterTitle;
        private System.Windows.Forms.ComboBox cmbboxRegionLevel2;
        private System.Windows.Forms.ComboBox cmbboxRegionLevel1;
        private System.Windows.Forms.Label lblSASKindSelectionTitle;
        private System.Windows.Forms.Label lblRegionSelectionTitle;
        private System.Windows.Forms.ComboBox cmbboxSystemKind;
        private System.Windows.Forms.ListBox lstboxTargetSASList;
        private System.Windows.Forms.ListBox lstboxInquiredSASList;
        private System.Windows.Forms.Label lblTargetListTitle;
        private System.Windows.Forms.Label lblSystemListTitle;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnRemoveFromTarget;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnToTarget;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnDeleteGroup;
    }
}