using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using StdWarningInstallation.DataClass;

namespace StdWarningInstallation
{
    public partial class OrderForm : Form
    {
        public event EventHandler<OrderPrepareEventArgs> NotifyOrderProvisionResult;

        private OrderProvisionInfo currentOrderInfo = new OrderProvisionInfo();

        public OrderForm()
        {
            InitializeComponent();
        }

        public OrderForm(ref OrderProvisionInfo info)
        {
            InitializeComponent();

            SetOrderInfo(info);
        }

        public void SetOrderInfo(OrderProvisionInfo info)
        {
            if (info == null)
            {
                return;
            }
            this.currentOrderInfo = info;

            CheckDefaultSet();
        }

        private void CheckDefaultSet()
        {
            if (this.currentOrderInfo == null)
            {
                this.currentOrderInfo = new OrderProvisionInfo();
            }

            // 발령 모드
            if (this.currentOrderInfo.Mode == null)
            {
                //this.currentOrderInfo.Mode = BasisData.FindOrderModeInfoByCode(CAPLib.StatusType.Actual);
                this.currentOrderInfo.Mode = BasisData.FindOrderModeInfoByCode(CAPLib.StatusType.Test);
            }

            // 재난 정보
            if (this.currentOrderInfo.Disaster == null)
            {
                this.currentOrderInfo.Disaster = new Disaster();
            }
            if (this.currentOrderInfo.Disaster.Category == null)
            {
                this.currentOrderInfo.Disaster.Category = new DisasterCategory();
            }
            if (this.currentOrderInfo.Disaster.Kind == null)
            {
                this.currentOrderInfo.Disaster.Kind = new DisasterKind();
            }

            // 문안 정보
            if (this.currentOrderInfo.MsgTextInfo == null)
            {
                this.currentOrderInfo.MsgTextInfo = new SendingMsgTextInfo();
            }
            if (this.currentOrderInfo.MsgTextInfo.CurrentTransmitMsgText == null)
            {
                this.currentOrderInfo.MsgTextInfo.CurrentTransmitMsgText = new List<MsgText>();
            }
            if (this.currentOrderInfo.MsgTextInfo.OriginalTransmitMsgText == null)
            {
                this.currentOrderInfo.MsgTextInfo.OriginalTransmitMsgText = new List<MsgText>();
            }
            if (this.currentOrderInfo.MsgTextInfo.SelectedLanguages == null)
            {
                this.currentOrderInfo.MsgTextInfo.SelectedLanguages = new List<MsgTextDisplayLanguageKind>();
            }
            if (this.currentOrderInfo.MsgTextInfo.SelectedLanguages.Count < 1)
            {
                MsgTextDisplayLanguageKind defaultLanguage = BasisData.FindMsgTextLanguageInfoByCode(BasisData.DEFAULT_LANGUAGECODE);
                this.currentOrderInfo.MsgTextInfo.SelectedLanguages.Add(defaultLanguage);

                foreach (MsgTextDisplayLanguageKind language in BasisData.MsgTextLanguageKind)
                {
                    if (language.LanguageCode == BasisData.DEFAULT_LANGUAGECODE)
                    {
                        continue;
                    }
                    if (language.IsDefault)
                    {
                        MsgTextDisplayLanguageKind copy = new MsgTextDisplayLanguageKind();
                        copy.DeepCopyFrom(language);
                        this.currentOrderInfo.MsgTextInfo.SelectedLanguages.Add(copy);
                    }
                }
            }
            if (this.currentOrderInfo.MsgTextInfo.SelectedCityType == null)
            {
                this.currentOrderInfo.MsgTextInfo.SelectedCityType = new MsgTextCityType();
            }

            // 발령 대상
            if (this.currentOrderInfo.TargetRegions == null)
            {
                this.currentOrderInfo.TargetRegions = new List<DataClass.RegionDefinition>();
            }
            if (this.currentOrderInfo.TargetSystems == null)
            {
                this.currentOrderInfo.TargetSystems = new List<SASProfile>();
            }
            // 발령 대상 시스템 종류
            if (this.currentOrderInfo.TargetSystemsKinds == null)
            {
                this.currentOrderInfo.TargetSystemsKinds = new List<SASKind>();
                // 디폴트는 ALL 이므로, 오브젝트는 존재하되 카운트는 0
            }

            // 발령지 구분 (고정값: 자체 발령)
            if (this.currentOrderInfo.LocationKind == OrderLocationKind.Unknown)
            {
                this.currentOrderInfo.LocationKind = OrderLocationKind.Local;
            }

            // 
            if (this.currentOrderInfo.CAPData == null)
            {
                this.currentOrderInfo.CAPData = new CAPLib.CAP();
            }
        }

        private void LoadForm(object sender, EventArgs e)
        {
            if (BasisData.TransmitMsgTextInfo == null || BasisData.TransmitMsgTextInfo.Count < 1)
            {
                MessageBox.Show("문안 정보가 존재하지 않습니다. 발령 준비를 취소합니다.", "발령 준비 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Dispose();
                return;
            }
//            if (this.currentOrderInfo == null)
            {
                CheckDefaultSet();
            }

            this.btnMsgText.Enabled = false;

            UpdateOrderModeDisplay(this.currentOrderInfo.Mode);

            ShowHeadline(false);

            switch (this.currentOrderInfo.RefType)
            {
                case OrderReferenceType.SWR:
                    {
                        // 기상특보 연계 발령
                        this.Text = "기상특보 연계 발령";
                        SetHeadline();
                        ShowHeadline(true);

                        // 기상특보 해제
                        if (this.currentOrderInfo.Disaster.Kind.Code == "DWC")
                        {
                            this.cmbboxDisasterCategory.Enabled = false;
                            this.cmbboxDisasterKind.Enabled = false;
                        }
                    }
                    break;
                case OrderReferenceType.Cancel:
                    {
                        // 발령 취소
                        this.Text = "발령 취소";
                        this.cmbboxDisasterCategory.Enabled = false;
                        this.cmbboxDisasterKind.Enabled = false;
                    }
                    break;
                case OrderReferenceType.Clear:
                    {
                        // 경보 해제
                        this.Text = "경보 해제 발령";
                        this.cmbboxDisasterCategory.Enabled = false;
                        this.cmbboxDisasterKind.Enabled = false;
                    }
                    break;
                case OrderReferenceType.None:
                default:
                    {
                        // 통상 발령
                        this.Text = "발령 정보 설정";
                    }
                    break;
            }

            LoadDisasterInfo();

            if (this.currentOrderInfo.Scope == CAPLib.ScopeType.Private ||
                (this.currentOrderInfo.TargetSystems != null &&
                 this.currentOrderInfo.TargetSystems.Count > 0))
            {
                this.currentOrderInfo.Scope = CAPLib.ScopeType.Private;

                this.btnStdAlertSystemKind.Enabled = false;
            }
        }

        private void SetHeadline()
        {
            if (this.currentOrderInfo.RefType == OrderReferenceType.SWR)
            {
                if (this.currentOrderInfo.Tag == null)
                {
                    return;
                }
                SWRProfile profile = this.currentOrderInfo.Tag as SWRProfile;
                if (profile == null)
                {
                    return;
                }
                SWRWarningItemProfile warningItem = profile.GetWarningItemProfile();
                if (warningItem == null)
                {
                    return;
                }

                StringBuilder builder = new StringBuilder("[기상특보연계] ");
                if (warningItem.AnnounceTime.Ticks > 0)
                {
                    builder.Append(warningItem.AnnounceTime.ToString());
                }
                string areaNames = profile.GetTargetAreaNames();
                if (!string.IsNullOrEmpty(areaNames))
                {
                    builder.Append(" " + areaNames);
                }
                if (!string.IsNullOrEmpty(profile.WarnKindCode))
                {
                    string kindName = BasisData.FindSWRKindStringByKindCode(profile.WarnKindCode);
                    if (!string.IsNullOrEmpty(kindName))
                    {
                        builder.Append(" " + kindName);
                    }
                }
                if (!string.IsNullOrEmpty(profile.WarnStressCode))
                {
                    string stressName = BasisData.FindSWRStressStringByStressCode(profile.WarnStressCode);
                    if (!string.IsNullOrEmpty(stressName))
                    {
                        builder.Append(" " + stressName);
                    }
                }
                if (!string.IsNullOrEmpty(profile.CommandCode))
                {
                    string commandName = BasisData.FindSWRCommandStringByCommandCode(profile.CommandCode);
                    if (!string.IsNullOrEmpty(commandName))
                    {
                        builder.Append(" " + commandName);
                    }
                }

                this.lblHeadline.Text = builder.ToString();
            }
        }
        private void ShowHeadline(bool toVisible)
        {
            if (toVisible)
            {
                this.Size = new Size(506, 288 + 30);
                this.pnlBody.Location = new Point(7, 47 + 30);
            }
            else
            {
                this.Size = new Size(506, 288);
                this.pnlBody.Location = new Point(7, 48);
            }
            this.lblHeadline.Visible = toVisible;
        }

        /// <summary>
        /// [취소] 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.None;

            DialogResult result = MessageBox.Show("모든 발령 설정이 초기화됩니다. 취소하시겠습니까?", "발령 준비 취소", MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                if (this.NotifyOrderProvisionResult != null)
                {
                    this.NotifyOrderProvisionResult(this, new OrderPrepareEventArgs(false, null));
                }
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
        }
        /// <summary>
        /// [발령] 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOrder_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.None;

            if (this.cmbboxDisasterCategory.SelectedItem == null ||
                this.cmbboxDisasterKind.SelectedItem == null)
            {
                DialogResult result = MessageBox.Show("재난 종류를 선택해야 합니다.", "발령 준비 확인", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.btnOrder.ChkValue = false;
                return;
            }
            DisasterKind selectedDisaster = this.cmbboxDisasterKind.SelectedItem as DisasterKind;
            if (selectedDisaster == null)
            {
                FileLogManager.GetInstance().WriteLog("[OrderForm] btnOrder_Click( 재난 정보 데이터 오류 )");
                DialogResult result = MessageBox.Show("재난 종류 정보에 오류가 있습니다. 발령을 수행할 수 없습니다.", "발령 준비 오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
                return;
            }
            DisasterKind copy = new DisasterKind();
            copy.DeepCopyFrom(selectedDisaster);
            this.currentOrderInfo.Disaster.Kind = copy;

            // 문안 체크
            foreach (MsgText msg in this.currentOrderInfo.MsgTextInfo.CurrentTransmitMsgText)
            {
                bool isTargetLanguage = false;
                foreach (MsgTextDisplayLanguageKind language in this.currentOrderInfo.MsgTextInfo.SelectedLanguages)
                {
                    if (msg.LanguageKindID == language.ID)
                    {
                        isTargetLanguage = true;
                        break;
                    }
                }
                if (!isTargetLanguage)
                {
                    continue;
                }
                if (msg.CityTypeID != this.currentOrderInfo.MsgTextInfo.SelectedCityType.ID)
                {
                    continue;
                }

                foreach (string key in BasisData.KEYWORD_TIMES)
                {
                    int index = msg.Text.IndexOf(key);
                    if (index > 0)
                    {
                        msg.Text = msg.Text.Replace(key, "현재시각");
                        break;
                    }
                }
                foreach (string key in BasisData.KEYWORD_REGIONS)
                {
                    int index = msg.Text.IndexOf(key);
                    if (index > 0)
                    {
                        msg.Text = msg.Text.Replace(key, "우리지역");
                        break;
                    }
                }
            }

            // [2016-03-29] 실제 발령시 확인창 표출을 위해 추가함 - by Gonzi
            if(this.currentOrderInfo.Mode.Code == CAPLib.StatusType.Actual)
            {
                DialogResult result = MessageBox.Show("실제 발령을 수행하시겠습니까?", "발령 확인", MessageBoxButtons.YesNo);

                if (result != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }
            }

            if (this.NotifyOrderProvisionResult != null)
            {
                this.NotifyOrderProvisionResult(this, new OrderPrepareEventArgs(true, this.currentOrderInfo));
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }


        #region 재난정보
        /// <summary>
        /// 재난 카테고리/종류의 기초 데이터를 로드.
        /// </summary>
        private void LoadDisasterInfo()
        {
            this.cmbboxDisasterCategory.Items.Clear();

            if (BasisData.Disasters == null || BasisData.Disasters.Count <= 0)
            {
                return;
            }
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

            // 선택 상태 갱신
            UpdateDisasterSelection();
        }
        /// <summary>
        /// 발령 정보에서 설정한 대로 재난 카테고리/종류 정보를 설정(선택)
        /// </summary>
        private void UpdateDisasterSelection()
        {
            // 재난 카테고리 선택 상태
            if (this.cmbboxDisasterCategory.Items == null)
            {
                this.cmbboxDisasterCategory.Enabled = false;

                return;
            }
            for (int index = 0; index < this.cmbboxDisasterCategory.Items.Count; index++)
            {
                DisasterInfo info = this.cmbboxDisasterCategory.Items[index] as DisasterInfo;
                if (info == null || info.Category == null)
                {
                    continue;
                }
                if (info.Category.Code == this.currentOrderInfo.Disaster.Category.Code)
                {
                    this.cmbboxDisasterCategory.SelectedIndex = index;
                    break;
                }
            }
            if (this.cmbboxDisasterCategory.SelectedIndex < 0)
            {
                this.cmbboxDisasterKind.Items.Clear();
                this.cmbboxDisasterKind.Enabled = false;

                return;
            }

            // 재난 종류 선택 상태
            if (this.cmbboxDisasterKind.Items == null)
            {
                this.cmbboxDisasterKind.Enabled = false;

                return;
            }
            for (int index = 0; index < this.cmbboxDisasterKind.Items.Count; index++)
            {
                DisasterKind info = this.cmbboxDisasterKind.Items[index] as DisasterKind;
                if (info == null)
                {
                    // 아직 데이터가 설정되지 않았다.
                    continue;
                }
                if (info.Code == this.currentOrderInfo.Disaster.Kind.Code)
                {
                    this.cmbboxDisasterKind.SelectedIndex = index;
                    break;
                }
            }
        }

        /// <summary>
        /// 재난 카테고리 선택 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbboxDisasterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbbox = sender as ComboBox;

            this.cmbboxDisasterKind.Items.Clear();
            this.cmbboxDisasterKind.ResetText();

            if (cmbbox.SelectedIndex < 0)
            {
                this.cmbboxDisasterKind.Enabled = false;

                this.currentOrderInfo.Disaster.Category = new DisasterCategory();
                this.currentOrderInfo.Disaster.Kind = new DisasterKind();
            }
            else
            {
                if (this.currentOrderInfo.RefType == OrderReferenceType.Clear ||
                     this.currentOrderInfo.Disaster.Kind.Code == "DWC")
                {
                    this.cmbboxDisasterCategory.Enabled = false;
                    this.cmbboxDisasterKind.Enabled = false;
                }
                else
                {
                    this.cmbboxDisasterCategory.Enabled = true;
                    this.cmbboxDisasterKind.Enabled = true;
                }

                DisasterInfo selectedCategoryInfo = cmbbox.SelectedItem as DisasterInfo;
                if (selectedCategoryInfo == null || selectedCategoryInfo.Category == null)
                {
                    System.Console.WriteLine("[OrderForm] cmbboxDisasterCategory_SelectedIndexChanged (데이터 오류) ");
                    return;
                }

                if (selectedCategoryInfo.KindList != null)
                {
                    foreach (DisasterKind kind in selectedCategoryInfo.KindList)
                    {
                        this.cmbboxDisasterKind.Items.Add(kind);
                    }
                }

                if (cmbbox.Focused)
                {
                    DisasterCategory category = new DisasterCategory();
                    category.DeepCopyFrom(selectedCategoryInfo.Category);
                    this.currentOrderInfo.Disaster.Category = category;
                    this.currentOrderInfo.Disaster.Kind = new DisasterKind();
                }
            }

            this.btnMsgText.Enabled = false;
        }
        /// <summary>
        /// 재난 종류 선택 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbboxDisasterKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox combbox = sender as ComboBox;

            this.currentOrderInfo.MsgTextInfo.OriginalTransmitMsgText.Clear();
            this.currentOrderInfo.MsgTextInfo.CurrentTransmitMsgText.Clear();

            if (combbox.SelectedItem == null)
            {
                return;
            }

            DisasterKind info = combbox.SelectedItem as DisasterKind;
            if (info == null)
            {
                return;
            }

            this.btnMsgText.Enabled = true;

            this.currentOrderInfo.Disaster.Kind.DeepCopyFrom(info);

            // 문안 변경 적용
            foreach (DisasterMsgText msgTxt in BasisData.TransmitMsgTextInfo.Values)
            {
                if (msgTxt.Disaster == null || msgTxt.Disaster.Kind == null)
                {
                    continue;
                }
                if (msgTxt.Disaster.Kind.Code == info.Code)
                {
                    MsgText original = new MsgText();
                    MsgText current = new MsgText();
                    original.DeepCopyFrom(msgTxt.MsgTxt);
                    int index = original.Text.IndexOf("(지역명)");
                    if (index > 0)
                    {
                        original.Text = original.Text.Replace("(지역명)", BasisData.TopRegion.Name);
                    }
                    current.DeepCopyFrom(original);

                    this.currentOrderInfo.MsgTextInfo.OriginalTransmitMsgText.Add(original);
                    this.currentOrderInfo.MsgTextInfo.CurrentTransmitMsgText.Add(current);
                }
            }
        }
        #endregion


        #region 발령 모드
        private void btnOrderMode_Click(object sender, EventArgs e)
        {
            OrderModeForm orderModeForm = new OrderModeForm(currentOrderInfo.Mode.Code);
            orderModeForm.NotifyOrderModeChanged += new EventHandler<OrderModeEventArgs>(orderModeForm_OnNotifyOrderModeChanged);
            DialogResult result = orderModeForm.ShowDialog();
            orderModeForm.NotifyOrderModeChanged -= new EventHandler<OrderModeEventArgs>(orderModeForm_OnNotifyOrderModeChanged);

            this.btnOrderMode.ChkValue = false;
        }

        void orderModeForm_OnNotifyOrderModeChanged(object sender, OrderModeEventArgs e)
        {
            if (this.currentOrderInfo.Mode == null)
            {
                this.currentOrderInfo.Mode = new OrderMode();
            }

            OrderMode modeInfo = BasisData.FindOrderModeInfoByCode(e.Mode);
            if (modeInfo != null)
            {
                this.currentOrderInfo.Mode.DeepCopyFrom(modeInfo);
            }
            else
            {
                this.currentOrderInfo.Mode.Code = e.Mode;
                this.currentOrderInfo.Mode.Name = "실제";
            }

            UpdateOrderModeDisplay(this.currentOrderInfo.Mode);
        }

        private void UpdateOrderModeDisplay(OrderMode mode)
        {
            if (mode.Code == CAPLib.StatusType.Actual)
            {
                this.lblOrderMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            }
            else if (mode.Code == CAPLib.StatusType.Exercise)
            {
                this.lblOrderMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(203)))), ((int)(((byte)(3)))));
            }
            else
            {
                this.lblOrderMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(149)))), ((int)(((byte)(30)))));
            }

            this.lblOrderMode.Text = mode.Name + "모드";
        }
        #endregion


        #region 표준경보시스템 종류
        /// <summary>
        /// [표준경보시스템 종류 선택]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStdAlertSystemKind_Click(object sender, EventArgs e)
        {
            SystemKindForm systemKindForm = new SystemKindForm(this.currentOrderInfo.TargetSystemsKinds);
            systemKindForm.NotifySASKindChanged += new EventHandler<SASKindEventArgs>(orderModeForm_OnNotifySASKindChanged);
            DialogResult result = systemKindForm.ShowDialog();
            systemKindForm.NotifySASKindChanged -= new EventHandler<SASKindEventArgs>(orderModeForm_OnNotifySASKindChanged);

            this.btnStdAlertSystemKind.ChkValue = false;
        }
        /// <summary>
        /// 표준경보시스템 종류 선택 변경 통지 이벤트 핸들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void orderModeForm_OnNotifySASKindChanged(object sender, SASKindEventArgs e)
        {
            if (e.IsAllSelected || e.SelectedSystemKinds == null)
            {
                // 전체선택
                this.currentOrderInfo.TargetSystemsKinds = null;

                this.currentOrderInfo.Scope = CAPLib.ScopeType.Public;
            }
            else
            {
                this.currentOrderInfo.Scope = CAPLib.ScopeType.Restricted;

                this.currentOrderInfo.TargetSystemsKinds.Clear();
                foreach (SASKind kind in e.SelectedSystemKinds)
                {
                    SASKind copy = new SASKind();
                    copy.DeepCopyFrom(kind);
                    this.currentOrderInfo.TargetSystemsKinds.Add(copy);
                }
            }
        }
        #endregion


        #region 문안 설정
        /// <summary>
        /// [문안] 확인/변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMsgText_Click(object sender, EventArgs e)
        {
            MsgTextForm msgTextForm = new MsgTextForm(this.currentOrderInfo.MsgTextInfo);
            DialogResult result = msgTextForm.ShowDialog(this);

            this.btnMsgText.ChkValue = false;
        }
        #endregion


        /// <summary>
        /// 헤드라인 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblHeadline_Click(object sender, EventArgs e)
        {
            if (this.currentOrderInfo.RefType != OrderReferenceType.SWR)
            {
                return;
            }

            if (this.currentOrderInfo.Tag == null)
            {
                return;
            }
            SWRProfile profile = this.currentOrderInfo.Tag as SWRProfile;
            if (profile == null)
            {
                return;
            }

            SWRWarningItemDetailForm swrDetailForm = new SWRWarningItemDetailForm(profile);
            swrDetailForm.Show();
        }
    }

    /// <summary>
    /// 발령 준비 정보 전달 이벤트 아규먼트 클래스
    /// </summary>
    public class OrderPrepareEventArgs : EventArgs
    {
        bool isOrder = false;
        public bool IsOrder
        {
            get { return isOrder; }
            set { isOrder = value; }
        }
        private OrderProvisionInfo orderProvisionInfo;
        public OrderProvisionInfo OrderProvisionInfo
        {
            get { return orderProvisionInfo; }
            set { orderProvisionInfo = value; }
        }

        public OrderPrepareEventArgs(bool executeOrder, OrderProvisionInfo order)
        {
            this.isOrder = executeOrder;
            this.orderProvisionInfo = order;
        }
    }

    /// <summary>
    /// 발령 정보 갱신 통보 이벤트 아규먼트 클래스
    /// </summary>
    public class OrderEventArgs : EventArgs
    {
        private OrderRecord orderInfo;
        public OrderRecord OrderInfo
        {
            get { return orderInfo; }
            set { orderInfo = value; }
        }
        private string headline = string.Empty;
        public string Headline
        {
            get { return headline; }
            set { headline = value; }
        }
        public OrderEventArgs()
        {
        }
        public OrderEventArgs(OrderRecord order, string headline)
        {
            this.orderInfo = order;
            this.headline = headline;
        }
    }
    
    /// <summary>
    /// 발령 응답 갱신 통보 이벤트 아규먼트 클래스
    /// </summary>
    public class OrderResponseEventArgs : EventArgs
    {
        string orderRecordID;
        public string OrderRecordID
        {
            get { return orderRecordID; }
//            set { orderRecordID = value; }
        }
        private List<OrderResponseProfile> responseInfo;
        public List<OrderResponseProfile> ResponseInfo
        {
            get { return responseInfo; }
//            set { responseInfo = value; }
        }
        public OrderResponseEventArgs(string orderID, List<OrderResponseProfile> responseInfo)
        {
            this.orderRecordID = orderID;
            this.responseInfo = responseInfo;
        }
    }
}
