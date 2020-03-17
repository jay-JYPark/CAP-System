//#define debug
#define release

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Runtime.InteropServices;

using NCASFND;
using NCASFND.NCasIo;
using NCASFND.NCasNet;
using NCASFND.NCasCtrl;
using NCASFND.NCasLogging;
using NCASBIZ;
using NCASBIZ.NCasEnv;
using NCASBIZ.NCasType;
using NCASBIZ.NCasBiz;
using NCASBIZ.NCasData;
using NCASBIZ.NCasDefine;
using NCASBIZ.NCasUtility;
using NCASBIZ.NCasProtocol;
using NCASBIZ.NCasPlcProtocol;
using NCASBIZ.NCasTwcProtocol;
using NCASBIZ.NCasWmaLedProtocol;
using NCasAppCommon.Define;
using NCasAppCommon.Type;
using NCasMsgCommon.Tts;
using NCasMsgCommon.Std;
using NCasContentsModule;
using NCasContentsModule.StoMsg;
using NCasContentsModule.TTS;
using IEASProtocol;
using WmaBoardTextManager;

namespace WmaPAlmScreen
{
    public partial class MainForm : Form
    {
        #region Dll Import
        [DllImport("user32")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32")]
        public static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        [DllImport("user32.dll")]
        public static extern int PostMessage(int hwnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(int hwnd, int Msg, int wParam, int lParam);
        #endregion

        #region enum
        public enum ViewKind
        {
            None = 0,
            OrderView19201080 = 1,
            ResultView = 2,
            DevMonView = 3
        }
        #endregion

        #region element
        private readonly string IP_LOOPBACK = "127.0.0.1";
        private readonly int PDMainSessionPort = 19999;
        private readonly string TtsEditorFilePath = "C:\\NCAS\\PROV\\BIN\\NCasTtsEditor.exe";
        private readonly float OrderTextSize = 23;
        private NCasMMFMng mmfMng = null;
        private PAlmScreenBiz pDAlmScreenBiz = null;
        private Dictionary<ViewKind, ViewBase> dicViews = new Dictionary<ViewKind, ViewBase>();
        private Timer commonTimer = null;
        private Timer boardTimer = null;
        private List<ViewBase> lstTimerMembers = new List<ViewBase>();
        private NCasNetSessionServerMng pDMainTcpServer = null;
        private NCasUdpSocket udpCasMon = null;
        private NCasUdpSocket recvUdpKey = null;
        private NCasUdpSocket recvUdpLauncher = null;
        private NCasUdpSocket recvWeather = null;
        private NCasUdpSocket recvCapRecv = null;
        private ProvInfo provInfo = null;
        private ViewKind currentViewKind = ViewKind.None;
        private int ttsDelayTime = 5000;
        private uint multiTermCount = 0;
        #endregion

        #region property
        /// <summary>
        /// 시도 데이터파일 프로퍼티
        /// </summary>
        public ProvInfo ProvInfo
        {
            get { return this.provInfo; }
        }

        /// <summary>
        /// NCasMMFMng 프로퍼티
        /// </summary>
        public NCasMMFMng MmfMng
        {
            get { return this.mmfMng; }
        }

        /// <summary>
        /// TTS Delay Time 프로퍼티
        /// </summary>
        public int TtsDelayTime
        {
            get { return this.ttsDelayTime; }
        }
        #endregion

        #region 생성자
        /// <summary>
        /// 생성자
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }
        #endregion

        #region override method
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                NCasBizActivator.Active(NCASBIZ.NCasDefine.NCasDefineActivatorCode.ForProv);
                this.InitMmfInfo(NCasUtilityMng.INCasEtcUtility.GetIPv4());
                NCasEnvironmentMng.NCasEnvConfig.NetSessionContext.UseConnectionChecking = true;
                NCasEnvironmentMng.NCasEnvConfig.NetSessionContext.UsePolling = true;
                NCasEnvironmentMng.NCasEnvConfig.LoggingContext.UseDebugLogging = true;

                NCasProfile profile = new NCasProfile();

#if release
                profile.IpAddr = NCasUtilityMng.INCasCommUtility.AddIpAddr(NCasUtilityMng.INCasEtcUtility.GetIPv4(), 0, 0, 0, 1);
#endif

#if debug
                profile.IpAddr = "158.181.17.226";
#endif

                profile.Port = this.PDMainSessionPort;
                profile.Name = "PDMainScreen";

                this.pDMainTcpServer = new NCasNetSessionServerMng();
                this.pDMainTcpServer.PollingDatas = new byte[] { 0x01 };
                this.pDMainTcpServer.RecvNetSessionClient += new NCasNetSessionRecvEventHandler(pDMainTcpServer_RecvNetSessionClient);
                this.pDMainTcpServer.AddProfile(profile);
                this.pDMainTcpServer.StartSessionServerMng(NCasUtilityMng.INCasEtcUtility.GetIPv4(), this.PDMainSessionPort);

                //this.udpCasMon = new NCasUdpSocket();
                //this.udpCasMon.Listen(IP_LOOPBACK, (int)NCasPortID.PortIdRecvCasMonData);
                //this.udpCasMon.ReceivedData += new NCasUdpRecvEventHandler(udpCasMon_ReceivedData);

                //this.recvUdpKey = new NCasUdpSocket();
                //this.recvUdpKey.Listen(this.IP_LOOPBACK, (int)NCasPipes.PipePdaDevAlmKey);
                //this.recvUdpKey.ReceivedData += new NCasUdpRecvEventHandler(recvUdpKey_ReceivedData);

                //this.recvUdpLauncher = new NCasUdpSocket();
                //this.recvUdpLauncher.Listen(this.IP_LOOPBACK, (int)NCasPipes.PipePdaScreenLauncher);
                //this.recvUdpLauncher.ReceivedData += new NCasUdpRecvEventHandler(recvUdpLauncher_ReceivedData);

                this.recvWeather = new NCasUdpSocket();
                this.recvWeather.Listen(this.IP_LOOPBACK, (int)NCasPipes.PipeWMAWeathData);
                this.recvWeather.ReceivedData += new NCasUdpRecvEventHandler(recvWeather_ReceivedData);

                //recvCapRecv
                this.recvCapRecv = new NCasUdpSocket();
                this.recvCapRecv.Listen(this.IP_LOOPBACK, (int)NCasPipes.PipeWpaScreenCap);
                this.recvCapRecv.ReceivedData += new NCasUdpRecvEventHandler(recvCapRecv_ReceivedData);

                DeviceStatusMng.LoadDeviceStatusDatas();
                PasswordMng.LoadPassword();
                NCasContentsMng.LoadTtsOptionFromFile();
                TtsDelayTimeMng.LoadTtsDelayTime();
                GroupContentMng.LoadGroupContent();
                DistIconMng.LoadDistIconDatas();
                DisasterBroadFlagMng.LoadDisasterBroadFlag();
                HeightOptionMng.LoadHeightOptionDatas();
                HeightPointContentMng.LoadHeightPointContent();
                WeatherOptionMng.LoadWeatherOptionDatas();
                BoardTextMng.LoadBoardTextDatas();

                this.ttsDelayTime = int.Parse(TtsDelayTimeMng.TtsDelayTime);
                this.pDAlmScreenBiz = new PAlmScreenBiz(this);
                this.InitView();
                NCasAnimator.InitAnimator();
                this.OpenView(ViewKind.None);
                this.InitLogoImage(this.provInfo.Code);
                this.StartTimer(1000);
                this.StartBoardTimer(90000);
                this.Text = "시도 서해안 경보시스템 " + NCasUtilityMng.INCasEtcUtility.GetVersionInfo();

                foreach (TermInfo eachTerminfo in this.provInfo.LstTerms)
                {
                    if (eachTerminfo.UseFlag != NCasDefineUseStatus.Use)
                        continue;

                    if (eachTerminfo.TermFlag != NCasDefineTermKind.TermMutil)
                        continue;

                    this.multiTermCount++;
                }

                this.labelTotalTermCount.Text = this.multiTermCount.ToString();

#if release
                PDevInfo pDevInfo = this.mmfMng.GetPDevInfoByIp(NCasUtilityMng.INCasEtcUtility.GetIPv4());
#endif

#if debug
                PDevInfo pDevInfo = this.mmfMng.GetPDevInfoByIp("10.24.1.221");
#endif

                if (pDevInfo == null)
                {
                    MessageBox.Show("IP가 정상적이지 않습니다.\n네트워크를 확인하세요.", "시도 서해안 경보시스템", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    NCasLoggingMng.ILogging.WriteLog("MainForm", "IP가 정상적이지 않습니다.");
                }

                if (!(pDevInfo.DevId == NCasDefineDeviceKind.MutilAlarmCtrlSys))
                {
                    MessageBox.Show("IP가 정상적이지 않습니다.\n네트워크를 확인하세요.", "시도 서해안 경보시스템", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    NCasLoggingMng.ILogging.WriteLog("MainForm", "IP가 정상적이지 않습니다.");
                }

                NCasLoggingMng.ILogging.WriteLog("MainForm", "기초데이터 로딩 성공!");
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILoggingException.WriteException("MainForm", "MainForm.OnLoad(EventArgs e) Method", ex);
                NCasLoggingMng.ILogging.WriteLog("MainForm", "기초데이터 로딩 실패!");
                return;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.StopTimer();
            this.StopBoardTimer();

            if (this.pDMainTcpServer != null)
            {
                this.pDMainTcpServer.RecvNetSessionClient -= new NCasNetSessionRecvEventHandler(pDMainTcpServer_RecvNetSessionClient);
                this.pDMainTcpServer.StopSessionServerMng();
            }

            if (this.udpCasMon != null)
            {
                this.udpCasMon.Close();
            }

            if (this.recvUdpKey != null)
            {
                this.recvUdpKey.Close();
            }

            if (this.recvUdpLauncher != null)
            {
                this.recvUdpLauncher.Close();
            }

            if (this.recvWeather != null)
            {
                this.recvWeather.Close();
            }

            if (this.pDAlmScreenBiz != null)
            {
                this.pDAlmScreenBiz.UnInit();
            }

            this.mmfMng.CloseAllMMF();
            NCasAnimator.UninitAnimator();
            NCasBizActivator.Inactive();

            Process[] findProcess = Process.GetProcessesByName("NCasTtsEditor");

            foreach (Process process in findProcess)
            {
                process.Kill();
            }
        }
        #endregion

        #region UI Event
        /// <summary>
        /// 환경설정 창
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void topRightLogoPictureBox_DoubleClick(object sender, EventArgs e)
        {
            using (OptionForm optionForm = new OptionForm(this.provInfo))
            {
                optionForm.ShowDialog();
            }
        }

        /// <summary>
        /// cover 메뉴 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void topLeftNameLabel_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.currentViewKind == ViewKind.None)
                return;

            btnOrderMenu.CheckedValue = false;
            btnOrderResultMenu.CheckedValue = false;
            btnDevMonMenu.CheckedValue = false;
            this.OpenView(ViewKind.None);
            this.currentViewKind = ViewKind.None;
        }

        /// <summary>
        /// Top 메뉴 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOrderMenu_MouseDown(object sender, MouseEventArgs e)
        {
            NCasButton btnSelect = sender as NCasButton;
            ViewKind selectViewKind = (ViewKind)btnSelect.Tag;
            btnOrderMenu.CheckedValue = false;
            btnOrderResultMenu.CheckedValue = false;
            btnDevMonMenu.CheckedValue = false;

            if (selectViewKind == this.currentViewKind)
            {
                if (btnSelect.CheckedValue == false)
                {
                    btnSelect.CheckedValue = true;
                }

                return;
            }

            this.OpenView(selectViewKind);
            this.currentViewKind = selectViewKind;
        }

        /// <summary>
        /// TTS편집 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBroadTextMenu_Click(object sender, EventArgs e)
        {
            try
            {
                if (!System.IO.File.Exists(this.TtsEditorFilePath))
                {
                    MessageBox.Show("TTS편집기가 아래 경로에 없습니다.\n" + this.TtsEditorFilePath + "를 확인하세요.", "TTS편집", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.btnBroadTextMenu.CheckedValue = false;
                    return;
                }

                Process process = new Process();
                process.StartInfo.FileName = this.TtsEditorFilePath;
                process.Start();

                Process[] findProcess = Process.GetProcessesByName("NCasTtsEditor");
                IntPtr windows = IntPtr.Zero;

                foreach (Process eachProcess in findProcess)
                {
                    if (eachProcess.ProcessName == "NCasTtsEditor")
                    {
                        windows = eachProcess.MainWindowHandle;
                        break;
                    }
                }

                if (windows != IntPtr.Zero && (int)windows > 0)
                {
                    Screen scr = Screen.PrimaryScreen;
                    Rectangle rec = scr.Bounds;

                    if (rec.Width < 1002 || rec.Height < 785)
                    {
                        SetWindowPos(windows.ToInt32(), -1, 0, 0, 1002, 785, 0x10);
                    }
                    else
                    {
                        SetWindowPos(windows.ToInt32(), -1, (rec.Width - 1002) / 2, (rec.Height - 785) / 2, 1002, 785, 0x10);
                    }
                }

                process.WaitForExit();
                this.btnBroadTextMenu.CheckedValue = false;
                findProcess = Process.GetProcessesByName("NCasTtsEditor");

                foreach (Process eachProcess in findProcess)
                {
                    eachProcess.Kill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("TTS편집기가 정상적이지 않습니다.\nTTS편집기를 확인하세요.", "TTS편집", MessageBoxButtons.OK, MessageBoxIcon.Error);
                NCasLoggingMng.ILogging.WriteLog("MainForm.btnBroadTextMenu_Click", "TTS편집기 실행 - " + ex.Message);
            }
        }

        /// <summary>
        /// 전광판 관리 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBoardTextMenu_Click(object sender, EventArgs e)
        {
            using (WmaBoardTextForm form = new WmaBoardTextForm())
            {
                form.OnBoardMsgAddEvent += new EventHandler<BoardMsgAddEventArgs>(form_OnBoardMsgAddEvent);
                form.OnBoardMsgModifyEvent += new EventHandler<BoardMsgAddEventArgs>(form_OnBoardMsgModifyEvent);
                form.OnBoardMsgDeleteEvent += new EventHandler<BoardMsgAddEventArgs>(form_OnBoardMsgDeleteEvent);
                form.OnBoardMsgSendEvent += new EventHandler<BoardMsgSendEventArgs>(form_OnBoardMsgSendEvent);
                form.ShowDialog();
                form.OnBoardMsgAddEvent -= new EventHandler<BoardMsgAddEventArgs>(form_OnBoardMsgAddEvent);
                form.OnBoardMsgModifyEvent -= new EventHandler<BoardMsgAddEventArgs>(form_OnBoardMsgModifyEvent);
                form.OnBoardMsgDeleteEvent -= new EventHandler<BoardMsgAddEventArgs>(form_OnBoardMsgDeleteEvent);
                form.OnBoardMsgSendEvent -= new EventHandler<BoardMsgSendEventArgs>(form_OnBoardMsgSendEvent);
            }

            this.btnBoardTextMenu.CheckedValue = false;
        }
        #endregion

        #region event method
        /// <summary>
        /// 전광판 문안관리화면에서 방생시키는 문안 전송 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void form_OnBoardMsgSendEvent(object sender, BoardMsgSendEventArgs e)
        {
            foreach (string eachStr in e.LstTerm)
            {
                NCasWmaLedProtocolCmd1 proto1 = NCasWmaLedProtocolFactory.CreateWmaProtocol(NCasDefineWmaCommand.LedMsg) as NCasWmaLedProtocolCmd1;
                proto1.StartYear = (byte)(DateTime.Now.Year - 2000);
                proto1.StartMonth = (byte)DateTime.Now.Month;
                proto1.StartDay = (byte)DateTime.Now.Day;
                proto1.StartHour = (byte)DateTime.Now.Hour;
                proto1.StartMin = (byte)DateTime.Now.Minute;
                proto1.EndYear = (byte)(DateTime.Now.Year - 2000);
                proto1.EndMonth = (byte)DateTime.Now.Month;
                proto1.EndDay = (byte)DateTime.Now.Day;
                proto1.EndHour = (byte)DateTime.Now.Hour;
                proto1.EndMin = (byte)(DateTime.Now.Minute + 2);
                proto1.FinishNum = NCASBIZ.NCasDefine.NCasDefineWmaFinishEffect.LeftToClear;
                proto1.FinishSpeed = NCASBIZ.NCasDefine.NCasDefineWmaSpeedEffect.Fast4;
                proto1.Msg = e.BoardText.Message;
                proto1.PrintFinishWaitTimeInSec = 4;
                proto1.PrintNum = NCASBIZ.NCasDefine.NCasDefineWmaViewEffect.LeftScroll;
                proto1.PrintSpeed = NCASBIZ.NCasDefine.NCasDefineWmaSpeedEffect.Fast4;
                proto1.RoomNum = 1;
                proto1.Siren = NCASBIZ.NCasDefine.NCasDefineWmaSirenUse.UnUsed;
                byte[] ledBuff = NCasWmaLedProtocolFactory.MakeFrame(proto1);

                NCasProtocolTc80 proto80 = NCasProtocolFactory.CreateCasProtocol(NCasDefineTcCode.TcWmaLed) as NCasProtocolTc80;
                proto80.LedData = new byte[ledBuff.Length];
                proto80.TermIpByString = eachStr;
                Array.Copy(ledBuff, proto80.LedData, ledBuff.Length);
                byte[] sendBuff = NCasProtocolFactory.MakeTcpFrame(proto80);

                LedBizData ledBizData = new LedBizData();
                ledBizData.ProtocolTc80 = proto80;
                ledBizData.SendBuff = sendBuff;
                this.SetLedBizData(ledBizData);
            }
        }

        /// <summary>
        /// 전광판 문안관리화면에서 발생시키는 문안 삭제 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void form_OnBoardMsgDeleteEvent(object sender, BoardMsgAddEventArgs e)
        {
            this.StopBoardTimer();
            BoardTextMng.LoadBoardTextDatas();
            this.StartBoardTimer(90000);
        }

        /// <summary>
        /// 전광판 문안관리화면에서 발생시키는 문안 수정 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void form_OnBoardMsgModifyEvent(object sender, BoardMsgAddEventArgs e)
        {
            this.StopBoardTimer();
            BoardTextMng.LoadBoardTextDatas();
            this.StartBoardTimer(90000);
        }

        /// <summary>
        /// 전광판 문안관리화면에서 발생시키는 문안 추가 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void form_OnBoardMsgAddEvent(object sender, BoardMsgAddEventArgs e)
        {
            this.StopBoardTimer();
            BoardTextMng.LoadBoardTextDatas();
            this.StartBoardTimer(90000);
        }

        /// <summary>
        /// 재난운영대에서 수신되는 데이터 리시브 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pDMainTcpServer_RecvNetSessionClient(object sender, NCasNetSessionRecvEventArgs e)
        {
            if (e.Len == 1)
                return;

            byte[] tmpBuff = new byte[e.Len];
            System.Buffer.BlockCopy(e.Buff, 0, tmpBuff, 0, e.Len);
            XmlSerializer serializer = new XmlSerializer(typeof(DistIconDataContainer), new Type[] { typeof(DistIconData) });
            string tmpStr = Encoding.UTF8.GetString(tmpBuff, 0, tmpBuff.Length);
            StringReader sr = new StringReader(tmpStr);
            DistIconDataContainer distIconDataContainer = (DistIconDataContainer)serializer.Deserialize(sr);

            DistIconMng.LstDistIconData = distIconDataContainer.LstDistIconData;
            DistIconMng.SaveDistIconDatas();
            OrderView19201080 orderView = (OrderView19201080)this.dicViews[ViewKind.OrderView19201080];

            MethodInvoker invoker = delegate()
            {
                orderView.SetDistIconReArrange();
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

        /// <summary>
        /// commonTimer Tick 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void commonTimer_Tick(object sender, EventArgs e)
        {
            this.labelMainTime.Text = NCasUtilityMng.INCasCommUtility.MakeDateTimeFormatForCasSystem(DateTime.Now);

            if (this.provInfo.AlarmOrderInfo.OccurTimeToDateTime > this.provInfo.WwsAlarmOrderInfo.OccurTimeToDateTime)
            {
                this.SetOrderText();
                this.SetOrderResponseCount();
            }
            else
            {
                this.SetAutoOrderText();
                this.SetAutoOrderResponseCount();
            }

            foreach (ViewBase viewBase in this.lstTimerMembers)
            {
                viewBase.OnTimer();
            }
        }

        /// <summary>
        /// CAP발령에 대한 응답 처리
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void recvCapRecv_ReceivedData(object sender, NCasUdpRecvEventArgs e)
        {
            try
            {
                byte[] tmpBuff = new byte[e.Len];
                System.Buffer.BlockCopy(e.Buff, 0, tmpBuff, 0, e.Len);
                NCasLoggingMng.ILogging.WriteLog("CAP 응답 데이터", "단말에서 수신받는 CAP응답 Low Data - " + NCasUtilityMng.INCasEtcUtility.Bytes2HexString(tmpBuff));

                IEASProtocolBase prtBase = IEASProtocolManager.ParseFrame(tmpBuff);
                IEASPrtCmd2 prtCmd2 = prtBase as IEASPrtCmd2;
                NCasLoggingMng.ILogging.WriteLog("CAP 응답 데이터", prtCmd2.CAPMessage);
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILoggingException.WriteException("MainForm", "단말에서 수신받는 CAP응답 Method Exception", ex);
            }
        }

        /// <summary>
        /// 기상수집데몬에서 수신받는 기상정보 데이트 (특보/조위)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void recvWeather_ReceivedData(object sender, NCasUdpRecvEventArgs e)
        {
            try
            {
                byte[] tmpBuff = new byte[e.Len];
                System.Buffer.BlockCopy(e.Buff, 0, tmpBuff, 0, e.Len);
                NCasTwcProtocolBase protobase = NCasTwcProtocolFactory.ParseFrame(tmpBuff);

                if (protobase.Command == NCasDefineTwcCommand.SpecialWeather)
                {
                    NCasTwcProtocolCmd1 protoTc1 = protobase as NCasTwcProtocolCmd1;
                    this.AutoProcWeather(protoTc1);
                }
                else if (protobase.Command == NCasDefineTwcCommand.TideLevel)
                {
                    NCasTwcProtocolCmd2 protoTc2 = protobase as NCasTwcProtocolCmd2;
                    this.AutoProcHeight(protoTc2);
                }
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILogging.WriteLog("MainForm", "기상수집 데몬에서 수신받는 Method Exception - " + ex.Message);
            }
        }

        /// <summary>
        /// NCasLauncher에서 수신받는 프로그램 종료 데이터
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void recvUdpLauncher_ReceivedData(object sender, NCasUdpRecvEventArgs e)
        {
            try
            {
                byte[] tmpBuff = new byte[e.Len];
                System.Buffer.BlockCopy(e.Buff, 0, tmpBuff, 0, e.Len);
                NCasLauncherCmdData launcherCmdData = new NCasLauncherCmdData(tmpBuff);

                if (launcherCmdData.Command == NCasLauncherCmd.CloseProcess)
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILogging.WriteLog("MainForm", "NCasLauncher에서 수신받는 Method Exception - " + ex.Message);
            }
        }

        /// <summary>
        /// 지진해일 조작반 UDP 데이터 수신 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void recvUdpKey_ReceivedData(object sender, NCasUdpRecvEventArgs e)
        {
            byte[] tmpBuff = new byte[e.Len];
            System.Buffer.BlockCopy(e.Buff, 0, tmpBuff, 0, e.Len);

            if (tmpBuff[5] != 13)
                return;

            NCasProtocolBase protoBase = NCasProtocolFactory.ParseFrame(tmpBuff);
            NCasProtocolTc148Sub13 protoSub13 = protoBase as NCasProtocolTc148Sub13;
            OrderView19201080 orderView = (OrderView19201080)this.dicViews[ViewKind.OrderView19201080];

            MethodInvoker invoker = delegate()
            {
                orderView.SetKeyPlc(protoSub13);
            };

            if (this.InvokeRequired)
            {
                this.Invoke(invoker);
            }
            else
            {
                invoker();
            }
        }

        /// <summary>
        /// CasMon UDP 데이터 수신 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void udpCasMon_ReceivedData(object sender, NCasUdpRecvEventArgs e)
        {
        }
        #endregion

        #region public method
        /// <summary>
        /// 그룹정보 리스트 클리어 메소드
        /// </summary>
        public void SetGroupListClear()
        {
            OrderView19201080 orderView = (OrderView19201080)this.dicViews[ViewKind.OrderView19201080];
            orderView.GroupNameLst.Clear();
        }

        /// <summary>
        /// 조작반 데이터를 pDAlmScreenBiz로 넘겨주는 메소드
        /// </summary>
        /// <param name="plcProtocol"></param>
        public void SetPlcKeyData(NCasProtocolBase plcProtocol)
        {
            this.pDAlmScreenBiz.AddBizData(plcProtocol);
        }

        /// <summary>
        /// 발령 데이터 처리 메소드
        /// </summary>
        /// <param name="orderBizData"></param>
        public void SetOrderBizData(OrderBizData orderBizData)
        {
            this.pDAlmScreenBiz.AddBizData(orderBizData);
        }

        /// <summary>
        /// 전광판 데이터 처리 메소드
        /// </summary>
        /// <param name="ledBizData"></param>
        public void SetLedBizData(LedBizData ledBizData)
        {
            this.pDAlmScreenBiz.AddBizData(ledBizData);
        }

        /// <summary>
        /// 자동방송 데이터 처리 메소드
        /// </summary>
        /// <param name="autoOrderBizData"></param>
        public void SetAutoOrderBizData(AutoOrderBizData autoOrderBizData)
        {
            this.pDAlmScreenBiz.AddBizData(autoOrderBizData);
        }

        /// <summary>
        /// Timer 멤버로 등록하는 메소드
        /// </summary>
        /// <param name="viewBase"></param>
        public void AddTimerMember(ViewBase viewBase)
        {
            if (!this.lstTimerMembers.Contains(viewBase))
            {
                this.lstTimerMembers.Add(viewBase);
            }
        }

        /// <summary>
        /// Timer 멤버에서 제거하는 메소드
        /// </summary>
        /// <param name="viewBase"></param>
        public void RemoveTimerMember(ViewBase viewBase)
        {
            if (this.lstTimerMembers.Contains(viewBase))
            {
                this.lstTimerMembers.Remove(viewBase);
            }
        }
        #endregion

        #region private method
        /// <summary>
        /// 특보 로그메시지를 가져온다
        /// </summary>
        /// <param name="weatherProto"></param>
        /// <returns></returns>
        private string GetWeatherLogMsg(NCasTwcProtocolCmd1 weatherProto)
        {
            string tmpLog = string.Format("!fH2발효시각 {0}/{1} {2}:{3}[", weatherProto.StartTimeByDateTime.Month.ToString().PadLeft(2, '0'),
                weatherProto.StartTimeByDateTime.Day.ToString().PadLeft(2, '0'), weatherProto.StartTimeByDateTime.Hour.ToString().PadLeft(2, '0'),
                weatherProto.StartTimeByDateTime.Minute.ToString().PadLeft(2, '0'));
            for (int i = 0; i < weatherProto.LstAreaName.Count; i++)
            {
                tmpLog += weatherProto.LstAreaName[i];

                if (i != weatherProto.LstAreaName.Count - 1)
                {
                    tmpLog += "/";
                }
                else
                {
                    tmpLog += "] ";
                }
            }

            if (weatherProto.SpcKind == NCasDefineSpcKind.StrongWind)
                tmpLog += "강풍";
            else if (weatherProto.SpcKind == NCasDefineSpcKind.HeavyRain)
                tmpLog += "호우";
            else if (weatherProto.SpcKind == NCasDefineSpcKind.ColdWave)
                tmpLog += "한파";
            else if (weatherProto.SpcKind == NCasDefineSpcKind.Dry)
                tmpLog += "건조";
            else if (weatherProto.SpcKind == NCasDefineSpcKind.Tsunami)
                tmpLog += "해일";
            else if (weatherProto.SpcKind == NCasDefineSpcKind.HighSeas)
                tmpLog += "풍랑";
            else if (weatherProto.SpcKind == NCasDefineSpcKind.Typhoon)
                tmpLog += "태풍";
            else if (weatherProto.SpcKind == NCasDefineSpcKind.HeavySnow)
                tmpLog += "대설";
            else if (weatherProto.SpcKind == NCasDefineSpcKind.YellowDust)
                tmpLog += "황사";

            if (weatherProto.SpcStress == NCasDefineSpcStress.Watch)
                tmpLog += " 주의보";
            else if (weatherProto.SpcStress == NCasDefineSpcStress.Warning)
                tmpLog += " 경보";

            return tmpLog;
        }

        public WeatherKindData GetWeatherKindData(NCasTwcProtocolCmd1 cmd)
        {
            WeatherKindData rst = new WeatherKindData();

            switch (cmd.SpcKind)
            {
                case NCasDefineSpcKind.StrongWind:
                    if (cmd.SpcStress == NCasDefineSpcStress.Watch)
                    {
                        rst.EWeatherKind = WeatherKindData.WeatherKind.galeWatch;
                    }
                    else if (cmd.SpcStress == NCasDefineSpcStress.Warning)
                    {
                        rst.EWeatherKind = WeatherKindData.WeatherKind.galeAlarm;
                    }

                    break;

                case NCasDefineSpcKind.HeavyRain:
                    if (cmd.SpcStress == NCasDefineSpcStress.Watch)
                    {
                        rst.EWeatherKind = WeatherKindData.WeatherKind.downpourWatch;
                    }
                    else if (cmd.SpcStress == NCasDefineSpcStress.Warning)
                    {
                        rst.EWeatherKind = WeatherKindData.WeatherKind.downpourAlarm;
                    }

                    break;

                case NCasDefineSpcKind.ColdWave:
                    if (cmd.SpcStress == NCasDefineSpcStress.Watch)
                    {
                        rst.EWeatherKind = WeatherKindData.WeatherKind.coldwaveWatch;
                    }
                    else if (cmd.SpcStress == NCasDefineSpcStress.Warning)
                    {
                        rst.EWeatherKind = WeatherKindData.WeatherKind.coldwaveAlarm;
                    }

                    break;

                case NCasDefineSpcKind.Dry:
                    if (cmd.SpcStress == NCasDefineSpcStress.Watch)
                    {
                        rst.EWeatherKind = WeatherKindData.WeatherKind.dryWatch;
                    }
                    else if (cmd.SpcStress == NCasDefineSpcStress.Warning)
                    {
                        rst.EWeatherKind = WeatherKindData.WeatherKind.dryAlarm;
                    }

                    break;

                case NCasDefineSpcKind.Tsunami:
                    if (cmd.SpcStress == NCasDefineSpcStress.Watch)
                    {
                        rst.EWeatherKind = WeatherKindData.WeatherKind.tsunamiWatch;
                    }
                    else if (cmd.SpcStress == NCasDefineSpcStress.Warning)
                    {
                        rst.EWeatherKind = WeatherKindData.WeatherKind.tsunamiAlarm;
                    }

                    break;

                case NCasDefineSpcKind.HighSeas:
                    if (cmd.SpcStress == NCasDefineSpcStress.Watch)
                    {
                        rst.EWeatherKind = WeatherKindData.WeatherKind.heavyseasWatch;
                    }
                    else if (cmd.SpcStress == NCasDefineSpcStress.Warning)
                    {
                        rst.EWeatherKind = WeatherKindData.WeatherKind.heavyseasAlarm;
                    }

                    break;

                case NCasDefineSpcKind.Typhoon:
                    if (cmd.SpcStress == NCasDefineSpcStress.Watch)
                    {
                        rst.EWeatherKind = WeatherKindData.WeatherKind.stormWatch;
                    }
                    else if (cmd.SpcStress == NCasDefineSpcStress.Warning)
                    {
                        rst.EWeatherKind = WeatherKindData.WeatherKind.stormAlarm;
                    }

                    break;

                case NCasDefineSpcKind.HeavySnow:
                    if (cmd.SpcStress == NCasDefineSpcStress.Watch)
                    {
                        rst.EWeatherKind = WeatherKindData.WeatherKind.heavysnowWatch;
                    }
                    else if (cmd.SpcStress == NCasDefineSpcStress.Warning)
                    {
                        rst.EWeatherKind = WeatherKindData.WeatherKind.heavysnowAlarm;
                    }

                    break;

                case NCasDefineSpcKind.YellowDust:
                    if (cmd.SpcStress == NCasDefineSpcStress.Watch)
                    {
                        rst.EWeatherKind = WeatherKindData.WeatherKind.sandstormWatch;
                    }
                    else if (cmd.SpcStress == NCasDefineSpcStress.Warning)
                    {
                        rst.EWeatherKind = WeatherKindData.WeatherKind.sandstormAlarm;
                    }

                    break;
            }

            foreach (WeatherKindData eachKind in WeatherOptionMng.LstWeatherOptionData[0].WeatherKindMsg)
            {
                if (eachKind.EWeatherKind == rst.EWeatherKind)
                {
                    rst = eachKind;
                    break;
                }
            }

            return rst;
        }

        /// <summary>
        /// 특보데이터 처리 메소드
        /// </summary>
        /// <param name="weatherProto"></param>
        private void AutoProcWeather(NCasTwcProtocolCmd1 weatherProto)
        {
            NCasLoggingMng.ILogging.WriteLog("특보 데이터 처리", this.GetWeatherLogMsg(weatherProto));

            foreach (string eachArea in weatherProto.LstAreaCode)
            {
                NCasLoggingMng.ILogging.WriteLog("특보 데이터 처리", "아래 정보가 없다면 해당 지역이 특보에 포함되어 있지 않아서 처리되지 않음");

                foreach (string eachSaveArea in WeatherOptionMng.LstWeatherOptionData[0].LstAreaCode)
                {
                    if (eachArea != eachSaveArea)
                        continue;

                    if (WeatherOptionMng.LstWeatherOptionData[0].UseTime &&
                    (WeatherOptionMng.LstWeatherOptionData[0].FirstTime > DateTime.Now.Hour ||
                    WeatherOptionMng.LstWeatherOptionData[0].SecondTime < DateTime.Now.Hour))
                    {
                        NCasLoggingMng.ILogging.WriteLog("특보 데이터 처리", string.Format("설정된 운영시간 - '{0}'시 ~ '{1}'시 보다 수집된 운영시간 - '{2}'시가 포함되지 않아 처리하지 않음",
                            WeatherOptionMng.LstWeatherOptionData[0].FirstTime.ToString(), WeatherOptionMng.LstWeatherOptionData[0].SecondTime.ToString(), weatherProto.StartTimeByDateTime.Hour));
                        return;
                    }

                    this.AutoProcWeatherLedSend(weatherProto, eachArea);

                    if (DateTime.Now > weatherProto.StartTimeByDateTime)
                    {
                        NCasLoggingMng.ILogging.WriteLog("특보 데이터 처리", string.Format("현재시각 - {0} 보다 발효시각 - {1}이 과거이기 때문에 처리하지 않음", DateTime.Now, weatherProto.StartTimeByDateTime));
                        return;
                    }

                    WeatherKindData kindData = this.GetWeatherKindData(weatherProto);

                    if (kindData.LastTime == weatherProto.StartTimeByDateTime)
                    {
                        NCasLoggingMng.ILogging.WriteLog("특보 데이터 처리", "발령을 하거나 반려한 적이 있는 특보이므로 처리하지 않음");
                        return;
                    }

                    foreach (WeatherKindData each in WeatherOptionMng.LstWeatherOptionData[0].WeatherKindMsg)
                    {
                        if (each.EWeatherKind == kindData.EWeatherKind)
                        {
                            each.LastTime = weatherProto.StartTimeByDateTime;
                            WeatherOptionMng.SaveWeatherOptionDatas();
                            break;
                        }
                    }

                    MethodInvoker invoker = delegate()
                    {
                        AutoAlarmForm autoForm = new AutoAlarmForm();
                        autoForm.SetAutoAlarmFormWeather(weatherProto, WeatherOptionMng.LstWeatherOptionData[0].UseAuto,
                            WeatherOptionMng.LstWeatherOptionData[0].AutoTime, eachArea, this);
                        autoForm.Show(this);
                        return;
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
            }
        }

        /// <summary>
        /// 조위데이터 처리 메소드
        /// </summary>
        /// <param name="heightProto"></param>
        private void AutoProcHeight(NCasTwcProtocolCmd2 heightProto)
        {
            NCasLoggingMng.ILogging.WriteLog("조위 데이터 처리", string.Format("조위 데이터 => 지역 - {0}({1}), 관측시각 - {2}, 수위 - {3}(cm)"
                , heightProto.PostName, heightProto.PostId, heightProto.RecordTimeByDateTime.ToString(), heightProto.TideLevel.ToString()));

            foreach (HeightPointContent eachPoint in HeightPointContentMng.LstHeightPointContent)
            {
                if (eachPoint.Title != heightProto.PostName)
                {
                    NCasLoggingMng.ILogging.WriteLog("조위 데이터 처리", string.Format("설정된 관측소 정보 - '{0}' 와 수집된 관측소 정보 - '{1}' 가 일치하지 않아 처리하지 않음",
                        eachPoint.Title, heightProto.PostName));
                    continue;
                }

                if (HeightOptionMng.LstHeightOptionData[0].UseTime &&
                    (HeightOptionMng.LstHeightOptionData[0].FirstTime > heightProto.RecordTimeByDateTime.Hour ||
                    HeightOptionMng.LstHeightOptionData[0].SecondTime < heightProto.RecordTimeByDateTime.Hour))
                {
                    NCasLoggingMng.ILogging.WriteLog("조위 데이터 처리", string.Format("설정된 운영시간 - '{0}'시 ~ '{1}'시 보다 수집된 운영시간 - '{2}'시가 포함되지 않아 처리하지 않음",
                        HeightOptionMng.LstHeightOptionData[0].FirstTime.ToString(), HeightOptionMng.LstHeightOptionData[0].SecondTime.ToString(), heightProto.RecordTimeByDateTime.Hour));
                    continue;
                }

                //this.AutoProcHeightLedSend(heightProto); //전광판 표출방식 변경으로 주석처리

                if (HeightOptionMng.LstHeightOptionData[0].MaxValue > heightProto.TideLevel)
                {
                    NCasLoggingMng.ILogging.WriteLog("조위 데이터 처리", string.Format("설정된 임계치 정보 - '{0}' 보다 수집된 임계치 정보 - '{1}' 가 작아 처리하지 않음",
                        HeightOptionMng.LstHeightOptionData[0].MaxValue, heightProto.TideLevel));
                    continue;
                }

                this.AutoProcHeightLedSend(heightProto);

                if (eachPoint.LastTime == heightProto.RecordTimeByDateTime)
                {
                    NCasLoggingMng.ILogging.WriteLog("조위 데이터 처리", string.Format("이미 처리한 시각의 조위 정보 - '{0}' 이므로 처리하지 않음",
                        eachPoint.LastTime));
                    continue;
                }

                eachPoint.LastTime = heightProto.RecordTimeByDateTime;
                HeightPointContentMng.SaveHeightPointContent();

                MethodInvoker invoker = delegate()
                {
                    try
                    {
                        AutoAlarmForm autoForm = new AutoAlarmForm();
                        autoForm.SetAutoAlarmFormHeight(heightProto, HeightOptionMng.LstHeightOptionData[0].UseAuto,
                            HeightOptionMng.LstHeightOptionData[0].AutoTime, this);
                        autoForm.Show(this);
                    }
                    catch (Exception ex)
                    {
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
        }

        /// <summary>
        /// 조위 정보를 전광판으로 전송
        /// </summary>
        /// <param name="heightProto"></param>
        private void AutoProcHeightLedSend(NCasTwcProtocolCmd2 heightProto)
        {
            NCasWmaLedProtocolCmd1 proto1 = NCasWmaLedProtocolFactory.CreateWmaProtocol(NCasDefineWmaCommand.LedMsg) as NCasWmaLedProtocolCmd1;
            proto1.StartYear = (byte)(DateTime.Now.Year - 2000);
            proto1.StartMonth = (byte)DateTime.Now.Month;
            proto1.StartDay = (byte)DateTime.Now.Day;
            proto1.StartHour = (byte)DateTime.Now.Hour;
            proto1.StartMin = (byte)DateTime.Now.Minute;
            proto1.EndYear = (byte)(DateTime.Now.Year - 2000);
            proto1.EndMonth = (byte)DateTime.Now.Month;
            proto1.EndDay = (byte)DateTime.Now.Day;
            proto1.EndHour = (byte)DateTime.Now.Hour;
            proto1.EndMin = (byte)(DateTime.Now.Minute + 2);
            proto1.FinishNum = NCASBIZ.NCasDefine.NCasDefineWmaFinishEffect.LeftToClear;
            proto1.FinishSpeed = NCASBIZ.NCasDefine.NCasDefineWmaSpeedEffect.Fast4;
            
            string tmpStr = "관심";

            if (heightProto.TideLevel >= HeightOptionMng.LstHeightOptionData[0].MaxValue && heightProto.TideLevel < HeightOptionMng.LstHeightOptionData[0].MaxValue2)
            {
                tmpStr = "관심";
            }
            else if (heightProto.TideLevel >= HeightOptionMng.LstHeightOptionData[0].MaxValue2 && heightProto.TideLevel < HeightOptionMng.LstHeightOptionData[0].MaxValue3)
            {
                tmpStr = "주의";
            }
            else if (heightProto.TideLevel >= HeightOptionMng.LstHeightOptionData[0].MaxValue3 && heightProto.TideLevel < HeightOptionMng.LstHeightOptionData[0].MaxValue4)
            {
                tmpStr = "경계";
            }
            else if (heightProto.TideLevel >= HeightOptionMng.LstHeightOptionData[0].MaxValue4)
            {
                tmpStr = "위험";
            }

            proto1.Msg = string.Format("!fH2{0}/{1} {3}:{4} [{2}]  {6} 단계 발생! 관측조위 {5} cm", heightProto.RecordTimeByDateTime.Month.ToString().PadLeft(2, '0'),
                heightProto.RecordTimeByDateTime.Day.ToString().PadLeft(2, '0'), heightProto.PostName, heightProto.RecordTimeByDateTime.Hour.ToString().PadLeft(2, '0'),
                heightProto.RecordTimeByDateTime.Minute.ToString().PadLeft(2, '0'), heightProto.TideLevel.ToString(), tmpStr);
            proto1.PrintFinishWaitTimeInSec = 4;
            proto1.PrintNum = NCASBIZ.NCasDefine.NCasDefineWmaViewEffect.LeftScroll;
            proto1.PrintSpeed = NCASBIZ.NCasDefine.NCasDefineWmaSpeedEffect.Fast4;
            proto1.RoomNum = 1;
            proto1.Siren = NCASBIZ.NCasDefine.NCasDefineWmaSirenUse.UnUsed;
            byte[] ledBuff = NCasWmaLedProtocolFactory.MakeFrame(proto1);

            NCasProtocolTc80 proto80 = NCasProtocolFactory.CreateCasProtocol(NCasDefineTcCode.TcWmaLed) as NCasProtocolTc80;
            proto80.LedData = new byte[ledBuff.Length];

            foreach (HeightPointContent eachPoint in HeightPointContentMng.LstHeightPointContent)
            {
                if (eachPoint.Title == heightProto.PostName)
                {
                    proto80.TermIpByString = eachPoint.LstHeightPointData[0].IpAddr;
                    break;
                }
            }

            Array.Copy(ledBuff, proto80.LedData, ledBuff.Length);
            byte[] sendBuff = NCasProtocolFactory.MakeTcpFrame(proto80);

            LedBizData ledBizData = new LedBizData();
            ledBizData.ProtocolTc80 = proto80;
            ledBizData.SendBuff = sendBuff;
            this.SetLedBizData(ledBizData);
        }

        /// <summary>
        /// 특보 정보를 전광판으로 전송
        /// </summary>
        /// <param name="heightProto"></param>
        private void AutoProcWeatherLedSend(NCasTwcProtocolCmd1 weatherProto, string areaCode)
        {
            NCasWmaLedProtocolCmd1 proto1 = NCasWmaLedProtocolFactory.CreateWmaProtocol(NCasDefineWmaCommand.LedMsg) as NCasWmaLedProtocolCmd1;
            proto1.StartYear = (byte)(DateTime.Now.Year - 2000);
            proto1.StartMonth = (byte)DateTime.Now.Month;
            proto1.StartDay = (byte)DateTime.Now.Day;
            proto1.StartHour = (byte)DateTime.Now.Hour;
            proto1.StartMin = (byte)DateTime.Now.Minute;
            proto1.EndYear = (byte)(DateTime.Now.Year - 2000);
            proto1.EndMonth = (byte)DateTime.Now.Month;
            proto1.EndDay = (byte)DateTime.Now.Day;
            proto1.EndHour = (byte)DateTime.Now.Hour;
            proto1.EndMin = (byte)(DateTime.Now.Minute + 2);
            proto1.FinishNum = NCASBIZ.NCasDefine.NCasDefineWmaFinishEffect.LeftToClear;
            proto1.FinishSpeed = NCASBIZ.NCasDefine.NCasDefineWmaSpeedEffect.Fast4;
            proto1.Msg = this.GetWeatherLogMsg(weatherProto);
            proto1.PrintFinishWaitTimeInSec = 4;
            proto1.PrintNum = NCASBIZ.NCasDefine.NCasDefineWmaViewEffect.LeftScroll;
            proto1.PrintSpeed = NCASBIZ.NCasDefine.NCasDefineWmaSpeedEffect.Fast4;
            proto1.RoomNum = 1;
            proto1.Siren = NCASBIZ.NCasDefine.NCasDefineWmaSirenUse.UnUsed;
            byte[] ledBuff = NCasWmaLedProtocolFactory.MakeFrame(proto1);

            NCasProtocolTc80 proto80 = NCasProtocolFactory.CreateCasProtocol(NCasDefineTcCode.TcWmaLed) as NCasProtocolTc80;
            proto80.LedData = new byte[ledBuff.Length];

            if (areaCode == "L1000000") //전국
            {
            }
            else if (areaCode == "L1010000") //경기도
            {
            }
            else if (areaCode == "L1012500") //평택시
            {
            }

            proto80.TermIpByString = "10.24.8.129"; //areaCode에 따라 단말IP 담아서 보내면 됨. 일단 단말 1개니까 임시로 테스트..
            Array.Copy(ledBuff, proto80.LedData, ledBuff.Length);
            byte[] sendBuff = NCasProtocolFactory.MakeTcpFrame(proto80);

            LedBizData ledBizData = new LedBizData();
            ledBizData.ProtocolTc80 = proto80;
            ledBizData.SendBuff = sendBuff;
            this.SetLedBizData(ledBizData);
        }

        /// <summary>
        /// 경보발령에 대한 응답 카운트를 화면에 표시한다.
        /// </summary>
        private void SetOrderResponseCount()
        {
            uint respCnt = 0;
            uint errorCnt = 0;

            foreach (TermInfo eachTerm in this.provInfo.LstTerms)
            {
                if (eachTerm.UseFlag != NCasDefineUseStatus.Use)
                    continue;

                if (eachTerm.TermFlag != NCasDefineTermKind.TermMutil)
                    continue;

                if (eachTerm.AlarmResponseInfo.AlarmResponse == NCasDefineResponse.None)
                {
                    errorCnt++;
                }
                else
                {
                    respCnt++;
                }
            }

            this.labelResponseTermCount.Text = respCnt.ToString();
            this.labelErrorTermCount.Text = errorCnt.ToString();
        }

        /// <summary>
        /// 경보발령에 대한 정보를 화면 상단에 표시한다. (발령 현황)
        /// </summary>
        private void SetOrderText()
        {
            this.orderTextBoard.ClearTextBlock();
            this.orderTextBoard.FontSize = this.OrderTextSize;

            if (this.provInfo.AlarmOrderInfo.Kind != NCasDefineOrderKind.None)
            {
                this.orderTextBoard.AddTextBlock(new NCasTextBoard.NCasTextBoardBlock(
                    NCasUtilityMng.INCasCommUtility.MakeDateTimeFormatForCasSystem3(this.provInfo.AlarmOrderInfo.OccurTimeToDateTime) +
                    "     [" + NCasUtilityMng.INCasCommUtility.NCasDefineOrderSource2String(this.provInfo.AlarmOrderInfo.Source) + "] [" +
                    NCasUtilityMng.INCasCommUtility.NCasDefineOrderMedia2String(this.provInfo.AlarmOrderInfo.Media) + "]", Color.FromArgb(255, 255, 255)));

                this.orderTextBoard.AddTextBlock(new NCasTextBoard.NCasTextBoardBlock(
                    " [" + NCasUtilityMng.INCasCommUtility.NCasDefineOrderMode2String(this.provInfo.AlarmOrderInfo.Mode) + "]",
                    (this.provInfo.AlarmOrderInfo.Mode == NCasDefineOrderMode.RealMode) ? Color.FromArgb(232, 82, 53) : Color.FromArgb(6, 147, 6)));

                this.orderTextBoard.AddTextBlock(new NCasTextBoard.NCasTextBoardBlock(
                    " [" + NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(this.provInfo.AlarmOrderInfo.Kind) + "]", Color.FromArgb(255, 255, 255)));
            }
        }

        /// <summary>
        /// 자동발령에 대한 응답 카운트를 화면에 표시한다.
        /// </summary>
        private void SetAutoOrderResponseCount()
        {
            uint respCnt = 0;
            uint errorCnt = 0;

            foreach (TermInfo eachTerm in this.provInfo.LstTerms)
            {
                if (eachTerm.UseFlag != NCasDefineUseStatus.Use)
                    continue;

                if (eachTerm.TermFlag != NCasDefineTermKind.TermMutil)
                    continue;

                if (eachTerm.WwsAlarmResponseInfo.AlarmResponse == NCasDefineResponse.None)
                {
                    errorCnt++;
                }
                else
                {
                    respCnt++;
                }
            }

            this.labelResponseTermCount.Text = respCnt.ToString();
            this.labelErrorTermCount.Text = errorCnt.ToString();
        }

        /// <summary>
        /// 자동발령에 대한 정보를 화면 상단에 표시한다. (발령 현황)
        /// </summary>
        private void SetAutoOrderText()
        {
            this.orderTextBoard.ClearTextBlock();
            this.orderTextBoard.FontSize = this.OrderTextSize;

            if (this.provInfo.WwsAlarmOrderInfo.Kind != NCasDefineOrderKind.None)
            {
                this.orderTextBoard.AddTextBlock(new NCasTextBoard.NCasTextBoardBlock(
                    NCasUtilityMng.INCasCommUtility.MakeDateTimeFormatForCasSystem3(this.provInfo.WwsAlarmOrderInfo.OccurTimeToDateTime) +
                    "     [" + NCasUtilityMng.INCasCommUtility.NCasDefineOrderSource2String(this.provInfo.WwsAlarmOrderInfo.Source) + "] [" +
                    NCasUtilityMng.INCasCommUtility.NCasDefineOrderMedia2String(this.provInfo.WwsAlarmOrderInfo.Media) + "]", Color.FromArgb(255, 255, 255)));

                this.orderTextBoard.AddTextBlock(new NCasTextBoard.NCasTextBoardBlock(
                    " [" + NCasUtilityMng.INCasCommUtility.NCasDefineOrderMode2String(this.provInfo.WwsAlarmOrderInfo.Mode) + "]",
                    (this.provInfo.WwsAlarmOrderInfo.Mode == NCasDefineOrderMode.RealMode) ? Color.FromArgb(232, 82, 53) : Color.FromArgb(6, 147, 6)));

                this.orderTextBoard.AddTextBlock(new NCasTextBoard.NCasTextBoardBlock(
                    " [" + NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(this.provInfo.WwsAlarmOrderInfo.Kind) + "]", Color.FromArgb(255, 255, 255)));
            }
        }

        /// <summary>
        /// 타이머 초기화 및 실행 메소드
        /// </summary>
        /// <param name="interval"></param>
        private void StartTimer(int interval)
        {
            this.commonTimer = new Timer();
            this.commonTimer.Interval = interval;
            this.commonTimer.Tick += new EventHandler(commonTimer_Tick);
            this.commonTimer.Start();
        }

        /// <summary>
        /// 타이머 종료 메소드
        /// </summary>
        private void StopTimer()
        {
            if (this.commonTimer != null)
            {
                this.commonTimer.Tick -= new EventHandler(commonTimer_Tick);
                this.commonTimer.Stop();
            }
        }

        /// <summary>
        /// 전광판 타이머 초기화 및 실행 메소드
        /// </summary>
        /// <param name="interval"></param>
        private void StartBoardTimer(int interval)
        {
            this.boardTimer = new Timer();
            this.boardTimer.Interval = interval;
            this.boardTimer.Tick += new EventHandler(boardTimer_Tick);
            this.boardTimer.Start();
        }

        /// <summary>
        /// 전광판 타이머 종료 메소드
        /// </summary>
        private void StopBoardTimer()
        {
            if (this.boardTimer != null)
            {
                this.boardTimer.Tick -= new EventHandler(boardTimer_Tick);
                this.boardTimer.Stop();
            }
        }

        /// <summary>
        /// 전광판 타이머 Tick 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void boardTimer_Tick(object sender, EventArgs e)
        {
            DateTime nowDt = DateTime.Now;

            foreach (BoardTextData eachBoard in BoardTextMng.LstBoardTextData)
            {
                if (eachBoard.RepeatUseFlag == false)
                    continue;

                if (nowDt < eachBoard.LastSendTime.AddMinutes((double)eachBoard.RepeatMin))
                    continue;

                if (nowDt.Month < eachBoard.StartMonth || nowDt.Month > eachBoard.EndMonth)
                    continue;

                if (nowDt.DayOfWeek == DayOfWeek.Monday)
                {
                    if (eachBoard.Mon == false)
                        continue;
                }
                else if (nowDt.DayOfWeek == DayOfWeek.Tuesday)
                {
                    if (eachBoard.Tue == false)
                        continue;
                }
                else if (nowDt.DayOfWeek == DayOfWeek.Wednesday)
                {
                    if (eachBoard.Wed == false)
                        continue;
                }
                else if (nowDt.DayOfWeek == DayOfWeek.Thursday)
                {
                    if (eachBoard.Thu == false)
                        continue;
                }
                else if (nowDt.DayOfWeek == DayOfWeek.Friday)
                {
                    if (eachBoard.Fri == false)
                        continue;
                }
                else if (nowDt.DayOfWeek == DayOfWeek.Saturday)
                {
                    if (eachBoard.Sat == false)
                        continue;
                }
                else if (nowDt.DayOfWeek == DayOfWeek.Sunday)
                {
                    if (eachBoard.Sun == false)
                        continue;
                }

                if (nowDt.Hour < eachBoard.StartHour || nowDt.Hour > eachBoard.EndHour)
                    continue;

                if (nowDt.Hour == eachBoard.StartHour)
                {
                    if (nowDt.Minute < eachBoard.StartMin)
                        continue;
                }

                if (nowDt.Hour == eachBoard.EndHour)
                {
                    if (nowDt.Minute > eachBoard.EndMin)
                        continue;
                }

                eachBoard.LastSendTime = nowDt;
                BoardTextMng.SaveBoardTextDatas();

                foreach (string eachIp in eachBoard.LstTerm)
                {
                    NCasWmaLedProtocolCmd1 proto1 = NCasWmaLedProtocolFactory.CreateWmaProtocol(NCasDefineWmaCommand.LedMsg) as NCasWmaLedProtocolCmd1;
                    proto1.StartYear = (byte)(DateTime.Now.Year - 2000);
                    proto1.StartMonth = (byte)DateTime.Now.Month;
                    proto1.StartDay = (byte)DateTime.Now.Day;
                    proto1.StartHour = (byte)DateTime.Now.Hour;
                    proto1.StartMin = (byte)DateTime.Now.Minute;
                    proto1.EndYear = (byte)(DateTime.Now.Year - 2000);
                    proto1.EndMonth = (byte)DateTime.Now.Month;
                    proto1.EndDay = (byte)DateTime.Now.Day;
                    proto1.EndHour = (byte)DateTime.Now.Hour;
                    proto1.EndMin = (byte)(DateTime.Now.Minute + eachBoard.RepeatMin);
                    proto1.FinishNum = NCASBIZ.NCasDefine.NCasDefineWmaFinishEffect.LeftToClear;
                    proto1.FinishSpeed = NCASBIZ.NCasDefine.NCasDefineWmaSpeedEffect.Fast4;
                    proto1.Msg = eachBoard.Message;
                    proto1.PrintFinishWaitTimeInSec = 4;
                    proto1.PrintNum = NCASBIZ.NCasDefine.NCasDefineWmaViewEffect.LeftScroll;
                    proto1.PrintSpeed = NCASBIZ.NCasDefine.NCasDefineWmaSpeedEffect.Fast4;
                    proto1.RoomNum = 1;
                    proto1.Siren = NCASBIZ.NCasDefine.NCasDefineWmaSirenUse.UnUsed;
                    byte[] ledBuff = NCasWmaLedProtocolFactory.MakeFrame(proto1);

                    NCasProtocolTc80 proto80 = NCasProtocolFactory.CreateCasProtocol(NCasDefineTcCode.TcWmaLed) as NCasProtocolTc80;
                    proto80.LedData = new byte[ledBuff.Length];
                    proto80.TermIpByString = eachIp;
                    Array.Copy(ledBuff, proto80.LedData, ledBuff.Length);
                    byte[] sendBuff = NCasProtocolFactory.MakeTcpFrame(proto80);

                    LedBizData ledBizData = new LedBizData();
                    ledBizData.ProtocolTc80 = proto80;
                    ledBizData.SendBuff = sendBuff;
                    this.SetLedBizData(ledBizData);
                }
            }
        }

        /// <summary>
        /// 각 ViewKind와 툴바를 매핑하는 작업
        /// </summary>
        private void InitView()
        {
            //메인 화면
            ViewBase coverViewBase = CoverView.CreateView(ViewKind.None, this);
            coverViewBase.ViewKind = ViewKind.None;
            coverViewBase.Dock = System.Windows.Forms.DockStyle.Fill;
            coverViewBase.Location = new System.Drawing.Point(0, 0);
            coverViewBase.Name = "coverView";
            coverViewBase.Size = new System.Drawing.Size(1904, 937);
            this.middlePanel.Controls.Add(coverViewBase);
            this.dicViews.Add(ViewKind.None, coverViewBase);

            //경보발령 화면
            ViewBase orderViewBase = OrderView19201080.CreateView(ViewKind.OrderView19201080, this);
            orderViewBase.Interval = 1000;
            orderViewBase.ViewKind = ViewKind.OrderView19201080;
            orderViewBase.Dock = System.Windows.Forms.DockStyle.Fill;
            orderViewBase.Location = new System.Drawing.Point(0, 0);
            orderViewBase.Name = "orderView";
            orderViewBase.Size = new System.Drawing.Size(1904, 937);
            this.middlePanel.Controls.Add(orderViewBase);
            this.dicViews.Add(ViewKind.OrderView19201080, orderViewBase);

            //경보결과 화면
            ViewBase resultViewBase = OrderResultView.CreateView(ViewKind.ResultView, this);
            resultViewBase.Interval = 1000;
            resultViewBase.ViewKind = ViewKind.ResultView;
            resultViewBase.Dock = System.Windows.Forms.DockStyle.Fill;
            resultViewBase.Location = new System.Drawing.Point(0, 0);
            resultViewBase.Name = "resultView";
            resultViewBase.Size = new System.Drawing.Size(1904, 937);
            this.middlePanel.Controls.Add(resultViewBase);
            this.dicViews.Add(ViewKind.ResultView, resultViewBase);

            //장비감시 화면
            ViewBase deviceMonViewBase = DeviceMonitorView.CreateView(ViewKind.DevMonView, this);
            deviceMonViewBase.Interval = 1000;
            deviceMonViewBase.ViewKind = ViewKind.DevMonView;
            deviceMonViewBase.Dock = System.Windows.Forms.DockStyle.Fill;
            deviceMonViewBase.Location = new System.Drawing.Point(0, 0);
            deviceMonViewBase.Name = "deviceMonitorView";
            deviceMonViewBase.Size = new System.Drawing.Size(1904, 937);
            this.middlePanel.Controls.Add(deviceMonViewBase);
            this.dicViews.Add(ViewKind.DevMonView, deviceMonViewBase);

            this.btnOrderMenu.Tag = ViewKind.OrderView19201080;
            this.btnOrderResultMenu.Tag = ViewKind.ResultView;
            this.btnDevMonMenu.Tag = ViewKind.DevMonView;
        }

        /// <summary>
        /// 시도 로고이미지를 화면에 셋팅한다.
        /// </summary>
        /// <param name="proveCode">셋팅할 시도의 Code</param>
        private void InitLogoImage(int proveCode)
        {
            Image image = null;

            switch (proveCode)
            {
                case 1670:
                    image = WmaPAlmScreenRsc._1670;
                    break;

                case 1671:
                    image = WmaPAlmScreenRsc._1671;
                    break;

                case 1672:
                    image = WmaPAlmScreenRsc._1672;
                    break;

                case 1673:
                    image = WmaPAlmScreenRsc._1673;
                    break;

                case 1674:
                    image = WmaPAlmScreenRsc._1674;
                    break;

                case 1675:
                    image = WmaPAlmScreenRsc._1675;
                    break;

                case 1676:
                    image = WmaPAlmScreenRsc._1676;
                    break;

                case 1677:
                    image = WmaPAlmScreenRsc._1677;
                    break;

                case 1678:
                    image = WmaPAlmScreenRsc._1678;
                    break;

                case 1679:
                    image = WmaPAlmScreenRsc._1679;
                    break;

                case 1680:
                    image = WmaPAlmScreenRsc._1680;
                    break;

                case 1681:
                    image = WmaPAlmScreenRsc._1681;
                    break;

                case 1682:
                    image = WmaPAlmScreenRsc._1682;
                    break;

                case 1683:
                    image = WmaPAlmScreenRsc._1683;
                    break;

                case 1684:
                    image = WmaPAlmScreenRsc._1684;
                    break;

                case 1685:
                    image = WmaPAlmScreenRsc._1685;
                    break;

                case 2481:
                    image = WmaPAlmScreenRsc._1686;
                    break;

                default:
                    image = WmaPAlmScreenRsc._1670;
                    break;
            }

            this.topRightLogoPictureBox.BackgroundImage = image;
        }

        /// <summary>
        /// 선택된 ViewKind를 화면에 표시
        /// </summary>
        /// <param name="viewKind"></param>
        private void OpenView(ViewKind viewKind)
        {
            switch (viewKind)
            {
                case ViewKind.None:
                    this.dicViews[ViewKind.None].BringToFront();
                    break;

                case ViewKind.OrderView19201080:
                    this.dicViews[ViewKind.OrderView19201080].BringToFront();
                    this.btnOrderMenu.CheckedValue = true;
                    break;

                case ViewKind.ResultView:
                    this.dicViews[ViewKind.ResultView].BringToFront();
                    this.btnOrderResultMenu.CheckedValue = true;
                    break;

                case ViewKind.DevMonView:
                    this.dicViews[ViewKind.DevMonView].BringToFront();
                    this.btnDevMonMenu.CheckedValue = true;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// ViewKind 화면을 숨긴다
        /// </summary>
        /// <param name="viewKind"></param>
        private void CloseView(ViewKind viewKind)
        {
            switch (viewKind)
            {
                case ViewKind.None:
                    this.dicViews[ViewKind.None].SendToBack();
                    break;

                case ViewKind.OrderView19201080:
                    this.dicViews[ViewKind.OrderView19201080].SendToBack();
                    this.btnOrderMenu.CheckedValue = false;
                    break;

                case ViewKind.ResultView:
                    this.dicViews[ViewKind.ResultView].SendToBack();
                    this.btnOrderResultMenu.CheckedValue = false;
                    break;

                case ViewKind.DevMonView:
                    this.dicViews[ViewKind.DevMonView].SendToBack();
                    this.btnDevMonMenu.CheckedValue = false;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 모든 ViewKind 화면을 숨긴다
        /// </summary>
        private void HideAllViewForm()
        {
            foreach (KeyValuePair<ViewKind, ViewBase> eachDic in this.dicViews)
            {
                eachDic.Value.SendToBack();
            }

            this.btnOrderMenu.CheckedValue = false;
            this.btnOrderResultMenu.CheckedValue = false;
            this.btnDevMonMenu.CheckedValue = false;
        }

        /// <summary>
        /// 서버 로컬IP를 받아 해당하는 MMF파일을 로드한다.
        /// </summary>
        /// <param name="localIpAddr">서버 로컬IP</param>
        private void InitMmfInfo(string localIpAddr)
        {
            this.mmfMng = new NCasMMFMng();
            this.mmfMng.LoadAllMMF();

#if release
            this.provInfo = this.mmfMng.GetProvInfoByNetId(localIpAddr);
#endif

#if debug
            this.provInfo = this.mmfMng.GetProvInfoByNetId("10.24.1.231");
#endif

            if (this.provInfo == null)
            {
                MessageBox.Show("데이터파일을 정상적으로 로드하지 못했습니다.", "데이터파일 로드", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw new Exception("MainForm.InitMmfInfo(string localIpAddr) Method Error!");
            }
        }
        #endregion
    }
}