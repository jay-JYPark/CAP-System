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
using CAPLib;

namespace StdWarningInstallation
{
    public partial class OrderDeTailForm : Form
    {
        private OrderRecord currentOrderRecord;
        private List<OrderResponseProfile> currentResponseList;

        public OrderDeTailForm(OrderRecord orderRecord, List<OrderResponseProfile> responseInfoList)
        {
            InitializeComponent();

            this.currentOrderRecord = orderRecord;
            this.currentResponseList = responseInfoList;

            InitializeSelectedOrderList();
            InitializeOrderDetailList();
            InitializeOrderResponseList();
        }

        #region 초기화
        private void InitializeSelectedOrderList()
        {
            this.lvSelectedOrder.Clear();

            AdengColumnHeader header = new AdengColumnHeader();
            header.Text = "결과";
            header.Width = 50;
            this.lvSelectedOrder.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Text = "발령 시각";
            header.Width = 180;
            this.lvSelectedOrder.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Text = "발령지 구분";
            header.Width = 150;
            this.lvSelectedOrder.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Text = "모드";
            header.Width = 50;
            this.lvSelectedOrder.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Text = "이벤트 종류";
            header.Width = 150;
            this.lvSelectedOrder.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Text = "비고";
            header.Width = 150;
            this.lvSelectedOrder.Columns.Add(header);
        }
        private void InitializeOrderDetailList()
        {
            this.dgvOrderDetail.Rows.Clear();
            this.dgvOrderDetail.Columns.Clear();

            DataGridViewColumn column = new DataGridViewColumn();
            column.Name = "항목";
            column.Width = 80;
            column.CellTemplate = new DataGridViewTextBoxCell();
            this.dgvOrderDetail.Columns.Add(column);
            column = new DataGridViewColumn();
            column.Name = "내용";
            column.Width = 276;
            column.CellTemplate = new DataGridViewTextBoxCell();
            this.dgvOrderDetail.Columns.Add(column);
        }
        private void InitializeOrderResponseList()
        {
            this.lvOrderResponse.Clear();

            AdengColumnHeader header = new AdengColumnHeader();
            header.Text = "";
            header.Width = 30;
            this.lvOrderResponse.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Text = "응답 시스템 이름";
            header.Width = 200;
            this.lvOrderResponse.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Text = "리소스 인증";
            header.Width = 80;
            this.lvOrderResponse.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Text = "경보 서비스";
            header.Width = 110;
            this.lvOrderResponse.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Text = "수행 결과 (전체/성공/실패)";
            header.Width = 160;
            this.lvOrderResponse.Columns.Add(header);
        }
        #endregion

        #region 이벤트핸들러
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            UpdateSelectedOrderList();
            UpdateOrderDetailList();
            UpdateResponseList();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 화면갱신
        private void UpdateSelectedOrderList()
        {
            this.lvSelectedOrder.Items.Clear();

            if (this.lvSelectedOrder.Columns == null)
            {
                return;
            }
            if (this.currentOrderRecord == null)
            {
                return;
            }

            // 발령 결과
            AdengListViewItem item = this.lvSelectedOrder.Items.Add(" ");
            // 발령 시각
            item.SubItems.Add(this.currentOrderRecord.OrderedTime.ToString());
            // 발령지 구분
            item.SubItems.Add(BasisData.GetDisplayStringLocationKindName(this.currentOrderRecord.LocationKind));
            // 발령 모드
            item.SubItems.Add(BasisData.GetDisplayStringOrderMode(this.currentOrderRecord.OrderMode));
            // 재난 종류
            item.SubItems.Add(BasisData.GetDisplayStringDisasterKindName(this.currentOrderRecord.DisasterKindCode));
            // 비고(참조 정보)
            if (!string.IsNullOrEmpty(currentOrderRecord.RefRecordID))
            {
                if (currentOrderRecord.RefType == OrderReferenceType.Cancel)
                {
                    item.SubItems.Add("[발령취소] " + currentOrderRecord.RefRecordID);
                }
                else
                {
                    item.SubItems.Add(currentOrderRecord.RefRecordID);
                }
            }

            item.Name = this.currentOrderRecord.CAPID;
            item.Tag = this.currentOrderRecord;
        }
        private void UpdateOrderDetailList()
        {
            this.dgvOrderDetail.Rows.Clear();

            if (this.dgvOrderDetail.Columns == null)
            {
                return;
            }

            this.dgvOrderDetail.Rows.Add("발령 아이디", this.currentOrderRecord.CAPID.ToString());

            this.dgvOrderDetail.Rows.Add("발령 시각", this.currentOrderRecord.OrderedTime.ToString());

            CAP capMsg = new CAP(this.currentOrderRecord.CapText);
            if (capMsg == null)
            {
                return;
            }
            this.dgvOrderDetail.Rows.Add("발령원", capMsg.Source);
            this.dgvOrderDetail.Rows.Add("모드", BasisData.GetDisplayStringOrderMode(this.currentOrderRecord.OrderMode));
            this.dgvOrderDetail.Rows.Add("이벤트 종류", BasisData.GetDisplayStringDisasterKindName(this.currentOrderRecord.DisasterKindCode));

            CAPHelper helper = new CAPHelper();
            string targets = helper.ExtractTargetNamesFromCAP(capMsg);
            if (!string.IsNullOrEmpty(targets))
            {
                this.dgvOrderDetail.Rows.Add("대상", targets);
            }

            List<SASKind> targetKinds = helper.ExtractTargetKindsFromCAP(capMsg);
            if (targetKinds != null && targetKinds.Count > 0)
            {
                bool isFirst = true;
                StringBuilder builder = new StringBuilder();
                foreach (SASKind kind in targetKinds)
                {
                    if (isFirst)
                    {
                        isFirst = false;
                        builder.Append(kind.Name);
                    }
                    else
                    {
                        builder.Append("," + kind.Name);
                    }
                }

                if (builder.Length > 0)
                {
                    this.dgvOrderDetail.Rows.Add("시스템 종류", builder.ToString());
                }
            }
            else
            {
                this.dgvOrderDetail.Rows.Add("시스템 종류", "전체");
            }

            string languages = helper.GetLanguageNamesFromCAP(capMsg);
            if (!string.IsNullOrEmpty(languages))
            {
                this.dgvOrderDetail.Rows.Add("언어", languages);
            }

            List<CAPParameterMsgInfo> msgInfo = helper.ExtractMsgTextFromCAP(capMsg, BasisData.DEFAULT_LANGUAGECODE);
            if (msgInfo != null && msgInfo.Count > 0)
            {
                foreach (CAPParameterMsgInfo info in msgInfo)
                {
                    if (info.ValueName.ToUpper().Contains("TTS"))
                    {
                        int height = this.dgvOrderDetail.Rows.GetRowsHeight(DataGridViewElementStates.Displayed);
                        int remainHeight = this.dgvOrderDetail.Height - height;

                        int index = this.dgvOrderDetail.Rows.Add("문안[TTS]", info.Value);
                        break;
                    }
                }

                foreach (CAPParameterMsgInfo info in msgInfo)
                {
                    if (info.ValueName.ToUpper().Contains("CBS"))
                    {
                        int height = this.dgvOrderDetail.Rows.GetRowsHeight(DataGridViewElementStates.Displayed);
                        int remainHeight = this.dgvOrderDetail.Height - height;

                        int index = this.dgvOrderDetail.Rows.Add("문안[CBS]", info.Value);
                        break;
                    }
                }

                foreach (CAPParameterMsgInfo info in msgInfo)
                {
                    if (info.ValueName.ToUpper().Contains("BOARD"))
                    {
                        int height = this.dgvOrderDetail.Rows.GetRowsHeight(DataGridViewElementStates.Displayed);
                        height += this.dgvOrderDetail.ColumnHeadersHeight + 2;
                        int remainHeight = this.dgvOrderDetail.Height - height;

                        int index = this.dgvOrderDetail.Rows.Add("문안[Board]", info.Value);
                        if (remainHeight > 0)
                        {
                            //this.dgvOrderDetail.Rows[index].MinimumHeight = remainHeight;
                        }
                        break;
                    }
                }

                foreach (CAPParameterMsgInfo info in msgInfo)
                {
                    if (info.ValueName.ToUpper().Contains("DMB"))
                    {
                        int height = this.dgvOrderDetail.Rows.GetRowsHeight(DataGridViewElementStates.Displayed);
                        height += this.dgvOrderDetail.ColumnHeadersHeight + 2;
                        int remainHeight = this.dgvOrderDetail.Height - height;

                        int index = this.dgvOrderDetail.Rows.Add("문안[DMB]", info.Value);
                        if (remainHeight > 0)
                        {
                            //this.dgvOrderDetail.Rows[index].MinimumHeight = remainHeight;
                        }
                        break;
                    }
                }
            }
            else
            {
                this.dgvOrderDetail.Rows.Add("문안[TTS]");
                this.dgvOrderDetail.Rows.Add("문안[CBS]");
                this.dgvOrderDetail.Rows.Add("문안[Board]");
                this.dgvOrderDetail.Rows.Add("문안[DMB]");
            }
        }
        private void UpdateResponseList()
        {
            this.lvOrderResponse.Items.Clear();

            if (this.lvOrderResponse.Columns == null)
            {
                return;
            }
            if (this.currentResponseList == null)
            {
                return;
            }

            Dictionary<string, SASProfile> sasProfiles = new Dictionary<string, SASProfile>();
            List<SASProfile> sasList = DBManager.GetInstance().QuerySASInfo();
            if (sasList != null)
            {
                foreach (SASProfile profile in sasList)
                {
                    sasProfiles.Add(profile.ID, profile);
                }
            }

            bool isAllOK = true;
            bool isAllNG = true;
            foreach (OrderResponseProfile response in this.currentResponseList)
            {
                if (response == null)
                {
                    continue;
                }

                bool existOK = false;
                bool existNG = false;

                // 발령 결과
                AdengListViewItem item = this.lvOrderResponse.Items.Add(" ");
                item.ForeColor = Color.Gray;
                item.SubItems[0].Text = "?";
                // 응답 시스템 이름
                if (sasProfiles.ContainsKey(response.SenderID))
                {
                    item.SubItems.Add(sasProfiles[response.SenderID].Name);
                }
                else
                {
                    item.SubItems.Add(response.SenderID);
                }

                if (string.IsNullOrEmpty(response.CapMsg))
                {
                    // 리소스 인증
                    item.SubItems.Add("");
                    // 경보 서비스
                    item.SubItems.Add("");
                    // 수행 결과
                    item.SubItems.Add("");
                }
                else
                {
                    CAP capMsg = new CAP(response.CapMsg);
                    if (capMsg == null)
                    {
                        continue;
                    }
                    if (capMsg == null || capMsg.MessageType == null ||
                        (capMsg.MessageType.Value != MsgType.Ack && capMsg.MessageType.Value != MsgType.Error))
                    {
                        MessageBox.Show("올바르지 않는 데이터가 존재합니다.", "데이터 오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    }
                    if (string.IsNullOrEmpty(capMsg.Note))
                    {
                        continue;
                    }
                    string[] seperator = { " ", };
                    string[] resAll = capMsg.Note.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
                    if (resAll == null || resAll.Count() < 1)
                    {
                        continue;
                    }
                    string responseCodeStr = resAll[0];
                    int responseCode = -1;
                    if (!int.TryParse(responseCodeStr, out responseCode))
                    {
                        responseCode = -1;
                    }

                    // 리소스 인증
                    if (responseCode < (int)OrderResponseCodes.SasResourceCerciticationOK)
                    {
                        item.SubItems.Add("");
                    }
                    else if (responseCode >= (int)OrderResponseCodes.SasResourceCerciticationOK &&
                        responseCode != (int)OrderResponseCodes.SasResourceCerciticationNG)
                    {
                        item.SubItems.Add("성공");
                        existOK = true;
                    }
                    else
                    {
                        item.SubItems.Add("실패");
                        existNG = true;
                    }

                    // 경보 서비스
                    if (responseCode < (int)OrderResponseCodes.SasAlertServiceOK)
                    {
                        item.SubItems.Add("");
                    }
                    else if (responseCode >= (int)OrderResponseCodes.SasAlertServiceOK &&
                        responseCode != (int)OrderResponseCodes.SasAlertServiceError)
                    {
                        item.SubItems.Add("성공");
                        existOK = true;
                    }
                    else
                    {
                        item.SubItems.Add("실패");
                        existNG = true;
                    }

                    // 수행 결과 (전체/성공/실패)
                    if (resAll.Count() > 1 && !string.IsNullOrEmpty(resAll[1]))
                    {
                        string res = resAll[1].Replace(",", "/");
                        item.SubItems.Add(res);
                    }


                    if (existNG)
                    {
                        // 하나라도 실패가 있으면 실패로 표시
                        item.ForeColor = Color.Red;
                        item.SubItems[0].Text = "X";

                        isAllOK = false;
                    }
                    else if (existOK)
                    {
                        if (responseCode == 800)
                        {
                            // 성공
                            item.ForeColor = Color.Black;
                            item.SubItems[0].Text = "O";

                            isAllNG = false;
                        }
                        else
                        {
                            // 하나라도 성공이 있으면 응답중
                            item.ForeColor = Color.Black;
                            item.SubItems[0].Text = "△";
                        }
                    }
                    else
                    {
                        // 아무 것도 없으면 무응답
                        item.ForeColor = Color.Gray;
                        item.SubItems[0].Text = "?";
                    }
                }
            }

            if (isAllOK)
            {
                this.lvSelectedOrder.Items[0].SubItems[0].Text = "O";
            }
            else if (isAllNG)
            {
                this.lvSelectedOrder.Items[0].SubItems[0].Text = "X";
            }
            else
            {
                this.lvSelectedOrder.Items[0].SubItems[0].Text = "△";
            }
        }
        /// <summary>
        /// [외부공개용] 응답 결과 데이터 변경 시 호출되어 화면을 갱신함.
        /// </summary>
        public void UpdateOrderResponseInfo(List<OrderResponseProfile> responseInfo)
        {
            this.currentResponseList = responseInfo;

            UpdateResponseList();
        }
        #endregion
    }
}
