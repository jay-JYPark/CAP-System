namespace StdWarningInstallation.Ctrl
{
    partial class RegionGroupBaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RegionGroupBaseForm));
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("읍면동1");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("읍면동2");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("읍면동3");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("읍면동4");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("읍면동5");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("시군구1", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5});
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("시군구2");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("시군구3");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("시군구4");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("강원도", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9});
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblDescription = new System.Windows.Forms.Label();
            this.pnlGroupNameBack = new System.Windows.Forms.Panel();
            this.txtboxGroupName = new System.Windows.Forms.TextBox();
            this.lblGroupNameLimitCount = new System.Windows.Forms.Label();
            this.lblGroupNameTitle = new System.Windows.Forms.Label();
            this.cmbboxDisasterKind = new System.Windows.Forms.ComboBox();
            this.cmbboxDisasterCategory = new System.Windows.Forms.ComboBox();
            this.lblSystemKindListTitle = new System.Windows.Forms.Label();
            this.chkboxUseDisasterSet = new System.Windows.Forms.CheckBox();
            this.lblDisasterTitle = new System.Windows.Forms.Label();
            this.btnCancel = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnOK = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlDisasterInfoBack = new System.Windows.Forms.Panel();
            this.lblTargetSelectionTitle = new System.Windows.Forms.Label();
            this.pnlTargetingBack = new System.Windows.Forms.Panel();
            this.pnlSASKindListBack = new System.Windows.Forms.Panel();
            this.chkListBoxSASKind = new System.Windows.Forms.CheckedListBox();
            this.tviewRegionList = new Adeng.Framework.Ctrl.TreeViewEx();
            this.lblRegionListTitle = new System.Windows.Forms.Label();
            this.pnlBodyBackground = new System.Windows.Forms.Panel();
            this.btnDeleteGroup = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlTop.SuspendLayout();
            this.pnlGroupNameBack.SuspendLayout();
            this.pnlDisasterInfoBack.SuspendLayout();
            this.pnlTargetingBack.SuspendLayout();
            this.pnlSASKindListBack.SuspendLayout();
            this.pnlBodyBackground.SuspendLayout();
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
            this.pnlTop.TabIndex = 6;
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
            this.lblDescription.Text = "지역을 선택하여 그룹을 생성합니다.";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // lblSystemKindListTitle
            // 
            this.lblSystemKindListTitle.AutoSize = true;
            this.lblSystemKindListTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSystemKindListTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSystemKindListTitle.ForeColor = System.Drawing.Color.Black;
            this.lblSystemKindListTitle.Location = new System.Drawing.Point(274, 38);
            this.lblSystemKindListTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblSystemKindListTitle.Name = "lblSystemKindListTitle";
            this.lblSystemKindListTitle.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblSystemKindListTitle.Size = new System.Drawing.Size(130, 15);
            this.lblSystemKindListTitle.TabIndex = 5;
            this.lblSystemKindListTitle.Text = "발령 대상 시스템 종류";
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
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ChkValue = false;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
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
            this.btnCancel.Location = new System.Drawing.Point(450, 720);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 36);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "닫기";
            this.btnCancel.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnCancel.UseChecked = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
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
            this.btnOK.Location = new System.Drawing.Point(343, 720);
            this.btnOK.Margin = new System.Windows.Forms.Padding(0);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 36);
            this.btnOK.TabIndex = 8;
            this.btnOK.Text = "그룹 등록";
            this.btnOK.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnOK.UseChecked = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
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
            // pnlTargetingBack
            // 
            this.pnlTargetingBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.pnlTargetingBack.Controls.Add(this.pnlSASKindListBack);
            this.pnlTargetingBack.Controls.Add(this.tviewRegionList);
            this.pnlTargetingBack.Controls.Add(this.lblSystemKindListTitle);
            this.pnlTargetingBack.Controls.Add(this.lblRegionListTitle);
            this.pnlTargetingBack.Controls.Add(this.lblTargetSelectionTitle);
            this.pnlTargetingBack.Location = new System.Drawing.Point(6, 140);
            this.pnlTargetingBack.Name = "pnlTargetingBack";
            this.pnlTargetingBack.Size = new System.Drawing.Size(534, 512);
            this.pnlTargetingBack.TabIndex = 1;
            // 
            // pnlSASKindListBack
            // 
            this.pnlSASKindListBack.BackColor = System.Drawing.Color.White;
            this.pnlSASKindListBack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSASKindListBack.Controls.Add(this.chkListBoxSASKind);
            this.pnlSASKindListBack.Location = new System.Drawing.Point(275, 57);
            this.pnlSASKindListBack.Name = "pnlSASKindListBack";
            this.pnlSASKindListBack.Size = new System.Drawing.Size(229, 236);
            this.pnlSASKindListBack.TabIndex = 14;
            // 
            // chkListBoxSASKind
            // 
            this.chkListBoxSASKind.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chkListBoxSASKind.CheckOnClick = true;
            this.chkListBoxSASKind.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkListBoxSASKind.FormattingEnabled = true;
            this.chkListBoxSASKind.HorizontalScrollbar = true;
            this.chkListBoxSASKind.Location = new System.Drawing.Point(7, 7);
            this.chkListBoxSASKind.Margin = new System.Windows.Forms.Padding(0);
            this.chkListBoxSASKind.Name = "chkListBoxSASKind";
            this.chkListBoxSASKind.Size = new System.Drawing.Size(213, 220);
            this.chkListBoxSASKind.TabIndex = 13;
            // 
            // tviewRegionList
            // 
            this.tviewRegionList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tviewRegionList.CheckBoxes = true;
            this.tviewRegionList.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tviewRegionList.Location = new System.Drawing.Point(26, 57);
            this.tviewRegionList.Margin = new System.Windows.Forms.Padding(0);
            this.tviewRegionList.Name = "tviewRegionList";
            treeNode1.Name = "노드6";
            treeNode1.Text = "읍면동1";
            treeNode2.Name = "노드7";
            treeNode2.Text = "읍면동2";
            treeNode3.Name = "노드8";
            treeNode3.Text = "읍면동3";
            treeNode4.Name = "노드9";
            treeNode4.Text = "읍면동4";
            treeNode5.Name = "노드10";
            treeNode5.Text = "읍면동5";
            treeNode6.Name = "노드2";
            treeNode6.Text = "시군구1";
            treeNode7.Name = "노드3";
            treeNode7.Text = "시군구2";
            treeNode8.Name = "노드4";
            treeNode8.Text = "시군구3";
            treeNode9.Name = "노드5";
            treeNode9.Text = "시군구4";
            treeNode10.Name = "노드0";
            treeNode10.Text = "강원도";
            this.tviewRegionList.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode10});
            this.tviewRegionList.Size = new System.Drawing.Size(229, 432);
            this.tviewRegionList.TabIndex = 6;
            this.tviewRegionList.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tviewRegionList_AfterCheck);
            // 
            // lblRegionListTitle
            // 
            this.lblRegionListTitle.AutoSize = true;
            this.lblRegionListTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblRegionListTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRegionListTitle.ForeColor = System.Drawing.Color.Black;
            this.lblRegionListTitle.Location = new System.Drawing.Point(25, 38);
            this.lblRegionListTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblRegionListTitle.Name = "lblRegionListTitle";
            this.lblRegionListTitle.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblRegionListTitle.Size = new System.Drawing.Size(90, 15);
            this.lblRegionListTitle.TabIndex = 5;
            this.lblRegionListTitle.Text = "발령 대상 지역";
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
            this.pnlBodyBackground.TabIndex = 10;
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
            this.btnDeleteGroup.Location = new System.Drawing.Point(236, 720);
            this.btnDeleteGroup.Margin = new System.Windows.Forms.Padding(0);
            this.btnDeleteGroup.Name = "btnDeleteGroup";
            this.btnDeleteGroup.Size = new System.Drawing.Size(100, 36);
            this.btnDeleteGroup.TabIndex = 7;
            this.btnDeleteGroup.Text = "그룹 삭제";
            this.btnDeleteGroup.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnDeleteGroup.UseChecked = false;
            this.btnDeleteGroup.Click += new System.EventHandler(this.btnDeleteGroup_Click);
            // 
            // RegionGroupBaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 765);
            this.ControlBox = false;
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pnlBodyBackground);
            this.Controls.Add(this.btnDeleteGroup);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "RegionGroupBaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RegionGroupBaseForm";
            this.Load += new System.EventHandler(this.LoadForm);
            this.pnlTop.ResumeLayout(false);
            this.pnlGroupNameBack.ResumeLayout(false);
            this.pnlGroupNameBack.PerformLayout();
            this.pnlDisasterInfoBack.ResumeLayout(false);
            this.pnlDisasterInfoBack.PerformLayout();
            this.pnlTargetingBack.ResumeLayout(false);
            this.pnlTargetingBack.PerformLayout();
            this.pnlSASKindListBack.ResumeLayout(false);
            this.pnlBodyBackground.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Panel pnlGroupNameBack;
        private System.Windows.Forms.TextBox txtboxGroupName;
        private System.Windows.Forms.Label lblGroupNameLimitCount;
        private System.Windows.Forms.Label lblGroupNameTitle;
        private System.Windows.Forms.ComboBox cmbboxDisasterKind;
        private System.Windows.Forms.ComboBox cmbboxDisasterCategory;
        private System.Windows.Forms.Label lblSystemKindListTitle;
        private System.Windows.Forms.CheckBox chkboxUseDisasterSet;
        private System.Windows.Forms.Label lblDisasterTitle;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnCancel;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnOK;
        private System.Windows.Forms.Panel pnlDisasterInfoBack;
        private System.Windows.Forms.Label lblTargetSelectionTitle;
        private System.Windows.Forms.Panel pnlTargetingBack;
        private System.Windows.Forms.Label lblRegionListTitle;
        private System.Windows.Forms.Panel pnlBodyBackground;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnDeleteGroup;
        private Adeng.Framework.Ctrl.TreeViewEx tviewRegionList;
        private System.Windows.Forms.Panel pnlSASKindListBack;
        private System.Windows.Forms.CheckedListBox chkListBoxSASKind;

    }
}