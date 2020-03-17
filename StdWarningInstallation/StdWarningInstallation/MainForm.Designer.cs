namespace StdWarningInstallation
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
            this.topFileMenuStrip = new System.Windows.Forms.MenuStrip();
            this.프로그램PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.종료XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.이력조회HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.발령이력OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.프로그램사용이력PToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.설정SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.사용자관리UToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.시스템설정관리SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.기능옵션OToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.기본문안관리ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.기상특보연계설정ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.정보IToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.표준발령대정보AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlMainCenter = new System.Windows.Forms.Panel();
            this.pnlOuterLineVisibleCtrl = new System.Windows.Forms.Panel();
            this.pnlOuterLineVisibleTitle = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOuterLineLow = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnOuterLineMiddle = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnOuterLineHigh = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlTargetSystemBody = new System.Windows.Forms.Panel();
            this.pnlSystemGroupList = new System.Windows.Forms.Panel();
            this.lblSystemGroupListTitle = new System.Windows.Forms.Label();
            this.lvSystemGroupList = new StdWarningInstallation.Ctrl.MyListView();
            this.pnlSystemList = new System.Windows.Forms.Panel();
            this.lvStdAlertSystemList = new System.Windows.Forms.ListView();
            this.lblSystemListTitle = new System.Windows.Forms.Label();
            this.btnAddSystemGroup = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnClearSystemSelection = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlTargetAreaBody = new System.Windows.Forms.Panel();
            this.btnClearAreaSelection = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.lvSelectedAreaList = new Adeng.Framework.Ctrl.AdengListView();
            this.pnlLoading = new System.Windows.Forms.Panel();
            this.pnlLoadingBottom = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBoxLoading = new System.Windows.Forms.PictureBox();
            this.pnlMouseModeMain = new System.Windows.Forms.Panel();
            this.lblMouseModeTitle = new System.Windows.Forms.Label();
            this.btnApplyDrawCircle = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.txtboxLongitude = new System.Windows.Forms.TextBox();
            this.txtboxRadius = new System.Windows.Forms.TextBox();
            this.txtboxLatitude = new System.Windows.Forms.TextBox();
            this.pnlUseDrawModeBg = new System.Windows.Forms.Panel();
            this.lblUseCircleModeTitle = new System.Windows.Forms.Label();
            this.btnUseCircleMode = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlDivisionLine = new System.Windows.Forms.Panel();
            this.lblRadiusUnitMeter = new System.Windows.Forms.Label();
            this.lblCircleRadius = new System.Windows.Forms.Label();
            this.lblCircleCoordinator = new System.Windows.Forms.Label();
            this.pnlJumpToRegion = new System.Windows.Forms.Panel();
            this.pnlJumpToRegionTitle = new System.Windows.Forms.Panel();
            this.lblJumpToRegionTitle = new System.Windows.Forms.Label();
            this.btnJumpToRegionListRoll = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.cmbboxJumpToRegionLevel2 = new System.Windows.Forms.ComboBox();
            this.cmbboxJumpToRegionLevel1 = new System.Windows.Forms.ComboBox();
            this.lblJumpingRegionLevel2 = new System.Windows.Forms.Label();
            this.lblJumpingRegionLevel1 = new System.Windows.Forms.Label();
            this.pnlTargetListHeader = new System.Windows.Forms.Panel();
            this.btnTargetListRoll = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.lblTargetListHeaderTitle = new System.Windows.Forms.Label();
            this.pnlTargetRegionBody = new System.Windows.Forms.Panel();
            this.btnAddRegionGroup = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlRegionGroupList = new System.Windows.Forms.Panel();
            this.lblRegionGroupListTitle = new System.Windows.Forms.Label();
            this.lvRegionGroupList = new StdWarningInstallation.Ctrl.MyListView();
            this.pnlRegionList = new System.Windows.Forms.Panel();
            this.btnRegionListRoll = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.lblRegionListTitle = new System.Windows.Forms.Label();
            this.tviewRegionList = new Adeng.Framework.Ctrl.TreeViewEx();
            this.btnClearRegionSelection = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.adengGoogleEarthCtrl = new AdengGE.AdengGoogleEarthCtrl();
            this.bottomStatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelCoord = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelAuth = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelGWComm = new System.Windows.Forms.ToolStripStatusLabel();
            this.imgListCommunicationState = new System.Windows.Forms.ImageList(this.components);
            this.pnlButtonMenuBar = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblLatestOrderSummary = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnMainMenuClearAlert = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnMainMenuSWR = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnTargetingBySystem = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnTargetingByArea = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnTargetingByRegion = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.언어설정ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topFileMenuStrip.SuspendLayout();
            this.pnlMainCenter.SuspendLayout();
            this.pnlOuterLineVisibleCtrl.SuspendLayout();
            this.pnlOuterLineVisibleTitle.SuspendLayout();
            this.pnlTargetSystemBody.SuspendLayout();
            this.pnlSystemGroupList.SuspendLayout();
            this.pnlSystemList.SuspendLayout();
            this.pnlTargetAreaBody.SuspendLayout();
            this.pnlLoading.SuspendLayout();
            this.pnlLoadingBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).BeginInit();
            this.pnlMouseModeMain.SuspendLayout();
            this.pnlUseDrawModeBg.SuspendLayout();
            this.pnlJumpToRegion.SuspendLayout();
            this.pnlJumpToRegionTitle.SuspendLayout();
            this.pnlTargetListHeader.SuspendLayout();
            this.pnlTargetRegionBody.SuspendLayout();
            this.pnlRegionGroupList.SuspendLayout();
            this.pnlRegionList.SuspendLayout();
            this.bottomStatusStrip.SuspendLayout();
            this.pnlButtonMenuBar.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // topFileMenuStrip
            // 
            this.topFileMenuStrip.BackColor = System.Drawing.Color.WhiteSmoke;
            this.topFileMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.프로그램PToolStripMenuItem,
            this.이력조회HToolStripMenuItem,
            this.설정SToolStripMenuItem,
            this.정보IToolStripMenuItem});
            this.topFileMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.topFileMenuStrip.Name = "topFileMenuStrip";
            this.topFileMenuStrip.Size = new System.Drawing.Size(1264, 24);
            this.topFileMenuStrip.TabIndex = 0;
            this.topFileMenuStrip.Text = "상단 메뉴";
            // 
            // 프로그램PToolStripMenuItem
            // 
            this.프로그램PToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.종료XToolStripMenuItem});
            this.프로그램PToolStripMenuItem.Name = "프로그램PToolStripMenuItem";
            this.프로그램PToolStripMenuItem.ShortcutKeyDisplayString = "P";
            this.프로그램PToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
            this.프로그램PToolStripMenuItem.Text = "프로그램(&P)";
            // 
            // 종료XToolStripMenuItem
            // 
            this.종료XToolStripMenuItem.Name = "종료XToolStripMenuItem";
            this.종료XToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.종료XToolStripMenuItem.Text = "종료(&X)";
            this.종료XToolStripMenuItem.Click += new System.EventHandler(this.종료XToolStripMenuItem_Click);
            // 
            // 이력조회HToolStripMenuItem
            // 
            this.이력조회HToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.발령이력OToolStripMenuItem,
            this.프로그램사용이력PToolStripMenuItem});
            this.이력조회HToolStripMenuItem.Name = "이력조회HToolStripMenuItem";
            this.이력조회HToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.이력조회HToolStripMenuItem.Text = "이력 조회(&H)";
            // 
            // 발령이력OToolStripMenuItem
            // 
            this.발령이력OToolStripMenuItem.Name = "발령이력OToolStripMenuItem";
            this.발령이력OToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.발령이력OToolStripMenuItem.Text = "발령 이력(&O)";
            this.발령이력OToolStripMenuItem.Click += new System.EventHandler(this.발령이력OToolStripMenuItem_Click);
            // 
            // 프로그램사용이력PToolStripMenuItem
            // 
            this.프로그램사용이력PToolStripMenuItem.Name = "프로그램사용이력PToolStripMenuItem";
            this.프로그램사용이력PToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.프로그램사용이력PToolStripMenuItem.Text = "프로그램 사용 이력(&P)";
            this.프로그램사용이력PToolStripMenuItem.Click += new System.EventHandler(this.프로그램사용이력PToolStripMenuItem_Click);
            // 
            // 설정SToolStripMenuItem
            // 
            this.설정SToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.사용자관리UToolStripMenuItem,
            this.시스템설정관리SToolStripMenuItem,
            this.기능옵션OToolStripMenuItem});
            this.설정SToolStripMenuItem.Name = "설정SToolStripMenuItem";
            this.설정SToolStripMenuItem.ShortcutKeyDisplayString = "S";
            this.설정SToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.설정SToolStripMenuItem.Text = "설정(&S)";
            // 
            // 사용자관리UToolStripMenuItem
            // 
            this.사용자관리UToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("사용자관리UToolStripMenuItem.Image")));
            this.사용자관리UToolStripMenuItem.Name = "사용자관리UToolStripMenuItem";
            this.사용자관리UToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.사용자관리UToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.사용자관리UToolStripMenuItem.Text = "사용자 관리(&U)";
            this.사용자관리UToolStripMenuItem.Click += new System.EventHandler(this.사용자관리UToolStripMenuItem_Click);
            // 
            // 시스템설정관리SToolStripMenuItem
            // 
            this.시스템설정관리SToolStripMenuItem.Name = "시스템설정관리SToolStripMenuItem";
            this.시스템설정관리SToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.시스템설정관리SToolStripMenuItem.Text = "시스템 설정 관리(&S)";
            this.시스템설정관리SToolStripMenuItem.Click += new System.EventHandler(this.시스템설정관리SToolStripMenuItem_Click);
            // 
            // 기능옵션OToolStripMenuItem
            // 
            this.기능옵션OToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.언어설정ToolStripMenuItem,
            this.기본문안관리ToolStripMenuItem,
            this.기상특보연계설정ToolStripMenuItem});
            this.기능옵션OToolStripMenuItem.Name = "기능옵션OToolStripMenuItem";
            this.기능옵션OToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.기능옵션OToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.기능옵션OToolStripMenuItem.Text = "발령 설정(&O)";
            // 
            // 기본문안관리ToolStripMenuItem
            // 
            this.기본문안관리ToolStripMenuItem.Name = "기본문안관리ToolStripMenuItem";
            this.기본문안관리ToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.기본문안관리ToolStripMenuItem.Text = "기본 문안 관리(&M)";
            this.기본문안관리ToolStripMenuItem.Click += new System.EventHandler(this.기본문안관리ToolStripMenuItem_Click);
            // 
            // 기상특보연계설정ToolStripMenuItem
            // 
            this.기상특보연계설정ToolStripMenuItem.Name = "기상특보연계설정ToolStripMenuItem";
            this.기상특보연계설정ToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.기상특보연계설정ToolStripMenuItem.Text = "기상특보 연계 설정(&W)";
            this.기상특보연계설정ToolStripMenuItem.Click += new System.EventHandler(this.기상특보연계설정ToolStripMenuItem_Click);
            // 
            // 정보IToolStripMenuItem
            // 
            this.정보IToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.표준발령대정보AToolStripMenuItem});
            this.정보IToolStripMenuItem.Name = "정보IToolStripMenuItem";
            this.정보IToolStripMenuItem.ShortcutKeyDisplayString = "I";
            this.정보IToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.정보IToolStripMenuItem.Text = "정보(&I)";
            // 
            // 표준발령대정보AToolStripMenuItem
            // 
            this.표준발령대정보AToolStripMenuItem.Name = "표준발령대정보AToolStripMenuItem";
            this.표준발령대정보AToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.표준발령대정보AToolStripMenuItem.Text = "표준발령대 정보(&A)";
            this.표준발령대정보AToolStripMenuItem.Click += new System.EventHandler(this.표준발령대정보AToolStripMenuItem_Click);
            // 
            // pnlMainCenter
            // 
            this.pnlMainCenter.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMainCenter.BackColor = System.Drawing.SystemColors.Control;
            this.pnlMainCenter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMainCenter.Controls.Add(this.pnlOuterLineVisibleCtrl);
            this.pnlMainCenter.Controls.Add(this.pnlTargetSystemBody);
            this.pnlMainCenter.Controls.Add(this.pnlTargetAreaBody);
            this.pnlMainCenter.Controls.Add(this.pnlLoading);
            this.pnlMainCenter.Controls.Add(this.pnlMouseModeMain);
            this.pnlMainCenter.Controls.Add(this.pnlJumpToRegion);
            this.pnlMainCenter.Controls.Add(this.pnlTargetListHeader);
            this.pnlMainCenter.Controls.Add(this.pnlTargetRegionBody);
            this.pnlMainCenter.Controls.Add(this.adengGoogleEarthCtrl);
            this.pnlMainCenter.Location = new System.Drawing.Point(0, 90);
            this.pnlMainCenter.Margin = new System.Windows.Forms.Padding(0);
            this.pnlMainCenter.Name = "pnlMainCenter";
            this.pnlMainCenter.Size = new System.Drawing.Size(1264, 870);
            this.pnlMainCenter.TabIndex = 3;
            // 
            // pnlOuterLineVisibleCtrl
            // 
            this.pnlOuterLineVisibleCtrl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.pnlOuterLineVisibleCtrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlOuterLineVisibleCtrl.Controls.Add(this.pnlOuterLineVisibleTitle);
            this.pnlOuterLineVisibleCtrl.Controls.Add(this.btnOuterLineLow);
            this.pnlOuterLineVisibleCtrl.Controls.Add(this.btnOuterLineMiddle);
            this.pnlOuterLineVisibleCtrl.Controls.Add(this.btnOuterLineHigh);
            this.pnlOuterLineVisibleCtrl.Location = new System.Drawing.Point(102, 9);
            this.pnlOuterLineVisibleCtrl.Name = "pnlOuterLineVisibleCtrl";
            this.pnlOuterLineVisibleCtrl.Size = new System.Drawing.Size(50, 150);
            this.pnlOuterLineVisibleCtrl.TabIndex = 11;
            this.pnlOuterLineVisibleCtrl.Visible = false;
            // 
            // pnlOuterLineVisibleTitle
            // 
            this.pnlOuterLineVisibleTitle.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlOuterLineVisibleTitle.BackgroundImage")));
            this.pnlOuterLineVisibleTitle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlOuterLineVisibleTitle.Controls.Add(this.label3);
            this.pnlOuterLineVisibleTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlOuterLineVisibleTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.pnlOuterLineVisibleTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlOuterLineVisibleTitle.Margin = new System.Windows.Forms.Padding(0);
            this.pnlOuterLineVisibleTitle.Name = "pnlOuterLineVisibleTitle";
            this.pnlOuterLineVisibleTitle.Size = new System.Drawing.Size(48, 44);
            this.pnlOuterLineVisibleTitle.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 44);
            this.label3.TabIndex = 0;
            this.label3.Text = "경계\r\n표시";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOuterLineLow
            // 
            this.btnOuterLineLow.ChkValue = false;
            this.btnOuterLineLow.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnOuterLineLow.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOuterLineLow.ForeColor = System.Drawing.Color.White;
            this.btnOuterLineLow.Image = ((System.Drawing.Image)(resources.GetObject("btnOuterLineLow.Image")));
            this.btnOuterLineLow.ImgDisable = null;
            this.btnOuterLineLow.ImgHover = null;
            this.btnOuterLineLow.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnOuterLineLow.ImgSelect")));
            this.btnOuterLineLow.ImgStatusEvent = null;
            this.btnOuterLineLow.ImgStatusNormal = null;
            this.btnOuterLineLow.ImgStatusOffsetX = 2;
            this.btnOuterLineLow.ImgStatusOffsetY = 0;
            this.btnOuterLineLow.ImgStretch = true;
            this.btnOuterLineLow.IsImgStatusNormal = true;
            this.btnOuterLineLow.Location = new System.Drawing.Point(4, 112);
            this.btnOuterLineLow.Margin = new System.Windows.Forms.Padding(0);
            this.btnOuterLineLow.Name = "btnOuterLineLow";
            this.btnOuterLineLow.Size = new System.Drawing.Size(40, 32);
            this.btnOuterLineLow.TabIndex = 9;
            this.btnOuterLineLow.Text = "읍면동";
            this.btnOuterLineLow.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnOuterLineLow.UseChecked = true;
            this.btnOuterLineLow.Click += new System.EventHandler(this.btnOuterLineLow_Click);
            // 
            // btnOuterLineMiddle
            // 
            this.btnOuterLineMiddle.ChkValue = false;
            this.btnOuterLineMiddle.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnOuterLineMiddle.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOuterLineMiddle.ForeColor = System.Drawing.Color.White;
            this.btnOuterLineMiddle.Image = ((System.Drawing.Image)(resources.GetObject("btnOuterLineMiddle.Image")));
            this.btnOuterLineMiddle.ImgDisable = null;
            this.btnOuterLineMiddle.ImgHover = null;
            this.btnOuterLineMiddle.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnOuterLineMiddle.ImgSelect")));
            this.btnOuterLineMiddle.ImgStatusEvent = null;
            this.btnOuterLineMiddle.ImgStatusNormal = null;
            this.btnOuterLineMiddle.ImgStatusOffsetX = 2;
            this.btnOuterLineMiddle.ImgStatusOffsetY = 0;
            this.btnOuterLineMiddle.ImgStretch = true;
            this.btnOuterLineMiddle.IsImgStatusNormal = true;
            this.btnOuterLineMiddle.Location = new System.Drawing.Point(4, 80);
            this.btnOuterLineMiddle.Margin = new System.Windows.Forms.Padding(0);
            this.btnOuterLineMiddle.Name = "btnOuterLineMiddle";
            this.btnOuterLineMiddle.Size = new System.Drawing.Size(40, 32);
            this.btnOuterLineMiddle.TabIndex = 9;
            this.btnOuterLineMiddle.Text = "시군구";
            this.btnOuterLineMiddle.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnOuterLineMiddle.UseChecked = true;
            this.btnOuterLineMiddle.Click += new System.EventHandler(this.btnOuterLineMiddle_Click);
            // 
            // btnOuterLineHigh
            // 
            this.btnOuterLineHigh.ChkValue = false;
            this.btnOuterLineHigh.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnOuterLineHigh.Font = new System.Drawing.Font("맑은 고딕", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOuterLineHigh.ForeColor = System.Drawing.Color.White;
            this.btnOuterLineHigh.Image = ((System.Drawing.Image)(resources.GetObject("btnOuterLineHigh.Image")));
            this.btnOuterLineHigh.ImgDisable = null;
            this.btnOuterLineHigh.ImgHover = null;
            this.btnOuterLineHigh.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnOuterLineHigh.ImgSelect")));
            this.btnOuterLineHigh.ImgStatusEvent = null;
            this.btnOuterLineHigh.ImgStatusNormal = null;
            this.btnOuterLineHigh.ImgStatusOffsetX = 2;
            this.btnOuterLineHigh.ImgStatusOffsetY = 0;
            this.btnOuterLineHigh.ImgStretch = true;
            this.btnOuterLineHigh.IsImgStatusNormal = true;
            this.btnOuterLineHigh.Location = new System.Drawing.Point(4, 48);
            this.btnOuterLineHigh.Margin = new System.Windows.Forms.Padding(0);
            this.btnOuterLineHigh.Name = "btnOuterLineHigh";
            this.btnOuterLineHigh.Size = new System.Drawing.Size(40, 32);
            this.btnOuterLineHigh.TabIndex = 9;
            this.btnOuterLineHigh.Text = "시도";
            this.btnOuterLineHigh.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnOuterLineHigh.UseChecked = true;
            this.btnOuterLineHigh.Click += new System.EventHandler(this.btnOuterLineHigh_Click);
            // 
            // pnlTargetSystemBody
            // 
            this.pnlTargetSystemBody.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTargetSystemBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.pnlTargetSystemBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTargetSystemBody.Controls.Add(this.pnlSystemGroupList);
            this.pnlTargetSystemBody.Controls.Add(this.pnlSystemList);
            this.pnlTargetSystemBody.Controls.Add(this.btnAddSystemGroup);
            this.pnlTargetSystemBody.Controls.Add(this.btnClearSystemSelection);
            this.pnlTargetSystemBody.Location = new System.Drawing.Point(603, 34);
            this.pnlTargetSystemBody.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTargetSystemBody.Name = "pnlTargetSystemBody";
            this.pnlTargetSystemBody.Size = new System.Drawing.Size(210, 776);
            this.pnlTargetSystemBody.TabIndex = 23;
            this.pnlTargetSystemBody.Visible = false;
            // 
            // pnlSystemGroupList
            // 
            this.pnlSystemGroupList.Controls.Add(this.lblSystemGroupListTitle);
            this.pnlSystemGroupList.Controls.Add(this.lvSystemGroupList);
            this.pnlSystemGroupList.Location = new System.Drawing.Point(6, 476);
            this.pnlSystemGroupList.Margin = new System.Windows.Forms.Padding(0);
            this.pnlSystemGroupList.Name = "pnlSystemGroupList";
            this.pnlSystemGroupList.Size = new System.Drawing.Size(194, 249);
            this.pnlSystemGroupList.TabIndex = 24;
            // 
            // lblSystemGroupListTitle
            // 
            this.lblSystemGroupListTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSystemGroupListTitle.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSystemGroupListTitle.ForeColor = System.Drawing.Color.White;
            this.lblSystemGroupListTitle.Image = ((System.Drawing.Image)(resources.GetObject("lblSystemGroupListTitle.Image")));
            this.lblSystemGroupListTitle.Location = new System.Drawing.Point(0, 0);
            this.lblSystemGroupListTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblSystemGroupListTitle.Name = "lblSystemGroupListTitle";
            this.lblSystemGroupListTitle.Size = new System.Drawing.Size(194, 32);
            this.lblSystemGroupListTitle.TabIndex = 3;
            this.lblSystemGroupListTitle.Text = " 그룹 목록";
            this.lblSystemGroupListTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvSystemGroupList
            // 
            this.lvSystemGroupList.BackColor = System.Drawing.Color.White;
            this.lvSystemGroupList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvSystemGroupList.CheckBoxes = true;
            this.lvSystemGroupList.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.lvSystemGroupList.FullRowSelect = true;
            this.lvSystemGroupList.GridLines = true;
            this.lvSystemGroupList.HideSelection = false;
            this.lvSystemGroupList.Location = new System.Drawing.Point(0, 32);
            this.lvSystemGroupList.Margin = new System.Windows.Forms.Padding(0);
            this.lvSystemGroupList.MultiSelect = false;
            this.lvSystemGroupList.Name = "lvSystemGroupList";
            this.lvSystemGroupList.ShowItemToolTips = true;
            this.lvSystemGroupList.Size = new System.Drawing.Size(194, 217);
            this.lvSystemGroupList.TabIndex = 7;
            this.lvSystemGroupList.UseCompatibleStateImageBehavior = false;
            this.lvSystemGroupList.View = System.Windows.Forms.View.Details;
            this.lvSystemGroupList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lvSystemGroupList_ItemCheck);
            this.lvSystemGroupList.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvSystemGroupList_ItemChecked);
            this.lvSystemGroupList.DoubleClick += new System.EventHandler(this.lvSystemGroupList_DoubleClick);
            // 
            // pnlSystemList
            // 
            this.pnlSystemList.Controls.Add(this.lvStdAlertSystemList);
            this.pnlSystemList.Controls.Add(this.lblSystemListTitle);
            this.pnlSystemList.Location = new System.Drawing.Point(6, 6);
            this.pnlSystemList.Margin = new System.Windows.Forms.Padding(0);
            this.pnlSystemList.Name = "pnlSystemList";
            this.pnlSystemList.Size = new System.Drawing.Size(194, 464);
            this.pnlSystemList.TabIndex = 22;
            // 
            // lvStdAlertSystemList
            // 
            this.lvStdAlertSystemList.BackColor = System.Drawing.Color.White;
            this.lvStdAlertSystemList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvStdAlertSystemList.CheckBoxes = true;
            this.lvStdAlertSystemList.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lvStdAlertSystemList.FullRowSelect = true;
            this.lvStdAlertSystemList.GridLines = true;
            this.lvStdAlertSystemList.HideSelection = false;
            this.lvStdAlertSystemList.Location = new System.Drawing.Point(0, 32);
            this.lvStdAlertSystemList.Margin = new System.Windows.Forms.Padding(0);
            this.lvStdAlertSystemList.MultiSelect = false;
            this.lvStdAlertSystemList.Name = "lvStdAlertSystemList";
            this.lvStdAlertSystemList.Size = new System.Drawing.Size(194, 432);
            this.lvStdAlertSystemList.TabIndex = 20;
            this.lvStdAlertSystemList.UseCompatibleStateImageBehavior = false;
            this.lvStdAlertSystemList.View = System.Windows.Forms.View.Details;
            this.lvStdAlertSystemList.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvStdAlertSystemList_ItemChecked);
            // 
            // lblSystemListTitle
            // 
            this.lblSystemListTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSystemListTitle.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSystemListTitle.ForeColor = System.Drawing.Color.White;
            this.lblSystemListTitle.Image = ((System.Drawing.Image)(resources.GetObject("lblSystemListTitle.Image")));
            this.lblSystemListTitle.Location = new System.Drawing.Point(0, 0);
            this.lblSystemListTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblSystemListTitle.Name = "lblSystemListTitle";
            this.lblSystemListTitle.Size = new System.Drawing.Size(194, 32);
            this.lblSystemListTitle.TabIndex = 0;
            this.lblSystemListTitle.Text = " 시스템 목록";
            this.lblSystemListTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAddSystemGroup
            // 
            this.btnAddSystemGroup.ChkValue = false;
            this.btnAddSystemGroup.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnAddSystemGroup.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAddSystemGroup.ForeColor = System.Drawing.Color.White;
            this.btnAddSystemGroup.Image = ((System.Drawing.Image)(resources.GetObject("btnAddSystemGroup.Image")));
            this.btnAddSystemGroup.ImgDisable = null;
            this.btnAddSystemGroup.ImgHover = null;
            this.btnAddSystemGroup.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnAddSystemGroup.ImgSelect")));
            this.btnAddSystemGroup.ImgStatusEvent = null;
            this.btnAddSystemGroup.ImgStatusNormal = null;
            this.btnAddSystemGroup.ImgStatusOffsetX = 2;
            this.btnAddSystemGroup.ImgStatusOffsetY = 0;
            this.btnAddSystemGroup.ImgStretch = true;
            this.btnAddSystemGroup.IsImgStatusNormal = true;
            this.btnAddSystemGroup.Location = new System.Drawing.Point(110, 732);
            this.btnAddSystemGroup.Margin = new System.Windows.Forms.Padding(0);
            this.btnAddSystemGroup.Name = "btnAddSystemGroup";
            this.btnAddSystemGroup.Size = new System.Drawing.Size(90, 36);
            this.btnAddSystemGroup.TabIndex = 21;
            this.btnAddSystemGroup.Text = "그룹 추가";
            this.btnAddSystemGroup.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnAddSystemGroup.UseChecked = false;
            this.btnAddSystemGroup.Click += new System.EventHandler(this.btnAddSystemGroup_Click);
            // 
            // btnClearSystemSelection
            // 
            this.btnClearSystemSelection.ChkValue = false;
            this.btnClearSystemSelection.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnClearSystemSelection.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClearSystemSelection.ForeColor = System.Drawing.Color.White;
            this.btnClearSystemSelection.Image = ((System.Drawing.Image)(resources.GetObject("btnClearSystemSelection.Image")));
            this.btnClearSystemSelection.ImgDisable = null;
            this.btnClearSystemSelection.ImgHover = null;
            this.btnClearSystemSelection.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnClearSystemSelection.ImgSelect")));
            this.btnClearSystemSelection.ImgStatusEvent = null;
            this.btnClearSystemSelection.ImgStatusNormal = null;
            this.btnClearSystemSelection.ImgStatusOffsetX = 2;
            this.btnClearSystemSelection.ImgStatusOffsetY = 0;
            this.btnClearSystemSelection.ImgStretch = false;
            this.btnClearSystemSelection.IsImgStatusNormal = true;
            this.btnClearSystemSelection.Location = new System.Drawing.Point(6, 732);
            this.btnClearSystemSelection.Margin = new System.Windows.Forms.Padding(0);
            this.btnClearSystemSelection.Name = "btnClearSystemSelection";
            this.btnClearSystemSelection.Size = new System.Drawing.Size(90, 36);
            this.btnClearSystemSelection.TabIndex = 3;
            this.btnClearSystemSelection.Text = "선택 취소";
            this.btnClearSystemSelection.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnClearSystemSelection.UseChecked = true;
            this.btnClearSystemSelection.Click += new System.EventHandler(this.btnClearTargetSelection_Click);
            // 
            // pnlTargetAreaBody
            // 
            this.pnlTargetAreaBody.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTargetAreaBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.pnlTargetAreaBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTargetAreaBody.Controls.Add(this.btnClearAreaSelection);
            this.pnlTargetAreaBody.Controls.Add(this.lvSelectedAreaList);
            this.pnlTargetAreaBody.Location = new System.Drawing.Point(832, 99);
            this.pnlTargetAreaBody.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTargetAreaBody.Name = "pnlTargetAreaBody";
            this.pnlTargetAreaBody.Size = new System.Drawing.Size(210, 676);
            this.pnlTargetAreaBody.TabIndex = 23;
            this.pnlTargetAreaBody.Visible = false;
            // 
            // btnClearAreaSelection
            // 
            this.btnClearAreaSelection.ChkValue = false;
            this.btnClearAreaSelection.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnClearAreaSelection.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClearAreaSelection.ForeColor = System.Drawing.Color.White;
            this.btnClearAreaSelection.Image = ((System.Drawing.Image)(resources.GetObject("btnClearAreaSelection.Image")));
            this.btnClearAreaSelection.ImgDisable = null;
            this.btnClearAreaSelection.ImgHover = null;
            this.btnClearAreaSelection.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnClearAreaSelection.ImgSelect")));
            this.btnClearAreaSelection.ImgStatusEvent = null;
            this.btnClearAreaSelection.ImgStatusNormal = null;
            this.btnClearAreaSelection.ImgStatusOffsetX = 2;
            this.btnClearAreaSelection.ImgStatusOffsetY = 0;
            this.btnClearAreaSelection.ImgStretch = false;
            this.btnClearAreaSelection.IsImgStatusNormal = true;
            this.btnClearAreaSelection.Location = new System.Drawing.Point(6, 632);
            this.btnClearAreaSelection.Margin = new System.Windows.Forms.Padding(0);
            this.btnClearAreaSelection.Name = "btnClearAreaSelection";
            this.btnClearAreaSelection.Size = new System.Drawing.Size(90, 36);
            this.btnClearAreaSelection.TabIndex = 3;
            this.btnClearAreaSelection.Text = "선택 취소";
            this.btnClearAreaSelection.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnClearAreaSelection.UseChecked = true;
            this.btnClearAreaSelection.Click += new System.EventHandler(this.btnClearTargetSelection_Click);
            // 
            // lvSelectedAreaList
            // 
            this.lvSelectedAreaList.AntiAlias = false;
            this.lvSelectedAreaList.AutoFit = false;
            this.lvSelectedAreaList.BackColor = System.Drawing.Color.White;
            this.lvSelectedAreaList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvSelectedAreaList.ColumnHeight = 24;
            this.lvSelectedAreaList.ColumnOffset = new System.Windows.Forms.Padding(2, 2, 0, 1);
            this.lvSelectedAreaList.ColumnStyle = Adeng.Framework.Ctrl.AdengListViewColumnStyle.DEF_STYLE;
            this.lvSelectedAreaList.FocusedItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(185)))), ((int)(((byte)(230)))));
            this.lvSelectedAreaList.FrozenColumnIndex = -1;
            this.lvSelectedAreaList.FullRowSelect = true;
            this.lvSelectedAreaList.GridDashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.lvSelectedAreaList.GridLineStyle = Adeng.Framework.Ctrl.AdengListViewGridLine.NONE;
            this.lvSelectedAreaList.HideSelection = false;
            this.lvSelectedAreaList.HoverSelection = false;
            this.lvSelectedAreaList.IconOffset = new System.Drawing.Point(1, 0);
            this.lvSelectedAreaList.InteraceColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.lvSelectedAreaList.InteraceColor2 = System.Drawing.Color.White;
            this.lvSelectedAreaList.ItemHeight = 18;
            this.lvSelectedAreaList.ItemOffset = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lvSelectedAreaList.Location = new System.Drawing.Point(6, 6);
            this.lvSelectedAreaList.Margin = new System.Windows.Forms.Padding(0);
            this.lvSelectedAreaList.MultiSelect = false;
            this.lvSelectedAreaList.Name = "lvSelectedAreaList";
            this.lvSelectedAreaList.ScrollType = Adeng.Framework.Ctrl.AdengListViewScrollType.BOTH;
            this.lvSelectedAreaList.SelectioinItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(232)))), ((int)(((byte)(250)))));
            this.lvSelectedAreaList.Size = new System.Drawing.Size(194, 618);
            this.lvSelectedAreaList.TabIndex = 4;
            this.lvSelectedAreaList.UseInteraceColor = true;
            this.lvSelectedAreaList.UseSelFocusedBar = false;
            this.lvSelectedAreaList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvSelectedAreaList_KeyDown);
            this.lvSelectedAreaList.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.lvSelectedAreaList_PreviewKeyDown);
            // 
            // pnlLoading
            // 
            this.pnlLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.pnlLoading.BackColor = System.Drawing.Color.Transparent;
            this.pnlLoading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlLoading.Controls.Add(this.pnlLoadingBottom);
            this.pnlLoading.Controls.Add(this.pictureBoxLoading);
            this.pnlLoading.Location = new System.Drawing.Point(534, 275);
            this.pnlLoading.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLoading.MaximumSize = new System.Drawing.Size(170, 170);
            this.pnlLoading.MinimumSize = new System.Drawing.Size(170, 170);
            this.pnlLoading.Name = "pnlLoading";
            this.pnlLoading.Size = new System.Drawing.Size(170, 170);
            this.pnlLoading.TabIndex = 10;
            this.pnlLoading.Visible = false;
            // 
            // pnlLoadingBottom
            // 
            this.pnlLoadingBottom.BackColor = System.Drawing.Color.Transparent;
            this.pnlLoadingBottom.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlLoadingBottom.BackgroundImage")));
            this.pnlLoadingBottom.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlLoadingBottom.Controls.Add(this.label1);
            this.pnlLoadingBottom.Location = new System.Drawing.Point(0, 130);
            this.pnlLoadingBottom.Margin = new System.Windows.Forms.Padding(0);
            this.pnlLoadingBottom.Name = "pnlLoadingBottom";
            this.pnlLoadingBottom.Size = new System.Drawing.Size(170, 40);
            this.pnlLoadingBottom.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 40);
            this.label1.TabIndex = 8;
            this.label1.Text = "지도 데이터 로딩 중";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBoxLoading
            // 
            this.pictureBoxLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxLoading.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxLoading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxLoading.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxLoading.Image")));
            this.pictureBoxLoading.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBoxLoading.InitialImage")));
            this.pictureBoxLoading.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxLoading.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxLoading.Name = "pictureBoxLoading";
            this.pictureBoxLoading.Size = new System.Drawing.Size(170, 130);
            this.pictureBoxLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxLoading.TabIndex = 8;
            this.pictureBoxLoading.TabStop = false;
            // 
            // pnlMouseModeMain
            // 
            this.pnlMouseModeMain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlMouseModeMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.pnlMouseModeMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMouseModeMain.Controls.Add(this.lblMouseModeTitle);
            this.pnlMouseModeMain.Controls.Add(this.btnApplyDrawCircle);
            this.pnlMouseModeMain.Controls.Add(this.txtboxLongitude);
            this.pnlMouseModeMain.Controls.Add(this.txtboxRadius);
            this.pnlMouseModeMain.Controls.Add(this.txtboxLatitude);
            this.pnlMouseModeMain.Controls.Add(this.pnlUseDrawModeBg);
            this.pnlMouseModeMain.Controls.Add(this.pnlDivisionLine);
            this.pnlMouseModeMain.Controls.Add(this.lblRadiusUnitMeter);
            this.pnlMouseModeMain.Controls.Add(this.lblCircleRadius);
            this.pnlMouseModeMain.Controls.Add(this.lblCircleCoordinator);
            this.pnlMouseModeMain.Location = new System.Drawing.Point(3, 727);
            this.pnlMouseModeMain.Name = "pnlMouseModeMain";
            this.pnlMouseModeMain.Size = new System.Drawing.Size(279, 138);
            this.pnlMouseModeMain.TabIndex = 6;
            this.pnlMouseModeMain.Visible = false;
            // 
            // lblMouseModeTitle
            // 
            this.lblMouseModeTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblMouseModeTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMouseModeTitle.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblMouseModeTitle.ForeColor = System.Drawing.Color.White;
            this.lblMouseModeTitle.Image = ((System.Drawing.Image)(resources.GetObject("lblMouseModeTitle.Image")));
            this.lblMouseModeTitle.Location = new System.Drawing.Point(0, 0);
            this.lblMouseModeTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblMouseModeTitle.Name = "lblMouseModeTitle";
            this.lblMouseModeTitle.Size = new System.Drawing.Size(277, 32);
            this.lblMouseModeTitle.TabIndex = 10;
            this.lblMouseModeTitle.Text = " 반경 그리기 모드";
            this.lblMouseModeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnApplyDrawCircle
            // 
            this.btnApplyDrawCircle.ChkValue = false;
            this.btnApplyDrawCircle.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnApplyDrawCircle.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnApplyDrawCircle.ForeColor = System.Drawing.Color.White;
            this.btnApplyDrawCircle.Image = ((System.Drawing.Image)(resources.GetObject("btnApplyDrawCircle.Image")));
            this.btnApplyDrawCircle.ImgDisable = null;
            this.btnApplyDrawCircle.ImgHover = null;
            this.btnApplyDrawCircle.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnApplyDrawCircle.ImgSelect")));
            this.btnApplyDrawCircle.ImgStatusEvent = null;
            this.btnApplyDrawCircle.ImgStatusNormal = null;
            this.btnApplyDrawCircle.ImgStatusOffsetX = 2;
            this.btnApplyDrawCircle.ImgStatusOffsetY = 0;
            this.btnApplyDrawCircle.ImgStretch = false;
            this.btnApplyDrawCircle.IsImgStatusNormal = true;
            this.btnApplyDrawCircle.Location = new System.Drawing.Point(226, 77);
            this.btnApplyDrawCircle.Margin = new System.Windows.Forms.Padding(0);
            this.btnApplyDrawCircle.Name = "btnApplyDrawCircle";
            this.btnApplyDrawCircle.Size = new System.Drawing.Size(45, 53);
            this.btnApplyDrawCircle.TabIndex = 9;
            this.btnApplyDrawCircle.Text = "적용";
            this.btnApplyDrawCircle.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnApplyDrawCircle.UseChecked = true;
            this.btnApplyDrawCircle.Click += new System.EventHandler(this.btnApplyDrawCircle_Click);
            // 
            // txtboxLongitude
            // 
            this.txtboxLongitude.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxLongitude.Location = new System.Drawing.Point(132, 77);
            this.txtboxLongitude.Margin = new System.Windows.Forms.Padding(0);
            this.txtboxLongitude.Name = "txtboxLongitude";
            this.txtboxLongitude.Size = new System.Drawing.Size(86, 23);
            this.txtboxLongitude.TabIndex = 8;
            // 
            // txtboxRadius
            // 
            this.txtboxRadius.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxRadius.Location = new System.Drawing.Point(43, 106);
            this.txtboxRadius.Margin = new System.Windows.Forms.Padding(0);
            this.txtboxRadius.Name = "txtboxRadius";
            this.txtboxRadius.Size = new System.Drawing.Size(86, 23);
            this.txtboxRadius.TabIndex = 8;
            // 
            // txtboxLatitude
            // 
            this.txtboxLatitude.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxLatitude.Location = new System.Drawing.Point(43, 77);
            this.txtboxLatitude.Margin = new System.Windows.Forms.Padding(0);
            this.txtboxLatitude.Name = "txtboxLatitude";
            this.txtboxLatitude.Size = new System.Drawing.Size(86, 23);
            this.txtboxLatitude.TabIndex = 8;
            // 
            // pnlUseDrawModeBg
            // 
            this.pnlUseDrawModeBg.Controls.Add(this.lblUseCircleModeTitle);
            this.pnlUseDrawModeBg.Controls.Add(this.btnUseCircleMode);
            this.pnlUseDrawModeBg.Location = new System.Drawing.Point(0, 32);
            this.pnlUseDrawModeBg.Margin = new System.Windows.Forms.Padding(0);
            this.pnlUseDrawModeBg.Name = "pnlUseDrawModeBg";
            this.pnlUseDrawModeBg.Size = new System.Drawing.Size(277, 34);
            this.pnlUseDrawModeBg.TabIndex = 7;
            // 
            // lblUseCircleModeTitle
            // 
            this.lblUseCircleModeTitle.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblUseCircleModeTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblUseCircleModeTitle.Location = new System.Drawing.Point(43, 4);
            this.lblUseCircleModeTitle.Name = "lblUseCircleModeTitle";
            this.lblUseCircleModeTitle.Size = new System.Drawing.Size(227, 26);
            this.lblUseCircleModeTitle.TabIndex = 3;
            this.lblUseCircleModeTitle.Text = "그리기 모드 실행";
            this.lblUseCircleModeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblUseCircleModeTitle.Click += new System.EventHandler(this.lblUseCircleModeTitle_Click);
            // 
            // btnUseCircleMode
            // 
            this.btnUseCircleMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUseCircleMode.ChkValue = false;
            this.btnUseCircleMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUseCircleMode.Image = ((System.Drawing.Image)(resources.GetObject("btnUseCircleMode.Image")));
            this.btnUseCircleMode.ImgDisable = null;
            this.btnUseCircleMode.ImgHover = null;
            this.btnUseCircleMode.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnUseCircleMode.ImgSelect")));
            this.btnUseCircleMode.ImgStatusEvent = null;
            this.btnUseCircleMode.ImgStatusNormal = null;
            this.btnUseCircleMode.ImgStatusOffsetX = 2;
            this.btnUseCircleMode.ImgStatusOffsetY = 0;
            this.btnUseCircleMode.ImgStretch = false;
            this.btnUseCircleMode.IsImgStatusNormal = true;
            this.btnUseCircleMode.Location = new System.Drawing.Point(11, 4);
            this.btnUseCircleMode.Name = "btnUseCircleMode";
            this.btnUseCircleMode.Size = new System.Drawing.Size(26, 26);
            this.btnUseCircleMode.TabIndex = 2;
            this.btnUseCircleMode.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnUseCircleMode.UseChecked = true;
            this.btnUseCircleMode.Click += new System.EventHandler(this.btnUseCircleMode_Click);
            // 
            // pnlDivisionLine
            // 
            this.pnlDivisionLine.BackColor = System.Drawing.Color.Gray;
            this.pnlDivisionLine.Location = new System.Drawing.Point(5, 35);
            this.pnlDivisionLine.Name = "pnlDivisionLine";
            this.pnlDivisionLine.Size = new System.Drawing.Size(268, 32);
            this.pnlDivisionLine.TabIndex = 6;
            // 
            // lblRadiusUnitMeter
            // 
            this.lblRadiusUnitMeter.AutoSize = true;
            this.lblRadiusUnitMeter.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRadiusUnitMeter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblRadiusUnitMeter.Location = new System.Drawing.Point(130, 114);
            this.lblRadiusUnitMeter.Name = "lblRadiusUnitMeter";
            this.lblRadiusUnitMeter.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblRadiusUnitMeter.Size = new System.Drawing.Size(20, 12);
            this.lblRadiusUnitMeter.TabIndex = 3;
            this.lblRadiusUnitMeter.Text = "m";
            // 
            // lblCircleRadius
            // 
            this.lblCircleRadius.AutoSize = true;
            this.lblCircleRadius.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCircleRadius.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblCircleRadius.Location = new System.Drawing.Point(4, 111);
            this.lblCircleRadius.Name = "lblCircleRadius";
            this.lblCircleRadius.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblCircleRadius.Size = new System.Drawing.Size(34, 12);
            this.lblCircleRadius.TabIndex = 3;
            this.lblCircleRadius.Text = "반경";
            // 
            // lblCircleCoordinator
            // 
            this.lblCircleCoordinator.AutoSize = true;
            this.lblCircleCoordinator.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCircleCoordinator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblCircleCoordinator.Location = new System.Drawing.Point(4, 83);
            this.lblCircleCoordinator.Margin = new System.Windows.Forms.Padding(0);
            this.lblCircleCoordinator.Name = "lblCircleCoordinator";
            this.lblCircleCoordinator.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblCircleCoordinator.Size = new System.Drawing.Size(34, 12);
            this.lblCircleCoordinator.TabIndex = 3;
            this.lblCircleCoordinator.Text = "좌표";
            // 
            // pnlJumpToRegion
            // 
            this.pnlJumpToRegion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlJumpToRegion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.pnlJumpToRegion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlJumpToRegion.Controls.Add(this.pnlJumpToRegionTitle);
            this.pnlJumpToRegion.Controls.Add(this.cmbboxJumpToRegionLevel2);
            this.pnlJumpToRegion.Controls.Add(this.cmbboxJumpToRegionLevel1);
            this.pnlJumpToRegion.Controls.Add(this.lblJumpingRegionLevel2);
            this.pnlJumpToRegion.Controls.Add(this.lblJumpingRegionLevel1);
            this.pnlJumpToRegion.Location = new System.Drawing.Point(822, 3);
            this.pnlJumpToRegion.Name = "pnlJumpToRegion";
            this.pnlJumpToRegion.Size = new System.Drawing.Size(220, 93);
            this.pnlJumpToRegion.TabIndex = 5;
            this.pnlJumpToRegion.Visible = false;
            // 
            // pnlJumpToRegionTitle
            // 
            this.pnlJumpToRegionTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlJumpToRegionTitle.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlJumpToRegionTitle.BackgroundImage")));
            this.pnlJumpToRegionTitle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlJumpToRegionTitle.Controls.Add(this.lblJumpToRegionTitle);
            this.pnlJumpToRegionTitle.Controls.Add(this.btnJumpToRegionListRoll);
            this.pnlJumpToRegionTitle.Location = new System.Drawing.Point(0, 0);
            this.pnlJumpToRegionTitle.Margin = new System.Windows.Forms.Padding(0);
            this.pnlJumpToRegionTitle.Name = "pnlJumpToRegionTitle";
            this.pnlJumpToRegionTitle.Size = new System.Drawing.Size(220, 32);
            this.pnlJumpToRegionTitle.TabIndex = 5;
            // 
            // lblJumpToRegionTitle
            // 
            this.lblJumpToRegionTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblJumpToRegionTitle.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblJumpToRegionTitle.ForeColor = System.Drawing.Color.White;
            this.lblJumpToRegionTitle.Location = new System.Drawing.Point(0, 0);
            this.lblJumpToRegionTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblJumpToRegionTitle.Name = "lblJumpToRegionTitle";
            this.lblJumpToRegionTitle.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblJumpToRegionTitle.Size = new System.Drawing.Size(162, 32);
            this.lblJumpToRegionTitle.TabIndex = 0;
            this.lblJumpToRegionTitle.Text = "빠른 지역 이동";
            this.lblJumpToRegionTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnJumpToRegionListRoll
            // 
            this.btnJumpToRegionListRoll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnJumpToRegionListRoll.ChkValue = false;
            this.btnJumpToRegionListRoll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnJumpToRegionListRoll.Image = ((System.Drawing.Image)(resources.GetObject("btnJumpToRegionListRoll.Image")));
            this.btnJumpToRegionListRoll.ImgDisable = null;
            this.btnJumpToRegionListRoll.ImgHover = null;
            this.btnJumpToRegionListRoll.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnJumpToRegionListRoll.ImgSelect")));
            this.btnJumpToRegionListRoll.ImgStatusEvent = null;
            this.btnJumpToRegionListRoll.ImgStatusNormal = null;
            this.btnJumpToRegionListRoll.ImgStatusOffsetX = 2;
            this.btnJumpToRegionListRoll.ImgStatusOffsetY = 0;
            this.btnJumpToRegionListRoll.ImgStretch = false;
            this.btnJumpToRegionListRoll.IsImgStatusNormal = true;
            this.btnJumpToRegionListRoll.Location = new System.Drawing.Point(195, 6);
            this.btnJumpToRegionListRoll.Name = "btnJumpToRegionListRoll";
            this.btnJumpToRegionListRoll.Size = new System.Drawing.Size(18, 18);
            this.btnJumpToRegionListRoll.TabIndex = 2;
            this.btnJumpToRegionListRoll.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnJumpToRegionListRoll.UseChecked = true;
            this.btnJumpToRegionListRoll.Click += new System.EventHandler(this.btnJumpToRegionListRoll_Click);
            // 
            // cmbboxJumpToRegionLevel2
            // 
            this.cmbboxJumpToRegionLevel2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbboxJumpToRegionLevel2.FormattingEnabled = true;
            this.cmbboxJumpToRegionLevel2.Location = new System.Drawing.Point(92, 64);
            this.cmbboxJumpToRegionLevel2.Name = "cmbboxJumpToRegionLevel2";
            this.cmbboxJumpToRegionLevel2.Size = new System.Drawing.Size(122, 23);
            this.cmbboxJumpToRegionLevel2.TabIndex = 4;
            this.cmbboxJumpToRegionLevel2.SelectedIndexChanged += new System.EventHandler(this.cmbboxJumpToRegionLevel2_SelectedIndexChanged);
            // 
            // cmbboxJumpToRegionLevel1
            // 
            this.cmbboxJumpToRegionLevel1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbboxJumpToRegionLevel1.FormattingEnabled = true;
            this.cmbboxJumpToRegionLevel1.Location = new System.Drawing.Point(92, 36);
            this.cmbboxJumpToRegionLevel1.Name = "cmbboxJumpToRegionLevel1";
            this.cmbboxJumpToRegionLevel1.Size = new System.Drawing.Size(122, 23);
            this.cmbboxJumpToRegionLevel1.TabIndex = 4;
            this.cmbboxJumpToRegionLevel1.SelectedIndexChanged += new System.EventHandler(this.cmbboxJumpToRegionLevel1_SelectedIndexChanged);
            // 
            // lblJumpingRegionLevel2
            // 
            this.lblJumpingRegionLevel2.AutoSize = true;
            this.lblJumpingRegionLevel2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblJumpingRegionLevel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblJumpingRegionLevel2.Location = new System.Drawing.Point(3, 70);
            this.lblJumpingRegionLevel2.Name = "lblJumpingRegionLevel2";
            this.lblJumpingRegionLevel2.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblJumpingRegionLevel2.Size = new System.Drawing.Size(47, 12);
            this.lblJumpingRegionLevel2.TabIndex = 3;
            this.lblJumpingRegionLevel2.Text = "읍면동";
            // 
            // lblJumpingRegionLevel1
            // 
            this.lblJumpingRegionLevel1.AutoSize = true;
            this.lblJumpingRegionLevel1.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblJumpingRegionLevel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblJumpingRegionLevel1.Location = new System.Drawing.Point(3, 42);
            this.lblJumpingRegionLevel1.Margin = new System.Windows.Forms.Padding(0);
            this.lblJumpingRegionLevel1.Name = "lblJumpingRegionLevel1";
            this.lblJumpingRegionLevel1.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblJumpingRegionLevel1.Size = new System.Drawing.Size(80, 12);
            this.lblJumpingRegionLevel1.TabIndex = 3;
            this.lblJumpingRegionLevel1.Text = "전체/시군구";
            // 
            // pnlTargetListHeader
            // 
            this.pnlTargetListHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTargetListHeader.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlTargetListHeader.BackgroundImage")));
            this.pnlTargetListHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlTargetListHeader.Controls.Add(this.btnTargetListRoll);
            this.pnlTargetListHeader.Controls.Add(this.lblTargetListHeaderTitle);
            this.pnlTargetListHeader.Location = new System.Drawing.Point(1049, 3);
            this.pnlTargetListHeader.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTargetListHeader.Name = "pnlTargetListHeader";
            this.pnlTargetListHeader.Size = new System.Drawing.Size(210, 32);
            this.pnlTargetListHeader.TabIndex = 4;
            // 
            // btnTargetListRoll
            // 
            this.btnTargetListRoll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTargetListRoll.ChkValue = false;
            this.btnTargetListRoll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTargetListRoll.Image = ((System.Drawing.Image)(resources.GetObject("btnTargetListRoll.Image")));
            this.btnTargetListRoll.ImgDisable = null;
            this.btnTargetListRoll.ImgHover = null;
            this.btnTargetListRoll.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnTargetListRoll.ImgSelect")));
            this.btnTargetListRoll.ImgStatusEvent = null;
            this.btnTargetListRoll.ImgStatusNormal = null;
            this.btnTargetListRoll.ImgStatusOffsetX = 2;
            this.btnTargetListRoll.ImgStatusOffsetY = 0;
            this.btnTargetListRoll.ImgStretch = false;
            this.btnTargetListRoll.IsImgStatusNormal = true;
            this.btnTargetListRoll.Location = new System.Drawing.Point(185, 7);
            this.btnTargetListRoll.Name = "btnTargetListRoll";
            this.btnTargetListRoll.Size = new System.Drawing.Size(18, 18);
            this.btnTargetListRoll.TabIndex = 1;
            this.btnTargetListRoll.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnTargetListRoll.UseChecked = true;
            this.btnTargetListRoll.Click += new System.EventHandler(this.btnTargetListRoll_Click);
            // 
            // lblTargetListHeaderTitle
            // 
            this.lblTargetListHeaderTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTargetListHeaderTitle.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTargetListHeaderTitle.ForeColor = System.Drawing.Color.White;
            this.lblTargetListHeaderTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTargetListHeaderTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTargetListHeaderTitle.Name = "lblTargetListHeaderTitle";
            this.lblTargetListHeaderTitle.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblTargetListHeaderTitle.Size = new System.Drawing.Size(162, 32);
            this.lblTargetListHeaderTitle.TabIndex = 0;
            this.lblTargetListHeaderTitle.Text = "지역 목록";
            this.lblTargetListHeaderTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlTargetRegionBody
            // 
            this.pnlTargetRegionBody.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTargetRegionBody.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.pnlTargetRegionBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTargetRegionBody.Controls.Add(this.btnAddRegionGroup);
            this.pnlTargetRegionBody.Controls.Add(this.pnlRegionGroupList);
            this.pnlTargetRegionBody.Controls.Add(this.pnlRegionList);
            this.pnlTargetRegionBody.Controls.Add(this.btnClearRegionSelection);
            this.pnlTargetRegionBody.Location = new System.Drawing.Point(1049, 34);
            this.pnlTargetRegionBody.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTargetRegionBody.Name = "pnlTargetRegionBody";
            this.pnlTargetRegionBody.Size = new System.Drawing.Size(210, 776);
            this.pnlTargetRegionBody.TabIndex = 3;
            this.pnlTargetRegionBody.Visible = false;
            // 
            // btnAddRegionGroup
            // 
            this.btnAddRegionGroup.ChkValue = false;
            this.btnAddRegionGroup.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnAddRegionGroup.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnAddRegionGroup.ForeColor = System.Drawing.Color.White;
            this.btnAddRegionGroup.Image = ((System.Drawing.Image)(resources.GetObject("btnAddRegionGroup.Image")));
            this.btnAddRegionGroup.ImgDisable = null;
            this.btnAddRegionGroup.ImgHover = null;
            this.btnAddRegionGroup.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnAddRegionGroup.ImgSelect")));
            this.btnAddRegionGroup.ImgStatusEvent = null;
            this.btnAddRegionGroup.ImgStatusNormal = null;
            this.btnAddRegionGroup.ImgStatusOffsetX = 2;
            this.btnAddRegionGroup.ImgStatusOffsetY = 0;
            this.btnAddRegionGroup.ImgStretch = true;
            this.btnAddRegionGroup.IsImgStatusNormal = true;
            this.btnAddRegionGroup.Location = new System.Drawing.Point(110, 732);
            this.btnAddRegionGroup.Margin = new System.Windows.Forms.Padding(0);
            this.btnAddRegionGroup.Name = "btnAddRegionGroup";
            this.btnAddRegionGroup.Size = new System.Drawing.Size(90, 36);
            this.btnAddRegionGroup.TabIndex = 24;
            this.btnAddRegionGroup.Text = "그룹 추가";
            this.btnAddRegionGroup.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnAddRegionGroup.UseChecked = false;
            this.btnAddRegionGroup.Click += new System.EventHandler(this.btnAddRegionGroup_Click);
            // 
            // pnlRegionGroupList
            // 
            this.pnlRegionGroupList.Controls.Add(this.lblRegionGroupListTitle);
            this.pnlRegionGroupList.Controls.Add(this.lvRegionGroupList);
            this.pnlRegionGroupList.Location = new System.Drawing.Point(6, 476);
            this.pnlRegionGroupList.Margin = new System.Windows.Forms.Padding(0);
            this.pnlRegionGroupList.Name = "pnlRegionGroupList";
            this.pnlRegionGroupList.Size = new System.Drawing.Size(194, 249);
            this.pnlRegionGroupList.TabIndex = 23;
            // 
            // lblRegionGroupListTitle
            // 
            this.lblRegionGroupListTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRegionGroupListTitle.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRegionGroupListTitle.ForeColor = System.Drawing.Color.White;
            this.lblRegionGroupListTitle.Image = ((System.Drawing.Image)(resources.GetObject("lblRegionGroupListTitle.Image")));
            this.lblRegionGroupListTitle.Location = new System.Drawing.Point(0, 0);
            this.lblRegionGroupListTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblRegionGroupListTitle.Name = "lblRegionGroupListTitle";
            this.lblRegionGroupListTitle.Size = new System.Drawing.Size(194, 32);
            this.lblRegionGroupListTitle.TabIndex = 6;
            this.lblRegionGroupListTitle.Text = " 그룹 목록";
            this.lblRegionGroupListTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvRegionGroupList
            // 
            this.lvRegionGroupList.BackColor = System.Drawing.Color.White;
            this.lvRegionGroupList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvRegionGroupList.CheckBoxes = true;
            this.lvRegionGroupList.Font = new System.Drawing.Font("맑은 고딕", 10F);
            this.lvRegionGroupList.FullRowSelect = true;
            this.lvRegionGroupList.GridLines = true;
            this.lvRegionGroupList.HideSelection = false;
            this.lvRegionGroupList.Location = new System.Drawing.Point(0, 32);
            this.lvRegionGroupList.Margin = new System.Windows.Forms.Padding(0);
            this.lvRegionGroupList.MultiSelect = false;
            this.lvRegionGroupList.Name = "lvRegionGroupList";
            this.lvRegionGroupList.ShowItemToolTips = true;
            this.lvRegionGroupList.Size = new System.Drawing.Size(194, 217);
            this.lvRegionGroupList.TabIndex = 7;
            this.lvRegionGroupList.UseCompatibleStateImageBehavior = false;
            this.lvRegionGroupList.View = System.Windows.Forms.View.Details;
            this.lvRegionGroupList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lvRegionGroupList_ItemCheck);
            this.lvRegionGroupList.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvRegionGroupList_ItemChecked);
            this.lvRegionGroupList.DoubleClick += new System.EventHandler(this.lvRegionGroupList_DoubleClick);
            // 
            // pnlRegionList
            // 
            this.pnlRegionList.Controls.Add(this.btnRegionListRoll);
            this.pnlRegionList.Controls.Add(this.lblRegionListTitle);
            this.pnlRegionList.Controls.Add(this.tviewRegionList);
            this.pnlRegionList.Location = new System.Drawing.Point(6, 6);
            this.pnlRegionList.Margin = new System.Windows.Forms.Padding(0);
            this.pnlRegionList.Name = "pnlRegionList";
            this.pnlRegionList.Size = new System.Drawing.Size(194, 464);
            this.pnlRegionList.TabIndex = 22;
            // 
            // btnRegionListRoll
            // 
            this.btnRegionListRoll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegionListRoll.ChkValue = false;
            this.btnRegionListRoll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRegionListRoll.Image = ((System.Drawing.Image)(resources.GetObject("btnRegionListRoll.Image")));
            this.btnRegionListRoll.ImgDisable = null;
            this.btnRegionListRoll.ImgHover = null;
            this.btnRegionListRoll.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnRegionListRoll.ImgSelect")));
            this.btnRegionListRoll.ImgStatusEvent = null;
            this.btnRegionListRoll.ImgStatusNormal = null;
            this.btnRegionListRoll.ImgStatusOffsetX = 2;
            this.btnRegionListRoll.ImgStatusOffsetY = 0;
            this.btnRegionListRoll.ImgStretch = false;
            this.btnRegionListRoll.IsImgStatusNormal = true;
            this.btnRegionListRoll.Location = new System.Drawing.Point(170, 6);
            this.btnRegionListRoll.Name = "btnRegionListRoll";
            this.btnRegionListRoll.Size = new System.Drawing.Size(18, 18);
            this.btnRegionListRoll.TabIndex = 2;
            this.btnRegionListRoll.Tag = "Region";
            this.btnRegionListRoll.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnRegionListRoll.UseChecked = true;
            this.btnRegionListRoll.Visible = false;
            // 
            // lblRegionListTitle
            // 
            this.lblRegionListTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRegionListTitle.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblRegionListTitle.ForeColor = System.Drawing.Color.White;
            this.lblRegionListTitle.Image = ((System.Drawing.Image)(resources.GetObject("lblRegionListTitle.Image")));
            this.lblRegionListTitle.Location = new System.Drawing.Point(0, 0);
            this.lblRegionListTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblRegionListTitle.Name = "lblRegionListTitle";
            this.lblRegionListTitle.Size = new System.Drawing.Size(194, 32);
            this.lblRegionListTitle.TabIndex = 0;
            this.lblRegionListTitle.Text = " 행정동 목록";
            this.lblRegionListTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tviewRegionList
            // 
            this.tviewRegionList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tviewRegionList.CheckBoxes = true;
            this.tviewRegionList.Font = new System.Drawing.Font("맑은 고딕", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.tviewRegionList.Location = new System.Drawing.Point(0, 32);
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
            this.tviewRegionList.Size = new System.Drawing.Size(194, 432);
            this.tviewRegionList.TabIndex = 2;
            this.tviewRegionList.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.tviewRegionList_AfterCheck);
            this.tviewRegionList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tviewRegionList_AfterSelect);
            this.tviewRegionList.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tviewRegionList_NodeMouseClick);
            // 
            // btnClearRegionSelection
            // 
            this.btnClearRegionSelection.ChkValue = false;
            this.btnClearRegionSelection.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnClearRegionSelection.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClearRegionSelection.ForeColor = System.Drawing.Color.White;
            this.btnClearRegionSelection.Image = ((System.Drawing.Image)(resources.GetObject("btnClearRegionSelection.Image")));
            this.btnClearRegionSelection.ImgDisable = null;
            this.btnClearRegionSelection.ImgHover = null;
            this.btnClearRegionSelection.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnClearRegionSelection.ImgSelect")));
            this.btnClearRegionSelection.ImgStatusEvent = null;
            this.btnClearRegionSelection.ImgStatusNormal = null;
            this.btnClearRegionSelection.ImgStatusOffsetX = 2;
            this.btnClearRegionSelection.ImgStatusOffsetY = 0;
            this.btnClearRegionSelection.ImgStretch = false;
            this.btnClearRegionSelection.IsImgStatusNormal = true;
            this.btnClearRegionSelection.Location = new System.Drawing.Point(6, 732);
            this.btnClearRegionSelection.Margin = new System.Windows.Forms.Padding(0);
            this.btnClearRegionSelection.Name = "btnClearRegionSelection";
            this.btnClearRegionSelection.Size = new System.Drawing.Size(90, 36);
            this.btnClearRegionSelection.TabIndex = 3;
            this.btnClearRegionSelection.Text = "선택 취소";
            this.btnClearRegionSelection.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnClearRegionSelection.UseChecked = true;
            this.btnClearRegionSelection.Click += new System.EventHandler(this.btnClearTargetSelection_Click);
            // 
            // adengGoogleEarthCtrl
            // 
            this.adengGoogleEarthCtrl.BackColor = System.Drawing.Color.White;
            this.adengGoogleEarthCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.adengGoogleEarthCtrl.Location = new System.Drawing.Point(0, 0);
            this.adengGoogleEarthCtrl.Margin = new System.Windows.Forms.Padding(0);
            this.adengGoogleEarthCtrl.MinimumSize = new System.Drawing.Size(100, 100);
            this.adengGoogleEarthCtrl.Name = "adengGoogleEarthCtrl";
            this.adengGoogleEarthCtrl.Size = new System.Drawing.Size(1262, 868);
            this.adengGoogleEarthCtrl.TabIndex = 1;
            // 
            // bottomStatusStrip
            // 
            this.bottomStatusStrip.BackColor = System.Drawing.Color.Transparent;
            this.bottomStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelCoord,
            this.toolStripStatusLabelAuth,
            this.toolStripStatusLabelGWComm});
            this.bottomStatusStrip.Location = new System.Drawing.Point(0, 960);
            this.bottomStatusStrip.Name = "bottomStatusStrip";
            this.bottomStatusStrip.Size = new System.Drawing.Size(1264, 25);
            this.bottomStatusStrip.TabIndex = 0;
            this.bottomStatusStrip.Text = "하단 상태 표시";
            // 
            // toolStripStatusLabelCoord
            // 
            this.toolStripStatusLabelCoord.BackColor = System.Drawing.Color.Transparent;
            this.toolStripStatusLabelCoord.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripStatusLabelCoord.Name = "toolStripStatusLabelCoord";
            this.toolStripStatusLabelCoord.Size = new System.Drawing.Size(0, 0);
            this.toolStripStatusLabelCoord.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabelAuth
            // 
            this.toolStripStatusLabelAuth.BackColor = System.Drawing.Color.Transparent;
            this.toolStripStatusLabelAuth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.toolStripStatusLabelAuth.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.toolStripStatusLabelAuth.ForeColor = System.Drawing.Color.Red;
            this.toolStripStatusLabelAuth.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripStatusLabelAuth.Name = "toolStripStatusLabelAuth";
            this.toolStripStatusLabelAuth.Size = new System.Drawing.Size(123, 25);
            this.toolStripStatusLabelAuth.Spring = true;
            this.toolStripStatusLabelAuth.Text = "(인증 코드 승인 오류)";
            this.toolStripStatusLabelAuth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // toolStripStatusLabelGWComm
            // 
            this.toolStripStatusLabelGWComm.BackColor = System.Drawing.Color.Transparent;
            this.toolStripStatusLabelGWComm.Image = ((System.Drawing.Image)(resources.GetObject("toolStripStatusLabelGWComm.Image")));
            this.toolStripStatusLabelGWComm.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripStatusLabelGWComm.Name = "toolStripStatusLabelGWComm";
            this.toolStripStatusLabelGWComm.Size = new System.Drawing.Size(163, 20);
            this.toolStripStatusLabelGWComm.Text = "통합경보게이트웨이 통신";
            this.toolStripStatusLabelGWComm.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            // 
            // imgListCommunicationState
            // 
            this.imgListCommunicationState.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListCommunicationState.ImageStream")));
            this.imgListCommunicationState.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListCommunicationState.Images.SetKeyName(0, "IconListGreen.png");
            this.imgListCommunicationState.Images.SetKeyName(1, "IconListRed.png");
            this.imgListCommunicationState.Images.SetKeyName(2, "statusNormal.png");
            this.imgListCommunicationState.Images.SetKeyName(3, "statusAbnormal.png");
            // 
            // pnlButtonMenuBar
            // 
            this.pnlButtonMenuBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlButtonMenuBar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlButtonMenuBar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlButtonMenuBar.BackgroundImage")));
            this.pnlButtonMenuBar.Controls.Add(this.panel2);
            this.pnlButtonMenuBar.Controls.Add(this.panel3);
            this.pnlButtonMenuBar.Controls.Add(this.panel1);
            this.pnlButtonMenuBar.Controls.Add(this.btnMainMenuClearAlert);
            this.pnlButtonMenuBar.Controls.Add(this.btnMainMenuSWR);
            this.pnlButtonMenuBar.Controls.Add(this.btnTargetingBySystem);
            this.pnlButtonMenuBar.Controls.Add(this.btnTargetingByArea);
            this.pnlButtonMenuBar.Controls.Add(this.btnTargetingByRegion);
            this.pnlButtonMenuBar.Location = new System.Drawing.Point(0, 24);
            this.pnlButtonMenuBar.Margin = new System.Windows.Forms.Padding(0);
            this.pnlButtonMenuBar.Name = "pnlButtonMenuBar";
            this.pnlButtonMenuBar.Size = new System.Drawing.Size(1264, 66);
            this.pnlButtonMenuBar.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel2.BackgroundImage")));
            this.panel2.Location = new System.Drawing.Point(1254, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(6, 50);
            this.panel2.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel3.BackgroundImage")));
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel3.Controls.Add(this.lblLatestOrderSummary);
            this.panel3.Location = new System.Drawing.Point(611, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(643, 50);
            this.panel3.TabIndex = 3;
            // 
            // lblLatestOrderSummary
            // 
            this.lblLatestOrderSummary.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLatestOrderSummary.AutoEllipsis = true;
            this.lblLatestOrderSummary.BackColor = System.Drawing.Color.Transparent;
            this.lblLatestOrderSummary.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblLatestOrderSummary.Font = new System.Drawing.Font("맑은 고딕", 13.5F, System.Drawing.FontStyle.Bold);
            this.lblLatestOrderSummary.ForeColor = System.Drawing.Color.Red;
            this.lblLatestOrderSummary.Location = new System.Drawing.Point(0, 0);
            this.lblLatestOrderSummary.Name = "lblLatestOrderSummary";
            this.lblLatestOrderSummary.Size = new System.Drawing.Size(643, 50);
            this.lblLatestOrderSummary.TabIndex = 2;
            this.lblLatestOrderSummary.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLatestOrderSummary.Click += new System.EventHandler(this.lblLatestOrderSummary_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.Location = new System.Drawing.Point(605, 6);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(6, 50);
            this.panel1.TabIndex = 3;
            // 
            // btnMainMenuClearAlert
            // 
            this.btnMainMenuClearAlert.ChkValue = false;
            this.btnMainMenuClearAlert.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnMainMenuClearAlert.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMainMenuClearAlert.ForeColor = System.Drawing.Color.White;
            this.btnMainMenuClearAlert.Image = ((System.Drawing.Image)(resources.GetObject("btnMainMenuClearAlert.Image")));
            this.btnMainMenuClearAlert.ImgDisable = null;
            this.btnMainMenuClearAlert.ImgHover = null;
            this.btnMainMenuClearAlert.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnMainMenuClearAlert.ImgSelect")));
            this.btnMainMenuClearAlert.ImgStatusEvent = null;
            this.btnMainMenuClearAlert.ImgStatusNormal = null;
            this.btnMainMenuClearAlert.ImgStatusOffsetX = 2;
            this.btnMainMenuClearAlert.ImgStatusOffsetY = 0;
            this.btnMainMenuClearAlert.ImgStretch = false;
            this.btnMainMenuClearAlert.IsImgStatusNormal = true;
            this.btnMainMenuClearAlert.Location = new System.Drawing.Point(472, 14);
            this.btnMainMenuClearAlert.Margin = new System.Windows.Forms.Padding(0);
            this.btnMainMenuClearAlert.Name = "btnMainMenuClearAlert";
            this.btnMainMenuClearAlert.Size = new System.Drawing.Size(74, 38);
            this.btnMainMenuClearAlert.TabIndex = 1;
            this.btnMainMenuClearAlert.Text = "경보해제";
            this.btnMainMenuClearAlert.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnMainMenuClearAlert.UseChecked = true;
            this.btnMainMenuClearAlert.Click += new System.EventHandler(this.btnMainMenuClearAlert_Click);
            // 
            // btnMainMenuSWR
            // 
            this.btnMainMenuSWR.ChkValue = false;
            this.btnMainMenuSWR.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnMainMenuSWR.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMainMenuSWR.ForeColor = System.Drawing.Color.White;
            this.btnMainMenuSWR.Image = ((System.Drawing.Image)(resources.GetObject("btnMainMenuSWR.Image")));
            this.btnMainMenuSWR.ImgDisable = null;
            this.btnMainMenuSWR.ImgHover = null;
            this.btnMainMenuSWR.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnMainMenuSWR.ImgSelect")));
            this.btnMainMenuSWR.ImgStatusEvent = null;
            this.btnMainMenuSWR.ImgStatusNormal = null;
            this.btnMainMenuSWR.ImgStatusOffsetX = 2;
            this.btnMainMenuSWR.ImgStatusOffsetY = 0;
            this.btnMainMenuSWR.ImgStretch = false;
            this.btnMainMenuSWR.IsImgStatusNormal = true;
            this.btnMainMenuSWR.Location = new System.Drawing.Point(393, 14);
            this.btnMainMenuSWR.Margin = new System.Windows.Forms.Padding(0);
            this.btnMainMenuSWR.Name = "btnMainMenuSWR";
            this.btnMainMenuSWR.Size = new System.Drawing.Size(74, 38);
            this.btnMainMenuSWR.TabIndex = 1;
            this.btnMainMenuSWR.Text = "기상특보";
            this.btnMainMenuSWR.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnMainMenuSWR.UseChecked = true;
            this.btnMainMenuSWR.Click += new System.EventHandler(this.btnMainMenuSWR_Click);
            // 
            // btnTargetingBySystem
            // 
            this.btnTargetingBySystem.ChkValue = false;
            this.btnTargetingBySystem.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnTargetingBySystem.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnTargetingBySystem.ForeColor = System.Drawing.Color.White;
            this.btnTargetingBySystem.Image = ((System.Drawing.Image)(resources.GetObject("btnTargetingBySystem.Image")));
            this.btnTargetingBySystem.ImgDisable = null;
            this.btnTargetingBySystem.ImgHover = null;
            this.btnTargetingBySystem.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnTargetingBySystem.ImgSelect")));
            this.btnTargetingBySystem.ImgStatusEvent = null;
            this.btnTargetingBySystem.ImgStatusNormal = null;
            this.btnTargetingBySystem.ImgStatusOffsetX = 2;
            this.btnTargetingBySystem.ImgStatusOffsetY = 0;
            this.btnTargetingBySystem.ImgStretch = false;
            this.btnTargetingBySystem.IsImgStatusNormal = true;
            this.btnTargetingBySystem.Location = new System.Drawing.Point(206, 0);
            this.btnTargetingBySystem.Margin = new System.Windows.Forms.Padding(0);
            this.btnTargetingBySystem.MinimumSize = new System.Drawing.Size(103, 66);
            this.btnTargetingBySystem.Name = "btnTargetingBySystem";
            this.btnTargetingBySystem.Size = new System.Drawing.Size(103, 66);
            this.btnTargetingBySystem.TabIndex = 0;
            this.btnTargetingBySystem.Text = "시스템";
            this.btnTargetingBySystem.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnTargetingBySystem.UseChecked = true;
            this.btnTargetingBySystem.Click += new System.EventHandler(this.btnTargetingBySystem_Click);
            // 
            // btnTargetingByArea
            // 
            this.btnTargetingByArea.ChkValue = false;
            this.btnTargetingByArea.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnTargetingByArea.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnTargetingByArea.ForeColor = System.Drawing.Color.White;
            this.btnTargetingByArea.Image = ((System.Drawing.Image)(resources.GetObject("btnTargetingByArea.Image")));
            this.btnTargetingByArea.ImgDisable = null;
            this.btnTargetingByArea.ImgHover = null;
            this.btnTargetingByArea.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnTargetingByArea.ImgSelect")));
            this.btnTargetingByArea.ImgStatusEvent = null;
            this.btnTargetingByArea.ImgStatusNormal = null;
            this.btnTargetingByArea.ImgStatusOffsetX = 2;
            this.btnTargetingByArea.ImgStatusOffsetY = 0;
            this.btnTargetingByArea.ImgStretch = false;
            this.btnTargetingByArea.IsImgStatusNormal = true;
            this.btnTargetingByArea.Location = new System.Drawing.Point(103, 0);
            this.btnTargetingByArea.Margin = new System.Windows.Forms.Padding(0);
            this.btnTargetingByArea.MinimumSize = new System.Drawing.Size(103, 66);
            this.btnTargetingByArea.Name = "btnTargetingByArea";
            this.btnTargetingByArea.Size = new System.Drawing.Size(103, 66);
            this.btnTargetingByArea.TabIndex = 0;
            this.btnTargetingByArea.Text = "원점 기준 \r\n반경";
            this.btnTargetingByArea.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnTargetingByArea.UseChecked = true;
            this.btnTargetingByArea.Click += new System.EventHandler(this.btnTargetingByArea_Click);
            // 
            // btnTargetingByRegion
            // 
            this.btnTargetingByRegion.ChkValue = true;
            this.btnTargetingByRegion.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnTargetingByRegion.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnTargetingByRegion.ForeColor = System.Drawing.Color.White;
            this.btnTargetingByRegion.Image = ((System.Drawing.Image)(resources.GetObject("btnTargetingByRegion.Image")));
            this.btnTargetingByRegion.ImgDisable = null;
            this.btnTargetingByRegion.ImgHover = null;
            this.btnTargetingByRegion.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnTargetingByRegion.ImgSelect")));
            this.btnTargetingByRegion.ImgStatusEvent = null;
            this.btnTargetingByRegion.ImgStatusNormal = null;
            this.btnTargetingByRegion.ImgStatusOffsetX = 2;
            this.btnTargetingByRegion.ImgStatusOffsetY = 0;
            this.btnTargetingByRegion.ImgStretch = false;
            this.btnTargetingByRegion.IsImgStatusNormal = true;
            this.btnTargetingByRegion.Location = new System.Drawing.Point(0, 0);
            this.btnTargetingByRegion.Margin = new System.Windows.Forms.Padding(0);
            this.btnTargetingByRegion.MinimumSize = new System.Drawing.Size(103, 66);
            this.btnTargetingByRegion.Name = "btnTargetingByRegion";
            this.btnTargetingByRegion.Size = new System.Drawing.Size(103, 66);
            this.btnTargetingByRegion.TabIndex = 0;
            this.btnTargetingByRegion.Tag = "";
            this.btnTargetingByRegion.Text = "행정 구역";
            this.btnTargetingByRegion.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnTargetingByRegion.UseChecked = true;
            this.btnTargetingByRegion.Click += new System.EventHandler(this.btnTargetingByRegion_Click);
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentTime.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblCurrentTime.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCurrentTime.Location = new System.Drawing.Point(1050, 0);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.lblCurrentTime.Size = new System.Drawing.Size(214, 24);
            this.lblCurrentTime.TabIndex = 4;
            this.lblCurrentTime.Text = "2015-07-01 오후 3:41";
            this.lblCurrentTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // 언어설정ToolStripMenuItem
            // 
            this.언어설정ToolStripMenuItem.Name = "언어설정ToolStripMenuItem";
            this.언어설정ToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.언어설정ToolStripMenuItem.Text = "언어 설정(&L)";
            this.언어설정ToolStripMenuItem.Click += new System.EventHandler(this.언어설정ToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1264, 985);
            this.Controls.Add(this.lblCurrentTime);
            this.Controls.Add(this.topFileMenuStrip);
            this.Controls.Add(this.pnlButtonMenuBar);
            this.Controls.Add(this.pnlMainCenter);
            this.Controls.Add(this.bottomStatusStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.topFileMenuStrip;
            this.MinimumSize = new System.Drawing.Size(1280, 1024);
            this.Name = "MainForm";
            this.Text = "통합경보시스템 표준발령대";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.MdiChildActivate += new System.EventHandler(this.MainForm_MdiChildActivate);
            this.topFileMenuStrip.ResumeLayout(false);
            this.topFileMenuStrip.PerformLayout();
            this.pnlMainCenter.ResumeLayout(false);
            this.pnlOuterLineVisibleCtrl.ResumeLayout(false);
            this.pnlOuterLineVisibleTitle.ResumeLayout(false);
            this.pnlTargetSystemBody.ResumeLayout(false);
            this.pnlSystemGroupList.ResumeLayout(false);
            this.pnlSystemList.ResumeLayout(false);
            this.pnlTargetAreaBody.ResumeLayout(false);
            this.pnlLoading.ResumeLayout(false);
            this.pnlLoading.PerformLayout();
            this.pnlLoadingBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoading)).EndInit();
            this.pnlMouseModeMain.ResumeLayout(false);
            this.pnlMouseModeMain.PerformLayout();
            this.pnlUseDrawModeBg.ResumeLayout(false);
            this.pnlJumpToRegion.ResumeLayout(false);
            this.pnlJumpToRegion.PerformLayout();
            this.pnlJumpToRegionTitle.ResumeLayout(false);
            this.pnlTargetListHeader.ResumeLayout(false);
            this.pnlTargetRegionBody.ResumeLayout(false);
            this.pnlRegionGroupList.ResumeLayout(false);
            this.pnlRegionList.ResumeLayout(false);
            this.bottomStatusStrip.ResumeLayout(false);
            this.bottomStatusStrip.PerformLayout();
            this.pnlButtonMenuBar.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip topFileMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 프로그램PToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 종료XToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 이력조회HToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 발령이력OToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 프로그램사용이력PToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 설정SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 사용자관리UToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 기능옵션OToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 정보IToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 표준발령대정보AToolStripMenuItem;
        private System.Windows.Forms.Panel pnlButtonMenuBar;
        private System.Windows.Forms.Panel pnlMainCenter;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnTargetingByRegion;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnTargetingByArea;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnTargetingBySystem;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnMainMenuSWR;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnMainMenuClearAlert;
        private AdengGE.AdengGoogleEarthCtrl adengGoogleEarthCtrl;
        private System.Windows.Forms.StatusStrip bottomStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelGWComm;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCoord;
        private System.Windows.Forms.Label lblLatestOrderSummary;
        private Adeng.Framework.Ctrl.TreeViewEx tviewRegionList;
        private System.Windows.Forms.Panel pnlTargetRegionBody;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnClearRegionSelection;
        private System.Windows.Forms.Panel pnlTargetListHeader;
        private System.Windows.Forms.Label lblTargetListHeaderTitle;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnTargetListRoll;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelAuth;
        private System.Windows.Forms.ImageList imgListCommunicationState;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private Adeng.Framework.Ctrl.AdengListView lvSelectedAreaList;
        private System.Windows.Forms.Panel pnlJumpToRegion;
        private System.Windows.Forms.ComboBox cmbboxJumpToRegionLevel2;
        private System.Windows.Forms.ComboBox cmbboxJumpToRegionLevel1;
        private System.Windows.Forms.Label lblJumpingRegionLevel2;
        private System.Windows.Forms.Label lblJumpingRegionLevel1;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnJumpToRegionListRoll;
        private System.Windows.Forms.Panel pnlJumpToRegionTitle;
        private System.Windows.Forms.Label lblJumpToRegionTitle;
        private System.Windows.Forms.ListView lvStdAlertSystemList;
        private System.Windows.Forms.Panel pnlMouseModeMain;
        private System.Windows.Forms.Panel pnlDivisionLine;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnUseCircleMode;
        private System.Windows.Forms.Label lblCircleRadius;
        private System.Windows.Forms.Label lblCircleCoordinator;
        private System.Windows.Forms.Panel pnlUseDrawModeBg;
        private System.Windows.Forms.TextBox txtboxLongitude;
        private System.Windows.Forms.TextBox txtboxRadius;
        private System.Windows.Forms.TextBox txtboxLatitude;
        private System.Windows.Forms.Label lblRadiusUnitMeter;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnApplyDrawCircle;
        private System.Windows.Forms.Panel pnlLoading;
        private System.Windows.Forms.Panel pnlLoadingBottom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBoxLoading;
        private System.Windows.Forms.Panel pnlRegionList;
        private System.Windows.Forms.Label lblRegionListTitle;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnRegionListRoll;
        private System.Windows.Forms.Panel pnlTargetSystemBody;
        private System.Windows.Forms.Panel pnlSystemList;
        private System.Windows.Forms.Label lblSystemGroupListTitle;
        private StdWarningInstallation.Ctrl.MyListView lvSystemGroupList;
        private System.Windows.Forms.Label lblSystemListTitle;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnClearSystemSelection;
        private System.Windows.Forms.Panel pnlTargetAreaBody;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnClearAreaSelection;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnAddSystemGroup;
        private System.Windows.Forms.Panel pnlRegionGroupList;
        private System.Windows.Forms.Label lblRegionGroupListTitle;
        private StdWarningInstallation.Ctrl.MyListView lvRegionGroupList;
        private System.Windows.Forms.Panel pnlSystemGroupList;
        private System.Windows.Forms.Label lblMouseModeTitle;
        private System.Windows.Forms.Label lblUseCircleModeTitle;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnAddRegionGroup;
        private System.Windows.Forms.ToolStripMenuItem 시스템설정관리SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 기본문안관리ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 기상특보연계설정ToolStripMenuItem;
        private System.Windows.Forms.Panel pnlOuterLineVisibleCtrl;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnOuterLineHigh;
        private System.Windows.Forms.Panel pnlOuterLineVisibleTitle;
        private System.Windows.Forms.Label label3;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnOuterLineLow;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnOuterLineMiddle;
        private System.Windows.Forms.Label lblCurrentTime;
        private System.Windows.Forms.ToolStripMenuItem 언어설정ToolStripMenuItem;
    }
}

