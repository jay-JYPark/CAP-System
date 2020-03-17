namespace WmaPAlmScreen
{
    partial class OrderResultView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderResultView));
            this.orderResultImageList = new System.Windows.Forms.ImageList(this.components);
            this.topLabel = new System.Windows.Forms.Label();
            this.orderResultListView = new NCASFND.NCasCtrl.NCasListView();
            this.topPanel = new NCASFND.NCasCtrl.NCasPanel();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // orderResultImageList
            // 
            this.orderResultImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("orderResultImageList.ImageStream")));
            this.orderResultImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.orderResultImageList.Images.SetKeyName(0, "alertIcon1.png");
            this.orderResultImageList.Images.SetKeyName(1, "alertIcon2.png");
            this.orderResultImageList.Images.SetKeyName(2, "alertIcon3.png");
            this.orderResultImageList.Images.SetKeyName(3, "alertIcon4.png");
            this.orderResultImageList.Images.SetKeyName(4, "alertIcon5.png");
            this.orderResultImageList.Images.SetKeyName(5, "alertIcon6.png");
            this.orderResultImageList.Images.SetKeyName(6, "alertIcon7.png");
            // 
            // topLabel
            // 
            this.topLabel.AutoSize = true;
            this.topLabel.BackColor = System.Drawing.Color.Transparent;
            this.topLabel.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.topLabel.ForeColor = System.Drawing.Color.White;
            this.topLabel.Location = new System.Drawing.Point(7, 7);
            this.topLabel.Name = "topLabel";
            this.topLabel.Size = new System.Drawing.Size(134, 20);
            this.topLabel.TabIndex = 2;
            this.topLabel.Text = "경보단말 발령결과";
            // 
            // orderResultListView
            // 
            this.orderResultListView.AntiAlias = false;
            this.orderResultListView.AutoFit = false;
            this.orderResultListView.BackColor = System.Drawing.Color.White;
            this.orderResultListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.orderResultListView.ColumnHeight = 24;
            this.orderResultListView.ColumnOffset = new System.Windows.Forms.Padding(2, 2, 0, 1);
            this.orderResultListView.ColumnStyle = NCASFND.NCasListViewColumnStyle.DefaultStyle;
            this.orderResultListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.orderResultListView.FocusedItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(185)))), ((int)(((byte)(230)))));
            this.orderResultListView.FrozenColumnIndex = -1;
            this.orderResultListView.FullRowSelect = true;
            this.orderResultListView.GridDashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.orderResultListView.GridLineStyle = NCASFND.NCasListViewGridLine.GridNone;
            this.orderResultListView.HideColumnCheckBox = false;
            this.orderResultListView.HideSelection = false;
            this.orderResultListView.HoverSelection = false;
            this.orderResultListView.IconOffset = new System.Drawing.Point(1, 0);
            this.orderResultListView.InteraceColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.orderResultListView.InteraceColor2 = System.Drawing.Color.White;
            this.orderResultListView.ItemHeight = 18;
            this.orderResultListView.ItemOffset = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.orderResultListView.Location = new System.Drawing.Point(0, 33);
            this.orderResultListView.MultiSelect = false;
            this.orderResultListView.Name = "orderResultListView";
            this.orderResultListView.ScrollType = NCASFND.NCasListViewScrollType.ScrollBoth;
            this.orderResultListView.SelectioinItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(232)))), ((int)(((byte)(250)))));
            this.orderResultListView.Size = new System.Drawing.Size(1920, 881);
            this.orderResultListView.StateImageList = this.orderResultImageList;
            this.orderResultListView.TabIndex = 3;
            this.orderResultListView.UseFocusBar = true;
            this.orderResultListView.UseInteraceColor = true;
            this.orderResultListView.UseItemFitBox = false;
            this.orderResultListView.UseSelFocusedBar = false;
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(146)))), ((int)(((byte)(213)))));
            this.topPanel.BackgroundImage = global::WmaPAlmScreen.WmaPAlmScreenRsc.bgHeader;
            this.topPanel.Controls.Add(this.topLabel);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(1920, 33);
            this.topPanel.TabIndex = 4;
            // 
            // OrderResultView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Controls.Add(this.orderResultListView);
            this.Controls.Add(this.topPanel);
            this.DoubleBuffered = true;
            this.Name = "OrderResultView";
            this.Size = new System.Drawing.Size(1920, 914);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList orderResultImageList;
        private System.Windows.Forms.Label topLabel;
        private NCASFND.NCasCtrl.NCasListView orderResultListView;
        private NCASFND.NCasCtrl.NCasPanel topPanel;
    }
}
