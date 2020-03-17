using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NCASBIZ.NCasDefine;
using NCASBIZ.NCasProtocol;
using NCASBIZ.NCasTwcProtocol;
using NCASFND.NCasLogging;

namespace WmaPAlmScreen
{
    public partial class AutoAlarmForm : Form
    {
        private Timer timer = null;
        private NCasTwcProtocolCmd1 cmd1 = null;
        private NCasTwcProtocolCmd2 cmd2 = null;
        private bool autoFlag = false;
        private int waitTime = 0;
        private string areaCode = string.Empty;
        private MainForm mainForm = null;
        private bool isWeather = false;

        public AutoAlarmForm()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this.timer.Tick -= new EventHandler(timer_Tick);
        }

        /// <summary>
        /// 특보 데이터 처리
        /// </summary>
        /// <param name="baseCmd"></param>
        /// <param name="autoFlag"></param>
        /// <param name="waitTime"></param>
        /// <param name="areaCode"></param>
        /// <param name="mainForm"></param>
        public void SetAutoAlarmFormWeather(NCasTwcProtocolCmd1 cmd1, bool autoFlag, int waitTime, string areaCode, MainForm mainForm)
        {
            this.cmd1 = cmd1;
            this.autoFlag = autoFlag;
            this.waitTime = waitTime * 60;
            this.areaCode = areaCode;
            this.mainForm = mainForm;
            this.isWeather = true;
            this.commonLabel1.Text = this.GetWeatherKindName(cmd1);
            this.commonLabel2.Text = cmd1.StartTimeByDateTime.ToString();
            this.commonLabel3.Text = this.GetWeatherAreaName(cmd1);
            this.commonLabel4.Text = this.GetWeatherTermName(areaCode);
            this.commonLabel5.Text = string.Format("{0} ({1})", this.mainForm.GetWeatherKindData(cmd1).StoMsg.Title,
                this.mainForm.GetWeatherKindData(cmd1).StoMsg.MsgNum);

            if (autoFlag)
            {
                this.oklBtn_Click(this, new EventArgs());
            }
            else
            {
                this.timer = new Timer();
                this.timer.Interval = 1000;
                this.timer.Tick += new EventHandler(timer_Tick);
                this.timer.Start();
            }
        }

        void timer_Tick(object sender, EventArgs e)
        {
            this.discountTimeLabel.Text = string.Format("{0}:{1}", (this.waitTime / 60).ToString(), (this.waitTime % 60).ToString().PadLeft(2, '0'));
            this.waitTime -= 1;

            if (waitTime < 0)
            {
                NCasLoggingMng.ILogging.WriteLog("의사결정", "디스카운트 시간 0이 될때까지 선택이 없었으므로 반려");
                this.Close();
            }
        }

        /// <summary>
        /// 조위 데이터 처리
        /// </summary>
        /// <param name="baseCmd"></param>
        /// <param name="autoFlag"></param>
        /// <param name="waitTime"></param>
        /// <param name="mainForm"></param>
        public void SetAutoAlarmFormHeight(NCasTwcProtocolCmd2 cmd2, bool autoFlag, int waitTime, MainForm mainForm)
        {
            this.cmd2 = cmd2;
            this.autoFlag = autoFlag;
            this.waitTime = waitTime * 60;
            this.mainForm = mainForm;
            this.isWeather = false;
            this.titleLabel1.Text = "관측소";
            this.titleLabel2.Text = "관측시각";
            this.titleLabel3.Text = "수위(cm)";
            this.commonLabel1.Text = cmd2.PostName;
            this.commonLabel2.Text = cmd2.RecordTimeByDateTime.ToString();
            this.commonLabel3.Text = this.cmd2.TideLevel.ToString();
            this.commonLabel4.Text = this.GetHeightTermName(cmd2);

            if (cmd2.TideLevel >= HeightOptionMng.LstHeightOptionData[0].MaxValue && cmd2.TideLevel < HeightOptionMng.LstHeightOptionData[0].MaxValue2)
            {
                this.commonLabel5.Text = string.Format("{0} ({1})", HeightOptionMng.LstHeightOptionData[0].Msg.Title,
                    HeightOptionMng.LstHeightOptionData[0].Msg.MsgNum);
            }
            else if (cmd2.TideLevel >= HeightOptionMng.LstHeightOptionData[0].MaxValue2 && cmd2.TideLevel < HeightOptionMng.LstHeightOptionData[0].MaxValue3)
            {
                this.commonLabel5.Text = string.Format("{0} ({1})", HeightOptionMng.LstHeightOptionData[0].Msg2.Title,
                    HeightOptionMng.LstHeightOptionData[0].Msg2.MsgNum);
            }
            else if (cmd2.TideLevel >= HeightOptionMng.LstHeightOptionData[0].MaxValue3 && cmd2.TideLevel < HeightOptionMng.LstHeightOptionData[0].MaxValue4)
            {
                this.commonLabel5.Text = string.Format("{0} ({1})", HeightOptionMng.LstHeightOptionData[0].Msg3.Title,
                    HeightOptionMng.LstHeightOptionData[0].Msg3.MsgNum);
            }
            else if (cmd2.TideLevel >= HeightOptionMng.LstHeightOptionData[0].MaxValue4)
            {
                this.commonLabel5.Text = string.Format("{0} ({1})", HeightOptionMng.LstHeightOptionData[0].Msg4.Title,
                    HeightOptionMng.LstHeightOptionData[0].Msg4.MsgNum);
            }

            //if (HeightOptionMng.LstHeightOptionData[0].Msg.MsgNum == string.Empty ||
            //    HeightOptionMng.LstHeightOptionData[0].Msg2.MsgNum == string.Empty ||
            //    HeightOptionMng.LstHeightOptionData[0].Msg3.MsgNum == string.Empty ||
            //    HeightOptionMng.LstHeightOptionData[0].Msg4.MsgNum == string.Empty)
            //{
            //    MessageBox.Show("조위 단계 별 저장메시지가 설정되어 있지 않아 발령을 종료합니다.", "조위 방송", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    NCasLoggingMng.ILogging.WriteLog("조위 데이터 처리 - 의사결정 전 저장메시지 미설정으로 방송 직전 종료함", "자동방송 저장메시지 방송 종료함");
            //    this.Close();
            //}

            if (autoFlag)
            {
                this.oklBtn_Click(this, new EventArgs());
            }
            else
            {
                this.timer = new Timer();
                this.timer.Interval = 1000;
                this.timer.Tick += new EventHandler(timer_Tick);
                this.timer.Start();
            }
        }

        private string GetHeightTermName(NCasTwcProtocolCmd2 heightProto)
        {
            string rst = string.Empty;

            foreach (HeightPointContent each in HeightPointContentMng.LstHeightPointContent)
            {
                if (each.Title == heightProto.PostName)
                {
                    for (int i = 0; i < each.LstHeightPointData.Count; i++)
                    {
                        rst += each.LstHeightPointData[i].Title;

                        if (i != each.LstHeightPointData.Count - 1)
                        {
                            rst += "/";
                        }
                    }
                }
            }

            return rst;
        }

        private string GetWeatherKindName(NCasTwcProtocolCmd1 weatherProto)
        {
            string tmpLog = string.Empty;

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

        private string GetWeatherAreaName(NCasTwcProtocolCmd1 weatherProto)
        {
            string tmpLog = string.Empty;

            for (int i = 0; i < weatherProto.LstAreaName.Count; i++)
            {
                tmpLog += weatherProto.LstAreaName[i];

                if (i != weatherProto.LstAreaName.Count - 1)
                {
                    tmpLog += "/";
                }
            }

            return tmpLog;
        }

        private string GetWeatherTermName(string areaCode)
        {
            string rst = string.Empty;

            switch (areaCode)
            {
                case "L1000000": //전국
                    break;

                case "L1010000": //경기도
                    break;

                case "L1012500": //평택시
                    break;
            }

            //위에서 찾아서 처리하면 됨. 임시 테스트
            rst = this.mainForm.MmfMng.GetTermInfoByIp("10.24.8.129").Name;
            return rst;
        }

        /// <summary>
        /// 발령 버튼 (즉시 발령)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void oklBtn_Click(object sender, EventArgs e)
        {
            NCasProtocolTc171 proto171 = NCasProtocolFactory.CreateCasProtocol(NCasDefineTcCode.TcAutoAlarmOrder) as NCasProtocolTc171;
            proto171.AlarmNetIdOrIpByString = "10.24.8.129"; //임시
            proto171.Media = NCasDefineOrderMedia.MediaLine;
            proto171.CtrlKind = NCasDefineControlKind.ControlAlarm;
            proto171.OrderTimeByDateTime = DateTime.Now;
            proto171.Sector = NCasDefineSectionCode.SectionTerm;
            proto171.Source = NCasDefineOrderSource.ProvCtrlRoom;
            proto171.RespReqFlag = NCasDefineRespReq.ResponseReq;
            proto171.AuthenFlag = NCasDefineAuthenticationFlag.EncodeUsed;
            proto171.AlarmKind = NCasDefineOrderKind.WmaAutoAlarm;

            if (isWeather) //특보
            {
                proto171.Mode = (WeatherOptionMng.LstWeatherOptionData[0].TestOrder == true) ? NCasDefineOrderMode.TestMode : NCasDefineOrderMode.RealMode;
                proto171.MsgNum1 = this.GetStoredMsgHeaderNumber(int.Parse(this.mainForm.GetWeatherKindData(cmd1).StoMsg.MsgNum));
                proto171.MsgNum2 = int.Parse(this.mainForm.GetWeatherKindData(cmd1).StoMsg.MsgNum);
                proto171.MsgNum3 = this.GetStoredMsgTailNumber(int.Parse(this.mainForm.GetWeatherKindData(cmd1).StoMsg.MsgNum));
                proto171.RepeatNum = (byte)this.mainForm.GetWeatherKindData(cmd1).StoMsg.RepeatCount;
                NCasLoggingMng.ILogging.WriteLog("특보 데이터 처리 - 의사결정", string.Format("자동방송 저장메시지 [{0}] 방송 실행함", proto171.MsgNum2.ToString()));
            }
            else //조위
            {
                try
                {
                    proto171.Mode = (HeightOptionMng.LstHeightOptionData[0].TestOrder == true) ? NCasDefineOrderMode.TestMode : NCasDefineOrderMode.RealMode;

                    if (cmd2.TideLevel >= HeightOptionMng.LstHeightOptionData[0].MaxValue && cmd2.TideLevel < HeightOptionMng.LstHeightOptionData[0].MaxValue2)
                    {
                        proto171.MsgNum1 = this.GetStoredMsgHeaderNumber(int.Parse(HeightOptionMng.LstHeightOptionData[0].Msg.MsgNum));
                        proto171.MsgNum2 = int.Parse(HeightOptionMng.LstHeightOptionData[0].Msg.MsgNum);
                        proto171.MsgNum3 = this.GetStoredMsgTailNumber(int.Parse(HeightOptionMng.LstHeightOptionData[0].Msg.MsgNum));
                        proto171.RepeatNum = (byte)HeightOptionMng.LstHeightOptionData[0].Msg.RepeatCount;
                    }
                    else if (cmd2.TideLevel >= HeightOptionMng.LstHeightOptionData[0].MaxValue2 && cmd2.TideLevel < HeightOptionMng.LstHeightOptionData[0].MaxValue3)
                    {
                        proto171.MsgNum1 = this.GetStoredMsgHeaderNumber(int.Parse(HeightOptionMng.LstHeightOptionData[0].Msg2.MsgNum));
                        proto171.MsgNum2 = int.Parse(HeightOptionMng.LstHeightOptionData[0].Msg2.MsgNum);
                        proto171.MsgNum3 = this.GetStoredMsgTailNumber(int.Parse(HeightOptionMng.LstHeightOptionData[0].Msg2.MsgNum));
                        proto171.RepeatNum = (byte)HeightOptionMng.LstHeightOptionData[0].Msg2.RepeatCount;
                    }
                    else if (cmd2.TideLevel >= HeightOptionMng.LstHeightOptionData[0].MaxValue3 && cmd2.TideLevel < HeightOptionMng.LstHeightOptionData[0].MaxValue4)
                    {
                        proto171.MsgNum1 = this.GetStoredMsgHeaderNumber(int.Parse(HeightOptionMng.LstHeightOptionData[0].Msg3.MsgNum));
                        proto171.MsgNum2 = int.Parse(HeightOptionMng.LstHeightOptionData[0].Msg3.MsgNum);
                        proto171.MsgNum3 = this.GetStoredMsgTailNumber(int.Parse(HeightOptionMng.LstHeightOptionData[0].Msg3.MsgNum));
                        proto171.RepeatNum = (byte)HeightOptionMng.LstHeightOptionData[0].Msg3.RepeatCount;
                    }
                    else if (cmd2.TideLevel >= HeightOptionMng.LstHeightOptionData[0].MaxValue4)
                    {
                        proto171.MsgNum1 = this.GetStoredMsgHeaderNumber(int.Parse(HeightOptionMng.LstHeightOptionData[0].Msg4.MsgNum));
                        proto171.MsgNum2 = int.Parse(HeightOptionMng.LstHeightOptionData[0].Msg4.MsgNum);
                        proto171.MsgNum3 = this.GetStoredMsgTailNumber(int.Parse(HeightOptionMng.LstHeightOptionData[0].Msg4.MsgNum));
                        proto171.RepeatNum = (byte)HeightOptionMng.LstHeightOptionData[0].Msg4.RepeatCount;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("조위 단계 별 저장메시지가 설정되어 있지 않아 발령을 종료합니다.", "조위 방송", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    NCasLoggingMng.ILogging.WriteLog("조위 데이터 처리 - 의사결정 전 저장메시지 미설정으로 방송 직전 종료함", "자동방송 저장메시지 방송 종료함");
                    this.Close();
                }

                NCasLoggingMng.ILogging.WriteLog("조위 데이터 처리 - 의사결정", string.Format("자동방송 저장메시지 [{0}] 방송 실행함", proto171.MsgNum2.ToString()));
            }

            byte[] buff = NCasProtocolFactory.MakeUdpFrame(proto171);
            AutoOrderBizData autoOrderBizData = new AutoOrderBizData();
            autoOrderBizData.AlmProtocol = proto171;
            autoOrderBizData.SendBuff = buff;
            this.mainForm.SetAutoOrderBizData(autoOrderBizData);
            this.Close();
        }

        /// <summary>
        /// 취소 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            NCasLoggingMng.ILogging.WriteLog("의사결정", "사용자가 취소 버튼 클릭함");
            this.Close();
        }

        /// <summary>
        /// 방송할 저장메시지에 해당되는 Header 메시지 번호를 반환한다.
        /// </summary>
        /// <param name="storedMsgNumber">방송할 저장메시지 번호</param>
        /// <returns>방송할 저장메시지에 해당되는 Header 메시지 번호</returns>
        private int GetStoredMsgHeaderNumber(int storedMsgNumber)
        {
            int resultNum = 0;

            if ((storedMsgNumber % 2 == 0) && (storedMsgNumber > 201 && storedMsgNumber < 219))
            {
                resultNum = 954;
            }
            else if (storedMsgNumber == 157 || storedMsgNumber == 158)
            {
                resultNum = 954;
            }
            else
            {
                resultNum = 951;
            }

            return resultNum;
        }

        /// <summary>
        /// 방송할 저장메시지에 해당되는 Tail 메시지 번호를 반환한다.
        /// </summary>
        /// <param name="storedMsgNumber">방송할 저장메시지 번호</param>
        /// <returns>방송할 저장메시지에 해당되는 Tail 메시지 번호</returns>
        private int GetStoredMsgTailNumber(int storedMsgNumber)
        {
            int resultNum = 0;

            if (storedMsgNumber > 155 && storedMsgNumber < 170)
            {
                resultNum = 509;
            }
            else
            {
                resultNum = 502;
            }

            return resultNum;
        }
    }
}