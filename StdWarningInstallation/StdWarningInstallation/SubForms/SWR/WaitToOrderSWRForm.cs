using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Adeng.Framework.Ctrl;
//using CAPLib;
using StdWarningInstallation.DataClass;

namespace StdWarningInstallation
{
    public partial class WaitToOrderSWRForm : Form
    {
        public event EventHandler<UpdateSWRAssociationStateEventArgs> NotifyUpdateSWRAssociationState;
        public event EventHandler<UpdateSWRAreaVisibleEventArgs> NotifyUpdateSWRAreaVisible;

        private List<SWRProfile> currentWaitingList = null;
        private int previousSelectedItemIndex = -1;

        /// <summary>
        /// 생성자.
        /// </summary>
        /// <param name="waitingList"></param>
        public WaitToOrderSWRForm(List<SWRProfile> waitingList)
        {
            InitializeComponent();

            if (waitingList == null)
            {
                MessageBox.Show("발령 대기 중인 기상특보가 없습니다.", "미발령 기상특보 목록", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Dispose();
                return;
            }

            if (this.currentWaitingList == null)
            {
                this.currentWaitingList = new List<SWRProfile>();
            }
            this.currentWaitingList.Clear();

            foreach (SWRProfile profile in waitingList)
            {
                SWRProfile copy = new SWRProfile();
                copy.DeepCopyFrom(profile);

                this.currentWaitingList.Add(copy);
            }

            InitReportList();
        }
        public void UpdateSWRList(List<SWRProfile> waitingList)
        {
            try
            {
                MethodInvoker invoker = delegate()
                {
                    // 현재 선택된 아이템을 백업
                    string selectedItemName = string.Empty;
                    if (this.lvWaitToOrderSWRList.SelectedItems != null && this.lvWaitToOrderSWRList.SelectedItems.Count > 0)
                    {
                        selectedItemName = this.lvWaitToOrderSWRList.SelectedItems[0].Text;
                    }

                    // 로컬 리스트 클리어
                    if (this.currentWaitingList == null)
                    {
                        this.currentWaitingList = new List<SWRProfile>();
                    }
                    this.currentWaitingList.Clear();

                    // 새 데이터로 변경
                    foreach (SWRProfile profile in waitingList)
                    {
                        SWRProfile copy = new SWRProfile();
                        copy.DeepCopyFrom(profile);

                        this.currentWaitingList.Add(copy);
                    }

                    // 리스트 갱신
                    UpdateReportList();

                    // 원래 선택했던 아이템 재선택 처리
                    if (this.lvWaitToOrderSWRList.Items != null && !string.IsNullOrEmpty(selectedItemName))
                    {
                        if (this.lvWaitToOrderSWRList.Items.ContainsKey(selectedItemName))
                        {
                            this.lvWaitToOrderSWRList.Items[selectedItemName].Selected = true;
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
                System.Console.WriteLine("[WaitToOrderForm] UpdateSWRList( Exception: \n" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[WaitToOrderForm] UpdateSWRList( Exception=[" + ex.ToString() + "] )");
            }
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
            header.Name = "ReportId";
            header.Text = "ID";
            header.Width = 74;
            this.lvWaitToOrderSWRList.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Name = "AnnounceTime";
            header.Text = "발표 시각";
            header.Width = 160;
            this.lvWaitToOrderSWRList.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Name = "AreaName";
            header.Text = "구역";
            header.Width = 210;
            this.lvWaitToOrderSWRList.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Name = "WarnKind";
            header.Text = "종류";
            header.Width = 100;
            this.lvWaitToOrderSWRList.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Name = "CommandCode";
            header.Text = "코드";
            header.Width = 90;
            this.lvWaitToOrderSWRList.Columns.Add(header);
        }
        /// <summary>
        /// 리스트 갱신.
        /// </summary>
        private void UpdateReportList()
        {
            try
            {
                this.lvWaitToOrderSWRList.Items.Clear();
                if (this.currentWaitingList == null || this.currentWaitingList.Count <= 0)
                {
                    return;
                }

                CAPHelper helper = new CAPHelper();
                foreach (SWRProfile profile in this.currentWaitingList)
                {
                    if (profile == null)
                    {
                        continue;
                    }
                    SWRWarningItemProfile warningItem = profile.GetWarningItemProfile();
                    if (warningItem == null)
                    {
                        continue;
                    }

                    // ReportId: 특보 아이디
                    AdengListViewItem newItem = this.lvWaitToOrderSWRList.Items.Add(warningItem.ReportID);

                    // AnnounceTime: 발표 시각
                    newItem.SubItems.Add(warningItem.AnnounceTime.ToString());

                    // AreaName: 특보 구역
                    string targetAreaNames = profile.GetTargetAreaNames();
                    if (!string.IsNullOrEmpty(targetAreaNames))
                    {
                        newItem.SubItems.Add(targetAreaNames);
                    }
                    else
                    {
                        newItem.SubItems.Add("Unknown(" + profile.TargetAreas + ")");
                    }

                    // WarnKind: 특보 종류
                    string kind = BasisData.FindSWRKindStringByKindCode(profile.WarnKindCode);
                    string stress = BasisData.FindSWRStressStringByStressCode(profile.WarnStressCode);
                    newItem.SubItems.Add(kind + " " + stress);

                    // CommandCode: 발표 코드
                    string command = BasisData.FindSWRCommandStringByCommandCode(profile.CommandCode);
                    newItem.SubItems.Add(command);

                    newItem.Name = profile.ID;
                    newItem.Tag = profile;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[WaitToOrderForm] UpdateReportList( Exception: \n" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[WaitToOrderForm] UpdateReportList( Exception=[" + ex.ToString() + "] )");
            }
        }

        /// <summary>
        /// 리스트 아이템 선택 변경.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvWaitToOrderSWRList_SelectedIndexChanged(object sender, Adeng.Framework.Ctrl.AdengEventArgs e)
        {
            try
            {
                if (this.lvWaitToOrderSWRList.SelectedItems == null ||
                    this.lvWaitToOrderSWRList.SelectedItems.Count <= 0)
                {
                    this.btnDelete.Enabled = false;
                    this.btnOrder.Enabled = false;

                    return;
                }

                // 어째서인지 동일한 아이템을 재선택해도 이 이벤트가 발생한다.
                if (this.previousSelectedItemIndex == this.lvWaitToOrderSWRList.SelectedItems[0].Index)
                {
                    return;
                }
                this.previousSelectedItemIndex = this.lvWaitToOrderSWRList.SelectedItems[0].Index;

                this.btnDelete.Enabled = true;
                this.btnOrder.Enabled = true;

                SWRProfile profile = this.lvWaitToOrderSWRList.SelectedItems[0].Tag as SWRProfile;
                if (profile == null)
                {
                    return;
                }

                if (this.NotifyUpdateSWRAreaVisible != null)
                {
                    this.NotifyUpdateSWRAreaVisible(this, new UpdateSWRAreaVisibleEventArgs(true, profile));
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[WaitToOrderForm] lvWaitToOrderSWRList_SelectedIndexChanged( Exception: \n" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[WaitToOrderForm] lvWaitToOrderSWRList_SelectedIndexChanged( Exception=[" + ex.ToString() + "] )");
            }
        }
        /// <summary>
        /// 리스트 아이템 더블클릭.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvWaitToOrderSWRList_DoubleClick(object sender, EventArgs e)
        {
            try
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

                SWRProfile profile = item.Tag as SWRProfile;
                if (profile == null)
                {
                    return;
                }

                SWRWarningItemDetailForm detailForm = new SWRWarningItemDetailForm(profile);
                detailForm.ShowDialog(this);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[WaitToOrderForm] lvWaitToOrderSWRList_DoubleClick( Exception: \n" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[WaitToOrderForm] lvWaitToOrderSWRList_DoubleClick( Exception=[" + ex.ToString() + "] )");
            }
        }

        /// <summary>
        /// [선택 항목 연계 제외] 버튼 클릭.
        /// 연계 발령이 불필요한 경우, 항목 삭제를 통해 제외 시킨다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.lvWaitToOrderSWRList.SelectedItems == null || this.lvWaitToOrderSWRList.SelectedItems.Count <= 0)
                {
                    return;
                }
                string targetID = this.lvWaitToOrderSWRList.SelectedItems[0].Name;
                this.lvWaitToOrderSWRList.Items.Remove(this.lvWaitToOrderSWRList.SelectedItems[0]);

                if (targetID == null)
                {
                    return;
                }

                // 메인에 전달
                if (this.NotifyUpdateSWRAssociationState != null)
                {
                    this.NotifyUpdateSWRAssociationState(this, new UpdateSWRAssociationStateEventArgs(targetID, SWRAssociationStateCode.Exclude, null));
                }

                // 현 윈도우를 종료
                if (this.lvWaitToOrderSWRList.Items.Count <= 0)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[WaitToOrderForm] btnDelete_Click( Exception: \n" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[WaitToOrderForm] btnDelete_Click( Exception=[" + ex.ToString() + "] )");
            }
        }
        /// <summary>
        /// [닫기] 버튼 클릭.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (this.NotifyUpdateSWRAreaVisible != null)
            {
                SWRProfile profile = null;
                if (this.lvWaitToOrderSWRList.SelectedItems != null && this.lvWaitToOrderSWRList.SelectedItems.Count > 0)
                {
                    profile = this.lvWaitToOrderSWRList.SelectedItems[0].Tag as SWRProfile;
                }
                this.NotifyUpdateSWRAreaVisible(this, new UpdateSWRAreaVisibleEventArgs(false, profile));
            }
            this.Close();
        }
        /// <summary>
        /// [경보 발령] 버튼 클릭.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.lvWaitToOrderSWRList.SelectedItems == null || this.lvWaitToOrderSWRList.SelectedItems.Count <= 0)
                {
                    return;
                }
                string targetID = this.lvWaitToOrderSWRList.SelectedItems[0].Name;
                SWRProfile targetRecord = this.lvWaitToOrderSWRList.SelectedItems[0].Tag as SWRProfile;
                if (targetID == null || targetRecord == null)
                {
                    return;
                }

                // 메인에 전달
                if (this.NotifyUpdateSWRAssociationState != null)
                {
                    this.Visible = false;
                    this.NotifyUpdateSWRAssociationState(this, new UpdateSWRAssociationStateEventArgs(targetID, SWRAssociationStateCode.Order, targetRecord));
                    this.Close();
                }
                else
                {
                    DialogResult result = MessageBox.Show("발령 처리에서 오류가 발생하였습니다. \n errorCode=[-1]", "발령 실패", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[WaitToOrderForm] btnOrder_Click( Exception: \n" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[WaitToOrderForm] btnOrder_Click( Exception=[" + ex.ToString() + "] )");
            }
        }
    }

    /// <summary>
    /// 기상특보 연계발령 정보 전달 이벤트 아규먼트 클래스
    /// </summary>
    public class UpdateSWRAssociationStateEventArgs : EventArgs
    {
        private string reportID = string.Empty;
        public string TargetReportID
        {
            get { return reportID; }
            set { reportID = value; }
        }
        private SWRAssociationStateCode associationState;
        public SWRAssociationStateCode AssociationState
        {
            get { return associationState; }
            set { associationState = value; }
        }
        private SWRProfile requestedReport;
        public SWRProfile RequestedReport
        {
            get { return requestedReport; }
            set { requestedReport = value; }
        }

        public UpdateSWRAssociationStateEventArgs()
        {
        }
        public UpdateSWRAssociationStateEventArgs(string reportID, SWRAssociationStateCode associationState, SWRProfile requestedReport)
        {
            this.reportID = reportID;
            this.associationState = associationState;
            this.requestedReport = requestedReport;
        }
    }

    /// <summary>
    /// 기상특보 특부구역 표시 요청 이벤트 아규먼트 클래스
    /// </summary>
    public class UpdateSWRAreaVisibleEventArgs : EventArgs
    {
        private bool isVisible;
        public bool IsVisible
        {
          get { return isVisible; }
          set { isVisible = value; }
        }
        private SWRProfile requestedReport;
        public SWRProfile RequestedReport
        {
            get { return requestedReport; }
            set { requestedReport = value; }
        }

        public UpdateSWRAreaVisibleEventArgs()
        {
        }
        public UpdateSWRAreaVisibleEventArgs(bool isVisible, SWRProfile requestedReport)
        {
            this.isVisible = isVisible;
            this.requestedReport = requestedReport;
        }
    }
}
