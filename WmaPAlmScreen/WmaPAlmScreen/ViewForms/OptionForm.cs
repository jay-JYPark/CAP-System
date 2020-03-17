using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

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
    public partial class OptionForm : Form
    {
        private ProvInfo provInfo = null;
        private StoredMessageText heightSelectMsg = new StoredMessageText();
        private StoredMessageText heightSelectMsg2 = new StoredMessageText();
        private StoredMessageText heightSelectMsg3 = new StoredMessageText();
        private StoredMessageText heightSelectMsg4 = new StoredMessageText();

        public OptionForm(ProvInfo provInfo)
        {
            InitializeComponent();
            this.provInfo = provInfo;
            this.Init();
        }

        #region private method
        /// <summary>
        /// 설정정보 저장 메소드
        /// </summary>
        private void Save()
        {
            if (this.firstHeightTimeNumeric.Value > this.secondHeightTimeNumeric.Value)
            {
                MessageBox.Show("조위 운영시간 설정 시 시작시간이 더 느릴 수 없습니다.\n설정이 저장되지 않았습니다.", "시도 서해안 경보시스템", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if (this.firstWeatherTimeNumeric.Value > this.secondWeatherTimeNumeric.Value)
            {
                MessageBox.Show("특보 운영시간 설정 시 시작시간이 더 느릴 수 없습니다.\n설정이 저장되지 않았습니다.", "시도 서해안 경보시스템", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            if ((int)this.heightValueNumeric.Value >= (int)this.heightValueNumeric2.Value
                || (int)this.heightValueNumeric2.Value >= (int)this.heightValueNumeric3.Value
                || (int)this.heightValueNumeric3.Value >= (int)this.heightValueNumeric4.Value)
            {
                MessageBox.Show("조위 단계 별 임계치 설정 값은 상위 단계보다 같거나 높은 값으로\n설정할 수 없습니다. 설정이 저장되지 않았습니다.", "시도 서해안 경보시스템", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            #region 조위정보 저장
            HeightOptionMng.LstHeightOptionData[0].UseTime = this.heightUseCheckBox.Checked;
            HeightOptionMng.LstHeightOptionData[0].FirstTime = (int)this.firstHeightTimeNumeric.Value;
            HeightOptionMng.LstHeightOptionData[0].SecondTime = (int)this.secondHeightTimeNumeric.Value;
            HeightOptionMng.LstHeightOptionData[0].UseAuto = this.heightAutoCheckBox.Checked;
            HeightOptionMng.LstHeightOptionData[0].MaxValue = (int)this.heightValueNumeric.Value;
            HeightOptionMng.LstHeightOptionData[0].MaxValue2 = (int)this.heightValueNumeric2.Value;
            HeightOptionMng.LstHeightOptionData[0].MaxValue3 = (int)this.heightValueNumeric3.Value;
            HeightOptionMng.LstHeightOptionData[0].MaxValue4 = (int)this.heightValueNumeric4.Value;
            HeightOptionMng.LstHeightOptionData[0].Msg = this.heightSelectMsg;
            HeightOptionMng.LstHeightOptionData[0].Msg2 = this.heightSelectMsg2;
            HeightOptionMng.LstHeightOptionData[0].Msg3 = this.heightSelectMsg3;
            HeightOptionMng.LstHeightOptionData[0].Msg4 = this.heightSelectMsg4;
            HeightOptionMng.LstHeightOptionData[0].TestOrder = this.heightTestOrderCB.Checked;
            HeightOptionMng.SaveHeightOptionDatas();

            if ((this.heightSelectComboBox.SelectedItem as string) != null)
            {
                foreach (HeightPointContent eachPointContent in HeightPointContentMng.LstHeightPointContent)
                {
                    if (eachPointContent.Title != (this.heightSelectComboBox.SelectedItem as string))
                        continue;

                    eachPointContent.LstHeightPointData.Clear();
                    HeightPointData tmpData = null;

                    foreach (NCasListViewItem eachLvi in this.heightTermListView.Items)
                    {
                        if (eachLvi.Checked == false)
                            continue;

                        tmpData = new HeightPointData();
                        tmpData.Title = eachLvi.SubItems[1].Text;
                        tmpData.IpAddr = eachLvi.Name;

                        eachPointContent.LstHeightPointData.Add(tmpData);
                    }
                }

                HeightPointContentMng.SaveHeightPointContent();
            }
            #endregion

            #region 특보정보 저장
            WeatherOptionMng.LstWeatherOptionData[0].UseTime = this.weatherUseCheckBox.Checked;
            WeatherOptionMng.LstWeatherOptionData[0].FirstTime = (int)this.firstWeatherTimeNumeric.Value;
            WeatherOptionMng.LstWeatherOptionData[0].SecondTime = (int)this.secondWeatherTimeNumeric.Value;
            WeatherOptionMng.LstWeatherOptionData[0].UseAuto = this.weatherAutoCheckBox.Checked;
            WeatherOptionMng.LstWeatherOptionData[0].TestOrder = this.weatherTestOrderCB.Checked;
            WeatherOptionMng.SaveWeatherOptionDatas();
            #endregion
        }

        /// <summary>
        /// 기초데이터 초기화
        /// </summary>
        private void Init()
        {
            #region 조위ListView 초기화
            this.heightTermListView.GridLineStyle = NCasListViewGridLine.GridBoth;
            this.heightTermListView.GridDashStyle = DashStyle.Dot;
            this.heightTermListView.ScrollType = NCasListViewScrollType.ScrollBoth;
            this.heightTermListView.Font = new Font(WmaPAlmScreenRsc.FontName, 10.0f);
            this.heightTermListView.ColumnHeight = 29;
            this.heightTermListView.ItemHeight = 26;
            this.heightTermListView.HideColumnCheckBox = false;

            NCasColumnHeader col = new NCasColumnHeader();
            col.Text = string.Empty;
            col.Width = 30;
            col.SortType = NCasListViewSortType.SortChecked;
            col.TextAlign = HorizontalAlignment.Center;
            col.ColumnLock = true;
            col.ColumnHide = false;
            col.CheckBoxes = true;
            this.heightTermListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "이름";
            col.Width = 180;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Left;
            this.heightTermListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "IP";
            col.Width = 90;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Center;
            this.heightTermListView.Columns.Add(col);
            #endregion

            #region 조위ListView Items 셋팅
            foreach (TermInfo termInfo in this.provInfo.LstTerms)
            {
                if (termInfo.UseFlag != NCasDefineUseStatus.Use)
                    continue;

                if (termInfo.TermFlag != NCasDefineTermKind.TermMutil)
                    continue;

                NCasListViewItem lvi = new NCasListViewItem();
                lvi.Name = termInfo.IpAddrToSring;

                NCasListViewItem.NCasListViewSubItem sub = new NCasListViewItem.NCasListViewSubItem();
                sub.Text = termInfo.Name;
                sub.TextAlign = HorizontalAlignment.Left;
                lvi.SubItems.Add(sub);

                sub = new NCasListViewItem.NCasListViewSubItem();
                sub.Text = termInfo.IpAddrToSring;
                sub.TextAlign = HorizontalAlignment.Center;
                lvi.SubItems.Add(sub);

                this.heightTermListView.Items.Add(lvi);
            }
            #endregion

            #region 조위설정 불러오기
            this.heightUseCheckBox.Checked = HeightOptionMng.LstHeightOptionData[0].UseTime;
            this.firstHeightTimeNumeric.Value = HeightOptionMng.LstHeightOptionData[0].FirstTime;
            this.secondHeightTimeNumeric.Value = HeightOptionMng.LstHeightOptionData[0].SecondTime;
            this.heightAutoCheckBox.Checked = HeightOptionMng.LstHeightOptionData[0].UseAuto;
            this.heightValueNumeric.Value = HeightOptionMng.LstHeightOptionData[0].MaxValue;
            this.heightValueNumeric2.Value = HeightOptionMng.LstHeightOptionData[0].MaxValue2;
            this.heightValueNumeric3.Value = HeightOptionMng.LstHeightOptionData[0].MaxValue3;
            this.heightValueNumeric4.Value = HeightOptionMng.LstHeightOptionData[0].MaxValue4;
            this.heightTestOrderCB.Checked = HeightOptionMng.LstHeightOptionData[0].TestOrder;

            if (HeightOptionMng.LstHeightOptionData[0].Msg.MsgNum != string.Empty)
            {
                this.heightStoMsgSelLabel.Text = HeightOptionMng.LstHeightOptionData[0].Msg.Title;
                this.heightSelectMsg = HeightOptionMng.LstHeightOptionData[0].Msg;
            }
            else
            {
                this.heightStoMsgSelLabel.Text = "선택 없음";
            }

            if (HeightOptionMng.LstHeightOptionData[0].Msg2.MsgNum != string.Empty)
            {
                this.heightStoMsgSelLabel2.Text = HeightOptionMng.LstHeightOptionData[0].Msg2.Title;
                this.heightSelectMsg2 = HeightOptionMng.LstHeightOptionData[0].Msg2;
            }
            else
            {
                this.heightStoMsgSelLabel2.Text = "선택 없음";
            }

            if (HeightOptionMng.LstHeightOptionData[0].Msg3.MsgNum != string.Empty)
            {
                this.heightStoMsgSelLabel3.Text = HeightOptionMng.LstHeightOptionData[0].Msg3.Title;
                this.heightSelectMsg3 = HeightOptionMng.LstHeightOptionData[0].Msg3;
            }
            else
            {
                this.heightStoMsgSelLabel3.Text = "선택 없음";
            }

            if (HeightOptionMng.LstHeightOptionData[0].Msg4.MsgNum != string.Empty)
            {
                this.heightStoMsgSelLabel4.Text = HeightOptionMng.LstHeightOptionData[0].Msg4.Title;
                this.heightSelectMsg4 = HeightOptionMng.LstHeightOptionData[0].Msg4;
            }
            else
            {
                this.heightStoMsgSelLabel4.Text = "선택 없음";
            }

            foreach (HeightPointContent eachContent in HeightPointContentMng.LstHeightPointContent)
            {
                this.heightSelectComboBox.Items.Add(eachContent.Title);
            }
            #endregion

            #region 특보설정 불러오기
            this.weatherUseCheckBox.Checked = WeatherOptionMng.LstWeatherOptionData[0].UseTime;
            this.firstWeatherTimeNumeric.Value = WeatherOptionMng.LstWeatherOptionData[0].FirstTime;
            this.secondWeatherTimeNumeric.Value = WeatherOptionMng.LstWeatherOptionData[0].SecondTime;
            this.weatherAutoCheckBox.Checked = WeatherOptionMng.LstWeatherOptionData[0].UseAuto;
            this.weatherTestOrderCB.Checked = WeatherOptionMng.LstWeatherOptionData[0].TestOrder;

            foreach (WeatherKindData eachWeatherKind in WeatherOptionMng.LstWeatherOptionData[0].WeatherKindMsg)
            {
                if (eachWeatherKind.StoMsg.MsgNum == string.Empty)
                    continue;

                foreach (Control eachControl in this.weatherSelGroupBox.Controls)
                {
                    CheckBox eachCb = eachControl as CheckBox;

                    if (eachCb.Tag.ToString() != eachWeatherKind.EWeatherKind.ToString())
                        continue;

                    eachCb.Text = this.GetWeatherKindName(eachWeatherKind.EWeatherKind) + " - "
                        + eachWeatherKind.StoMsg.MsgNum + " " + eachWeatherKind.StoMsg.Title;
                }
            }
            #endregion
        }

        /// <summary>
        /// 특보명 반화 메소드
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        private string GetWeatherKindName(WeatherKindData.WeatherKind kind)
        {
            string rst = string.Empty;

            switch (kind)
            {
                case WeatherKindData.WeatherKind.galeWatch:
                    rst = "강풍 주의보";
                    break;

                case WeatherKindData.WeatherKind.galeAlarm:
                    rst = "강풍 경보";
                    break;

                case WeatherKindData.WeatherKind.downpourWatch:
                    rst = "호우 주의보";
                    break;

                case WeatherKindData.WeatherKind.downpourAlarm:
                    rst = "호우 경보";
                    break;

                case WeatherKindData.WeatherKind.coldwaveWatch:
                    rst = "한파 주의보";
                    break;

                case WeatherKindData.WeatherKind.coldwaveAlarm:
                    rst = "한파 경보";
                    break;

                case WeatherKindData.WeatherKind.dryWatch:
                    rst = "건조 주의보";
                    break;

                case WeatherKindData.WeatherKind.dryAlarm:
                    rst = "건조 경보";
                    break;

                case WeatherKindData.WeatherKind.tsunamiWatch:
                    rst = "해일 주의보";
                    break;

                case WeatherKindData.WeatherKind.tsunamiAlarm:
                    rst = "해일 경보";
                    break;

                case WeatherKindData.WeatherKind.heavyseasWatch:
                    rst = "풍랑 주의보";
                    break;

                case WeatherKindData.WeatherKind.heavyseasAlarm:
                    rst = "풍랑 경보";
                    break;

                case WeatherKindData.WeatherKind.stormWatch:
                    rst = "태풍 주의보";
                    break;

                case WeatherKindData.WeatherKind.stormAlarm:
                    rst = "태풍 경보";
                    break;

                case WeatherKindData.WeatherKind.heavysnowWatch:
                    rst = "대설 주의보";
                    break;

                case WeatherKindData.WeatherKind.heavysnowAlarm:
                    rst = "대설 경보";
                    break;

                case WeatherKindData.WeatherKind.sandstormWatch:
                    rst = "황사 주의보";
                    break;

                case WeatherKindData.WeatherKind.sandstormAlarm:
                    rst = "황사 경보";
                    break;
            }

            return rst;
        }
        #endregion

        #region UI Event
        /// <summary>
        /// 조위정보 사용 유무 체크박스 체인지 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void heightUseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (heightUseCheckBox.Checked)
            {
                firstHeightTimeNumeric.Enabled = true;
                secondHeightTimeNumeric.Enabled = true;
            }
            else
            {
                firstHeightTimeNumeric.Enabled = false;
                secondHeightTimeNumeric.Enabled = false;
            }
        }

        /// <summary>
        /// 조위정보 저장메시지 선택 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void heightStoMsgSelbutton_Click(object sender, EventArgs e)
        {
            using (NCasStoredMsgForm form = new NCasStoredMsgForm())
            {
                form.SelectStoredMsgEvent += new EventHandler<SelectStoredMsgEventArgs>(form_SelectStoredMsgEvent);
                form.ShowDialog();
                form.SelectStoredMsgEvent -= new EventHandler<SelectStoredMsgEventArgs>(form_SelectStoredMsgEvent);
            }
        }

        /// <summary>
        /// 조위2단계 정보 저장메시지 선택 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void heightStoMsgSelbutton2_Click(object sender, EventArgs e)
        {
            using (NCasStoredMsgForm form = new NCasStoredMsgForm())
            {
                form.SelectStoredMsgEvent += new EventHandler<SelectStoredMsgEventArgs>(form_SelectStoredMsgEvent2);
                form.ShowDialog();
                form.SelectStoredMsgEvent -= new EventHandler<SelectStoredMsgEventArgs>(form_SelectStoredMsgEvent2);
            }
        }

        /// <summary>
        /// 조위3단계 정보 저장메시지 선택 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void heightStoMsgSelbutton3_Click(object sender, EventArgs e)
        {
            using (NCasStoredMsgForm form = new NCasStoredMsgForm())
            {
                form.SelectStoredMsgEvent += new EventHandler<SelectStoredMsgEventArgs>(form_SelectStoredMsgEvent3);
                form.ShowDialog();
                form.SelectStoredMsgEvent -= new EventHandler<SelectStoredMsgEventArgs>(form_SelectStoredMsgEvent3);
            }
        }

        /// <summary>
        /// 조위4단계 정보 저장메시지 선택 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void heightStoMsgSelbutton4_Click(object sender, EventArgs e)
        {
            using (NCasStoredMsgForm form = new NCasStoredMsgForm())
            {
                form.SelectStoredMsgEvent += new EventHandler<SelectStoredMsgEventArgs>(form_SelectStoredMsgEvent4);
                form.ShowDialog();
                form.SelectStoredMsgEvent -= new EventHandler<SelectStoredMsgEventArgs>(form_SelectStoredMsgEvent4);
            }
        }

        void form_SelectStoredMsgEvent(object sender, SelectStoredMsgEventArgs e)
        {
            if (e.SelectFlag) //저장메시지를 선택했으면..
            {
                this.heightSelectMsg.IsUse = e.StoredMsg.IsUse;
                this.heightSelectMsg.MsgNum = e.StoredMsg.MsgNum;
                this.heightSelectMsg.PlayTime = e.StoredMsg.PlayTime;
                this.heightSelectMsg.RepeatCount = e.StoredMsg.RepeatCount;
                this.heightSelectMsg.Text = e.StoredMsg.Text;
                this.heightSelectMsg.Title = e.StoredMsg.Title;
                this.heightStoMsgSelLabel.Text = e.StoredMsg.Title;
            }
            else
            {
                this.heightSelectMsg = null;
                this.heightSelectMsg = new StoredMessageText();
                this.heightStoMsgSelLabel.Text = "선택 없음";
            }
        }

        void form_SelectStoredMsgEvent2(object sender, SelectStoredMsgEventArgs e)
        {
            if (e.SelectFlag) //저장메시지를 선택했으면..
            {
                this.heightSelectMsg2.IsUse = e.StoredMsg.IsUse;
                this.heightSelectMsg2.MsgNum = e.StoredMsg.MsgNum;
                this.heightSelectMsg2.PlayTime = e.StoredMsg.PlayTime;
                this.heightSelectMsg2.RepeatCount = e.StoredMsg.RepeatCount;
                this.heightSelectMsg2.Text = e.StoredMsg.Text;
                this.heightSelectMsg2.Title = e.StoredMsg.Title;
                this.heightStoMsgSelLabel2.Text = e.StoredMsg.Title;
            }
            else
            {
                this.heightSelectMsg2 = null;
                this.heightSelectMsg2 = new StoredMessageText();
                this.heightStoMsgSelLabel2.Text = "선택 없음";
            }
        }

        void form_SelectStoredMsgEvent3(object sender, SelectStoredMsgEventArgs e)
        {
            if (e.SelectFlag) //저장메시지를 선택했으면..
            {
                this.heightSelectMsg3.IsUse = e.StoredMsg.IsUse;
                this.heightSelectMsg3.MsgNum = e.StoredMsg.MsgNum;
                this.heightSelectMsg3.PlayTime = e.StoredMsg.PlayTime;
                this.heightSelectMsg3.RepeatCount = e.StoredMsg.RepeatCount;
                this.heightSelectMsg3.Text = e.StoredMsg.Text;
                this.heightSelectMsg3.Title = e.StoredMsg.Title;
                this.heightStoMsgSelLabel3.Text = e.StoredMsg.Title;
            }
            else
            {
                this.heightSelectMsg3 = null;
                this.heightSelectMsg3 = new StoredMessageText();
                this.heightStoMsgSelLabel3.Text = "선택 없음";
            }
        }

        void form_SelectStoredMsgEvent4(object sender, SelectStoredMsgEventArgs e)
        {
            if (e.SelectFlag) //저장메시지를 선택했으면..
            {
                this.heightSelectMsg4.IsUse = e.StoredMsg.IsUse;
                this.heightSelectMsg4.MsgNum = e.StoredMsg.MsgNum;
                this.heightSelectMsg4.PlayTime = e.StoredMsg.PlayTime;
                this.heightSelectMsg4.RepeatCount = e.StoredMsg.RepeatCount;
                this.heightSelectMsg4.Text = e.StoredMsg.Text;
                this.heightSelectMsg4.Title = e.StoredMsg.Title;
                this.heightStoMsgSelLabel4.Text = e.StoredMsg.Title;
            }
            else
            {
                this.heightSelectMsg4 = null;
                this.heightSelectMsg4 = new StoredMessageText();
                this.heightStoMsgSelLabel4.Text = "선택 없음";
            }
        }

        /// <summary>
        /// 특보정보 사용 유무 체크박스 체인지 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void weatherUseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (weatherUseCheckBox.Checked)
            {
                firstWeatherTimeNumeric.Enabled = true;
                secondWeatherTimeNumeric.Enabled = true;
            }
            else
            {
                firstWeatherTimeNumeric.Enabled = false;
                secondWeatherTimeNumeric.Enabled = false;
            }
        }

        /// <summary>
        /// 특보정보 저장메시지 선택 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void weatherStoMsgSelbutton_Click(object sender, EventArgs e)
        {
            bool isCheck = false;

            foreach (Control eachControl in this.weatherSelGroupBox.Controls)
            {
                CheckBox eachCb = eachControl as CheckBox;

                if (eachCb.Checked)
                {
                    isCheck = true;
                    break;
                }
            }

            if (!isCheck)
            {
                MessageBox.Show("저장메시지를 설정할 특보를 체크한 후 선택하세요.", "시도 서해안다목적 경보시스템", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            using (NCasStoredMsgForm form = new NCasStoredMsgForm())
            {
                form.SelectStoredMsgEvent += new EventHandler<SelectStoredMsgEventArgs>(form_weatherSelectStoredMsgEvent);
                form.ShowDialog();
                form.SelectStoredMsgEvent -= new EventHandler<SelectStoredMsgEventArgs>(form_weatherSelectStoredMsgEvent);
            }
        }

        void form_weatherSelectStoredMsgEvent(object sender, SelectStoredMsgEventArgs e)
        {
            if (e.SelectFlag) //저장메시지를 선택했으면..
            {
                foreach (WeatherKindData eachWeatherKind in WeatherOptionMng.LstWeatherOptionData[0].WeatherKindMsg)
                {
                    foreach (Control eachControl in this.weatherSelGroupBox.Controls)
                    {
                        CheckBox eachCb = eachControl as CheckBox;

                        if (eachCb.Checked && (eachCb.Tag.ToString() == eachWeatherKind.EWeatherKind.ToString()))
                        {
                            eachWeatherKind.StoMsg.IsUse = e.StoredMsg.IsUse;
                            eachWeatherKind.StoMsg.MsgNum = e.StoredMsg.MsgNum;
                            eachWeatherKind.StoMsg.PlayTime = e.StoredMsg.PlayTime;
                            eachWeatherKind.StoMsg.RepeatCount = e.StoredMsg.RepeatCount;
                            eachWeatherKind.StoMsg.Text = e.StoredMsg.Text;
                            eachWeatherKind.StoMsg.Title = e.StoredMsg.Title;

                            eachCb.Text = this.GetWeatherKindName(eachWeatherKind.EWeatherKind) + " - "
                            + eachWeatherKind.StoMsg.MsgNum + " " + eachWeatherKind.StoMsg.Title;
                        }
                    }
                }
            }

            foreach (Control eachControl in this.weatherSelGroupBox.Controls)
            {
                CheckBox eachCb = eachControl as CheckBox;
                eachCb.Checked = false;
            }
        }

        /// <summary>
        /// 저장버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveBtn_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        /// <summary>
        /// 닫기버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            //this.Save();
            this.Close();
        }

        /// <summary>
        /// 관측소 선택 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void heightSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            HeightPointContent content = HeightPointContentMng.GetHeightPointContent(this.heightSelectComboBox.SelectedItem as string);

            foreach (NCasListViewItem eachTerm in this.heightTermListView.Items)
            {
                eachTerm.Checked = false;
                Refresh();

                foreach (HeightPointData eachPoint in content.LstHeightPointData)
                {
                    if (eachTerm.Name != eachPoint.IpAddr)
                        continue;

                    eachTerm.Checked = true;
                    Refresh();
                    break;
                }
            }
        }
        #endregion
    }
}