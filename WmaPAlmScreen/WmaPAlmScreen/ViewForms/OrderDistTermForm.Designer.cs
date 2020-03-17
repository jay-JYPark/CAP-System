namespace WmaPAlmScreen
{
    partial class OrderDistTermForm
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
            this.distNameTextBox = new System.Windows.Forms.TextBox();
            this.topPanel = new NCASFND.NCasCtrl.NCasPanel();
            this.mainPictureBox = new System.Windows.Forms.PictureBox();
            this.titlelabel = new System.Windows.Forms.Label();
            this.distLabel = new System.Windows.Forms.Label();
            this.termListView = new NCASFND.NCasCtrl.NCasListView();
            this.listLabel = new System.Windows.Forms.Label();
            this.namePanel = new NCASFND.NCasCtrl.NCasPanel();
            this.cancelBtn = new NCASFND.NCasCtrl.NCasButton();
            this.titlePanel = new NCASFND.NCasCtrl.NCasPanel();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
            this.namePanel.SuspendLayout();
            this.titlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // distNameTextBox
            // 
            this.distNameTextBox.BackColor = System.Drawing.Color.White;
            this.distNameTextBox.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.distNameTextBox.Location = new System.Drawing.Point(146, 12);
            this.distNameTextBox.MaxLength = 14;
            this.distNameTextBox.Name = "distNameTextBox";
            this.distNameTextBox.ReadOnly = true;
            this.distNameTextBox.Size = new System.Drawing.Size(190, 25);
            this.distNameTextBox.TabIndex = 1;
            this.distNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
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
            this.topPanel.TabIndex = 4;
            // 
            // mainPictureBox
            // 
            this.mainPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.mainPictureBox.BackgroundImage = global::WmaPAlmScreen.WmaPAlmScreenRsc.popTermList;
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
            this.titlelabel.Size = new System.Drawing.Size(208, 19);
            this.titlelabel.TabIndex = 4;
            this.titlelabel.Text = "시군구 소속 단말을 선택합니다.";
            // 
            // distLabel
            // 
            this.distLabel.AutoSize = true;
            this.distLabel.BackColor = System.Drawing.Color.Transparent;
            this.distLabel.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.distLabel.ForeColor = System.Drawing.Color.White;
            this.distLabel.Location = new System.Drawing.Point(44, 16);
            this.distLabel.Name = "distLabel";
            this.distLabel.Size = new System.Drawing.Size(76, 17);
            this.distLabel.TabIndex = 0;
            this.distLabel.Text = "선택 시군구";
            // 
            // termListView
            // 
            this.termListView.AntiAlias = false;
            this.termListView.AutoFit = false;
            this.termListView.BackColor = System.Drawing.Color.White;
            this.termListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.termListView.ColumnHeight = 24;
            this.termListView.ColumnOffset = new System.Windows.Forms.Padding(2, 2, 0, 1);
            this.termListView.ColumnStyle = NCASFND.NCasListViewColumnStyle.DefaultStyle;
            this.termListView.FocusedItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(185)))), ((int)(((byte)(230)))));
            this.termListView.FrozenColumnIndex = -1;
            this.termListView.FullRowSelect = true;
            this.termListView.GridDashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.termListView.GridLineStyle = NCASFND.NCasListViewGridLine.GridNone;
            this.termListView.HideColumnCheckBox = false;
            this.termListView.HideSelection = false;
            this.termListView.HoverSelection = false;
            this.termListView.IconOffset = new System.Drawing.Point(1, 0);
            this.termListView.InteraceColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.termListView.InteraceColor2 = System.Drawing.Color.White;
            this.termListView.ItemHeight = 18;
            this.termListView.ItemOffset = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.termListView.Location = new System.Drawing.Point(14, 85);
            this.termListView.MultiSelect = false;
            this.termListView.Name = "termListView";
            this.termListView.ScrollType = NCASFND.NCasListViewScrollType.ScrollBoth;
            this.termListView.SelectioinItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(232)))), ((int)(((byte)(250)))));
            this.termListView.Size = new System.Drawing.Size(349, 419);
            this.termListView.TabIndex = 2;
            this.termListView.UseFocusBar = true;
            this.termListView.UseInteraceColor = true;
            this.termListView.UseItemFitBox = false;
            this.termListView.UseSelFocusedBar = false;
            this.termListView.ItemChecked += new NCASFND.NCasItemCheckedEventHandler(this.termListView_ItemChecked);
            this.termListView.ColumnChecked += new NCASFND.NCasColumnCheckedEventHandler(this.termListView_ColumnChecked);
            // 
            // listLabel
            // 
            this.listLabel.AutoSize = true;
            this.listLabel.BackColor = System.Drawing.Color.Transparent;
            this.listLabel.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.listLabel.ForeColor = System.Drawing.Color.White;
            this.listLabel.Location = new System.Drawing.Point(11, 63);
            this.listLabel.Name = "listLabel";
            this.listLabel.Size = new System.Drawing.Size(76, 17);
            this.listLabel.TabIndex = 1;
            this.listLabel.Text = "단말 리스트";
            // 
            // namePanel
            // 
            this.namePanel.BackgroundImage = global::WmaPAlmScreen.WmaPAlmScreenRsc.box1Line;
            this.namePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.namePanel.Controls.Add(this.distNameTextBox);
            this.namePanel.Controls.Add(this.distLabel);
            this.namePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.namePanel.Location = new System.Drawing.Point(0, 0);
            this.namePanel.Name = "namePanel";
            this.namePanel.Size = new System.Drawing.Size(377, 48);
            this.namePanel.TabIndex = 0;
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
            this.cancelBtn.TabIndex = 7;
            this.cancelBtn.Text = "취소";
            this.cancelBtn.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.cancelBtn.UseAutoWrap = true;
            this.cancelBtn.UseCheck = false;
            this.cancelBtn.UseImgStretch = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // titlePanel
            // 
            this.titlePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.titlePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.titlePanel.Controls.Add(this.termListView);
            this.titlePanel.Controls.Add(this.listLabel);
            this.titlePanel.Controls.Add(this.namePanel);
            this.titlePanel.Location = new System.Drawing.Point(15, 69);
            this.titlePanel.Name = "titlePanel";
            this.titlePanel.Size = new System.Drawing.Size(377, 517);
            this.titlePanel.TabIndex = 5;
            // 
            // OrderDistTermForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ClientSize = new System.Drawing.Size(406, 639);
            this.ControlBox = false;
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.titlePanel);
            this.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Location = new System.Drawing.Point(500, 500);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderDistTermForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "단말 선택";
            this.TopMost = true;
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
            this.namePanel.ResumeLayout(false);
            this.namePanel.PerformLayout();
            this.titlePanel.ResumeLayout(false);
            this.titlePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox distNameTextBox;
        private NCASFND.NCasCtrl.NCasPanel topPanel;
        private System.Windows.Forms.PictureBox mainPictureBox;
        private System.Windows.Forms.Label titlelabel;
        private System.Windows.Forms.Label distLabel;
        private NCASFND.NCasCtrl.NCasListView termListView;
        private System.Windows.Forms.Label listLabel;
        private NCASFND.NCasCtrl.NCasPanel namePanel;
        private NCASFND.NCasCtrl.NCasButton cancelBtn;
        private NCASFND.NCasCtrl.NCasPanel titlePanel;
    }
}