namespace WmaPAlmScreen
{
    partial class OrderGroupEditForm
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
            this.topPanel = new NCASFND.NCasCtrl.NCasPanel();
            this.mainPictureBox = new System.Windows.Forms.PictureBox();
            this.titlelabel = new System.Windows.Forms.Label();
            this.titlePanel = new NCASFND.NCasCtrl.NCasPanel();
            this.groupListView = new NCASFND.NCasCtrl.NCasListView();
            this.listLabel = new System.Windows.Forms.Label();
            this.namePanel = new NCASFND.NCasCtrl.NCasPanel();
            this.groupNameTextBox = new System.Windows.Forms.TextBox();
            this.groupLabel = new System.Windows.Forms.Label();
            this.saveBtn = new NCASFND.NCasCtrl.NCasButton();
            this.cancelBtn = new NCASFND.NCasCtrl.NCasButton();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
            this.titlePanel.SuspendLayout();
            this.namePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.BackgroundImage = global::WmaPAlmScreen.WmaPAlmScreenRsc.popBgTop;
            this.topPanel.Controls.Add(this.mainPictureBox);
            this.topPanel.Controls.Add(this.titlelabel);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(406, 56);
            this.topPanel.TabIndex = 0;
            // 
            // mainPictureBox
            // 
            this.mainPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.mainPictureBox.BackgroundImage = global::WmaPAlmScreen.WmaPAlmScreenRsc.popGroupEditIcon;
            this.mainPictureBox.Location = new System.Drawing.Point(363, 12);
            this.mainPictureBox.Name = "mainPictureBox";
            this.mainPictureBox.Size = new System.Drawing.Size(32, 32);
            this.mainPictureBox.TabIndex = 5;
            this.mainPictureBox.TabStop = false;
            // 
            // titlelabel
            // 
            this.titlelabel.AutoSize = true;
            this.titlelabel.BackColor = System.Drawing.Color.Transparent;
            this.titlelabel.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.titlelabel.ForeColor = System.Drawing.Color.White;
            this.titlelabel.Location = new System.Drawing.Point(9, 19);
            this.titlelabel.Name = "titlelabel";
            this.titlelabel.Size = new System.Drawing.Size(130, 19);
            this.titlelabel.TabIndex = 4;
            this.titlelabel.Text = "그룹을 편집합니다.";
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.titlePanel.Controls.Add(this.groupListView);
            this.titlePanel.Controls.Add(this.listLabel);
            this.titlePanel.Controls.Add(this.namePanel);
            this.titlePanel.Location = new System.Drawing.Point(15, 71);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(377, 517);
            this.titlePanel.TabIndex = 1;
            // 
            // groupListView
            // 
            this.groupListView.AntiAlias = false;
            this.groupListView.AutoFit = false;
            this.groupListView.BackColor = System.Drawing.Color.White;
            this.groupListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.groupListView.ColumnHeight = 24;
            this.groupListView.ColumnOffset = new System.Windows.Forms.Padding(2, 2, 0, 1);
            this.groupListView.ColumnStyle = NCASFND.NCasListViewColumnStyle.DefaultStyle;
            this.groupListView.FocusedItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(185)))), ((int)(((byte)(230)))));
            this.groupListView.FrozenColumnIndex = -1;
            this.groupListView.FullRowSelect = true;
            this.groupListView.GridDashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.groupListView.GridLineStyle = NCASFND.NCasListViewGridLine.GridNone;
            this.groupListView.HideColumnCheckBox = false;
            this.groupListView.HideSelection = false;
            this.groupListView.HoverSelection = false;
            this.groupListView.IconOffset = new System.Drawing.Point(1, 0);
            this.groupListView.InteraceColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.groupListView.InteraceColor2 = System.Drawing.Color.White;
            this.groupListView.ItemHeight = 18;
            this.groupListView.ItemOffset = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.groupListView.Location = new System.Drawing.Point(14, 87);
            this.groupListView.MultiSelect = false;
            this.groupListView.Name = "groupListView";
            this.groupListView.ScrollType = NCASFND.NCasListViewScrollType.ScrollBoth;
            this.groupListView.SelectioinItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(232)))), ((int)(((byte)(250)))));
            this.groupListView.Size = new System.Drawing.Size(349, 419);
            this.groupListView.TabIndex = 2;
            this.groupListView.UseFocusBar = true;
            this.groupListView.UseInteraceColor = true;
            this.groupListView.UseItemFitBox = false;
            this.groupListView.UseSelFocusedBar = false;
            // 
            // listLabel
            // 
            this.listLabel.AutoSize = true;
            this.listLabel.BackColor = System.Drawing.Color.Transparent;
            this.listLabel.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.listLabel.ForeColor = System.Drawing.Color.White;
            this.listLabel.Location = new System.Drawing.Point(11, 65);
            this.listLabel.Name = "listLabel";
            this.listLabel.Size = new System.Drawing.Size(105, 17);
            this.listLabel.TabIndex = 1;
            this.listLabel.Text = "그룹 항목 리스트";
            // 
            // namePanel
            // 
            this.namePanel.BackgroundImage = global::WmaPAlmScreen.WmaPAlmScreenRsc.box1Line;
            this.namePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.namePanel.Controls.Add(this.groupNameTextBox);
            this.namePanel.Controls.Add(this.groupLabel);
            this.namePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.namePanel.Location = new System.Drawing.Point(0, 0);
            this.namePanel.Name = "namePanel";
            this.namePanel.Size = new System.Drawing.Size(377, 48);
            this.namePanel.TabIndex = 0;
            // 
            // groupNameTextBox
            // 
            this.groupNameTextBox.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupNameTextBox.Location = new System.Drawing.Point(146, 12);
            this.groupNameTextBox.MaxLength = 14;
            this.groupNameTextBox.Name = "groupNameTextBox";
            this.groupNameTextBox.Size = new System.Drawing.Size(190, 25);
            this.groupNameTextBox.TabIndex = 1;
            this.groupNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupLabel
            // 
            this.groupLabel.AutoSize = true;
            this.groupLabel.BackColor = System.Drawing.Color.Transparent;
            this.groupLabel.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.groupLabel.ForeColor = System.Drawing.Color.White;
            this.groupLabel.Location = new System.Drawing.Point(56, 16);
            this.groupLabel.Name = "groupLabel";
            this.groupLabel.Size = new System.Drawing.Size(47, 17);
            this.groupLabel.TabIndex = 0;
            this.groupLabel.Text = "그룹명";
            // 
            // saveBtn
            // 
            this.saveBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveBtn.AnimationInterval = 300;
            this.saveBtn.CheckedValue = false;
            this.saveBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.saveBtn.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.saveBtn.ForeColor = System.Drawing.Color.White;
            this.saveBtn.Image = global::WmaPAlmScreen.WmaPAlmScreenRsc.btnSmallNormal;
            this.saveBtn.ImgDisable = null;
            this.saveBtn.ImgHover = null;
            this.saveBtn.ImgSelect = global::WmaPAlmScreen.WmaPAlmScreenRsc.btnSmallPress;
            this.saveBtn.ImgStatusEvent = null;
            this.saveBtn.ImgStatusNormal = null;
            this.saveBtn.ImgStatusOffsetX = 2;
            this.saveBtn.ImgStatusOffsetY = 0;
            this.saveBtn.IsStatusNormal = true;
            this.saveBtn.Location = new System.Drawing.Point(214, 594);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(89, 39);
            this.saveBtn.TabIndex = 2;
            this.saveBtn.Text = "저장";
            this.saveBtn.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.saveBtn.UseAutoWrap = true;
            this.saveBtn.UseCheck = false;
            this.saveBtn.UseImgStretch = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // cancelBtn
            // 
            this.cancelBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelBtn.AnimationInterval = 300;
            this.cancelBtn.CheckedValue = false;
            this.cancelBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancelBtn.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cancelBtn.ForeColor = System.Drawing.Color.White;
            this.cancelBtn.Image = global::WmaPAlmScreen.WmaPAlmScreenRsc.btnSmallNormal;
            this.cancelBtn.ImgDisable = null;
            this.cancelBtn.ImgHover = null;
            this.cancelBtn.ImgSelect = global::WmaPAlmScreen.WmaPAlmScreenRsc.btnSmallPress;
            this.cancelBtn.ImgStatusEvent = null;
            this.cancelBtn.ImgStatusNormal = null;
            this.cancelBtn.ImgStatusOffsetX = 2;
            this.cancelBtn.ImgStatusOffsetY = 0;
            this.cancelBtn.IsStatusNormal = true;
            this.cancelBtn.Location = new System.Drawing.Point(308, 594);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(89, 39);
            this.cancelBtn.TabIndex = 3;
            this.cancelBtn.Text = "닫기";
            this.cancelBtn.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.cancelBtn.UseAutoWrap = true;
            this.cancelBtn.UseCheck = false;
            this.cancelBtn.UseImgStretch = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // OrderGroupEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ClientSize = new System.Drawing.Size(406, 640);
            this.ControlBox = false;
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.titlePanel);
            this.Controls.Add(this.topPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(500, 500);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderGroupEditForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "그룹 편집";
            this.TopMost = true;
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
            this.titlePanel.ResumeLayout(false);
            this.titlePanel.PerformLayout();
            this.namePanel.ResumeLayout(false);
            this.namePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private NCASFND.NCasCtrl.NCasPanel topPanel;
        private System.Windows.Forms.PictureBox mainPictureBox;
        private System.Windows.Forms.Label titlelabel;
        private NCASFND.NCasCtrl.NCasPanel titlePanel;
        private System.Windows.Forms.Label listLabel;
        private NCASFND.NCasCtrl.NCasPanel namePanel;
        private System.Windows.Forms.TextBox groupNameTextBox;
        private System.Windows.Forms.Label groupLabel;
        private NCASFND.NCasCtrl.NCasButton saveBtn;
        private NCASFND.NCasCtrl.NCasButton cancelBtn;
        private NCASFND.NCasCtrl.NCasListView groupListView;
    }
}