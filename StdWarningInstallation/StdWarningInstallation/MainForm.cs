using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

using Adeng.Framework;
using Adeng.Framework.Ctrl;
using Adeng.Framework.Util;
using AdengGE;
using CAPLib;
using GEPlugin;
using StdWarningInstallation.DataClass;

namespace StdWarningInstallation
{
    public partial class MainForm : Form
    {
        #region 개발 버전 히스토리
        // 버전 관리는 프로젝트 프러퍼티 버전 관리 페이지에서 하자. 추후 수정 요망
        private readonly string PROGRAM_NAME = "통합경보시스템 표준발령대(SWI)";
        private readonly string VERSION_INFO = "Version 3.0.1 (2016.04.01)"; // SVN 이력 참고.
        #endregion
        
        #region DefineENUM
        enum TargetingMode { None, Region, Radius, System, };
        enum MouseMode { Normal, Region, DrawCircle, System };
        enum GEMouseButton { Left = 0, Center, Right };
        enum OuterLineVisibleMode { All, High, Middle, Low };
        #endregion

        #region field
        private Core core = null;

        private bool isGISLoadingContinue = true;
        private bool isGISLoadingCompleted = false;
        private GEController externalGE = null;

        private LoginForm loginForm = null;
        private bool loginCompleted = false;

        private bool isContinueDisplayTime = true;
        private Thread timeThread = null;

        RecentlyOrderHistoryForm recentlyOrderForm = null;

        private TargetingMode targetingMode = TargetingMode.Region;     // 발령 대상 선택 모드 (행정동지역/ 좌표기준반경/ 시스템개별/ 그룹)
        private MouseMode currentMouseMode = MouseMode.Normal;
        private OuterLineVisibleMode currentOuterLineVisibleMode = OuterLineVisibleMode.Middle;
        private Point clickStartPosition = new Point(0, 0);

        private bool isDatabaseLoaded = false;
        private bool isUseSWRAssociation = false;
        private bool isConnectedWithIAGW = false;
        private bool isAuthenticated = false;

        private OrderForm orderWindow = null;
        private OrderProvisionInfo currentOrderProvisionInfo = new OrderProvisionInfo();

        private WaitToOrderSWRForm waitToOrderSwrForm = null;

        private string latestOrderHeadline = string.Empty;
        private ManualResetEvent headlineManualEvent = new ManualResetEvent(false);

        private Dictionary<string, SASInfo> currentDicSystemInfo = null;
        private Dictionary<string, SWRProfile> currentSWRInfo = new Dictionary<string, SWRProfile>();

        private List<IKmlPlacemark> currentSelectedPlacemark = new List<IKmlPlacemark>();
        #endregion

        public MainForm()
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] 프로그램 기동 시작");

            InitializeComponent();

            this.Init();
        }

        #region 메인 폼 처리
        /// <summary>
        /// 초기 처리
        /// </summary>
        public void Init()
        {
            try
            {
                FileLogManager.GetInstance().WriteLog("[MainForm] 프로그램 초기 처리 시작");

                this.Text = this.PROGRAM_NAME + " " + this.VERSION_INFO;

                // 시각 표시 쓰레드 시작
                StartDisplayTime();


                // 로딩 화면 쓰레드


                this.core = new Core();
                this.core.NotifyIAGWConnectionState += new EventHandler<IAGWConnectionEventArgs>(core_OnNotifyIAGWConnectionState);
                this.core.NotifySASProfileUpdated += new EventHandler<SASProfileUpdateEventArgs>(core_OnNotifySASProfileUpdated);
                this.core.NotifyLatestOrderInfoChanged += new EventHandler<OrderEventArgs>(core_OnNotifyLatestOrderInfoChanged);
                this.core.NotifySWRProfileUpdated += new EventHandler<EventArgs>(core_OnNotifySWRProfileUpdated);

                // 컨트롤 초기 처리
                this.externalGE = new GEController();
                this.adengGoogleEarthCtrl.evtGoogleEventInitEarthSuccess += new GoogleEventInitEarthSuccess(AdengGoogleEarthCtrlEvtGoogleEventInitEarthSuccess);
                this.adengGoogleEarthCtrl.evtGoogleEventInitEarthFailure += new GoogleEventInitEarthFailure(AdengGoogleEarthCtrlEvtGoogleEventInitEarthFailure);
                this.adengGoogleEarthCtrl.evtGoogleEventMouseDown += new GoogleEventMouseDown(AdengGoogleEarthCtrlEvtGoogleEventMouseDown);
                this.adengGoogleEarthCtrl.evtGoogleEventMouseMove += new GoogleEventMouseMove(AdengGoogleEarthCtrlEvtGoogleEventMouseMove);
                this.adengGoogleEarthCtrl.evtGoogleEventMouseUp += new GoogleEventMouseUp(AdengGoogleEarthCtrlEvtGoogleEventMouseUp);
                this.adengGoogleEarthCtrl.evtGoogleDrawCircle += new GoogleEventDrawCircle(AdengGoogleEarthCtrl_evtGoogleDrawCircle);

                // 표준경보시스템 데이터 취득
                this.core.GetSASInfo(out this.currentDicSystemInfo);

                // 기상특보 연계여부설정
                ConfigData config = this.core.GetConfigInfo();
                if (config != null)
                {
                    this.isUseSWRAssociation = config.SWR.UseService;
                }

                // 발령 대상 리스트 및 빠른 지역 이동 컨트롤의 초기 데이터 설정
                InitializeRegionNameList();
                InitializeRegionGroupList();
                InitializeSelectedAreaList();
                InitializeSystemList();
                InitializeSystemGroupList();
                InitializeJumpToRegionCmbbox();
                InitializeOuterBoundaryStepCtrl();

                // 일시적으로 경보해제 버튼을 고정 유효 처리
                this.btnMainMenuClearAlert.ChkValue = true;

                FileLogManager.GetInstance().WriteLog("[MainForm] 프로그램 초기 처리 종료");
            }
            catch (Exception ex)
            {
                FileLogManager.GetInstance().WriteLog("[MainForm] Init( [ERROR] 시스템 기동 중에 예외가 발생하여 시스템을 로딩할 수 없습니다. \nException=[" + ex.ToString() + "] )");

                MessageBox.Show("[ERROR] 시스템 기동 중에 오류가 발생하여 시스템을 로딩할 수 없습니다.", "기동 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// 종료 메소드
        /// </summary>
        public void UnInit()
        {
            if (this.loginForm != null)
            {
                this.loginForm.NotifyLoginResult -= new EventHandler<LoginEventArgs>(loginForm_OnNotifyLoginResult);
            }

            if (this.core != null)
            {
                this.core.NotifyIAGWConnectionState -= new EventHandler<IAGWConnectionEventArgs>(core_OnNotifyIAGWConnectionState);
                this.core.NotifySASProfileUpdated -= new EventHandler<SASProfileUpdateEventArgs>(core_OnNotifySASProfileUpdated);
                this.core.NotifyLatestOrderInfoChanged -= new EventHandler<OrderEventArgs>(core_OnNotifyLatestOrderInfoChanged);
                this.core.NotifySWRProfileUpdated -= new EventHandler<EventArgs>(core_OnNotifySWRProfileUpdated);
            }

            if (this.adengGoogleEarthCtrl != null)
            {
                this.adengGoogleEarthCtrl.evtGoogleEventInitEarthSuccess -= new GoogleEventInitEarthSuccess(AdengGoogleEarthCtrlEvtGoogleEventInitEarthSuccess);
                this.adengGoogleEarthCtrl.evtGoogleEventInitEarthFailure -= new GoogleEventInitEarthFailure(AdengGoogleEarthCtrlEvtGoogleEventInitEarthFailure);
                this.adengGoogleEarthCtrl.evtGoogleEventMouseDown -= new GoogleEventMouseDown(AdengGoogleEarthCtrlEvtGoogleEventMouseDown);
                this.adengGoogleEarthCtrl.evtGoogleEventMouseMove -= new GoogleEventMouseMove(AdengGoogleEarthCtrlEvtGoogleEventMouseMove);
                this.adengGoogleEarthCtrl.evtGoogleEventMouseUp -= new GoogleEventMouseUp(AdengGoogleEarthCtrlEvtGoogleEventMouseUp);
                this.adengGoogleEarthCtrl.evtGoogleDrawCircle -= new GoogleEventDrawCircle(AdengGoogleEarthCtrl_evtGoogleDrawCircle);

                this.adengGoogleEarthCtrl.TerminateGEPlugin();
            }

            // 시각 표시 쓰레드 종료
            EndDisplayTime();
        }

        /// <summary>
        /// 종료 확인
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("프로그램을 종료하겠습니까?", this.PROGRAM_NAME + " " + this.VERSION_INFO, 
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2))
            {
                EventLogManager.GetInstance().WriteLog("프로그램 종료");
                this.UnInit();
            }
            else
            {
                e.Cancel = true;
            }

            base.OnClosing(e);
        }

        /// <summary>
        /// 로딩
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string thisName = this.Text;

            if (AdengUtil.CheckAppOverlap("StdWarningInstallation"))
            {
                MessageBox.Show("프로그램이 이미 실행 중 입니다.", "Standard Warning Installation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose(true);
            }

            bool isDBError = false;
            int result = this.core.StartUp();
            if (-1 == result)
            {
                // [DB 연결 실패]
                // DB연결이 실패해도 어플리케이션은 로딩. 그러므로 디폴트 로그인 유저 정보를 설정해 준다.
                MessageBox.Show("DB 연결에 문제가 발생했습니다\nDB 점검 또는 메뉴 '도구 - 옵션'의 DB 항목을 다시 설정하세요\n"
                    + "접속 테스트 결과 정상이면 프로그램 재시작을 진행하세요", "통합경보시스템 표준발령대(SWI)", MessageBoxButtons.OK, MessageBoxIcon.Error);

                isDBError = true;
            }
            else
            {
                List<UserAccount> userAccountList = this.core.GetUserAccountInfo();
                this.loginForm = new LoginForm(userAccountList);
                this.loginForm.NotifyLoginResult += new EventHandler<LoginEventArgs>(loginForm_OnNotifyLoginResult);
                DialogResult loginResult = this.loginForm.ShowDialog();
                this.loginForm.NotifyLoginResult -= new EventHandler<LoginEventArgs>(loginForm_OnNotifyLoginResult);
                if (DialogResult.OK != loginResult)
                {
                    EventLogManager.GetInstance().WriteLog("로그인 실패 - 프로그램 종료");
                    this.Dispose(true);
                    return;
                }

                this.loginCompleted = true;
            }

            // 행정동 경계선 표시 모드 설정
            if (isDBError)
            {
                this.pnlOuterLineVisibleCtrl.Visible = false;
            }
            else
            {
                this.pnlOuterLineVisibleCtrl.Visible = true;
                this.btnOuterLineHigh.ChkValue = (this.currentOuterLineVisibleMode == OuterLineVisibleMode.High);
                this.btnOuterLineMiddle.ChkValue = (this.currentOuterLineVisibleMode == OuterLineVisibleMode.Middle);
                this.btnOuterLineLow.ChkValue = (this.currentOuterLineVisibleMode == OuterLineVisibleMode.Low);
            }

            // 발령 대상 선택 모드 초기 설정(행정동 선택)
            ChangeTargetSelectionMode(TargetingMode.None, TargetingMode.Region);


            // 기상특보
            UpdateSWRList();
            // 경보해제
            UpdateClearAlertInfo();


            // 헤드라인 정보
            this.lblLatestOrderSummary.Text = "";



            EventLogManager.GetInstance().WriteLog("프로그램 시작 /" + BasisData.CurrentLoginUser.IDAndName + " 로그인");
        }
        #endregion

        #region 각종 데이터 초기 설정
        /// <summary>
        /// 행정동 지역명 목록의 초기화 (데이터 설정)
        /// </summary>
        private void InitializeRegionNameList()
        {
            this.tviewRegionList.Nodes.Clear();

            if (!BasisData.IsRegionDataLoaded())
            {
                System.Console.WriteLine("[MainForm] 지역목록 초기화 실패 - BasisData 가 로딩되지 않음");

                this.pnlOuterLineVisibleCtrl.Visible = false;
                return;
            }
            Dictionary<string, RegionProfile> regionList = BasisData.Regions.LstRegion;

            foreach (RegionProfile region1 in regionList.Values)
            {
                // [시도]
                TreeNode level1 = this.tviewRegionList.Nodes.Add(region1.Code, region1.Name);
                level1.Tag = region1;

                if (region1.LstSubRegion == null || region1.LstSubRegion.Count < 1)
                {
                    continue;
                }
                foreach (RegionProfile region2 in region1.LstSubRegion.Values)
                {
                    // [시군구]
                    TreeNode level2 = level1.Nodes.Add(region2.Code, region2.Name);
                    level2.Tag = region2;

                    if (region2.LstSubRegion == null || region2.LstSubRegion.Count < 1)
                    {
                        continue;
                    }
                    foreach (RegionProfile region3 in region2.LstSubRegion.Values)
                    {
                        // [읍면동]
                        TreeNode level3 = level2.Nodes.Add(region3.Code, region3.Name);
                        level3.Tag = region3;
                    }
                }
            }

            int nodeCnt = tviewRegionList.GetNodeCount(false);
            for (int i = 0; i < nodeCnt; i++)
            {
                tviewRegionList.Nodes[i].Expand();
            }
        }
        /// <summary>
        /// 등록된 지역 그룹 목록 리스트뷰 초기화
        /// </summary>
        private void InitializeRegionGroupList()
        {
            if (this.lvRegionGroupList.Columns != null)
            {
                this.lvRegionGroupList.Columns.Clear();
            }

            ColumnHeader header = new ColumnHeader();
            header.Text = "";
            header.Width = 20;
            this.lvRegionGroupList.Columns.Add(header);
            header = new ColumnHeader();
            header.Text = "그룹명";
            header.Width = 150;
            this.lvRegionGroupList.Columns.Add(header);
            header = new ColumnHeader();
            header.Text = "재난 종류";
            header.Width = 150;
            this.lvRegionGroupList.Columns.Add(header);

            UpdateRegionGroupList();
        }
        private void UpdateRegionGroupList()
        {
            if (this.lvRegionGroupList.Columns == null || this.lvRegionGroupList.Columns.Count <= 0)
            {
                System.Console.WriteLine("[MainForm] UpdateRegionGroupList( 컬럼 정보가 초기화 되지 않았습니다. )");
                return;
            }
            this.lvRegionGroupList.Items.Clear();

            List<GroupProfile> grbList = this.core.QueryRegionGroupInfo();
            if (grbList == null)
            {
                return;
            }

            foreach (GroupProfile grb in grbList)
            {
                ListViewItem newItem = new ListViewItem();
                newItem.Name = grb.GroupID;

                newItem.SubItems.Add(grb.Name);
                if (!string.IsNullOrEmpty(grb.DisasterKindCode))
                {
                    DisasterKind disasterKindInfo = BasisData.FindDisasterKindByCode(grb.DisasterKindCode);
                    if (disasterKindInfo != null)
                    {
                        newItem.SubItems.Add(disasterKindInfo.Name);
                    }
                }
                newItem.Tag = grb;

                this.lvRegionGroupList.Items.Add(newItem);
            }
        }
        /// <summary>
        /// [좌표 기준 반경 선택] 선택된 지역 목록의 초기화
        /// </summary>
        private void InitializeSelectedAreaList()
        {
            this.lvSelectedAreaList.Location = new System.Drawing.Point(6, 6);
            this.lvSelectedAreaList.Size = new System.Drawing.Size(196, 620);

            this.lvSelectedAreaList.Columns.Clear();
            AdengColumnHeader header = new AdengColumnHeader();
            header.Text = "";
            header.Width = 30;
            this.lvSelectedAreaList.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Text = "지역";
            header.Width = 200;
            this.lvSelectedAreaList.Columns.Add(header);
            //header = new AdengColumnHeader();
            //header.Text = "읍면동";
            //header.Width = 100;
            //this.lvSelectedAreaList.Columns.Add(header);

            this.lvSelectedAreaList.Items.Clear();
        }
        /// <summary>
        /// 선택된 시스템 목록 리스트뷰 초기화
        /// </summary>
        private void InitializeSystemList()
        {
            this.lvStdAlertSystemList.Columns.Clear();
            this.lvStdAlertSystemList.Items.Clear();

            ColumnHeader header = new ColumnHeader();
            header.Name = "SystemName";
            header.Text = "명칭";
            header.Width = 150;
            this.lvStdAlertSystemList.Columns.Add(header);
            header = new ColumnHeader();
            header.Name = "IP";
            header.Text = "IP";
            header.Width = 150;
            this.lvStdAlertSystemList.Columns.Add(header);
            header = new ColumnHeader();
            header.Name = "SystemType";
            header.Text = "종류";
            header.Width = 150;
            this.lvStdAlertSystemList.Columns.Add(header);

            UpdateSystemList(false);
        }
        private void UpdateSystemList(bool updateData = true)
        {
            if (this.lvStdAlertSystemList.Columns == null || this.lvStdAlertSystemList.Columns.Count <= 0)
            {
                return;
            }

            this.lvStdAlertSystemList.Items.Clear();

            if (this.currentDicSystemInfo == null || updateData)
            {
                int result = this.core.GetSASInfo(out this.currentDicSystemInfo);
                if (result != 0 || this.currentDicSystemInfo == null)
                {
                    return;
                }
            }
            foreach (SASInfo systemInfo in this.currentDicSystemInfo.Values)
            {
                if (systemInfo.Profile == null)
                {
                    continue;
                }
                ListViewItem item = this.lvStdAlertSystemList.Items.Add(systemInfo.Profile.Name);
                item.SubItems.Add(systemInfo.Profile.IpAddress);
                item.SubItems.Add(systemInfo.Profile.KindCode);
                item.Name = systemInfo.Profile.ID;
                item.Tag = systemInfo.Profile;
            }
        }
        /// <summary>
        /// 등록된 그룹 목록 리스트뷰 초기화
        /// </summary>
        private void InitializeSystemGroupList()
        {
            if (this.lvSystemGroupList.Columns != null)
            {
                this.lvSystemGroupList.Columns.Clear();
            }

            ColumnHeader header = new ColumnHeader();
            header.Text = "";
            header.Width = 20;
            this.lvSystemGroupList.Columns.Add(header);
            header = new ColumnHeader();
            header.Text = "그룹명";
            header.Width = 150;
            this.lvSystemGroupList.Columns.Add(header);
            header = new ColumnHeader();
            header.Text = "재난 종류";
            header.Width = 150;
            this.lvSystemGroupList.Columns.Add(header);

            UpdateSystemGroupList();
        }
        private void UpdateSystemGroupList()
        {
            if (this.lvSystemGroupList.Columns == null || this.lvSystemGroupList.Columns.Count <= 0)
            {
                System.Console.WriteLine("[MainForm] UpdateSystemGroupList( 컬럼 정보가 초기화 되지 않았습니다. )");
                return;
            }
            this.lvSystemGroupList.Items.Clear();

            List<GroupProfile> grbList = this.core.QuerySystemGroupInfo();
            if (grbList == null)
            {
                return;
            }

            foreach (GroupProfile grb in grbList)
            {
                if (grb == null)
                {
                    continue;
                }

                ListViewItem newItem = new ListViewItem();
                newItem.Name = grb.GroupID;
                newItem.Text = "";
                newItem.SubItems.Add(grb.Name);

                if (!string.IsNullOrEmpty(grb.DisasterKindCode))
                {
                    DisasterKind disasterKindInfo = BasisData.FindDisasterKindByCode(grb.DisasterKindCode);
                    if (disasterKindInfo != null)
                    {
                        newItem.SubItems.Add(disasterKindInfo.Name);
                    }
                }
                newItem.Tag = grb;

                this.lvSystemGroupList.Items.Add(newItem);
            }
        }

        /// <summary>
        /// 빠른 지역 이동 컴보 박스의 아이템 설정
        /// </summary>
        private void InitializeJumpToRegionCmbbox()
        {
            this.cmbboxJumpToRegionLevel1.Items.Clear();
            this.cmbboxJumpToRegionLevel2.Items.Clear();

            foreach (RegionProfile region1 in BasisData.Regions.LstRegion.Values)
            {
                this.cmbboxJumpToRegionLevel1.Items.Add(region1);

                // 시도 또는 시군구
                foreach (RegionProfile region2 in region1.LstSubRegion.Values)
                {
                    // 시군구 또는 읍면동
                    int index = this.cmbboxJumpToRegionLevel1.Items.Add(region2);
                }
            }
        }

        /// <summary>
        /// 지역 경계선 표시 수준 표시 설정
        /// </summary>
        public void InitializeOuterBoundaryStepCtrl()
        {
            if (BasisData.TopRegion.Code.Level1 == "00")
            {
                // 중앙
                this.btnOuterLineHigh.Text = "전국";
                this.btnOuterLineMiddle.Text = "시도";
                this.btnOuterLineLow.Text = "시군구";

                this.btnOuterLineHigh.Enabled = true;
                this.btnOuterLineMiddle.Enabled = true;
                this.btnOuterLineLow.Enabled = true;
            }
            else if (BasisData.TopRegion.Code.Level2 == "000")
            {
                    this.btnOuterLineHigh.Text = "시도";
                    this.btnOuterLineMiddle.Text = "시군구";
                    this.btnOuterLineLow.Text = "읍면동";

                    this.btnOuterLineHigh.Enabled = true;
                    this.btnOuterLineMiddle.Enabled = true;
                    this.btnOuterLineLow.Enabled = true;
            }
            else if (BasisData.TopRegion.Code.Level3 == "00000")
            {
                this.btnOuterLineHigh.Text = "시군구";
                this.btnOuterLineMiddle.Text = "읍면동";
                this.btnOuterLineLow.Text = "";

                this.btnOuterLineHigh.Enabled = true;
                this.btnOuterLineMiddle.Enabled = true;
                this.btnOuterLineLow.Enabled = false;
            }
            else
            {
                this.btnOuterLineHigh.Text = "";
                this.btnOuterLineMiddle.Text = "";
                this.btnOuterLineLow.Text = "";

                this.btnOuterLineHigh.Enabled = false;
                this.btnOuterLineMiddle.Enabled = false;
                this.btnOuterLineLow.Enabled = false;
            }
        }
        #endregion


        #region 구글어스 이벤트 핸들러
        /// <summary>
        /// GEPlugin 초기화 성공 Event
        /// </summary>
        /// <param name="geObject"></param>
        private void AdengGoogleEarthCtrlEvtGoogleEventInitEarthSuccess(IGEPlugin geObject)
        {
            System.Console.WriteLine("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventInitEarthSuccess");
            FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventInitEarthSuccess( 로딩 성공 )");

            try
            {
                this.isGISLoadingContinue = false;
                this.pnlLoading.Visible = false;
                this.isGISLoadingCompleted = true;

                this.externalGE.Ge = this.adengGoogleEarthCtrl.IGePlugin;
                this.externalGE.SetBordersVisibility(false);
                this.externalGE.MakeAllStyleMap();

                LoadPlaceMarkData();

                // 지도 초기 위치
                if (BasisData.IsRegionDataLoaded())
                {
                    KeyValuePair<string, RegionProfile> first = BasisData.Regions.LstRegion.First();
                    JumpToSpecifiedRegion(first.Value);
                }
                else
                {
                    double Latitude = 37.82322654551308;
                    double Longitude = 128.2739862268925;
                    double Altitude = 350313;
                    double tilt = 0.0;
                    double range = 350313;

                    this.externalGE.MoveFlyTo(Latitude, Longitude, Altitude, tilt, range, this.externalGE.Ge.SPEED_TELEPORT);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventInitEarthSuccess( Exception : " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventInitEarthSuccess( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// GEPlugin 초기화 실패 Event
        /// </summary>
        /// <param name="err"></param>
        private void AdengGoogleEarthCtrlEvtGoogleEventInitEarthFailure(string err)
        {
            System.Console.WriteLine("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventInitEarthFailure");
            FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventInitEarthFailure( 로딩 실패 )");

            MessageBox.Show("구글 어스 연결을 실패하였습니다.\r\n" + err, "구글 어스 연결 실패", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        /// <summary>
        /// MouseDown Event
        /// </summary>
        /// <param name="geObject"></param>
        private void AdengGoogleEarthCtrlEvtGoogleEventMouseDown(IKmlMouseEvent geObject)
        {
            System.Console.WriteLine("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventMouseDown");
            //FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventMouseDown( Start )");

            try
            {
                if (geObject == null)
                {
                    FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventMouseDown( geObject is null )");
                    return;
                }

                this.clickStartPosition.X = geObject.getClientX();
                this.clickStartPosition.Y = geObject.getClientY();

                //FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventMouseDown( Click StartPoint=[" + clickStartPosition.X + ", " + clickStartPosition.Y + "] )");

                if (this.targetingMode == TargetingMode.Region && geObject.getButton() == 0)
                {
                    // MouseUp 에서 처리
                }
                else if (this.targetingMode == TargetingMode.Radius && geObject.getButton() == 0)
                {
                    double latitude = geObject.getLatitude();
                    double longitude = geObject.getLongitude();

                    string strLat = latitude.ToString();
                    int divinLat = strLat.IndexOf('.');
                    string strRadLatitude = strLat.Substring(0, divinLat + 7);  // 소수점 이하 6자리 까지

                    string strLon = longitude.ToString();
                    int divinLon = strLon.IndexOf('.');
                    string strRadLongitude = strLon.Substring(0, divinLon + 7);

                    this.txtboxLatitude.Text = strRadLatitude;
                    this.txtboxLongitude.Text = strRadLongitude;
                }
                else if (this.targetingMode == TargetingMode.System)
                {
                    IKmlPlacemark placeMark = geObject.getTarget() as IKmlPlacemark;
                    if (placeMark != null)
                    {
                        string id = placeMark.getId();
                        if (id != null && id.Contains("icon"))
                        {
                            ClickedPlacemarkIcon(placeMark);
                        }
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventMouseDown(" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventMouseDown( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// MouseMove Event
        /// </summary>
        /// <param name="geObject"></param>
        private void AdengGoogleEarthCtrlEvtGoogleEventMouseMove(IKmlMouseEvent geObject)
        {
            try
            {
                if ((this.targetingMode == TargetingMode.Radius) ||
                    (this.targetingMode == TargetingMode.Radius && geObject.getButton() != 0))
                {
                    double latitude = geObject.getLatitude();
                    double longitude = geObject.getLongitude();

                    // [DMS] 위/경도
                    string strLatitude = Adeng.Framework.Gis.GisConvert.D2DmsWithFormat(latitude, true);
                    string strLongitude = Adeng.Framework.Gis.GisConvert.D2DmsWithFormat(longitude, false);

                    // [RAD] 위/경도
                    string strLat = latitude.ToString();
                    int divinLat = strLat.IndexOf('.');
                    string strRadLatitude = strLat.Substring(0, divinLat + 7);  // 소수점 이하 6자리 까지
                    string strLon = longitude.ToString();
                    int divinLon = strLon.IndexOf('.');
                    string strRadLongitude = strLon.Substring(0, divinLon + 7);  // 소수점 이하 6자리 까지


                    // 상태 표시줄 표시
                    this.toolStripStatusLabelCoord.Text = "좌표 [RAD](" + strRadLatitude + ", " + strRadLongitude + ")    [DMS](" + strLatitude + ", " + strLongitude + ")";
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventMouseMove( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventMouseMove( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// MouseUp Event
        /// </summary>
        /// <param name="geObject"></param>
        private void AdengGoogleEarthCtrlEvtGoogleEventMouseUp(IKmlMouseEvent geObject)
        {
            System.Console.WriteLine("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventMouseUp");
            //FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventMouseUp( Start )");

            try
            {
                if (geObject == null)
                {
                    FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventMouseUp( geObject is NULL )");
                    return;
                }

                if (this.targetingMode == TargetingMode.Region && geObject.getButton() == 0)
                {
                    if (this.clickStartPosition.X != geObject.getClientX() ||
                        this.clickStartPosition.Y != geObject.getClientY())
                    {
                        // 드래그 모드
                        //FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventMouseUp( 지역선택 모드 / 드래그 모드 )");

                        return;
                    }

                    IKmlPlacemark placeMark = geObject.getTarget() as IKmlPlacemark;
                    if (placeMark != null)
                    {
                        string id = placeMark.getId();

                        if (string.IsNullOrEmpty(id) && !id.Contains("icon"))
                        {
                            //기본 지점 풍선 표시 안함
                            geObject.preventDefault();

                            // 지역 플레이스마크 색깔 변경 처리.
                            ClickedPlacemarkArea(placeMark);
                        }
                    }
                }
                else if (this.targetingMode == TargetingMode.Radius && geObject.getButton() == 0)
                {
                    //FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventMouseUp( 반경 그리기 모드 / 마우스 왼쪽 버튼 클릭 )");

                    IKmlPlacemark placeMark = geObject.getTarget() as IKmlPlacemark;
                    if (placeMark != null)
                    {
                        string name = placeMark.getName();
                        if (!string.IsNullOrEmpty(name) && name.ToUpper().Contains("CIRCLE"))
                        {
                            geObject.preventDefault();
                        }
                    }

                    // 커서 모양 복원
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    // 시스템 선택 모드는 처리 불필요.
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventMouseUp(" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrlEvtGoogleEventMouseUp( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// Draw Circle Event
        /// </summary>
        /// <param name="geObject"></param>
        /// <param name="centerLatitude"></param>
        /// <param name="centerLongitude"></param>
        /// <param name="radiusInMeter"></param>
        private void AdengGoogleEarthCtrl_evtGoogleDrawCircle(IKmlPlacemark geObject, CircleInfo circleInfo)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrl_evtGoogleDrawCircle( Start )");

            try
            {
                // 드래그 중에는 모래시계표시(실제로는 빙빙도는 원형 커서)
                this.Cursor = Cursors.WaitCursor;

                // 써클 정보 설정
                if (circleInfo != null)
                {
                    //this.txtboxLatitude.Text = circleInfo.centerX.ToString();
                    //this.txtboxLongitude.Text = circleInfo.centerY.ToString();
                    this.txtboxRadius.Text = circleInfo.radiusInMeter.ToString();
                }
                else
                {
                    FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrl_evtGoogleDrawCircle( 써클 정보가 없음 )");
                }

                bool isExtracted = AreaExtractionInCircle(geObject);
                if (!isExtracted)
                {
                    FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrl_evtGoogleDrawCircle( 반경 내 지역정보 추출 처리 실패 )");
                    return;
                }

                //sw.Stop();
                //Console.WriteLine("★★★ 수행시간: {0}", sw.Elapsed);

                if (this.lvSelectedAreaList.Items.Count > 0)
                {
                    FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrl_evtGoogleDrawCircle( 발령창 표시 준비 )");

                    // 발령 준비 정보 추가
                    if (this.currentOrderProvisionInfo == null)
                    {
                        this.currentOrderProvisionInfo = new OrderProvisionInfo();
                    }
                    if (this.currentOrderProvisionInfo.TargetRegions == null)
                    {
                        this.currentOrderProvisionInfo.TargetRegions = new List<RegionDefinition>();
                    }
                    this.currentOrderProvisionInfo.TargetRegions.Clear();

                    foreach (AdengListViewItem item in this.lvSelectedAreaList.Items)
                    {
                        RegionProfile region = item.Tag as RegionProfile;
                        if (region != null)
                        {
                            this.currentOrderProvisionInfo.TargetRegions.Add(new RegionDefinition(region.Code, region.Name));
                        }
                    }

                    if (this.currentOrderProvisionInfo.Circle == null)
                    {
                        this.currentOrderProvisionInfo.Circle = new List<CircleInfo>();
                    }
                    this.currentOrderProvisionInfo.Circle.Add(new CircleInfo(circleInfo.centerX, circleInfo.centerY, circleInfo.radiusInMeter));

                    this.currentOrderProvisionInfo.Scope = CAPLib.ScopeType.Public;
                    ShowSimpleOrderWindow();
                }
                else
                {
                    FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrl_evtGoogleDrawCircle( 발령창 닫기 준비 )");

                    // 선택한 지역이 한 개도 없으므로, 발령 팝업 닫기
                    if (IsOrderWindowAlive())
                    {
                        this.orderWindow.Close();
                        this.orderWindow.NotifyOrderProvisionResult -= new EventHandler<OrderPrepareEventArgs>(orderWindow_OnNotifyOrderPrepareResult);
                        this.orderWindow = null;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] AdengGoogleEarthCtrl_evtGoogleDrawCircle( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrl_evtGoogleDrawCircle( " + ex.ToString() + " )");
            }

            //FileLogManager.GetInstance().WriteLog("[MainForm] AdengGoogleEarthCtrl_evtGoogleDrawCircle( End )");
        }
        #endregion

        #region 플레이스마크 제어
        /// <summary>
        /// 행정동 경계 데이터와 표준경보시스템 아이콘 정보를 로드.
        /// </summary>
        private void LoadPlaceMarkData()
        {
            LoadRegionPlacemark();
            LoadSystemPlacemark();
        }

        /// <summary>
        /// 행정 구역 경계 지도 데이터 로딩
        /// </summary>
        private void LoadRegionPlacemark()
        {
            if (!this.isGISLoadingCompleted)
            {
                return;
            }

            try
            {
                if (!BasisData.IsRegionDataLoaded())
                {
                    MessageBox.Show("지역 데이터가 존재하지 않습니다. 지도 정보를 로딩할 수 없습니다.", "지역 데이터 로딩 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (RegionProfile region1 in BasisData.Regions.LstRegion.Values)
                {
                    // 현 발령대의 최상위 지역(중앙표준발령대: 전국, 시도표준발령대: 시도, 시군표준발령대: 시군)
                    KmlDocumentInfo kmlDocInfo = MakeRegionKmlDocument(region1.Code, region1.Name, region1.KmlText);
                    if (kmlDocInfo != null && kmlDocInfo.KmlObject != null)
                    {
                        region1.KmlInfo = kmlDocInfo;

                        bool visible = (this.currentOuterLineVisibleMode == OuterLineVisibleMode.High);
                        this.externalGE.SetKmlDocumentVisible(kmlDocInfo.KmlObject as IKmlDocument, visible);
                    }

                    // 현 발령대의 하위 지역(중앙표준발령대: 시도, 시도표준발령대: 시군, 시군표준발령대: 읍면동)
                    if (region1.LstSubRegion == null)
                    {
                        continue;
                    }
                    foreach (RegionProfile region2 in region1.LstSubRegion.Values)
                    {
                        kmlDocInfo = MakeRegionKmlDocument(region2.Code, region2.Name, region2.KmlText);
                        if (kmlDocInfo != null && kmlDocInfo.KmlObject != null)
                        {
                            region2.KmlInfo = kmlDocInfo;

                            bool visible = (this.currentOuterLineVisibleMode == OuterLineVisibleMode.Middle);
                            this.externalGE.SetKmlDocumentVisible(kmlDocInfo.KmlObject as IKmlDocument, visible);
                        }

                        // 현 발령대의 최하위 지역(중앙표준발령대: 시군, 시도표준발령대: 읍면동)
                        if (region2.LstSubRegion == null)
                        {
                            continue;
                        }
                        foreach (RegionProfile region3 in region2.LstSubRegion.Values)
                        {
                            kmlDocInfo = MakeRegionKmlDocument(region3.Code, region3.Name, region3.KmlText);
                            if (kmlDocInfo != null && kmlDocInfo.KmlObject != null)
                            {
                                region3.KmlInfo = kmlDocInfo;

                                bool visible = (this.currentOuterLineVisibleMode == OuterLineVisibleMode.Low);
                                this.externalGE.SetKmlDocumentVisible(kmlDocInfo.KmlObject as IKmlDocument, visible);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] LoadRegionPlacemark( LoadRegionAllGroundOverlay Exception : " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] LoadRegionPlacemark( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// 문자열 데이터로 지역 경계 KML 데이터를 작성.
        /// </summary>
        /// <param name="regionCode"></param>
        /// <param name="regionName"></param>
        /// <param name="kmlText"></param>
        /// <returns></returns>
        private KmlDocumentInfo MakeRegionKmlDocument(string regionCode, string regionName, string kmlText)
        {
            KmlDocumentInfo kmlDocumentInfo = null;

            try
            {
                if (!this.isGISLoadingCompleted)
                {
                    return null;
                }

                if (string.IsNullOrEmpty(regionCode) || string.IsNullOrEmpty(regionName) || string.IsNullOrEmpty(kmlText))
                {
                    return null;
                }

                kmlDocumentInfo = new KmlDocumentInfo(regionCode, regionName);
                kmlDocumentInfo.KmlObject = this.externalGE.AppendKml(kmlText);
                if (kmlDocumentInfo.KmlObject == null)
                {
                    return null;
                }

                // ★★★ KmlObject를 생성할 때 아이디를 지정해서 생성할 수 있는 방법을 찾아보자.
                // 분위기를 봐서는, 문자열로 직접 생성하는 것 외에는 방법이 없는 듯 하다.
                if (string.IsNullOrEmpty(kmlDocumentInfo.KmlObject.getId()))
                {
                    this.externalGE.RemoveKml(kmlDocumentInfo.KmlObject);
                    return null;
                }
                IKmlDocument kmlDocument = kmlDocumentInfo.KmlObject as IKmlDocument;
                if (kmlDocument == null)
                {
                    return kmlDocumentInfo;
                }

                IKmlAbstractView view = kmlDocument.getAbstractView();
                if (view == null)
                {
                    if (kmlDocument.getFeatures() != null)
                    {
                        KmlObjectListCoClass nodeLst = kmlDocument.getFeatures().getChildNodes();
                        if (nodeLst != null && nodeLst.getLength() > 0)
                        {
                            IKmlPlacemark placemark = nodeLst.item(0) as IKmlPlacemark;
                            if (placemark != null)
                            {
                                view = placemark.getAbstractView();
                            }
                        }
                    }
                }
                if (view != null)
                {
                    IKmlLookAt lookat = view.copyAsLookAt();
                    if (lookat != null)
                    {
                        kmlDocumentInfo.Latitude = lookat.getLatitude();
                        kmlDocumentInfo.Longitude = lookat.getLongitude();
                        kmlDocumentInfo.Altitude = lookat.getAltitude();
                        kmlDocumentInfo.Tilt = lookat.getTilt();
                        kmlDocumentInfo.Range = lookat.getRange();
                    }
                }

                IKmlStyleMap styleMap = this.externalGE.GetStyleMap(EColorStyle.RED);
                string url = styleMap.getUrl();
                if (styleMap != null)
                {
                    kmlDocument.getStyleSelectors().appendChild(styleMap);
                }
                styleMap = this.externalGE.GetStyleMap(EColorStyle.BLUE);
                if (styleMap != null)
                {
                    kmlDocument.getStyleSelectors().appendChild(styleMap);
                }
                styleMap = this.externalGE.GetStyleMap(EColorStyle.WHITE);
                if (styleMap != null)
                {
                    kmlDocument.getStyleSelectors().appendChild(styleMap);
                }
                KmlObjectListCoClass lst = kmlDocument.getFeatures().getChildNodes();
                for (int i = 0; i < lst.getLength(); i++)
                {
                    if (lst.item(i) is IKmlPlacemark)
                    {
                        IKmlPlacemark placemark = lst.item(i) as IKmlPlacemark;
                        placemark.setStyleUrl(styleMap.getUrl());
                    }
                    else if (lst.item(i) is IKmlFolder)
                    {
                        IKmlFolder kmlFolder = lst.item(i) as IKmlFolder;
                        KmlObjectListCoClass folderChilds = kmlFolder.getFeatures().getChildNodes();
                        for (int j = 0; j < folderChilds.getLength(); j++)
                        {
                            if (folderChilds.item(j) is IKmlPlacemark)
                            {
                                IKmlPlacemark placemark = folderChilds.item(j) as IKmlPlacemark;
                                placemark.setStyleUrl(styleMap.getUrl());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] MakeRegionKmlDocument( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] MakeRegionKmlDocument( " + ex.ToString() + " )");
            }

            return kmlDocumentInfo;
        }

        /// <summary>
        /// 표준경보시스템 아이콘 로딩
        /// </summary>
        private void LoadSystemPlacemark()
        {
            if (!this.isGISLoadingCompleted)
            {
                return;
            }

            try
            {
                if (this.currentDicSystemInfo == null || this.currentDicSystemInfo.Count <= 0)
                {
                    System.Console.WriteLine("[MainForm] LoadSystemPlacemark (표준경보시스템 목록이 비었습니다.)");
                    return;
                }

                foreach (SASInfo info in this.currentDicSystemInfo.Values)
                {
                    IconInfo iconInfo = new IconInfo();
                    iconInfo.PlacemarkID = info.Profile.ID;
                    iconInfo.IconName = info.Profile.Name;

                    iconInfo.IconURL = GetSystemIconUrl(info.Profile.KindCode);

                    info.IconURL = iconInfo.IconURL;
                    double coord = 0.0;
                    if (double.TryParse(info.Profile.Latitude, out coord))
                    {
                        iconInfo.Latitude = coord;
                    }
                    else
                    {
                        iconInfo.Latitude = 0.0;
                    }
                    if (double.TryParse(info.Profile.Longitude, out coord))
                    {
                        iconInfo.Longitude = coord;
                    }
                    else
                    {
                        iconInfo.Longitude = 0.0;
                    }
                    KmlExtendedData data = new KmlExtendedData();
                    data.DataName = "SystemID";
                    data.Data = info.Profile.ID;
                    iconInfo.LstExtendedData.Add(data);
                    data = new KmlExtendedData();
                    data.DataName = "SystemName";
                    data.Data = info.Profile.Name;
                    iconInfo.LstExtendedData.Add(data);
                    data = new KmlExtendedData();
                    data.DataName = "IP";
                    data.Data = info.Profile.IpAddress;
                    iconInfo.LstExtendedData.Add(data);
                    data = new KmlExtendedData();
                    data.DataName = "SystemType";
                    SASKind kind = BasisData.FindSASKindByCode(info.Profile.KindCode);
                    if (kind != null)
                    {
                        data.Data = kind.Name;
                    }
                    else
                    {
                        data.Data = "Unknown";
                    }
                    iconInfo.LstExtendedData.Add(data);
                    info.Placemark = this.externalGE.CreateSystemIcon(iconInfo) as IKmlPlacemark;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] LoadSystemPlacemark( Exception=[" + ex.ToString() + "] )");
                FileLogManager.GetInstance().WriteLog("[MainForm] LoadSystemPlacemark( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// 표준경보시스템 정보 갱신(GIS 아이콘)
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="profile"></param>
        private void UpdateSystemPlacemark(ProfileUpdateMode mode, SASProfile profile)
        {
            if (mode == ProfileUpdateMode.Regist)
            {
                RegistSystemPlacemark(profile);
            }
            else if (mode == ProfileUpdateMode.Delete)
            {
                DeleteSystemPlacemark(profile);
            }
            else if (mode == ProfileUpdateMode.Modify)
            {
                ModifySystemPlacemark(profile);
            }
            else
            {
                // do nothing
            }
        }
        /// <summary>
        /// 표준경보시스템 정보 등록(GIS 아이콘)
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        private bool RegistSystemPlacemark(SASProfile profile)
        {
            if (!this.isGISLoadingCompleted)
            {
                return false;
            }

            try
            {
                // 종류를 보고, 딕셔너리에서 삭제할지 갱신할지 추가할지 .. 해야한다.
                SASInfo systemInfo = null;
                foreach (SASInfo info in this.currentDicSystemInfo.Values)
                {
                    if (profile.ID == info.Profile.ID)
                    {
                        systemInfo = info;
                        break;
                    }
                }
                if (systemInfo == null)
                {
                    systemInfo = new SASInfo();
                }
                else
                {
                    System.Console.WriteLine("[시스템 프로필 추가] 이미 등록되어 있는 표준경보시스템 => 수정");
                    //                return false;
                }

                IconInfo iconInfo = CreateSystemPlacemark(profile);
                if (iconInfo == null)
                {
                    System.Console.WriteLine("[시스템 프로필 수정] 아이콘 생성 실패");
                    return false;
                }

                systemInfo.Profile = profile;
                systemInfo.IconURL = iconInfo.IconURL;
                systemInfo.Placemark = this.externalGE.CreateSystemIcon(iconInfo) as IKmlPlacemark;

                this.currentDicSystemInfo.Add(profile.ID, systemInfo);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] RegistSystemPlacemark( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] RegistSystemPlacemark( " + ex.ToString() + " )");
            }
            return true;
        }
        /// <summary>
        /// 표준경보시스템 정보 수정(GIS 아이콘)
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        private bool ModifySystemPlacemark(SASProfile profile)
        {
            if (!this.isGISLoadingCompleted)
            {
                return false;
            }

            try
            {
                // 종류를 보고, 딕셔너리에서 삭제할지 갱신할지 추가할지 .. 해야한다.
                SASInfo systemInfo = null;

                Dictionary<string, SASInfo> systemList = null;
                int result = this.core.GetSASInfo(out systemList);
                if (result != 0 || systemList == null || systemList.Count <= 0)
                {
                    System.Console.WriteLine("[시스템 프로필 수정] 프로필 정보가 존재하지 않습니다.");
                    return false;
                }

                foreach (SASInfo info in currentDicSystemInfo.Values)
                {
                    if (profile.ID == info.Profile.ID)
                    {
                        systemInfo = info;
                        break;
                    }
                }
                if (systemInfo == null)
                {
                    System.Console.WriteLine("[시스템 프로필 수정] 미등록 표준경보시스템 => 신규 등록");

                    return RegistSystemPlacemark(profile);
                }

                // 기존 정보 삭제
                this.externalGE.SetKmlDocumentVisible(systemInfo.Placemark as IKmlDocument, false);
                this.externalGE.RemoveKml(systemInfo.Placemark as IKmlObject);

                IconInfo iconInfo = CreateSystemPlacemark(profile);
                if (iconInfo == null)
                {
                    System.Console.WriteLine("[시스템 프로필 수정] 아이콘 생성 실패");
                    return false;
                }

                // 갱신
                systemInfo.Profile = profile;
                systemInfo.IconURL = iconInfo.IconURL;
                systemInfo.Placemark = this.externalGE.CreateSystemIcon(iconInfo) as IKmlPlacemark;
                this.externalGE.SetKmlDocumentVisible(systemInfo.Placemark as IKmlDocument, true);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] ModifySystemPlacemark( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] ModifySystemPlacemark( " + ex.ToString() + " )");
            }

            return true;
        }
        /// <summary>
        /// 표준경보시스템 정보 삭제(GIS 아이콘)
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        private bool DeleteSystemPlacemark(SASProfile profile)
        {
            if (!this.isGISLoadingCompleted)
            {
                return false;
            }

            try
            {
                // 풍선 닫기 처리 필요★★★


                // 종류를 보고, 딕셔너리에서 삭제할지 갱신할지 추가할지 .. 해야한다.
                SASInfo systemInfo = null;
                foreach (SASInfo info in this.currentDicSystemInfo.Values)
                {
                    if (profile.ID == info.Profile.ID)
                    {
                        systemInfo = info;
                        break;
                    }
                }
                if (systemInfo == null)
                {
                    System.Console.WriteLine("[시스템 프로필 삭제] 미등록/삭제된 표준경보시스템");
                    return false;
                }

                this.externalGE.SetKmlDocumentVisible(systemInfo.Placemark as IKmlDocument, false);
                this.externalGE.RemoveKml(systemInfo.Placemark as IKmlObject);

                this.currentDicSystemInfo.Remove(profile.ID);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] DeleteSystemPlacemark( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] DeleteSystemPlacemark( " + ex.ToString() + " )");
            }

            return true;
        }
        /// <summary>
        /// 표준경보시스템 정보 리셋(GIS 아이콘)
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        private void DeleteAllSystemPlacemark()
        {
            if (!this.isGISLoadingCompleted)
            {
                return;
            }

            try
            {
                // 풍선 닫기 처리 필요★★★

                if (this.currentDicSystemInfo == null || this.currentDicSystemInfo.Values == null)
                {
                    return;
                }
                foreach (SASInfo info in this.currentDicSystemInfo.Values)
                {
                    if (info.Placemark == null)
                    {
                        continue;
                    }

                    this.externalGE.SetKmlDocumentVisible(info.Placemark as IKmlDocument, false);
                    this.externalGE.RemoveKml(info.Placemark as IKmlObject);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] DeleteAllSystemPlacemark( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] DeleteAllSystemPlacemark( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// 표준경보시스템 GIS 아이콘 생성
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        private IconInfo CreateSystemPlacemark(SASProfile profile)
        {
            IconInfo iconInfo = new IconInfo();
            iconInfo.PlacemarkID = profile.ID;

            iconInfo.IconURL = GetSystemIconUrl(profile.KindCode);

            double coord = 0.0;
            if (double.TryParse(profile.Latitude, out coord))
            {
                iconInfo.Latitude = coord;
            }
            else
            {
                iconInfo.Latitude = 0.0;
            }
            if (double.TryParse(profile.Longitude, out coord))
            {
                iconInfo.Longitude = coord;
            }
            else
            {
                iconInfo.Longitude = 0.0;
            }

            if (iconInfo.LstExtendedData == null)
            {
                iconInfo.LstExtendedData = new List<KmlExtendedData>();
            }

            KmlExtendedData data = new KmlExtendedData();
            data.DataName = "SystemID";
            data.Data = profile.ID;
            iconInfo.LstExtendedData.Add(data);
            data = new KmlExtendedData();
            data.DataName = "SystemName";
            data.Data = profile.Name;
            iconInfo.LstExtendedData.Add(data);
            data = new KmlExtendedData();
            data.DataName = "IP";
            data.Data = profile.IpAddress;
            iconInfo.LstExtendedData.Add(data);
            data = new KmlExtendedData();
            data.DataName = "SystemType";
            SASKind systemKind = BasisData.FindSASKindByCode(profile.KindCode);
            if (systemKind != null)
            {
                data.Data = systemKind.Name;
            }
            else
            {
                data.Data = string.Empty;
            }
            iconInfo.LstExtendedData.Add(data);

            return iconInfo;
        }
        private string GetSystemIconUrl(string systemKindCode, bool isSelected = false)
        {
            System.Diagnostics.Debug.Assert(systemKindCode != null);

            string iconName = systemKindCode;
            string url = string.Empty;
            string defaultPath = "http://localhost/icon/";

            switch (systemKindCode)
            {
                case "APAS":
                case "AVNS":
                case "DMB":
                case "RDS":
                case "CAS":
                case "CBS":
                case "DMND":
                case "IABS":
                case "EABS":
                case "ED":
                case "BIS":
                    break;
                default:
                    iconName = "Unknown";
                    break;
            }

            if (isSelected)
            {
                url = defaultPath + "SelectedImg/" + iconName + ".png";
            }
            else
            {
                url = defaultPath + iconName + ".png";
            }

            return url;
        }


        /// <summary>
        /// 맵 상의 아이콘을 클릭했을 때의 처리
        /// </summary>
        /// <param name="placeMark">플레이스 마크 오브젝트</param>
        private void ClickedPlacemarkIcon(IKmlPlacemark placeMark)
        {
            try
            {
                IKmlExtendedData extendData = placeMark.getExtendedData();
                if (extendData == null)
                {
                    return;
                }

                bool isChecked = false;
                SASProfile selectedSystemProfile = null;

                for (int i = 0; i < extendData.getDataCount(); i++)
                {
                    IKmlData data = extendData.getData(i);
                    string dataName = data.getName();
                    string dataValue = data.getValue();

                    if (dataName == "SystemID")
                    {
                        ListViewItem item = null;
                        foreach (ListViewItem view in this.lvStdAlertSystemList.Items)
                        {
                            if (view.Name == dataValue)
                            {
                                item = view;
                                break;
                            }
                        }
                        if (item != null)
                        {
                            item.Checked = !item.Checked;
                            isChecked = item.Checked;
                            selectedSystemProfile = item.Tag as SASProfile;
                            break;
                        }
                    }
                }

                if (!this.adengGoogleEarthCtrl.ContainsFocus)
                {
                    return;
                }
                if (selectedSystemProfile == null)
                {
                    return;
                }

                ChangeSystemColor(selectedSystemProfile.ID, isChecked);

                // 발령 준비 정보 추가
                if (this.currentOrderProvisionInfo == null)
                {
                    this.currentOrderProvisionInfo = new OrderProvisionInfo();
                }
                if (this.currentOrderProvisionInfo.TargetSystems == null)
                {
                    this.currentOrderProvisionInfo.TargetSystems = new List<SASProfile>();
                }

                if (isChecked)
                {
                    this.currentOrderProvisionInfo.TargetSystems.Add(selectedSystemProfile);
                }
                else
                {
                    for (int index = 0; index < this.currentOrderProvisionInfo.TargetSystems.Count; index++)
                    {
                        if (selectedSystemProfile.Name == this.currentOrderProvisionInfo.TargetSystems[index].Name)
                        {
                            this.currentOrderProvisionInfo.TargetSystems.RemoveAt(index);
                            break;
                        }
                    }
                }

                // 모든 아이템의 체크 상태 확인
                int count = this.lvStdAlertSystemList.CheckedItems.Count;
                if (count > 0)
                {
                    this.btnAddSystemGroup.Enabled = false;

                    this.currentOrderProvisionInfo.Scope = CAPLib.ScopeType.Private;
                    ShowSimpleOrderWindow();
                }
                else
                {
                    // 선택한 지역이 한 개도 없으므로, 발령 팝업 닫기
                    if (IsOrderWindowAlive())
                    {
                        this.orderWindow.Close();
                        this.orderWindow.NotifyOrderProvisionResult -= new EventHandler<OrderPrepareEventArgs>(orderWindow_OnNotifyOrderPrepareResult);
                        this.orderWindow = null;
                    }

                    this.btnAddSystemGroup.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] ClickedPlacemarkIcon( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] ClickedPlacemarkIcon( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// 맵 상에서 지역(다각형)을 클릭했을 때의 처리
        /// </summary>
        /// <param name="placeMark">플레이스 마크 오브젝트</param>
        private void ClickedPlacemarkArea(IKmlPlacemark placeMark)
        {
            System.Console.WriteLine("[MainFom] ClickedPlacemarkArea ( 시작 )");

            if (!this.isGISLoadingCompleted)
            {
                return;
            }

            try
            {
                if (placeMark == null || placeMark.getParentNode() == null ||
                    string.IsNullOrEmpty(placeMark.getParentNode().getType()))
                {
                    System.Console.WriteLine("[MainFom] ClickedPlacemarkArea ( placeMark 타입 취득 실패 )");
                    return;
                }

                string currentPlacemarkType = placeMark.getParentNode().getType();
                System.Console.WriteLine("[MainFom] ClickedPlacemarkArea ( currentPlacemarkType=[" + currentPlacemarkType + "] )");
                if ("KmlDocument" != currentPlacemarkType && "IKmlDocument" != currentPlacemarkType)
                {
                    // 일단 여기서는 대상외
                    System.Console.WriteLine("ClickedPlacemarkArea : ParentNode 의 타입이 올바르지 않습니다. [" + currentPlacemarkType + ")");
                    //                return;
                }

                string regionId = null;
                IKmlDocument document = null;
                KmlObjectListCoClass lst = null;

                if ("KmlDocument" == currentPlacemarkType || "IKmlDocument" == currentPlacemarkType)
                {
                    document = placeMark.getParentNode() as IKmlDocument;
                    lst = document.getFeatures().getChildNodes();
                    regionId = document.getId();
                }
                else if ("KmlFolder" == currentPlacemarkType)
                {
                    IKmlFolder folder = placeMark.getParentNode() as IKmlFolder;
                    if (folder != null)
                    {
                        lst = folder.getFeatures().getChildNodes();
                        regionId = folder.getId();
                    }
                }
                else if ("KmlLayer" == currentPlacemarkType)
                {
                    //IKmlLayer layer = placeMark.getParentNode() as IKmlLayer;
                    //IKmlDocument layerDoc = placeMark.getParentNode() as IKmlDocument;

                    //if (layer != null)
                    //{
                    //    lst = layer.getFeatures().getChildNodes();
                    //    string layerID = layer.getId();

                    //    // 이 접근은 실패
                    //    Object rootObj = layer.getFeatures().getRootObject();
                    //    string objType = rootObj.GetType().ToString();
                    //    IKmlObject obj = rootObj as IKmlObject;
                    //    if (obj != null)
                    //    {
                    //        IKmlDocument doc = obj as IKmlDocument;
                    //        regionId = doc.getId();
                    //    }

                    //    // is null
                    //    //IKmlDocument kmlDoc2 = layer.getOwnerDocument();
                    //    IKmlObject kmlObj2 = layer.getParentNode();
                    //    if (kmlObj2 != null)
                    //    {

                    //    }
                    //}
                }
                else
                {
                }


                IKmlStyleMap styleMap = this.externalGE.GetStyleMap(EColorStyle.BLUE);
                if (styleMap == null)
                {
                    System.Console.WriteLine("ClickedPlacemarkArea : styleMap 가 null 입니다.");
                    return;
                }

                bool selectionState = false;
                int listCount = 0;
                if (lst != null)
                {
                    listCount = lst.getLength();
                }
                for (int i = 0; i < listCount; i++)
                {
                    if (lst.item(i) is IKmlPlacemark)
                    {
                        IKmlPlacemark placemark = lst.item(i) as IKmlPlacemark;
                        if (placemark.getStyleUrl() == styleMap.getUrl())
                        {
                            placemark.setStyleUrl("#stm_white");
                        }
                        else
                        {
                            placemark.setStyleUrl(styleMap.getUrl());
                            selectionState = true;
                        }

                        placemark.setVisibility(placemark.getVisibility());
                    }
                    else if (lst.item(i) is IKmlFolder)
                    {
                        IKmlFolder kmlFolder = lst.item(i) as IKmlFolder;
                        KmlObjectListCoClass folderChilds = kmlFolder.getFeatures().getChildNodes();
                        for (int j = 0; j < folderChilds.getLength(); j++)
                        {
                            if (folderChilds.item(j) is IKmlPlacemark)
                            {
                                IKmlPlacemark placemark = folderChilds.item(j) as IKmlPlacemark;
                                if (placemark.getStyleUrl() == styleMap.getUrl())
                                {
                                    placemark.setStyleUrl("#stm_white");
                                }
                                else
                                {
                                    placemark.setStyleUrl(styleMap.getUrl());
                                    selectionState = true;
                                }

                                placemark.setVisibility(placemark.getVisibility());
                            }
                        }
                    }
                    else
                    {
                        // do nothing
                    }
                }

                if (!this.adengGoogleEarthCtrl.ContainsFocus)
                {
                    return;
                }

                if (!string.IsNullOrEmpty(regionId))
                {
                    //
                    // 행정동명 리스트와 연동하는 코드 추가
                    //
                    ApplyGisToList(regionId, selectionState);
                }

                //
                // 발령 팝업 띄우기
                //
                if (GetSelectedRegionCountFromList() > 0)
                {
                    this.btnAddRegionGroup.Enabled = false;

                    this.currentOrderProvisionInfo.Scope = CAPLib.ScopeType.Public;
                    ShowSimpleOrderWindow();
                }
                else
                {
                    // 선택한 지역이 한 개도 없는 경우, 발령 팝업 닫기
                    if (IsOrderWindowAlive())
                    {
                        this.orderWindow.Close();
                        this.orderWindow.NotifyOrderProvisionResult -= new EventHandler<OrderPrepareEventArgs>(orderWindow_OnNotifyOrderPrepareResult);
                        this.orderWindow = null;
                    }
                    this.btnAddRegionGroup.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] ClickedPlacemarkArea( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] ClickedPlacemarkArea( " + ex.ToString() + " )");
            }
        }


        /// <summary>
        /// 지정된 지역과 반경(써클 플레이스마크)의 충돌 체크
        /// </summary>
        /// <param name="circlePlacemark"></param>
        /// <param name="document"></param>
        /// <returns></returns>
        public bool CheckAreaCrash(IKmlPlacemark circlePlacemark, RegionProfile regionProfile)
        {
            try
            {
                if (circlePlacemark == null || regionProfile == null)
                {
                    System.Console.WriteLine("[MainForm] CheckAreaCrash( input parameter is null )");
                    return false;
                }
                if (regionProfile.KmlInfo == null || regionProfile.KmlInfo.KmlObject == null)
                {
                    System.Console.WriteLine("[MainForm] CheckAreaCrash( input parameter is null(2) )");
                    return false;
                }

                IKmlDocument document = regionProfile.KmlInfo.KmlObject as IKmlDocument;
                GEFeatureContainerCoClass features = document.getFeatures();
                if (features == null)
                {
                    System.Console.WriteLine("[MainForm] CheckAreaCrash( document.getFeatures() is null )");
                    return false;
                }

                KmlObjectListCoClass kmlLst = features.getChildNodes();
                if (kmlLst == null)
                {
                    System.Console.WriteLine("[MainForm] CheckAreaCrash( features.getChildNodes() is null )");
                    return false;
                }

                //FileLogManager.GetInstance().WriteLog("[MainForm] CheckAreaCrash( 지역명: " + regionProfile.Name + " )");
                for (int i = 0; i < kmlLst.getLength(); i++)
                {
                    if (kmlLst.item(i) is IKmlPlacemark)
                    {
                        IKmlPlacemark placemark = kmlLst.item(i) as IKmlPlacemark;
                        bool isCross = CheckPlacemarkCrash(circlePlacemark, placemark);
                        if (isCross)
                        {
                            //FileLogManager.GetInstance().WriteLog("[MainForm] CheckAreaCrash( isCrossed )");
                            return true;
                        }
                    }
                }
                //FileLogManager.GetInstance().WriteLog("[MainForm] CheckAreaCrash( NOT Crossed )");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] CheckAreaCrash( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] CheckAreaCrash( " + ex.ToString() + " )");
            }

            return false;
        }
        /// <summary>
        /// 입력 받은 두 플레이스마크의 충돌 체크
        /// </summary>
        /// <param name="circlePlacemark"></param>
        /// <param name="regionPlacemark"></param>
        /// <returns></returns>
        public bool CheckPlacemarkCrash(IKmlPlacemark circlePlacemark, IKmlPlacemark regionPlacemark)
        {
            //System.Console.WriteLine("[MainForm] CheckPlacemarkCrash( start )");

            if (!this.isGISLoadingCompleted)
            {
                return false;
            }
            if (circlePlacemark == null || regionPlacemark == null)
            {
                System.Console.WriteLine("[MainForm] CheckPlacemarkCrash( input parameter is null )");
                return false;
            }

            try
            {
                bool isCrossed = false;
                List<Point> circlePointList = this.adengGoogleEarthCtrl.GetKmlPointListByKmlPlacemark(circlePlacemark);

                int targetPolygonItemCount = this.adengGoogleEarthCtrl.GetPolygonItemCount(regionPlacemark);
                for (int itemIndex = 0; itemIndex < targetPolygonItemCount; itemIndex++)
                {
                    //FileLogManager.GetInstance().WriteLog("[MainForm] CheckPlacemarkCrash( " + regionPlacemark.getName() + " )");

                    List<Point> regionPointList = this.adengGoogleEarthCtrl.GetKmlPointListByKmlPlacemark(regionPlacemark, itemIndex);

                    for (int index1 = 0; index1 < circlePointList.Count - 1; index1++)
                    {
                        TLine line1 = new TLine();
                        line1.p1 = circlePointList[index1];
                        line1.p2 = circlePointList[index1 + 1];

                        for (int index2 = 0; index2 < regionPointList.Count - 1; index2++)
                        {
                            TLine line2 = new TLine();
                            line2.p1 = regionPointList[index2];
                            line2.p2 = regionPointList[index2 + 1];

                            if ((line1.p1.X == line1.p2.X && line1.p1.Y == line1.p2.Y) &&
                                (line2.p1.X == line2.p2.X && line2.p1.Y == line2.p2.Y) &&
                                (line1.p1.X != line2.p1.X && line1.p1.Y != line2.p1.Y))
                            {
                                continue;
                            }

                            isCrossed = this.externalGE.IsLineCrossed(line1, line2);
                            if (isCrossed)
                            {
                                break;
                            }
                        }
                        if (isCrossed)
                        {
                            break;
                        }
                    }

                    if (isCrossed)
                    {
                        //FileLogManager.GetInstance().WriteLog("[MainForm] CheckAreaCrash( 선분 교차 )");
                        return true;
                    }

                    // 거리 값도 좌표에 비례한 픽셀 값으로 변환해야 한다.
                    // 클릭했을 때의 좌표에서 원점 좌표를 뽑아옴.
                    double latitude = Convert.ToDouble(this.txtboxLatitude.Text);
                    double longitude = Convert.ToDouble(this.txtboxLongitude.Text);

                    Point circleCenter = this.adengGoogleEarthCtrl.GetPixelByGps(latitude, longitude);

                    int disX = Math.Abs(circleCenter.X - circlePointList[0].X);
                    int disY = Math.Abs(circleCenter.Y - circlePointList[0].Y);
                    double disZ = Math.Pow((Math.Pow(disX, 2) + Math.Pow(disY, 2)), 0.5);
                    int pxlRadius = (int)(disZ);

                    // 내포(지정 지역이 원 안에 완전히 포함됨)
                    bool isIncluded = this.externalGE.IsPolygonInsideTheCircle(regionPointList, circleCenter, pxlRadius);
                    if (isIncluded)
                    {
                        //FileLogManager.GetInstance().WriteLog("[MainForm] CheckAreaCrash( 내포: 지정지역이 원 안에 완전히 포함됨. )");
                        return true;
                    }

                    // 외포(원이 지정 지역 안에 완전히 포함됨)
                    bool isFullyInside = this.externalGE.IsPointInsideThePolygon(regionPointList, circleCenter);
                    if (isFullyInside)
                    {
                        //FileLogManager.GetInstance().WriteLog("[MainForm] CheckAreaCrash( 외포: 원이 지정 지역 안에 완전히 포함됨. )");
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] CheckPlacemarkCrash( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] CheckPlacemarkCrash( " + ex.ToString() + " )");
            }

            return false;
        }
        #endregion

        #region 지역경계선표시모드
        /// <summary>
        /// [경계 표시] 모드 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOuterLineHigh_Click(object sender, EventArgs e)
        {
            if (this.currentOuterLineVisibleMode == OuterLineVisibleMode.High)
            {
                AdengImgButtonEx btn = sender as AdengImgButtonEx;
                btn.ChkValue = true;
                return;
            }
            ExchangeOuterLineVisibleMode(this.currentOuterLineVisibleMode, OuterLineVisibleMode.High);
        }
        private void btnOuterLineMiddle_Click(object sender, EventArgs e)
        {
            if (this.currentOuterLineVisibleMode == OuterLineVisibleMode.Middle)
            {
                AdengImgButtonEx btn = sender as AdengImgButtonEx;
                btn.ChkValue = true;
                return;
            }
            ExchangeOuterLineVisibleMode(this.currentOuterLineVisibleMode, OuterLineVisibleMode.Middle);
        }
        private void btnOuterLineLow_Click(object sender, EventArgs e)
        {
            if (this.currentOuterLineVisibleMode == OuterLineVisibleMode.Low)
            {
                AdengImgButtonEx btn = sender as AdengImgButtonEx;
                btn.ChkValue = true;
                return;
            }
            ExchangeOuterLineVisibleMode(this.currentOuterLineVisibleMode, OuterLineVisibleMode.Low);
        }
        private void ExchangeOuterLineVisibleMode(OuterLineVisibleMode oldMode, OuterLineVisibleMode newMode)
        {
            if (oldMode == newMode)
            {
                return;
            }

            // 기존 표시를 숨기기
            if (oldMode == OuterLineVisibleMode.High)
            {
                UpdateOuterLineVisibieStateForHigh(false);
                this.btnOuterLineHigh.ChkValue = false;
            }
            else if (oldMode == OuterLineVisibleMode.Middle)
            {
                UpdateOuterLineVisibieStateForMiddle(false);
                this.btnOuterLineMiddle.ChkValue = false;
            }
            else
            {
                UpdateOuterLineVisibieStateForLow(false);
                this.btnOuterLineLow.ChkValue = false;
            }

            // 새 모드 경계선 표시
            if (newMode == OuterLineVisibleMode.High)
            {
                UpdateOuterLineVisibieStateForHigh(true);
                this.btnOuterLineHigh.ChkValue = true;
            }
            else if (newMode == OuterLineVisibleMode.Middle)
            {
                UpdateOuterLineVisibieStateForMiddle(true);
                this.btnOuterLineMiddle.ChkValue = true;
            }
            else if (newMode == OuterLineVisibleMode.Low)
            {
                UpdateOuterLineVisibieStateForLow(true);
                this.btnOuterLineLow.ChkValue = true;
            }
            else
            {
            }

            this.currentOuterLineVisibleMode = newMode;
        }

        /// <summary>
        /// 상위 지역의 경계선 표시 갱신.
        /// </summary>
        /// <param name="isVisible"></param>
        private void UpdateOuterLineVisibieStateForHigh(bool isVisible)
        {
            try
            {
                if (!this.isGISLoadingCompleted)
                {
                    return;
                }
                if (!BasisData.IsRegionDataLoaded())
                {
                    return;
                }

                foreach (RegionProfile highRegion in BasisData.Regions.LstRegion.Values)
                {
                    // 현 발령대의 최상위 지역(중앙표준발령대: 전국, 시도표준발령대: 시도, 시군표준발령대: 시군)
                    if (highRegion.KmlInfo == null || highRegion.KmlInfo.KmlObject == null)
                    {
                        System.Console.WriteLine("[MainForm] UpdateOuterLineVisibieStateForHigh ( KML Document 정보가 없음 =[" + highRegion.Code + "] )");
                        continue;
                    }

                    if (this.externalGE != null)
                    {
                        this.externalGE.SetKmlDocumentVisible(highRegion.KmlInfo.KmlObject as IKmlDocument, isVisible);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] UpdateOuterLineVisibieStateForHigh( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] UpdateOuterLineVisibieStateForHigh( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// 중간 지역의 경계선 표시 갱신.
        /// </summary>
        /// <param name="isVisible"></param>
        private void UpdateOuterLineVisibieStateForMiddle(bool isVisible)
        {
            try
            {
                if (!this.isGISLoadingCompleted)
                {
                    return;
                }
                if (!BasisData.IsRegionDataLoaded())
                {
                    return;
                }

                foreach (RegionProfile highRegion in BasisData.Regions.LstRegion.Values)
                {
                    // 현 발령대의 중간 지역(중앙표준발령대: 전국, 시도표준발령대: 시도, 시군표준발령대: 시군)
                    if (highRegion.LstSubRegion == null || highRegion.LstSubRegion.Count < 1)
                    {
                        System.Console.WriteLine("[MainForm] UpdateOuterLineVisibieStateForMiddle ( 지역 정보가 없음 =[" + highRegion.Code + "] )");
                        continue;
                    }

                    foreach (RegionProfile middleRegion in highRegion.LstSubRegion.Values)
                    {
                        if (middleRegion.KmlInfo == null || middleRegion.KmlInfo.KmlObject == null)
                        {
                            System.Console.WriteLine("[MainForm] UpdateOuterLineVisibieStateForMiddle ( KML Document 정보가 없음 =[" + middleRegion.Code + "] )");
                            continue;
                        }
                        if (this.externalGE != null)
                        {
                            this.externalGE.SetKmlDocumentVisible(middleRegion.KmlInfo.KmlObject as IKmlDocument, isVisible);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] UpdateOuterLineVisibieStateForMiddle( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] UpdateOuterLineVisibieStateForMiddle( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// 하위 지역의 경계선 표시 갱신.
        /// </summary>
        /// <param name="isVisible"></param>
        private void UpdateOuterLineVisibieStateForLow(bool isVisible)
        {
            try
            {
                if (!this.isGISLoadingCompleted)
                {
                    return;
                }
                if (!BasisData.IsRegionDataLoaded())
                {
                    return;
                }

                foreach (RegionProfile highRegion in BasisData.Regions.LstRegion.Values)
                {
                    // 현 발령대의 중간 지역(중앙표준발령대: 전국, 시도표준발령대: 시도, 시군표준발령대: 시군)
                    if (highRegion.LstSubRegion == null || highRegion.LstSubRegion.Count < 1)
                    {
                        System.Console.WriteLine("[MainForm] UpdateOuterLineVisibieStateForLow ( 지역 정보가 없음[1] =[" + highRegion.Code + "] )");
                        continue;
                    }

                    foreach (RegionProfile middleRegion in highRegion.LstSubRegion.Values)
                    {
                        if (middleRegion.LstSubRegion == null || middleRegion.LstSubRegion.Count < 1)
                        {
                            System.Console.WriteLine("[MainForm] UpdateOuterLineVisibieStateForLow ( 지역 정보가 없음[2] =[" + middleRegion.Code + "] )");
                            continue;
                        }

                        foreach (RegionProfile lowRegion in middleRegion.LstSubRegion.Values)
                        {
                            if (lowRegion.KmlInfo == null || lowRegion.KmlInfo.KmlObject == null)
                            {
                                System.Console.WriteLine("[MainForm] UpdateOuterLineVisibieStateForLow ( KML Document 정보가 없음 =[" + lowRegion.Code + "] )");
                                continue;
                            }
                            if (this.externalGE != null)
                            {
                                this.externalGE.SetKmlDocumentVisible(lowRegion.KmlInfo.KmlObject as IKmlDocument, isVisible);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] UpdateOuterLineVisibieStateForLow( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] UpdateOuterLineVisibieStateForLow( " + ex.ToString() + " )");
            }
        }
        #endregion


        #region 발령대상선택 모드 변경
        /// <summary>
        /// [행정동 지역 선택] 버튼 클릭 (모드 변경)
        /// </summary>
        private void btnTargetingByRegion_Click(object sender, EventArgs e)
        {
            if (this.targetingMode == TargetingMode.Region)
            {
                this.btnTargetingByRegion.ChkValue = true;
                return;
            }

            bool isChanged = ChangeTargetSelectionMode(this.targetingMode, TargetingMode.Region);
            if (!isChanged)
            {
                this.btnTargetingByRegion.ChkValue = false;
            }
        }
        /// <summary>
        /// [좌표 기준 반경 선택] 버튼 클릭 (모드 변경)
        /// </summary>
        private void btnTargetingByArea_Click(object sender, EventArgs e)
        {
            if (this.targetingMode == TargetingMode.Radius)
            {
                this.btnTargetingByArea.ChkValue = true;
                return;
            }

            bool isChanged = ChangeTargetSelectionMode(this.targetingMode, TargetingMode.Radius);
            if (!isChanged)
            {
                this.btnTargetingByArea.ChkValue = false;
            }
        }
        /// <summary>
        /// [시스템 개별 선택] 버튼 클릭 (모드 변경)
        /// </summary>
        private void btnTargetingBySystem_Click(object sender, EventArgs e)
        {
            if (this.targetingMode == TargetingMode.System)
            {
                this.btnTargetingBySystem.ChkValue = true;
                return;
            }

            bool isChanged = ChangeTargetSelectionMode(this.targetingMode, TargetingMode.System);
            if (!isChanged)
            {
                this.btnTargetingBySystem.ChkValue = false;
            }
        }

        /// <summary>
        /// 발령대상 선택모드를 변경할 때의 전반적인 처리
        /// </summary>
        private bool ChangeTargetSelectionMode(TargetingMode oldMode, TargetingMode newMode)
        {
            if (IsOrderWindowAlive())
            {
                return false;
            }

            if (oldMode != newMode)
            {
                // 반경 그리기 모드의 해제 및 그리기 지우기
                this.adengGoogleEarthCtrl.StopDragCircle();
                this.adengGoogleEarthCtrl.ClearDragCircle();

                this.currentMouseMode = MouseMode.Normal;

                UpdateRadiusModeCtrl(false);
                UpdateSystemModeCtrl(false);
                UpdateRegionModeCtrl(false);

                // 패널 부분의 갱신(새 모드에 대한 컨트롤 표시)
                switch (newMode)
                {
                    case TargetingMode.Radius:
                        {
                            UpdateRadiusModeCtrl(true);
                        }
                        break;
                    case TargetingMode.System:
                        {
                            UpdateSystemModeCtrl(true);
                        }
                        break;
                    case TargetingMode.Region:
                    default:
                        {
                            UpdateRegionModeCtrl(true);
                        }
                        break;
                }

                this.btnTargetListRoll.ChkValue = false;
            }

            // 모드 버튼 그룹의 갱신
            TargetingModeButtonCtrl(newMode);

            // 모드 관리 변수의 갱신
            this.targetingMode = newMode;

            return true;
        }

        /// <summary>
        /// 발령대상선택 모드를 변경할 때의 버튼 그룹의 표시 처리
        /// </summary>
        private void TargetingModeButtonCtrl(TargetingMode newMode)
        {
            this.btnTargetingByRegion.ChkValue = false;
            this.btnTargetingByArea.ChkValue = false;
            this.btnTargetingBySystem.ChkValue = false;

            if (newMode == TargetingMode.Radius)
            {
                this.btnTargetingByArea.ChkValue = true;
            }
            else if (newMode == TargetingMode.System)
            {
                this.btnTargetingBySystem.ChkValue = true;
            }
            else
            {
                this.btnTargetingByRegion.ChkValue = true;
            }
        }

        /// <summary>
        /// 행정동 지역 선택 모드일 때의 컨트롤 표시 업데이트
        /// </summary>
        private void UpdateRegionModeCtrl(bool visible)
        {
            if (visible)
            {
                this.lblTargetListHeaderTitle.Text = "지역 목록";

                int topX = this.pnlTargetListHeader.Location.X;
                int topY = this.pnlTargetListHeader.Location.Y + this.lblTargetListHeaderTitle.Height;

                this.pnlTargetRegionBody.Location = new System.Drawing.Point(topX, topY);

                // [행정동 목록] 지역 선택 상태를 초기화
                foreach (TreeNode node1 in this.tviewRegionList.Nodes)
                {
                    node1.Checked = false;
                    foreach (TreeNode node2 in node1.Nodes)
                    {
                        node2.Checked = false;
                        foreach (TreeNode node3 in node2.Nodes)
                        {
                            node3.Checked = false;
                        }
                    }
                }
                // [지역 그룹] 정보 갱신
                UpdateRegionGroupList();

                this.btnAddRegionGroup.Enabled = true;
            }

            this.pnlTargetRegionBody.Visible = visible;

        }
        /// <summary>
        /// 좌표 기준 반경 선택 모드일 떄의 컨트롤 표시 업데이트
        /// </summary>
        private void UpdateRadiusModeCtrl(bool visible)
        {
            if (visible)
            {
                this.lblTargetListHeaderTitle.Text = "선택된 지역 목록";

                int topX = this.pnlTargetListHeader.Location.X;
                int topY = this.pnlTargetListHeader.Location.Y + this.lblTargetListHeaderTitle.Height;

                this.pnlTargetAreaBody.Location = new System.Drawing.Point(topX, topY);

                // 선택된 지역 목록을 초기화
                this.lvSelectedAreaList.Items.Clear();

                // 반경그리기 모드를 기본 모드로 설정
                this.currentMouseMode = MouseMode.DrawCircle;
                this.lblUseCircleModeTitle.Font = new Font("굴림", 9.0f, FontStyle.Bold);
                this.btnUseCircleMode.ChkValue = true;
                adengGoogleEarthCtrl.StartDragCircle();
            }
            else
            {
                // Default set
                this.currentMouseMode = MouseMode.Normal;
                this.btnUseCircleMode.ChkValue = false;
            }

            this.txtboxLatitude.Text = string.Empty;
            this.txtboxLongitude.Text = string.Empty;
            this.txtboxRadius.Text = string.Empty;
            this.toolStripStatusLabelCoord.Text = string.Empty;

            this.pnlTargetAreaBody.Visible = visible;
            this.pnlJumpToRegion.Visible = visible;
            this.pnlMouseModeMain.Visible = visible;
        }
        /// <summary>
        /// 시스템 개별 선택 모드일 떄의 컨트롤 표시 업데이트
        /// </summary>
        private void UpdateSystemModeCtrl(bool visible)
        {
            if (visible)
            {
                this.lblTargetListHeaderTitle.Text = "표준경보시스템 목록";

                int topX = this.pnlTargetListHeader.Location.X;
                int topY = this.pnlTargetListHeader.Location.Y + this.lblTargetListHeaderTitle.Height;

                this.pnlTargetSystemBody.Location = new System.Drawing.Point(topX, topY);

                // 데이터 갱신
                DeleteAllSystemPlacemark();
                UpdateSystemList(true);
                LoadSystemPlacemark();

                UpdateSystemGroupList();

                // 표준경보시스템 선택 상태를 초기화
                if (this.lvStdAlertSystemList.SelectedItems.Count > 0)
                {
                    for (int index = 0; index < this.lvStdAlertSystemList.Items.Count; index++)
                    {
                        this.lvStdAlertSystemList.Items[index].Checked = false;
                    }
                }

                this.btnAddSystemGroup.Enabled = true;
            }

            this.pnlTargetSystemBody.Visible = visible;
            this.btnAddSystemGroup.Visible = visible;
            this.pnlJumpToRegion.Visible = visible;
        }
        /// <summary>
        /// [지역 그룹 추가] 버튼 클릭 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddRegionGroup_Click(object sender, EventArgs e)
        {
            CreateRegionGroupForm createGroupForm = new CreateRegionGroupForm();

            createGroupForm.NotifyUpdateGroupProfile += new EventHandler<UpdateGroupProfileEventArgs>(groupForm_OnNotifyUpdateGroupProfile);
            createGroupForm.ShowDialog(this);
            createGroupForm.NotifyUpdateGroupProfile -= new EventHandler<UpdateGroupProfileEventArgs>(groupForm_OnNotifyUpdateGroupProfile);
        }
        #endregion

        #region 행정동지역선택 모드
        private int GetSelectedRegionCountFromList()
        {
            int count = 0;
            foreach (TreeNode node1 in this.tviewRegionList.Nodes)
            {
                if (node1.Checked)
                {
                    count++;
                    continue;
                }
                foreach (TreeNode node2 in node1.Nodes)
                {
                    if (node2.Checked)
                    {
                        count++;
                        continue;
                    }
                    foreach (TreeNode node3 in node2.Nodes)
                    {
                        if (node3.Checked)
                        {
                            count++;
                        }
                    }
                }
            }
            return count;
        }
        private void SetRegionCheck(string regionCode)
        {
            if (string.IsNullOrEmpty(regionCode))
            {
                return;
            }

            bool isCompleted = false;
            foreach (TreeNode node1 in this.tviewRegionList.Nodes)
            {
                if (isCompleted)
                {
                    break;
                }

                if (node1.Name == regionCode)
                {
                    node1.Checked = true;
                    isCompleted = true;
                    break;
                }

                if (node1.Nodes == null)
                {
                    continue;
                }
                foreach (TreeNode node2 in node1.Nodes)
                {
                    if (isCompleted)
                    {
                        break;
                    }
                    if (node2.Name == regionCode)
                    {
                        node2.Checked = true;
                        isCompleted = true;
                        break;
                    }

                    if (node2.Nodes == null)
                    {
                        continue;
                    }
                    foreach (TreeNode node3 in node2.Nodes)
                    {
                        if (node3.Name == regionCode)
                        {
                            node3.Checked = true;
                            isCompleted = true;
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 행정동 선택 상태를 초기화.
        /// 행정동 목록 리스트와 지도 상의 선택 상태를 초기화 한다.
        /// </summary>
        private void ClearSelectedRegion()
        {
            this.btnAddRegionGroup.Enabled = true;

            foreach (TreeNode node1 in this.tviewRegionList.Nodes)
            {
                if (node1.Checked)
                {
                    RegionProfile profile = node1.Tag as RegionProfile;
                    ChangeRegionColor(profile, false);
                }
                node1.Checked = false;

                foreach (TreeNode node2 in node1.Nodes)
                {
                    if (node2.Checked)
                    {
                        RegionProfile profile = node2.Tag as RegionProfile;
                        ChangeRegionColor(profile, false);
                    }
                    node2.Checked = false;

                    foreach (TreeNode node3 in node2.Nodes)
                    {
                        if (node3.Checked)
                        {
                            RegionProfile profile = node3.Tag as RegionProfile;
                            ChangeRegionColor(profile, false);
                        }
                        node3.Checked = false;
                    }
                }
            }
        }
        /// <summary>
        /// 지역 그룹 선택 상태를 초기화.
        /// </summary>
        private void ClearSelectedRegionGroup()
        {
            this.btnAddRegionGroup.Enabled = true;

            if (this.lvRegionGroupList.Items == null)
            {
                return;
            }
            foreach (ListViewItem item in this.lvRegionGroupList.Items)
            {
                item.Checked = false;
            }
        }
        /// <summary>
        /// 행정동 지역명 목록의 텍스트를 선택하면 해당 지역으로 이동
        /// </summary>
        private void tviewRegionList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                string selectedRegionCode = e.Node.Name;
                RegionProfile selectedRegionProfile = null;

                if (e.Node.Tag != null)
                {
                    selectedRegionProfile = e.Node.Tag as RegionProfile;
                }
                if (selectedRegionProfile == null || selectedRegionProfile.KmlInfo == null)
                {
                    return;
                }

                // 경계선 표시 모드 변경
                OuterLineVisibleMode newMode = currentOuterLineVisibleMode;
                if (selectedRegionProfile.RegionLevel == RelativeRegionLevel.High)
                {
                    newMode = OuterLineVisibleMode.High;
                }
                else if (selectedRegionProfile.RegionLevel == RelativeRegionLevel.Middle)
                {
                    newMode = OuterLineVisibleMode.Middle;
                }
                else if (selectedRegionProfile.RegionLevel == RelativeRegionLevel.Low)
                {
                    newMode = OuterLineVisibleMode.Low;
                }
                else
                {
                    // 그대로 유지
                }
                ExchangeOuterLineVisibleMode(this.currentOuterLineVisibleMode, newMode);

                JumpToSpecifiedRegion(selectedRegionProfile);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] tviewRegionList_AfterSelect( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] tviewRegionList_AfterSelect( " + ex.ToString() + " )");
            }
        }

        private void tviewRegionList_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                if (e.Node.IsSelected)
                {
                    this.tviewRegionList_AfterSelect(sender, new TreeViewEventArgs(e.Node));
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] tviewRegionList_NodeMouseClick( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] tviewRegionList_NodeMouseClick( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// 행정동 지역명 목록에서 체크 박스에 체크를 설정/해제할 때의 처리
        ///   1. 지도와 연동
        ///   2. 발령 팝업의 표시
        /// </summary>
        private void tviewRegionList_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // 상호 호출 무한 루프를 막기 위해 포커스가 현 컨트롤에 있을 때만 다른 컨트롤 연계
            if (!this.tviewRegionList.Focused || !BasisData.IsRegionDataLoaded())
            {
                return;
            }

            bool isChecked = e.Node.Checked;

            RegionProfile selectedRegionProfile = null;

            if (e.Node.Tag != null)
            {
                selectedRegionProfile = e.Node.Tag as RegionProfile;
            }
            if (selectedRegionProfile == null || selectedRegionProfile.KmlInfo == null)
            {
                System.Console.WriteLine("[MainForm] tviewRegionList_AfterCheck( 선택된 아이템의 데이터 오류 )");
                return;
            }

            // 발령 준비 정보 추가
            if (this.currentOrderProvisionInfo == null)
            {
                this.currentOrderProvisionInfo = new OrderProvisionInfo();
            }
            if (this.currentOrderProvisionInfo.TargetRegions == null)
            {
                this.currentOrderProvisionInfo.TargetRegions = new List<RegionDefinition>();
            }

            if (isChecked)
            {
                if (this.currentOrderProvisionInfo.TargetRegions == null)
                {
                    this.currentOrderProvisionInfo.TargetRegions = new List<DataClass.RegionDefinition>();
                }
                DataClass.RegionDefinition newRegion = new RegionDefinition(selectedRegionProfile.Code, selectedRegionProfile.Name);
                this.currentOrderProvisionInfo.TargetRegions.Add(newRegion);
            }
            else
            {
                for (int index = 0; index < this.currentOrderProvisionInfo.TargetRegions.Count; index++)
                {
                    if (selectedRegionProfile.Code == this.currentOrderProvisionInfo.TargetRegions[index].Code.ToString())
                    {
                        this.currentOrderProvisionInfo.TargetRegions.RemoveAt(index);
                        break;
                    }
                }
            }

            // 색깔 변경
            ChangeRegionColor(selectedRegionProfile, isChecked);

            //
            // 발령 팝업 띄우기
            //
            if (GetSelectedRegionCountFromList() > 0)
            {
                this.btnAddRegionGroup.Enabled = false;

                this.currentOrderProvisionInfo.Scope = CAPLib.ScopeType.Public;
                ShowSimpleOrderWindow();
            }
            else
            {
                // 선택한 지역이 한 개도 없는 경우, 발령 팝업 닫기
                if (IsOrderWindowAlive())
                {
                    this.orderWindow.Close();
                    this.orderWindow.NotifyOrderProvisionResult -= new EventHandler<OrderPrepareEventArgs>(orderWindow_OnNotifyOrderPrepareResult);
                    this.orderWindow = null;
                }

                this.btnAddRegionGroup.Enabled = true;
            }
        }
        /// <summary>
        /// 지역 그룹 목록에서 체크 박스에 체크를 설정/해제할 때의 처리(체크 전 처리)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvRegionGroupList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (IsOrderWindowAlive())
            {
                // 발령 준비 중에는 그룹 선택 변경 불가.
                e.NewValue = CheckState.Unchecked;
                return;
            }
        }
        /// <summary>
        /// 지역 그룹 목록에서 체크 박스에 체크를 설정/해제할 때의 처리
        ///   1. 행정동 목록과 연동
        ///   2. 발령 팝업의 표시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvRegionGroupList_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            // 지역명에 체크를 넣으면 지도 상에서 해당 지역이 선택되어 나타나야 함.
            GroupProfile selectedItem = e.Item.Tag as GroupProfile;

            if (selectedItem == null)
            {
                System.Console.WriteLine("[MainForm] lvRegionGroupList_ItemChecked (리스트 아이템 데이터 오류");
                return;
            }

            // 상호 호출 무한 루프를 막기 위해 포커스가 현 컨트롤에 있을 때만 다른 컨트롤 연계
            if (!this.lvRegionGroupList.Focused || !BasisData.IsRegionDataLoaded())
            {
                return;
            }

            if (e.Item.Checked)
            {
                // 발령 준비 정보에 반영
                this.currentOrderProvisionInfo = ConvertToOrderProvisionInfo(selectedItem);
                if (this.currentOrderProvisionInfo == null)
                {
                    MessageBox.Show("현재의 그룹에 등록된 지역 정보을 식별할 수 없습니다.\n" +
                                "지역 코드가 변경된 행정동일 가능성이 있습니다.\n" +
                                "그룹 정보에서 행정동 선택을 수정하거나 현재의 그룹을 삭제하고 재작성 하십시오.",
                                "발령 준비 오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    return;
                }

                // 지도 표시 갱신
                foreach (string regionCode in selectedItem.Targets)
                {
                    SetRegionCheck(regionCode);
                    ChangeRegionColor(regionCode, true);
                }
            }
            else
            {
                // 그룹 선택 후 지역 선택 변경이 가능한 한, 제약사항이 많아 그룹 개념으로는 할 수 있는게 없음.
                // 단지 그룹 선택 시에 데이터를 로딩하는 것만 대응.
            }

            //
            // 발령 팝업 띄우기
            //
            if (e.Item.Checked || GetSelectedRegionCountFromList() > 0)
            {
                this.btnAddRegionGroup.Enabled = false;

                //this.currentOrderProvisionInfo.Scope = CAPLib.ScopeType.Public;
                ShowSimpleOrderWindow(true);

                this.btnAddRegionGroup.Enabled = true;
            }
            else
            {
                // 선택한 지역이 한 개도 없으므로, 발령 팝업 닫기
                if (IsOrderWindowAlive())
                {
                    this.orderWindow.Close();
                    this.orderWindow.NotifyOrderProvisionResult -= new EventHandler<OrderPrepareEventArgs>(orderWindow_OnNotifyOrderPrepareResult);
                    this.orderWindow = null;
                }

                this.btnAddRegionGroup.Enabled = true;
            }
        }
        /// <summary>
        /// [지역 그룹 선택] 지역 그룹 목록 더블클릭 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvRegionGroupList_DoubleClick(object sender, EventArgs e)
        {
            ListView view = sender as ListView;
            if (view == null)
            {
                return;
            }
            if (view.SelectedItems == null || view.SelectedItems.Count != 1)
            {
                return;
            }
            GroupProfile selectedItem = view.SelectedItems[0].Tag as GroupProfile;
            if (selectedItem == null)
            {
                System.Console.WriteLine("[MainForm] lvSystemGroupList_DoubleClick (리스트 아이템 데이터 오류");
                return;
            }

            EditRegionGroupForm editGroupForm = new EditRegionGroupForm(selectedItem);

            editGroupForm.NotifyUpdateGroupProfile += new EventHandler<UpdateGroupProfileEventArgs>(groupForm_OnNotifyUpdateGroupProfile);
            DialogResult result = editGroupForm.ShowDialog(this);
            editGroupForm.NotifyUpdateGroupProfile -= new EventHandler<UpdateGroupProfileEventArgs>(groupForm_OnNotifyUpdateGroupProfile);
        }

        /// <summary>
        /// 지정된 행정동 아이디에 대한 지도 상의 지역 색깔 표시를 변경
        /// </summary>
        private bool ChangeRegionColor(string regionId, bool toSelect)
        {
            try
            {
                if (!this.isGISLoadingCompleted)
                {
                    return false;
                }
                if (!BasisData.IsRegionDataLoaded())
                {
                    return false;
                }

                OuterLineVisibleMode selectedRegionLevel = OuterLineVisibleMode.All;
                RegionProfile selectedProfile = null;
                bool isFound = BasisData.Regions.LstRegion.ContainsKey(regionId);
                if (isFound)
                {
                    selectedRegionLevel = OuterLineVisibleMode.High;
                    selectedProfile = BasisData.Regions.LstRegion[regionId];
                }
                else
                {
                    foreach (RegionProfile region1 in BasisData.Regions.LstRegion.Values)
                    {
                        if (region1.LstSubRegion == null || region1.LstSubRegion.Count <= 0)
                        {
                            continue;
                        }
                        isFound = region1.LstSubRegion.ContainsKey(regionId);
                        if (isFound)
                        {
                            selectedRegionLevel = OuterLineVisibleMode.Middle;
                            selectedProfile = region1.LstSubRegion[regionId];
                            break;
                        }

                        foreach (RegionProfile region2 in region1.LstSubRegion.Values)
                        {
                            if (region2.LstSubRegion == null || region2.LstSubRegion.Count <= 0)
                            {
                                continue;
                            }
                            isFound = region2.LstSubRegion.ContainsKey(regionId);
                            if (isFound)
                            {
                                selectedRegionLevel = OuterLineVisibleMode.Low;
                                selectedProfile = region2.LstSubRegion[regionId];
                                break;
                            }
                        }
                        if (isFound)
                        {
                            break;
                        }
                    }
                }

                if (selectedProfile == null || selectedProfile.KmlInfo == null || selectedProfile.KmlInfo.KmlObject == null)
                {
                    System.Console.WriteLine("[MainForm] ChangeRegionColor(KML 정보가 없는 행정동 ID [" + regionId + "])");
                    return false;
                }

                bool isVisible = (this.currentOuterLineVisibleMode == selectedRegionLevel);
                IKmlStyleMap styleMap = this.externalGE.GetStyleMap(EColorStyle.BLUE);

                IKmlDocument kmlDoc = selectedProfile.KmlInfo.KmlObject as IKmlDocument;
                if (kmlDoc == null || kmlDoc.getFeatures() == null)
                {
                    return false;
                }
                KmlObjectListCoClass kmlLst = kmlDoc.getFeatures().getChildNodes();
                if (kmlLst == null)
                {
                    return false;
                }
                for (int i = 0; i < kmlLst.getLength(); i++)
                {
                    if (kmlLst.item(i) is IKmlPlacemark)
                    {
                        IKmlPlacemark placemark = kmlLst.item(i) as IKmlPlacemark;

                        if (toSelect)
                        {
                            placemark.setStyleUrl(styleMap.getUrl());
                        }
                        else
                        {
                            placemark.setStyleUrl("#stm_white");
                        }
                    }
                }
                kmlDoc.setVisibility(kmlDoc.getVisibility());
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] ChangeRegionColor (Exception: " + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] ChangeRegionColor( " + ex.ToString() + " )");
            }
            return true;
        }
        /// <summary>
        /// 지정된 행정동 아이디에 대한 지도 상의 지역 색깔 표시를 변경
        /// </summary>
        private bool ChangeRegionColor(RegionProfile regionProfile, bool toSelect, EColorStyle selectionColor = EColorStyle.BLUE)
        {
            try
            {
                if (!this.isGISLoadingCompleted)
                {
                    return false;
                }
                if (regionProfile == null)
                {
                    System.Console.WriteLine("[MainForm] ChangeRegionColor2( 입력 파라메터 에러 )");
                    return false;
                }
                if (regionProfile.KmlInfo == null || regionProfile.KmlInfo.KmlObject == null)
                {
                    System.Console.WriteLine("[MainForm] ChangeRegionColor2( KML 정보가 없는 행정동 ID [" + regionProfile.Code + "] )");
                    return false;
                }

                IKmlStyleMap styleMap = this.externalGE.GetStyleMap(selectionColor);

                IKmlDocument kmlDoc = regionProfile.KmlInfo.KmlObject as IKmlDocument;
                if (kmlDoc == null || kmlDoc.getFeatures() == null)
                {
                    return false;
                }
                KmlObjectListCoClass kmlLst = kmlDoc.getFeatures().getChildNodes();
                if (kmlLst == null)
                {
                    return false;
                }

                for (int i = 0; i < kmlLst.getLength(); i++)
                {
                    if (kmlLst.item(i) is IKmlPlacemark)
                    {
                        IKmlPlacemark placemark = kmlLst.item(i) as IKmlPlacemark;

                        if (toSelect)
                        {
                            placemark.setStyleUrl(styleMap.getUrl());
                        }
                        else
                        {
                            placemark.setStyleUrl("#stm_white");
                        }
                    }
                }
                kmlDoc.setVisibility(kmlDoc.getVisibility());
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] ChangeRegionColor2( Exception: " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] ChangeRegionColor2( " + ex.ToString() + " )");
            }

            return true;
        }
        /// <summary>
        /// 지정된 행정동 아이디에 대한 지도 상의 지역 색깔 표시를 변경
        /// </summary>
        private bool ChangeRegionColorBySWRKindCode(string regionId, bool toSelect, string warnKindCode)
        {
            try
            {
                if (!BasisData.IsRegionDataLoaded())
                {
                    return false;
                }

                RegionProfile selectedProfile = null;
                selectedProfile = BasisData.FindRegion(regionId);

                if (selectedProfile == null)
                {
                    System.Console.WriteLine("[MainForm] ChangeRegionColor( 알 수 없는 행정동 ID [" + regionId + "] )");
                    return false;
                }
                else if (selectedProfile.KmlInfo == null || selectedProfile.KmlInfo.KmlObject == null)
                {
                    System.Console.WriteLine("[MainForm] ChangeRegionColor(KML 정보가 없는 행정동 ID [" + regionId + "])");
                    return false;
                }
                else
                {
                }

                EColorStyle warningColor = EColorStyle.RED;
                switch (warnKindCode)
                {
                    case "1": // 강풍
                        {
                            warningColor = EColorStyle.LIMEGREEN;
                        }
                        break;
                    case "2": // 호우
                        {
                            warningColor = EColorStyle.ROYALBLUE;
                        }
                        break;
                    case "3": // 한파
                        {
                            warningColor = EColorStyle.DODGERBLUE;
                        }
                        break;
                    case "4": // 건조
                        {
                            warningColor = EColorStyle.ORANGE;
                        }
                        break;
                    case "5": // 해일
                        {
                            warningColor = EColorStyle.PALEGOLDENROD;
                        }
                        break;
                    case "6": // 풍랑
                        {
                            warningColor = EColorStyle.MEDIUMTURQUOISE;
                        }
                        break;
                    case "7": // 태풍
                        {
                            warningColor = EColorStyle.BRIGHTRED;
                        }
                        break;
                    case "8": // 대설
                        {
                            warningColor = EColorStyle.DEEPPINK;
                        }
                        break;
                    case "9": // 황사
                        {
                            warningColor = EColorStyle.GOLD;
                        }
                        break;
                    case "12": // 폭염
                        {
                            warningColor = EColorStyle.PURPLE;
                        }
                        break;
                    default:
                        {
                            warningColor = EColorStyle.BLUE;
                        }
                        break;
                }

                ChangeRegionColor(selectedProfile, toSelect, warningColor);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] ChangeRegionColorBySWRKindCode (Exception: " + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] ChangeRegionColorBySWRKindCode( " + ex.ToString() + " )");
            }

            return true;
        }

        /// <summary>
        /// 지정된 행정동 아이디에 대한 지도 상의 지역 색깔 표시를 변경
        /// </summary>
        private bool ClearAllPlacemarkColor()
        {
            try
            {
                if (!BasisData.IsRegionDataLoaded())
                {
                    return false;
                }

                foreach (RegionProfile region1 in BasisData.Regions.LstRegion.Values)
                {
                    // 현 발령대의 최상위 지역(중앙표준발령대: 전국, 시도표준발령대: 시도, 시군표준발령대: 시군)
                    ChangeRegionColor(region1, false);

                    if (region1.LstSubRegion == null || region1.LstSubRegion.Values.Count < 1)
                    {
                        continue;
                    }
                    foreach (RegionProfile region2 in region1.LstSubRegion.Values)
                    {
                        ChangeRegionColor(region2, false);

                        if (region2.LstSubRegion == null || region2.LstSubRegion.Values.Count < 1)
                        {
                            continue;
                        }
                        foreach (RegionProfile region3 in region2.LstSubRegion.Values)
                        {
                            ChangeRegionColor(region3, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] ClearAllPlacemarkColor (Exception: " + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] ClearAllPlacemarkColor( " + ex.ToString() + " )");
            }

            return true;
        }

        /// <summary>
        /// 지도 상의 행정동 선택을 행정동 지역명 리스트에 연동
        /// </summary>
        private void ApplyGisToList(string regionId, bool selected)
        {
            try
            {
                if (this.tviewRegionList.Nodes == null || this.tviewRegionList.Nodes.Count <= 0)
                {
                    System.Console.WriteLine("[MainForm] ApplyGisToList(행정동 리스트에 노드가 없습니다.)");
                    return;
                }

                // Node Level 1 : 시도
                TreeNode[] items = this.tviewRegionList.Nodes.Find(regionId, true);
                if (items == null || items.Count() != 1)
                {
                    System.Console.WriteLine("[MainForm] ApplyGisToList(행정동 리스트 검색 오류)");
                    return;
                }

                if (!string.IsNullOrEmpty(regionId))
                {
                    if (this.currentOrderProvisionInfo == null)
                    {
                        this.currentOrderProvisionInfo = new OrderProvisionInfo();
                    }
                    if (this.currentOrderProvisionInfo.TargetRegions == null)
                    {
                        this.currentOrderProvisionInfo.TargetRegions = new List<RegionDefinition>();
                    }

                    if (selected)
                    {
                        RegionProfile profile = BasisData.FindRegion(regionId);
                        if (profile != null)
                        {

                            RegionDefinition region = new RegionDefinition(profile.Code, profile.Name);
                            this.currentOrderProvisionInfo.TargetRegions.Add(region);
                        }
                    }
                    else
                    {
                        for (int index = 0; index < this.currentOrderProvisionInfo.TargetRegions.Count; index++)
                        {
                            if (regionId == this.currentOrderProvisionInfo.TargetRegions[index].Code.ToString())
                            {
                                this.currentOrderProvisionInfo.TargetRegions.RemoveAt(index);
                                break;
                            }
                        }
                    }
                }

                items[0].Checked = selected;
                // 발령 팝업 처리는 클릭 상태 통지 이벤트 내에서 처리
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] ApplyGisToList ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] ApplyGisToList( " + ex.ToString() + " )");
            }
        }
        #endregion

        #region 좌표기준반경선택 모드
        private int GetSelectedAreaCountFromList()
        {
            int count = 0;

            if (this.lvSelectedAreaList.Items != null)
            {
                count = this.lvSelectedAreaList.Items.Count;
            }

            return count;
        }
        /// <summary>
        /// [좌표 기준 반경 선택] 발령 대상 목록 초기화
        /// </summary>
        private void ClearSelectedArea()
        {
            if (this.lvSelectedAreaList.Items != null)
            {
                this.lvSelectedAreaList.Items.Clear();
            }

            adengGoogleEarthCtrl.ClearDragCircle();
        }
        private bool IsAlreadyAddedArea(RegionProfile region)
        {
            if (this.lvSelectedAreaList.Items == null)
            {
                return false;
            }
            if (this.lvSelectedAreaList.Items.Count <= 0)
            {
                return false;
            }

            bool isExist = false;
            foreach (AdengListViewItem item in this.lvSelectedAreaList.Items)
            {
                if (item.Name == region.Code)
                {
                    isExist = true;
                    break;
                }
            }

            return isExist;
        }
        private void AddSelectedArea(RegionProfile region, string fullName)
        {
            if (IsAlreadyAddedArea(region))
            {
                return;
            }

            int number = this.lvSelectedAreaList.Items.Count + 1;
            AdengListViewItem newItem = this.lvSelectedAreaList.Items.Add(number.ToString());
            newItem.SubItems.Add(fullName);

            newItem.Name = region.Code;
            newItem.Tag = region;
        }
        private bool AreaExtractionInCircle(IKmlPlacemark circlePlacemark)
        {
            if (!BasisData.IsRegionDataLoaded())
            {
                return false;
            }

            foreach (RegionProfile region1 in BasisData.Regions.LstRegion.Values)
            {
                // 현 발령대의 최상위 지역(중앙표준발령대: 전국, 시도표준발령대: 시도, 시군표준발령대: 시군)
                if (this.currentOuterLineVisibleMode == OuterLineVisibleMode.High)
                {
                    if (CheckAreaCrash(circlePlacemark, region1))
                    {
                        // 행정동 추가
                        string fullName = region1.Name;
                        AddSelectedArea(region1, fullName);
                    }
                }

                if (region1.LstSubRegion == null || region1.LstSubRegion.Values.Count < 1)
                {
                    continue;
                }
                foreach (RegionProfile region2 in region1.LstSubRegion.Values)
                {
                    if (this.currentOuterLineVisibleMode == OuterLineVisibleMode.Middle)
                    {
                        if (CheckAreaCrash(circlePlacemark, region2))
                        {
                            // 행정동 추가
                            string fullName = region2.Name;
                            AddSelectedArea(region2, fullName);
                        }
                    }

                    if (this.currentOuterLineVisibleMode != OuterLineVisibleMode.Low)
                    {
                        continue;
                    }
                    if (region2.LstSubRegion == null || region2.LstSubRegion.Values.Count < 1)
                    {
                        continue;
                    }
                    foreach (RegionProfile region3 in region2.LstSubRegion.Values)
                    {
                        if (CheckAreaCrash(circlePlacemark, region3))
                        {
                            // 행정동 추가
                            string fullName = region2.Name + " " + region3.Name;
                            AddSelectedArea(region3, fullName);
                        }
                    }
                }
            }

            return true;
        }
        /// <summary>
        /// [좌표 기준 반경 선택] 지역 목록에서 선택한 아이템을 삭제.
        /// </summary>
        /// <param name="targetRegion"></param>
        private void DeleteFromSelectedArea(RegionProfile targetRegion)
        {
            if (this.lvSelectedAreaList.Items == null || this.lvSelectedAreaList.Items.Count < 1)
            {
                return;
            }
            if (this.currentOrderProvisionInfo == null || this.currentOrderProvisionInfo.TargetRegions == null)
            {
                FileLogManager.GetInstance().WriteLog("[MainForm] DeleteFromSelectedArea( 발령 준비 정보 데이터 오류 )");
                throw new Exception("[선택 지역 삭제] 발령 준비 정보 데이터 오류");
            }

            // 발령 준비 정보 갱신
            RegionDefinition target = new RegionDefinition(targetRegion.Code, targetRegion.Name);
            foreach (RegionDefinition region in this.currentOrderProvisionInfo.TargetRegions)
            {
                if (region.Code.ToString() == targetRegion.Code)
                {
                    this.currentOrderProvisionInfo.TargetRegions.Remove(region);
                    break;
                }
            }

            // 선택된 영역 리스트 갱신
            foreach (AdengListViewItem item in this.lvSelectedAreaList.Items)
            {
                if (item.Name == targetRegion.Code)
                {
                    this.lvSelectedAreaList.Items.Remove(item);
                    break;
                }
            }
            int count = 1;
            foreach (AdengListViewItem item in lvSelectedAreaList.Items)
            {
                item.SubItems[0].Text = count.ToString();
                count++;
            }

            // 발령창 체크.
            if (this.lvSelectedAreaList.Items.Count <= 0)
            {
                if (IsOrderWindowAlive())
                {
                    this.orderWindow.Close();
                    this.orderWindow.NotifyOrderProvisionResult -= new EventHandler<OrderPrepareEventArgs>(orderWindow_OnNotifyOrderPrepareResult);
                    this.orderWindow = null;
                }

                ClearSelectedArea();
                this.btnClearAreaSelection.ChkValue = false;
            }
        }
        /// <summary>
        /// [Delete] 키 대응. 선택 항목 삭제.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvSelectedAreaList_KeyDown(object sender, KeyEventArgs e)
        {
            //System.Console.WriteLine("[MainForm] lvSelectedAreaList_KeyDown (  )");

            if (e.KeyValue != (int)Keys.Delete || e.KeyCode != Keys.Delete)
            {
                return;
            }

            if (this.lvSelectedAreaList.SelectedItems == null)
            {
                FileLogManager.GetInstance().WriteLog("[MainForm] lvSelectedAreaList_KeyDown( 데이터 오류(1) 선택된 아이템의 정보를 찾을 수 없습니다. )");
                return;
            }

            RegionProfile selectedRegion = this.lvSelectedAreaList.SelectedItems[0].Tag as RegionProfile;
            if (selectedRegion == null)
            {
                FileLogManager.GetInstance().WriteLog("[MainForm] lvSelectedAreaList_KeyDown( 데이터 오류(2) 선택된 아이템의 정보를 찾을 수 없습니다. )");
                return;
            }

            DeleteFromSelectedArea(selectedRegion);
        }
        private void lvSelectedAreaList_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.IsInputKey = true;
            }
        }

        /// <summary>
        /// 반경 그리기 마우스 모드 선택
        /// </summary>
        private void btnUseCircleMode_Click(object sender, EventArgs e)
        {
            Button thisButton = sender as Button;

            if (this.currentMouseMode == MouseMode.Normal)
            {
                // 일반 모드 -> 반경 모드
                this.currentMouseMode = MouseMode.DrawCircle;

                this.lblUseCircleModeTitle.Font = new Font("굴림", 9.0f, FontStyle.Bold);

                adengGoogleEarthCtrl.StartDragCircle();
            }
            else
            {
                // 반경 모드 -> 일반 모드
                this.currentMouseMode = MouseMode.Normal;

                this.lblUseCircleModeTitle.Font = new Font("굴림", 9.0f, FontStyle.Regular);

                adengGoogleEarthCtrl.StopDragCircle();
            }
        }
        /// <summary>
        /// [적용] 버튼 클릭 이벤트 핸들러.
        /// 수동 입력된 좌표 값을 기준으로 원을 그린다.
        /// </summary>
        private void btnApplyDrawCircle_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.isGISLoadingCompleted)
                {
                    return;
                }

                // 마우스 커서 모양 변경
                this.Cursor = Cursors.WaitCursor;

                if (string.IsNullOrEmpty(this.txtboxLatitude.Text))
                {
                    MessageBox.Show("위도 좌표를 입력하세요.", "반경 표시 실패", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.btnApplyDrawCircle.ChkValue = false;
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtboxLongitude.Text))
                {
                    MessageBox.Show("경도 좌표를 입력하세요.", "반경 표시 실패", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.btnApplyDrawCircle.ChkValue = false;
                    return;
                }
                else if (string.IsNullOrEmpty(this.txtboxRadius.Text))
                {
                    MessageBox.Show("반경 값을 입력하세요.", "반경 표시 실패", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.btnApplyDrawCircle.ChkValue = false;
                    return;
                }
                else
                {
                    // 처리 계속
                }

                // 좌표 변환
                double latitude = double.MinValue;
                double longitude = double.MinValue;
                int radius = 0;

                if (!double.TryParse(this.txtboxLatitude.Text, out latitude))
                {
                    MessageBox.Show("위도 좌표가 유효하지 않습니다.", "반경 표시 실패", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.btnApplyDrawCircle.ChkValue = false;
                    return;
                }

                if (!double.TryParse(this.txtboxLongitude.Text, out longitude))
                {
                    MessageBox.Show("경도 좌표가 유효하지 않습니다.", "반경 표시 실패", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.btnApplyDrawCircle.ChkValue = false;
                    return;
                }

                if (!int.TryParse(this.txtboxRadius.Text, out radius))
                {
                    MessageBox.Show("반경 값이 유효하지 않습니다.", "반경 표시 실패", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.btnApplyDrawCircle.ChkValue = false;
                    return;
                }

                this.externalGE.MoveFlyTo(latitude, longitude, radius * 100 / 22, 1.5);
                adengGoogleEarthCtrl.SetCircleRadius(latitude, longitude, radius);

                this.btnApplyDrawCircle.ChkValue = false;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] btnApplyDrawCircle_Click( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] btnApplyDrawCircle_Click( " + ex.ToString() + " )");
            }
            finally
            {
                // 마우스 커서 모양 변경
                this.Cursor = Cursors.Default;
            }
        }
        /// <summary>
        /// [그리기 모드 실행] 레이블 클릭 이벤트 핸들러.
        /// 그리기 모드 실행 버튼 클릭 이벤트 수동 호출.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblUseCircleModeTitle_Click(object sender, EventArgs e)
        {
            bool chkState = this.btnUseCircleMode.ChkValue;

            this.btnUseCircleMode.PerformClick();
            this.btnUseCircleMode.ChkValue = !chkState;
        }
        #endregion

        #region 시스템 개별 선택 모드
        /// <summary>
        /// 표준경보시스템 목록에서 발령 대상으로 선택된 항목의 수를 얻는다.
        /// </summary>
        /// <returns></returns>
        private int GetSelectedSystemCountFromList()
        {
            if (this.lvStdAlertSystemList.CheckedItems == null)
            {
                return 0;
            }

            return this.lvStdAlertSystemList.CheckedItems.Count;
        }
        /// <summary>
        /// 표준경보시스템 목록에서 입력 아이디에 해당하는 아이템에 체크를 넣는다.
        /// </summary>
        /// <param name="systemID"></param>
        private void SetCheckToSystemList(string systemID)
        {
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(systemID));

            if (this.lvStdAlertSystemList.Items == null ||
                this.lvStdAlertSystemList.Items.Count <= 0)
            {
                return;
            }

            foreach (ListViewItem item in this.lvStdAlertSystemList.Items)
            {
                if (item == null || item.Tag == null)
                {
                    continue;
                }
                if (item.Name == systemID)
                {
                    item.Checked = true;
                    break;
                }
            }
        }
        /// <summary>
        /// 표준경보시스템 선택 상태를 초기화.
        /// 표준경보시스템 목록 리스트와 지도 상의 선택 상태를 초기화 한다.
        /// </summary>
        private void ClearSelectedSystem()
        {
            this.btnAddSystemGroup.Enabled = true;

            if (this.lvStdAlertSystemList == null || 
                this.lvStdAlertSystemList.Items == null)
            {
                return;
            }

            // 모든 아이템의 체크 상태 해제
            int count = this.lvStdAlertSystemList.Items.Count;
            for (int index = 0; index < count; index++)
            {
                if (this.lvStdAlertSystemList.Items[index].Checked)
                {
                    SASProfile system = this.lvStdAlertSystemList.Items[index].Tag as SASProfile;
                    if (system == null)
                    {
                        continue;
                    }
                    ChangeSystemColor(system.ID, false);
                }
                this.lvStdAlertSystemList.Items[index].Checked = false;
            }
        }
        /// <summary>
        /// 표준경보시스템 그룹의 선택 상태 초기화.
        /// </summary>
        private void ClearSelectedSystemGroup()
        {
            this.btnAddSystemGroup.Enabled = true;

            if (this.lvSystemGroupList.Items == null)
            {
                return;
            }
            foreach (ListViewItem item in this.lvSystemGroupList.Items)
            {
                item.Checked = false;
            }
        }
        /// <summary>
        /// 표준경보시스템 아이콘의 색깔을 변경
        /// </summary>
        /// <param name="systemIp"></param>
        /// <param name="selectionStatus"></param>
        private void ChangeSystemColor(string systemID, bool selectionStatus)
        {
            if (this.currentDicSystemInfo == null || this.currentDicSystemInfo.Count <= 0)
            {
                System.Console.WriteLine("[MainForm] UpdateSystemIconOfGE(표준경보시스템 정보가 존재하지 않습니다.!!!)");
                return;
            }

            foreach (SASInfo info in this.currentDicSystemInfo.Values)
            {
                if (info == null || info.Profile == null || info.Placemark == null)
                {
                    continue;
                }

                if (info.Profile.ID == systemID)
                {
                    ChangeSystemIconColor(info.Placemark, info.Profile.KindCode, selectionStatus);
                    break;
                }
            }
        }
        /// <summary>
        /// 지정한 프로필의 시스템 아이콘의 강조 표시/해제
        /// </summary>
        /// <param name="target"></param>
        /// <param name="systemKindCode"></param>
        /// <param name="highlight"></param>
        /// <returns></returns>
        private bool ChangeSystemIconColor(IKmlPlacemark target, string systemKindCode, bool highlight)
        {
            System.Diagnostics.Debug.Assert(target != null);
            System.Diagnostics.Debug.Assert(systemKindCode != null);

            try
            {
                if (!this.isGISLoadingCompleted)
                {
                    return false;
                }

                string iconUrl = GetSystemIconUrl(systemKindCode, highlight);
                if (string.IsNullOrEmpty(iconUrl))
                {
                    System.Console.WriteLine("[MainForm] UpdateSystemIconOfGE(아이콘 링크 정보가 올바르지 않습니다. systemKindCode=[" + systemKindCode + "])");
                    return false;
                }

                this.externalGE.SetIconUrl(target, iconUrl, false);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] ChangeSystemIconColor( Exception=[" + ex.ToString() + "] )");
                FileLogManager.GetInstance().WriteLog("[MainForm] ChangeSystemIconColor( " + ex.ToString() + " )");

                return false;
            }

            return true;
        }

        /// <summary>
        /// [시스템 개별 선택] 표준경보시스템 목록 체크 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvStdAlertSystemList_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            ListView ctrl = sender as ListView;

            if (ctrl == null || ctrl.CheckedItems == null)
            {
                return;
            }

            if (!ctrl.Focused)
            {
                return;
            }

            SASProfile profile = e.Item.Tag as SASProfile;
            if (profile != null)
            {
                ChangeSystemColor(profile.ID, e.Item.Checked);
            }

            if (this.currentOrderProvisionInfo == null)
            {
                this.currentOrderProvisionInfo = new OrderProvisionInfo();
            }
            if (this.currentOrderProvisionInfo.TargetSystems == null)
            {
                this.currentOrderProvisionInfo.TargetSystems = new List<SASProfile>();
            }

            if (e.Item.Checked)
            {
                if (profile != null)
                {
                    this.currentOrderProvisionInfo.TargetSystems.Add(profile);
                }
            }
            else
            {
                for (int index = 0; index < this.currentOrderProvisionInfo.TargetSystems.Count; index++)
                {
                    if (e.Item.Name == this.currentOrderProvisionInfo.TargetSystems[index].Name)
                    {
                        this.currentOrderProvisionInfo.TargetSystems.RemoveAt(index);
                        break;
                    }
                }
            }

            // 모든 아이템의 체크 상태 확인
            int count = this.lvStdAlertSystemList.CheckedItems.Count;
            if (count > 0)
            {
                this.btnAddSystemGroup.Enabled = false;

                this.currentOrderProvisionInfo.Scope = CAPLib.ScopeType.Private;
                ShowSimpleOrderWindow();
            }
            else
            {
                // 선택한 지역이 한 개도 없으므로, 발령 팝업 닫기
                if (IsOrderWindowAlive())
                {
                    this.orderWindow.Close();
                    this.orderWindow.NotifyOrderProvisionResult -= new EventHandler<OrderPrepareEventArgs>(orderWindow_OnNotifyOrderPrepareResult);
                    this.orderWindow = null;
                }

                this.btnAddSystemGroup.Enabled = true;
            }

        }
        /// <summary>
        /// [시스템 그룹 선택] 표준경보시스템 그룹 목록 체크 이벤트 핸들러(체크 전 처리)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvSystemGroupList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (IsOrderWindowAlive())
            {
                // 발령 준비 중에는 그룹 선택 변경 불가.
                e.NewValue = CheckState.Unchecked;
                return;
            }
        }

        /// <summary>
        /// [시스템 그룹 선택] 표준경보시스템 그룹 목록 체크 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvSystemGroupList_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            // 그룹 아이템에 체크를 넣으면 지도 상에서 해당 시스템이 선택되어 나타나야 함.
            GroupProfile selectedItem = e.Item.Tag as GroupProfile;
            if (selectedItem == null)
            {
                System.Console.WriteLine("[MainForm] lvSystemGroupList_ItemChecked (리스트 아이템 데이터 오류");
                return;
            }

            // 상호 호출 무한 루프를 막기 위해 포커스가 현 컨트롤에 있을 때만 다른 컨트롤 연계
            if (!this.lvSystemGroupList.Focused || 
                this.currentDicSystemInfo == null || this.currentDicSystemInfo.Values == null)
            {
                return;
            }

            if (e.Item.Checked)
            {
                // 발령 준비 정보에 반영
                this.currentOrderProvisionInfo = ConvertToOrderProvisionInfo(selectedItem);
                if (this.currentOrderProvisionInfo == null)
                {
                    MessageBox.Show("현재의 그룹에 등록된 대상 표준경보시스템을 식별할 수 없습니다.\n" +
                                "이미 삭제된 표준경보시스템일 가능성이 있습니다.\n" +
                                "그룹 정보를 확인하고 발령 대상 표준경보시스템을 재설정해 주십시오.",
                                "발령 준비 오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                    e.Item.Checked = false;
                    return;
                }

                // 지도 표시 갱신
                foreach (string systemID in selectedItem.Targets)
                {
                    SetCheckToSystemList(systemID);
                    ChangeSystemColor(systemID, true);
                }
            }
            else
            {
                // 그룹 발령 시에는 모달 팝업이므로, 이 조건에 들어오는 경우는 발령 정보 리셋의 경우 뿐이다.
                // 개별 선택 중에 그룹을 선택하게 되면, 단지 정보만 참조하므로 해야할 작업은 없다.
            }

            //
            // 발령 팝업 띄우기
            //
            if (e.Item.Checked || GetSelectedSystemCountFromList() > 0)
            {
                this.btnAddSystemGroup.Enabled = false;

                this.currentOrderProvisionInfo.Scope = CAPLib.ScopeType.Private;
                ShowSimpleOrderWindow(true);

                this.btnAddSystemGroup.Enabled = true;
            }
            else
            {
                // 선택한 지역이 한 개도 없으므로, 발령 팝업 닫기
                if (IsOrderWindowAlive())
                {
                    this.orderWindow.Close();
                    this.orderWindow.NotifyOrderProvisionResult -= new EventHandler<OrderPrepareEventArgs>(orderWindow_OnNotifyOrderPrepareResult);
                    this.orderWindow = null;
                }

                this.btnAddSystemGroup.Enabled = true;
            }
        }
        /// <summary>
        /// [시스템 그룹 선택] 표준경보시스템 그룹 목록 더블클릭 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvSystemGroupList_DoubleClick(object sender, EventArgs e)
        {
            ListView view = sender as ListView;
            if (view == null)
            {
                return;
            }
            if (view.SelectedItems == null || view.SelectedItems.Count != 1)
            {
                return;
            }
            GroupProfile selectedItem = view.SelectedItems[0].Tag as GroupProfile;
            if (selectedItem == null)
            {
                System.Console.WriteLine("[MainForm] lvSystemGroupList_DoubleClick (리스트 아이템 데이터 오류");
                return;
            }

            EditSystemGroupForm editGroupForm = new EditSystemGroupForm(selectedItem);
            editGroupForm.SetSASInfo(this.currentDicSystemInfo);

            editGroupForm.NotifyUpdateGroupProfile += new EventHandler<UpdateGroupProfileEventArgs>(groupForm_OnNotifyUpdateGroupProfile);
            DialogResult result = editGroupForm.ShowDialog(this);
            editGroupForm.NotifyUpdateGroupProfile -= new EventHandler<UpdateGroupProfileEventArgs>(groupForm_OnNotifyUpdateGroupProfile);
        }
        /// <summary>
        /// [시스템 그룹 추가] 버튼 클릭 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddSystemGroup_Click(object sender, EventArgs e)
        {
            CreateSystemGroupForm createGroupForm = new CreateSystemGroupForm();
            createGroupForm.SetSASInfo(this.currentDicSystemInfo);

            createGroupForm.NotifyUpdateGroupProfile += new EventHandler<UpdateGroupProfileEventArgs>(groupForm_OnNotifyUpdateGroupProfile);
            createGroupForm.ShowDialog(this);
            createGroupForm.NotifyUpdateGroupProfile -= new EventHandler<UpdateGroupProfileEventArgs>(groupForm_OnNotifyUpdateGroupProfile);
        }
        #endregion

        #region 모드 공통 기능
        /// <summary>
        /// [발령] 창 표시
        /// </summary>
        /// <param name="pointX">스크린 좌표 X</param>
        /// <param name="pointY">스크린 좌표 Y</param>
        private void ShowSimpleOrderWindow(bool isModal = false, bool setPosition = false, int pointX = 0, int pointY = 0)
        {
            if (!IsOrderWindowAlive())
            {
                this.orderWindow = new OrderForm(ref this.currentOrderProvisionInfo);
                this.orderWindow.NotifyOrderProvisionResult += new EventHandler<OrderPrepareEventArgs>(orderWindow_OnNotifyOrderPrepareResult);

                if (setPosition)
                {
                    this.orderWindow.StartPosition = FormStartPosition.Manual;
                    this.orderWindow.Location = new Point(pointX, pointY);
                }
                else
                {
                    this.orderWindow.StartPosition = FormStartPosition.CenterParent;

                    this.orderWindow.StartPosition = FormStartPosition.Manual;
                    this.orderWindow.Location = new Point(this.Location.X + (this.Width - this.orderWindow.Width) / 2, this.Location.Y + (this.Height - this.orderWindow.Height) / 2);
                }

                if (isModal)
                {
                    this.orderWindow.ShowDialog(this);
                    this.orderWindow.NotifyOrderProvisionResult -= new EventHandler<OrderPrepareEventArgs>(orderWindow_OnNotifyOrderPrepareResult);
                    this.orderWindow.Dispose();
                    this.orderWindow = null;
                }
                else
                {
                    this.orderWindow.Show(this);
                }
            }
            else if (!this.orderWindow.Visible)
            {
                if (setPosition)
                {
                    this.orderWindow.StartPosition = FormStartPosition.Manual;
                    this.orderWindow.Location = new Point(pointX, pointY);
                }
                else
                {
                    // 포커스 이동 할까?
                }
                this.orderWindow.Visible = true;
                this.orderWindow.WindowState = FormWindowState.Normal;
            }
            else
            {
                // 반경으로 발령 대상 선택 모드 + 반경 그리기 마우스 모드
                if (this.targetingMode == TargetingMode.Radius && this.currentMouseMode == MouseMode.DrawCircle)
                {
                    // 일단 매번 클릭한 위치에 팝업을 이동시켜 보자.
                    if (setPosition)
                    {
                        this.orderWindow.StartPosition = FormStartPosition.Manual;
                        this.orderWindow.Location = new Point(pointX, pointY);
                    }
                }
            }

            this.btnAddRegionGroup.Enabled = false;
            this.btnAddSystemGroup.Enabled = false;
        }
        /// <summary>
        /// 발령 대상의 초기화
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearTargetSelection_Click(object sender, EventArgs e)
        {
            bool isOrderWindowAlive = IsOrderWindowAlive();

            if (!isOrderWindowAlive)
            {
                if (this.targetingMode == TargetingMode.Region)
                {
                    ClearSelectedRegion();
                    ClearSelectedRegionGroup();
                }
                else if (this.targetingMode == TargetingMode.Radius)
                {
                    ClearSelectedArea();
                }
                else if (this.targetingMode == TargetingMode.System)
                {
                    ClearSelectedSystem();
                    ClearSelectedSystemGroup();
                }
                else
                {
                }

                AdengImgButtonEx btn = sender as AdengImgButtonEx;
                if (btn != null)
                {
                    btn.ChkValue = false;
                }
                return;
            }
            else
            {
                // 발령창이 떠 있는 상태라면, 일단 최소화
                this.orderWindow.WindowState = FormWindowState.Minimized;
            }

            DialogResult result = MessageBox.Show("모든 발령 설정이 초기화됩니다. 취소하시겠습니까?", "발령 준비 취소", MessageBoxButtons.YesNo);

            this.btnClearRegionSelection.ChkValue = false;
            this.btnClearAreaSelection.ChkValue = false;
            this.btnClearSystemSelection.ChkValue = false;

            if (result != System.Windows.Forms.DialogResult.Yes)
            {
                // 발령창 닫는 동작 취소
                if (isOrderWindowAlive)
                {
                    this.orderWindow.WindowState = FormWindowState.Normal;
                }

                return;
            }

            if (isOrderWindowAlive)
            {
                this.orderWindow.Close();
                this.orderWindow.NotifyOrderProvisionResult -= new EventHandler<OrderPrepareEventArgs>(orderWindow_OnNotifyOrderPrepareResult);
                this.orderWindow = null;
            }

            if (this.targetingMode == TargetingMode.Region)
            {
                ClearSelectedRegion();
                ClearSelectedRegionGroup();

                this.btnAddRegionGroup.Enabled = true;
            }
            else if (this.targetingMode == TargetingMode.Radius)
            {
                ClearSelectedArea();
            }
            else if (this.targetingMode == TargetingMode.System)
            {
                ClearSelectedSystem();
                ClearSelectedSystemGroup();

                this.btnAddSystemGroup.Enabled = true;
            }
            else
            {
            }
        }

        /// <summary>
        /// 리스트 접기/펴기 버튼 클릭 시의 처리(공통)
        /// </summary>
        /// <param name="geObject"></param>
        private void btnTargetListRoll_Click(object sender, EventArgs e)
        {
            AdengImgButtonEx button = sender as AdengImgButtonEx;
            Panel currentPanel = null;

            if (this.targetingMode == TargetingMode.Region)
            {
                currentPanel = this.pnlTargetRegionBody;
            }
            else if (this.targetingMode == TargetingMode.Radius)
            {
                currentPanel = this.pnlTargetAreaBody;
            }
            else if (this.targetingMode == TargetingMode.System)
            {
                currentPanel = this.pnlTargetSystemBody;
            }
            else
            {
                // 해당사항 없음.
                return;
            }
            currentPanel.Visible = !button.ChkValue;
        }

        /// <summary>
        /// 지정한 지역 코드에 해당하는 곳으로 이동
        /// </summary>
        /// <param name="regionCode"></param>
        private void JumpToSpecifiedRegion(RegionProfile targetRegion)
        {
            try
            {
                if (!this.isGISLoadingCompleted)
                {
                    return;
                }

                // 이동
                if (targetRegion != null && targetRegion.KmlInfo != null)
                {
                    double latitude = targetRegion.KmlInfo.Latitude;
                    double longitude = targetRegion.KmlInfo.Longitude;
                    double altitude = targetRegion.KmlInfo.Altitude;
                    double tilt = targetRegion.KmlInfo.Tilt;
                    double range = targetRegion.KmlInfo.Range;

                    if (latitude > double.MinValue && longitude > double.MinValue)
                    {
                        this.externalGE.MoveFlyTo(latitude, longitude, altitude, tilt, range, 1);

                        this.txtboxLatitude.Text = latitude.ToString().Substring(0, 10);
                        this.txtboxLongitude.Text = longitude.ToString().Substring(0, 10);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] JumpToSpecifiedRegion( Exception=[" + ex.ToString() + "] )");
                FileLogManager.GetInstance().WriteLog("[MainForm] JumpToSpecifiedRegion( " + ex.ToString() + " )");

                return;
            }
        }
        #endregion

        #region 빠른지역이동
        /// <summary>
        /// [빠른 지역 이동] 상위 지역 단위 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbboxJumpToRegionLevel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbbox = sender as ComboBox;

            this.cmbboxJumpToRegionLevel2.ResetText();
            if (this.cmbboxJumpToRegionLevel2.Items != null)
            {
                this.cmbboxJumpToRegionLevel2.Items.Clear();
            }

            RegionProfile currentRegion = cmbbox.SelectedItem as RegionProfile;
            if (currentRegion == null)
            {
                return;
            }

            // 경계선 표시 모드 변경
            ExchangeOuterLineVisibleMode(this.currentOuterLineVisibleMode, OuterLineVisibleMode.Middle);

            foreach (RegionProfile subRegion in currentRegion.LstSubRegion.Values)
            {
                // 시군구 또는 읍면동
                int index = this.cmbboxJumpToRegionLevel2.Items.Add(subRegion);
            }

            JumpToSpecifiedRegion(currentRegion);
        }
        /// <summary>
        /// [빠른 지역 이동] 하위 지역 단위 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbboxJumpToRegionLevel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbbox = sender as ComboBox;

            RegionProfile currentRegion = cmbbox.SelectedItem as RegionProfile;
            if (currentRegion == null)
            {
                return;
            }

            // 경계선 표시 모드 변경
            ExchangeOuterLineVisibleMode(this.currentOuterLineVisibleMode, OuterLineVisibleMode.Low);

            JumpToSpecifiedRegion(currentRegion);
        }
        /// <summary>
        /// [빠른 지역 이동] 리스트 접기/펴기 버튼 클릭 시의 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnJumpToRegionListRoll_Click(object sender, EventArgs e)
        {
            AdengImgButtonEx button = sender as AdengImgButtonEx;

//            this.pnlJumpToRegion.Visible = !button.ChkValue;
            if (button.ChkValue)
            {
                this.pnlJumpToRegion.Size = new Size(220, 32);
            }
            else
            {
                this.pnlJumpToRegion.Size = new Size(220, 93);
            }
        }
        #endregion

        #region 최근발령
        /// <summary>
        /// 최근발령이력 목록 표시
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblLatestOrderSummary_Click(object sender, EventArgs e)
        {
            List<OrderRecord> orderList = this.core.GetRecentlyOrderList();

            if (this.recentlyOrderForm == null)
            {
                this.recentlyOrderForm = new RecentlyOrderHistoryForm(orderList);
                this.recentlyOrderForm.NotifyRequestCancelOrder += new EventHandler<RequestCancelOrderEventArgs>(recentlyOrderHistoryForm_OnNotifyRequestCancelOrder);
                this.core.NotifyOrderResponseUpdated += new EventHandler<OrderResponseEventArgs>(this.recentlyOrderForm.OnNotifyOrderResponseUpdated);
                this.recentlyOrderForm.ShowDialog(this);
                this.core.NotifyOrderResponseUpdated -= new EventHandler<OrderResponseEventArgs>(this.recentlyOrderForm.OnNotifyOrderResponseUpdated);
                this.recentlyOrderForm.NotifyRequestCancelOrder -= new EventHandler<RequestCancelOrderEventArgs>(recentlyOrderHistoryForm_OnNotifyRequestCancelOrder);
            }
            if (recentlyOrderForm != null && !recentlyOrderForm.Disposing)
            {
                this.recentlyOrderForm.Dispose();
            }
            this.recentlyOrderForm = null;
        }
        #endregion

        #region 기상특보
        private void btnMainMenuSWR_Click(object sender, EventArgs e)
        {
            this.btnMainMenuSWR.ChkValue = true;

            List<SWRProfile> waitingList = null;
            int result = this.core.GetWaitToOrderSWRList(out waitingList);
            if (result != 0)
            {
                return;
            }

            ShowWaitToOrderListForm(waitingList);
        }

        private void UpdateSWRList(List<SWRProfile> profileList = null)
        {
            if (!this.isUseSWRAssociation)
            {
                this.btnMainMenuSWR.Enabled = false;
                this.btnMainMenuSWR.ChkValue = false;

                if (this.currentSWRInfo != null)
                {
                    this.currentSWRInfo.Clear();
                }

                return;
            }

            List<SWRProfile> swrList = null;
            if (profileList == null)
            {
                int result = this.core.GetWaitToOrderSWRList(out swrList);
                if (result != 0)
                {
                    System.Console.WriteLine("[MainForm] UpdateSWRList ( 기상특보 데이터 조회 처리 중 에러가 발생하였습니다. )\nerror=[" + result + "] )");

                    this.btnMainMenuSWR.Enabled = false;
                    this.btnMainMenuSWR.ChkValue = false;

                    if (this.currentSWRInfo != null)
                    {
                        this.currentSWRInfo.Clear();
                    }
                    return;
                }
            }
            else
            {
                swrList = profileList;
            }

            if (this.currentSWRInfo != null)
            {
                this.currentSWRInfo.Clear();
            }
            if (swrList != null && swrList.Count > 0)
            {
                this.btnMainMenuSWR.Enabled = true;
                this.btnMainMenuSWR.ChkValue = true;

                foreach (SWRProfile profile in swrList)
                {
                    SWRProfile copy = new SWRProfile();
                    copy.DeepCopyFrom(profile);
                    this.currentSWRInfo.Add(copy.ID, copy);
                }
            }
            else
            {
                this.btnMainMenuSWR.Enabled = false;
                this.btnMainMenuSWR.ChkValue = false;
            }
        }
        private void ShowWaitToOrderListForm(List<SWRProfile> profileList = null)
        {
            if (this.waitToOrderSwrForm == null || this.waitToOrderSwrForm.IsDisposed)
            {
                if (profileList == null)
                {
                    if (this.currentSWRInfo != null && this.currentSWRInfo.Values != null)
                    {
                        profileList = this.currentSWRInfo.Values.ToList();
                    }
                }

                this.waitToOrderSwrForm = new WaitToOrderSWRForm(profileList);
                this.waitToOrderSwrForm.NotifyUpdateSWRAssociationState += new EventHandler<UpdateSWRAssociationStateEventArgs>(waitToOrderSwrForm_OnNotifyUpdateSWRAssociationState);
                this.waitToOrderSwrForm.NotifyUpdateSWRAreaVisible += new EventHandler<UpdateSWRAreaVisibleEventArgs>(waitToOrderSwrForm_OnNotifyUpdateSWRAreaVisible);
                this.waitToOrderSwrForm.ShowDialog(this);
                this.waitToOrderSwrForm.NotifyUpdateSWRAssociationState -= new EventHandler<UpdateSWRAssociationStateEventArgs>(waitToOrderSwrForm_OnNotifyUpdateSWRAssociationState);
                this.waitToOrderSwrForm.NotifyUpdateSWRAreaVisible -= new EventHandler<UpdateSWRAreaVisibleEventArgs>(waitToOrderSwrForm_OnNotifyUpdateSWRAreaVisible);
                this.waitToOrderSwrForm.Dispose();
                this.waitToOrderSwrForm = null;
            }
            else
            {
                this.waitToOrderSwrForm.WindowState = FormWindowState.Normal;
            }

            UpdateSWRList();
        }
        #endregion

        #region 경보해제
        private void btnMainMenuClearAlert_Click(object sender, EventArgs e)
        {
            this.btnMainMenuClearAlert.ChkValue = true;

            List<OrderRecord> waitingList = this.core.GetWaitToClearAlertList();

            ClearAlertWaitingListForm clearAlertWaitingListForm = new ClearAlertWaitingListForm(waitingList);
            clearAlertWaitingListForm.NotifyUpdateClearAlertState += new EventHandler<UpdateClearAlertStateEventArgs>(clearAlertWaitingListForm_OnNotifyUpdateClearAlertState);
            clearAlertWaitingListForm.ShowDialog(this);
            clearAlertWaitingListForm.NotifyUpdateClearAlertState -= new EventHandler<UpdateClearAlertStateEventArgs>(clearAlertWaitingListForm_OnNotifyUpdateClearAlertState);
        }
        private void UpdateClearAlertInfo()
        {
            this.btnMainMenuClearAlert.Enabled = false;
            this.btnMainMenuClearAlert.ChkValue = false;

            List<OrderRecord> waitingList = this.core.GetWaitToClearAlertList();
            if (waitingList != null && waitingList.Count > 0)
            {
                this.btnMainMenuClearAlert.Enabled = true;
                this.btnMainMenuClearAlert.ChkValue = true;
            }
        }
        #endregion

        #region 이벤트 핸들러
        /// <summary>
        /// 접속 인증 결과 통지 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void core_OnNotifyIAGWConnectionState(object sender, IAGWConnectionEventArgs e)
        {
            try
            {
                MethodInvoker invoker = delegate()
                {
                    //Debug.WriteLine("[MainForm] : auth(" + e.IsAuthenticated.ToString() + "), conn(" + e.IsConnected.ToString() + ")");

                    // 인증 상태
                    if (this.isAuthenticated != e.IsAuthenticated)
                    {
                        EventLogManager.GetInstance().WriteLog(EventLogEntryType.Error, "통합경보게이트웨이 TCP 통신( " + this.toolStripStatusLabelAuth.Text + ")");
                    }
                    this.isAuthenticated = e.IsAuthenticated;
                    if (e.IsAuthenticated)
                    {
                        this.toolStripStatusLabelAuth.ForeColor = Color.Blue;
                        this.toolStripStatusLabelAuth.Text = "(인증 코드 승인)";
                    }
                    else
                    {
                        this.toolStripStatusLabelAuth.ForeColor = Color.Red;
                        this.toolStripStatusLabelAuth.Text = "(인증 코드 오류)";
                    }

                    // 연결 상태
                    if (this.isConnectedWithIAGW != e.IsConnected)
                    {
                        string status = (e.IsConnected ? "연결" : "종료");
                        EventLogManager.GetInstance().WriteLog("통합경보게이트웨이 TCP 통신 " + status);
                    }
                    this.isConnectedWithIAGW = e.IsConnected;
                    int connectStateImageIndex = (e.IsConnected ? 0 : 1);
                    this.toolStripStatusLabelGWComm.Image = this.imgListCommunicationState.Images[connectStateImageIndex + 2];

                };

                if (this.InvokeRequired)
                {
                    Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] core_OnNotifyIAGWConnectionState ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] core_OnNotifyIAGWConnectionState( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// 표준경보시스템 프로필 갱신 통지 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void core_OnNotifySASProfileUpdated(object sender, SASProfileUpdateEventArgs e)
        {
            try
            {
                MethodInvoker invoker = delegate()
                {
                    System.Console.WriteLine("[MainForm] : 표준경보시스템 프로필 갱신 통지 수신)");

                    DeleteAllSystemPlacemark();

                    this.currentDicSystemInfo.Clear();
                    this.currentDicSystemInfo = null;

                    UpdateSystemList(true);
                    LoadSystemPlacemark();
                };

                if (this.InvokeRequired)
                {
                    Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] core_OnNotifySASProfileUpdated ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] core_OnNotifySASProfileUpdated( " + ex.ToString() + " )");
            }
        }
        /// <summary>)
        /// 기상 특보 이력 갱신 통지 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void core_OnNotifySWRProfileUpdated(object sender, EventArgs e)
        {
            try
            {
                MethodInvoker invoker = delegate()
                {
                    System.Console.WriteLine("[MainForm] : 기상특보 프로필 갱신 통지 수신)");

                    if (!this.loginCompleted)
                    {
                        return;
                    }

                    List<SWRProfile> profileList = null;
                    int result = this.core.GetWaitToOrderSWRList(out profileList);
                    if (result != 0 || profileList == null)
                    {
                        UpdateSWRList(null);
                        return;
                    }

                    // 발령창이 표시 중이라면
                    if (IsOrderWindowAlive())
                    {
                        UpdateSWRList(profileList);
                        return;
                    }
                    // 미발령 기상특보목록 윈도우가 표시 중이라면
                    if (this.waitToOrderSwrForm != null && !this.waitToOrderSwrForm.IsDisposed)
                    {
                        UpdateSWRList(profileList);
                        this.waitToOrderSwrForm.UpdateSWRList(profileList);
                        return;
                    }

                    // 새로 추가된 아이템만 추출
                    List<SWRProfile> newProfileList = new List<SWRProfile>();
                    if (this.currentSWRInfo != null && this.currentSWRInfo.Count > 0)
                    {
                        foreach (SWRProfile newProfile in profileList)
                        {
                            if (this.currentSWRInfo.ContainsKey(newProfile.ID))
                            {
                                continue;
                            }
                            newProfileList.Add(newProfile);
                        }
                    }
                    else
                    {
                        newProfileList = profileList;
                    }

                    if (newProfileList.Count <= 0)
                    {
                        // 변화 없음
                    }
                    else if (newProfileList.Count == 1)
                    {
                        // 1건 추출 -> 발령창 표시
                        ExecuteSWROrder(newProfileList[0]);
                    }
                    else
                    {
                        ShowWaitToOrderListForm(profileList);
                    }

                    this.currentSWRInfo.Clear();
                    if (profileList != null)
                    {
                        foreach (SWRProfile profile in profileList)
                        {
                            SWRProfile copy = new SWRProfile();
                            copy.DeepCopyFrom(profile);

                            this.currentSWRInfo.Add(copy.ID, copy);
                        }
                    }

                    UpdateSWRList(profileList);
                };
                if (this.InvokeRequired)
                {
                    Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] core_OnNotifySWRProfileUpdated ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] core_OnNotifySWRProfileUpdated( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// 최근발령 이력 갱신 통지 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void core_OnNotifyLatestOrderInfoChanged(object sender, OrderEventArgs e)
        {
            try
            {
                MethodInvoker invoker = delegate()
                {
                    if (e.OrderInfo.OrderMode == CAPLib.StatusType.Exercise)
                    {
                        this.lblLatestOrderSummary.ForeColor = System.Drawing.Color.Orange;
                    }
                    else if (e.OrderInfo.OrderMode == CAPLib.StatusType.Test)
                    {
                        this.lblLatestOrderSummary.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        this.lblLatestOrderSummary.ForeColor = System.Drawing.Color.Red;
                    }
                    this.lblLatestOrderSummary.Text = e.Headline;

                };

                if (this.InvokeRequired)
                {
                    Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] core_OnNotifyLatestOrderInfoChanged ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] core_OnNotifyLatestOrderInfoChanged( " + ex.ToString() + " )");
            }
        }


        /// <summary>
        /// 로그인 결과 통지 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loginForm_OnNotifyLoginResult(object sender, LoginEventArgs e)
        {
            try
            {
                MethodInvoker invoker = delegate()
                {
                    if (e.IsLogin)
                    {
                        if (e.LoginUser == null)
                        {
                            BasisData.CurrentLoginUser = new UserAccount("ADMIN", "ADMIN", "관리자", "국민안전처", "02-2100-2114", "");
                        }
                        else
                        {
                            BasisData.CurrentLoginUser = e.LoginUser;
                        }
                    }
                };

                if (this.InvokeRequired)
                {
                    Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] loginForm_OnNotifyLoginResult ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] loginForm_OnNotifyLoginResult( " + ex.ToString() + " )");
            }
        }

        /// <summary>
        /// 발령 준비 정보 통지 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void orderWindow_OnNotifyOrderPrepareResult(object sender, OrderPrepareEventArgs args)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] orderWindow_OnNotifyOrderPrepareResult( start )");

            try
            {
                MethodInvoker invoker = delegate()
                {
                    // 발령
                    if (args.IsOrder)
                    {
                        FileLogManager.GetInstance().WriteLog("[MainForm] orderWindow_OnNotifyOrderPrepareResult( 발령 요청 )");

                        int result = this.core.SendOrder(args.OrderProvisionInfo);
                        if (result != 0)
                        {
                            MessageBox.Show("발령 처리 중에 오류가 발생하였습니다. ErrorCode2=[" + result + "]", "발령 처리 오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            FileLogManager.GetInstance().WriteLog("[MainForm] orderWindow_OnNotifyOrderPrepareResult( 발령 처리 오류. result=[" + result + "] )");
                        }
                    }
                    else
                    {
                        FileLogManager.GetInstance().WriteLog("[MainForm] orderWindow_OnNotifyOrderPrepareResult( 발령 준비 취소 )");
                    }

                    // 발령 설정 초기화 처리
                    if (this.targetingMode == TargetingMode.Region)
                    {
                        ClearSelectedRegion();
                        ClearSelectedRegionGroup();
                    }
                    else if (this.targetingMode == TargetingMode.Radius)
                    {
                        ClearSelectedArea();
                    }
                    else if (this.targetingMode == TargetingMode.System)
                    {
                        ClearSelectedSystem();
                        ClearSelectedSystemGroup();
                    }
                    else
                    {
                    }

                    if (this.currentOrderProvisionInfo.RefType == OrderReferenceType.SWR)
                    {
                        UpdateSWRList();
                    }
                    else if (this.currentOrderProvisionInfo.RefType == OrderReferenceType.Clear)
                    {
                        UpdateClearAlertInfo();
                    }
                    else
                    {
                        // 그 외에 발령 이후에 화면 갱신이 필요한 경우 추가.
                    }

                    // 발령 준비 정보 초기화
                    this.currentOrderProvisionInfo = null;
                    this.currentOrderProvisionInfo = new OrderProvisionInfo();
                };

                if (this.InvokeRequired)
                {
                    Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] orderWindow_OnNotifyOrderPrepareResult ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] orderWindow_OnNotifyOrderPrepareResult( " + ex.ToString() + " )");

                MessageBox.Show("발령 처리 중에 오류가 발생하였습니다. ErrorCode=[99]", "발령 처리 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                FileLogManager.GetInstance().WriteLog("[MainForm] orderWindow_OnNotifyOrderPrepareResult( end )");
            }
        }

        /// <summary>
        /// 발령 취소 요청 이벤트 핸들러.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void recentlyOrderHistoryForm_OnNotifyRequestCancelOrder(object sender, RequestCancelOrderEventArgs e)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] recentlyOrderHistoryForm_OnNotifyRequestCancelOrder( start )");

            try
            {
                MethodInvoker invoker = delegate()
                {
                    if (string.IsNullOrEmpty(e.OrderCAPID))
                    {
                        MessageBox.Show("발령 취소를 실패하였습니다. \n[아이디 참조 에러]", "발령 취소 실패", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        FileLogManager.GetInstance().WriteLog("[MainForm] recentlyOrderHistoryForm_OnNotifyRequestCancelOrder( 발령 정보 참조 에러(아이디) )");

                        return;
                    }

                    int result = this.core.CancelOrder(e.OrderCAPID, e.RequestedOrder);
                    if (result != 0)
                    {
                        MessageBox.Show("발령 취소를 실패하였습니다. \n(에러코드[" + result + "])", "발령 취소 실패", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        FileLogManager.GetInstance().WriteLog("[MainForm] recentlyOrderHistoryForm_OnNotifyRequestCancelOrder( 발령 취소 처리 실패. result=[" + result + "] )");

                        return;
                    }
                };

                if (this.InvokeRequired)
                {
                    Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] recentlyOrderHistoryForm_OnNotifyRequestCancelOrder ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] recentlyOrderHistoryForm_OnNotifyRequestCancelOrder( " + ex.ToString() + " )");
            }
            finally
            {
                FileLogManager.GetInstance().WriteLog("[MainForm] recentlyOrderHistoryForm_OnNotifyRequestCancelOrder( end )");
            }
        }

        /// <summary>
        /// 경보 해제 상태 갱신 요청 이벤트 핸들러.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearAlertWaitingListForm_OnNotifyUpdateClearAlertState(object sender, UpdateClearAlertStateEventArgs e)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] clearAlertWaitingListForm_OnNotifyUpdateClearAlertState( start )");

            try
            {
                MethodInvoker invoker = delegate()
                {
                    if (string.IsNullOrEmpty(e.TargetCAPID))
                    {
                        FileLogManager.GetInstance().WriteLog("[MainForm] clearAlertWaitingListForm_OnNotifyUpdateClearAlertState( 아이디 참조 에러 )");

                        if (e.ClearState == ClearAlertState.Exclude)
                        {
                            string message = "경보 해제 상태 정보를 갱신할 수 없습니다. \n[아이디 참조 에러]";
                            message = "경보 해제를 수행할 수 없습니다. \n[아이디 참조 에러]";

                            MessageBox.Show(message, "경보 해제", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        return;
                    }

                    int result = 0;
                    if (e.ClearState == ClearAlertState.Exclude)
                    {
                        FileLogManager.GetInstance().WriteLog("[MainForm] clearAlertWaitingListForm_OnNotifyUpdateClearAlertState( e.ClearState=제외 )");

                        result = this.core.UpdateClearAlertingState(e.TargetCAPID, e.ClearState);
                    }
                    else if (e.ClearState == ClearAlertState.Clear)
                    {
                        FileLogManager.GetInstance().WriteLog("[MainForm] clearAlertWaitingListForm_OnNotifyUpdateClearAlertState( e.ClearState=해제 )");

                        bool prepareResult = PrepareClearAlertOrderInfo(e.TargetCAPID, e.RequestedOrder);
                        if (prepareResult)
                        {
                            if (IsOrderWindowAlive())
                            {
                            }
                            else
                            {
                                this.btnAddRegionGroup.Enabled = false;
                                this.btnAddSystemGroup.Enabled = false;

                                ShowSimpleOrderWindow(true);

                                this.btnAddRegionGroup.Enabled = true;
                                this.btnAddSystemGroup.Enabled = true;
                            }
                        }
                    }
                    else
                    {
                        // 이외에는 Waiting(대기) 상태 뿐으로, 논리적으로 올 수 없음.
                    }

                    if (result != 0)
                    {
                        FileLogManager.GetInstance().WriteLog("[MainForm] clearAlertWaitingListForm_OnNotifyUpdateClearAlertState( 처리 실패. result=[" + result + "] )");

                        MessageBox.Show("경보 해제 상태 정보를 갱신할 수 없습니다. \n(에러코드[" + result + "])", "경보 해제 상태 갱신 실패", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                };

                if (this.InvokeRequired)
                {
                    Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] clearAlertWaitingListForm_OnNotifyUpdateClearAlertState ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] clearAlertWaitingListForm_OnNotifyUpdateClearAlertState( " + ex.ToString() + " )");
            }
            finally
            {
                FileLogManager.GetInstance().WriteLog("[MainForm] clearAlertWaitingListForm_OnNotifyUpdateClearAlertState( end )");
            }
        }

        /// <summary>
        /// 기상특보 발령 연계 상태 갱신 요청 이벤트 핸들러.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void waitToOrderSwrForm_OnNotifyUpdateSWRAssociationState(object sender, UpdateSWRAssociationStateEventArgs e)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] waitToOrderSwrForm_OnNotifyUpdateSWRAssociationState( start )");

            try
            {
                MethodInvoker invoker = delegate()
                {
                    if (string.IsNullOrEmpty(e.TargetReportID))
                    {
                        FileLogManager.GetInstance().WriteLog("[MainForm] waitToOrderSwrForm_OnNotifyUpdateSWRAssociationState( 아이디 참조 에러 )");

                        string message = "기상특보 발령 연계 상태 정보를 갱신할 수 없습니다. \n[아이디 참조 에러]";
                        MessageBox.Show(message, "기상특보 발령 연계 제외", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    int result = 0;
                    if (e.AssociationState == SWRAssociationStateCode.Exclude)
                    {
                        result = this.core.UpdateSWRAssociationState(e.TargetReportID, e.AssociationState);
                    }
                    else if (e.AssociationState == SWRAssociationStateCode.Order)
                    {
                        if (!IsOrderWindowAlive())
                        {
                            ExecuteSWROrder(e.RequestedReport);
                        }
                    }
                    else
                    {
                    }

                    if (result != 0)
                    {
                        FileLogManager.GetInstance().WriteLog("[MainForm] waitToOrderSwrForm_OnNotifyUpdateSWRAssociationState( 기상특보 발령 연계상태 갱신 실패. result=[" + result + "] )");

                        MessageBox.Show("기상특보 발령 연계 상태 정보를 갱신할 수 없습니다. \n(에러코드[" + result + "])", "기상특보 발령 연계 상태 갱신 실패", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                };

                if (this.InvokeRequired)
                {
                    Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] waitToOrderSwrForm_OnNotifyUpdateSWRAssociationState ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] waitToOrderSwrForm_OnNotifyUpdateSWRAssociationState( " + ex.ToString() + " )");
            }
            finally
            {
                FileLogManager.GetInstance().WriteLog("[MainForm] waitToOrderSwrForm_OnNotifyUpdateSWRAssociationState( end )");
            }
        }

        /// <summary>
        /// 기상특보 목록창에서 아이템 선택 시 지역 표시.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void waitToOrderSwrForm_OnNotifyUpdateSWRAreaVisible(object sender, UpdateSWRAreaVisibleEventArgs e)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] waitToOrderSwrForm_OnNotifyUpdateSWRAreaVisible( start )");

            try
            {
                MethodInvoker invoker = delegate()
                {
                    if (e.IsVisible && e.RequestedReport == null)
                    {
                        FileLogManager.GetInstance().WriteLog("[MainForm] waitToOrderSwrForm_OnNotifyUpdateSWRAreaVisible( 프로필 데이터 참조 에러 )");

                        string message = "기상특보 대상 지역을 표시할 수 없습니다. \n[프로필 데이터 참조 에러]";
                        MessageBox.Show(message, "기상특보 대상 지역 표시", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    // 선택 상태 리셋
                    bool changed = ClearAllPlacemarkColor();

                    if (e.IsVisible)
                    {
                        string[] targetRegionCodes = e.RequestedReport.GetTargetRegionCodes();
                        if (targetRegionCodes == null)
                        {
                            FileLogManager.GetInstance().WriteLog("[MainForm] waitToOrderSwrForm_OnNotifyUpdateSWRAreaVisible( 대상 지역 코드 정보 에러 )");

                            return;
                        }

                        foreach (string regionCode in targetRegionCodes)
                        {
                            bool isChanged = ChangeRegionColorBySWRKindCode(regionCode, e.IsVisible, e.RequestedReport.WarnKindCode);
                        }
                    }
                };

                if (this.InvokeRequired)
                {
                    Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] waitToOrderSwrForm_OnNotifyUpdateSWRAreaVisible ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] waitToOrderSwrForm_OnNotifyUpdateSWRAreaVisible( " + ex.ToString() + " )");
            }
            finally
            {
                FileLogManager.GetInstance().WriteLog("[MainForm] waitToOrderSwrForm_OnNotifyUpdateSWRAreaVisible( end )");
            }
        }

        /// <summary>
        /// 그룹 프로필 갱신 요청 이벤트 핸들러.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void groupForm_OnNotifyUpdateGroupProfile(object sender, UpdateGroupProfileEventArgs e)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] groupForm_OnNotifyUpdateGroupProfile( start )");

            try
            {
                MethodInvoker invoker = delegate()
                {
                    if (e.TargetProfile == null)
                    {
                        FileLogManager.GetInstance().WriteLog("[MainForm] groupForm_OnNotifyUpdateGroupProfile( 프로필 참조 에러 )");

                        string message = "그룹 프로필 정보 변경 요청을 수행할 수 없습니다. [프로필 참조 에러]";
                        MessageBox.Show(message, "그룹 프로필 정보 변경", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        return;
                    }

                    int result = 0;
                    if (e.UpdateMode == ProfileUpdateMode.Regist)
                    {
                        result = this.core.CreateGroup(e.TargetProfile);
                    }
                    else if (e.UpdateMode == ProfileUpdateMode.Modify)
                    {
                        result = this.core.UpdateGroupInfo(e.TargetProfile);
                    }
                    else if (e.UpdateMode == ProfileUpdateMode.Delete)
                    {
                        result = this.core.DeleteGroup(e.TargetProfile.GroupID);
                    }
                    else
                    {
                        // do nothing
                    }

                    if (this.targetingMode == TargetingMode.Region)
                    {
                        UpdateRegionGroupList();
                    }
                    else if (this.targetingMode == TargetingMode.System)
                    {
                        UpdateSystemGroupList();
                    }
                    else
                    {
                        // 반경 선택은 그룹이 없음.
                    }

                    if (result != 0)
                    {
                        FileLogManager.GetInstance().WriteLog("[MainForm] groupForm_OnNotifyUpdateGroupProfile( 처리 실패. e.UpdateMode=[" + e.UpdateMode + "], result=[" + result + "] )");

                        MessageBox.Show("그룹 프로필 정보 변경 요청이 실패하였습니다. \n(에러코드[" + result + "])", "그룹 프로필 정보 변경 실패", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                };

                if (this.InvokeRequired)
                {
                    Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] groupForm_OnNotifyUpdateGroupProfile ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] groupForm_OnNotifyUpdateGroupProfile( " + ex.ToString() + " )");
            }
            finally
            {
                FileLogManager.GetInstance().WriteLog("[MainForm] groupForm_OnNotifyUpdateGroupProfile( end )");
            }
        }


        /// <summary>
        /// 현재 시각 표시 폴링 메소드
        /// </summary>
        private void DisplayTimePolling()
        {
            System.Console.WriteLine("[MainForm] DisplayTimePolling()");
            FileLogManager.GetInstance().WriteLog("[MainForm] DisplayTimePolling()");

            try
            {
                while (this.isContinueDisplayTime)
                {
                    MethodInvoker setBtn = delegate()
                    {
                        //this.lblCurrentTime.Text = "[ " + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + " ]";
                        this.lblCurrentTime.Text = "[ " + DateTime.Now.ToString() + " ]";
                    };
                    if (this.InvokeRequired)
                    {
                        this.Invoke(setBtn);
                    }
                    else
                    {
                        setBtn();
                    }

                    Thread.Sleep(100);
                }
            }
            catch (ThreadAbortException exAbort)
            {
                System.Console.WriteLine("[MainForm] DisplayTimePolling( Exception=[ ThreadAbortException ] )");
                FileLogManager.GetInstance().WriteLog("[MainForm] DisplayTimePolling( Exception=[ " + exAbort.Message + " ] )");

                Thread.ResetAbort();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] DisplayTimePolling ( Exception Occured!!! - " + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] DisplayTimePolling( " + ex.ToString() + " )");
            }
        }

        /// <summary>
        /// [메뉴] 시스템 설정 관리의 통합경보게이트웨이 접속 연결/종료 테스트 처리.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void systemSettingForm_OnNotifyIAGWConnectionTest(object sender, IAGWConnectionTestEventArgs e)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] systemSettingForm_OnNotifyIAGWConnectionTest( start )");

            try
            {
                MethodInvoker invoker = delegate()
                {
                    if (e == null)
                    {
                        FileLogManager.GetInstance().WriteLog("[MainForm] systemSettingForm_OnNotifyIAGWConnectionTest( 입력 파라미터 에러 )");

                        string message = "데이터에 오류가 있어 연결 테스트를 수행할 수 없습니다.";
                        MessageBox.Show(message, "통합경보게이트웨이 연결 테스트", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        return;
                    }

                    bool result = false;
                    if (e.IsConnectState)
                    {
                        result = this.core.ConnectToGateway(e.IP, e.Port);
                    }
                    else
                    {
                        result = this.core.DisconnectFromGateway();
                    }
                };

                if (this.InvokeRequired)
                {
                    Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] systemSettingForm_OnNotifyIAGWConnectionTest ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] systemSettingForm_OnNotifyIAGWConnectionTest( " + ex.ToString() + " )");
            }
            finally
            {
                FileLogManager.GetInstance().WriteLog("[MainForm] systemSettingForm_OnNotifyIAGWConnectionTest( end )");
            }
        }
        /// <summary>
        /// [메뉴] 시스템 설정 관리의 데이터베이스 접속 [연결 테스트] 처리 이벤트 핸들러.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void systemSettingForm_OnNotifyDBConnectionTest(object sender, DBConnectionTestEventArgs e)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] systemSettingForm_OnNotifyDBConnectionTest( start )");

            try
            {
                MethodInvoker invoker = delegate()
                {
                    bool isOpen = this.core.TestOpenDB(e.DbInfo);
                    if (isOpen)
                    {
                        MessageBox.Show("데이터베이스에 성공적으로 접속 하였습니다.", "데이터베이스 접속 시험", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("데이터베이스에 접속할 수 없습니다.\n접속 설정 정보를 확인해 주십시오.", "데이터베이스 접속 시험", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                };

                if (this.InvokeRequired)
                {
                    Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] systemSettingForm_OnNotifyDBConnectionTest ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] systemSettingForm_OnNotifyDBConnectionTest( " + ex.ToString() + " )");
            }
            finally
            {
                FileLogManager.GetInstance().WriteLog("[MainForm] systemSettingForm_OnNotifyDBConnectionTest( end )");
            }
        }
        /// <summary>
        /// [메뉴] 시스템 설정 관리의 [설정 저장] 처리 이벤트.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void systemSettingForm_OnNotifyConfigSettingUpdate(object sender, ConfigSettingUpdateEventArgs e)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] systemSettingForm_OnNotifyConfigSettingUpdate( start )");

            try
            {
                MethodInvoker invoker = delegate()
                {
                    if (e.SettingInfo == null)
                    {
                        FileLogManager.GetInstance().WriteLog("[MainForm] systemSettingForm_OnNotifyConfigSettingUpdate( 입력 파라미터 오류 )");

                        MessageBox.Show("시스템 설정 정보 변경 중에 오류가 발생 하였습니다. [데이터 오류]", "시스템 설정 관리", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    ConfigData oldSetting = new ConfigData();
                    oldSetting.DeepCopyFrom(this.core.GetConfigInfo());

                    this.core.SetConfigInfo(e.SettingInfo);

                    ConfigData newSetting = new ConfigData();
                    SystemConnectionSettingForm senderWnd = sender as SystemConnectionSettingForm;
                    if (senderWnd != null)
                    {
                        newSetting.DeepCopyFrom(this.core.GetConfigInfo());
                        senderWnd.SetSystemSettingInfo(newSetting);
                    }

                    if (oldSetting.LOCAL.RegionCode != newSetting.LOCAL.RegionCode ||
                        oldSetting.DB.HostIP != newSetting.DB.HostIP ||
                        oldSetting.DB.ServiceID != newSetting.DB.ServiceID ||
                        oldSetting.DB.UserID != newSetting.DB.UserID ||
                        oldSetting.DB.UserPassword != newSetting.DB.UserPassword)
                    {
                        MessageBox.Show("시스템 설정이 변경되었습니다. 변경된 설정을 적용하려면 시스템을 재시작해야 합니다.",
                                        "시스템 설정 관리", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("시스템 설정이 저장되었습니다.",
                                        "시스템 설정 관리", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                };
                if (this.InvokeRequired)
                {
                    Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] systemSettingForm_OnNotifyConfigSettingUpdate ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] systemSettingForm_OnNotifyConfigSettingUpdate( " + ex.ToString() + " )");
            }
            finally
            {
                FileLogManager.GetInstance().WriteLog("[MainForm] systemSettingForm_OnNotifyConfigSettingUpdate( end )");
            }
        }

        /// <summary>
        /// [언어 설정] 갱신 처리.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void languageSettingForm_OnNotifyLanguageSettingUpdate(object sender, LanguageSettingUpdateEventArgs e)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] languageSettingForm_OnNotifyLanguageSettingUpdate( start )");

            try
            {
                MethodInvoker invoker = delegate()
                {
                    if (e.LanguageList != null)
                    {
                        int result = this.core.UpdateLanguageSetting(e.LanguageList);
                        if (result != 0)
                        {
                            FileLogManager.GetInstance().WriteLog("[MainForm] swrConditionSettingForm_OnNotifySWRAssociationConditionUpdate( 언어 설정 변경 처리 실패. result=[" + result + "] )");

                            MessageBox.Show("언어 설정 변경 처리 중에 오류가 발생하였습니다. ErrorCode=[" + result + "]", "언어 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }

                    MessageBox.Show("설정을 저장하였습니다.", "언어 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                if (this.InvokeRequired)
                {
                    Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] languageSettingForm_OnNotifyLanguageSettingUpdate ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] languageSettingForm_OnNotifyLanguageSettingUpdate( " + ex.ToString() + " )");
            }
            finally
            {
                FileLogManager.GetInstance().WriteLog("[MainForm] languageSettingForm_OnNotifyLanguageSettingUpdate( end )");
            }
        }

        /// <summary>
        /// [기상 특보 연계 조건 설정] 갱신 처리.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void swrConditionSettingForm_OnNotifySWRAssociationConditionUpdate(object sender, SWRConditionUpdateEventArgs e)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] swrConditionSettingForm_OnNotifySWRAssociationConditionUpdate( start )");

            try
            {
                MethodInvoker invoker = delegate()
                {
                    ConfigData configData = this.core.GetConfigInfo();
                    if (configData != null)
                    {
                        configData.SWR.UseService = e.UseAssociation;
                        this.core.SetConfigInfo(configData);
                    }

                    if (e.ConditionList != null)
                    {
                        int result = this.core.UpdateSWRAssociationCondition(e.ConditionList);
                        if (result != 0)
                        {
                            FileLogManager.GetInstance().WriteLog("[MainForm] swrConditionSettingForm_OnNotifySWRAssociationConditionUpdate( 기상특보 연계조건 변경 처리 실패. result=[" + result + "] )");

                            MessageBox.Show("기상 특보 연계 조건 변경 처리 중에 오류가 발생하였습니다. ErrorCode=[" + result + "]", "기상 특보 연계 조건 설정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }

                    // 메인 상단 기상특보 메뉴 버튼 갱신
                    this.btnMainMenuSWR.Visible = e.UseAssociation;

                    // 정보 갱신
                    this.isUseSWRAssociation = e.UseAssociation;
                    UpdateSWRList();

                    MessageBox.Show("설정을 저장하였습니다.", "기상 특보 연계 조건 설정", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                if (this.InvokeRequired)
                {
                    Invoke(invoker);
                }
                else
                {
                    invoker();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] swrConditionSettingForm_OnNotifySWRAssociationConditionUpdate ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] swrConditionSettingForm_OnNotifySWRAssociationConditionUpdate( " + ex.ToString() + " )");
            }
            finally
            {
                FileLogManager.GetInstance().WriteLog("[MainForm] swrConditionSettingForm_OnNotifySWRAssociationConditionUpdate( end )");
            }
        }
        #endregion

        #region 내부함수
        /// <summary>
        /// 발령창 표시 중인지 체크
        /// </summary>
        private bool IsOrderWindowAlive()
        {
            if (this.orderWindow == null || this.orderWindow.IsDisposed)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void ExecuteSWROrder(SWRProfile profile)
        {
            if (IsOrderWindowAlive())
            {
                return;
            }
            bool prepareResult = PrepareSWROrderInfo(profile.ID, profile);
            if (prepareResult)
            {
                // 지도 표시 갱신
                string[] targetRegions = profile.GetTargetRegionCodes();
                foreach (string regionCode in targetRegions)
                {
                    SetRegionCheck(regionCode);
                    ChangeRegionColor(regionCode, true);
                }

                this.btnAddRegionGroup.Enabled = false;
                this.btnAddSystemGroup.Enabled = false;

                ShowSimpleOrderWindow(false);
            }
        }
        private bool PrepareClearAlertOrderInfo(string targetCAPID, OrderRecord targetRecord)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] PrepareClearAlertOrderInfo( start )");

            try
            {
                if (string.IsNullOrEmpty(targetCAPID) || targetRecord == null)
                {
                    FileLogManager.GetInstance().WriteLog("[MainForm] PrepareClearAlertOrderInfo( 입력 파라미터 오류 )");
                    return false;
                }

                // 발령준비 정보 설정
                CAP capMsg = new CAP(targetRecord.CapText);
                CAPHelper helper = new CAPHelper();
                if (this.currentOrderProvisionInfo == null)
                {
                    this.currentOrderProvisionInfo = new OrderProvisionInfo();
                }

                this.currentOrderProvisionInfo.CAPData = null;
                // 발령모드
                this.currentOrderProvisionInfo.Mode = BasisData.FindOrderModeInfoByCode(targetRecord.OrderMode);
                if (this.currentOrderProvisionInfo.Mode == null)
                {
                    FileLogManager.GetInstance().WriteLog("[MainForm] PrepareClearAlertOrderInfo( 모드 정보 오류 )");
                    return false;
                }
                // 메시지 유형
                this.currentOrderProvisionInfo.MessageType = capMsg.MessageType.Value;  // MsgType.Alert ...
                this.currentOrderProvisionInfo.Scope = capMsg.Scope.Value;              // ScopeType.Public ...
                DisasterKind disasterKind = BasisData.FindDisasterKindByCode("DWC");
                if (disasterKind == null)
                {
                    FileLogManager.GetInstance().WriteLog("[MainForm] PrepareClearAlertOrderInfo( 재난 정보 오류(종류) )");
                    return false;
                }
                DisasterCategory category = BasisData.FindDisasterCategoryByID(disasterKind.CategoryID);
                if (category == null)
                {
                    FileLogManager.GetInstance().WriteLog("[MainForm] PrepareClearAlertOrderInfo( 재난 정보 오류(카테고리) )");
                    return false;
                }
                this.currentOrderProvisionInfo.Disaster = new Disaster(category, disasterKind);
                this.currentOrderProvisionInfo.RefType = OrderReferenceType.Clear;      // [고정값]
                this.currentOrderProvisionInfo.RefRecordID = targetRecord.CAPID;
                this.currentOrderProvisionInfo.LocationKind = OrderLocationKind.Local;
                this.currentOrderProvisionInfo.ClearAlertState = ClearAlertState.Clear; // 경보해제 발령이므로 경보 상황은 해제된다고 봐야한다.
                SendingMsgTextInfo msgTxtInfo = new SendingMsgTextInfo();
                msgTxtInfo.SelectedLanguages = new List<MsgTextDisplayLanguageKind>();
                msgTxtInfo.SelectedCityType = new MsgTextCityType();

                if (capMsg.Info == null)
                {
                    FileLogManager.GetInstance().WriteLog("[MainForm] PrepareClearAlertOrderInfo( CAP 데이터 오류 )");
                    return false;
                }
                foreach (InfoType info in capMsg.Info)
                {
                    MsgTextDisplayLanguageKind languageKind = BasisData.FindMsgTextLanguageInfoByCode(info.Language);
                    if (languageKind == null)
                    {
                        FileLogManager.GetInstance().WriteLog("[MainForm] PrepareClearAlertOrderInfo( 언어 정보 오류. Lnaguage=[" + info.Language + "] )");
                        continue;
                    }
                    msgTxtInfo.SelectedLanguages.Add(languageKind);
                }

                // 경보 해제 시 재난코드 고정
                List<MsgText> textList = BasisData.FindMsgTextInfoByDisasterCode(disasterKind.Code);
                if (textList == null)
                {
                    FileLogManager.GetInstance().WriteLog("[MainForm] PrepareClearAlertOrderInfo( 문안정보 오류. disasterKindCode=[" + disasterKind.Code + "] )");
                    return false;
                }
                if (msgTxtInfo.OriginalTransmitMsgText == null)
                {
                    msgTxtInfo.OriginalTransmitMsgText = new List<MsgText>();
                }
                msgTxtInfo.OriginalTransmitMsgText.Clear();
                foreach (MsgTextDisplayLanguageKind languageKind in msgTxtInfo.SelectedLanguages)
                {
                    foreach (MsgText text in textList)
                    {
                        if (text.LanguageKindID == languageKind.ID)
                        {
                            msgTxtInfo.OriginalTransmitMsgText.Add(text);
                        }
                    }
                }
                this.currentOrderProvisionInfo.MsgTextInfo = msgTxtInfo;

                if (capMsg.Scope.Value == ScopeType.Public || capMsg.Scope.Value == ScopeType.Restricted)
                {
                    this.currentOrderProvisionInfo.TargetRegions = helper.ExtractTargetRegionsFromCAP(capMsg);
                }
                else
                {
                    this.currentOrderProvisionInfo.TargetSystems = helper.ExtractTargetSystemsFromCAP(capMsg);
                }
                if (capMsg.Scope.Value == ScopeType.Restricted)
                {
                    this.currentOrderProvisionInfo.TargetSystemsKinds = helper.ExtractTargetKindsFromCAP(capMsg);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] PrepareClearAlertOrderInfo ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] PrepareClearAlertOrderInfo( " + ex.ToString() + " )");

                return false;
            }
            finally
            {
                FileLogManager.GetInstance().WriteLog("[MainForm] PrepareClearAlertOrderInfo( end )");
            }

            return true;
        }
        private bool PrepareSWROrderInfo(string targetReportID, SWRProfile targetProfile)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] PrepareSWROrderInfo( start )");

            try
            {
                if (string.IsNullOrEmpty(targetReportID) || targetProfile == null)
                {
                    System.Console.WriteLine("[MainForm] PrepareSWROrderInfo ( 입력 파라미터 에러 )");
                    FileLogManager.GetInstance().WriteLog("[MainForm] PrepareSWROrderInfo( 입력 파라미터 오류 )");

                    return false;
                }

                // 발령준비 정보 설정
                if (this.currentOrderProvisionInfo == null)
                {
                    this.currentOrderProvisionInfo = new OrderProvisionInfo();
                }

                this.currentOrderProvisionInfo.CAPData = null;
                OrderMode orderMode = BasisData.FindOrderModeInfoByCode(StatusType.Actual);
                if (orderMode == null)
                {
                    System.Console.WriteLine("[MainForm] PrepareSWROrderInfo ( 발령 모드 정보 취득 실패 )");
                    FileLogManager.GetInstance().WriteLog("[MainForm] PrepareSWROrderInfo( 발령 모드 정보 오류 )");

                    return false;
                }
                this.currentOrderProvisionInfo.Mode = orderMode;

                this.currentOrderProvisionInfo.MessageType = MsgType.Alert;
                this.currentOrderProvisionInfo.Scope = ScopeType.Public;
                this.currentOrderProvisionInfo.ClearAlertState = ClearAlertState.Waiting;

                DisasterKind disasterKind = null;
                string cmdString = BasisData.FindSWRCommandStringByCommandCode(targetProfile.CommandCode);
                if (cmdString.Contains("해제"))
                {
                    disasterKind = BasisData.FindDisasterKindByCode("DWC");
                    this.currentOrderProvisionInfo.ClearAlertState = ClearAlertState.Clear;
                }
                else
                {
                    disasterKind = BasisData.FindDisasterKindBySWRInfo(targetProfile.WarnKindCode, targetProfile.WarnStressCode);
                }
                if (disasterKind == null)
                {
                    System.Console.WriteLine("[MainForm] PrepareSWROrderInfo ( 재난 종류 정보 취득 실패 )");
                    FileLogManager.GetInstance().WriteLog("[MainForm] PrepareSWROrderInfo( 재난 종류 정보 오류 )");

                    return false;
                }
                DisasterCategory category = BasisData.FindDisasterCategoryByID(disasterKind.CategoryID);
                if (category == null)
                {
                    System.Console.WriteLine("[MainForm] PrepareSWROrderInfo ( 재난 카테고리 정보 취득 실패 )");
                    FileLogManager.GetInstance().WriteLog("[MainForm] PrepareSWROrderInfo( 재난 카테고리 정보 오류 )");

                    return false;
                }
                this.currentOrderProvisionInfo.Disaster = new Disaster(category, disasterKind);
                this.currentOrderProvisionInfo.RefType = OrderReferenceType.SWR;    // 기상특보연계발령
                this.currentOrderProvisionInfo.RefRecordID = targetProfile.ID;
                this.currentOrderProvisionInfo.LocationKind = OrderLocationKind.Local;

                SendingMsgTextInfo msgTxtInfo = new SendingMsgTextInfo();
                msgTxtInfo.SelectedLanguages = new List<MsgTextDisplayLanguageKind>();
                msgTxtInfo.SelectedCityType = new MsgTextCityType();

                foreach (MsgTextDisplayLanguageKind languageKind in BasisData.MsgTextLanguageKind)
                {
                    if (languageKind.IsDefault)
                    {
                        MsgTextDisplayLanguageKind copy = new MsgTextDisplayLanguageKind();
                        copy.DeepCopyFrom(languageKind);
                        msgTxtInfo.SelectedLanguages.Add(copy);
                    }
                }

                List<MsgText> textList = BasisData.FindMsgTextInfoByDisasterCode(disasterKind.Code);
                if (textList == null)
                {
                    System.Console.WriteLine("[MainForm] PrepareSWROrderInfo ( 문안 취득 실패 )");
                    FileLogManager.GetInstance().WriteLog("[MainForm] PrepareSWROrderInfo( 문안 정보 오류. disasterKindCode=[" + disasterKind.Code + "] )");

                    return false;
                }
                if (msgTxtInfo.OriginalTransmitMsgText == null)
                {
                    msgTxtInfo.OriginalTransmitMsgText = new List<MsgText>();
                }
                msgTxtInfo.OriginalTransmitMsgText.Clear();
                foreach (MsgText text in textList)
                {
                    foreach (MsgTextDisplayLanguageKind info in msgTxtInfo.SelectedLanguages)
                    {
                        if (text.LanguageKindID == info.ID)
                        {
                            msgTxtInfo.OriginalTransmitMsgText.Add(text);
                        }
                    }
                }
                this.currentOrderProvisionInfo.MsgTextInfo = msgTxtInfo;

                if (this.currentOrderProvisionInfo.TargetRegions == null)
                {
                    this.currentOrderProvisionInfo.TargetRegions = new List<RegionDefinition>();
                }
                this.currentOrderProvisionInfo.TargetRegions.Clear();

                string[] targetRegionCodes = targetProfile.GetTargetRegionCodes();
                if (targetRegionCodes == null)
                {
                    System.Console.WriteLine("[MainForm] PrepareSWROrderInfo ( 대상 지역 정보 에러 )");
                    FileLogManager.GetInstance().WriteLog("[MainForm] PrepareSWROrderInfo( 대상 지역 정보 오류. )");

                    return false;
                }
                foreach (string regionCode in targetRegionCodes)
                {
                    RegionProfile profile = BasisData.FindRegion(regionCode);
                    if (profile == null)
                    {
                        FileLogManager.GetInstance().WriteLog("[MainForm] PrepareSWROrderInfo( 지역 프로필 정보 조회 오류. regionCode=[" + regionCode + "])");
                        continue;
                    }
                    RegionDefinition region = new RegionDefinition(profile.Code, profile.Name);
                    this.currentOrderProvisionInfo.TargetRegions.Add(region);
                }

                this.currentOrderProvisionInfo.Tag = targetProfile;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] PrepareSWROrderInfo ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] PrepareSWROrderInfo( " + ex.ToString() + " )");

                return false;
            }
            finally
            {
                FileLogManager.GetInstance().WriteLog("[MainForm] PrepareSWROrderInfo( end )");
            }

            return true;
        }
        /// <summary>
        /// 그룹프로필 데이터를 발령준비 데이터로 변환.
        /// </summary>
        /// <param name="groupProfile"></param>
        /// <param name="orderInfo"></param>
        private OrderProvisionInfo ConvertToOrderProvisionInfo(GroupProfile groupProfile)
        {
            System.Diagnostics.Debug.Assert(groupProfile != null);
            FileLogManager.GetInstance().WriteLog("[MainForm] ConvertToOrderProvisionInfo( start )");

            OrderProvisionInfo newOrderInfo = new OrderProvisionInfo();

            try
            {
                if (groupProfile == null ||
                    groupProfile.Targets == null || groupProfile.Targets.Count < 1)
                {
                    // 프로필 정보 오류
                    FileLogManager.GetInstance().WriteLog("[MainForm] ConvertToOrderProvisionInfo( 입력 파라미터 오류 )");

                    return null;
                }

                newOrderInfo.Mode = BasisData.FindOrderModeInfoByCode(StatusType.Actual);
                newOrderInfo.MessageType = MsgType.Alert;

                // 재난 정보
                if (groupProfile.DisasterCategoryID > 0)
                {
                    newOrderInfo.Disaster = new Disaster();
                    newOrderInfo.Disaster.Category = BasisData.FindDisasterCategoryByID(groupProfile.DisasterCategoryID);
                    newOrderInfo.Disaster.Kind = BasisData.FindDisasterKindByCode(groupProfile.DisasterKindCode);
                }

                // 발령 참조 구분
                newOrderInfo.RefType = OrderReferenceType.None;
                // 발령 참조 레코드 아이디
                newOrderInfo.RefRecordID = string.Empty;
                // 전송 문안
                if (!string.IsNullOrEmpty(groupProfile.DisasterKindCode))
                {
                    List<MsgText> msgList = BasisData.FindMsgTextInfoByDisasterCode(groupProfile.DisasterKindCode);
                    if (msgList != null)
                    {
                        newOrderInfo.MsgTextInfo = new SendingMsgTextInfo();
                        if (BasisData.MsgTextCityType != null && BasisData.MsgTextCityType.Count > 0)
                        {
                            newOrderInfo.MsgTextInfo.SelectedCityType = new MsgTextCityType();
                            newOrderInfo.MsgTextInfo.SelectedCityType.DeepCopyFrom(BasisData.MsgTextCityType[0]);
                        }
                        newOrderInfo.MsgTextInfo.OriginalTransmitMsgText = msgList;
                        newOrderInfo.MsgTextInfo.CurrentTransmitMsgText = null;
                    }
                }
                // 언어 종류
                if (newOrderInfo.MsgTextInfo == null)
                {
                    newOrderInfo.MsgTextInfo = new SendingMsgTextInfo();
                }
                if (newOrderInfo.MsgTextInfo.SelectedLanguages == null)
                {
                    newOrderInfo.MsgTextInfo.SelectedLanguages = new List<MsgTextDisplayLanguageKind>();
                }
                foreach (MsgTextDisplayLanguageKind languageKind in BasisData.MsgTextLanguageKind)
                {
                    if (languageKind.IsDefault)
                    {
                        MsgTextDisplayLanguageKind copy = new MsgTextDisplayLanguageKind();
                        copy.DeepCopyFrom(languageKind);
                        newOrderInfo.MsgTextInfo.SelectedLanguages.Add(copy);
                    }
                }
                if (newOrderInfo.MsgTextInfo.SelectedLanguages.Count <= 0)
                {
                    MsgTextDisplayLanguageKind language = BasisData.FindMsgTextLanguageInfoByCode(BasisData.DEFAULT_LANGUAGECODE);
                    if (language != null)
                    {
                        newOrderInfo.MsgTextInfo.SelectedLanguages.Add(language);
                    }
                }

                // 발령 대상
                if (groupProfile.GroupType == GroupTypeCodes.Region)
                {
                    newOrderInfo.Scope = ScopeType.Public;

                    newOrderInfo.TargetRegions = new List<RegionDefinition>();
                    foreach (string regionCode in groupProfile.Targets)
                    {
                        RegionProfile regionProfile = BasisData.FindRegion(regionCode);
                        if (regionProfile == null)
                        {
                            FileLogManager.GetInstance().WriteLog("[MainForm] ConvertToOrderProvisionInfo( 지역 프로필 조회 오류. regionCode=[" + regionCode + "] )");
                            continue;
                        }
                        RegionDefinition region = new RegionDefinition(regionProfile.Code, regionProfile.Name);
                        newOrderInfo.TargetRegions.Add(region);
                    }
                    if (newOrderInfo.TargetRegions.Count < 1)
                    {
                        FileLogManager.GetInstance().WriteLog("[MainForm] ConvertToOrderProvisionInfo( 지역 프로필 조회 오류. 0건 )");
                        return null;
                    }

                    // 발령 준비 데이터에 표준경보시스템 종류 정보 추가
                    if (groupProfile.TargetSystemKinds != null)
                    {
                        newOrderInfo.TargetSystemsKinds = new List<SASKind>();

                        foreach (string kindCode in groupProfile.TargetSystemKinds)
                        {
                            SASKind systemKind = BasisData.FindSASKindByCode(kindCode);
                            if (systemKind == null)
                            {
                                FileLogManager.GetInstance().WriteLog("[MainForm] ConvertToOrderProvisionInfo( 시스템 프로필 조회 오류. kindCode=[" + kindCode + "] )");
                                continue;
                            }

                            SASKind copy = new SASKind(systemKind.Code, systemKind.Name);
                            newOrderInfo.TargetSystemsKinds.Add(copy);
                        }
                        if (BasisData.SASKindInfo == null ||
                            (BasisData.SASKindInfo.Count == newOrderInfo.TargetSystemsKinds.Count))
                        {
                            newOrderInfo.Scope = ScopeType.Public;
                        }
                        else
                        {
                            newOrderInfo.Scope = ScopeType.Restricted;
                        }
                    }
                }
                else if (groupProfile.GroupType == GroupTypeCodes.System)
                {
                    newOrderInfo.Scope = ScopeType.Private;

                    newOrderInfo.TargetSystems = new List<SASProfile>();

                    if (groupProfile.Targets != null && groupProfile.Targets.Count > 0)
                    {
                        foreach (string systemID in groupProfile.Targets)
                        {
                            if (this.currentDicSystemInfo == null || this.currentDicSystemInfo.Values == null ||
                                this.currentDicSystemInfo.Values.Count < 1)
                            {
                                break;
                            }

                            bool isFound = this.currentDicSystemInfo.ContainsKey(systemID);
                            if (isFound)
                            {
                                SASProfile systemProfile = new SASProfile();
                                SASInfo systemInfo = this.currentDicSystemInfo[systemID];
                                systemProfile.DeepCopyFrom(systemInfo.Profile);

                                newOrderInfo.TargetSystems.Add(systemProfile);
                            }
                        }
                    }
                    if (newOrderInfo.TargetSystems.Count < 1)
                    {
                        FileLogManager.GetInstance().WriteLog("[MainForm] ConvertToOrderProvisionInfo( 시스템 프로필 0건 )");
                        return null;
                    }
                }
                else
                {
                    // error
                    FileLogManager.GetInstance().WriteLog("[MainForm] ConvertToOrderProvisionInfo( error?? )");
                }

                // 경보해제상태
                newOrderInfo.ClearAlertState = ClearAlertState.Waiting;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] ConvertToOrderProvisionInfo ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[MainForm] ConvertToOrderProvisionInfo( " + ex.ToString() + " )");

                return null;
            }
            finally
            {
                FileLogManager.GetInstance().WriteLog("[MainForm] ConvertToOrderProvisionInfo( end )");
            }

            return newOrderInfo;
        }
        #endregion


        #region 시각표시
        /// <summary>
        /// 현재 시각 표시 쓰레드 시작.
        /// </summary>
        public int StartDisplayTime()
        {
            System.Console.WriteLine("[MainForm] StartDisplayTime()");
            FileLogManager.GetInstance().WriteLog("[MainForm] StartDisplayTime()");

            try
            {
                this.isContinueDisplayTime = true;
                if (this.timeThread == null)
                {
                    this.timeThread = new Thread(new ThreadStart(this.DisplayTimePolling));
                    this.timeThread.IsBackground = true;
                    this.timeThread.Name = "DisplayTimeThread";
                    this.timeThread.Start();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] StartDisplayTime( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] StartDisplayTime( Exception=[" + ex.ToString() + "] )");

                return -99;
            }

            return 0;
        }
        /// <summary>
        /// 현재 시각 표시 쓰레드 종료.
        /// </summary>
        public int EndDisplayTime()
        {
            System.Console.WriteLine("[MainForm] EndDisplayTime()");
            FileLogManager.GetInstance().WriteLog("[MainForm] EndDisplayTime()");

            try
            {
                this.isContinueDisplayTime = false;
                if (this.timeThread != null && this.timeThread.IsAlive)
                {
                    bool isTerminated = this.timeThread.Join(500);
                    if (!isTerminated)
                    {
                        this.timeThread.Abort();
                    }
                }
                this.timeThread = null;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[MainForm] EndDisplayTime( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[MainForm] EndDisplayTime( Exception=[" + ex.ToString() + "] )");

                return -99;
            }

            return 0;
        }
        #endregion

        #region 메뉴 클릭 event
        private void 종료XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            //this.Dispose();
        }

        private void 발령이력OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] 발령이력OToolStripMenuItem_Click( start )");

            InquiryHistoryForm inquiryHistoryForm = new InquiryHistoryForm();
            inquiryHistoryForm.SetInquiryKind(0);
            this.core.NotifyOrderResponseUpdated += new EventHandler<OrderResponseEventArgs>(inquiryHistoryForm.OnNotifyOrderResponseUpdated);
            inquiryHistoryForm.ShowDialog();
            this.core.NotifyOrderResponseUpdated -= new EventHandler<OrderResponseEventArgs>(inquiryHistoryForm.OnNotifyOrderResponseUpdated);

            FileLogManager.GetInstance().WriteLog("[MainForm] 발령이력OToolStripMenuItem_Click( end )");
        }

        private void 프로그램사용이력PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] 프로그램사용이력PToolStripMenuItem_Click( start )");

            InquiryHistoryForm inquiryHistoryForm = new InquiryHistoryForm();
            inquiryHistoryForm.SetInquiryKind(1);
            inquiryHistoryForm.ShowDialog(this);

            FileLogManager.GetInstance().WriteLog("[MainForm] 프로그램사용이력PToolStripMenuItem_Click( end )");
        }


        private void 언어설정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] 언어설정ToolStripMenuItem_Click_Click( start )");

            LanguageSettingForm languageSettingForm = new LanguageSettingForm();
            languageSettingForm.NotifyLanguageSettingUpdate += new EventHandler<LanguageSettingUpdateEventArgs>(languageSettingForm_OnNotifyLanguageSettingUpdate);
            languageSettingForm.ShowDialog(this);
            languageSettingForm.NotifyLanguageSettingUpdate -= new EventHandler<LanguageSettingUpdateEventArgs>(languageSettingForm_OnNotifyLanguageSettingUpdate);

            FileLogManager.GetInstance().WriteLog("[MainForm] 언어설정ToolStripMenuItem_Click_Click( end )");
        }
        private void 기본문안관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] 기본문안관리ToolStripMenuItem_Click( start )");

            MsgTextManageForm msgTextManageForm = new MsgTextManageForm();
            msgTextManageForm.ShowDialog(this);

            FileLogManager.GetInstance().WriteLog("[MainForm] 기본문안관리ToolStripMenuItem_Click( end )");
        }

        private void 기상특보연계설정ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] 기상특보연계설정ToolStripMenuItem_Click( start )");

            ConfigData configData = this.core.GetConfigInfo();
            List<SWRAssociationCondition> conditions = this.core.GetSWRAssociationCondition();
            SWRConditionSettingForm swrConditionSettingForm = new SWRConditionSettingForm(configData.SWR, conditions);
            swrConditionSettingForm.NotifySWRAssociationConditionUpdate += new EventHandler<SWRConditionUpdateEventArgs>(swrConditionSettingForm_OnNotifySWRAssociationConditionUpdate);
            swrConditionSettingForm.ShowDialog(this);
            swrConditionSettingForm.NotifySWRAssociationConditionUpdate -= new EventHandler<SWRConditionUpdateEventArgs>(swrConditionSettingForm_OnNotifySWRAssociationConditionUpdate);

            FileLogManager.GetInstance().WriteLog("[MainForm] 기상특보연계설정ToolStripMenuItem_Click( end )");
        }
        /// <summary>
        /// [메뉴][시스템 설정 관리] 클릭 이벤트 핸들러.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 시스템설정관리SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] 시스템설정관리SToolStripMenuItem_Click( start )");

            ConfigData configData = this.core.GetConfigInfo();
            SystemConnectionSettingForm systemSettingForm = new SystemConnectionSettingForm(configData);
            systemSettingForm.NotifyIAGWConnectionTest += new EventHandler<IAGWConnectionTestEventArgs>(systemSettingForm_OnNotifyIAGWConnectionTest);
            systemSettingForm.NotifyDBConnectionTest += new EventHandler<DBConnectionTestEventArgs>(systemSettingForm_OnNotifyDBConnectionTest);
            systemSettingForm.NotifyConfigSettingUpdate += new EventHandler<ConfigSettingUpdateEventArgs>(systemSettingForm_OnNotifyConfigSettingUpdate);
            systemSettingForm.ShowDialog(this);
            systemSettingForm.NotifyIAGWConnectionTest -= new EventHandler<IAGWConnectionTestEventArgs>(systemSettingForm_OnNotifyIAGWConnectionTest);
            systemSettingForm.NotifyDBConnectionTest -= new EventHandler<DBConnectionTestEventArgs>(systemSettingForm_OnNotifyDBConnectionTest);
            systemSettingForm.NotifyConfigSettingUpdate -= new EventHandler<ConfigSettingUpdateEventArgs>(systemSettingForm_OnNotifyConfigSettingUpdate);

            FileLogManager.GetInstance().WriteLog("[MainForm] 시스템설정관리SToolStripMenuItem_Click( end )");
        }
        /// <summary>
        /// [메뉴][사용자 관리] 클릭 이벤트 핸들러.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 사용자관리UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] 사용자관리UToolStripMenuItem_Click( start )");

            List<UserAccount> accountInfo = DBManager.GetInstance().QueryUserAccountInfo();
            UserAccountForm userAccountForm = new UserAccountForm(accountInfo);
            userAccountForm.ShowDialog(this);

            FileLogManager.GetInstance().WriteLog("[MainForm] 사용자관리UToolStripMenuItem_Click( end )");
        }
        /// <summary>
        /// [메뉴][프로그램 정보] 클릭 이벤트 핸들러.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 표준발령대정보AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileLogManager.GetInstance().WriteLog("[MainForm] 표준발령대정보AToolStripMenuItem_Click( start )");

            string versionInfo = VERSION_INFO;
            ProgramInformationForm infoForm = new ProgramInformationForm(versionInfo);
            infoForm.ShowDialog(this);

            FileLogManager.GetInstance().WriteLog("[MainForm] 표준발령대정보AToolStripMenuItem_Click( end )");
        }
        #endregion

        private void MainForm_MdiChildActivate(object sender, EventArgs e)
        {
            System.Console.WriteLine("[MainForm] MainForm_MdiChildActivate (  )");
        }

    }
}