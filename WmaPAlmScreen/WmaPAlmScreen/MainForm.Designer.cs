namespace WmaPAlmScreen
{
    partial class MainForm
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다.
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.topMainPanel = new NCASFND.NCasCtrl.NCasPanel();
            this.btnBoardTextMenu = new NCASFND.NCasCtrl.NCasButton();
            this.orderTextBoard = new NCASFND.NCasCtrl.NCasTextBoard();
            this.label1 = new System.Windows.Forms.Label();
            this.labelResponseTermCountTitle = new System.Windows.Forms.Label();
            this.labelTotalTermCountTitle = new System.Windows.Forms.Label();
            this.labelErrorTermCount = new System.Windows.Forms.Label();
            this.labelResponseTermCount = new System.Windows.Forms.Label();
            this.labelTotalTermCount = new System.Windows.Forms.Label();
            this.btnBroadTextMenu = new NCASFND.NCasCtrl.NCasButton();
            this.btnDevMonMenu = new NCASFND.NCasCtrl.NCasButton();
            this.btnOrderResultMenu = new NCASFND.NCasCtrl.NCasButton();
            this.btnOrderMenu = new NCASFND.NCasCtrl.NCasButton();
            this.labelMainTime = new System.Windows.Forms.Label();
            this.topLeftNameLabel = new System.Windows.Forms.Label();
            this.topRightLogoPictureBox = new System.Windows.Forms.PictureBox();
            this.middlePanel = new NCASFND.NCasCtrl.NCasPanel();
            this.topMainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.topRightLogoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // topMainPanel
            // 
            this.topMainPanel.BackColor = System.Drawing.Color.Black;
            this.topMainPanel.BackgroundImage = global::WmaPAlmScreen.WmaPAlmScreenRsc.bgMainUp;
            this.topMainPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.topMainPanel.Controls.Add(this.btnBoardTextMenu);
            this.topMainPanel.Controls.Add(this.orderTextBoard);
            this.topMainPanel.Controls.Add(this.label1);
            this.topMainPanel.Controls.Add(this.labelResponseTermCountTitle);
            this.topMainPanel.Controls.Add(this.labelTotalTermCountTitle);
            this.topMainPanel.Controls.Add(this.labelErrorTermCount);
            this.topMainPanel.Controls.Add(this.labelResponseTermCount);
            this.topMainPanel.Controls.Add(this.labelTotalTermCount);
            this.topMainPanel.Controls.Add(this.btnBroadTextMenu);
            this.topMainPanel.Controls.Add(this.btnDevMonMenu);
            this.topMainPanel.Controls.Add(this.btnOrderResultMenu);
            this.topMainPanel.Controls.Add(this.btnOrderMenu);
            this.topMainPanel.Controls.Add(this.labelMainTime);
            this.topMainPanel.Controls.Add(this.topLeftNameLabel);
            this.topMainPanel.Controls.Add(this.topRightLogoPictureBox);
            this.topMainPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topMainPanel.Location = new System.Drawing.Point(0, 0);
            this.topMainPanel.Name = "topMainPanel";
            this.topMainPanel.Size = new System.Drawing.Size(1920, 165);
            this.topMainPanel.TabIndex = 0;
            // 
            // btnBoardTextMenu
            // 
            this.btnBoardTextMenu.AnimationInterval = 300;
            this.btnBoardTextMenu.CheckedValue = false;
            this.btnBoardTextMenu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBoardTextMenu.Font = new System.Drawing.Font("나눔바른고딕", 23.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnBoardTextMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(203)))), ((int)(((byte)(203)))));
            this.btnBoardTextMenu.Image = global::WmaPAlmScreen.WmaPAlmScreenRsc.btn003Up;
            this.btnBoardTextMenu.ImgDisable = null;
            this.btnBoardTextMenu.ImgHover = null;
            this.btnBoardTextMenu.ImgSelect = global::WmaPAlmScreen.WmaPAlmScreenRsc.btn003Down;
            this.btnBoardTextMenu.ImgStatusEvent = null;
            this.btnBoardTextMenu.ImgStatusNormal = null;
            this.btnBoardTextMenu.ImgStatusOffsetX = 2;
            this.btnBoardTextMenu.ImgStatusOffsetY = 0;
            this.btnBoardTextMenu.IsStatusNormal = true;
            this.btnBoardTextMenu.Location = new System.Drawing.Point(752, 81);
            this.btnBoardTextMenu.Name = "btnBoardTextMenu";
            this.btnBoardTextMenu.Size = new System.Drawing.Size(179, 69);
            this.btnBoardTextMenu.TabIndex = 40;
            this.btnBoardTextMenu.Text = "전광판";
            this.btnBoardTextMenu.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnBoardTextMenu.UseAutoWrap = false;
            this.btnBoardTextMenu.UseCheck = true;
            this.btnBoardTextMenu.UseImgStretch = false;
            this.btnBoardTextMenu.Click += new System.EventHandler(this.btnBoardTextMenu_Click);
            // 
            // orderTextBoard
            // 
            this.orderTextBoard.BackColor = System.Drawing.Color.Transparent;
            this.orderTextBoard.FontName = "맑은 고딕";
            this.orderTextBoard.FontSize = 50F;
            this.orderTextBoard.Location = new System.Drawing.Point(1484, 72);
            this.orderTextBoard.Name = "orderTextBoard";
            this.orderTextBoard.Size = new System.Drawing.Size(400, 75);
            this.orderTextBoard.TabIndex = 39;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("나눔바른고딕 옛한글", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.label1.Location = new System.Drawing.Point(1312, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 23);
            this.label1.TabIndex = 38;
            this.label1.Text = "응답이상 단말";
            // 
            // labelResponseTermCountTitle
            // 
            this.labelResponseTermCountTitle.AutoSize = true;
            this.labelResponseTermCountTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelResponseTermCountTitle.Font = new System.Drawing.Font("나눔바른고딕 옛한글", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelResponseTermCountTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.labelResponseTermCountTitle.Location = new System.Drawing.Point(1159, 80);
            this.labelResponseTermCountTitle.Name = "labelResponseTermCountTitle";
            this.labelResponseTermCountTitle.Size = new System.Drawing.Size(86, 23);
            this.labelResponseTermCountTitle.TabIndex = 37;
            this.labelResponseTermCountTitle.Text = "응답 단말";
            // 
            // labelTotalTermCountTitle
            // 
            this.labelTotalTermCountTitle.AutoSize = true;
            this.labelTotalTermCountTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelTotalTermCountTitle.Font = new System.Drawing.Font("나눔바른고딕 옛한글", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTotalTermCountTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.labelTotalTermCountTitle.Location = new System.Drawing.Point(987, 80);
            this.labelTotalTermCountTitle.Name = "labelTotalTermCountTitle";
            this.labelTotalTermCountTitle.Size = new System.Drawing.Size(86, 23);
            this.labelTotalTermCountTitle.TabIndex = 36;
            this.labelTotalTermCountTitle.Text = "전체 단말";
            // 
            // labelErrorTermCount
            // 
            this.labelErrorTermCount.BackColor = System.Drawing.Color.Transparent;
            this.labelErrorTermCount.Font = new System.Drawing.Font("나눔바른고딕", 33F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelErrorTermCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(82)))), ((int)(((byte)(53)))));
            this.labelErrorTermCount.Location = new System.Drawing.Point(1293, 107);
            this.labelErrorTermCount.Name = "labelErrorTermCount";
            this.labelErrorTermCount.Size = new System.Drawing.Size(160, 50);
            this.labelErrorTermCount.TabIndex = 35;
            this.labelErrorTermCount.Text = "0";
            this.labelErrorTermCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelResponseTermCount
            // 
            this.labelResponseTermCount.BackColor = System.Drawing.Color.Transparent;
            this.labelResponseTermCount.Font = new System.Drawing.Font("나눔바른고딕", 33F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelResponseTermCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(147)))), ((int)(((byte)(6)))));
            this.labelResponseTermCount.Location = new System.Drawing.Point(1124, 107);
            this.labelResponseTermCount.Name = "labelResponseTermCount";
            this.labelResponseTermCount.Size = new System.Drawing.Size(160, 50);
            this.labelResponseTermCount.TabIndex = 34;
            this.labelResponseTermCount.Text = "0";
            this.labelResponseTermCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelTotalTermCount
            // 
            this.labelTotalTermCount.BackColor = System.Drawing.Color.Transparent;
            this.labelTotalTermCount.Font = new System.Drawing.Font("나눔바른고딕", 33F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTotalTermCount.ForeColor = System.Drawing.Color.White;
            this.labelTotalTermCount.Location = new System.Drawing.Point(952, 107);
            this.labelTotalTermCount.Name = "labelTotalTermCount";
            this.labelTotalTermCount.Size = new System.Drawing.Size(160, 50);
            this.labelTotalTermCount.TabIndex = 33;
            this.labelTotalTermCount.Text = "0";
            this.labelTotalTermCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnBroadTextMenu
            // 
            this.btnBroadTextMenu.AnimationInterval = 300;
            this.btnBroadTextMenu.CheckedValue = false;
            this.btnBroadTextMenu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBroadTextMenu.Font = new System.Drawing.Font("나눔바른고딕", 23.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnBroadTextMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(203)))), ((int)(((byte)(203)))));
            this.btnBroadTextMenu.Image = global::WmaPAlmScreen.WmaPAlmScreenRsc.btn002Up;
            this.btnBroadTextMenu.ImgDisable = null;
            this.btnBroadTextMenu.ImgHover = null;
            this.btnBroadTextMenu.ImgSelect = global::WmaPAlmScreen.WmaPAlmScreenRsc.btn002Down;
            this.btnBroadTextMenu.ImgStatusEvent = null;
            this.btnBroadTextMenu.ImgStatusNormal = null;
            this.btnBroadTextMenu.ImgStatusOffsetX = 2;
            this.btnBroadTextMenu.ImgStatusOffsetY = 0;
            this.btnBroadTextMenu.IsStatusNormal = true;
            this.btnBroadTextMenu.Location = new System.Drawing.Point(574, 81);
            this.btnBroadTextMenu.Name = "btnBroadTextMenu";
            this.btnBroadTextMenu.Size = new System.Drawing.Size(179, 69);
            this.btnBroadTextMenu.TabIndex = 32;
            this.btnBroadTextMenu.Text = "TTS편집";
            this.btnBroadTextMenu.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnBroadTextMenu.UseAutoWrap = false;
            this.btnBroadTextMenu.UseCheck = true;
            this.btnBroadTextMenu.UseImgStretch = false;
            this.btnBroadTextMenu.Click += new System.EventHandler(this.btnBroadTextMenu_Click);
            // 
            // btnDevMonMenu
            // 
            this.btnDevMonMenu.AnimationInterval = 300;
            this.btnDevMonMenu.CheckedValue = false;
            this.btnDevMonMenu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDevMonMenu.Font = new System.Drawing.Font("나눔바른고딕", 23.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDevMonMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(203)))), ((int)(((byte)(203)))));
            this.btnDevMonMenu.Image = global::WmaPAlmScreen.WmaPAlmScreenRsc.btn002Up;
            this.btnDevMonMenu.ImgDisable = null;
            this.btnDevMonMenu.ImgHover = null;
            this.btnDevMonMenu.ImgSelect = global::WmaPAlmScreen.WmaPAlmScreenRsc.btn002Down;
            this.btnDevMonMenu.ImgStatusEvent = null;
            this.btnDevMonMenu.ImgStatusNormal = null;
            this.btnDevMonMenu.ImgStatusOffsetX = 2;
            this.btnDevMonMenu.ImgStatusOffsetY = 0;
            this.btnDevMonMenu.IsStatusNormal = true;
            this.btnDevMonMenu.Location = new System.Drawing.Point(396, 81);
            this.btnDevMonMenu.Name = "btnDevMonMenu";
            this.btnDevMonMenu.Size = new System.Drawing.Size(178, 69);
            this.btnDevMonMenu.TabIndex = 31;
            this.btnDevMonMenu.Text = "장비감시";
            this.btnDevMonMenu.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnDevMonMenu.UseAutoWrap = false;
            this.btnDevMonMenu.UseCheck = true;
            this.btnDevMonMenu.UseImgStretch = false;
            this.btnDevMonMenu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnOrderMenu_MouseDown);
            // 
            // btnOrderResultMenu
            // 
            this.btnOrderResultMenu.AnimationInterval = 300;
            this.btnOrderResultMenu.CheckedValue = false;
            this.btnOrderResultMenu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOrderResultMenu.Font = new System.Drawing.Font("나눔바른고딕", 23.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOrderResultMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(203)))), ((int)(((byte)(203)))));
            this.btnOrderResultMenu.Image = global::WmaPAlmScreen.WmaPAlmScreenRsc.btn002Up;
            this.btnOrderResultMenu.ImgDisable = null;
            this.btnOrderResultMenu.ImgHover = null;
            this.btnOrderResultMenu.ImgSelect = global::WmaPAlmScreen.WmaPAlmScreenRsc.btn002Down;
            this.btnOrderResultMenu.ImgStatusEvent = null;
            this.btnOrderResultMenu.ImgStatusNormal = null;
            this.btnOrderResultMenu.ImgStatusOffsetX = 2;
            this.btnOrderResultMenu.ImgStatusOffsetY = 0;
            this.btnOrderResultMenu.IsStatusNormal = true;
            this.btnOrderResultMenu.Location = new System.Drawing.Point(218, 81);
            this.btnOrderResultMenu.Name = "btnOrderResultMenu";
            this.btnOrderResultMenu.Size = new System.Drawing.Size(178, 69);
            this.btnOrderResultMenu.TabIndex = 30;
            this.btnOrderResultMenu.Text = "발령결과";
            this.btnOrderResultMenu.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnOrderResultMenu.UseAutoWrap = false;
            this.btnOrderResultMenu.UseCheck = true;
            this.btnOrderResultMenu.UseImgStretch = false;
            this.btnOrderResultMenu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnOrderMenu_MouseDown);
            // 
            // btnOrderMenu
            // 
            this.btnOrderMenu.AnimationInterval = 300;
            this.btnOrderMenu.CheckedValue = false;
            this.btnOrderMenu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOrderMenu.Font = new System.Drawing.Font("나눔바른고딕", 23.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOrderMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(203)))), ((int)(((byte)(203)))));
            this.btnOrderMenu.Image = global::WmaPAlmScreen.WmaPAlmScreenRsc.btn001Up;
            this.btnOrderMenu.ImgDisable = null;
            this.btnOrderMenu.ImgHover = null;
            this.btnOrderMenu.ImgSelect = global::WmaPAlmScreen.WmaPAlmScreenRsc.btn001Down;
            this.btnOrderMenu.ImgStatusEvent = null;
            this.btnOrderMenu.ImgStatusNormal = null;
            this.btnOrderMenu.ImgStatusOffsetX = 2;
            this.btnOrderMenu.ImgStatusOffsetY = 0;
            this.btnOrderMenu.IsStatusNormal = true;
            this.btnOrderMenu.Location = new System.Drawing.Point(39, 81);
            this.btnOrderMenu.Name = "btnOrderMenu";
            this.btnOrderMenu.Size = new System.Drawing.Size(179, 69);
            this.btnOrderMenu.TabIndex = 29;
            this.btnOrderMenu.Text = "경보발령";
            this.btnOrderMenu.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnOrderMenu.UseAutoWrap = false;
            this.btnOrderMenu.UseCheck = true;
            this.btnOrderMenu.UseImgStretch = false;
            this.btnOrderMenu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnOrderMenu_MouseDown);
            // 
            // labelMainTime
            // 
            this.labelMainTime.AutoSize = true;
            this.labelMainTime.BackColor = System.Drawing.Color.Transparent;
            this.labelMainTime.Font = new System.Drawing.Font("나눔바른고딕 옛한글", 27F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelMainTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.labelMainTime.Location = new System.Drawing.Point(1450, 10);
            this.labelMainTime.Name = "labelMainTime";
            this.labelMainTime.Size = new System.Drawing.Size(0, 42);
            this.labelMainTime.TabIndex = 28;
            // 
            // topLeftNameLabel
            // 
            this.topLeftNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.topLeftNameLabel.Font = new System.Drawing.Font("맑은 고딕", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.topLeftNameLabel.ForeColor = System.Drawing.SystemColors.Window;
            this.topLeftNameLabel.Image = global::WmaPAlmScreen.WmaPAlmScreenRsc.btnTop;
            this.topLeftNameLabel.Location = new System.Drawing.Point(776, 6);
            this.topLeftNameLabel.Name = "topLeftNameLabel";
            this.topLeftNameLabel.Size = new System.Drawing.Size(368, 35);
            this.topLeftNameLabel.TabIndex = 2;
            this.topLeftNameLabel.Tag = "";
            this.topLeftNameLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.topLeftNameLabel_MouseDown);
            // 
            // topRightLogoPictureBox
            // 
            this.topRightLogoPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.topRightLogoPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.topRightLogoPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.topRightLogoPictureBox.Location = new System.Drawing.Point(154, 4);
            this.topRightLogoPictureBox.Name = "topRightLogoPictureBox";
            this.topRightLogoPictureBox.Size = new System.Drawing.Size(247, 56);
            this.topRightLogoPictureBox.TabIndex = 1;
            this.topRightLogoPictureBox.TabStop = false;
            this.topRightLogoPictureBox.DoubleClick += new System.EventHandler(this.topRightLogoPictureBox_DoubleClick);
            // 
            // middlePanel
            // 
            this.middlePanel.BackColor = System.Drawing.Color.Black;
            this.middlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.middlePanel.Location = new System.Drawing.Point(0, 165);
            this.middlePanel.Name = "middlePanel";
            this.middlePanel.Size = new System.Drawing.Size(1920, 915);
            this.middlePanel.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 1080);
            this.ControlBox = false;
            this.Controls.Add(this.middlePanel);
            this.Controls.Add(this.topMainPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1280, 1024);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.topMainPanel.ResumeLayout(false);
            this.topMainPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.topRightLogoPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private NCASFND.NCasCtrl.NCasPanel topMainPanel;
        private NCASFND.NCasCtrl.NCasPanel middlePanel;
        private NCASFND.NCasCtrl.NCasButton btnBroadTextMenu;
        private NCASFND.NCasCtrl.NCasButton btnDevMonMenu;
        private NCASFND.NCasCtrl.NCasButton btnOrderResultMenu;
        private NCASFND.NCasCtrl.NCasButton btnOrderMenu;
        private System.Windows.Forms.Label labelMainTime;
        private System.Windows.Forms.Label topLeftNameLabel;
        private System.Windows.Forms.PictureBox topRightLogoPictureBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelResponseTermCountTitle;
        private System.Windows.Forms.Label labelTotalTermCountTitle;
        private System.Windows.Forms.Label labelErrorTermCount;
        private System.Windows.Forms.Label labelResponseTermCount;
        private System.Windows.Forms.Label labelTotalTermCount;
        private NCASFND.NCasCtrl.NCasTextBoard orderTextBoard;
        private NCASFND.NCasCtrl.NCasButton btnBoardTextMenu;
    }
}

