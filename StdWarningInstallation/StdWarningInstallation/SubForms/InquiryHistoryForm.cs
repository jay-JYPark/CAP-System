using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Adeng.Framework.Ctrl;
using StdWarningInstallation.DataClass;

namespace StdWarningInstallation
{
    public partial class InquiryHistoryForm : Form
    {
        private readonly int PROGRAM_HISTORY_LIMIT_CNT = 1000;  // 프로그램 사용 이력은 최대 1000개까지만 화면에 출력.

        private OrderInquiryCondition orderInquiryCondition = new OrderInquiryCondition();
        private OrderDeTailForm orderDetailForm = null;

        public InquiryHistoryForm()
        {
            InitializeComponent();

            InitializeOrderTab();
            InitializeProgramTab();
        }
        public void SetInquiryKind(int tabIndex)
        {
            if (tabIndex == 0)
            {
                // 디폴트
            }
            else
            {
                this.tabCtlMain.SelectedIndex = tabIndex;
            }
        }

        #region 컨트롤초기설정
        private void InitializeOrderTab()
        {
            this.dtmPickerOrderDurtionStart.Value = DateTime.Now;
            this.dtmPickerOrderDurtionEnd.Value = DateTime.Now;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            DateTime start = new DateTime(year, month, day);

            DateTime end = start.AddDays(1).AddMilliseconds(-1);

            this.dtmPickerOrderDurtionStart.Value = start;
            this.dtmPickerOrderDurtionEnd.Value = end;

            // 발령이력 컨트롤 초기화
            InitializeDisasterCmbbox();
            InitializeOrderModeCmbbox();
            InitializeOrderLocationCmbbox();
            InitializeOrderHistoryList();
        }
        private void InitializeProgramTab()
        {
            this.dtmPickerProgramDurtionStart.Value = DateTime.Now;
            this.dtmPickerProgramDurtionEnd.Value = DateTime.Now;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int day = DateTime.Now.Day;
            DateTime start = new DateTime(year, month, day);

            DateTime end = start.AddDays(1).AddMilliseconds(-1);

            this.dtmPickerProgramDurtionStart.Value = start;
            this.dtmPickerProgramDurtionEnd.Value = end;

            InitializeProgramHistoryList();
        }
        /// <summary>
        /// 재난 카테고리/종류 콤보박스 설정.
        /// </summary>
        private void InitializeDisasterCmbbox()
        {
            this.cmbboxDisasterCategory.Items.Clear();
            this.cmbboxDisasterKind.Items.Clear();

            this.cmbboxDisasterCategory.Enabled = false;
            this.cmbboxDisasterKind.Enabled = false;

            if (BasisData.Disasters == null || BasisData.Disasters.Count <= 0)
            {
                this.cmbboxDisasterCategory.Enabled = false;
                this.cmbboxDisasterKind.Enabled = false;

                return;
            }

            this.cmbboxDisasterCategory.Enabled = true;
            this.cmbboxDisasterCategory.Items.Add("전체");
            foreach (DisasterInfo info in BasisData.Disasters)
            {
                if (info == null || info.Category == null)
                {
                    continue;
                }
                DisasterInfo copy = new DisasterInfo();
                copy.DeepCopyFrom(info);

                this.cmbboxDisasterCategory.Items.Add(copy);
            }
            this.cmbboxDisasterCategory.SelectedIndex = 0;
        }
        /// <summary>
        /// 발령 모드 콤보박스 설정.
        /// </summary>
        private void InitializeOrderModeCmbbox()
        {
            this.cmbboxOrderMode.Enabled = false;
            this.cmbboxOrderMode.Items.Clear();

            this.cmbboxOrderMode.Enabled = true;
            this.cmbboxOrderMode.Items.Add("전체");
            foreach (OrderMode mode in BasisData.OrderModeInfo.Values)
            {
                OrderMode copy = new OrderMode();
                copy.DeepCopyFrom(mode);

                this.cmbboxOrderMode.Items.Add(copy);
            }

            this.cmbboxOrderMode.SelectedIndex = 0;
        }
        /// <summary>
        /// 발령지 구분 콤보박스 설정.
        /// </summary>
        private void InitializeOrderLocationCmbbox()
        {
            this.cmbboxOrderLocation.Items.Clear();

            this.cmbboxOrderLocation.Enabled = true;
            this.cmbboxOrderLocation.Items.Add("전체");
            foreach (OrderLocationKindInfo location in BasisData.OrderLocationType.Values)
            {
                OrderLocationKindInfo copy = new OrderLocationKindInfo();
                copy.DeepCopyFrom(location);
                this.cmbboxOrderLocation.Items.Add(copy);
            }

            this.cmbboxOrderLocation.SelectedIndex = 0;
        }
        /// <summary>
        /// 발령 이력 조회 결과 표시 리스트 설정.
        /// </summary>
        private void InitializeOrderHistoryList()
        {
            this.lvOrderHistory.Columns.Clear();

            AdengColumnHeader header = new AdengColumnHeader();
            header.Text = "";
            header.Width = 40;
            this.lvOrderHistory.Columns.Add(header);

            header = new AdengColumnHeader();
            header.Text = "아이디";
            header.Width = 160;
            this.lvOrderHistory.Columns.Add(header);

            header = new AdengColumnHeader();
            header.Text = "발령 시각";
            header.Width = 180;
            this.lvOrderHistory.Columns.Add(header);

            header = new AdengColumnHeader();
            header.Text = "발령지 구분";
            header.Width = 70;
            this.lvOrderHistory.Columns.Add(header);

            header = new AdengColumnHeader();
            header.Text = "모드";
            header.Width = 50;
            this.lvOrderHistory.Columns.Add(header);

            header = new AdengColumnHeader();
            header.Text = "재난 종류";
            header.Width = 150;
            this.lvOrderHistory.Columns.Add(header);

            header = new AdengColumnHeader();
            header.Text = "발령 대상";
            header.Width = 220;
            this.lvOrderHistory.Columns.Add(header);

            header = new AdengColumnHeader();
            header.Text = "비고";
            header.Width = 70;
            this.lvOrderHistory.Columns.Add(header);
        }
        /// <summary>
        /// 프로그램 사용 이력 조회 결과 표시 리스트 설정.
        /// </summary>
        private void InitializeProgramHistoryList()
        {
            this.lvProgramHistoryList.Columns.Clear();

            if (this.imglstEventLogIcons != null && this.imglstEventLogIcons.Images != null)
            {
                foreach (Image img in this.imglstEventLogIcons.Images)
                {
                    this.lvProgramHistoryList.StateImageList.Images.Add(img);
                }
            }

            AdengColumnHeader header = new AdengColumnHeader();
            header.Text = "";
            header.Width = 30;
            header.TextAlign = HorizontalAlignment.Center;
            this.lvProgramHistoryList.Columns.Add(header);

            header = new AdengColumnHeader();
            header.Text = "종류";
            header.Width = 130;
            this.lvProgramHistoryList.Columns.Add(header);

            header = new AdengColumnHeader();
            header.Text = "날짜";
            header.Width = 110;
            this.lvProgramHistoryList.Columns.Add(header);

            header = new AdengColumnHeader();
            header.Text = "시간";
            header.Width = 100;
            this.lvProgramHistoryList.Columns.Add(header);

            header = new AdengColumnHeader();
            header.Text = "내용";
            header.Width = 400;
            this.lvProgramHistoryList.Columns.Add(header);
        }
        #endregion


        #region 데이터갱신
        /// <summary>
        /// 발령 이력 조회 리스트 갱신.
        /// </summary>
        /// <param name="historyList"></param>
        private void UpdateOrderHistoryList(List<OrderRecord> historyList)
        {
            this.lvOrderHistory.Items.Clear();
            if (this.lvOrderHistory.Columns == null)
            {
                return;
            }

            foreach (OrderRecord record in historyList)
            {
                string targetsName = string.Empty;
                if (!string.IsNullOrEmpty(record.CapText))
                {
                    CAPHelper helper = new CAPHelper();
                    targetsName = helper.ExtractTargetNamesFromCAP(new CAPLib.CAP(record.CapText));
                    if (string.IsNullOrEmpty(targetsName))
                    {
                        targetsName = "";
                    }
                }

                // 결과
                AdengListViewItem item = this.lvOrderHistory.Items.Add((this.lvOrderHistory.Items.Count + 1).ToString());

                // 아이디
                item.SubItems.Add(record.CAPID);

                // 발령 시각
                item.SubItems.Add(record.OrderedTime.ToString());

                // 발령지 구분
                item.SubItems.Add(BasisData.GetDisplayStringLocationKindName(record.LocationKind));

                // 발령 모드
                item.SubItems.Add(BasisData.GetDisplayStringOrderMode(record.OrderMode));

                // 재난 정보
                item.SubItems.Add(BasisData.GetDisplayStringDisasterKindName(record.DisasterKindCode));

                // 발령 대상
                item.SubItems.Add(targetsName);

                // 발령 참조 정보
                item.SubItems.Add(record.RefRecordID);

                item.Name = record.CAPID;
                item.Tag = record;
            }
        }
        private void UpdateProgramHistoryList(List<Log> logList)
        {
            this.lvProgramHistoryList.Items.Clear();
            if (this.lvProgramHistoryList.Columns == null)
            {
                return;
            }

            if (logList == null || logList.Count < 1)
            {
                return;
            }

            int count = logList.Count;
            if (count > PROGRAM_HISTORY_LIMIT_CNT)
            {
                count = PROGRAM_HISTORY_LIMIT_CNT;
            }
            for (int index = 0; index < count; index++)
            {
                Log log = logList.ElementAt(count - 1 - index);
                if (log == null)
                {
                    break;
                }

                // 상태 아이콘
                AdengListViewItem item =  this.lvProgramHistoryList.Items.Add("");
                item.ImageIndex = 0;
                if (log.Kind.ToUpper() == "ERROR")
                {
                    item.ImageIndex = 1;
                }

                // 종류
                item.SubItems.Add(log.Kind);

                // 날짜
                item.SubItems.Add(log.Date);

                // 시간
                item.SubItems.Add(log.Time);

                // 내용
                item.SubItems.Add(log.Message);
            }
        }
        #endregion


        #region 발령이력조회
        /// <summary>
        /// 발령 이력 조회 기간 설정 변경(시작일)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtmPickerOrderDurtionStart_ValueChanged(object sender, EventArgs e)
        {
            this.orderInquiryCondition.StartTime = this.dtmPickerOrderDurtionStart.Value;
        }
        /// <summary>
        /// 발령 이력 조회 기간 설정 변경(종료일)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtmPickerOrderDurtionEnd_ValueChanged(object sender, EventArgs e)
        {
            this.orderInquiryCondition.EndTime = this.dtmPickerOrderDurtionEnd.Value;
        }

        /// <summary>
        /// [재난 종류] 카테고리 선택 변경.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbboxDisasterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbboxDisasterKind.ResetText();
            this.cmbboxDisasterKind.Items.Clear();
            this.cmbboxDisasterKind.Enabled = false;

            this.orderInquiryCondition.Disaster = null;

            ComboBox cmb = sender as ComboBox;
            if (cmb.SelectedIndex < 0)
            {
                return;
            }
            else if (cmb.SelectedIndex == 0 && cmb.SelectedText == "전체")
            {
                return;
            }
            else
            {
            }

            this.cmbboxDisasterKind.Enabled = true;

            DisasterInfo info = cmb.SelectedItem as DisasterInfo;
            if (info == null || info.Category == null)
            {
                this.cmbboxDisasterKind.Enabled = false;
                return;
            }

            this.orderInquiryCondition.Disaster = new Disaster(info.Category, null);

            this.cmbboxDisasterKind.Items.Add("전체");
            foreach (DisasterKind kind in info.KindList)
            {
                this.cmbboxDisasterKind.Items.Add(kind);
            }
            this.cmbboxDisasterKind.SelectedIndex = 0;
        }
        /// <summary>
        /// [재난 종류] 코드 선택 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbboxDisasterKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.cmbboxDisasterKind.Focused)
            {
                return;
            }

            this.orderInquiryCondition.Disaster.Kind = null;

            if (this.orderInquiryCondition.Disaster == null ||
                this.orderInquiryCondition.Disaster.Category == null)
            {
                return;
            }

            ComboBox cmb = sender as ComboBox;
            if (cmb.SelectedIndex < 0)
            {
                return;
            }
            if (cmb.SelectedIndex == 0 && cmb.SelectedText == "전체")
            {
                return;
            }

            DisasterKind selectedKind = cmb.SelectedItem as DisasterKind;
            if (selectedKind == null)
            {
                return;
            }
            this.orderInquiryCondition.Disaster.Kind = selectedKind;
        }

        /// <summary>
        /// [발령 모드] 선택 변경.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbboxOrderMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.cmbboxOrderMode.Focused)
            {
                return;
            }

            this.orderInquiryCondition.OrderMode = null;
            if (this.cmbboxOrderMode.SelectedIndex == 0 && this.cmbboxOrderMode.SelectedText == "전체")
            {
                return;
            }
            OrderMode selectedMode = this.cmbboxOrderMode.SelectedItem as OrderMode;
            if (selectedMode == null)
            {
                return;
            }
            this.orderInquiryCondition.OrderMode = selectedMode;
        }

        /// <summary>
        /// [발령지 구분] 선택 변경.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbboxOrderLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.cmbboxOrderLocation.Focused)
            {
                return;
            }

            this.orderInquiryCondition.OrderLocationKind = null;
            if (this.cmbboxOrderLocation.SelectedIndex == 0 && this.cmbboxOrderLocation.SelectedText == "전체")
            {
                return;
            }
            OrderLocationKindInfo selectedLocation = this.cmbboxOrderLocation.SelectedItem as OrderLocationKindInfo;
            if (selectedLocation == null)
            {
                return;
            }
            this.orderInquiryCondition.OrderLocationKind = selectedLocation;
        }

        /// <summary>
        /// [발령 이력][조회] 버튼 클릭.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInquriyOrderHistory_Click(object sender, EventArgs e)
        {
            this.lvOrderHistory.Items.Clear();

            int year = this.dtmPickerOrderDurtionStart.Value.Year;
            int month = this.dtmPickerOrderDurtionStart.Value.Month;
            int day = this.dtmPickerOrderDurtionStart.Value.Day;
            DateTime start = new DateTime(year, month, day, 0, 0, 0);

            year = this.dtmPickerOrderDurtionEnd.Value.Year;
            month = this.dtmPickerOrderDurtionEnd.Value.Month;
            day = this.dtmPickerOrderDurtionEnd.Value.Day;
            DateTime end = new DateTime(year, month, day, 0, 0, 0);
            end = end.AddDays(1).AddMilliseconds(-1);

            if (this.orderInquiryCondition.StartTime.Ticks < 0 || this.orderInquiryCondition.EndTime.Ticks < 0)
            {
                this.orderInquiryCondition.StartTime = this.dtmPickerOrderDurtionStart.Value;
                this.orderInquiryCondition.EndTime = this.dtmPickerOrderDurtionEnd.Value;
            }
            this.orderInquiryCondition.Count = 1000;

            // 입력값 체크
            if (start > end)
            {
                MessageBox.Show("검색 기간 설정이 올바르지 않습니다. 종료일이 시작일보다 빠릅니다. \n날짜를 바르게 입력해 주세요.", "입력 날짜 오류", MessageBoxButtons.OK);

                this.dtmPickerOrderDurtionEnd.Focus();
                return;
            }


            // 조회
            List<OrderRecord> historyList = null;
            int result = DBManager.GetInstance().QueryOrderHistory(this.orderInquiryCondition, out historyList);
            if (result < 0)
            {
                MessageBox.Show("발령 이력 조회 중에 문제가 발생하여 조회 실패하였습니다. \n Error=[" + result + "]", 
                                    "발령 이력 조회 실패", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (result > 0)
            {
                MessageBox.Show("해당 조건의 발령 이력이 존재하지 않습니다.",
                                    "발령 이력 조회 결과", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                UpdateOrderHistoryList(historyList);
            }
        }

        /// <summary>
        /// 발령 이력 목록 아이템 더블 클릭.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvOrderHistory_DoubleClick(object sender, EventArgs e)
        {
            AdengListView ctrl = sender as AdengListView;
            if (ctrl == null)
            {
                return;
            }
            if (ctrl.SelectedItems == null || ctrl.SelectedItems.Count < 1)
            {
                return;
            }
            OrderRecord selectedRecord = ctrl.SelectedItems[0].Tag as OrderRecord;
            if (selectedRecord == null)
            {
                return;
            }

            List<OrderResponseProfile> responseList = null;
            int result = DBManager.GetInstance().QueryOrderResponse(selectedRecord.CAPID, out responseList);
            if (result < 0)
            {
                return;
            }

            if (this.orderDetailForm == null || this.orderDetailForm.IsDisposed)
            {
                this.orderDetailForm = new OrderDeTailForm(selectedRecord, responseList);
                this.orderDetailForm.ShowDialog(this);
            }
            this.orderDetailForm = null;
        }
        #endregion


        #region 프로그램이력조회
        /// <summary>
        /// 프로그램 이력 조회 기간 설정 변경(시작일)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtmPickerProgramDurtionStart_ValueChanged(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 프로그램 이력 조회 기간 설정 변경(종료일)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dtmPickerProgramDurtionEnd_ValueChanged(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// [프로그램 사용 이력][조회] 버튼 클릭.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInquiryProgramHistory_Click(object sender, EventArgs e)
        {
            this.lvProgramHistoryList.Items.Clear();

            int year = this.dtmPickerProgramDurtionStart.Value.Year;
            int month = this.dtmPickerProgramDurtionStart.Value.Month;
            int day = this.dtmPickerProgramDurtionStart.Value.Day;
            int hour = this.dtmPickerProgramDurtionStart.Value.Hour;
            DateTime start = new DateTime(year, month, day, hour, 00, 00);

            year = this.dtmPickerProgramDurtionEnd.Value.Year;
            month = this.dtmPickerProgramDurtionEnd.Value.Month;
            day = this.dtmPickerProgramDurtionEnd.Value.Day;
            hour = this.dtmPickerProgramDurtionEnd.Value.Hour;
            DateTime end = new DateTime(year, month, day, hour, 00, 00);
            end = end.AddHours(1).AddMilliseconds(-1);

            if (start.Ticks > end.Ticks)
            {
                MessageBox.Show("검색 기간 설정이 올바르지 않습니다. 종료일이 시작일보다 빠릅니다. \n날짜를 바르게 입력해 주세요.", "입력 날짜 오류", MessageBoxButtons.OK);

                this.dtmPickerProgramDurtionEnd.Focus();
                return;
            }

            List<Log> logList = EventLogManager.GetInstance().ReadLog(start, end);
            if (logList == null | logList.Count() < 1)
            {
                MessageBox.Show("해당 조건의 프로그램 사용 이력이 존재하지 않습니다.", "프로그램 사용 이력 조회 결과", MessageBoxButtons.OK);

                return;
            }

            UpdateProgramHistoryList(logList);
        }
        #endregion


        #region 공통(컨트롤)
        /// <summary>
        /// [초기화] 버튼 클릭.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (this.tabCtlMain.SelectedTab == this.tpOrderHistory)
            {
                this.dtmPickerOrderDurtionStart.Value = DateTime.Now;
                this.dtmPickerOrderDurtionEnd.Value = DateTime.Now;

                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;
                int day = DateTime.Now.Day;
                DateTime start = new DateTime(year, month, day);

                DateTime end = start.AddDays(1).AddMilliseconds(-1);

                this.dtmPickerOrderDurtionStart.Value = start;
                this.dtmPickerOrderDurtionEnd.Value = end;

                this.cmbboxDisasterCategory.SelectedIndex = 0;
                this.cmbboxDisasterKind.ResetText();
                this.cmbboxDisasterKind.Items.Clear();
                this.cmbboxDisasterKind.Enabled = false;

                this.cmbboxOrderMode.SelectedIndex = 0;
                this.cmbboxOrderLocation.SelectedIndex = 0;

                this.lvOrderHistory.Items.Clear();
            }
            else
            {
                int year = DateTime.Now.Year;
                int month = DateTime.Now.Month;
                int day = DateTime.Now.Day;
                DateTime start = new DateTime(year, month, day);

                DateTime end = start.AddDays(1).AddMilliseconds(-1);

                this.dtmPickerProgramDurtionStart.Value = start;
                this.dtmPickerProgramDurtionEnd.Value = end;

                this.lvProgramHistoryList.Items.Clear();
            }
        }
        /// <summary>
        /// [닫기] 버튼 클릭.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion


        #region 외부이벤트수신
        public void OnNotifyOrderResponseUpdated(object sender, OrderResponseEventArgs e)
        {
            try
            {
                MethodInvoker invoker = delegate()
                {
                    System.Console.WriteLine("[InquiryHistoryForm] OnNotifyOrderResponseUpdated ( 응답 수신 )");
                    FileLogManager.GetInstance().WriteLog("[InquiryHistoryForm] OnNotifyOrderResponseUpdated( 응답 수신 )");

                    if (this.orderDetailForm == null || this.orderDetailForm.IsDisposed)
                    {
                        System.Console.WriteLine("[InquiryHistoryForm] OnNotifyOrderResponseUpdated ( 상세 표시 중이 아님. )");
                        FileLogManager.GetInstance().WriteLog("[InquiryHistoryForm] OnNotifyOrderResponseUpdated( 상세 표시 중이 아님. )");

                        return;
                    }
                    if (this.lvOrderHistory.SelectedItems == null || this.lvOrderHistory.SelectedItems.Count < 1)
                    {
                        System.Console.WriteLine("[InquiryHistoryForm] OnNotifyOrderResponseUpdated ( 선택된 아이템이 없음 )");
                        FileLogManager.GetInstance().WriteLog("[InquiryHistoryForm] OnNotifyOrderResponseUpdated( 선택된 아이템이 없음 )");

                        return;
                    }
                    OrderRecord selectedRecord = this.lvOrderHistory.SelectedItems[0].Tag as OrderRecord;
                    if (selectedRecord == null)
                    {
                        System.Console.WriteLine("[InquiryHistoryForm] OnNotifyOrderResponseUpdated ( 선택된 아이템의 데이터 변환 오류 )");
                        FileLogManager.GetInstance().WriteLog("[InquiryHistoryForm] OnNotifyOrderResponseUpdated( 선택된 아이템의 데이터 변환 오류 )");

                        return;
                    }
                    if (e.ResponseInfo == null || e.ResponseInfo.Count < 1 || selectedRecord.CAPID != e.OrderRecordID)
                    {
                        System.Console.WriteLine("[InquiryHistoryForm] OnNotifyOrderResponseUpdated ( 상세 표시 중인 레코드와 응답 레코드가 다름 )");
                        FileLogManager.GetInstance().WriteLog("[InquiryHistoryForm] OnNotifyOrderResponseUpdated( 상세 표시 중인 레코드와 응답 레코드가 다름 )");

                        return;
                    }

                    this.orderDetailForm.UpdateOrderResponseInfo(e.ResponseInfo);
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
                System.Console.WriteLine("[RecentlyOrderHistoryForm] OnNotifyOrderResponseUpdated ( Exception Occured!!!" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[InquiryHistoryForm] OnNotifyOrderResponseUpdated( " + ex.ToString() + " )");
            }
        }
        #endregion

    }
}
