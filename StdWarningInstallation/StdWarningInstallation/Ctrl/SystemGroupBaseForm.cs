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
    public partial class SystemGroupBaseForm : Form
    {
        public event EventHandler<UpdateGroupProfileEventArgs> NotifyUpdateGroupProfile;

        private uint groupNameLengthLimit = 20;
        private ProfileUpdateMode currentMode = ProfileUpdateMode.Regist;

        protected GroupProfile originalProfile = null;
        protected GroupProfile currentProfile = null;

        private RegionProfile entireRegion1 = new RegionProfile();
        private RegionProfile entireRegion2 = new RegionProfile();
        private SASKind entireSystemKind = new SASKind();

        private Dictionary<string, SASProfile> currentDicSystemInfo = new Dictionary<string, SASProfile>();
        private SASFilter filter = new SASFilter();


        public SystemGroupBaseForm()
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
        public void SetSASInfo(Dictionary<string, SASInfo> systemInfo)
        {
            if (systemInfo != null && systemInfo.Values != null)
            {
                if (this.currentDicSystemInfo == null)
                {
                    this.currentDicSystemInfo = new Dictionary<string, SASProfile>();
                }

                foreach (SASInfo info in systemInfo.Values)
                {
                    if (info == null || info.Profile == null)
                    {
                        continue;
                    }

                    SASProfile profile = new SASProfile();
                    profile.DeepCopyFrom(info.Profile);
                    this.currentDicSystemInfo.Add(profile.ID, profile);
                }
            }
        }
        public void SetSASProfileList(Dictionary<string, SASProfile> profileList)
        {
            if (profileList != null && profileList.Values != null)
            {
                if (this.currentDicSystemInfo == null)
                {
                    this.currentDicSystemInfo = new Dictionary<string, SASProfile>();
                }

                foreach (SASProfile info in profileList.Values)
                {
                    if (info == null)
                    {
                        continue;
                    }

                    SASProfile profile = new SASProfile();
                    profile.DeepCopyFrom(info);
                    this.currentDicSystemInfo.Add(profile.ID, profile);
                }
            }
        }
        public void SetProfileUpdateMode(ProfileUpdateMode mode)
        {
            this.currentMode = mode;

            if (this.currentMode == ProfileUpdateMode.Regist)
            {
                this.Text = "신규 그룹 생성";
                this.lblDescription.Text = "표준경보시스템을 선택하여 그룹을 생성합니다.";

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
            this.entireRegion1.Code = string.Empty;
            this.entireRegion1.Name = "전체";
            this.entireRegion1.LstSubRegion = null;

            this.entireRegion2.Code = string.Empty;
            this.entireRegion2.Name = "전체";
            this.entireRegion2.LstSubRegion = null;

            this.entireSystemKind.Code = "ALL";
            this.entireSystemKind.Name = "시스템 전체";

            InitializeDisasterInfo();
            InitializeRegionInfo();
            InitializeSystemKindInfo();
            InitializeSystemProfileInfo();
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

                this.cmbboxDisasterCategory.Items.Add(copy);
            }
        }
        private void InitializeRegionInfo()
        {
            this.cmbboxRegionLevel1.Items.Clear();
            this.cmbboxRegionLevel2.Items.Clear();

            int index = this.cmbboxRegionLevel1.Items.Add(this.entireRegion1);
            index = this.cmbboxRegionLevel2.Items.Add(this.entireRegion2);

            if (BasisData.IsRegionDataLoaded())
            {
                foreach (RegionProfile region1 in BasisData.Regions.LstRegion.Values)
                {
                    if (region1.LstSubRegion == null || region1.LstSubRegion.Values == null)
                    {
                        continue;
                    }

                    // 시도 또는 시군구
                    foreach (RegionProfile region2 in region1.LstSubRegion.Values)
                    {
                        // 시군구 또는 읍면동
                        index = this.cmbboxRegionLevel1.Items.Add(region2);
                    }
                }
            }

            this.cmbboxRegionLevel1.SelectedIndex = 0;
            this.cmbboxRegionLevel2.SelectedIndex = 0;
            this.cmbboxRegionLevel2.Enabled = false;
        }
        private void InitializeSystemKindInfo()
        {
            this.cmbboxSystemKind.Items.Clear();

            // "시스템 전체"
            this.cmbboxSystemKind.Items.Add(entireSystemKind);
            this.cmbboxSystemKind.SelectedIndex = 0;

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

                int itemIndex = this.cmbboxSystemKind.Items.Add(copy);
            }
        }
        private void InitializeSystemProfileInfo()
        {

            if (this.currentDicSystemInfo == null ||
                this.currentDicSystemInfo.Values == null)
            {
                return;
            }

            foreach (SASProfile info in this.currentDicSystemInfo.Values)
            {
                if (info == null)
                {
                    continue;
                }

                this.lstboxInquiredSASList.Items.Add(info);
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
        /// 프로필 정보에 의거하여 발령대상 시스템 목록을 표시.
        /// </summary>
        private void UpdateTargetSystems()
        {
            if (this.currentProfile == null || this.currentProfile.Targets == null)
            {
                return;
            }
            if (this.currentDicSystemInfo == null || this.currentDicSystemInfo.Values == null)
            {
                return;
            }

            foreach (SASProfile profile in this.currentDicSystemInfo.Values)
            {
                if (profile == null)
                {
                    continue;
                }

                if (this.currentProfile.Targets.Contains(profile.ID))
                {
                    this.lstboxTargetSASList.Items.Add(profile);
                    continue;
                }
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
            UpdateTargetSystems();
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

        #region 필터
        /// <summary>
        /// [지역 선택] 상위지역 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbboxRegionLevel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbboxRegionLevel2.Items.Clear();
            this.cmbboxRegionLevel2.ResetText();
            this.cmbboxRegionLevel2.Enabled = false;

            ComboBox cmbbox = sender as ComboBox;
            if (cmbbox == null)
            {
                return;
            }
            RegionProfile currentRegion = cmbbox.SelectedItem as RegionProfile;
            if (currentRegion == null)
            {
                return;
            }

            this.cmbboxRegionLevel2.Items.Add(entireRegion2);
            if (currentRegion == this.entireRegion1 || string.IsNullOrEmpty(currentRegion.Code))
            {
                this.cmbboxRegionLevel2.Enabled = false;
                this.cmbboxRegionLevel2.SelectedIndex = 0;
            }
            else
            {
                this.cmbboxRegionLevel2.Enabled = true;
                foreach (RegionProfile subRegion in currentRegion.LstSubRegion.Values)
                {
                    int index = this.cmbboxRegionLevel2.Items.Add(subRegion);
                }
            }
            if (this.cmbboxRegionLevel2.Items.Count > 0)
            {
                this.cmbboxRegionLevel2.SelectedIndex = 0;
            }

            this.filter.Region1Code = currentRegion.Code;
            this.filter.Region2Code = string.Empty;
        }
        /// <summary>
        /// [지역 선택] 하위지역 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbboxRegionLevel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbbox = sender as ComboBox;
            if (cmbbox == null)
            {
                return;
            }
            
            RegionProfile currentRegion = cmbbox.SelectedItem as RegionProfile;
            if (currentRegion == null)
            {
                return;
            }

            if (cmbbox.Focused)
            {
                this.filter.Region2Code = currentRegion.Code;
            }
        }
        /// <summary>
        /// [표준경보시스템 종류 선택]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbboxSystemKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cmbbox = sender as ComboBox;
            if (cmbbox == null)
            {
                return;
            }
            SASKind systemKind = cmbbox.SelectedItem as SASKind;
            if (systemKind == null)
            {
                return;
            }

            this.filter.SystemKindCode = systemKind.Code;
        }
        /// <summary>
        /// [조회] 표준경보시스템 필터링
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFiltering_Click(object sender, EventArgs e)
        {
            if (this.currentDicSystemInfo == null || 
                this.currentDicSystemInfo.Values == null || this.currentDicSystemInfo.Values.Count <= 0)
            {
                return;
            }

            // 현재의 조회결과 리스트를 클리어
            this.lstboxInquiredSASList.Items.Clear();


            bool needRegionFilter1 = false;
            bool needRegionFilter2 = false;
            bool needKindFilter = false;
            string filterRegion1Code = string.Empty;
            string filterRegion2Code = string.Empty;
            SASKind filterSystemKind = null;

            if (this.cmbboxRegionLevel1.SelectedIndex > 0)
            {
                needRegionFilter1 = true;
                filterRegion1Code = this.filter.Region1Code.Trim('0');

                if (this.cmbboxRegionLevel2.SelectedIndex > 0)
                {
                    needRegionFilter2 = true;

                    int startIndex = filterRegion1Code.Length;
                    int count = 10 - filterRegion1Code.Length;
                    filterRegion2Code = this.filter.Region2Code.Substring(startIndex, count).Trim('0');
                }
            }

            if (this.cmbboxSystemKind.SelectedIndex > 0)
            {
                needKindFilter = true;
                filterSystemKind = this.cmbboxSystemKind.SelectedItem as SASKind;
            }

            foreach (SASProfile profile in this.currentDicSystemInfo.Values)
            {
                if (profile == null)
                {
                    continue;
                }

                if (needRegionFilter1)
                {
                    int startIndex = 0;
                    int count = filterRegion1Code.Length;
                    string temp = profile.AssignedIAGWRegionCode.Substring(startIndex, count);
                    if (temp != filterRegion1Code)
                    {
                        continue;
                    }
                }
                if (needRegionFilter2)
                {
                    int startIndex = filterRegion1Code.Length;
                    int count = filterRegion2Code.Length;
                    string temp = profile.AssignedIAGWRegionCode.Substring(startIndex, count);
                    if (temp != filterRegion2Code)
                    {
                        continue;
                    }
                }

                if (needKindFilter)
                {
                    if (profile.KindCode != filterSystemKind.Code)
                    {
                        continue;
                    }
                }

                this.lstboxInquiredSASList.Items.Add(profile);
            }
        }
        #endregion

        #region 발령대상 추가/삭제
        /// <summary>
        /// [ ▷ ] 버튼 클릭 이벤트 핸들러.
        /// 선택한 대상을 발령 대상으로 추가.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnToTarget_Click(object sender, EventArgs e)
        {
            if (this.lstboxInquiredSASList.SelectedItems == null ||
                this.lstboxInquiredSASList.SelectedItems.Count <= 0)
            {
                return;
            }

            if (this.currentProfile.Targets == null)
            {
                this.currentProfile.Targets = new List<string>();
            }

            foreach (object obj in this.lstboxInquiredSASList.SelectedItems)
            {
                if (obj == null)
                {
                    continue;
                }
                SASProfile profile = obj as SASProfile;
                if (profile == null)
                {
                    System.Console.WriteLine("[SystemGroupBaseForm] null?");
                    continue;
                }

                bool isAdded = AddToTarget(profile);
                if (isAdded)
                {
                    this.currentProfile.Targets.Add(profile.ID);
                }
            }

            this.lstboxInquiredSASList.SelectedItems.Clear();
        }
        /// <summary>
        /// [ ◁ ] 버튼 클릭 이벤트 핸들러.
        /// 선택한 대상을 발령 대상에서 삭제.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveFromTarget_Click(object sender, EventArgs e)
        {
            if (this.lstboxTargetSASList.SelectedItems == null ||
                this.lstboxTargetSASList.SelectedItems.Count <= 0)
            {
                return;
            }

            int itemCount = this.lstboxTargetSASList.Items.Count;
            for (int index = itemCount - 1; index >= 0; index--)
            {
                if (this.lstboxTargetSASList.GetSelected(index))
                {
                    this.currentProfile.Targets.RemoveAt(index);
                    this.lstboxTargetSASList.Items.RemoveAt(index);
                }
            }

        }
        /// <summary>
        /// 발령 대상 목록에 이미 추가된 프로필인지 확인하고 아니면 추가.
        /// </summary>
        /// <param name="newProfile"></param>
        /// <returns></returns>
        private bool AddToTarget(SASProfile newProfile)
        {
            if (this.lstboxTargetSASList.Items == null ||
                this.lstboxTargetSASList.Items.Count <= 0)
            {
                this.lstboxTargetSASList.Items.Add(newProfile);
                return true;
            }

            bool isRegisted = this.lstboxTargetSASList.Items.Contains(newProfile);
            if (!isRegisted)
            {
                this.lstboxTargetSASList.Items.Add(newProfile);
                return true;
            }

            return false;
        }
        #endregion

        #region 그룹정보 등록/수정/삭제
        /// <summary>
        /// 그룹 등록 / 그룹 정보 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void btnOK_Click(object sender, EventArgs e)
        {
            if (!CheckEnteredInformation())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            this.currentProfile.GroupType = GroupTypeCodes.System;
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

            this.currentProfile.GroupType = GroupTypeCodes.System;
            if (this.NotifyUpdateGroupProfile != null)
            {
                this.NotifyUpdateGroupProfile(this, new UpdateGroupProfileEventArgs(this.currentProfile, ProfileUpdateMode.Delete));
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            return;
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
            if (this.lstboxTargetSASList.Items == null || this.lstboxTargetSASList.Items.Count <= 0)
            {
                MessageBox.Show("하나 이상의 발령 대상을 선택해야 합니다.", "그룹 정보 오류", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
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
                    if (this.originalProfile.GetTargetsString() != this.currentProfile.GetTargetsString())
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

        public virtual void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    /// <summary>
    /// 그룹 정보 갱신 통보 이벤트 아규먼트 클래스
    /// </summary>
    public class UpdateGroupProfileEventArgs : EventArgs
    {
        private GroupProfile targetProfile = null;
        public GroupProfile TargetProfile
        {
            get { return targetProfile; }
            set { targetProfile = value; }
        }
        private ProfileUpdateMode updateMode;
        public ProfileUpdateMode UpdateMode
        {
            get { return updateMode; }
            set { updateMode = value; }
        }
        public UpdateGroupProfileEventArgs()
        {
        }
        public UpdateGroupProfileEventArgs(GroupProfile profile, ProfileUpdateMode mode)
        {
            this.targetProfile = profile;
            this.updateMode = mode;
        }
    }
}
