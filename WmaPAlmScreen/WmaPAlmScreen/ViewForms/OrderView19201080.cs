using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NCASFND;
using NCASFND.NCasLogging;
using NCASFND.NCasNet;
using NCASFND.NCasCtrl;
using NCASBIZ.NCasBiz;
using NCASBIZ.NCasDefine;
using NCASBIZ.NCasProtocol;
using NCASBIZ.NCasPlcProtocol;
using NCASBIZ.NCasData;
using NCASBIZ.NCasUtility;
using NCasAppCommon.Define;
using NCasAppCommon.Type;
using NCasMsgCommon.Tts;
using NCasMsgCommon.Std;
using NCasContentsModule;
using NCasContentsModule.TTS;
using NCasContentsModule.StoMsg;

namespace WmaPAlmScreen
{
    public partial class OrderView19201080 : ViewBase
    {
        #region enum
        public enum DisasterBroadKind
        {
            None = 0,
            Mic = 1,
            Tts = 2,
            StroredMessage = 3
        }

        public enum OrderDataSendStatus
        {
            None = 0,
            First = 1,
            End = 2,
            FirstEnd = 3
        }
        #endregion

        #region element
        private List<Control> lstDeviceStatusLabel = new List<Control>();
        private NCasDefineOrderMode selectedOrderMode = NCasDefineOrderMode.None;
        private NCasDefineOrderMedia selectedOrderMedia = NCasDefineOrderMedia.None;
        private List<string> lstSelectedOrderIP = new List<string>();
        private NCasDefineOrderKind selectedOrderKind = NCasDefineOrderKind.None;
        private NCasDefineOrderKind lastOrderKind = NCasDefineOrderKind.None;
        private List<NCasButton> lstSelectedButtons = new List<NCasButton>();
        private DisasterBroadKind selectedDisasterBroadKind = DisasterBroadKind.None;
        private DisasterBroadKind lastDisasterBroadKind = DisasterBroadKind.None;
        private List<GroupContent> selectedGroupContents = new List<GroupContent>();
        private StoredMessageText selectedStoredMessage = new StoredMessageText();
        private TtsMessageText selectedTtsMessage = new TtsMessageText();
        private TermDestination termDestination = null;
        private OrderDistTermForm orderDistTermForm = null;
        private OrderStoredViewForm orderStoredViewForm = null;
        private ProvInfo provInfo = null;
        private int storedMessageRepeatCount = 1;
        private int selectedDistCode = 0;
        private ImageList orderKindImageList = null;
        private ImageList orderKindImageListDistIcon = null;
        private List<string> groupNameLst = new List<string>();
        private bool orderStandbyFlag = false;
        private bool allDestinationFlag = false;
        private bool orderTermFlag = false;
        private bool orderTermAllFlag = false;
        private bool orderTermGroupFlag = false;
        private bool orderDistGroupFlag = false;
        private bool orderDistFlag = false;
        private bool orderDistTermFlag = false;
        private bool orderDistTermAllFlag = false;
        private bool realButtonFromPlc = false;
        private readonly string WrongOperationIP = "0.0.0.0";
        private readonly string NotifyOrderMode = "발령모드 먼저 선택하세요";
        private readonly string NotifyOrderdestination = "발령대상 먼저 선택하세요";
        private readonly string NotifyOrderKind = "발령종류 먼저 선택하세요";
        private readonly string NotifyAllDestinationSelected = "전체 발령이 선택되어 있습니다";
        private readonly string NotifyTermDestinationSelected = "개별 단말을 선택 중입니다";
        private readonly string NotifyGroupDestinationSelected = "그룹 발령이 선택되어 있습니다";
        private readonly string NotifyDistIconDestinationSelected = "시군 발령이 선택되어 있습니다";
        private readonly string NotifyRealSelected = "실제모드 선택을 해제하세요";
        private readonly string NotifyReadyOrderMiss = "예비 발령을 먼저 선택하세요";
        private readonly string NotifyDistIconTermDestinationSelected = "시군 단말을 선택 중입니다";
        private readonly int ButtonBlinkInterval = 700;
        #endregion

        #region 생성자
        /// <summary>
        /// 생성자
        /// </summary>
        public OrderView19201080()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="main"></param>
        public OrderView19201080(MainForm main)
            : this()
        {
            this.main = main;
            this.provInfo = main.ProvInfo;
        }
        #endregion

        #region override method
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.InitMapImage();
            this.DistIconArrange();
            this.InitOrderButton();
            this.OrderButtonArrange();
            this.InitImageList();
            this.InitDeviceStatus();
            this.main.AddTimerMember(this);
            this.DisasterBroadFlagInit();
        }

        public override void OnTimer()
        {
            base.OnTimer();
            this.UpdateDeviceStatus();
        }
        #endregion

        #region 프로퍼티
        /// <summary>
        /// ProvInfo 프로퍼티
        /// </summary>
        public ProvInfo ProvInfo
        {
            get { return this.provInfo; }
        }

        /// <summary>
        /// 그룹정보 리스트 프로퍼티
        /// </summary>
        public List<string> GroupNameLst
        {
            get { return this.groupNameLst; }
            set { this.groupNameLst = value; }
        }
        #endregion

        #region 발령종류에 따른 버튼색 ImageList 초기화
        /// <summary>
        /// 발령종류에 따른 ImageList 초기화
        /// </summary>
        private void InitImageList()
        {
            this.orderKindImageList = new ImageList();
            this.orderKindImageList.ImageSize = new Size(185, 44);
            this.orderKindImageList.ColorDepth = ColorDepth.Depth32Bit;
            this.orderKindImageList.Images.Add(WmaPAlmScreenRsc.btnAlertGray);
            this.orderKindImageList.Images.Add(WmaPAlmScreenRsc.btnAlertGray);
            this.orderKindImageList.Images.Add(WmaPAlmScreenRsc.btnAlertGreen);
            this.orderKindImageList.Images.Add(WmaPAlmScreenRsc.btnAlertNormal);
            this.orderKindImageList.Images.Add(WmaPAlmScreenRsc.btnAlertOrange);
            this.orderKindImageList.Images.Add(WmaPAlmScreenRsc.btnAlertPink);
            this.orderKindImageList.Images.Add(WmaPAlmScreenRsc.btnAlertPupple);
            this.orderKindImageList.Images.Add(WmaPAlmScreenRsc.btnAlertRed);
            this.orderKindImageList.Images.Add(WmaPAlmScreenRsc.btnAlertSelected);
            this.orderKindImageList.Images.Add(WmaPAlmScreenRsc.btnAlertWhite);
            this.orderKindImageList.Images.Add(WmaPAlmScreenRsc.btnAlertYellow);

            this.orderKindImageListDistIcon = new ImageList();
            this.orderKindImageListDistIcon.ImageSize = new Size(54, 47);
            this.orderKindImageListDistIcon.ColorDepth = ColorDepth.Depth32Bit;
            this.orderKindImageListDistIcon.Images.Add(WmaPAlmScreenRsc.iconMap);
            this.orderKindImageListDistIcon.Images.Add(WmaPAlmScreenRsc.iconMap);
            this.orderKindImageListDistIcon.Images.Add(WmaPAlmScreenRsc.iconMapGreen);
            this.orderKindImageListDistIcon.Images.Add(WmaPAlmScreenRsc.iconMap);
            this.orderKindImageListDistIcon.Images.Add(WmaPAlmScreenRsc.iconMapOrange);
            this.orderKindImageListDistIcon.Images.Add(WmaPAlmScreenRsc.iconMapPink);
            this.orderKindImageListDistIcon.Images.Add(WmaPAlmScreenRsc.iconMapPupple);
            this.orderKindImageListDistIcon.Images.Add(WmaPAlmScreenRsc.iconMapRed);
            this.orderKindImageListDistIcon.Images.Add(WmaPAlmScreenRsc.iconMapSelected);
            this.orderKindImageListDistIcon.Images.Add(WmaPAlmScreenRsc.iconMapWhite);
            this.orderKindImageListDistIcon.Images.Add(WmaPAlmScreenRsc.iconMapYellow);
        }
        #endregion

        #region 경보조작부 버튼 초기화 관련
        /// <summary>
        /// 경보조작부의 버튼을 초기화한다.
        /// </summary>
        private void InitOrderButton()
        {
            NCasKeyData tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.Real;
            tmpKeyData.ID = NCasDefineButtonCode.BtnRC11; //실제
            this.realBtn.Tag = tmpKeyData;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.Test;
            tmpKeyData.ID = NCasDefineButtonCode.BtnRC21; //시험
            this.testBtn.Tag = tmpKeyData;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.Line;
            tmpKeyData.ID = NCasDefineButtonCode.BtnRC31; //유선
            this.lineBtn.Tag = tmpKeyData;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.Sate;
            tmpKeyData.ID = NCasDefineButtonCode.BtnRC41; //위성
            this.satBtn.Tag = tmpKeyData;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.ProveAllDestination;
            tmpKeyData.ID = NCasDefineButtonCode.BtnRC12; //시도 전체
            this.allDestBtn.Tag = tmpKeyData;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.TermDestination;
            this.termDestBtn.Tag = tmpKeyData;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.TermGroupDestination;
            this.termGroupBtn1.Tag = tmpKeyData;
            this.termGroupBtn1.Text = GroupContentMng.LstGroupContent[0].Title;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.TermGroupDestination;
            this.termGroupBtn2.Tag = tmpKeyData;
            this.termGroupBtn2.Text = GroupContentMng.LstGroupContent[1].Title;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.TermGroupDestination;
            this.termGroupBtn3.Tag = tmpKeyData;
            this.termGroupBtn3.Text = GroupContentMng.LstGroupContent[2].Title;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.TermGroupDestination;
            this.termGroupBtn4.Tag = tmpKeyData;
            this.termGroupBtn4.Text = GroupContentMng.LstGroupContent[3].Title;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.TermGroupDestination;
            this.termGroupBtn5.Tag = tmpKeyData;
            this.termGroupBtn5.Text = GroupContentMng.LstGroupContent[4].Title;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.TermGroupDestination;
            this.termGroupBtn6.Tag = tmpKeyData;
            this.termGroupBtn6.Text = GroupContentMng.LstGroupContent[5].Title;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.TermGroupDestination;
            this.termGroupBtn7.Tag = tmpKeyData;
            this.termGroupBtn7.Text = GroupContentMng.LstGroupContent[6].Title;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.TermGroupDestination;
            this.termGroupBtn8.Tag = tmpKeyData;
            this.termGroupBtn8.Text = GroupContentMng.LstGroupContent[7].Title;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.TermGroupDestination;
            this.termGroupBtn9.Tag = tmpKeyData;
            this.termGroupBtn9.Text = GroupContentMng.LstGroupContent[8].Title;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.TermGroupDestination;
            this.termGroupBtn10.Tag = tmpKeyData;
            this.termGroupBtn10.Text = GroupContentMng.LstGroupContent[9].Title;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.DistGroupDestination;
            this.distGroupBtn1.Tag = tmpKeyData;
            this.distGroupBtn1.Text = GroupContentMng.LstGroupContent[10].Title;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.DistGroupDestination;
            this.distGroupBtn2.Tag = tmpKeyData;
            this.distGroupBtn2.Text = GroupContentMng.LstGroupContent[11].Title;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.Ready;
            tmpKeyData.ID = NCasDefineButtonCode.BtnRC13; //예비
            this.readyBtn.Tag = tmpKeyData;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.Watch;
            tmpKeyData.ID = NCasDefineButtonCode.BtnRC23; //경계
            this.watchBtn.Tag = tmpKeyData;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.Attack;
            tmpKeyData.ID = NCasDefineButtonCode.BtnRC33; //공습
            this.attackBtn.Tag = tmpKeyData;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.Biological;
            tmpKeyData.ID = NCasDefineButtonCode.BtnRC43; //화생방
            this.bioBtn.Tag = tmpKeyData;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.DisasterWatch;
            tmpKeyData.ID = NCasDefineButtonCode.BtnRC63; //재난위험
            this.disasterWatchBtn.Tag = tmpKeyData;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.DisasterBroad;
            tmpKeyData.ID = NCasDefineButtonCode.BtnRC73; //재난경계
            this.disasterBroadBtn.Tag = tmpKeyData;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.MicOrder;
            tmpKeyData.ID = NCasDefineButtonCode.BtnRC64; //MIC
            this.micBtn.Tag = tmpKeyData;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.TtsOrder;
            tmpKeyData.ID = NCasDefineButtonCode.BtnRC74; //TTS
            this.ttsBtn.Tag = tmpKeyData;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.MsgOrder;
            tmpKeyData.ID = NCasDefineButtonCode.BtnRC84; //저장메시지
            this.stoBtn.Tag = tmpKeyData;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.Clear;
            tmpKeyData.ID = NCasDefineButtonCode.BtnRC53; //해제
            this.clearBtn.Tag = tmpKeyData;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.Cancel;
            this.cancelBtn.Tag = tmpKeyData;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.Confirm;
            tmpKeyData.ID = NCasDefineButtonCode.BtnRC14; //확인
            this.confirmBtn.Tag = tmpKeyData;

            tmpKeyData = new NCasKeyData();
            tmpKeyData.KeyActioin = NCasKeyAction.None;
            this.revBtn.Tag = tmpKeyData;
        }

        /// <summary>
        /// 경보조작부의 버튼을 셋팅한다.
        /// </summary>
        private void OrderButtonArrange()
        {
            foreach (Control control in this.controlMainPanel.Controls)
            {
                if (control is Label)
                    continue;

                NCasButton curButton = control as NCasButton;
                NCasKeyData curKeyData = curButton.Tag as NCasKeyData;

                if (curKeyData.KeyActioin == NCasKeyAction.None)
                {
                    curButton.Text = String.Empty;
                    curButton.Enabled = false;
                }
                else if (curKeyData.KeyActioin == NCasKeyAction.Real) //실제 버튼
                {
                    //curButton.ForeColor = Color.Red;
                    curButton.ForeColor = Color.FromArgb(203, 203, 203);
                }
                else if (curKeyData.KeyActioin == NCasKeyAction.Test) //시험 버튼
                {
                    curButton.ForeColor = Color.FromArgb(203, 203, 203);
                }
                else if (curKeyData.KeyActioin == NCasKeyAction.Cancel || curKeyData.KeyActioin == NCasKeyAction.Confirm) //선택취소, 확인 버튼
                {
                    curButton.ForeColor = Color.FromArgb(203, 203, 203);
                    curButton.UseCheck = false;
                }
                else
                {
                    curButton.ForeColor = Color.FromArgb(203, 203, 203);
                    curButton.LstAnimation.ColorDepth = ColorDepth.Depth32Bit;
                    curButton.LstAnimation.ImageSize = new Size(WmaPAlmScreenRsc.btnAlertNormal.Width, WmaPAlmScreenRsc.btnAlertNormal.Height);
                    curButton.AnimationInterval = this.ButtonBlinkInterval;
                }
            }

            foreach (Control control in this.disasterBroadOrderPanel.Controls)
            {
                if (control is Label)
                    continue;

                NCasButton curButton = control as NCasButton;
                NCasKeyData curKeyData = curButton.Tag as NCasKeyData;

                if (curKeyData.KeyActioin == NCasKeyAction.None)
                {
                    curButton.Text = String.Empty;
                    curButton.Enabled = false;
                }
                else
                {
                    curButton.ForeColor = Color.FromArgb(203, 203, 203);
                    curButton.LstAnimation.ColorDepth = ColorDepth.Depth32Bit;
                    curButton.LstAnimation.ImageSize = new Size(WmaPAlmScreenRsc.btnAlertNormal.Width, WmaPAlmScreenRsc.btnAlertNormal.Height);
                    curButton.AnimationInterval = this.ButtonBlinkInterval;
                }
            }
        }
        #endregion

        #region 맵 초기화
        /// <summary>
        /// 시도에 맞는 맵 이미지를 셋팅한다.
        /// </summary>
        private void InitMapImage()
        {
            Image image = null;

            switch (this.main.ProvInfo.Code)
            {
                case 1670:
                    image = WmaPAlmScreenRsc._1670map;
                    break;

                case 1671:
                    image = WmaPAlmScreenRsc._1671map;
                    break;

                case 1672:
                    image = WmaPAlmScreenRsc._1672map;
                    break;

                case 1673:
                    image = WmaPAlmScreenRsc._1673map;
                    break;

                case 1674:
                    image = WmaPAlmScreenRsc._1674map;
                    break;

                case 1675:
                    image = WmaPAlmScreenRsc._1675map;
                    break;

                case 1676:
                    image = WmaPAlmScreenRsc._1676map;
                    break;

                case 1677:
                    image = WmaPAlmScreenRsc._1677map;
                    break;

                case 1678:
                    image = WmaPAlmScreenRsc._1678map;
                    break;

                case 1679:
                    image = WmaPAlmScreenRsc._1679map;
                    break;

                case 1680:
                    image = WmaPAlmScreenRsc._1680map;
                    break;

                case 1681:
                    image = WmaPAlmScreenRsc._1681map;
                    break;

                case 1682:
                    image = WmaPAlmScreenRsc._1682map;
                    break;

                case 1683:
                    image = WmaPAlmScreenRsc._1683map;
                    break;

                case 1684:
                    image = WmaPAlmScreenRsc._1684map;
                    break;

                case 1685:
                    image = WmaPAlmScreenRsc._1685map;
                    break;

                case 2481:
                    image = WmaPAlmScreenRsc._1686map;
                    break;

                default:
                    image = WmaPAlmScreenRsc._1670map;
                    break;
            }

            this.distMapPanel.BackgroundImage = image;
            this.distMapPanel.BackgroundImageLayout = ImageLayout.Zoom;
        }
        #endregion

        #region 맵의 시군아이콘 초기화
        /// <summary>
        /// 맵 위에 시군 아이콘을 셋팅한다.
        /// </summary>
        private void DistIconArrange()
        {
            DistIconUserControl distIcon = null;
            NCasKeyData tmpKeyData = null;
            DistIconData distIconData = null;
            int x = 50;
            int y = 50;
            
            foreach (DistInfo eachDistInfo in this.provInfo.LstDists)
            {
                bool isTermMulti = false;

                foreach (TermInfo eachTermInfo in eachDistInfo.LstTerms)
                {
                    if (eachTermInfo.TermFlag == NCasDefineTermKind.TermMutil)
                    {
                        isTermMulti = true;
                        break;
                    }
                }

                if (isTermMulti == false)
                {
                    continue;
                }

                tmpKeyData = new NCasKeyData();
                tmpKeyData.KeyActioin = NCasKeyAction.DistOneDestination;
                tmpKeyData.Info = eachDistInfo.Code.ToString();
                distIcon = new DistIconUserControl();
                distIcon.Tag = eachDistInfo;
                distIcon.DistIcon.ForeColor = Color.FromArgb(203, 203, 203);
                distIcon.DistIcon.LstAnimation.ColorDepth = ColorDepth.Depth32Bit;
                distIcon.DistIcon.LstAnimation.ImageSize = new Size(WmaPAlmScreenRsc.iconMapSelected.Width, WmaPAlmScreenRsc.iconMapSelected.Height);
                distIcon.DistIcon.AnimationInterval = this.ButtonBlinkInterval;
                distIcon.DistIcon.Tag = tmpKeyData;
                distIcon.DistIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.realBtn_MouseDown);
                distIcon.DistName = eachDistInfo.Name;
                distIconData = DistIconMng.GetDistIconData(eachDistInfo.Code);

                if (distIconData == null)
                {
                    distIcon.Location = new Point(x, y);
                    x += 50;
                }
                else
                {
                    distIcon.Location = new Point(distIconData.X, distIconData.Y);
                }

                this.distMapPanel.Controls.Add(distIcon);
            }
        }
        #endregion

        #region 재난경계 버튼 초기화
        /// <summary>
        /// 재난경계의 3가지 버튼 사용유무를 초기화한다.
        /// </summary>
        private void DisasterBroadFlagInit()
        {
            if (DisasterBroadFlagMng.DisasterBroadFlag[0] == '0') //MIC
            {
                this.micBtn.Visible = false;
            }

            if (DisasterBroadFlagMng.DisasterBroadFlag[1] == '0') //TTS
            {
                this.ttsBtn.Visible = false;
            }

            if (DisasterBroadFlagMng.DisasterBroadFlag[2] == '0') //STO
            {
                this.stoBtn.Visible = false;
            }
        }
        #endregion

        #region 장비 상태바 초기화 및 업데이트
        /// <summary>
        /// 장비 상태바를 초기화한다.
        /// </summary>
        private void InitDeviceStatus()
        {
            lstDeviceStatusLabel.Add(this.labelDevStatus1);
            lstDeviceStatusLabel.Add(this.labelDevStatus2);
            lstDeviceStatusLabel.Add(this.labelDevStatus3);
            lstDeviceStatusLabel.Add(this.labelDevStatus4);
            lstDeviceStatusLabel.Add(this.labelDevStatus5);
            lstDeviceStatusLabel.Add(this.labelDevStatus6);
            lstDeviceStatusLabel.Add(this.labelDevStatus7);
            lstDeviceStatusLabel.Add(this.labelDevStatus8);

            for (int i = 0; i < DeviceStatusMng.LstDeviceStatusData.Count; i++)
            {
                try
                {
                    lstDeviceStatusLabel[i].Visible = true;
                    lstDeviceStatusLabel[i].Text = "        " + DeviceStatusMng.LstDeviceStatusData[i].Name;
                    lstDeviceStatusLabel[i].Tag = DeviceStatusMng.LstDeviceStatusData[i].IpAddr;

                    if (i >= 7)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    NCasLoggingMng.ILoggingException.WriteException("MainForm", "MainForm.InitDeviceStatus() Method", ex);
                }
            }
        }

        /// <summary>
        /// 장비 상태를 업데이트한다.
        /// </summary>
        private void UpdateDeviceStatus()
        {
            try
            {
                foreach (Label label in this.lstDeviceStatusLabel)
                {
                    if (label.Visible == false)
                        continue;

                    DevStsInfo devInfo = this.main.MmfMng.GetDevStsInfoByIp(label.Tag.ToString());

                    if (devInfo.Status == NCasDefineNormalStatus.Noraml)
                    {
                        label.ImageIndex = 0;
                        label.ForeColor = Color.FromArgb(17, 231, 255);
                    }
                    else
                    {
                        label.ImageIndex = 1;
                        label.ForeColor = Color.FromArgb(255, 0, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILoggingException.WriteException("MainForm", "MainForm.UpdateDeviceStatus() Method", ex);
            }
        }
        #endregion

        #region 화면에서 사용하는 private method
        /// <summary>
        /// NCasKeyAction을 받아 해당하는 NCasButton을 반환한다.
        /// </summary>
        /// <param name="keyAction"></param>
        /// <returns></returns>
        private NCasButton GetNCasButton(NCasKeyAction keyAction, string btnCode, string info)
        {
            if (keyAction == NCasKeyAction.TermGroupDestination || keyAction == NCasKeyAction.DistGroupDestination)
            {
                for (int i = 0; i < this.controlMainPanel.Controls.Count; i++)
                {
                    if (this.controlMainPanel.Controls[i] is Label)
                        continue;

                    NCasButton ncasBtn = this.controlMainPanel.Controls[i] as NCasButton;
                    NCasKeyData btnKeyData = (NCasKeyData)ncasBtn.Tag;

                    if (btnKeyData.KeyActioin == keyAction)
                    {
                        if (btnKeyData.ID.ToString() == btnCode)
                        {
                            return ncasBtn;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < this.controlMainPanel.Controls.Count; i++)
                {
                    if (this.controlMainPanel.Controls[i] is Label)
                        continue;

                    NCasButton ncasBtn = this.controlMainPanel.Controls[i] as NCasButton;
                    NCasKeyData btnKeyData = (NCasKeyData)ncasBtn.Tag;

                    if (btnKeyData.KeyActioin == keyAction)
                    {
                        return ncasBtn;
                    }
                }

                foreach (NCasButton ncasBtn in this.disasterBroadOrderPanel.Controls)
                {
                    NCasKeyData btnKeyData = (NCasKeyData)ncasBtn.Tag;

                    if (btnKeyData.KeyActioin == keyAction)
                    {
                        return ncasBtn;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 버튼Code를 받아 해당하는 NCasButton을 반환한다.
        /// </summary>
        /// <param name="btnCode"></param>
        /// <returns></returns>
        private NCasButton GetNCasButton(string btnCode)
        {
            for (int i = 0; i < this.controlMainPanel.Controls.Count; i++)
            {
                if (this.controlMainPanel.Controls[i] is Label)
                    continue;

                NCasButton ncasBtn = this.controlMainPanel.Controls[i] as NCasButton;
                NCasKeyData btnKeyData = (NCasKeyData)ncasBtn.Tag;

                if (btnKeyData.ID.ToString() == btnCode)
                {
                    return ncasBtn;
                }
            }

            for (int i = 0; i < this.disasterBroadOrderPanel.Controls.Count; i++)
            {
                NCasButton ncasBtn = this.controlMainPanel.Controls[i] as NCasButton;
                NCasKeyData btnKeyData = (NCasKeyData)ncasBtn.Tag;

                if (btnKeyData.ID.ToString() == btnCode)
                {
                    return ncasBtn;
                }
            }

            return null;
        }

        /// <summary>
        /// MIC/TTS/STO 화면 컨트롤
        /// </summary>
        /// <param name="isShow">true - 보임, false - 안보임</param>
        private void ShowDisasterBroadPanel(bool isShow)
        {
            if (isShow)
            {
                this.disasterBroadOrderPanel.Visible = true;
            }
            else
            {
                this.disasterBroadOrderPanel.Visible = false;
            }
        }

        /// <summary>
        /// 오조작 메시지를 보여준다
        /// </summary>
        /// <param name="text"></param>
        private void ShowWrongOperationForm(string text)
        {
            using (WrongOperationForm form = new WrongOperationForm(text))
            {
                form.ShowDialog();
            }
        }
        #endregion

        #region 화면에서 사용하는 public method
        /// <summary>
        /// 재난운영대에서 시군아이콘 정보를 수신받아 화면에 반영한다.
        /// </summary>
        public void SetDistIconReArrange()
        {
            int x = 50;
            int y = 50;

            for (int i = 0; i < this.distMapPanel.Controls.Count; i++)
            {
                if (this.distMapPanel.Controls[i] is NCasPanel)
                    continue;

                DistIconUserControl userControl = this.distMapPanel.Controls[i] as DistIconUserControl;
                DistInfo tmpDistInfo = userControl.Tag as DistInfo;
                DistIconData distIconData = DistIconMng.GetDistIconData(tmpDistInfo.Code);

                if (distIconData == null)
                {
                    userControl.Location = new Point(x, y);
                    x += 50;
                }
                else
                {
                    userControl.Location = new Point(distIconData.X, distIconData.Y);
                }
            }
        }

        /// <summary>
        /// PLC로부터 데이터를 수신받아 화면에 반영한다.
        /// </summary>
        /// <param name="plcResponse"></param>
        public void SetKeyPlc(NCasProtocolTc148Sub13 plcResponse)
        {
            if (plcResponse.RealOrderKey == NCasDefineOnOffBlinkStatus.On) //실제 키를 돌린 경우..
            {
                NCasButton realPlcPushBtn = this.GetNCasButton(NCasKeyAction.Real, "", "");
                this.realButtonFromPlc = true;
                this.SetBtnChecked(realPlcPushBtn, true);
                this.realBtn_MouseDown(realPlcPushBtn, new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, 0, 0, 0));
                this.realButtonFromPlc = false;
            }
            else
            {
                if (plcResponse.RealOrderKey == NCasDefineOnOffBlinkStatus.Off) //실제 키를 시험으로 돌린 경우..
                {
                    NCasButton realPlcPushBtn = this.GetNCasButton(NCasKeyAction.Real, "", "");
                    this.realButtonFromPlc = true;
                    this.SetBtnChecked(realPlcPushBtn, false);
                    this.realBtn_MouseDown(realPlcPushBtn, new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, 0, 0, 0));
                    this.realButtonFromPlc = false;
                }

                //그 외 일반 버튼 눌린 경우..
                NCasButton plcPushBtn = null;

                if(plcResponse.ModeReal == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.realBtn;
                }
                else if(plcResponse.ModeTest == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.testBtn;
                }
                else if(plcResponse.MediaLine == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.lineBtn;
                }
                else if(plcResponse.MediaSate == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.satBtn;
                }
                else if(plcResponse.GroupAll == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.allDestBtn;
                }
                else if(plcResponse.Group2 == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.termGroupBtn1;
                }
                else if(plcResponse.Group3 == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.termGroupBtn2;
                }
                else if(plcResponse.Group4 == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.termGroupBtn3;
                }
                else if(plcResponse.Group5 == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.termGroupBtn4;
                }
                else if(plcResponse.Group6 == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.termGroupBtn5;
                }
                else if(plcResponse.Group7 == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.termGroupBtn6;
                }
                else if(plcResponse.Group8 == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.termGroupBtn7;
                }
                else if(plcResponse.Group9 == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.termGroupBtn8;
                }
                else if(plcResponse.Group10 == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.termGroupBtn9;
                }
                else if(plcResponse.KindReady == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.readyBtn;
                }
                else if(plcResponse.KindAttack == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.attackBtn;
                }
                else if(plcResponse.KindWatch == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.watchBtn;
                }
                else if(plcResponse.KindBiochemist == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.bioBtn;
                }
                else if(plcResponse.KindCancel == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.clearBtn;
                }
                else if(plcResponse.KindDisasterWatch == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.disasterBroadBtn;
                }
                else if(plcResponse.KindDisasterDanger == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.disasterWatchBtn;
                }
                else if(plcResponse.KindDisasterWatchMic == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.micBtn;
                }
                else if(plcResponse.KindDisasterWatchTts == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.ttsBtn;
                }
                else if(plcResponse.KindDisasterWatchMsg == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.stoBtn;
                }
                else if(plcResponse.Confirm == NCasDefineOnOffBlinkStatus.On)
                {
                    plcPushBtn = this.confirmBtn;
                }

                if(plcPushBtn == null)
                    return;

                if ((plcPushBtn.Tag as NCasKeyData).KeyActioin == NCasKeyAction.Real)
                    return;

                if (!((plcPushBtn.Tag as NCasKeyData).KeyActioin == NCasKeyAction.Cancel
                    || (plcPushBtn.Tag as NCasKeyData).KeyActioin == NCasKeyAction.Confirm))
                {
                    if (plcPushBtn.CheckedValue == true)
                    {
                        this.SetBtnChecked(plcPushBtn, false);
                    }
                    else
                    {
                        this.SetBtnChecked(plcPushBtn, true);
                    }
                }

                this.realBtn_MouseDown(plcPushBtn, new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, 0, 0, 0));
            }
        }
        #endregion

        #region 화면이나 폼에서 발생하는 event method
        /// <summary>
        /// 저장메시지 진행화면에서 방송종료 시 발생하는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void orderStoredViewForm_StoredMsgFinishEvent(object sender, EventArgs e)
        {
            if (this.orderStoredViewForm != null)
            {
                this.orderStoredViewForm.StoredMsgFinishEvent -= new EventHandler(orderStoredViewForm_StoredMsgFinishEvent);
                this.orderStoredViewForm.Close();
                this.orderStoredViewForm = null;
            }
        }

        /// <summary>
        /// 시군아이콘 개별 단말 선택창에서 단말 하나를 제거하는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void orderDistTermForm_RemoveTermDestinationEvent(object sender, TermSelectEventArgs e)
        {
            this.SetOrderDestinationButtonColorOff();
            this.allDestinationFlag = false;
            this.orderTermGroupFlag = false;
            this.orderDistGroupFlag = false;
            this.orderDistFlag = false;
            this.orderDistTermFlag = true;
            this.orderTermFlag = false;

            if (this.orderDistTermAllFlag)
            {
                this.lstSelectedOrderIP.Clear();

                foreach (TermInfo eachTermInfo in this.main.MmfMng.GetDistInfoByCode(this.selectedDistCode).LstTerms)
                {
                    if (eachTermInfo.UseFlag != NCasDefineUseStatus.Use)
                        continue;

                    if (eachTermInfo.IpAddrToSring == e.TermInfo.IpAddrToSring)
                        continue;

                    if (eachTermInfo.TermFlag != NCasDefineTermKind.TermMutil)
                        continue;

                    if (!this.lstSelectedOrderIP.Contains(eachTermInfo.IpAddrToSring))
                    {
                        this.lstSelectedOrderIP.Add(eachTermInfo.IpAddrToSring);
                    }
                }

                this.orderDistTermAllFlag = false;
            }
            else
            {
                if (this.lstSelectedOrderIP.Contains(e.TermInfo.IpAddrToSring))
                {
                    this.lstSelectedOrderIP.Remove(e.TermInfo.IpAddrToSring);
                }
            }

            this.ClearOrderKind();
        }

        /// <summary>
        /// 시군아이콘 개별 단말 선택창에서 단말 하나를 추가하는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void orderDistTermForm_AddTermDestinationEvent(object sender, TermSelectEventArgs e)
        {
            this.SetOrderDestinationButtonColorOff();
            this.allDestinationFlag = false;
            this.orderTermGroupFlag = false;
            this.orderDistGroupFlag = false;
            this.orderDistFlag = false;
            this.orderDistTermFlag = true;
            this.orderDistTermAllFlag = false;
            this.orderTermFlag = false;
            this.orderTermAllFlag = false;

            if (!this.lstSelectedOrderIP.Contains(e.TermInfo.IpAddrToSring))
            {
                this.lstSelectedOrderIP.Add(e.TermInfo.IpAddrToSring);
            }

            this.ClearOrderKind();
        }

        /// <summary>
        /// 시군아이콘 개별 단말 선택을 취소하는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void orderDistTermForm_TermDestinationCancelEvent(object sender, EventArgs e)
        {
            this.orderDistTermForm.TermAllSelectEvent -= new EventHandler(orderDistTermForm_TermAllSelectEvent);
            this.orderDistTermForm.TermAllCancelEvent -= new EventHandler(orderDistTermForm_TermAllCancelEvent);
            this.orderDistTermForm.TermDestinationCancelEvent -= new EventHandler(orderDistTermForm_TermDestinationCancelEvent);
            this.orderDistTermForm.AddTermDestinationEvent -= new EventHandler<TermSelectEventArgs>(orderDistTermForm_AddTermDestinationEvent);
            this.orderDistTermForm.RemoveTermDestinationEvent -= new EventHandler<TermSelectEventArgs>(orderDistTermForm_RemoveTermDestinationEvent);

            this.SetOrderDestinationButtonColorOff();
            this.orderDistTermFlag = false;
            this.orderDistTermAllFlag = false;
            this.orderDistFlag = false;
            this.ClearOrderKind();
            this.lstSelectedOrderIP.Clear();
            this.ShowDisasterBroadPanel(false);
        }

        /// <summary>
        /// 시군아이콘 개별 단말 선택창에서 전체 단말 선택을 해제한 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void orderDistTermForm_TermAllCancelEvent(object sender, EventArgs e)
        {
            this.SetOrderDestinationButtonColorOff();
            this.allDestinationFlag = false;
            this.orderTermGroupFlag = false;
            this.orderDistGroupFlag = false;
            this.orderDistFlag = false;
            this.orderDistTermFlag = true;
            this.orderDistTermAllFlag = false;
            this.orderTermFlag = false;
            this.orderTermAllFlag = false;
            this.lstSelectedOrderIP.Clear();
            this.ClearOrderKind();
        }

        /// <summary>
        /// 시군아이콘 개별 단말 선택창에서 전체 단말을 선택한 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void orderDistTermForm_TermAllSelectEvent(object sender, EventArgs e)
        {
            this.SetOrderDestinationButtonColorOff();
            this.allDestinationFlag = false;
            this.orderTermGroupFlag = false;
            this.orderDistGroupFlag = false;
            this.orderDistFlag = false;
            this.orderDistTermFlag = false;
            this.orderDistTermAllFlag = true;
            this.orderTermFlag = false;
            this.orderTermAllFlag = false;

            if (!this.lstSelectedOrderIP.Contains(NCasUtilityMng.INCasCommUtility.AddIpAddr(this.main.MmfMng.GetDistInfoByCode(this.selectedDistCode).NetIdToString, 0, 0, 0, 255)))
            {
                this.lstSelectedOrderIP.Clear();
                this.lstSelectedOrderIP.Add(NCasUtilityMng.INCasCommUtility.AddIpAddr(this.main.MmfMng.GetDistInfoByCode(this.selectedDistCode).NetIdToString, 0, 0, 0, 255));
            }

            this.ClearOrderKind();
        }

        /// <summary>
        /// 단말 개별을 취소하는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void termDestination_TermDestinationCancelEvent(object sender, EventArgs e)
        {
            if (this.termDestination != null)
            {
                this.termDestination.AddTermDestinationEvent -= new EventHandler<TermSelectEventArgs>(termDestination_AddTermDestinationEvent);
                this.termDestination.RemoveTermDestinationEvent -= new EventHandler<TermSelectEventArgs>(termDestination_RemoveTermDestinationEvent);
                this.termDestination.TermAllCancelEvent -= new EventHandler(termDestination_TermAllCancelEvent);
                this.termDestination.TermAllSelectEvent -= new EventHandler(termDestination_TermAllSelectEvent);
                this.termDestination.TermDestinationCancelEvent -= new EventHandler(termDestination_TermDestinationCancelEvent);
            }

            this.SetOrderDestinationButtonColorOff();
            this.SetOffButtonAny(NCasKeyAction.TermDestination);
            NCasButton ncasBtn = new NCasButton();
            NCasKeyData keyData = new NCasKeyData();
            keyData.KeyActioin = NCasKeyAction.TermDestination;
            keyData.KeyStatus = NCasKeyState.UnCheck;
            ncasBtn.Tag = keyData;
            this.orderTermFlag = false;
            this.realBtn_MouseDown(ncasBtn, new MouseEventArgs(System.Windows.Forms.MouseButtons.Left, 1, 0, 0, 0));
            this.ClearOrderKind();
        }

        /// <summary>
        /// 단말 개별에서 전체 단말을 선택한 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void termDestination_TermAllSelectEvent(object sender, EventArgs e)
        {
            this.SetOrderDestinationButtonColorOff();
            this.allDestinationFlag = false;
            this.orderTermGroupFlag = false;
            this.orderDistGroupFlag = false;
            this.orderDistFlag = false;
            this.orderDistTermFlag = false;
            this.orderDistTermAllFlag = false;
            this.orderTermFlag = false;
            this.orderTermAllFlag = true;

            if (!this.lstSelectedOrderIP.Contains(NCasUtilityMng.INCasCommUtility.MakeNetIdToProvBroadIp(NCasUtilityMng.INCasEtcUtility.GetIPv4())))
            {
                this.lstSelectedOrderIP.Clear();
                this.lstSelectedOrderIP.Add(NCasUtilityMng.INCasCommUtility.MakeNetIdToProvBroadIp(NCasUtilityMng.INCasEtcUtility.GetIPv4()));
            }

            this.ClearOrderKind();
        }

        /// <summary>
        /// 단말 개별에서 전체 단말을 취소하는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void termDestination_TermAllCancelEvent(object sender, EventArgs e)
        {
            this.SetOrderDestinationButtonColorOff();
            this.allDestinationFlag = false;
            this.orderTermGroupFlag = false;
            this.orderDistGroupFlag = false;
            this.orderDistFlag = false;
            this.orderDistTermFlag = false;
            this.orderDistTermAllFlag = false;
            this.orderTermFlag = true;
            this.orderTermAllFlag = false;
            this.lstSelectedOrderIP.Clear();
            this.ClearOrderKind();
        }

        /// <summary>
        /// 단말 개별에서 하나의 단말을 해제하는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void termDestination_RemoveTermDestinationEvent(object sender, TermSelectEventArgs e)
        {
            this.SetOrderDestinationButtonColorOff();
            this.allDestinationFlag = false;
            this.orderTermGroupFlag = false;
            this.orderDistGroupFlag = false;
            this.orderDistFlag = false;
            this.orderDistTermFlag = false;
            this.orderDistTermAllFlag = false;
            this.orderTermFlag = true;

            if (this.orderTermAllFlag == true)
            {
                this.lstSelectedOrderIP.Clear();

                foreach (TermInfo eachTermInfo in this.main.ProvInfo.LstTerms)
                {
                    if (eachTermInfo.UseFlag != NCasDefineUseStatus.Use)
                        continue;

                    if (eachTermInfo.IpAddrToSring == e.TermInfo.IpAddrToSring)
                        continue;

                    if (eachTermInfo.TermFlag != NCasDefineTermKind.TermMutil)
                        continue;

                    if (!this.lstSelectedOrderIP.Contains(eachTermInfo.IpAddrToSring))
                    {
                        this.lstSelectedOrderIP.Add(eachTermInfo.IpAddrToSring);
                    }
                }

                this.orderTermAllFlag = false;
            }
            else
            {
                if (this.lstSelectedOrderIP.Contains(e.TermInfo.IpAddrToSring))
                {
                    this.lstSelectedOrderIP.Remove(e.TermInfo.IpAddrToSring);
                }
            }

            this.ClearOrderKind();
        }

        /// <summary>
        /// 단말 개별에서 하나의 단말을 선택하는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void termDestination_AddTermDestinationEvent(object sender, TermSelectEventArgs e)
        {
            this.SetOrderDestinationButtonColorOff();
            this.allDestinationFlag = false;
            this.orderTermGroupFlag = false;
            this.orderDistGroupFlag = false;
            this.orderDistFlag = false;
            this.orderDistTermFlag = false;
            this.orderDistTermAllFlag = false;
            this.orderTermFlag = true;
            this.orderTermAllFlag = false;

            if (!this.lstSelectedOrderIP.Contains(e.TermInfo.IpAddrToSring))
            {
                this.lstSelectedOrderIP.Add(e.TermInfo.IpAddrToSring);
            }

            this.ClearOrderKind();
        }
        #endregion

        #region 버튼 관련 Method
        /// <summary>
        /// 해제되어 있는 버튼을 선택한다.
        /// </summary>
        /// <param name="keyActionDefine">선택할 버튼</param>
        private void SetOnButton(NCasKeyAction keyActionDefine)
        {
            for (int i = 0; i < this.controlMainPanel.Controls.Count; i++)
            {
                if (this.controlMainPanel.Controls[i] is Label)
                    continue;

                NCasButton ncasBtn = this.controlMainPanel.Controls[i] as NCasButton;
                NCasKeyData btnKeyData = (NCasKeyData)ncasBtn.Tag;

                if (btnKeyData.KeyActioin == keyActionDefine)
                {
                    if (ncasBtn.CheckedValue == false)
                    {
                        this.SetBtnChecked(ncasBtn, true);
                        ncasBtn.ForeColor = Color.FromArgb(86, 169, 255);

                        if (!this.lstSelectedButtons.Contains(ncasBtn))
                        {
                            this.lstSelectedButtons.Add(ncasBtn);
                        }
                    }
                }
            }

            for (int i = 0; i < this.disasterBroadOrderPanel.Controls.Count; i++)
            {
                if (this.disasterBroadOrderPanel.Controls[i] is Label)
                    continue;

                NCasButton ncasBtn = this.disasterBroadOrderPanel.Controls[i] as NCasButton;
                NCasKeyData btnKeyData = (NCasKeyData)ncasBtn.Tag;

                if (btnKeyData.KeyActioin == keyActionDefine)
                {
                    if (ncasBtn.CheckedValue == false)
                    {
                        this.SetBtnChecked(ncasBtn, true);
                        ncasBtn.ForeColor = Color.FromArgb(86, 169, 255);

                        if (!this.lstSelectedButtons.Contains(ncasBtn))
                        {
                            this.lstSelectedButtons.Add(ncasBtn);
                        }
                    }
                }
            }

            for (int i = 0; i < this.distMapPanel.Controls.Count; i++)
            {
                if (this.distMapPanel.Controls[i] is NCasPanel)
                    continue;

                DistIconUserControl userControl = this.distMapPanel.Controls[i] as DistIconUserControl;
                NCasButton ncasBtn = userControl.DistIcon;
                NCasKeyData btnKeyData = (NCasKeyData)ncasBtn.Tag;

                if (btnKeyData.KeyActioin == keyActionDefine)
                {
                    if (ncasBtn.CheckedValue == false)
                    {
                        this.SetBtnChecked(ncasBtn, true);
                        ncasBtn.ForeColor = Color.FromArgb(86, 169, 255);

                        if (!this.lstSelectedButtons.Contains(ncasBtn))
                        {
                            this.lstSelectedButtons.Add(ncasBtn);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 선택되어 있는 버튼 선택을 해제한다.
        /// </summary>
        /// <param name="keyActionDefine">해제할 버튼</param>
        private void SetOffButton(NCasKeyAction keyActionDefine)
        {
            for (int i = 0; i < this.controlMainPanel.Controls.Count; i++)
            {
                if (this.controlMainPanel.Controls[i] is Label)
                    continue;

                NCasButton ncasBtn = this.controlMainPanel.Controls[i] as NCasButton;
                NCasKeyData btnKeyData = (NCasKeyData)ncasBtn.Tag;

                if (btnKeyData.KeyActioin == keyActionDefine)
                {
                    if (ncasBtn.CheckedValue == true)
                    {
                        this.SetBtnChecked(ncasBtn, false);
                        ncasBtn.ForeColor = Color.FromArgb(203, 203, 203);

                        if (this.lstSelectedButtons.Contains(ncasBtn))
                        {
                            this.lstSelectedButtons.Remove(ncasBtn);
                        }

                        if (NCasAnimator.ContainsItem(ncasBtn))
                        {
                            NCasAnimator.RemoveItem(ncasBtn);
                        }
                    }
                }
            }

            for (int i = 0; i < this.disasterBroadOrderPanel.Controls.Count; i++)
            {
                if (this.disasterBroadOrderPanel.Controls[i] is Label)
                    continue;

                NCasButton ncasBtn = this.disasterBroadOrderPanel.Controls[i] as NCasButton;
                NCasKeyData btnKeyData = (NCasKeyData)ncasBtn.Tag;

                if (btnKeyData.KeyActioin == keyActionDefine)
                {
                    if (ncasBtn.CheckedValue == true)
                    {
                        this.SetBtnChecked(ncasBtn, false);
                        ncasBtn.ForeColor = Color.FromArgb(203, 203, 203);

                        if (this.lstSelectedButtons.Contains(ncasBtn))
                        {
                            this.lstSelectedButtons.Remove(ncasBtn);
                        }

                        if (NCasAnimator.ContainsItem(ncasBtn))
                        {
                            NCasAnimator.RemoveItem(ncasBtn);
                        }
                    }
                }
            }

            for (int i = 0; i < this.distMapPanel.Controls.Count; i++)
            {
                if (this.distMapPanel.Controls[i] is NCasPanel)
                    continue;

                DistIconUserControl userControl = this.distMapPanel.Controls[i] as DistIconUserControl;
                NCasButton ncasBtn = userControl.DistIcon;
                NCasKeyData btnKeyData = (NCasKeyData)ncasBtn.Tag;

                if (btnKeyData.KeyActioin == keyActionDefine)
                {
                    if (ncasBtn.CheckedValue == true)
                    {
                        this.SetBtnChecked(ncasBtn, false);
                        ncasBtn.ForeColor = Color.FromArgb(203, 203, 203);

                        if (this.lstSelectedButtons.Contains(ncasBtn))
                        {
                            this.lstSelectedButtons.Remove(ncasBtn);
                        }

                        if (NCasAnimator.ContainsItem(ncasBtn))
                        {
                            NCasAnimator.RemoveItem(ncasBtn);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 버튼 상태와 관계없이 버튼 선택을 해제한다.
        /// </summary>
        /// <param name="keyActionDefine">해제할 버튼</param>
        private void SetOffButtonAny(NCasKeyAction keyActionDefine)
        {
            for (int i = 0; i < this.controlMainPanel.Controls.Count; i++)
            {
                if (this.controlMainPanel.Controls[i] is Label)
                    continue;

                NCasButton ncasBtn = this.controlMainPanel.Controls[i] as NCasButton;
                NCasKeyData btnKeyData = (NCasKeyData)ncasBtn.Tag;

                if (btnKeyData.KeyActioin == keyActionDefine)
                {
                    this.SetBtnChecked(ncasBtn, false);
                    ncasBtn.ForeColor = Color.FromArgb(203, 203, 203);

                    if (this.lstSelectedButtons.Contains(ncasBtn))
                    {
                        this.lstSelectedButtons.Remove(ncasBtn);
                    }

                    if (NCasAnimator.ContainsItem(ncasBtn))
                    {
                        NCasAnimator.RemoveItem(ncasBtn);
                    }
                }
            }

            for (int i = 0; i < this.disasterBroadOrderPanel.Controls.Count; i++)
            {
                if (this.disasterBroadOrderPanel.Controls[i] is Label)
                    continue;

                NCasButton ncasBtn = this.disasterBroadOrderPanel.Controls[i] as NCasButton;
                NCasKeyData btnKeyData = (NCasKeyData)ncasBtn.Tag;

                if (btnKeyData.KeyActioin == keyActionDefine)
                {
                    this.SetBtnChecked(ncasBtn, false);
                    ncasBtn.ForeColor = Color.FromArgb(203, 203, 203);

                    if (this.lstSelectedButtons.Contains(ncasBtn))
                    {
                        this.lstSelectedButtons.Remove(ncasBtn);
                    }

                    if (NCasAnimator.ContainsItem(ncasBtn))
                    {
                        NCasAnimator.RemoveItem(ncasBtn);
                    }
                }
            }

            for (int i = 0; i < this.distMapPanel.Controls.Count; i++)
            {
                if (this.distMapPanel.Controls[i] is NCasPanel)
                    continue;

                DistIconUserControl userControl = this.distMapPanel.Controls[i] as DistIconUserControl;
                NCasButton ncasBtn = userControl.DistIcon;
                NCasKeyData btnKeyData = (NCasKeyData)ncasBtn.Tag;

                if (btnKeyData.KeyActioin == keyActionDefine)
                {
                    this.SetBtnChecked(ncasBtn, false);
                    ncasBtn.ForeColor = Color.FromArgb(203, 203, 203);

                    if (this.lstSelectedButtons.Contains(ncasBtn))
                    {
                        this.lstSelectedButtons.Remove(ncasBtn);
                    }

                    if (NCasAnimator.ContainsItem(ncasBtn))
                    {
                        NCasAnimator.RemoveItem(ncasBtn);
                    }
                }
            }
        }

        /// <summary>
        /// 발령종류에 따라 버튼 Normal 색을 셋팅한다.
        /// </summary>
        /// <param name="ncasBtn"></param>
        /// <param name="orderKind"></param>
        private void SetOrderKindButtonColor(NCasButton ncasBtn, NCasDefineOrderKind orderKind)
        {
            switch (orderKind)
            {
                case NCasDefineOrderKind.AlarmStandby:
                    ncasBtn.Image = this.orderKindImageList.Images[2];
                    break;

                case NCasDefineOrderKind.AlarmWatch:
                    ncasBtn.Image = this.orderKindImageList.Images[4];
                    break;

                case NCasDefineOrderKind.AlarmAttack:
                    ncasBtn.Image = this.orderKindImageList.Images[7];
                    break;

                case NCasDefineOrderKind.AlarmBiochemist:
                    ncasBtn.Image = this.orderKindImageList.Images[10];
                    break;

                case NCasDefineOrderKind.DisasterBroadcast:
                    ncasBtn.Image = this.orderKindImageList.Images[6];
                    break;

                case NCasDefineOrderKind.DisasterWatch:
                    ncasBtn.Image = this.orderKindImageList.Images[5];
                    break;

                case NCasDefineOrderKind.AlarmCancel:
                    ncasBtn.Image = this.orderKindImageList.Images[9];
                    break;

                default:
                    ncasBtn.Image = this.orderKindImageList.Images[3];
                    break;
            }
        }

        /// <summary>
        /// 발령종류에 따라 시군아이콘 버튼 Normal 색을 셋팅한다.
        /// </summary>
        /// <param name="ncasBtn"></param>
        /// <param name="orderKind"></param>
        private void SetOrderKindButtonColorDistIcon(NCasButton ncasBtn, NCasDefineOrderKind orderKind)
        {
            switch (orderKind)
            {
                case NCasDefineOrderKind.AlarmStandby:
                    ncasBtn.Image = this.orderKindImageListDistIcon.Images[2];
                    break;

                case NCasDefineOrderKind.AlarmWatch:
                    ncasBtn.Image = this.orderKindImageListDistIcon.Images[4];
                    break;

                case NCasDefineOrderKind.AlarmAttack:
                    ncasBtn.Image = this.orderKindImageListDistIcon.Images[7];
                    break;

                case NCasDefineOrderKind.AlarmBiochemist:
                    ncasBtn.Image = this.orderKindImageListDistIcon.Images[10];
                    break;

                case NCasDefineOrderKind.DisasterBroadcast:
                    ncasBtn.Image = this.orderKindImageListDistIcon.Images[6];
                    break;

                case NCasDefineOrderKind.DisasterWatch:
                    ncasBtn.Image = this.orderKindImageListDistIcon.Images[5];
                    break;

                case NCasDefineOrderKind.AlarmCancel:
                    ncasBtn.Image = this.orderKindImageListDistIcon.Images[9];
                    break;

                default:
                    ncasBtn.Image = this.orderKindImageListDistIcon.Images[3];
                    break;
            }
        }

        /// <summary>
        /// 발령대상의 모든 버튼을 발령종류에 따라 Color를 셋팅한다.
        /// </summary>
        /// <param name="orderKind"></param>
        private void SetOrderKindButtonColorProvAll(NCasDefineOrderKind orderKind)
        {
            for (int i = 0; i < this.controlMainPanel.Controls.Count; i++)
            {
                if (this.controlMainPanel.Controls[i] is Label)
                    continue;

                NCasButton ncasBtn = this.controlMainPanel.Controls[i] as NCasButton;
                NCasKeyData btnKeyData = (NCasKeyData)ncasBtn.Tag;

                if (btnKeyData.KeyActioin == NCasKeyAction.ProveAllDestination || btnKeyData.KeyActioin == NCasKeyAction.TermDestination ||
                    btnKeyData.KeyActioin == NCasKeyAction.TermGroupDestination || btnKeyData.KeyActioin == NCasKeyAction.DistGroupDestination ||
                    btnKeyData.KeyActioin == NCasKeyAction.DistOneDestination)
                {
                    this.SetBtnChecked(ncasBtn, false);
                    this.SetOrderKindButtonColor(ncasBtn, orderKind);
                }
            }

            for (int i = 0; i < this.distMapPanel.Controls.Count; i++)
            {
                if (this.distMapPanel.Controls[i] is NCasPanel)
                    continue;

                DistIconUserControl userControl = this.distMapPanel.Controls[i] as DistIconUserControl;
                NCasButton ncasBtn = userControl.DistIcon;
                NCasKeyData btnKeyData = (NCasKeyData)ncasBtn.Tag;

                if (btnKeyData.KeyActioin == NCasKeyAction.ProveAllDestination || btnKeyData.KeyActioin == NCasKeyAction.TermDestination ||
                    btnKeyData.KeyActioin == NCasKeyAction.TermGroupDestination || btnKeyData.KeyActioin == NCasKeyAction.DistGroupDestination ||
                    btnKeyData.KeyActioin == NCasKeyAction.DistOneDestination)
                {
                    this.SetBtnChecked(ncasBtn, false);
                    this.SetOrderKindButtonColorDistIcon(ncasBtn, orderKind);
                }
            }
        }

        /// <summary>
        /// 발령대상의 모든 버튼의 깜박임을 해제한다.
        /// </summary>
        private void SetOrderDestinationButtonColorOff()
        {
            foreach (NCasButton btn in this.lstSelectedButtons)
            {
                NCasKeyData keyData = (NCasKeyData)btn.Tag;

                if (keyData.KeyActioin == NCasKeyAction.ProveAllDestination || keyData.KeyActioin == NCasKeyAction.TermDestination ||
                    keyData.KeyActioin == NCasKeyAction.TermGroupDestination || keyData.KeyActioin == NCasKeyAction.DistGroupDestination ||
                    keyData.KeyActioin == NCasKeyAction.DistOneDestination)
                {
                    btn.LstAnimation.Images.Clear();

                    if (NCasAnimator.ContainsItem(btn))
                    {
                        NCasAnimator.RemoveItem(btn);
                    }
                }
            }
        }

        /// <summary>
        /// 버튼의 체크상태를 셋팅한다.
        /// </summary>
        /// <param name="check"></param>
        private void SetBtnChecked(NCasButton btn, bool check)
        {
            if (check)
            {
                btn.CheckedValue = check;
                btn.ForeColor = Color.FromArgb(86, 169, 255);
            }
            else
            {
                btn.CheckedValue = check;

                if ((btn.Tag as NCasKeyData).KeyActioin == NCasKeyAction.Real)
                {
                    //btn.ForeColor = Color.Red;
                    btn.ForeColor = Color.FromArgb(203, 203, 203);
                }
                else
                {
                    btn.ForeColor = Color.FromArgb(203, 203, 203);
                }
            }
        }

        /// <summary>
        /// 발령종류에 따라 버튼을 셋팅하고 깜빡인다.
        /// </summary>
        /// <param name="orderKind"></param>
        private void SetOrderKindButtonColorOn(NCasDefineOrderKind orderKind)
        {
            if (orderKind == NCasDefineOrderKind.None)
                return;

            foreach (NCasButton btn in this.lstSelectedButtons)
            {
                NCasKeyData keyData = (NCasKeyData)btn.Tag;

                if (keyData.KeyActioin == NCasKeyAction.ProveAllDestination || keyData.KeyActioin == NCasKeyAction.TermDestination ||
                    keyData.KeyActioin == NCasKeyAction.TermGroupDestination || keyData.KeyActioin == NCasKeyAction.DistGroupDestination)
                {
                    if (btn.CheckedValue == true)
                    {
                        this.SetBtnChecked(btn, false);
                    }

                    btn.LstAnimation.Images.Clear();
                    btn.LstAnimation.Images.Add(this.orderKindImageList.Images[3]);

                    switch (orderKind)
                    {
                        case NCasDefineOrderKind.AlarmStandby:
                            btn.LstAnimation.Images.Add(this.orderKindImageList.Images[2]);
                            btn.ForeColor = Color.FromArgb(144, 233, 84);
                            break;

                        case NCasDefineOrderKind.AlarmAttack:
                            btn.LstAnimation.Images.Add(this.orderKindImageList.Images[7]);
                            btn.ForeColor = Color.FromArgb(255, 12, 14);
                            break;

                        case NCasDefineOrderKind.AlarmWatch:
                            btn.LstAnimation.Images.Add(this.orderKindImageList.Images[4]);
                            btn.ForeColor = Color.FromArgb(249, 164, 82);
                            break;

                        case NCasDefineOrderKind.AlarmBiochemist:
                            btn.LstAnimation.Images.Add(this.orderKindImageList.Images[10]);
                            btn.ForeColor = Color.FromArgb(255, 249, 64);
                            break;

                        case NCasDefineOrderKind.DisasterBroadcast:
                            btn.LstAnimation.Images.Add(this.orderKindImageList.Images[6]);
                            btn.ForeColor = Color.FromArgb(210, 56, 190);
                            break;

                        case NCasDefineOrderKind.DisasterWatch:
                            btn.LstAnimation.Images.Add(this.orderKindImageList.Images[5]);
                            btn.ForeColor = Color.FromArgb(245, 117, 102);
                            break;

                        case NCasDefineOrderKind.AlarmCancel:
                            btn.LstAnimation.Images.Add(this.orderKindImageList.Images[9]);
                            btn.ForeColor = Color.FromArgb(255, 255, 255);
                            break;

                        default:
                            break;
                    }

                    if (!NCasAnimator.ContainsItem(btn))
                    {
                        NCasAnimator.AddItem(btn);
                    }
                }
                else if (keyData.KeyActioin == NCasKeyAction.DistOneDestination)
                {
                    if (btn.CheckedValue == true)
                    {
                        this.SetBtnChecked(btn, false);
                    }

                    btn.LstAnimation.Images.Clear();
                    btn.LstAnimation.Images.Add(this.orderKindImageListDistIcon.Images[3]);

                    switch (orderKind)
                    {
                        case NCasDefineOrderKind.AlarmStandby:
                            //btn.LstAnimation.Images.Add(this.orderKindImageListDistIcon.Images[2]);
                            btn.ForeColor = Color.FromArgb(144, 233, 84);
                            break;

                        case NCasDefineOrderKind.AlarmAttack:
                            //btn.LstAnimation.Images.Add(this.orderKindImageListDistIcon.Images[7]);
                            btn.ForeColor = Color.FromArgb(255, 12, 14);
                            break;

                        case NCasDefineOrderKind.AlarmWatch:
                            //btn.LstAnimation.Images.Add(this.orderKindImageListDistIcon.Images[4]);
                            btn.ForeColor = Color.FromArgb(249, 164, 82);
                            break;

                        case NCasDefineOrderKind.AlarmBiochemist:
                            //btn.LstAnimation.Images.Add(this.orderKindImageListDistIcon.Images[10]);
                            btn.ForeColor = Color.FromArgb(255, 249, 64);
                            break;

                        case NCasDefineOrderKind.DisasterBroadcast:
                            //btn.LstAnimation.Images.Add(this.orderKindImageListDistIcon.Images[6]);
                            btn.ForeColor = Color.FromArgb(210, 56, 190);
                            break;

                        case NCasDefineOrderKind.DisasterWatch:
                            //btn.LstAnimation.Images.Add(this.orderKindImageListDistIcon.Images[5]);
                            btn.ForeColor = Color.FromArgb(245, 117, 102);
                            break;

                        case NCasDefineOrderKind.AlarmCancel:
                            //btn.LstAnimation.Images.Add(this.orderKindImageListDistIcon.Images[9]);
                            btn.ForeColor = Color.FromArgb(255, 255, 255);
                            break;

                        default:
                            break;
                    }

                    if (!NCasAnimator.ContainsItem(btn))
                    {
                        //NCasAnimator.AddItem(btn);
                    }
                }
            }
        }

        /// <summary>
        /// 발령종류 버튼을 모두 해제한다.
        /// </summary>
        private void ClearOrderKind()
        {
            this.selectedOrderKind = NCasDefineOrderKind.None;
            this.selectedDisasterBroadKind = DisasterBroadKind.None;
            this.SetOffButton(NCasKeyAction.Ready);
            this.SetOffButton(NCasKeyAction.Attack);
            this.SetOffButton(NCasKeyAction.Watch);
            this.SetOffButton(NCasKeyAction.Biological);
            this.SetOffButton(NCasKeyAction.DisasterBroad);
            this.SetOffButton(NCasKeyAction.DisasterWatch);
            this.SetOffButton(NCasKeyAction.Clear);
            this.SetOffButton(NCasKeyAction.MicOrder);
            this.SetOffButton(NCasKeyAction.TtsOrder);
            this.SetOffButton(NCasKeyAction.MsgOrder);
        }
        #endregion

        #region 그룹 버튼 클릭 시 발령대상에 데이터 관리
        /// <summary>
        /// 그룹 버튼 클릭 시 발령대상에 데이터 제거
        /// </summary>
        /// <param name="selectBtn"></param>
        private void GroupBtnDataRemove(NCasButton selectBtn)
        {
            switch (selectBtn.Name)
            {
                case "termGroupBtn1":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[0].LstGroupData)
                    {
                        if (this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Remove(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "termGroupBtn2":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[1].LstGroupData)
                    {
                        if (this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Remove(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "termGroupBtn3":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[2].LstGroupData)
                    {
                        if (this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Remove(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "termGroupBtn4":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[3].LstGroupData)
                    {
                        if (this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Remove(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "termGroupBtn5":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[4].LstGroupData)
                    {
                        if (this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Remove(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "termGroupBtn6":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[5].LstGroupData)
                    {
                        if (this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Remove(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "termGroupBtn7":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[6].LstGroupData)
                    {
                        if (this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Remove(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "termGroupBtn8":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[7].LstGroupData)
                    {
                        if (this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Remove(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "termGroupBtn9":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[8].LstGroupData)
                    {
                        if (this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Remove(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "termGroupBtn10":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[9].LstGroupData)
                    {
                        if (this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Remove(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "distGroupBtn1":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[10].LstGroupData)
                    {
                        if (this.lstSelectedOrderIP.Contains(NCasUtilityMng.INCasCommUtility.AddIpAddr(eachGroupData.IpAddr, 0, 0, 0, 255)))
                        {
                            this.lstSelectedOrderIP.Remove(NCasUtilityMng.INCasCommUtility.AddIpAddr(eachGroupData.IpAddr, 0, 0, 0, 255));
                        }
                    }
                    break;

                case "distGroupBtn2":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[11].LstGroupData)
                    {
                        if (this.lstSelectedOrderIP.Contains(NCasUtilityMng.INCasCommUtility.AddIpAddr(eachGroupData.IpAddr, 0, 0, 0, 255)))
                        {
                            this.lstSelectedOrderIP.Remove(NCasUtilityMng.INCasCommUtility.AddIpAddr(eachGroupData.IpAddr, 0, 0, 0, 255));
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 그룹 버튼 클릭 시 발령대상에 데이터 넣기
        /// </summary>
        /// <param name="selectBtn"></param>
        private void GroupBtnDataAdd(NCasButton selectBtn)
        {
            switch (selectBtn.Name)
            {
                case "termGroupBtn1":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[0].LstGroupData)
                    {
                        if (!this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Add(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "termGroupBtn2":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[1].LstGroupData)
                    {
                        if (!this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Add(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "termGroupBtn3":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[2].LstGroupData)
                    {
                        if (!this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Add(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "termGroupBtn4":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[3].LstGroupData)
                    {
                        if (!this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Add(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "termGroupBtn5":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[4].LstGroupData)
                    {
                        if (!this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Add(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "termGroupBtn6":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[5].LstGroupData)
                    {
                        if (!this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Add(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "termGroupBtn7":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[6].LstGroupData)
                    {
                        if (!this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Add(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "termGroupBtn8":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[7].LstGroupData)
                    {
                        if (!this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Add(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "termGroupBtn9":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[8].LstGroupData)
                    {
                        if (!this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Add(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "termGroupBtn10":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[9].LstGroupData)
                    {
                        if (!this.lstSelectedOrderIP.Contains(eachGroupData.IpAddr))
                        {
                            this.lstSelectedOrderIP.Add(eachGroupData.IpAddr);
                        }
                    }
                    break;

                case "distGroupBtn1":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[10].LstGroupData)
                    {
                        if (!this.lstSelectedOrderIP.Contains(NCasUtilityMng.INCasCommUtility.AddIpAddr(eachGroupData.IpAddr, 0, 0, 0, 255)))
                        {
                            this.lstSelectedOrderIP.Add(NCasUtilityMng.INCasCommUtility.AddIpAddr(eachGroupData.IpAddr, 0, 0, 0, 255));
                        }
                    }
                    break;

                case "distGroupBtn2":
                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[11].LstGroupData)
                    {
                        if (!this.lstSelectedOrderIP.Contains(NCasUtilityMng.INCasCommUtility.AddIpAddr(eachGroupData.IpAddr, 0, 0, 0, 255)))
                        {
                            this.lstSelectedOrderIP.Add(NCasUtilityMng.INCasCommUtility.AddIpAddr(eachGroupData.IpAddr, 0, 0, 0, 255));
                        }
                    }
                    break;
            }
        }
        #endregion

        #region 경보조작부 버튼 클릭
        /// <summary>
        /// 경보조작부 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void realBtn_MouseDown(object sender, MouseEventArgs e)
        {
            NCasButton ncasBtn = null;
            NCasKeyData keyData = null;
            bool isLocal = true;

            if (sender is NCasButton)
            {
                ncasBtn = sender as NCasButton;
                keyData = (NCasKeyData)ncasBtn.Tag;

                if (ncasBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(ncasBtn, true);
                    keyData.KeyStatus = NCasKeyState.Check;
                }
                else
                {
                    this.SetBtnChecked(ncasBtn, false);
                    keyData.KeyStatus = NCasKeyState.UnCheck;
                }
            }

            if (ncasBtn == null)
            {
                NCasLoggingMng.ILogging.WriteLog("OrderView19201080", "realBtn_MouseDown.ncasBtn is null");
                return;
            }

            switch (keyData.KeyActioin)
            {
                case NCasKeyAction.Real: //실제
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        if (isLocal == true)
                        {
                            if (this.realButtonFromPlc == true)
                            {
                                this.CheckKeyRealFromDual(ncasBtn);
                            }
                            else
                            {
                                this.CheckKeyReal(ncasBtn);
                            }
                        }
                        else //확장을 생각해서..
                        {
                        }
                    }
                    break;

                case NCasKeyAction.Test: //시험
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.CheckKeyTest(ncasBtn);
                    }
                    break;

                case NCasKeyAction.Line: //유선
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.CheckKeyLine(ncasBtn);
                    }
                    break;

                case NCasKeyAction.Sate: //위성
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.CheckKeySate(ncasBtn);
                    }
                    break;

                case NCasKeyAction.ProveAllDestination: //시도 전체 (발령 대상)
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.CheckKeyProveAllDestination(ncasBtn);
                    }
                    break;

                case NCasKeyAction.TermDestination: //단말 개별 (발령 대상)
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.CheckKeyTermDestination(ncasBtn);
                    }
                    break;

                case NCasKeyAction.TermGroupDestination: //단말 그룹 (발령 대상)
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.CheckKeyTermGroupDestination(ncasBtn);
                    }
                    else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        using (OrderGroupEditForm orderGroupEditForm = new OrderGroupEditForm(this, 1, ncasBtn))
                        {
                            orderGroupEditForm.ShowDialog(this);
                        }

                        this.termGroupBtn1.Text = GroupContentMng.LstGroupContent[0].Title;
                        this.termGroupBtn2.Text = GroupContentMng.LstGroupContent[1].Title;
                        this.termGroupBtn3.Text = GroupContentMng.LstGroupContent[2].Title;
                        this.termGroupBtn4.Text = GroupContentMng.LstGroupContent[3].Title;
                        this.termGroupBtn5.Text = GroupContentMng.LstGroupContent[4].Title;
                        this.termGroupBtn6.Text = GroupContentMng.LstGroupContent[5].Title;
                        this.termGroupBtn7.Text = GroupContentMng.LstGroupContent[6].Title;
                        this.termGroupBtn8.Text = GroupContentMng.LstGroupContent[7].Title;
                        this.termGroupBtn9.Text = GroupContentMng.LstGroupContent[8].Title;
                        this.termGroupBtn10.Text = GroupContentMng.LstGroupContent[9].Title;
                        this.distGroupBtn1.Text = GroupContentMng.LstGroupContent[10].Title;
                        this.distGroupBtn2.Text = GroupContentMng.LstGroupContent[11].Title;
                    }
                    break;

                case NCasKeyAction.DistGroupDestination: //시군 그룹 (발령 대상)
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.CheckKeyDistGroupDestination(ncasBtn);
                    }
                    else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        using (OrderGroupEditForm orderGroupEditForm = new OrderGroupEditForm(this, 2, ncasBtn))
                        {
                            orderGroupEditForm.ShowDialog(this);
                        }

                        this.termGroupBtn1.Text = GroupContentMng.LstGroupContent[0].Title;
                        this.termGroupBtn2.Text = GroupContentMng.LstGroupContent[1].Title;
                        this.termGroupBtn3.Text = GroupContentMng.LstGroupContent[2].Title;
                        this.termGroupBtn4.Text = GroupContentMng.LstGroupContent[3].Title;
                        this.termGroupBtn5.Text = GroupContentMng.LstGroupContent[4].Title;
                        this.termGroupBtn6.Text = GroupContentMng.LstGroupContent[5].Title;
                        this.termGroupBtn7.Text = GroupContentMng.LstGroupContent[6].Title;
                        this.termGroupBtn8.Text = GroupContentMng.LstGroupContent[7].Title;
                        this.termGroupBtn9.Text = GroupContentMng.LstGroupContent[8].Title;
                        this.termGroupBtn10.Text = GroupContentMng.LstGroupContent[9].Title;
                        this.distGroupBtn1.Text = GroupContentMng.LstGroupContent[10].Title;
                        this.distGroupBtn2.Text = GroupContentMng.LstGroupContent[11].Title;
                    }
                    break;

                case NCasKeyAction.DistOneDestination: //시군 아이콘 (발령 대상)
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.CheckKeyDistOneDestination(ncasBtn);
                    }
                    else if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        this.CheckKeyDistOneDestinationRight(ncasBtn);
                    }
                    break;

                case NCasKeyAction.Ready: //예비
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.CheckKeyReady(ncasBtn);
                    }
                    break;

                case NCasKeyAction.Watch: //경계
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.CheckKeyWatch(ncasBtn);
                    }
                    break;

                case NCasKeyAction.Attack: //공습
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.CheckKeyAttack(ncasBtn);
                    }
                    break;

                case NCasKeyAction.Biological: //화생방
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.CheckKeyBiological(ncasBtn);
                    }
                    break;

                case NCasKeyAction.DisasterWatch: //재난위험
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.CheckKeyDisasterWatch(ncasBtn);
                    }
                    break;

                case NCasKeyAction.DisasterBroad: //재난경계(재해방송)
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.CheckKeyDisasterBroad(ncasBtn);
                    }
                    break;

                case NCasKeyAction.MicOrder: //재난경계 MIC
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.CheckKeyMicOrder(ncasBtn);
                    }
                    break;

                case NCasKeyAction.TtsOrder: //재난경계 TTS
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.CheckKeyTtsOrder(ncasBtn);
                    }
                    break;

                case NCasKeyAction.MsgOrder: //재난경계 저장메시지
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.CheckKeyMsgOrder(ncasBtn);
                    }
                    break;

                case NCasKeyAction.Clear: //해제
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.CheckKeyClear(ncasBtn);
                    }
                    break;

                case NCasKeyAction.Cancel: //선택 취소
                    this.CheckKeyCancel();
                    break;

                case NCasKeyAction.Confirm: //확인
                    if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        this.CheckKeyConfirm(ncasBtn);
                    }
                    break;
            }

            if (isLocal == true)
            {
                if (keyData.KeyActioin != NCasKeyAction.LampTest)
                {
                    NCasProtocolBase protocolBase = NCasProtocolFactory.CreateCasProtocolForTc148(NCasDefineOperPanelSubCode.KeyLampOutputSet);
                    NCasProtocolTc148Sub15 protocol148Sub15 = protocolBase as NCasProtocolTc148Sub15;

                    protocol148Sub15.Confirm = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.GroupAll = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.Group2 = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.Group3 = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.Group4 = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.Group5 = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.Group6 = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.Group7 = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.Group8 = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.Group9 = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.Group10 = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.KindAttack = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.KindBiochemist = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.KindCancel = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.KindDisasterDanger = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.KindDisasterWatch = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.KindDisasterWatchMic = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.KindDisasterWatchMsg = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.KindDisasterWatchSpare = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.KindDisasterWatchTts = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.KindReady = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.KindWatch = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.MediaLine = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.MediaSate = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.MisOperation = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.ModeReal = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.ModeTest = NCasDefineOnOffBlinkStatus.Off;
                    protocol148Sub15.RealOrderKey = NCasDefineOnOffBlinkStatus.Off;

                    for (int i = 0; i < this.lstSelectedButtons.Count; i++)
                    {
                        if ((this.lstSelectedButtons[i].Tag as NCasKeyData).KeyActioin == NCasKeyAction.Real)
                        {
                            protocol148Sub15.ModeReal = NCasDefineOnOffBlinkStatus.On;
                        }
                        else if ((this.lstSelectedButtons[i].Tag as NCasKeyData).KeyActioin == NCasKeyAction.Test)
                        {
                            protocol148Sub15.ModeTest = NCasDefineOnOffBlinkStatus.On;
                        }
                        else if ((this.lstSelectedButtons[i].Tag as NCasKeyData).KeyActioin == NCasKeyAction.Line)
                        {
                            protocol148Sub15.MediaLine = NCasDefineOnOffBlinkStatus.On;
                        }
                        else if ((this.lstSelectedButtons[i].Tag as NCasKeyData).KeyActioin == NCasKeyAction.Sate)
                        {
                            protocol148Sub15.MediaSate = NCasDefineOnOffBlinkStatus.On;
                        }
                        else if ((this.lstSelectedButtons[i].Tag as NCasKeyData).KeyActioin == NCasKeyAction.ProveAllDestination)
                        {
                            protocol148Sub15.GroupAll = NCasDefineOnOffBlinkStatus.On;
                        }
                        else if ((this.lstSelectedButtons[i].Tag as NCasKeyData).KeyActioin == NCasKeyAction.Ready)
                        {
                            protocol148Sub15.KindReady = NCasDefineOnOffBlinkStatus.On;
                        }
                        else if ((this.lstSelectedButtons[i].Tag as NCasKeyData).KeyActioin == NCasKeyAction.Attack)
                        {
                            protocol148Sub15.KindAttack = NCasDefineOnOffBlinkStatus.On;
                        }
                        else if ((this.lstSelectedButtons[i].Tag as NCasKeyData).KeyActioin == NCasKeyAction.Watch)
                        {
                            protocol148Sub15.KindWatch = NCasDefineOnOffBlinkStatus.On;
                        }
                        else if ((this.lstSelectedButtons[i].Tag as NCasKeyData).KeyActioin == NCasKeyAction.Biological)
                        {
                            protocol148Sub15.KindBiochemist = NCasDefineOnOffBlinkStatus.On;
                        }
                        else if ((this.lstSelectedButtons[i].Tag as NCasKeyData).KeyActioin == NCasKeyAction.Clear)
                        {
                            protocol148Sub15.KindCancel = NCasDefineOnOffBlinkStatus.On;
                        }
                        else if ((this.lstSelectedButtons[i].Tag as NCasKeyData).KeyActioin == NCasKeyAction.DisasterBroad)
                        {
                            protocol148Sub15.KindDisasterWatch = NCasDefineOnOffBlinkStatus.On;
                        }
                        else if ((this.lstSelectedButtons[i].Tag as NCasKeyData).KeyActioin == NCasKeyAction.DisasterWatch)
                        {
                            protocol148Sub15.KindDisasterDanger = NCasDefineOnOffBlinkStatus.On;
                        }
                        else if ((this.lstSelectedButtons[i].Tag as NCasKeyData).KeyActioin == NCasKeyAction.MicOrder)
                        {
                            protocol148Sub15.KindDisasterWatchMic = NCasDefineOnOffBlinkStatus.On;
                        }
                        else if ((this.lstSelectedButtons[i].Tag as NCasKeyData).KeyActioin == NCasKeyAction.TtsOrder)
                        {
                            protocol148Sub15.KindDisasterWatchTts = NCasDefineOnOffBlinkStatus.On;
                        }
                        else if ((this.lstSelectedButtons[i].Tag as NCasKeyData).KeyActioin == NCasKeyAction.MsgOrder)
                        {
                            protocol148Sub15.KindDisasterWatchMsg = NCasDefineOnOffBlinkStatus.On;
                        }
                        else if ((this.lstSelectedButtons[i].Tag as NCasKeyData).KeyActioin == NCasKeyAction.Confirm)
                        {
                            protocol148Sub15.Confirm = NCasDefineOnOffBlinkStatus.On;
                        }
                    }

                    if (this.orderTermGroupFlag)
                    {
                        if (this.termGroupBtn1.CheckedValue)
                        {
                            protocol148Sub15.Group2 = NCasDefineOnOffBlinkStatus.On;
                        }
                        if (this.termGroupBtn2.CheckedValue)
                        {
                            protocol148Sub15.Group3 = NCasDefineOnOffBlinkStatus.On;
                        }
                        if (this.termGroupBtn3.CheckedValue)
                        {
                            protocol148Sub15.Group4 = NCasDefineOnOffBlinkStatus.On;
                        }
                        if (this.termGroupBtn4.CheckedValue)
                        {
                            protocol148Sub15.Group5 = NCasDefineOnOffBlinkStatus.On;
                        }
                        if (this.termGroupBtn5.CheckedValue)
                        {
                            protocol148Sub15.Group6 = NCasDefineOnOffBlinkStatus.On;
                        }
                        if (this.termGroupBtn6.CheckedValue)
                        {
                            protocol148Sub15.Group7 = NCasDefineOnOffBlinkStatus.On;
                        }
                        if (this.termGroupBtn7.CheckedValue)
                        {
                            protocol148Sub15.Group8 = NCasDefineOnOffBlinkStatus.On;
                        }
                        if (this.termGroupBtn8.CheckedValue)
                        {
                            protocol148Sub15.Group9 = NCasDefineOnOffBlinkStatus.On;
                        }
                        if (this.termGroupBtn9.CheckedValue)
                        {
                            protocol148Sub15.Group10 = NCasDefineOnOffBlinkStatus.On;
                        }
                    }

                    NCasProtocolFactory.MakeUdpFrame(protocol148Sub15);
                    this.main.SetPlcKeyData(protocol148Sub15);
                }
            }

            if (keyData.KeyActioin == NCasKeyAction.Confirm)
            {
                if (isLocal == true)
                {
                    if (selectedOrderMode == NCasDefineOrderMode.TestMode)
                    {
                        if (lastOrderKind == NCasDefineOrderKind.AlarmStandby)
                        {
                            NCasProtocolBase protocolBase = NCasProtocolFactory.CreateCasProtocolForTc148(NCasDefineOperPanelSubCode.LedOutputSet);
                            NCasProtocolTc148Sub17 protocol148Sub17 = protocolBase as NCasProtocolTc148Sub17;
                            protocol148Sub17.LedGreen = NCasDefineOnOffBlinkStatus.On;
                            protocol148Sub17.LedRed = NCasDefineOnOffBlinkStatus.Off;
                            protocol148Sub17.LedYellow = NCasDefineOnOffBlinkStatus.Off;
                            protocol148Sub17.LedBuzz = NCasDefineOnOffBlinkStatus.Off;
                            protocol148Sub17.Led2Green = NCasDefineOnOffBlinkStatus.On;
                            protocol148Sub17.Led2Red = NCasDefineOnOffBlinkStatus.Off;
                            protocol148Sub17.Led2Yellow = NCasDefineOnOffBlinkStatus.Off;
                            protocol148Sub17.Led2Buzz = NCasDefineOnOffBlinkStatus.Off;

                            NCasProtocolFactory.MakeUdpFrame(protocol148Sub17);
                            this.main.SetPlcKeyData(protocol148Sub17);
                        }
                    }
                    else if (selectedOrderMode == NCasDefineOrderMode.RealMode)
                    {
                        if (lastOrderKind == NCasDefineOrderKind.AlarmStandby)
                        {
                            NCasProtocolBase protocolBase = NCasProtocolFactory.CreateCasProtocolForTc148(NCasDefineOperPanelSubCode.LedOutputSet);
                            NCasProtocolTc148Sub17 protocol148Sub17 = protocolBase as NCasProtocolTc148Sub17;
                            protocol148Sub17.LedGreen = NCasDefineOnOffBlinkStatus.Off;
                            protocol148Sub17.LedRed = NCasDefineOnOffBlinkStatus.On;
                            protocol148Sub17.LedYellow = NCasDefineOnOffBlinkStatus.Off;
                            protocol148Sub17.LedBuzz = NCasDefineOnOffBlinkStatus.On;
                            protocol148Sub17.Led2Green = NCasDefineOnOffBlinkStatus.Off;
                            protocol148Sub17.Led2Red = NCasDefineOnOffBlinkStatus.On;
                            protocol148Sub17.Led2Yellow = NCasDefineOnOffBlinkStatus.Off;
                            protocol148Sub17.Led2Buzz = NCasDefineOnOffBlinkStatus.On;

                            NCasProtocolFactory.MakeUdpFrame(protocol148Sub17);
                            this.main.SetPlcKeyData(protocol148Sub17);
                        }
                    }

                    if (lastOrderKind == NCasDefineOrderKind.AlarmCancel || lastOrderKind == NCasDefineOrderKind.DisasterBroadcast)
                    {
                        NCasProtocolBase protocolBase = NCasProtocolFactory.CreateCasProtocolForTc148(NCasDefineOperPanelSubCode.LedOutputSet);
                        NCasProtocolTc148Sub17 protocol148Sub17 = protocolBase as NCasProtocolTc148Sub17;
                        protocol148Sub17.LedGreen = NCasDefineOnOffBlinkStatus.Off;
                        protocol148Sub17.LedRed = NCasDefineOnOffBlinkStatus.Off;
                        protocol148Sub17.LedYellow = NCasDefineOnOffBlinkStatus.Off;
                        protocol148Sub17.LedBuzz = NCasDefineOnOffBlinkStatus.Off;
                        protocol148Sub17.Led2Green = NCasDefineOnOffBlinkStatus.Off;
                        protocol148Sub17.Led2Red = NCasDefineOnOffBlinkStatus.Off;
                        protocol148Sub17.Led2Yellow = NCasDefineOnOffBlinkStatus.Off;
                        protocol148Sub17.Led2Buzz = NCasDefineOnOffBlinkStatus.Off;

                        NCasProtocolFactory.MakeUdpFrame(protocol148Sub17);
                        this.main.SetPlcKeyData(protocol148Sub17);
                    }

                    System.Threading.Thread.Sleep(100);
                }
            }

            #region 상태 표출 테스트
            //Console.WriteLine("\n선택한 버튼리스트에 있는 버튼 수 - " + this.lstSelectedButtons.Count.ToString());

            //Console.WriteLine("선택한 발령 모드 - " + ((this.selectedOrderMode == NCasDefineOrderMode.None) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderMode2String(NCasDefineOrderMode.None) :
            //    (this.selectedOrderMode == NCasDefineOrderMode.RealMode) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderMode2String(NCasDefineOrderMode.RealMode) :
            //    (this.selectedOrderMode == NCasDefineOrderMode.TestMode) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderMode2String(NCasDefineOrderMode.TestMode) : "실패"));

            //Console.WriteLine("선택한 미디어 모드 - " + ((this.selectedOrderMedia == NCasDefineOrderMedia.None) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderMedia2String(NCasDefineOrderMedia.None) :
            //    (this.selectedOrderMedia == NCasDefineOrderMedia.MediaAll) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderMedia2String(NCasDefineOrderMedia.MediaAll) :
            //    (this.selectedOrderMedia == NCasDefineOrderMedia.MediaLine) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderMedia2String(NCasDefineOrderMedia.MediaLine) :
            //    (this.selectedOrderMedia == NCasDefineOrderMedia.MediaSate) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderMedia2String(NCasDefineOrderMedia.MediaSate) : "실패"));

            //Console.WriteLine("선택한 시도 발령대상 수 - " + lstSelectedOrderIP.Count.ToString());
            //foreach (string str in this.lstSelectedOrderIP)
            //{
            //    Console.WriteLine("      대상은 - " + str);
            //}

            //Console.WriteLine("선택한 발령종류 - " + ((this.selectedOrderKind == NCasDefineOrderKind.AlarmStandby) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmStandby) :
            //        (this.selectedOrderKind == NCasDefineOrderKind.AlarmWatch) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmWatch) :
            //        (this.selectedOrderKind == NCasDefineOrderKind.AlarmAttack) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmAttack) :
            //        (this.selectedOrderKind == NCasDefineOrderKind.AlarmBiochemist) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmBiochemist) :
            //        (this.selectedOrderKind == NCasDefineOrderKind.AlarmCancel) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmCancel) :
            //        (this.selectedOrderKind == NCasDefineOrderKind.AlarmClose) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmClose) :
            //        (this.selectedOrderKind == NCasDefineOrderKind.DisasterWatch) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.DisasterWatch) :
            //        (this.selectedOrderKind == NCasDefineOrderKind.VoiceLineTest) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.VoiceLineTest) :
            //        (this.selectedOrderKind == NCasDefineOrderKind.BroadPublicVoice) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.BroadPublicVoice) :
            //        (this.selectedOrderKind == NCasDefineOrderKind.BroadMessage) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.BroadMessage) :
            //        (this.selectedOrderKind == NCasDefineOrderKind.TermTts) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.TermTts) :
            //        (this.selectedOrderKind == NCasDefineOrderKind.CenterTts) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.CenterTts) :
            //        (this.selectedOrderKind == NCasDefineOrderKind.AlarmRecover) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmRecover) :
            //        (this.selectedOrderKind == NCasDefineOrderKind.DisasterBoradRecover) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.DisasterBoradRecover) :
            //        (this.selectedOrderKind == NCasDefineOrderKind.DisasterStandby) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.DisasterStandby) :
            //        (this.selectedOrderKind == NCasDefineOrderKind.DisasterBroadcast) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.DisasterBroadcast) : ""));

            //Console.WriteLine("가장 마지막 발령종류 - " + ((this.lastOrderKind == NCasDefineOrderKind.AlarmStandby) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmStandby) :
            //        (this.lastOrderKind == NCasDefineOrderKind.AlarmWatch) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmWatch) :
            //        (this.lastOrderKind == NCasDefineOrderKind.AlarmAttack) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmAttack) :
            //        (this.lastOrderKind == NCasDefineOrderKind.AlarmBiochemist) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmBiochemist) :
            //        (this.lastOrderKind == NCasDefineOrderKind.AlarmCancel) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmCancel) :
            //        (this.lastOrderKind == NCasDefineOrderKind.AlarmClose) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmClose) :
            //        (this.lastOrderKind == NCasDefineOrderKind.DisasterWatch) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.DisasterWatch) :
            //        (this.lastOrderKind == NCasDefineOrderKind.VoiceLineTest) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.VoiceLineTest) :
            //        (this.lastOrderKind == NCasDefineOrderKind.BroadPublicVoice) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.BroadPublicVoice) :
            //        (this.lastOrderKind == NCasDefineOrderKind.BroadMessage) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.BroadMessage) :
            //        (this.lastOrderKind == NCasDefineOrderKind.TermTts) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.TermTts) :
            //        (this.lastOrderKind == NCasDefineOrderKind.CenterTts) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.CenterTts) :
            //        (this.lastOrderKind == NCasDefineOrderKind.AlarmRecover) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmRecover) :
            //        (this.lastOrderKind == NCasDefineOrderKind.DisasterBoradRecover) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.DisasterBoradRecover) :
            //        (this.lastOrderKind == NCasDefineOrderKind.DisasterStandby) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.DisasterStandby) :
            //        (this.lastOrderKind == NCasDefineOrderKind.DisasterBroadcast) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.DisasterBroadcast) : ""));

            //Console.WriteLine("시도 전체 선택 - " + ((this.allDestinationFlag == true) ? "O" : "X"));

            //Console.WriteLine("단말 그룹 발령 선택 - " + ((this.orderTermGroupFlag == true) ? "O" : "X"));

            //Console.WriteLine("시군 그룹 발령 선택 - " + ((this.orderDistGroupFlag == true) ? "O" : "X"));

            //Console.WriteLine("시군 발령 선택 - " + ((this.orderDistFlag == true) ? "O" : "X"));

            //Console.WriteLine("시군아이콘으로 개별 발령 선택 - " + ((this.orderDistTermFlag == true) ? "O" : "X"));

            //Console.WriteLine("시군아이콘으로 시군전체 발령 선택 - " + ((this.orderDistTermAllFlag == true) ? "O" : "X"));

            //Console.WriteLine("개별단말 발령 선택 - " + ((this.orderTermFlag == true) ? "O" : "X"));

            //Console.WriteLine("개별단말로 시도전체 발령 선택 - " + ((this.orderTermAllFlag == true) ? "O" : "X"));

            //Console.WriteLine("예비 발령 - " + ((this.orderStandbyFlag == true) ? "O" : "X"));

            //Console.WriteLine("재난경계 종류 선택한 거 - " + ((this.selectedDisasterBroadKind == DisasterBroadKind.None) ? "선택안함" :
            //    (this.selectedDisasterBroadKind == DisasterBroadKind.Mic) ? "마이크" :
            //    (this.selectedDisasterBroadKind == DisasterBroadKind.Tts) ? "TTS" :
            //    (this.selectedDisasterBroadKind == DisasterBroadKind.StroredMessage) ? "저장메시지" : "선택안함"));

            //Console.WriteLine("마지막 재난경계 발령한 거 - " + ((this.lastDisasterBroadKind == DisasterBroadKind.None) ? "선택안함" :
            //    (this.lastDisasterBroadKind == DisasterBroadKind.Mic) ? "마이크" :
            //    (this.lastDisasterBroadKind == DisasterBroadKind.Tts) ? "TTS" :
            //    (this.lastDisasterBroadKind == DisasterBroadKind.StroredMessage) ? "저장메시지" : "선택안함"));

            //Console.WriteLine("선택한 그룹 버튼 수 - " + this.selectedGroupContents.Count.ToString());
            //foreach (GroupContent groupContent in this.selectedGroupContents)
            //{
            //    Console.WriteLine("      버튼은 - " + groupContent.Title);
            //}

            //Console.WriteLine("저장메시지 - ");
            //Console.WriteLine("      제목 - " + this.selectedStoredMessage.Title);
            //Console.WriteLine("      번호 - " + this.selectedStoredMessage.MsgNum);
            //Console.WriteLine("      시간 - " + this.selectedStoredMessage.PlayTime.ToString());
            //Console.WriteLine("      반복횟수 - " + this.storedMessageRepeatCount.ToString());

            //Console.WriteLine("TTS메시지 - ");
            //Console.WriteLine("      제목 - " + this.selectedTtsMessage.Title);
            //Console.WriteLine("      내용 - " + this.selectedTtsMessage.Text);

            //Console.WriteLine("그룹 정보 - " + this.groupNameLst.Count.ToString());
            #endregion
        }
        #endregion

        #region 실제 버튼
        /// <summary>
        /// 실제 버튼 동작
        /// </summary>
        /// <param name="selectBtn"></param>
        private void CheckKeyReal(NCasButton selectBtn)
        {
            if (selectBtn.CheckedValue == true)
            {
                using (OrderConfirmForm confirm = new OrderConfirmForm())
                {
                    confirm.PasswordConfirmEvent += new EventHandler(confirm_PasswordConfirmEvent);
                    confirm.ShowDialog();
                    confirm.PasswordConfirmEvent -= new EventHandler(confirm_PasswordConfirmEvent);
                }
            }
            else
            {
                if (MessageBox.Show("실제 발령을 해제하겠습니까?", "실제 발령 해제", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    if (this.termDestination != null)
                    {
                        this.termDestination.Close();
                        this.termDestination_TermDestinationCancelEvent(this, new EventArgs());
                        this.termDestination = null;
                    }

                    if (this.orderDistTermForm != null)
                    {
                        this.orderDistTermForm.Close();
                        this.orderDistTermForm.TermAllSelectEvent -= new EventHandler(orderDistTermForm_TermAllSelectEvent);
                        this.orderDistTermForm.TermAllCancelEvent -= new EventHandler(orderDistTermForm_TermAllCancelEvent);
                        this.orderDistTermForm.TermDestinationCancelEvent -= new EventHandler(orderDistTermForm_TermDestinationCancelEvent);
                        this.orderDistTermForm.AddTermDestinationEvent -= new EventHandler<TermSelectEventArgs>(orderDistTermForm_AddTermDestinationEvent);
                        this.orderDistTermForm.RemoveTermDestinationEvent -= new EventHandler<TermSelectEventArgs>(orderDistTermForm_RemoveTermDestinationEvent);
                        this.orderDistTermForm = null;
                    }

                    this.selectedOrderMode = NCasDefineOrderMode.None;
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                    return;
                }
            }

            if (this.selectedOrderMode == NCasDefineOrderMode.RealMode)
            {
                if (this.termDestination != null)
                {
                    this.termDestination.Close();
                    this.termDestination_TermDestinationCancelEvent(this, new EventArgs());
                    this.termDestination = null;
                }

                if (this.orderDistTermForm != null)
                {
                    this.orderDistTermForm.Close();
                    this.orderDistTermForm.TermAllSelectEvent -= new EventHandler(orderDistTermForm_TermAllSelectEvent);
                    this.orderDistTermForm.TermAllCancelEvent -= new EventHandler(orderDistTermForm_TermAllCancelEvent);
                    this.orderDistTermForm.TermDestinationCancelEvent -= new EventHandler(orderDistTermForm_TermDestinationCancelEvent);
                    this.orderDistTermForm.AddTermDestinationEvent -= new EventHandler<TermSelectEventArgs>(orderDistTermForm_AddTermDestinationEvent);
                    this.orderDistTermForm.RemoveTermDestinationEvent -= new EventHandler<TermSelectEventArgs>(orderDistTermForm_RemoveTermDestinationEvent);
                    this.orderDistTermForm = null;
                }

                foreach (NCasButton btn in this.lstSelectedButtons)
                {
                    this.SetBtnChecked(btn, false);
                }

                this.SetOrderDestinationButtonColorOff();
                this.lstSelectedOrderIP.Clear();
                this.selectedOrderKind = NCasDefineOrderKind.None;
                this.selectedDisasterBroadKind = DisasterBroadKind.None;
                this.lstSelectedButtons.Clear();
                this.allDestinationFlag = false;
                this.orderTermGroupFlag = false;
                this.orderDistGroupFlag = false;
                this.orderDistFlag = false;
                this.orderDistTermFlag = false;
                this.orderDistTermAllFlag = false;
                this.orderTermFlag = false;
                this.orderTermAllFlag = false;
                this.ShowDisasterBroadPanel(false);

                this.SetOrderKindButtonColorProvAll(this.selectedOrderKind);

                if (!this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Add(selectBtn);
                }

                this.SetOnButton(NCasKeyAction.Line);
                this.SetOnButton(NCasKeyAction.Sate);
                this.selectedOrderMedia = NCasDefineOrderMedia.MediaAll;
            }
            else if (this.selectedOrderMode == NCasDefineOrderMode.TestMode)
            {
                this.SetBtnChecked(selectBtn, false);
            }
            else if (this.selectedOrderMode == NCasDefineOrderMode.None)
            {
                foreach (NCasButton btn in this.lstSelectedButtons)
                {
                    this.SetBtnChecked(btn, false);
                }

                this.SetBtnChecked(selectBtn, false);
                this.SetOrderDestinationButtonColorOff();
                this.lstSelectedOrderIP.Clear();
                this.selectedOrderKind = NCasDefineOrderKind.None;
                this.selectedDisasterBroadKind = DisasterBroadKind.None;
                this.lstSelectedButtons.Clear();
                this.allDestinationFlag = false;
                this.orderTermGroupFlag = false;
                this.orderDistGroupFlag = false;
                this.orderDistFlag = false;
                this.orderDistTermFlag = false;
                this.orderDistTermAllFlag = false;
                this.orderTermFlag = false;
                this.orderTermAllFlag = false;
                this.SetOrderKindButtonColorProvAll(this.selectedOrderKind);
                this.selectedOrderMedia = NCasDefineOrderMedia.None;
                this.ShowDisasterBroadPanel(false);
            }
        }

        /// <summary>
        /// 실제발령 비밀번호 확인 컨펌 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void confirm_PasswordConfirmEvent(object sender, EventArgs e)
        {
            this.selectedOrderMode = NCasDefineOrderMode.RealMode;
        }

        /// <summary>
        /// 듀얼시스템에서 수신받은 실제 버튼 동작
        /// </summary>
        /// <param name="selectBtn"></param>
        private void CheckKeyRealFromDual(NCasButton selectBtn)
        {
            if (selectBtn.CheckedValue == true)
            {
                this.selectedOrderMode = NCasDefineOrderMode.RealMode;
            }
            else
            {
                if (this.selectedOrderMode == NCasDefineOrderMode.TestMode)
                {
                    this.selectedOrderMode = NCasDefineOrderMode.TestMode;
                }
                else
                {
                    if (this.termDestination != null)
                    {
                        this.termDestination.Close();
                        this.termDestination_TermDestinationCancelEvent(this, new EventArgs());
                        this.termDestination = null;
                    }

                    if (this.orderDistTermForm != null)
                    {
                        this.orderDistTermForm.Close();
                        this.orderDistTermForm.TermAllSelectEvent -= new EventHandler(orderDistTermForm_TermAllSelectEvent);
                        this.orderDistTermForm.TermAllCancelEvent -= new EventHandler(orderDistTermForm_TermAllCancelEvent);
                        this.orderDistTermForm.TermDestinationCancelEvent -= new EventHandler(orderDistTermForm_TermDestinationCancelEvent);
                        this.orderDistTermForm.AddTermDestinationEvent -= new EventHandler<TermSelectEventArgs>(orderDistTermForm_AddTermDestinationEvent);
                        this.orderDistTermForm.RemoveTermDestinationEvent -= new EventHandler<TermSelectEventArgs>(orderDistTermForm_RemoveTermDestinationEvent);
                        this.orderDistTermForm = null;
                    }

                    this.selectedOrderMode = NCasDefineOrderMode.None;
                }
            }

            if (this.selectedOrderMode == NCasDefineOrderMode.RealMode)
            {
                foreach (NCasButton btn in this.lstSelectedButtons)
                {
                    this.SetBtnChecked(btn, false);
                }

                this.SetOrderDestinationButtonColorOff();
                this.lstSelectedOrderIP.Clear();
                this.selectedOrderKind = NCasDefineOrderKind.None;
                this.selectedDisasterBroadKind = DisasterBroadKind.None;
                this.lstSelectedButtons.Clear();
                this.allDestinationFlag = false;
                this.orderTermGroupFlag = false;
                this.orderDistGroupFlag = false;
                this.orderDistFlag = false;
                this.orderDistTermFlag = false;
                this.orderDistTermAllFlag = false;
                this.orderTermFlag = false;
                this.orderTermAllFlag = false;

                this.SetOrderKindButtonColorProvAll(this.selectedOrderKind);

                if (!this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Add(selectBtn);
                }

                this.SetBtnChecked(selectBtn, true);
                this.SetOnButton(NCasKeyAction.Line);
                this.SetOnButton(NCasKeyAction.Sate);
                this.selectedOrderMedia = NCasDefineOrderMedia.MediaAll;
            }
            else if (this.selectedOrderMode == NCasDefineOrderMode.TestMode)
            {
                this.SetBtnChecked(selectBtn, false);
            }
            else if (this.selectedOrderMode == NCasDefineOrderMode.None)
            {
                foreach (NCasButton btn in this.lstSelectedButtons)
                {
                    this.SetBtnChecked(btn, false);
                }

                this.SetBtnChecked(selectBtn, false);
                this.SetOrderDestinationButtonColorOff();
                this.lstSelectedOrderIP.Clear();
                this.selectedOrderKind = NCasDefineOrderKind.None;
                this.selectedDisasterBroadKind = DisasterBroadKind.None;
                this.lstSelectedButtons.Clear();
                this.allDestinationFlag = false;
                this.orderTermGroupFlag = false;
                this.orderDistGroupFlag = false;
                this.orderDistFlag = false;
                this.orderDistTermFlag = false;
                this.orderDistTermAllFlag = false;
                this.orderTermFlag = false;
                this.orderTermAllFlag = false;
                this.SetOrderKindButtonColorProvAll(this.selectedOrderKind);
                this.selectedOrderMedia = NCasDefineOrderMedia.None;
            }
        }
        #endregion

        #region 시험 버튼
        /// <summary>
        /// 시험 버튼 동작
        /// </summary>
        /// <param name="keyData"></param>
        private void CheckKeyTest(NCasButton selectBtn)
        {
            if (selectBtn.CheckedValue == true)
            {
                if (this.selectedOrderMode == NCasDefineOrderMode.RealMode)
                {
                    this.SetBtnChecked(selectBtn, false);
                    this.ShowWrongOperationForm(NotifyRealSelected);
                }
                else
                {
                    this.selectedOrderMode = NCasDefineOrderMode.TestMode;

                    if (!this.lstSelectedButtons.Contains(selectBtn))
                    {
                        this.lstSelectedButtons.Add(selectBtn);
                    }

                    this.SetOnButton(NCasKeyAction.Line);
                    this.SetOnButton(NCasKeyAction.Sate);
                    this.selectedOrderMedia = NCasDefineOrderMedia.MediaAll;
                }
            }
            else
            {
                if (this.termDestination != null)
                {
                    this.termDestination.Close();
                    this.termDestination_TermDestinationCancelEvent(this, new EventArgs());
                    this.termDestination = null;
                }

                if (this.orderDistTermForm != null)
                {
                    this.orderDistTermForm.Close();
                    this.orderDistTermForm.TermAllSelectEvent -= new EventHandler(orderDistTermForm_TermAllSelectEvent);
                    this.orderDistTermForm.TermAllCancelEvent -= new EventHandler(orderDistTermForm_TermAllCancelEvent);
                    this.orderDistTermForm.TermDestinationCancelEvent -= new EventHandler(orderDistTermForm_TermDestinationCancelEvent);
                    this.orderDistTermForm.AddTermDestinationEvent -= new EventHandler<TermSelectEventArgs>(orderDistTermForm_AddTermDestinationEvent);
                    this.orderDistTermForm.RemoveTermDestinationEvent -= new EventHandler<TermSelectEventArgs>(orderDistTermForm_RemoveTermDestinationEvent);
                    this.orderDistTermForm = null;
                }

                this.selectedOrderMode = NCasDefineOrderMode.None;

                foreach (NCasButton btn in this.lstSelectedButtons)
                {
                    this.SetBtnChecked(btn, false);
                }

                this.SetBtnChecked(selectBtn, false);
                this.SetOrderDestinationButtonColorOff();
                this.lstSelectedOrderIP.Clear();
                this.selectedOrderKind = NCasDefineOrderKind.None;
                this.selectedDisasterBroadKind = DisasterBroadKind.None;
                this.lstSelectedButtons.Clear();
                this.allDestinationFlag = false;
                this.orderTermGroupFlag = false;
                this.orderDistGroupFlag = false;
                this.orderDistFlag = false;
                this.orderDistTermFlag = false;
                this.orderDistTermAllFlag = false;
                this.orderTermFlag = false;
                this.orderTermAllFlag = false;
                this.SetOrderKindButtonColorProvAll(this.selectedOrderKind);
                this.selectedOrderMedia = NCasDefineOrderMedia.None;
                this.ShowDisasterBroadPanel(false);
            }
        }
        #endregion

        #region 유선 버튼
        /// <summary>
        /// 유선 버튼 동작
        /// </summary>
        /// <param name="selectBtn"></param>
        private void CheckKeyLine(NCasButton selectBtn)
        {
            if (this.selectedOrderMode == NCasDefineOrderMode.None)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderMode);
                return;
            }

            if (selectBtn.CheckedValue == true)
            {
                if (this.selectedOrderMedia == NCasDefineOrderMedia.MediaSate)
                {
                    this.selectedOrderMedia = NCasDefineOrderMedia.MediaAll;
                }
                else
                {
                    this.selectedOrderMedia = NCasDefineOrderMedia.MediaLine;
                }

                if (!this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Add(selectBtn);
                }
            }
            else
            {
                if (this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Remove(selectBtn);
                }

                this.SetOnButton(NCasKeyAction.Sate);
                this.selectedOrderMedia = NCasDefineOrderMedia.MediaSate;
            }
        }
        #endregion

        #region 위성 버튼
        /// <summary>
        /// 위성 버튼 동작
        /// </summary>
        /// <param name="selectBtn"></param>
        private void CheckKeySate(NCasButton selectBtn)
        {
            if (this.selectedOrderMode == NCasDefineOrderMode.None)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderMode);
                return;
            }

            if (selectBtn.CheckedValue == true)
            {
                if (this.selectedOrderMedia == NCasDefineOrderMedia.MediaLine)
                {
                    this.selectedOrderMedia = NCasDefineOrderMedia.MediaAll;
                }
                else
                {
                    this.selectedOrderMedia = NCasDefineOrderMedia.MediaSate;
                }

                if (!this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Add(selectBtn);
                }
            }
            else
            {
                if (this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Remove(selectBtn);
                }

                this.SetOnButton(NCasKeyAction.Line);
                this.selectedOrderMedia = NCasDefineOrderMedia.MediaLine;
            }
        }
        #endregion

        #region 시도 전체(발령대상) 버튼
        /// <summary>
        /// 시도 전체(발령대상) 버튼 동작
        /// </summary>
        /// <param name="selectBtn"></param>
        private void CheckKeyProveAllDestination(NCasButton selectBtn)
        {
            if (this.selectedOrderMode == NCasDefineOrderMode.None || this.selectedOrderMedia == NCasDefineOrderMedia.None)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderMode);
                return;
            }

            if (selectBtn.CheckedValue == true)
            {
                this.SetOrderDestinationButtonColorOff();

                if (this.termDestination != null)
                {
                    this.termDestination.Close();
                    this.termDestination_TermDestinationCancelEvent(this, new EventArgs());
                    this.termDestination = null;
                }

                if (this.orderDistTermForm != null)
                {
                    this.orderDistTermForm.Close();
                    this.orderDistTermForm.TermAllSelectEvent -= new EventHandler(orderDistTermForm_TermAllSelectEvent);
                    this.orderDistTermForm.TermAllCancelEvent -= new EventHandler(orderDistTermForm_TermAllCancelEvent);
                    this.orderDistTermForm.TermDestinationCancelEvent -= new EventHandler(orderDistTermForm_TermDestinationCancelEvent);
                    this.orderDistTermForm.AddTermDestinationEvent -= new EventHandler<TermSelectEventArgs>(orderDistTermForm_AddTermDestinationEvent);
                    this.orderDistTermForm.RemoveTermDestinationEvent -= new EventHandler<TermSelectEventArgs>(orderDistTermForm_RemoveTermDestinationEvent);
                    this.orderDistTermForm = null;
                }

                this.allDestinationFlag = true;
                this.orderTermGroupFlag = false;
                this.orderDistGroupFlag = false;
                this.orderDistFlag = false;
                this.orderDistTermFlag = false;
                this.orderDistTermAllFlag = false;
                this.orderTermFlag = false;
                this.orderTermAllFlag = false;

                if (!this.lstSelectedOrderIP.Contains(NCasUtilityMng.INCasCommUtility.MakeNetIdToProvBroadIp(NCasUtilityMng.INCasEtcUtility.GetIPv4())))
                {
                    this.lstSelectedOrderIP.Clear();
                    this.lstSelectedOrderIP.Add(NCasUtilityMng.INCasCommUtility.MakeNetIdToProvBroadIp(NCasUtilityMng.INCasEtcUtility.GetIPv4()));
                }

                if (!this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Add(selectBtn);
                }

                this.SetOnButton(NCasKeyAction.ProveAllDestination);
                this.SetOnButton(NCasKeyAction.TermDestination);
                this.SetOnButton(NCasKeyAction.TermGroupDestination);
                this.SetOnButton(NCasKeyAction.DistGroupDestination);
                this.SetOnButton(NCasKeyAction.DistOneDestination);
                this.ClearOrderKind();
                this.ShowDisasterBroadPanel(false);
            }
            else
            {
                this.SetOrderDestinationButtonColorOff();
                this.allDestinationFlag = false;
                this.lstSelectedOrderIP.Clear();

                if (this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Remove(selectBtn);
                }

                this.SetOffButton(NCasKeyAction.ProveAllDestination);
                this.SetOffButton(NCasKeyAction.TermDestination);
                this.SetOffButton(NCasKeyAction.TermGroupDestination);
                this.SetOffButton(NCasKeyAction.DistGroupDestination);
                this.SetOffButton(NCasKeyAction.DistOneDestination);
                this.ClearOrderKind();
                this.ShowDisasterBroadPanel(false);
            }
        }
        #endregion

        #region 단말 개별(발령대상) 버튼
        /// <summary>
        /// 단말 개별(발령대상) 버튼 동작
        /// </summary>
        /// <param name="selectBtn"></param>
        private void CheckKeyTermDestination(NCasButton selectBtn)
        {
            if (this.selectedOrderMode == NCasDefineOrderMode.None || this.selectedOrderMedia == NCasDefineOrderMedia.None)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderMode);
                return;
            }

            if (this.allDestinationFlag == true)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.SetOrderKindButtonColorOn(this.selectedOrderKind);
                this.ShowWrongOperationForm(NotifyAllDestinationSelected);
                return;
            }

            if (this.orderTermGroupFlag || this.orderDistGroupFlag)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.SetOrderKindButtonColorOn(this.selectedOrderKind);
                this.ShowWrongOperationForm(NotifyGroupDestinationSelected);
                return;
            }

            if (this.orderDistFlag)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(NotifyDistIconDestinationSelected);
                return;
            }

            if (this.orderDistTermFlag || this.orderDistTermAllFlag)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(NotifyDistIconTermDestinationSelected);
                return;
            }

            if (this.orderTermFlag)
            {
                if (this.selectedOrderKind != NCasDefineOrderKind.None)
                {
                    this.SetOrderKindButtonColorOn(this.selectedOrderKind);
                    return;
                }
            }

            if (selectBtn.CheckedValue == true)
            {
                if (!this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Add(selectBtn);
                }

                Screen scr = Screen.PrimaryScreen;
                Rectangle rec = scr.Bounds;

                this.termDestination = new TermDestination(this.main.ProvInfo);
                this.termDestination.AddTermDestinationEvent += new EventHandler<TermSelectEventArgs>(termDestination_AddTermDestinationEvent);
                this.termDestination.RemoveTermDestinationEvent += new EventHandler<TermSelectEventArgs>(termDestination_RemoveTermDestinationEvent);
                this.termDestination.TermAllCancelEvent += new EventHandler(termDestination_TermAllCancelEvent);
                this.termDestination.TermAllSelectEvent += new EventHandler(termDestination_TermAllSelectEvent);
                this.termDestination.TermDestinationCancelEvent += new EventHandler(termDestination_TermDestinationCancelEvent);
                this.orderTermFlag = true;
                this.termDestination.Show(this);
                this.termDestination.Location = new Point(701, (rec.Height / 2) - 300);
                this.ShowDisasterBroadPanel(false);
            }
            else
            {
                if (this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Remove(selectBtn);
                }

                this.orderTermFlag = false;
                this.orderTermAllFlag = false;
                this.lstSelectedOrderIP.Clear();

                if (this.termDestination != null)
                {
                    this.termDestination.Close();
                }

                this.termDestination = null;
                this.ShowDisasterBroadPanel(false);
            }
        }
        #endregion

        #region 단말 그룹(발령대상) 버튼
        /// <summary>
        /// 단말 그룹(발령대상) 버튼 동작
        /// </summary>
        /// <param name="selectBtn"></param>
        private void CheckKeyTermGroupDestination(NCasButton selectBtn)
        {
            if (this.selectedOrderMode == NCasDefineOrderMode.None || this.selectedOrderMedia == NCasDefineOrderMedia.None)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderMode);
                return;
            }

            if (this.allDestinationFlag == true)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.SetOrderKindButtonColorOn(this.selectedOrderKind);
                this.ShowWrongOperationForm(NotifyAllDestinationSelected);
                return;
            }

            if (this.orderTermFlag || this.orderTermAllFlag)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(NotifyTermDestinationSelected);
                return;
            }

            if (this.orderDistFlag)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(NotifyDistIconDestinationSelected);
                return;
            }

            if (this.orderDistTermFlag || this.orderDistTermAllFlag)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(NotifyDistIconTermDestinationSelected);
                return;
            }

            if (this.orderDistGroupFlag == true)
            {
                this.SetOrderDestinationButtonColorOff();
                this.SetOffButtonAny(NCasKeyAction.DistGroupDestination);
                this.lstSelectedOrderIP.Clear();
                this.orderDistGroupFlag = false;
                this.ClearOrderKind();
                this.groupNameLst.Clear();
            }

            if (this.orderTermGroupFlag)
            {
                if (this.selectedOrderKind != NCasDefineOrderKind.None)
                {
                    this.ClearOrderKind();
                    this.SetOffButtonAny(NCasKeyAction.TermGroupDestination);
                    this.lstSelectedOrderIP.Clear();
                    this.orderTermGroupFlag = false;

                    if (selectBtn.CheckedValue == true)
                    {
                        this.SetBtnChecked(selectBtn, false);
                    }
                    else
                    {
                        this.SetBtnChecked(selectBtn, true);
                    }
                }
            }

            if (selectBtn.CheckedValue == true)
            {
                if (!this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Add(selectBtn);
                }

                if (!this.groupNameLst.Contains(selectBtn.Text))
                {
                    this.groupNameLst.Add(selectBtn.Text);
                }

                this.orderTermGroupFlag = true;
                this.GroupBtnDataAdd(selectBtn);
                this.ShowDisasterBroadPanel(false);
            }
            else
            {
                if (this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Remove(selectBtn);
                }

                this.GroupBtnDataRemove(selectBtn);

                if (this.groupNameLst.Contains(selectBtn.Text))
                {
                    this.groupNameLst.Remove(selectBtn.Text);
                }

                if (this.lstSelectedOrderIP.Count == 0)
                {
                    this.orderTermGroupFlag = false;
                }

                foreach (NCasButton ncasBtn in this.lstSelectedButtons)
                {
                    if (((NCasKeyData)ncasBtn.Tag).KeyActioin == NCasKeyAction.TermGroupDestination)
                    {
                        this.GroupBtnDataAdd(ncasBtn);
                        this.orderTermGroupFlag = true;
                    }
                }

                this.ShowDisasterBroadPanel(false);
            }
        }
        #endregion

        #region 시군 그룹(발령대상) 버튼
        /// <summary>
        /// 시군 그룹(발령대상) 버튼 동작
        /// </summary>
        /// <param name="selectBtn"></param>
        private void CheckKeyDistGroupDestination(NCasButton selectBtn)
        {
            if (this.selectedOrderMode == NCasDefineOrderMode.None || this.selectedOrderMedia == NCasDefineOrderMedia.None)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderMode);
                return;
            }

            if (this.allDestinationFlag == true)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.SetOrderKindButtonColorOn(this.selectedOrderKind);
                this.ShowWrongOperationForm(NotifyAllDestinationSelected);
                return;
            }

            if (this.orderTermFlag || this.orderTermAllFlag)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(NotifyTermDestinationSelected);
                return;
            }

            if (this.orderDistFlag)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(NotifyDistIconDestinationSelected);
                return;
            }

            if (this.orderDistTermFlag || this.orderDistTermAllFlag)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(NotifyDistIconTermDestinationSelected);
                return;
            }

            if (this.orderTermGroupFlag == true)
            {
                this.SetOrderDestinationButtonColorOff();
                this.SetOffButtonAny(NCasKeyAction.TermGroupDestination);
                this.lstSelectedOrderIP.Clear();
                this.orderTermGroupFlag = false;
                this.ClearOrderKind();
                this.groupNameLst.Clear();
            }

            if (this.orderDistGroupFlag)
            {
                if (this.selectedOrderKind != NCasDefineOrderKind.None)
                {
                    this.ClearOrderKind();
                    this.SetOffButtonAny(NCasKeyAction.DistGroupDestination);
                    this.lstSelectedOrderIP.Clear();
                    this.orderDistGroupFlag = false;

                    if (selectBtn.CheckedValue == true)
                    {
                        this.SetBtnChecked(selectBtn, false);
                    }
                    else
                    {
                        this.SetBtnChecked(selectBtn, true);
                    }
                }
            }

            if (selectBtn.CheckedValue == true)
            {
                if (!this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Add(selectBtn);
                }

                this.orderDistGroupFlag = true;
                this.GroupBtnDataAdd(selectBtn);
                this.ShowDisasterBroadPanel(false);

                if (!this.groupNameLst.Contains(selectBtn.Text))
                {
                    this.groupNameLst.Add(selectBtn.Text);
                }
            }
            else
            {
                if (this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Remove(selectBtn);
                }

                this.GroupBtnDataRemove(selectBtn);

                if (this.groupNameLst.Contains(selectBtn.Text))
                {
                    this.groupNameLst.Remove(selectBtn.Text);
                }

                if (this.lstSelectedOrderIP.Count == 0)
                {
                    this.orderDistGroupFlag = false;
                }

                foreach (NCasButton ncasBtn in this.lstSelectedButtons)
                {
                    if (((NCasKeyData)ncasBtn.Tag).KeyActioin == NCasKeyAction.DistGroupDestination)
                    {
                        this.GroupBtnDataAdd(ncasBtn);
                        this.orderDistGroupFlag = true;
                    }
                }

                this.ShowDisasterBroadPanel(false);
            }
        }
        #endregion

        #region 시군 아이콘 (발령 대상) 버튼
        /// <summary>
        /// 시군 아이콘 (발령 대상) 버튼 마우스왼쪽 동작
        /// </summary>
        /// <param name="selectBtn"></param>
        private void CheckKeyDistOneDestination(NCasButton selectBtn)
        {
            if (this.selectedOrderMode == NCasDefineOrderMode.None || this.selectedOrderMedia == NCasDefineOrderMedia.None)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderMode);
                return;
            }

            if (this.allDestinationFlag == true)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.SetOrderKindButtonColorOn(this.selectedOrderKind);
                this.ShowWrongOperationForm(NotifyAllDestinationSelected);
                return;
            }

            if (this.orderTermFlag || this.orderTermAllFlag)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(NotifyTermDestinationSelected);
                return;
            }

            if (this.orderTermGroupFlag || this.orderDistGroupFlag)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(NotifyGroupDestinationSelected);
                return;
            }

            if (this.orderDistTermFlag || this.orderDistTermAllFlag)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(NotifyDistIconTermDestinationSelected);
                return;
            }

            if (this.orderDistFlag)
            {
                if (this.selectedOrderKind != NCasDefineOrderKind.None)
                {
                    this.ClearOrderKind();
                    this.SetOffButtonAny(NCasKeyAction.DistOneDestination);
                    this.lstSelectedOrderIP.Clear();
                    this.orderDistFlag = false;

                    if (selectBtn.CheckedValue == true)
                    {
                        this.SetBtnChecked(selectBtn, false);
                    }
                    else
                    {
                        this.SetBtnChecked(selectBtn, true);
                    }
                }
            }

            if (selectBtn.CheckedValue)
            {
                this.SetOrderDestinationButtonColorOff();

                if (!this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Add(selectBtn);
                }

                this.orderDistFlag = true;
                DistInfo distInfo = this.main.MmfMng.GetDistInfoByCode(int.Parse((selectBtn.Tag as NCasKeyData).Info));
                this.lstSelectedOrderIP.Add(NCasUtilityMng.INCasCommUtility.AddIpAddr(distInfo.NetIdToString, 0, 0, 0, 255));
                this.ClearOrderKind();
                this.SetBtnChecked(selectBtn, true);
                this.ShowDisasterBroadPanel(false);
            }
            else
            {
                if (this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Remove(selectBtn);
                }

                DistInfo distInfo = this.main.MmfMng.GetDistInfoByCode(int.Parse((selectBtn.Tag as NCasKeyData).Info));
                this.lstSelectedOrderIP.Remove(NCasUtilityMng.INCasCommUtility.AddIpAddr(distInfo.NetIdToString, 0, 0, 0, 255));

                if (this.lstSelectedOrderIP.Count == 0)
                {
                    this.orderDistFlag = false;
                }

                this.ShowDisasterBroadPanel(false);
            }
        }

        /// <summary>
        /// 시군 아이콘 (발령 대상) 버튼 마우스오른쪽 동작
        /// </summary>
        /// <param name="selectBtn"></param>
        private void CheckKeyDistOneDestinationRight(NCasButton selectBtn)
        {
            if (this.selectedOrderMode == NCasDefineOrderMode.None || this.selectedOrderMedia == NCasDefineOrderMedia.None)
            {
                this.ShowWrongOperationForm(this.NotifyOrderMode);
                return;
            }

            if (this.allDestinationFlag == true)
            {
                this.SetOrderKindButtonColorOn(this.selectedOrderKind);
                this.ShowWrongOperationForm(NotifyAllDestinationSelected);
                return;
            }

            if (this.orderTermFlag || this.orderTermAllFlag)
            {
                this.ShowWrongOperationForm(NotifyTermDestinationSelected);
                return;
            }

            if (this.orderTermGroupFlag || this.orderDistGroupFlag)
            {
                this.ShowWrongOperationForm(NotifyGroupDestinationSelected);
                return;
            }

            if (this.orderDistFlag)
            {
                this.ShowWrongOperationForm(NotifyDistIconDestinationSelected);
                return;
            }

            if (this.orderDistTermFlag || this.orderDistTermAllFlag)
            {
                return;
            }

            this.orderDistTermFlag = true;
            this.selectedDistCode = int.Parse((selectBtn.Tag as NCasKeyData).Info);
            this.orderDistTermForm = new OrderDistTermForm(this.main.MmfMng.GetDistInfoByCode(this.selectedDistCode));
            this.orderDistTermForm.TermAllSelectEvent += new EventHandler(orderDistTermForm_TermAllSelectEvent);
            this.orderDistTermForm.TermAllCancelEvent += new EventHandler(orderDistTermForm_TermAllCancelEvent);
            this.orderDistTermForm.TermDestinationCancelEvent += new EventHandler(orderDistTermForm_TermDestinationCancelEvent);
            this.orderDistTermForm.AddTermDestinationEvent += new EventHandler<TermSelectEventArgs>(orderDistTermForm_AddTermDestinationEvent);
            this.orderDistTermForm.RemoveTermDestinationEvent += new EventHandler<TermSelectEventArgs>(orderDistTermForm_RemoveTermDestinationEvent);
            this.orderDistTermForm.Show(this);
            this.orderDistTermForm.SetDesktopLocation(465, 390);
        }
        #endregion

        #region 예비 버튼
        /// <summary>
        /// 예비 버튼 동작
        /// </summary>
        /// <param name="?"></param>
        private void CheckKeyReady(NCasButton selectBtn)
        {
            if (this.selectedOrderMode == NCasDefineOrderMode.None || this.selectedOrderMedia == NCasDefineOrderMedia.None)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderMode);
                return;
            }

            if (selectBtn.CheckedValue == false)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }
                return;
            }

            if (this.lstSelectedOrderIP.Count == 0)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderdestination);
                return;
            }

            if (selectBtn.CheckedValue == true)
            {
                this.selectedOrderKind = NCasDefineOrderKind.AlarmStandby;

                if (!this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Add(selectBtn);
                }

                this.SetOrderKindButtonColorOn(NCasDefineOrderKind.AlarmStandby);
                this.selectedDisasterBroadKind = DisasterBroadKind.None;
                this.SetOffButton(NCasKeyAction.Attack);
                this.SetOffButton(NCasKeyAction.Watch);
                this.SetOffButton(NCasKeyAction.Biological);
                this.SetOffButton(NCasKeyAction.DisasterBroad);
                this.SetOffButton(NCasKeyAction.DisasterWatch);
                this.SetOffButton(NCasKeyAction.Clear);
                this.SetOffButton(NCasKeyAction.MicOrder);
                this.SetOffButton(NCasKeyAction.TtsOrder);
                this.SetOffButton(NCasKeyAction.MsgOrder);
                this.ShowDisasterBroadPanel(false);
            }
        }
        #endregion

        #region 경계 버튼
        /// <summary>
        /// 경계 버튼 동작
        /// </summary>
        /// <param name="?"></param>
        private void CheckKeyWatch(NCasButton selectBtn)
        {
            if (this.selectedOrderMode == NCasDefineOrderMode.None || this.selectedOrderMedia == NCasDefineOrderMedia.None)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderMode);
                return;
            }

            if (selectBtn.CheckedValue == false)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }
                return;
            }

            if (this.lstSelectedOrderIP.Count == 0)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderdestination);
                return;
            }

            if (this.orderStandbyFlag == false)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyReadyOrderMiss);
                return;
            }

            if (selectBtn.CheckedValue == true)
            {
                this.selectedOrderKind = NCasDefineOrderKind.AlarmWatch;

                if (!this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Add(selectBtn);
                }

                this.SetOrderKindButtonColorOn(NCasDefineOrderKind.AlarmWatch);
                this.selectedDisasterBroadKind = DisasterBroadKind.None;
                this.SetOffButton(NCasKeyAction.Ready);
                this.SetOffButton(NCasKeyAction.Attack);
                this.SetOffButton(NCasKeyAction.Biological);
                this.SetOffButton(NCasKeyAction.DisasterBroad);
                this.SetOffButton(NCasKeyAction.DisasterWatch);
                this.SetOffButton(NCasKeyAction.Clear);
                this.SetOffButton(NCasKeyAction.MicOrder);
                this.SetOffButton(NCasKeyAction.TtsOrder);
                this.SetOffButton(NCasKeyAction.MsgOrder);
                this.ShowDisasterBroadPanel(false);
            }
        }
        #endregion

        #region 공습 버튼
        /// <summary>
        /// 공습 버튼 동작
        /// </summary>
        /// <param name="?"></param>
        private void CheckKeyAttack(NCasButton selectBtn)
        {
            if (this.selectedOrderMode == NCasDefineOrderMode.None || this.selectedOrderMedia == NCasDefineOrderMedia.None)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderMode);
                return;
            }

            if (selectBtn.CheckedValue == false)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }
                return;
            }

            if (this.lstSelectedOrderIP.Count == 0)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderdestination);
                return;
            }

            if (this.orderStandbyFlag == false)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyReadyOrderMiss);
                return;
            }

            if (selectBtn.CheckedValue == true)
            {
                this.selectedOrderKind = NCasDefineOrderKind.AlarmAttack;

                if (!this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Add(selectBtn);
                }

                this.SetOrderKindButtonColorOn(NCasDefineOrderKind.AlarmAttack);
                this.selectedDisasterBroadKind = DisasterBroadKind.None;
                this.SetOffButton(NCasKeyAction.Ready);
                this.SetOffButton(NCasKeyAction.Watch);
                this.SetOffButton(NCasKeyAction.Biological);
                this.SetOffButton(NCasKeyAction.DisasterBroad);
                this.SetOffButton(NCasKeyAction.DisasterWatch);
                this.SetOffButton(NCasKeyAction.Clear);
                this.SetOffButton(NCasKeyAction.MicOrder);
                this.SetOffButton(NCasKeyAction.TtsOrder);
                this.SetOffButton(NCasKeyAction.MsgOrder);
                this.ShowDisasterBroadPanel(false);
            }
        }
        #endregion

        #region 화생방 버튼
        /// <summary>
        /// 화생방 버튼 동작
        /// </summary>
        /// <param name="?"></param>
        private void CheckKeyBiological(NCasButton selectBtn)
        {
            if (this.selectedOrderMode == NCasDefineOrderMode.None || this.selectedOrderMedia == NCasDefineOrderMedia.None)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderMode);
                return;
            }

            if (selectBtn.CheckedValue == false)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }
                return;
            }

            if (this.lstSelectedOrderIP.Count == 0)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderdestination);
                return;
            }

            if (this.orderStandbyFlag == false)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyReadyOrderMiss);
                return;
            }

            if (selectBtn.CheckedValue == true)
            {
                this.selectedOrderKind = NCasDefineOrderKind.AlarmBiochemist;

                if (!this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Add(selectBtn);
                }

                this.SetOrderKindButtonColorOn(NCasDefineOrderKind.AlarmBiochemist);
                this.selectedDisasterBroadKind = DisasterBroadKind.None;
                this.SetOffButton(NCasKeyAction.Ready);
                this.SetOffButton(NCasKeyAction.Attack);
                this.SetOffButton(NCasKeyAction.Watch);
                this.SetOffButton(NCasKeyAction.DisasterBroad);
                this.SetOffButton(NCasKeyAction.DisasterWatch);
                this.SetOffButton(NCasKeyAction.Clear);
                this.SetOffButton(NCasKeyAction.MicOrder);
                this.SetOffButton(NCasKeyAction.TtsOrder);
                this.SetOffButton(NCasKeyAction.MsgOrder);
                this.ShowDisasterBroadPanel(false);
            }
        }
        #endregion

        #region 재난위험 버튼
        /// <summary>
        /// 재난위험 버튼 동작
        /// </summary>
        /// <param name="?"></param>
        private void CheckKeyDisasterWatch(NCasButton selectBtn)
        {
            if (this.selectedOrderMode == NCasDefineOrderMode.None || this.selectedOrderMedia == NCasDefineOrderMedia.None)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderMode);
                return;
            }

            if (selectBtn.CheckedValue == false)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }
                return;
            }

            if (this.lstSelectedOrderIP.Count == 0)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderdestination);
                return;
            }

            if (this.orderStandbyFlag == false)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyReadyOrderMiss);
                return;
            }

            if (selectBtn.CheckedValue == true)
            {
                this.selectedOrderKind = NCasDefineOrderKind.DisasterWatch;

                if (!this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Add(selectBtn);
                }

                this.SetOrderKindButtonColorOn(NCasDefineOrderKind.DisasterWatch);
                this.selectedDisasterBroadKind = DisasterBroadKind.None;
                this.SetOffButton(NCasKeyAction.Ready);
                this.SetOffButton(NCasKeyAction.Attack);
                this.SetOffButton(NCasKeyAction.Watch);
                this.SetOffButton(NCasKeyAction.Biological);
                this.SetOffButton(NCasKeyAction.DisasterBroad);
                this.SetOffButton(NCasKeyAction.Clear);
                this.SetOffButton(NCasKeyAction.MicOrder);
                this.SetOffButton(NCasKeyAction.TtsOrder);
                this.SetOffButton(NCasKeyAction.MsgOrder);
                this.ShowDisasterBroadPanel(false);
            }
        }
        #endregion

        #region 재난경계(재해방송) 버튼
        /// <summary>
        /// 재난경계(재해방송) 버튼 동작
        /// </summary>
        /// <param name="?"></param>
        private void CheckKeyDisasterBroad(NCasButton selectBtn)
        {
            if (this.selectedOrderMode == NCasDefineOrderMode.None || this.selectedOrderMedia == NCasDefineOrderMedia.None)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderMode);
                return;
            }

            if (selectBtn.CheckedValue == false)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }
                return;
            }

            if (this.lstSelectedOrderIP.Count == 0)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderdestination);
                return;
            }

            if (this.orderStandbyFlag == false)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyReadyOrderMiss);
                return;
            }

            if (selectBtn.CheckedValue == true)
            {
                this.selectedOrderKind = NCasDefineOrderKind.DisasterBroadcast;

                if (!this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Add(selectBtn);
                }

                this.SetOrderKindButtonColorOn(NCasDefineOrderKind.DisasterBroadcast);
                this.selectedDisasterBroadKind = DisasterBroadKind.None;
                this.SetOffButton(NCasKeyAction.Ready);
                this.SetOffButton(NCasKeyAction.Attack);
                this.SetOffButton(NCasKeyAction.Watch);
                this.SetOffButton(NCasKeyAction.Biological);
                this.SetOffButton(NCasKeyAction.DisasterWatch);
                this.SetOffButton(NCasKeyAction.Clear);
                this.SetOnButton(NCasKeyAction.MicOrder);
                this.selectedDisasterBroadKind = DisasterBroadKind.Mic;
                this.ShowDisasterBroadPanel(true);
            }
        }
        #endregion

        #region 재난경계 MIC 버튼
        /// <summary>
        /// 재난경계 MIC 버튼 동작
        /// </summary>
        /// <param name="?"></param>
        private void CheckKeyMicOrder(NCasButton selectBtn)
        {
            if (this.selectedOrderMode == NCasDefineOrderMode.None || this.selectedOrderMedia == NCasDefineOrderMedia.None)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderMode);
                return;
            }

            if (selectBtn.CheckedValue == false)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }
                return;
            }

            if (this.lstSelectedOrderIP.Count == 0)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderdestination);
                return;
            }

            if (selectBtn.CheckedValue == true)
            {
                if (!this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Add(selectBtn);
                }

                this.selectedDisasterBroadKind = DisasterBroadKind.Mic;
                this.SetOffButton(NCasKeyAction.TtsOrder);
                this.SetOffButton(NCasKeyAction.MsgOrder);
            }
            else
            {
                this.SetBtnChecked(selectBtn, true);
            }
        }
        #endregion

        #region 재난경계 TTS 버튼
        /// <summary>
        /// 재난경계 TTS 버튼 동작
        /// </summary>
        /// <param name="?"></param>
        private void CheckKeyTtsOrder(NCasButton selectBtn)
        {
            if (this.selectedOrderMode == NCasDefineOrderMode.None || this.selectedOrderMedia == NCasDefineOrderMedia.None)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderMode);
                return;
            }

            if (selectBtn.CheckedValue == false)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }
                return;
            }

            if (this.lstSelectedOrderIP.Count == 0)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }   

                this.ShowWrongOperationForm(this.NotifyOrderdestination);
                return;
            }

            if (selectBtn.CheckedValue == true)
            {
                if (!this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Add(selectBtn);
                }

                this.selectedDisasterBroadKind = DisasterBroadKind.Tts;
                this.SetOffButton(NCasKeyAction.MicOrder);
                this.SetOffButton(NCasKeyAction.MsgOrder);
            }
            else
            {
                this.SetBtnChecked(selectBtn, true);
            }

            using (NCasTtsMsgForm form = new NCasTtsMsgForm())
            {
                form.SelectTtsMsgEvent += new EventHandler<SelectTtsMsgEventArgs>(form_SelectTtsMsgEvent);
                form.ShowDialog();
                form.SelectTtsMsgEvent -= new EventHandler<SelectTtsMsgEventArgs>(form_SelectTtsMsgEvent);
            }
        }

        /// <summary>
        /// TTS메시지 화면에서 발생하는 메시지 선택 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void form_SelectTtsMsgEvent(object sender, SelectTtsMsgEventArgs e)
        {
            if (e.SelectFlag) //TTS메시지를 선택했으면..
            {
                this.selectedTtsMessage = e.TtsMsg;
            }
            else
            {
                this.selectedDisasterBroadKind = DisasterBroadKind.Mic;
                this.SetOnButton(NCasKeyAction.MicOrder);
                this.SetOffButton(NCasKeyAction.TtsOrder);
                this.SetOffButton(NCasKeyAction.MsgOrder);
            }
        }
        #endregion

        #region 재난경계 저장메시지 버튼
        /// <summary>
        /// 재난경계 저장메시지 버튼 동작
        /// </summary>
        /// <param name="?"></param>
        private void CheckKeyMsgOrder(NCasButton selectBtn)
        {
            if (this.selectedOrderMode == NCasDefineOrderMode.None || this.selectedOrderMedia == NCasDefineOrderMedia.None)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderMode);
                return;
            }

            if (selectBtn.CheckedValue == false)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }
                return;
            }

            if (this.lstSelectedOrderIP.Count == 0)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderdestination);
                return;
            }

            if (selectBtn.CheckedValue == true)
            {
                if (!this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Add(selectBtn);
                }

                this.selectedDisasterBroadKind = DisasterBroadKind.StroredMessage;
                this.SetOffButton(NCasKeyAction.MicOrder);
                this.SetOffButton(NCasKeyAction.TtsOrder);
            }
            else
            {
                this.SetBtnChecked(selectBtn, true);
            }

            using (NCasStoredMsgForm form = new NCasStoredMsgForm())
            {
                form.SelectStoredMsgEvent += new EventHandler<SelectStoredMsgEventArgs>(form_SelectStoredMsgEvent);
                form.ShowDialog();
                form.SelectStoredMsgEvent -= new EventHandler<SelectStoredMsgEventArgs>(form_SelectStoredMsgEvent);
            }
        }

        /// <summary>
        /// 저장메시지 화면에서 발생하는 메시지 선택 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void form_SelectStoredMsgEvent(object sender, SelectStoredMsgEventArgs e)
        {
            if (e.SelectFlag) //저장메시지를 선택했으면..
            {
                this.selectedStoredMessage = e.StoredMsg;
                this.storedMessageRepeatCount = e.RepeatCount;
            }
            else
            {
                this.selectedDisasterBroadKind = DisasterBroadKind.Mic;
                this.SetOnButton(NCasKeyAction.MicOrder);
                this.SetOffButton(NCasKeyAction.TtsOrder);
                this.SetOffButton(NCasKeyAction.MsgOrder);
            }
        }
        #endregion

        #region 해제 버튼
        /// <summary>
        /// 해제 버튼 동작
        /// </summary>
        /// <param name="?"></param>
        private void CheckKeyClear(NCasButton selectBtn)
        {
            if (this.selectedOrderMode == NCasDefineOrderMode.None || this.selectedOrderMedia == NCasDefineOrderMedia.None)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderMode);
                return;
            }

            if (selectBtn.CheckedValue == false)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }
                return;
            }

            if (this.lstSelectedOrderIP.Count == 0)
            {
                if (selectBtn.CheckedValue == true)
                {
                    this.SetBtnChecked(selectBtn, false);
                }
                else
                {
                    this.SetBtnChecked(selectBtn, true);
                }

                this.ShowWrongOperationForm(this.NotifyOrderdestination);
                return;
            }

            if (selectBtn.CheckedValue == true)
            {
                this.selectedOrderKind = NCasDefineOrderKind.AlarmCancel;

                if (!this.lstSelectedButtons.Contains(selectBtn))
                {
                    this.lstSelectedButtons.Add(selectBtn);
                }

                this.SetOrderKindButtonColorOn(NCasDefineOrderKind.AlarmCancel);
                this.selectedDisasterBroadKind = DisasterBroadKind.None;
                this.SetOffButton(NCasKeyAction.Ready);
                this.SetOffButton(NCasKeyAction.Attack);
                this.SetOffButton(NCasKeyAction.Watch);
                this.SetOffButton(NCasKeyAction.Biological);
                this.SetOffButton(NCasKeyAction.DisasterBroad);
                this.SetOffButton(NCasKeyAction.DisasterWatch);
                this.SetOffButton(NCasKeyAction.MicOrder);
                this.SetOffButton(NCasKeyAction.TtsOrder);
                this.SetOffButton(NCasKeyAction.MsgOrder);
                this.ShowDisasterBroadPanel(false);
            }
        }
        #endregion

        #region 선택취소 버튼
        /// <summary>
        /// 선택취소 버튼 동작
        /// </summary>
        /// <param name="?"></param>
        private void CheckKeyCancel()
        {
            foreach (Control control in this.controlMainPanel.Controls)
            {
                if (control is Label)
                    continue;

                NCasButton ncasBtn = control as NCasButton;
                NCasKeyData btnKeyData = ncasBtn.Tag as NCasKeyData;

                if (!(btnKeyData.KeyActioin == NCasKeyAction.Real || btnKeyData.KeyActioin == NCasKeyAction.Test ||
                    btnKeyData.KeyActioin == NCasKeyAction.Line || btnKeyData.KeyActioin == NCasKeyAction.Sate))
                {
                    if (NCasAnimator.ContainsItem(ncasBtn))
                    {
                        NCasAnimator.RemoveItem(ncasBtn);
                    }

                    this.SetBtnChecked(ncasBtn, false);

                    if (this.lstSelectedButtons.Contains(ncasBtn))
                    {
                        this.lstSelectedButtons.Remove(ncasBtn);
                    }
                }
            }

            foreach (Control control in this.disasterBroadOrderPanel.Controls)
            {
                if (control is Label)
                    continue;

                NCasButton ncasBtn = control as NCasButton;
                NCasKeyData btnKeyData = ncasBtn.Tag as NCasKeyData;

                if (!(btnKeyData.KeyActioin == NCasKeyAction.Real || btnKeyData.KeyActioin == NCasKeyAction.Test ||
                    btnKeyData.KeyActioin == NCasKeyAction.Line || btnKeyData.KeyActioin == NCasKeyAction.Sate))
                {
                    if (NCasAnimator.ContainsItem(ncasBtn))
                    {
                        NCasAnimator.RemoveItem(ncasBtn);
                    }

                    this.SetBtnChecked(ncasBtn, false);

                    if (this.lstSelectedButtons.Contains(ncasBtn))
                    {
                        this.lstSelectedButtons.Remove(ncasBtn);
                    }
                }
            }

            foreach (Control control in this.distMapPanel.Controls)
            {
                if (control is NCasPanel)
                    continue;

                DistIconUserControl userControl = control as DistIconUserControl;
                NCasButton ncasBtn = userControl.DistIcon;
                NCasKeyData btnKeyData = ncasBtn.Tag as NCasKeyData;

                if (!(btnKeyData.KeyActioin == NCasKeyAction.Real || btnKeyData.KeyActioin == NCasKeyAction.Test ||
                    btnKeyData.KeyActioin == NCasKeyAction.Line || btnKeyData.KeyActioin == NCasKeyAction.Sate))
                {
                    if (NCasAnimator.ContainsItem(ncasBtn))
                    {
                        NCasAnimator.RemoveItem(ncasBtn);
                    }

                    this.SetBtnChecked(ncasBtn, false);

                    if (this.lstSelectedButtons.Contains(ncasBtn))
                    {
                        this.lstSelectedButtons.Remove(ncasBtn);
                    }
                }
            }

            if (this.termDestination != null)
            {
                this.termDestination.Close();
                this.termDestination_TermDestinationCancelEvent(this, new EventArgs());
                this.termDestination = null;
            }

            if (this.orderDistTermForm != null)
            {
                this.orderDistTermForm.Close();
                this.orderDistTermForm.TermAllSelectEvent -= new EventHandler(orderDistTermForm_TermAllSelectEvent);
                this.orderDistTermForm.TermAllCancelEvent -= new EventHandler(orderDistTermForm_TermAllCancelEvent);
                this.orderDistTermForm.TermDestinationCancelEvent -= new EventHandler(orderDistTermForm_TermDestinationCancelEvent);
                this.orderDistTermForm.AddTermDestinationEvent -= new EventHandler<TermSelectEventArgs>(orderDistTermForm_AddTermDestinationEvent);
                this.orderDistTermForm.RemoveTermDestinationEvent -= new EventHandler<TermSelectEventArgs>(orderDistTermForm_RemoveTermDestinationEvent);
                this.orderDistTermForm = null;
            }

            this.lstSelectedOrderIP.Clear();
            this.selectedOrderKind = NCasDefineOrderKind.None;
            this.selectedDisasterBroadKind = DisasterBroadKind.None;
            this.allDestinationFlag = false;
            this.orderTermFlag = false;
            this.orderTermGroupFlag = false;
            this.orderDistGroupFlag = false;
            this.orderDistFlag = false;
            this.orderDistTermFlag = false;
            this.orderDistTermAllFlag = false;
            this.orderTermAllFlag = false;
            this.ShowDisasterBroadPanel(false);
        }
        #endregion

        #region 확인 버튼
        /// <summary>
        /// 확인 버튼 동작
        /// </summary>
        /// <param name="?"></param>
        private void CheckKeyConfirm(NCasButton selectBtn)
        {
            if (this.selectedOrderMode == NCasDefineOrderMode.None || this.selectedOrderMedia == NCasDefineOrderMedia.None)
            {
                this.ShowWrongOperationForm(this.NotifyOrderMode);
                return;
            }

            if (this.lstSelectedOrderIP.Count == 0)
            {
                this.ShowWrongOperationForm(this.NotifyOrderdestination);
                return;
            }

            if (this.selectedOrderKind == NCasDefineOrderKind.None)
            {
                this.ShowWrongOperationForm(this.NotifyOrderKind);
                return;
            }

            this.Confirm();
            this.SetOrderDestinationButtonColorOff();

            if (this.allDestinationFlag)
            {
                this.SetOrderKindButtonColorProvAll(this.selectedOrderKind);

                //발령 후 발령대상 선택 - 요구사항에 따라 주석 처리
                this.SetOnButton(NCasKeyAction.ProveAllDestination);
                this.SetOnButton(NCasKeyAction.TermDestination);
                this.SetOnButton(NCasKeyAction.TermGroupDestination);
                this.SetOnButton(NCasKeyAction.DistGroupDestination);
                this.SetOnButton(NCasKeyAction.DistOneDestination);
            }
            else
            {
                foreach (NCasButton btn in this.lstSelectedButtons)
                {
                    if ((btn.Tag as NCasKeyData).KeyActioin == NCasKeyAction.TermDestination
                        || (btn.Tag as NCasKeyData).KeyActioin == NCasKeyAction.TermGroupDestination
                        || (btn.Tag as NCasKeyData).KeyActioin == NCasKeyAction.DistGroupDestination)
                    {
                        this.SetOrderKindButtonColor(btn, this.selectedOrderKind);

                        if (this.orderTermFlag || this.orderTermAllFlag)
                        {
                            this.SetBtnChecked(btn, true);
                        }

                        //발령 후 발령대상 선택 해제 - 요구사항에 따라 주석 처리
                        this.SetBtnChecked(btn, true);
                    }
                    else if ((btn.Tag as NCasKeyData).KeyActioin == NCasKeyAction.DistOneDestination)
                    {
                        this.SetOrderKindButtonColorDistIcon(btn, this.selectedOrderKind);

                        //발령 후 발령대상 선택 해제 - 요구사항에 따라 주석 처리
                        this.SetBtnChecked(btn, true);
                    }
                }
            }

            //발령 후 발령대상 선택 해제 - 요구사항에 따라 처리
            //this.SetOffButtonAny(NCasKeyAction.ProveAllDestination);

            //if (!this.orderTermFlag && !this.orderTermAllFlag)
            //{
            //    this.SetOffButtonAny(NCasKeyAction.TermDestination);
            //}

            //this.SetOffButtonAny(NCasKeyAction.TermGroupDestination);
            //this.SetOffButtonAny(NCasKeyAction.DistGroupDestination);
            //this.SetOffButtonAny(NCasKeyAction.DistOneDestination);

            //if (!this.orderTermFlag && !this.orderTermAllFlag && !this.orderDistTermFlag && !this.orderDistTermAllFlag)
            //{
            //    this.lstSelectedOrderIP.Clear();
            //    this.orderTermFlag = false;
            //    this.orderTermAllFlag = false;
            //    this.orderDistTermFlag = false;
            //    this.orderDistTermAllFlag = false;
            //}

            //this.allDestinationFlag = false;
            //this.orderTermGroupFlag = false;
            //this.orderDistGroupFlag = false;
            //this.orderDistFlag = false;
            this.lastOrderKind = this.selectedOrderKind;
            this.lastDisasterBroadKind = this.selectedDisasterBroadKind;
            this.ShowDisasterBroadPanel(false);

            if (this.selectedDisasterBroadKind == DisasterBroadKind.StroredMessage)
            {
                if (this.orderStoredViewForm != null)
                {
                    this.orderStoredViewForm.StoredMsgFinishEvent -= new EventHandler(orderStoredViewForm_StoredMsgFinishEvent);
                    this.orderStoredViewForm.Close();
                    this.orderStoredViewForm = null;
                }

                this.orderStoredViewForm = new OrderStoredViewForm();
                this.orderStoredViewForm.StoredMsgFinishEvent += new EventHandler(orderStoredViewForm_StoredMsgFinishEvent);
                this.orderStoredViewForm.StartSirenForm(this.selectedStoredMessage.PlayTime);
                this.orderStoredViewForm.Show(this);
            }
            else
            {
                if (this.orderStoredViewForm != null)
                {
                    this.orderStoredViewForm.StoredMsgFinishEvent -= new EventHandler(orderStoredViewForm_StoredMsgFinishEvent);
                    this.orderStoredViewForm.Close();
                    this.orderStoredViewForm = null;
                }
            }

            if (this.lastOrderKind == NCasDefineOrderKind.AlarmStandby)
            {
                this.orderStandbyFlag = true;
            }
            else if (this.lastOrderKind == NCasDefineOrderKind.AlarmCancel)
            {
                this.orderStandbyFlag = false;
                this.SetOrderDestinationButtonColorOff();
                this.selectedOrderKind = NCasDefineOrderKind.None;
                this.lstSelectedOrderIP.Clear();
                this.allDestinationFlag = false;
                this.orderTermGroupFlag = false;
                this.orderDistGroupFlag = false;
                this.orderDistFlag = false;
                this.orderDistTermFlag = false;
                this.orderDistTermAllFlag = false;
                this.orderTermFlag = false;
                this.orderTermAllFlag = false;
                this.SetOrderKindButtonColorProvAll(this.selectedOrderKind);
                this.SetOffButtonAny(NCasKeyAction.ProveAllDestination);
                this.SetOffButtonAny(NCasKeyAction.TermDestination);
                this.SetOffButtonAny(NCasKeyAction.TermGroupDestination);
                this.SetOffButtonAny(NCasKeyAction.DistGroupDestination);
                this.SetOffButtonAny(NCasKeyAction.DistOneDestination);

                if (this.termDestination != null)
                {
                    this.termDestination.Close();
                    this.termDestination_TermDestinationCancelEvent(this, new EventArgs());
                    this.termDestination = null;
                }

                if (this.orderDistTermForm != null)
                {
                    this.orderDistTermForm.Close();
                    this.orderDistTermForm.TermAllSelectEvent -= new EventHandler(orderDistTermForm_TermAllSelectEvent);
                    this.orderDistTermForm.TermAllCancelEvent -= new EventHandler(orderDistTermForm_TermAllCancelEvent);
                    this.orderDistTermForm.TermDestinationCancelEvent -= new EventHandler(orderDistTermForm_TermDestinationCancelEvent);
                    this.orderDistTermForm.AddTermDestinationEvent -= new EventHandler<TermSelectEventArgs>(orderDistTermForm_AddTermDestinationEvent);
                    this.orderDistTermForm.RemoveTermDestinationEvent -= new EventHandler<TermSelectEventArgs>(orderDistTermForm_RemoveTermDestinationEvent);
                    this.orderDistTermForm = null;
                }
            }

            this.ClearOrderKind();
        }
        #endregion

        #region Confirm method
        /// <summary>
        /// 확인 버튼에 대한 처리 메소드
        /// </summary>
        private void Confirm()
        {
            DateTime orderTime = DateTime.Now;
            List<OrderBizData> tmpOrderBuff = new List<OrderBizData>();

            for (int i = 0; i < this.lstSelectedOrderIP.Count; i++)
            {
                NCasProtocolBase protoBase = NCasProtocolFactory.CreateCasProtocol(NCasDefineTcCode.TcAlarmOrder);
                NCasProtocolTc1 protoTc1 = protoBase as NCasProtocolTc1;

                protoTc1.AlarmNetIdOrIpByString = this.lstSelectedOrderIP[i];
                protoTc1.OrderTimeByDateTime = orderTime;
                protoTc1.CtrlKind = NCasDefineControlKind.ControlAlarm;
                protoTc1.Source = NCasDefineOrderSource.ProvCtrlRoom;
                protoTc1.AlarmKind = this.selectedOrderKind;
                protoTc1.Mode = this.selectedOrderMode;
                protoTc1.Media = this.selectedOrderMedia;
                protoTc1.RespReqFlag = NCasDefineRespReq.ResponseReq;
                protoTc1.AuthenFlag = NCasDefineAuthenticationFlag.EncodeUsed;
                protoTc1.Sector = (this.allDestinationFlag == true) ? NCasDefineSectionCode.SectionProv :
                    (this.orderTermFlag == true) ? NCasDefineSectionCode.SectionTerm :
                    (this.orderTermAllFlag == true) ? NCasDefineSectionCode.SectionProv :
                    (this.orderTermGroupFlag == true) ? NCasDefineSectionCode.SectionTerm :
                    (this.orderDistGroupFlag == true) ? NCasDefineSectionCode.SectionDist :
                    (this.orderDistFlag == true) ? NCasDefineSectionCode.SectionDist :
                    (this.orderDistTermFlag == true) ? NCasDefineSectionCode.SectionTerm :
                    (this.orderDistTermAllFlag == true) ? NCasDefineSectionCode.SectionDist : NCasDefineSectionCode.None;

                byte[] tmpBuff = NCasProtocolFactory.MakeUdpFrame(protoTc1);
                OrderBizData orderBizData = new OrderBizData();
                orderBizData.AlmProtocol = protoTc1;
                orderBizData.IsLocal = true;
                orderBizData.LastOrderKind = this.lastOrderKind;
                orderBizData.AllDestinationFlag = this.allDestinationFlag;
                orderBizData.OrderTermFlag = this.orderTermFlag;
                orderBizData.OrderTermAllFlag = this.orderTermAllFlag;
                orderBizData.OrderTermGroupFlag = this.orderTermGroupFlag;
                orderBizData.OrderDistGroupFlag = this.orderDistGroupFlag;
                orderBizData.OrderDistFlag = this.orderDistFlag;
                orderBizData.OrderDistTermFlag = this.orderDistTermFlag;
                orderBizData.OrderDistTermAllFlag = this.orderDistTermAllFlag;
                orderBizData.SelectedDisasterBroadKind = this.selectedDisasterBroadKind;
                orderBizData.SelectedStoredMessage = this.selectedStoredMessage;
                orderBizData.StoredMessageRepeatCount = this.storedMessageRepeatCount;
                orderBizData.SelectedTtsMessage = this.selectedTtsMessage;
                orderBizData.GroupName = this.groupNameLst;
                orderBizData.SendBuff = tmpBuff;

                if (i == 0)
                {
                    orderBizData.IsEnd = OrderDataSendStatus.First;
                }
                else if (i == (this.lstSelectedOrderIP.Count - 1))
                {
                    orderBizData.IsEnd = OrderDataSendStatus.End;
                }
                else
                {
                    orderBizData.IsEnd = OrderDataSendStatus.None;
                }

                if (this.lstSelectedOrderIP.Count == 1)
                {
                    orderBizData.IsEnd = OrderDataSendStatus.FirstEnd;
                }

                if (this.lastDisasterBroadKind == DisasterBroadKind.Tts) //마지막 발령이 TTS발령인지..
                {
                    orderBizData.TtsOrderFlag = true;
                }
                else
                {
                    orderBizData.TtsOrderFlag = false;
                }

                tmpOrderBuff.Add(orderBizData);
            }

            foreach (OrderBizData orderBizData in tmpOrderBuff)
            {
                this.main.SetOrderBizData(orderBizData);
            }
        }
        #endregion

        #region debug
        /// <summary>
        /// 버튼 상태를 확인하기 위한 메소드
        /// </summary>
        private void showButtonStateDebug()
        {
            #region 상태 표출 테스트
            Console.WriteLine("\n선택한 버튼리스트에 있는 버튼 수 - " + this.lstSelectedButtons.Count.ToString());

            Console.WriteLine("선택한 발령 모드 - " + ((this.selectedOrderMode == NCasDefineOrderMode.None) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderMode2String(NCasDefineOrderMode.None) :
                (this.selectedOrderMode == NCasDefineOrderMode.RealMode) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderMode2String(NCasDefineOrderMode.RealMode) :
                (this.selectedOrderMode == NCasDefineOrderMode.TestMode) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderMode2String(NCasDefineOrderMode.TestMode) : "실패"));

            Console.WriteLine("선택한 미디어 모드 - " + ((this.selectedOrderMedia == NCasDefineOrderMedia.None) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderMedia2String(NCasDefineOrderMedia.None) :
                (this.selectedOrderMedia == NCasDefineOrderMedia.MediaAll) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderMedia2String(NCasDefineOrderMedia.MediaAll) :
                (this.selectedOrderMedia == NCasDefineOrderMedia.MediaLine) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderMedia2String(NCasDefineOrderMedia.MediaLine) :
                (this.selectedOrderMedia == NCasDefineOrderMedia.MediaSate) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderMedia2String(NCasDefineOrderMedia.MediaSate) : "실패"));

            Console.WriteLine("선택한 시도 발령대상 수 - " + lstSelectedOrderIP.Count.ToString());
            foreach (string str in this.lstSelectedOrderIP)
            {
                Console.WriteLine("      대상은 - " + str);
            }

            Console.WriteLine("선택한 발령종류 - " + ((this.selectedOrderKind == NCasDefineOrderKind.AlarmStandby) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmStandby) :
                    (this.selectedOrderKind == NCasDefineOrderKind.AlarmWatch) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmWatch) :
                    (this.selectedOrderKind == NCasDefineOrderKind.AlarmAttack) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmAttack) :
                    (this.selectedOrderKind == NCasDefineOrderKind.AlarmBiochemist) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmBiochemist) :
                    (this.selectedOrderKind == NCasDefineOrderKind.AlarmCancel) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmCancel) :
                    (this.selectedOrderKind == NCasDefineOrderKind.AlarmClose) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmClose) :
                    (this.selectedOrderKind == NCasDefineOrderKind.DisasterWatch) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.DisasterWatch) :
                    (this.selectedOrderKind == NCasDefineOrderKind.VoiceLineTest) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.VoiceLineTest) :
                    (this.selectedOrderKind == NCasDefineOrderKind.BroadPublicVoice) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.BroadPublicVoice) :
                    (this.selectedOrderKind == NCasDefineOrderKind.BroadMessage) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.BroadMessage) :
                    (this.selectedOrderKind == NCasDefineOrderKind.TermTts) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.TermTts) :
                    (this.selectedOrderKind == NCasDefineOrderKind.CenterTts) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.CenterTts) :
                    (this.selectedOrderKind == NCasDefineOrderKind.AlarmRecover) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmRecover) :
                    (this.selectedOrderKind == NCasDefineOrderKind.DisasterBoradRecover) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.DisasterBoradRecover) :
                    (this.selectedOrderKind == NCasDefineOrderKind.DisasterStandby) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.DisasterStandby) :
                    (this.selectedOrderKind == NCasDefineOrderKind.DisasterBroadcast) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.DisasterBroadcast) : ""));

            Console.WriteLine("가장 마지막 발령종류 - " + ((this.lastOrderKind == NCasDefineOrderKind.AlarmStandby) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmStandby) :
                    (this.lastOrderKind == NCasDefineOrderKind.AlarmWatch) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmWatch) :
                    (this.lastOrderKind == NCasDefineOrderKind.AlarmAttack) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmAttack) :
                    (this.lastOrderKind == NCasDefineOrderKind.AlarmBiochemist) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmBiochemist) :
                    (this.lastOrderKind == NCasDefineOrderKind.AlarmCancel) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmCancel) :
                    (this.lastOrderKind == NCasDefineOrderKind.AlarmClose) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmClose) :
                    (this.lastOrderKind == NCasDefineOrderKind.DisasterWatch) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.DisasterWatch) :
                    (this.lastOrderKind == NCasDefineOrderKind.VoiceLineTest) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.VoiceLineTest) :
                    (this.lastOrderKind == NCasDefineOrderKind.BroadPublicVoice) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.BroadPublicVoice) :
                    (this.lastOrderKind == NCasDefineOrderKind.BroadMessage) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.BroadMessage) :
                    (this.lastOrderKind == NCasDefineOrderKind.TermTts) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.TermTts) :
                    (this.lastOrderKind == NCasDefineOrderKind.CenterTts) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.CenterTts) :
                    (this.lastOrderKind == NCasDefineOrderKind.AlarmRecover) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.AlarmRecover) :
                    (this.lastOrderKind == NCasDefineOrderKind.DisasterBoradRecover) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.DisasterBoradRecover) :
                    (this.lastOrderKind == NCasDefineOrderKind.DisasterStandby) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.DisasterStandby) :
                    (this.lastOrderKind == NCasDefineOrderKind.DisasterBroadcast) ? NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(NCasDefineOrderKind.DisasterBroadcast) : ""));

            Console.WriteLine("시도 전체 선택 - " + ((this.allDestinationFlag == true) ? "O" : "X"));

            Console.WriteLine("단말 그룹 발령 선택 - " + ((this.orderTermGroupFlag == true) ? "O" : "X"));

            Console.WriteLine("시군 그룹 발령 선택 - " + ((this.orderDistGroupFlag == true) ? "O" : "X"));

            Console.WriteLine("시군 발령 선택 - " + ((this.orderDistFlag == true) ? "O" : "X"));

            Console.WriteLine("시군아이콘으로 개별 발령 선택 - " + ((this.orderDistTermFlag == true) ? "O" : "X"));

            Console.WriteLine("시군아이콘으로 시군전체 발령 선택 - " + ((this.orderDistTermAllFlag == true) ? "O" : "X"));

            Console.WriteLine("개별단말 발령 선택 - " + ((this.orderTermFlag == true) ? "O" : "X"));

            Console.WriteLine("개별단말로 시도전체 발령 선택 - " + ((this.orderTermAllFlag == true) ? "O" : "X"));

            Console.WriteLine("예비 발령 - " + ((this.orderStandbyFlag == true) ? "O" : "X"));

            Console.WriteLine("재난경계 종류 선택한 거 - " + ((this.selectedDisasterBroadKind == DisasterBroadKind.None) ? "선택안함" :
                (this.selectedDisasterBroadKind == DisasterBroadKind.Mic) ? "마이크" :
                (this.selectedDisasterBroadKind == DisasterBroadKind.Tts) ? "TTS" :
                (this.selectedDisasterBroadKind == DisasterBroadKind.StroredMessage) ? "저장메시지" : "선택안함"));

            Console.WriteLine("마지막 재난경계 발령한 거 - " + ((this.lastDisasterBroadKind == DisasterBroadKind.None) ? "선택안함" :
                (this.lastDisasterBroadKind == DisasterBroadKind.Mic) ? "마이크" :
                (this.lastDisasterBroadKind == DisasterBroadKind.Tts) ? "TTS" :
                (this.lastDisasterBroadKind == DisasterBroadKind.StroredMessage) ? "저장메시지" : "선택안함"));

            Console.WriteLine("선택한 그룹 버튼 수 - " + this.selectedGroupContents.Count.ToString());
            foreach (GroupContent groupContent in this.selectedGroupContents)
            {
                Console.WriteLine("      버튼은 - " + groupContent.Title);
            }

            Console.WriteLine("저장메시지 - ");
            Console.WriteLine("      제목 - " + this.selectedStoredMessage.Title);
            Console.WriteLine("      번호 - " + this.selectedStoredMessage.MsgNum);
            Console.WriteLine("      시간 - " + this.selectedStoredMessage.PlayTime.ToString());
            Console.WriteLine("      반복횟수 - " + this.storedMessageRepeatCount.ToString());

            Console.WriteLine("TTS메시지 - ");
            Console.WriteLine("      제목 - " + this.selectedTtsMessage.Title);
            Console.WriteLine("      내용 - " + this.selectedTtsMessage.Text);
            #endregion
        }
        #endregion
    }
}