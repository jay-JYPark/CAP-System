using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Adeng.Framework.Ctrl;
using CAPLib;
using StdWarningInstallation.DataClass;

namespace StdWarningInstallation
{
    public partial class ClearAlertWaitingListForm : Form
    {
        public event EventHandler<UpdateClearAlertStateEventArgs> NotifyUpdateClearAlertState;

        private List<OrderRecord> currentWaitingList = null;


        /// <summary>
        /// 생성자.
        /// </summary>
        /// <param name="orderList"></param>
        public ClearAlertWaitingListForm(List<OrderRecord> orderList)
        {
            InitializeComponent();

            this.currentWaitingList = orderList;

            InitReportList();
        }
        /// <summary>
        /// 폼 로딩.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadForm(object sender, EventArgs e)
        {
            UpdateReportList();
        }

        /// <summary>
        /// 리스트 초기화.
        /// </summary>
        private void InitReportList()
        {
            AdengColumnHeader header = new AdengColumnHeader();
            header.Name = "No";
            header.Text = "";
            header.Width = 30;
            this.lvClearWaitingList.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Name = "RecordId";
            header.Text = "ID";
            header.Width = 170;
            this.lvClearWaitingList.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Name = "ReportedTime";
            header.Text = "발령 시각";
            header.Width = 170;
            this.lvClearWaitingList.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Name = "Mode";
            header.Text = "모드";
            header.Width = 40;
            this.lvClearWaitingList.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Name = "Disaster";
            header.Text = "재난 종류";
            header.Width = 100;
            this.lvClearWaitingList.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Name = "Target";
            header.Text = "발령 대상";
            header.Width = 300;
            this.lvClearWaitingList.Columns.Add(header);
            //header = new AdengColumnHeader();
            //header.Name = "Contents";
            //header.Text = "비고";
            //header.Width = 200;
            //this.lvClearWaitingList.Columns.Add(header);
        }
        /// <summary>
        /// 리스트 갱신.
        /// </summary>
        private void UpdateReportList()
        {
            try
            {
                this.lvClearWaitingList.Items.Clear();
                if (this.currentWaitingList == null || this.currentWaitingList.Count <= 0)
                {
                    return;
                }

                CAPHelper helper = new CAPHelper();
                foreach (OrderRecord record in this.currentWaitingList)
                {
                    // 번호
                    int itemNo = this.lvClearWaitingList.Items.Count + 1;
                    AdengListViewItem newItem = this.lvClearWaitingList.Items.Add(itemNo.ToString());
                    // 발령 아이디
                    newItem.SubItems.Add(record.CAPID);
                    // 발령 시각
                    newItem.SubItems.Add(record.OrderedTime.ToString());
                    // 발령 모드
                    newItem.SubItems.Add(BasisData.GetDisplayStringOrderMode(record.OrderMode));
                    // 재난종류
                    newItem.SubItems.Add(BasisData.GetDisplayStringDisasterKindName(record.DisasterKindCode));

                    CAP msg = new CAP(record.CapText);
                    if (msg == null)
                    {
                        continue;
                    }
                    // 발령 대상
                    string targetNames = helper.ExtractTargetNamesFromCAP(msg);
                    if (targetNames == null)
                    {
                        targetNames = "";
                    }
                    newItem.SubItems.Add(targetNames);

                    newItem.Name = record.CAPID;
                    newItem.Tag = record;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[ClearAlertWaitingListForm] UpdateReportList( Exception: \n" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[ClearAlertWaitingListForm] UpdateReportList( Exception=[" + ex.ToString() + "] )");
            }
        }

        /// <summary>
        /// [선택 항목 삭제] 버튼 클릭.
        /// 명시적인 경보 해제가 불필요한 경우, 항목 삭제를 통해 제외 시킨다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (this.lvClearWaitingList.SelectedItems == null || this.lvClearWaitingList.SelectedItems.Count <= 0)
            {
                return;
            }
            string targetID = this.lvClearWaitingList.SelectedItems[0].Name;
            this.lvClearWaitingList.Items.Remove(this.lvClearWaitingList.SelectedItems[0]);

            if (targetID == null)
            {
                return;
            }

            // 메인에 전달
            if (this.NotifyUpdateClearAlertState != null)
            {
                this.NotifyUpdateClearAlertState(this, new UpdateClearAlertStateEventArgs(targetID, ClearAlertState.Exclude, null));
            }

            // 현 윈도우를 종료
            if (this.lvClearWaitingList.Items.Count <= 0)
            {
                this.Close();
            }
        }
        /// <summary>
        /// [취소] 버튼 클릭.
        /// 윈도우를 닫는다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// [경보 해제] 버튼 클릭.
        /// 명시적으로 경보 상황을 해제하는 발령을 내린다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearAlert_Click(object sender, EventArgs e)
        {
            if (this.lvClearWaitingList.SelectedItems == null || this.lvClearWaitingList.SelectedItems.Count <= 0)
            {
                return;
            }
            string targetID = this.lvClearWaitingList.SelectedItems[0].Name;
            OrderRecord targetRecord = this.lvClearWaitingList.SelectedItems[0].Tag as OrderRecord;
            if (targetID == null || targetRecord == null)
            {
                return;
            }

            // 메인에 전달
            if (this.NotifyUpdateClearAlertState != null)
            {
                this.NotifyUpdateClearAlertState(this, new UpdateClearAlertStateEventArgs(targetID, ClearAlertState.Clear, targetRecord));

                this.lvClearWaitingList.Items.Remove(this.lvClearWaitingList.SelectedItems[0]);
            }

            // 현 윈도우를 종료
            this.Close();
        }

        /// <summary>
        /// [경보 상황 해제 대기 목록] 더블 클릭.
        /// 기발령 정보의 상세 내용을 표시한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvClearWaitingList_DoubleClick(object sender, EventArgs e)
        {
            AdengListView view = sender as AdengListView;
            if (view == null || view.SelectedItems == null || view.SelectedItems.Count <= 0)
            {
                return;
            }
            AdengListViewItem item = view.SelectedItems[0];
            if (item == null)
            {
                return;
            }

            //OrderRecordDetails details = new OrderRecordDetails();
            //Size defaultSize = details.Size;
            //details.Left = this.Right;
            //details.Top = this.Top;
            //details.Size = defaultSize;
            //details.ShowDialog(this);
        }
        /// <summary>
        /// [경보 상황 해제 대기 목록] 선택 변경.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvClearWaitingList_ItemSelectionChanged(object sender, AdengListViewItemSelectionChangedEventArgs e)
        {
            if (this.lvClearWaitingList.SelectedItems == null ||
                this.lvClearWaitingList.SelectedItems.Count <= 0)
            {
                this.btnDelete.Enabled = false;
                this.btnClearAlert.Enabled = false;
            }
            else
            {
                this.btnDelete.Enabled = true;
                this.btnClearAlert.Enabled = true;
            }
        }
    }

    /// <summary>
    /// 발령 취소 요청 정보 전달 이벤트 아규먼트 클래스
    /// </summary>
    public class UpdateClearAlertStateEventArgs : EventArgs
    {
        private string targetCAPID = string.Empty;
        public string TargetCAPID
        {
          get { return targetCAPID; }
          set { targetCAPID = value; }
        }
        private ClearAlertState clearState;
        public ClearAlertState ClearState
        {
            get { return clearState; }
            set { clearState = value; }
        }
        private OrderRecord requestedOrder;
        public OrderRecord RequestedOrder
        {
            get { return requestedOrder; }
            set { requestedOrder = value; }
        }

        public UpdateClearAlertStateEventArgs()
        {
        }
        public UpdateClearAlertStateEventArgs(string capID, ClearAlertState state, OrderRecord requestedRecord)
        {
            this.targetCAPID = capID;
            this.clearState = state;
            this.requestedOrder = requestedRecord;
        }
    }
}
