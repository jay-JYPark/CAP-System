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
    public partial class RecentlyOrderHistoryForm : Form
    {
        public event EventHandler<RequestCancelOrderEventArgs> NotifyRequestCancelOrder;

        private List<OrderRecord> currentOrderHistory = new List<OrderRecord>();

        OrderDeTailForm orderDetailForm = null;

        public RecentlyOrderHistoryForm()
        {
            InitializeComponent();
        }
        public RecentlyOrderHistoryForm(List<OrderRecord> history)
        {
            InitializeComponent();

            SetOrderHistory(history);
        }
        public void SetOrderHistory(List<OrderRecord> history)
        {
            if (history == null)
            {
                return;
            }
            foreach (OrderRecord record in history)
            {
                OrderRecord copy = new OrderRecord();
                copy.DeepCopyFrom(record);
                this.currentOrderHistory.Add(copy);
            }
        }

        private void LoadForm(object sender, EventArgs e)
        {
            if (this.lvOrderList.Columns == null || this.lvOrderList.Columns.Count < 1)
            {
                // [발령 이력 조회] 리스트 헤더 설정
                AdengColumnHeader header = new AdengColumnHeader();
                header.Text = "";
                header.Width = 30;
                header.TextAlign = HorizontalAlignment.Center;
                this.lvOrderList.Columns.Add(header);
                //header = new AdengColumnHeader();
                //header.Text = "결과";
                //header.Width = 50;
                //this.lvOrderList.Columns.Add(header);
                header = new AdengColumnHeader();
                header.Text = "발령 시각";
                header.Width = 172;
                this.lvOrderList.Columns.Add(header);
                header = new AdengColumnHeader();
                header.Text = "발령지 구분";
                header.Width = 80;
                this.lvOrderList.Columns.Add(header);
                header = new AdengColumnHeader();
                header.Text = "모드";
                header.Width = 50;
                this.lvOrderList.Columns.Add(header);
                header = new AdengColumnHeader();
                header.Text = "재난 종류";
                header.Width = 124;
                this.lvOrderList.Columns.Add(header);
                header = new AdengColumnHeader();
                header.Text = "발령 대상";
                header.Width = 220;
                this.lvOrderList.Columns.Add(header);
                header = new AdengColumnHeader();
                header.Text = "비고";
                header.Width = 70;
                this.lvOrderList.Columns.Add(header);
            }

            UpdateRecord();
        }

        private void UpdateRecord()
        {
            this.lvOrderList.Items.Clear();

            // 리스트 아이템 추가
            int index = 0;

            if (this.currentOrderHistory == null)
            {
                return;
            }

            foreach (OrderRecord record in this.currentOrderHistory)
            {
                if (record == null)
                {
                    continue;
                }

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

                // 번호
                AdengListViewItem item = this.lvOrderList.Items.Add((index + 1).ToString());
                item.TextAlign = HorizontalAlignment.Center;
                // 결과
                //this.lvOrderList.Items[index].SubItems.Add(" ");
                // 발령 시각
                item.SubItems.Add(record.OrderedTime.ToString());
                // 발령지 구분
                item.SubItems.Add(BasisData.GetDisplayStringLocationKindName(record.LocationKind));
                // 발령 모드
                item.SubItems.Add(BasisData.GetDisplayStringOrderMode(record.OrderMode));
                // 이벤트 종류
                item.SubItems.Add(BasisData.GetDisplayStringDisasterKindName(record.DisasterKindCode));
                // 발령 대상
                item.SubItems.Add(targetsName);
                // 비고(참조 유형)
                item.SubItems.Add(BasisData.GetOrderRefTypeDescription(record.RefType));

                item.Name = record.CAPID;
                item.Tag = record;

                index++;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// [발령 취소] 버튼 클릭 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOrderCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.None;

            // 선택된 발령 이력에서 정보를 추출
            if (this.lvOrderList.SelectedItems == null || this.lvOrderList.SelectedItems.Count != 1)
            {
                return;
            }
            string recordCAPID = this.lvOrderList.SelectedItems[0].Name;
            if (string.IsNullOrEmpty(recordCAPID))
            {
                MessageBox.Show("선택한 항목의 데이터가 올바르지 않습니다. \n발령 취소를 할 수 없습니다.", "발령 취소 실패", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            OrderRecord record = null;
            if (this.lvOrderList.SelectedItems[0].Tag != null)
            {
                record = this.lvOrderList.SelectedItems[0].Tag as OrderRecord;
            }

            // 메인에 전달
            if (this.NotifyRequestCancelOrder != null)
            {
                this.NotifyRequestCancelOrder(this, new RequestCancelOrderEventArgs(recordCAPID, record));
            }

            // 현 윈도우를 종료
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void lvOrderList_ItemSelectedChanged(object sender, AdengListViewItemSelectionChangedEventArgs e)
        {
            AdengListView ctrl = sender as AdengListView;
            if (ctrl == null)
            {
                return;
            }

            if (ctrl.SelectedItems == null || ctrl.SelectedItems.Count < 1)
            {
                this.btnOrderCancel.Enabled = false;
                return;
            }

            OrderRecord record = e.Item.Tag as OrderRecord;
            if (record != null)
            {
                // 발령 취소에는 많은 제약을 두지 않고, 뭐든지 취소 발령은 가능하도록 하라는 지시.
                // 단, 자체발령이 아닌 상위/동일레벨타지역의 발령을 취소하는 것에는 제약을 둔다.
                if (record.LocationKind != OrderLocationKind.Local)
                {
                    this.btnOrderCancel.Enabled = false;
                    return;
                }

                this.btnOrderCancel.Enabled = true;
            }
        }

        private void lvOrderList_DoubleClick(object sender, EventArgs e)
        {
            AdengListView ctrl = sender as AdengListView;
            if (ctrl == null)
            {
                return;
            }
            if (ctrl.SelectedItems == null || ctrl.SelectedItems.Count < 1)
            {
                this.btnOrderCancel.Enabled = false;
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

        #region 외부이벤트수신
        public void OnNotifyOrderResponseUpdated(object sender, OrderResponseEventArgs e)
        {
            try
            {
                MethodInvoker invoker = delegate()
                {
                    System.Console.WriteLine("[RecentlyOrderHistoryForm] OnNotifyOrderResponseUpdated ( 응답 수신 )");

                    if (this.orderDetailForm == null || this.orderDetailForm.IsDisposed)
                    {
                        System.Console.WriteLine("[RecentlyOrderHistoryForm] OnNotifyOrderResponseUpdated ( 상세 표시 중이 아님. )");
                        return;
                    }
                    if (this.lvOrderList.SelectedItems == null || this.lvOrderList.SelectedItems.Count < 1)
                    {
                        System.Console.WriteLine("[RecentlyOrderHistoryForm] OnNotifyOrderResponseUpdated ( 선택된 아이템이 없음 )");
                        return;
                    }
                    OrderRecord selectedRecord = this.lvOrderList.SelectedItems[0].Tag as OrderRecord;
                    if (selectedRecord == null)
                    {
                        System.Console.WriteLine("[RecentlyOrderHistoryForm] OnNotifyOrderResponseUpdated ( 선택된 아이템의 데이터 변환 오류 )");
                        return;
                    }
                    if (e.ResponseInfo == null || e.ResponseInfo.Count < 1 || selectedRecord.CAPID != e.OrderRecordID)
                    {
                        System.Console.WriteLine("[RecentlyOrderHistoryForm] OnNotifyOrderResponseUpdated ( 상세 표시 중인 레코드와 응답 레코드가 다름 )");
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
                FileLogManager.GetInstance().WriteLog("[RecentlyOrderHistoryForm] OnNotifyOrderResponseUpdated( " + ex.ToString() + " )");
            }
        }
        #endregion

    }
    /// <summary>
    /// 발령 취소 요청 정보 전달 이벤트 아규먼트 클래스
    /// </summary>
    public class RequestCancelOrderEventArgs : EventArgs
    {
        private string orderCAPID = string.Empty;
        public string OrderCAPID
        {
          get { return orderCAPID; }
          set { orderCAPID = value; }
        }
        private OrderRecord requestedOrder;
        public OrderRecord RequestedOrder
        {
            get { return requestedOrder; }
            set { requestedOrder = value; }
        }

        public RequestCancelOrderEventArgs()
        {
        }
        public RequestCancelOrderEventArgs(string capID, OrderRecord requestedRecord)
        {
            this.orderCAPID = capID;
            this.requestedOrder = requestedRecord;
        }
    }
}
