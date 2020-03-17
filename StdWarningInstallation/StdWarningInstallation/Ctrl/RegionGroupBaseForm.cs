using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using StdWarningInstallation.Ctrl;
using StdWarningInstallation.DataClass;

namespace StdWarningInstallation.Ctrl
{
    public partial class RegionGroupBaseForm : Form
    {
        public event EventHandler<UpdateGroupProfileEventArgs> NotifyUpdateGroupProfile;

        private uint groupNameLengthLimit = 20;
        private ProfileUpdateMode currentMode = ProfileUpdateMode.Regist;

        protected GroupProfile originalProfile = null;
        protected GroupProfile currentProfile = null;


        public RegionGroupBaseForm()
        {
            InitializeComponent();

            this.currentProfile = new GroupProfile();
        }
        public void SetTargetGroupProfile(GroupProfile profile)
        {
            this.originalProfile = profile;

            if (this.currentProfile == null)
            {
                this.currentProfile = new GroupProfile();
            }
            this.currentProfile.DeepCopyFrom(profile);
        }
        public void SetProfileUpdateMode(ProfileUpdateMode mode)
        {
            this.currentMode = mode;

            if (this.currentMode == ProfileUpdateMode.Regist)
            {
                this.Text = "신규 그룹 생성";
                this.lblDescription.Text = "지역을 선택하여 그룹을 생성합니다.";

                this.txtboxGroupName.ResetText();

                this.btnDeleteGroup.Visible = false;
                this.btnDeleteGroup.Enabled = false;
                this.btnOK.Text = "그룹 등록";
            }
            else
            {
                this.Text = "그룹 상세 정보";
                this.lblDescription.Text = "그룹 정보를 수정하거나 삭제할 수 있습니다.";

                this.btnDeleteGroup.Visible = true;
                this.btnDeleteGroup.Enabled = true;
                this.btnOK.Text = "변경 적용";
            }
        }

        #region 초기데이터설정
        private void InitLocalData()
        {
            InitializeDisasterInfo();
            InitializeRegionInfo();
            InitializeSystemKindInfo();
        }
        private void InitializeDisasterInfo()
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

                int itemIndex = this.cmbboxDisasterCategory.Items.Add(info);
            }
        }
        private void InitializeRegionInfo()
        {
            this.tviewRegionList.Nodes.Clear();

            if (!BasisData.IsRegionDataLoaded())
            {
                System.Console.WriteLine("[RegionGroupBaseForm] 지역목록 초기화 실패 - BasisData 가 로딩되지 않음");
                return;
            }

            Dictionary<string, RegionProfile> regionList = BasisData.Regions.LstRegion;

            foreach (RegionProfile region1 in regionList.Values)
            {
                // [시도]
                TreeNode level1 = this.tviewRegionList.Nodes.Add(region1.Code, region1.Name);
                foreach (RegionProfile region2 in region1.LstSubRegion.Values)
                {
                    // [시군구]
                    TreeNode level2 = level1.Nodes.Add(region2.Code, region2.Name);
                    foreach (RegionProfile region3 in region2.LstSubRegion.Values)
                    {
                        // [읍면동]
                        level2.Nodes.Add(region3.Code, region3.Name);
                    }
                }
            }

            int nodeCnt = tviewRegionList.GetNodeCount(false);
            for (int i = 0; i < nodeCnt; i++)
            {
                tviewRegionList.Nodes[i].Expand();
            }
        }
        private void InitializeSystemKindInfo()
        {
            // 각 종류 추가
            if (BasisData.SASKindInfo == null ||
                BasisData.SASKindInfo.Values == null)
            {
                return;
            }

            foreach (SASKind kind in BasisData.SASKindInfo.Values)
            {
                if (kind == null)
                {
                    continue;
                }
                SASKind copy = new SASKind();
                copy.DeepCopyFrom(kind);

                int itemIndex = this.chkListBoxSASKind.Items.Add(copy);
                if (itemIndex >= 0)
                {
                    this.chkListBoxSASKind.SetItemChecked(itemIndex, true);
                }
            }
        }
        #endregion


        #region 그룹정보 표시 갱신
        /// <summary>
        /// 그룹 정보에서 등록된 대로 재난 카테고리/종류 정보를 갱신.
        /// </summary>
        private void UpdateDisasterSelection()
        {
            // 사용 유무
            if (this.currentProfile.DisasterCategoryID <= 0)
            {
                this.chkboxUseDisasterSet.Checked = false;
                this.cmbboxDisasterCategory.Enabled = false;
                this.cmbboxDisasterKind.Enabled = false;

                return;
            }
            this.chkboxUseDisasterSet.Checked = true;

            // 재난 카테고리 선택 상태
            if (this.cmbboxDisasterCategory.Items == null)
            {
                this.cmbboxDisasterCategory.Enabled = false;
                return;
            }
            this.cmbboxDisasterCategory.Enabled = true;

            for (int index = 0; index < this.cmbboxDisasterCategory.Items.Count; index++)
            {
                DisasterInfo info = this.cmbboxDisasterCategory.Items[index] as DisasterInfo;
                if (info == null || info.Category == null)
                {
                    continue;
                }
                if (info.Category.ID == this.currentProfile.DisasterCategoryID)
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
            this.cmbboxDisasterKind.Enabled = true;

            for (int index = 0; index < this.cmbboxDisasterKind.Items.Count; index++)
            {
                DisasterKind info = this.cmbboxDisasterKind.Items[index] as DisasterKind;
                if (info == null)
                {
                    continue;
                }
                if (info.Code == this.currentProfile.DisasterKindCode)
                {
                    this.cmbboxDisasterKind.SelectedIndex = index;
                    break;
                }
            }
        }
        /// <summary>
        /// 프로필 정보에 기초하여 발령대상 지역 목록을 표시.
        /// </summary>
        private void UpdateTargetRegions()
        {
            if (this.currentProfile == null || this.currentProfile.Targets == null)
            {
                return;
            }

            foreach (string regionCode in this.currentProfile.Targets)
            {
                UpdateCheckState(regionCode, true);
            }
        }
        private void UpdateCheckState(string regionCode, bool check)
        {
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(regionCode));

            if (this.tviewRegionList.Nodes == null || this.tviewRegionList.Nodes.Count <= 0)
            {
                return;
            }

            bool isCompleted = false;
            foreach (TreeNode node1 in this.tviewRegionList.Nodes)
            {
                if (node1.Name == regionCode)
                {
                    node1.Checked = true;
                    isCompleted = true;
                    break;
                }

                if (node1.Nodes == null)
                {
                    continue;
                }
                foreach (TreeNode node2 in node1.Nodes)
                {
                    if (node2.Name == regionCode)
                    {
                        node2.Checked = true;
                        isCompleted = true;
                        break;
                    }

                    if (node2.Nodes == null)
                    {
                        continue;
                    }
                    foreach (TreeNode node3 in node2.Nodes)
                    {
                        if (node3.Name == regionCode)
                        {
                            node3.Checked = true;
                            isCompleted = true;
                            break;
                        }
                    }

                    if (isCompleted)
                    {
                        break;
                    }
                }

                if (isCompleted)
                {
                    break;
                }
            }

        }
        /// <summary>
        /// 프로필 정보에 기초하여 발령대상 시스템 종류를 표시.
        /// </summary>
        private void UpdateTargetSystemKinds()
        {
            if (this.currentProfile == null || this.chkListBoxSASKind.Items == null)
            {
                return;
            }
            if (this.currentProfile.TargetSystemKinds == null || this.currentProfile.TargetSystemKinds.Count < 1)
            {
                return;
            }

            for (int index = 0; index < this.chkListBoxSASKind.Items.Count; index++)
            {
                SASKind kind = this.chkListBoxSASKind.Items[index] as SASKind;
                if (kind == null)
                {
                    continue;
                }

                bool set = this.currentProfile.TargetSystemKinds.Contains(kind.Code);
                this.chkListBoxSASKind.SetItemChecked(index, set);
            }
        }
        #endregion


        private void LoadForm(object sender, EventArgs e)
        {
            InitLocalData();

            if (this.currentProfile != null)
            {
                this.txtboxGroupName.Text = this.currentProfile.Name;
            }
            else
            {
                this.txtboxGroupName.ResetText();
            }

            UpdateDisasterSelection();
            UpdateTargetRegions();
            UpdateTargetSystemKinds();
        }


        #region 그룹이름
        /// <summary>
        /// [그룹 이름] 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtboxGroupName_TextChanged(object sender, EventArgs e)
        {
            if (this.txtboxGroupName.Focused)
            {
                this.currentProfile.Name = this.txtboxGroupName.Text;
            }
        }
        /// <summary>
        /// 그룹 이름 제한 글자 수 
        /// </summary>
        /// <param name="limitCount"></param>
        protected void SetNameLengthLimitCount(uint limitCount)
        {
            this.groupNameLengthLimit = limitCount;
            this.txtboxGroupName.MaxLength = (int)limitCount;
            this.lblGroupNameLimitCount.Text = "(최대" + limitCount + "자)";
        }
        /// <summary>
        /// 그룹 이름 취득
        /// </summary>
        /// <returns></returns>
        protected string GetGroupName()
        {
            return this.txtboxGroupName.Text;
        }
        /// <summary>
        /// 그룹 이름 설정
        /// </summary>
        /// <param name="name"></param>
        protected void SetGroupName(string name)
        {
            this.txtboxGroupName.Text = name;
        }
        #endregion


        #region 재난 종류
        /// <summary>
        /// [재난 종류] 사용 유무
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkboxUseDisasterSet_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ctrl = sender as CheckBox;
            if (ctrl == null)
            {
                return;
            }

            this.cmbboxDisasterCategory.Enabled = ctrl.Checked;
            this.cmbboxDisasterKind.Enabled = ctrl.Checked;
        }
        /// <summary>
        /// [재난 종류] 카테고리 리스트 선택 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbboxDisasterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbbox = sender as ComboBox;

            this.cmbboxDisasterKind.Items.Clear();
            this.cmbboxDisasterKind.ResetText();
            this.cmbboxDisasterKind.Enabled = true;

            if (cmbbox.SelectedIndex < 0)
            {
                this.cmbboxDisasterKind.Enabled = false;
                return;
            }

            DisasterInfo selectedCategory = cmbbox.SelectedItem as DisasterInfo;
            if (selectedCategory == null)
            {
                return;
            }

            // 재난 종류 콤보박스 갱신
            if (selectedCategory.KindList != null)
            {
                foreach (DisasterKind kind in selectedCategory.KindList)
                {
                    this.cmbboxDisasterKind.Items.Add(kind);
                }
            }

            // 사용자에 의한 선택 변경 시에만 그룹 정보를 갱신
            if (cmbbox.Focused)
            {
                this.currentProfile.DisasterCategoryID = selectedCategory.Category.ID;
                this.currentProfile.DisasterKindCode = string.Empty;
            }
        }
        /// <summary>
        /// [재난 종류] 종류 리스트 선택 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbboxDisasterKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbbox = sender as ComboBox;
            if (cmbbox == null)
            {
                return;
            }

            string selectedKindCode = string.Empty;
            if (cmbbox.SelectedItem != null)
            {
                DisasterKind selectedKind = cmbbox.SelectedItem as DisasterKind;
                if (selectedKind != null)
                {
                    selectedKindCode = selectedKind.Code;
                }
            }

            if (cmbbox.Focused)
            {
                this.currentProfile.DisasterKindCode = selectedKindCode;
            }
        }
        #endregion


        #region 발령대상 추가/삭제
        private void tviewRegionList_AfterCheck(object sender, TreeViewEventArgs e)
        {
            string regionCode = e.Node.Name;
            bool isChecked = e.Node.Checked;

            // 상호 호출 무한 루프를 막기 위해 포커스가 현 컨트롤에 있을 때만 다른 컨트롤 연계
            if (!this.tviewRegionList.Focused || !BasisData.IsRegionDataLoaded())
            {
                return;
            }

            if (string.IsNullOrEmpty(regionCode))
            {
                System.Console.WriteLine("[RegionGroupBaseForm] tviewRegionList_AfterCheck (선택된 아이템의 아이디(==지역코드) 오류");
                return;
            }

            if (isChecked)
            {
                // 매번 추가/삭제를 하지 않고, 등록 또는 변경 버튼을 눌렀을 때 최종적으로 취합.
            }
            else
            {
            }

            //ChangeRegionColor(selectedId, e.Node.Checked);
        }
        /// <summary>
        /// 그룹 등록 / 그룹 정보 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            ReflectTargetRegions();
            ReflectTargetSystemKinds();

            if (!CheckEnteredInformation())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            this.currentProfile.GroupType = GroupTypeCodes.Region;
            if (this.NotifyUpdateGroupProfile != null)
            {
                this.NotifyUpdateGroupProfile(this, new UpdateGroupProfileEventArgs(this.currentProfile, this.currentMode));
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            return;
        }
        /// <summary>
        /// 그룹 정보 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteGroup_Click(object sender, EventArgs e)
        {
            DialogResult answer = MessageBox.Show("이 그룹의 모든 정보가 삭제되며, 삭제된 후에는 복구할 수 없습니다.\n그룹을 삭제하시겠습니까?",
                            "그룹 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            this.currentProfile.GroupType = GroupTypeCodes.Region;
            if (this.NotifyUpdateGroupProfile != null)
            {
                this.NotifyUpdateGroupProfile(this, new UpdateGroupProfileEventArgs(this.currentProfile, ProfileUpdateMode.Delete));
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            return;
        }
        /// <summary>
        /// 그룹 편집 취소 (= 닫기)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 입력 정보 체크
        /// </summary>
        /// <returns></returns>
        public bool CheckEnteredInformation()
        {
            if (string.IsNullOrEmpty(this.txtboxGroupName.Text))
            {
                MessageBox.Show("그룹 이름을 입력해야 합니다.", "그룹 정보 오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (this.currentProfile.Targets == null || this.currentProfile.Targets.Count < 1)
            {
                MessageBox.Show("하나 이상의 발령 대상을 선택해야 합니다.", "그룹 정보 오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (this.currentProfile.TargetSystemKinds == null || this.currentProfile.TargetSystemKinds.Count < 1)
            {
                MessageBox.Show("하나 이상의 시스템 종류를 선택해야 합니다.", "그룹 정보 오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }
        /// <summary>
        /// 발령대상으로 선택된 지역 정보를 그룹 프로필 정보에 반영.
        /// </summary>
        public void ReflectTargetRegions()
        {
            if (this.currentProfile.Targets == null)
            {
                this.currentProfile.Targets = new List<string>();
            }
            this.currentProfile.Targets.Clear();

            if (this.tviewRegionList.Nodes != null)
            {
                foreach (TreeNode node1 in this.tviewRegionList.Nodes)
                {
                    if (node1.Checked)
                    {
                        this.currentProfile.Targets.Add(node1.Name);
                    }

                    if (node1.Nodes == null || node1.Nodes.Count < 1)
                    {
                        continue;
                    }
                    foreach (TreeNode node2 in node1.Nodes)
                    {
                        if (node2.Checked)
                        {
                            this.currentProfile.Targets.Add(node2.Name);
                        }

                        if (node2.Nodes == null || node2.Nodes.Count < 1)
                        {
                            continue;
                        }
                        foreach (TreeNode node3 in node2.Nodes)
                        {
                            if (node3.Checked)
                            {
                                this.currentProfile.Targets.Add(node3.Name);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 발령대상으로 선택된 지역 정보를 그룹 프로필 정보에 반영.
        /// </summary>
        public void ReflectTargetSystemKinds()
        {
            if (this.currentProfile.TargetSystemKinds == null)
            {
                this.currentProfile.TargetSystemKinds = new List<string>();
            }
            this.currentProfile.TargetSystemKinds.Clear();

            if (this.chkListBoxSASKind.CheckedItems != null)
            {
                foreach (object obj in this.chkListBoxSASKind.CheckedItems)
                {
                    if (obj == null)
                    {
                        continue;
                    }

                    SASKind kind = obj as SASKind;
                    if (kind == null)
                    {
                        continue;
                    }

                    this.currentProfile.TargetSystemKinds.Add(kind.Code);
                }
            }
        }
        public bool IsProfileChanged()
        {
            bool isChanged = false;

            if (this.originalProfile != null && this.currentProfile != null)
            {
                if (this.originalProfile.Name != this.currentProfile.Name)
                {
                    isChanged = true;
                }
                if (!isChanged)
                {
                    if (this.originalProfile.DisasterCategoryID != this.currentProfile.DisasterCategoryID ||
                        this.originalProfile.DisasterKindCode != this.currentProfile.DisasterKindCode)
                    {
                        isChanged = true;
                    }
                }
                if (!isChanged)
                {
                    if (this.originalProfile.GetTargetsString() != this.currentProfile.GetTargetsString() ||
                        this.originalProfile.GetTargetSystemKindsString() != this.currentProfile.GetTargetSystemKindsString())
                    {
                        isChanged = true;
                    }
                }
            }
            else if (this.currentProfile != null)
            {
                if (!string.IsNullOrEmpty(this.currentProfile.Name))
                {
                    return true;
                }
                else
                {
                    // 그룹명을 입력하지 않은 상태에서는 모든 입력값 무시.
                    return false;
                }
            }
            else
            {
                return false;
            }

            return isChanged;
        }
        #endregion


    }
}
