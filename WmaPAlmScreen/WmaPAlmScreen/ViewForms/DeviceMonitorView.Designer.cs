namespace WmaPAlmScreen
{
    partial class DeviceMonitorView
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeviceMonitorView));
            this.mainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.serverStatusListView = new NCASFND.NCasCtrl.NCasListView();
            this.statusImageList = new System.Windows.Forms.ImageList(this.components);
            this.topPanel = new NCASFND.NCasCtrl.NCasPanel();
            this.topLabel = new System.Windows.Forms.Label();
            this.termStatusListView = new NCASFND.NCasCtrl.NCasListView();
            this.bottomPanel = new NCASFND.NCasCtrl.NCasPanel();
            this.bottomlabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).BeginInit();
            this.mainSplitContainer.Panel1.SuspendLayout();
            this.mainSplitContainer.Panel2.SuspendLayout();
            this.mainSplitContainer.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainSplitContainer
            // 
            this.mainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            this.mainSplitContainer.Panel1.Controls.Add(this.serverStatusListView);
            this.mainSplitContainer.Panel1.Controls.Add(this.topPanel);
            this.mainSplitContainer.Panel1MinSize = 120;
            // 
            // mainSplitContainer.Panel2
            // 
            this.mainSplitContainer.Panel2.Controls.Add(this.termStatusListView);
            this.mainSplitContainer.Panel2.Controls.Add(this.bottomPanel);
            this.mainSplitContainer.Panel2MinSize = 120;
            this.mainSplitContainer.Size = new System.Drawing.Size(1920, 914);
            this.mainSplitContainer.SplitterDistance = 954;
            this.mainSplitContainer.SplitterWidth = 3;
            this.mainSplitContainer.TabIndex = 0;
            // 
            // serverStatusListView
            // 
            this.serverStatusListView.AntiAlias = false;
            this.serverStatusListView.AutoFit = false;
            this.serverStatusListView.BackColor = System.Drawing.Color.White;
            this.serverStatusListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serverStatusListView.ColumnHeight = 24;
            this.serverStatusListView.ColumnOffset = new System.Windows.Forms.Padding(2, 2, 0, 1);
            this.serverStatusListView.ColumnStyle = NCASFND.NCasListViewColumnStyle.DefaultStyle;
            this.serverStatusListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverStatusListView.FocusedItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(185)))), ((int)(((byte)(230)))));
            this.serverStatusListView.FrozenColumnIndex = -1;
            this.serverStatusListView.FullRowSelect = true;
            this.serverStatusListView.GridDashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.serverStatusListView.GridLineStyle = NCASFND.NCasListViewGridLine.GridNone;
            this.serverStatusListView.HideColumnCheckBox = false;
            this.serverStatusListView.HideSelection = false;
            this.serverStatusListView.HoverSelection = false;
            this.serverStatusListView.IconOffset = new System.Drawing.Point(1, 0);
            this.serverStatusListView.InteraceColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.serverStatusListView.InteraceColor2 = System.Drawing.Color.White;
            this.serverStatusListView.ItemHeight = 18;
            this.serverStatusListView.ItemOffset = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.serverStatusListView.Location = new System.Drawing.Point(0, 33);
            this.serverStatusListView.MultiSelect = false;
            this.serverStatusListView.Name = "serverStatusListView";
            this.serverStatusListView.ScrollType = NCASFND.NCasListViewScrollType.ScrollBoth;
            this.serverStatusListView.SelectioinItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(232)))), ((int)(((byte)(250)))));
            this.serverStatusListView.Size = new System.Drawing.Size(954, 881);
            this.serverStatusListView.StateImageList = this.statusImageList;
            this.serverStatusListView.TabIndex = 6;
            this.serverStatusListView.UseFocusBar = true;
            this.serverStatusListView.UseInteraceColor = true;
            this.serverStatusListView.UseItemFitBox = false;
            this.serverStatusListView.UseSelFocusedBar = false;
            // 
            // statusImageList
            // 
            this.statusImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("statusImageList.ImageStream")));
            this.statusImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.statusImageList.Images.SetKeyName(0, "listIconError.png");
            this.statusImageList.Images.SetKeyName(1, "listIconOk.png");
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(146)))), ((int)(((byte)(213)))));
            this.topPanel.BackgroundImage = global::WmaPAlmScreen.WmaPAlmScreenRsc.bgHeader;
            this.topPanel.Controls.Add(this.topLabel);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(954, 33);
            this.topPanel.TabIndex = 5;
            // 
            // topLabel
            // 
            this.topLabel.AutoSize = true;
            this.topLabel.BackColor = System.Drawing.Color.Transparent;
            this.topLabel.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.topLabel.ForeColor = System.Drawing.Color.White;
            this.topLabel.Location = new System.Drawing.Point(7, 7);
            this.topLabel.Name = "topLabel";
            this.topLabel.Size = new System.Drawing.Size(159, 20);
            this.topLabel.TabIndex = 2;
            this.topLabel.Text = "통제소 장비 감시 현황";
            // 
            // termStatusListView
            // 
            this.termStatusListView.AntiAlias = false;
            this.termStatusListView.AutoFit = false;
            this.termStatusListView.BackColor = System.Drawing.Color.White;
            this.termStatusListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.termStatusListView.ColumnHeight = 24;
            this.termStatusListView.ColumnOffset = new System.Windows.Forms.Padding(2, 2, 0, 1);
            this.termStatusListView.ColumnStyle = NCASFND.NCasListViewColumnStyle.DefaultStyle;
            this.termStatusListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.termStatusListView.FocusedItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(185)))), ((int)(((byte)(230)))));
            this.termStatusListView.FrozenColumnIndex = -1;
            this.termStatusListView.FullRowSelect = true;
            this.termStatusListView.GridDashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.termStatusListView.GridLineStyle = NCASFND.NCasListViewGridLine.GridNone;
            this.termStatusListView.HideColumnCheckBox = false;
            this.termStatusListView.HideSelection = false;
            this.termStatusListView.HoverSelection = false;
            this.termStatusListView.IconOffset = new System.Drawing.Point(1, 0);
            this.termStatusListView.InteraceColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.termStatusListView.InteraceColor2 = System.Drawing.Color.White;
            this.termStatusListView.ItemHeight = 18;
            this.termStatusListView.ItemOffset = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.termStatusListView.Location = new System.Drawing.Point(0, 33);
            this.termStatusListView.MultiSelect = false;
            this.termStatusListView.Name = "termStatusListView";
            this.termStatusListView.ScrollType = NCASFND.NCasListViewScrollType.ScrollBoth;
            this.termStatusListView.SelectioinItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(232)))), ((int)(((byte)(250)))));
            this.termStatusListView.Size = new System.Drawing.Size(963, 881);
            this.termStatusListView.StateImageList = this.statusImageList;
            this.termStatusListView.TabIndex = 6;
            this.termStatusListView.UseFocusBar = true;
            this.termStatusListView.UseInteraceColor = true;
            this.termStatusListView.UseItemFitBox = false;
            this.termStatusListView.UseSelFocusedBar = false;
            // 
            // bottomPanel
            // 
            this.bottomPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(146)))), ((int)(((byte)(213)))));
            this.bottomPanel.BackgroundImage = global::WmaPAlmScreen.WmaPAlmScreenRsc.bgHeader;
            this.bottomPanel.Controls.Add(this.bottomlabel);
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.bottomPanel.Location = new System.Drawing.Point(0, 0);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(963, 33);
            this.bottomPanel.TabIndex = 5;
            // 
            // bottomlabel
            // 
            this.bottomlabel.AutoSize = true;
            this.bottomlabel.BackColor = System.Drawing.Color.Transparent;
            this.bottomlabel.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.bottomlabel.ForeColor = System.Drawing.Color.White;
            this.bottomlabel.Location = new System.Drawing.Point(7, 7);
            this.bottomlabel.Name = "bottomlabel";
            this.bottomlabel.Size = new System.Drawing.Size(174, 20);
            this.bottomlabel.TabIndex = 2;
            this.bottomlabel.Text = "시도 경보단말 감시 현황";
            // 
            // DeviceMonitorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.mainSplitContainer);
            this.DoubleBuffered = true;
            this.Name = "DeviceMonitorView";
            this.Size = new System.Drawing.Size(1920, 914);
            this.mainSplitContainer.Panel1.ResumeLayout(false);
            this.mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplitContainer)).EndInit();
            this.mainSplitContainer.ResumeLayout(false);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private NCASFND.NCasCtrl.NCasListView serverStatusListView;
        private System.Windows.Forms.ImageList statusImageList;
        private NCASFND.NCasCtrl.NCasPanel topPanel;
        private System.Windows.Forms.Label topLabel;
        private NCASFND.NCasCtrl.NCasListView termStatusListView;
        private NCASFND.NCasCtrl.NCasPanel bottomPanel;
        private System.Windows.Forms.Label bottomlabel;
    }
}
