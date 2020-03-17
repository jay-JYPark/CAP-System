namespace StdWarningInstallation
{
    partial class UserAccountForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserAccountForm));
            this.btnDelete = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnModify = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlBodyBack = new System.Windows.Forms.Panel();
            this.pnlDetailBack = new System.Windows.Forms.Panel();
            this.btnCheckUnique = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtboxDescription = new Adeng.Framework.Ctrl.TextBoxEx();
            this.txtboxPhone = new Adeng.Framework.Ctrl.TextBoxEx();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtboxDepartment = new Adeng.Framework.Ctrl.TextBoxEx();
            this.txtboxName = new Adeng.Framework.Ctrl.TextBoxEx();
            this.txtboxPassowrd = new Adeng.Framework.Ctrl.TextBoxEx();
            this.txtboxID = new Adeng.Framework.Ctrl.TextBoxEx();
            this.pnlClearWaitingList = new System.Windows.Forms.Panel();
            this.lvAccountList = new Adeng.Framework.Ctrl.AdengListView();
            this.lblClearWaitingListTitle = new System.Windows.Forms.Label();
            this.btnRegist = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnClose = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnSave = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnCancel = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlTop.SuspendLayout();
            this.pnlBodyBack.SuspendLayout();
            this.pnlDetailBack.SuspendLayout();
            this.pnlClearWaitingList.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.ChkValue = false;
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImgDisable = null;
            this.btnDelete.ImgHover = null;
            this.btnDelete.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnDelete.ImgSelect")));
            this.btnDelete.ImgStatusEvent = null;
            this.btnDelete.ImgStatusNormal = null;
            this.btnDelete.ImgStatusOffsetX = 2;
            this.btnDelete.ImgStatusOffsetY = 0;
            this.btnDelete.ImgStretch = true;
            this.btnDelete.IsImgStatusNormal = true;
            this.btnDelete.Location = new System.Drawing.Point(488, 353);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 36);
            this.btnDelete.TabIndex = 53;
            this.btnDelete.Text = "삭제";
            this.btnDelete.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnDelete.UseChecked = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
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
            this.pnlTop.Size = new System.Drawing.Size(702, 40);
            this.pnlTop.TabIndex = 51;
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
            this.lblDescription.Size = new System.Drawing.Size(702, 40);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "사용자 정보를 등록하거나 정보를 확인하고 편집합니다.";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnModify
            // 
            this.btnModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnModify.ChkValue = false;
            this.btnModify.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModify.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnModify.ForeColor = System.Drawing.Color.White;
            this.btnModify.Image = ((System.Drawing.Image)(resources.GetObject("btnModify.Image")));
            this.btnModify.ImgDisable = null;
            this.btnModify.ImgHover = null;
            this.btnModify.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnModify.ImgSelect")));
            this.btnModify.ImgStatusEvent = null;
            this.btnModify.ImgStatusNormal = null;
            this.btnModify.ImgStatusOffsetX = 2;
            this.btnModify.ImgStatusOffsetY = 0;
            this.btnModify.ImgStretch = true;
            this.btnModify.IsImgStatusNormal = true;
            this.btnModify.Location = new System.Drawing.Point(382, 353);
            this.btnModify.Margin = new System.Windows.Forms.Padding(0);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(100, 36);
            this.btnModify.TabIndex = 52;
            this.btnModify.Text = "수정";
            this.btnModify.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnModify.UseChecked = false;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // pnlBodyBack
            // 
            this.pnlBodyBack.BackColor = System.Drawing.Color.White;
            this.pnlBodyBack.Controls.Add(this.pnlDetailBack);
            this.pnlBodyBack.Controls.Add(this.pnlClearWaitingList);
            this.pnlBodyBack.Location = new System.Drawing.Point(7, 48);
            this.pnlBodyBack.Name = "pnlBodyBack";
            this.pnlBodyBack.Size = new System.Drawing.Size(688, 292);
            this.pnlBodyBack.TabIndex = 54;
            // 
            // pnlDetailBack
            // 
            this.pnlDetailBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.pnlDetailBack.Controls.Add(this.btnCheckUnique);
            this.pnlDetailBack.Controls.Add(this.label2);
            this.pnlDetailBack.Controls.Add(this.label3);
            this.pnlDetailBack.Controls.Add(this.txtboxDescription);
            this.pnlDetailBack.Controls.Add(this.txtboxPhone);
            this.pnlDetailBack.Controls.Add(this.label7);
            this.pnlDetailBack.Controls.Add(this.label6);
            this.pnlDetailBack.Controls.Add(this.label5);
            this.pnlDetailBack.Controls.Add(this.label4);
            this.pnlDetailBack.Controls.Add(this.txtboxDepartment);
            this.pnlDetailBack.Controls.Add(this.txtboxName);
            this.pnlDetailBack.Controls.Add(this.txtboxPassowrd);
            this.pnlDetailBack.Controls.Add(this.txtboxID);
            this.pnlDetailBack.Enabled = false;
            this.pnlDetailBack.Location = new System.Drawing.Point(303, 7);
            this.pnlDetailBack.Name = "pnlDetailBack";
            this.pnlDetailBack.Size = new System.Drawing.Size(378, 278);
            this.pnlDetailBack.TabIndex = 15;
            // 
            // btnCheckUnique
            // 
            this.btnCheckUnique.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheckUnique.ChkValue = false;
            this.btnCheckUnique.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCheckUnique.Enabled = false;
            this.btnCheckUnique.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCheckUnique.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(47)))));
            this.btnCheckUnique.Image = ((System.Drawing.Image)(resources.GetObject("btnCheckUnique.Image")));
            this.btnCheckUnique.ImgDisable = null;
            this.btnCheckUnique.ImgHover = null;
            this.btnCheckUnique.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnCheckUnique.ImgSelect")));
            this.btnCheckUnique.ImgStatusEvent = null;
            this.btnCheckUnique.ImgStatusNormal = null;
            this.btnCheckUnique.ImgStatusOffsetX = 2;
            this.btnCheckUnique.ImgStatusOffsetY = 0;
            this.btnCheckUnique.ImgStretch = true;
            this.btnCheckUnique.IsImgStatusNormal = true;
            this.btnCheckUnique.Location = new System.Drawing.Point(274, 30);
            this.btnCheckUnique.Name = "btnCheckUnique";
            this.btnCheckUnique.Size = new System.Drawing.Size(79, 25);
            this.btnCheckUnique.TabIndex = 50;
            this.btnCheckUnique.Text = "중복 체크";
            this.btnCheckUnique.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnCheckUnique.UseChecked = false;
            this.btnCheckUnique.Visible = false;
            this.btnCheckUnique.Click += new System.EventHandler(this.btnCheckUnique_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label2.Location = new System.Drawing.Point(25, 175);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 25);
            this.label2.TabIndex = 47;
            this.label2.Text = "설명";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label3.Location = new System.Drawing.Point(25, 146);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 25);
            this.label3.TabIndex = 48;
            this.label3.Text = "연락처";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtboxDescription
            // 
            this.txtboxDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtboxDescription.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxDescription.Location = new System.Drawing.Point(102, 175);
            this.txtboxDescription.MaxLength = 256;
            this.txtboxDescription.Multiline = true;
            this.txtboxDescription.Name = "txtboxDescription";
            this.txtboxDescription.Size = new System.Drawing.Size(251, 80);
            this.txtboxDescription.TabIndex = 46;
            // 
            // txtboxPhone
            // 
            this.txtboxPhone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtboxPhone.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxPhone.Location = new System.Drawing.Point(102, 146);
            this.txtboxPhone.MaxLength = 16;
            this.txtboxPhone.Name = "txtboxPhone";
            this.txtboxPhone.Size = new System.Drawing.Size(251, 25);
            this.txtboxPhone.TabIndex = 45;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label7.Location = new System.Drawing.Point(25, 117);
            this.label7.Margin = new System.Windows.Forms.Padding(0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 25);
            this.label7.TabIndex = 41;
            this.label7.Text = "소속";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label6.Location = new System.Drawing.Point(25, 88);
            this.label6.Margin = new System.Windows.Forms.Padding(0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 25);
            this.label6.TabIndex = 42;
            this.label6.Text = "이름(*)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label5.Location = new System.Drawing.Point(25, 59);
            this.label5.Margin = new System.Windows.Forms.Padding(0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 25);
            this.label5.TabIndex = 43;
            this.label5.Text = "비밀번호(*)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label4.Location = new System.Drawing.Point(25, 30);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 25);
            this.label4.TabIndex = 44;
            this.label4.Text = "아이디(*)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtboxDepartment
            // 
            this.txtboxDepartment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtboxDepartment.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxDepartment.Location = new System.Drawing.Point(102, 117);
            this.txtboxDepartment.MaxLength = 128;
            this.txtboxDepartment.Name = "txtboxDepartment";
            this.txtboxDepartment.Size = new System.Drawing.Size(251, 25);
            this.txtboxDepartment.TabIndex = 40;
            // 
            // txtboxName
            // 
            this.txtboxName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtboxName.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxName.Location = new System.Drawing.Point(102, 88);
            this.txtboxName.MaxLength = 32;
            this.txtboxName.Name = "txtboxName";
            this.txtboxName.Size = new System.Drawing.Size(251, 25);
            this.txtboxName.TabIndex = 39;
            // 
            // txtboxPassowrd
            // 
            this.txtboxPassowrd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtboxPassowrd.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxPassowrd.Location = new System.Drawing.Point(102, 59);
            this.txtboxPassowrd.MaxLength = 16;
            this.txtboxPassowrd.Name = "txtboxPassowrd";
            this.txtboxPassowrd.Size = new System.Drawing.Size(251, 25);
            this.txtboxPassowrd.TabIndex = 38;
            this.txtboxPassowrd.UseSystemPasswordChar = true;
            // 
            // txtboxID
            // 
            this.txtboxID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtboxID.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxID.Location = new System.Drawing.Point(102, 30);
            this.txtboxID.MaxLength = 10;
            this.txtboxID.Name = "txtboxID";
            this.txtboxID.Size = new System.Drawing.Size(166, 25);
            this.txtboxID.TabIndex = 37;
            this.txtboxID.TextChanged += new System.EventHandler(this.txtboxID_TextChanged);
            // 
            // pnlClearWaitingList
            // 
            this.pnlClearWaitingList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.pnlClearWaitingList.Controls.Add(this.lvAccountList);
            this.pnlClearWaitingList.Controls.Add(this.lblClearWaitingListTitle);
            this.pnlClearWaitingList.Location = new System.Drawing.Point(7, 7);
            this.pnlClearWaitingList.Name = "pnlClearWaitingList";
            this.pnlClearWaitingList.Size = new System.Drawing.Size(290, 278);
            this.pnlClearWaitingList.TabIndex = 1;
            // 
            // lvAccountList
            // 
            this.lvAccountList.AntiAlias = false;
            this.lvAccountList.AutoFit = false;
            this.lvAccountList.BackColor = System.Drawing.Color.White;
            this.lvAccountList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvAccountList.ColumnHeight = 24;
            this.lvAccountList.ColumnOffset = new System.Windows.Forms.Padding(2, 2, 0, 1);
            this.lvAccountList.ColumnStyle = Adeng.Framework.Ctrl.AdengListViewColumnStyle.DEF_STYLE;
            this.lvAccountList.FocusedItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(185)))), ((int)(((byte)(230)))));
            this.lvAccountList.FrozenColumnIndex = -1;
            this.lvAccountList.FullRowSelect = true;
            this.lvAccountList.GridDashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.lvAccountList.GridLineStyle = Adeng.Framework.Ctrl.AdengListViewGridLine.NONE;
            this.lvAccountList.HideSelection = false;
            this.lvAccountList.HoverSelection = false;
            this.lvAccountList.IconOffset = new System.Drawing.Point(1, 0);
            this.lvAccountList.InteraceColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.lvAccountList.InteraceColor2 = System.Drawing.Color.White;
            this.lvAccountList.ItemHeight = 18;
            this.lvAccountList.ItemOffset = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lvAccountList.Location = new System.Drawing.Point(7, 36);
            this.lvAccountList.MultiSelect = false;
            this.lvAccountList.Name = "lvAccountList";
            this.lvAccountList.ScrollType = Adeng.Framework.Ctrl.AdengListViewScrollType.BOTH;
            this.lvAccountList.SelectioinItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(232)))), ((int)(((byte)(250)))));
            this.lvAccountList.Size = new System.Drawing.Size(276, 236);
            this.lvAccountList.TabIndex = 14;
            this.lvAccountList.UseInteraceColor = true;
            this.lvAccountList.UseSelFocusedBar = false;
            this.lvAccountList.ItemSelectionChanged += new Adeng.Framework.Ctrl.AdengListViewItemSelectionChangedEventHandler(this.lvAccountList_ItemSelectionChanged);
            // 
            // lblClearWaitingListTitle
            // 
            this.lblClearWaitingListTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblClearWaitingListTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblClearWaitingListTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblClearWaitingListTitle.Location = new System.Drawing.Point(0, 0);
            this.lblClearWaitingListTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblClearWaitingListTitle.Name = "lblClearWaitingListTitle";
            this.lblClearWaitingListTitle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblClearWaitingListTitle.Size = new System.Drawing.Size(290, 32);
            this.lblClearWaitingListTitle.TabIndex = 0;
            this.lblClearWaitingListTitle.Text = "사용자 목록";
            this.lblClearWaitingListTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnRegist
            // 
            this.btnRegist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegist.ChkValue = false;
            this.btnRegist.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRegist.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnRegist.ForeColor = System.Drawing.Color.White;
            this.btnRegist.Image = ((System.Drawing.Image)(resources.GetObject("btnRegist.Image")));
            this.btnRegist.ImgDisable = null;
            this.btnRegist.ImgHover = null;
            this.btnRegist.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnRegist.ImgSelect")));
            this.btnRegist.ImgStatusEvent = null;
            this.btnRegist.ImgStatusNormal = null;
            this.btnRegist.ImgStatusOffsetX = 2;
            this.btnRegist.ImgStatusOffsetY = 0;
            this.btnRegist.ImgStretch = true;
            this.btnRegist.IsImgStatusNormal = true;
            this.btnRegist.Location = new System.Drawing.Point(275, 353);
            this.btnRegist.Margin = new System.Windows.Forms.Padding(0);
            this.btnRegist.Name = "btnRegist";
            this.btnRegist.Size = new System.Drawing.Size(100, 36);
            this.btnRegist.TabIndex = 52;
            this.btnRegist.Text = "등록";
            this.btnRegist.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnRegist.UseChecked = false;
            this.btnRegist.Click += new System.EventHandler(this.btnRegist_Click);
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
            this.btnClose.Location = new System.Drawing.Point(594, 353);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 36);
            this.btnClose.TabIndex = 53;
            this.btnClose.Text = "닫기";
            this.btnClose.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnClose.UseChecked = false;
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
            this.btnSave.Location = new System.Drawing.Point(168, 353);
            this.btnSave.Margin = new System.Windows.Forms.Padding(0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 36);
            this.btnSave.TabIndex = 52;
            this.btnSave.Text = "저장";
            this.btnSave.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnSave.UseChecked = false;
            this.btnSave.Visible = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ChkValue = false;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
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
            this.btnCancel.Location = new System.Drawing.Point(61, 353);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 36);
            this.btnCancel.TabIndex = 52;
            this.btnCancel.Text = "취소";
            this.btnCancel.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnCancel.UseChecked = false;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // UserAccountForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 398);
            this.ControlBox = false;
            this.Controls.Add(this.pnlBodyBack);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnRegist);
            this.Controls.Add(this.btnModify);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UserAccountForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "사용자 관리";
            this.pnlTop.ResumeLayout(false);
            this.pnlBodyBack.ResumeLayout(false);
            this.pnlDetailBack.ResumeLayout(false);
            this.pnlDetailBack.PerformLayout();
            this.pnlClearWaitingList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Adeng.Framework.Ctrl.AdengImgButtonEx btnDelete;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblDescription;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnModify;
        private System.Windows.Forms.Panel pnlBodyBack;
        private System.Windows.Forms.Panel pnlClearWaitingList;
        private Adeng.Framework.Ctrl.AdengListView lvAccountList;
        private System.Windows.Forms.Label lblClearWaitingListTitle;
        private System.Windows.Forms.Panel pnlDetailBack;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnRegist;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private Adeng.Framework.Ctrl.TextBoxEx txtboxDepartment;
        private Adeng.Framework.Ctrl.TextBoxEx txtboxName;
        private Adeng.Framework.Ctrl.TextBoxEx txtboxPassowrd;
        private Adeng.Framework.Ctrl.TextBoxEx txtboxID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Adeng.Framework.Ctrl.TextBoxEx txtboxDescription;
        private Adeng.Framework.Ctrl.TextBoxEx txtboxPhone;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnClose;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnSave;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnCheckUnique;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnCancel;
    }
}