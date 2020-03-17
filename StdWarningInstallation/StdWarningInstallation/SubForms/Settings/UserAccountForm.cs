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
    public partial class UserAccountForm : Form
    {
        private List<UserAccount> currentAccountList = null;
        private ProfileUpdateMode updateMode = ProfileUpdateMode.Modify;
        private bool isUniqueChecked = false;


        public UserAccountForm(List<UserAccount> accountList)
        {
            InitializeComponent();

            this.currentAccountList = accountList;

            InitializeAccountList();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            UpdateAccountList();
        }

        private void InitializeAccountList()
        {
            this.lvAccountList.Columns.Clear();

            AdengColumnHeader header = new AdengColumnHeader();
            header.Text = "아이디";
            header.Width = 110;
            this.lvAccountList.Columns.Add(header);
            header = new AdengColumnHeader();
            header.Text = "이름";
            header.Width = 160;
            this.lvAccountList.Columns.Add(header);
        }

        private void UpdateAccountList()
        {
            this.lvAccountList.Items.Clear();

            if (this.currentAccountList == null)
            {
                return;
            }
            foreach (UserAccount account in this.currentAccountList)
            {
                AdengListViewItem item = this.lvAccountList.Items.Add(account.ID);
                item.SubItems.Add(account.Name);

                item.Tag = account;
            }
        }
        private void ResetInputText()
        {
            this.txtboxID.ResetText();
            this.txtboxPassowrd.ResetText();
            this.txtboxName.ResetText();
            this.txtboxDepartment.ResetText();
            this.txtboxPhone.ResetText();
            this.txtboxDescription.ResetText();
        }

        private void lvAccountList_ItemSelectionChanged(object sender, Adeng.Framework.Ctrl.AdengListViewItemSelectionChangedEventArgs e)
        {
            if (e.ItemIndex < 0)
            {
                this.btnModify.Enabled = false;
                this.btnDelete.Enabled = false;
            }
            else
            {
                this.btnModify.Enabled = true;
                this.btnDelete.Enabled = true;
            }


            UserAccount selectedAccount = e.Item.Tag as UserAccount;
            if (selectedAccount == null)
            {
                return;
            }
            if (selectedAccount.ID == BasisData.CurrentLoginUser.ID)
            {
                // 로그인 유저가 자기 자신의 아이디를 삭제할 수는 없다.
                this.btnDelete.Enabled = false;
            }

            this.txtboxID.Text = selectedAccount.ID;
            this.txtboxPassowrd.Text = selectedAccount.Password;
            this.txtboxName.Text = selectedAccount.Name;
            this.txtboxDepartment.Text = selectedAccount.Departure;
            this.txtboxPhone.Text = selectedAccount.Telephone;
            this.txtboxDescription.Text = selectedAccount.Description;
        }

        /// <summary>
        /// [등록] 사용자 신규 추가.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegist_Click(object sender, EventArgs e)
        {
            this.updateMode = ProfileUpdateMode.Regist;
            this.isUniqueChecked = false;

            // 버튼 표시 전환
            this.btnRegist.Visible = false;
            this.btnModify.Visible = false;
            this.btnDelete.Visible = false;

            this.btnCancel.Location = this.btnModify.Location;
            this.btnCancel.Visible = true;
            this.btnSave.Location = this.btnDelete.Location;
            this.btnSave.Visible = true;

            this.pnlDetailBack.Enabled = true;
            this.btnCheckUnique.Visible = true;

            this.lvAccountList.SelectedItems.Clear();

            this.txtboxID.Enabled = true;
            ResetInputText();
        }
        /// <summary>
        /// [수정] 사용자 정보 수정.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModify_Click(object sender, EventArgs e)
        {
            this.updateMode = ProfileUpdateMode.Modify;
            this.isUniqueChecked = true;

            // 버튼 표시 전환
            this.btnRegist.Visible = false;
            this.btnModify.Visible = false;
            this.btnDelete.Visible = false;

            this.btnCancel.Location = this.btnModify.Location;
            this.btnCancel.Visible = true;
            this.btnSave.Location = this.btnDelete.Location;
            this.btnSave.Visible = true;

            this.pnlDetailBack.Enabled = true;
            this.btnCheckUnique.Visible = false;

            this.lvAccountList.SelectedItems.Clear();

            this.txtboxID.Enabled = false;
        }
        /// <summary>
        /// [삭제] 사용자 삭제.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult answer = MessageBox.Show("사용자 정보가 완전히 삭제됩니다. 계속하시겠습니까?", "사용자 정보 삭제", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (answer != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }

            updateMode = ProfileUpdateMode.Delete;

            string deleteUserID = this.txtboxID.Text;
            int result = DBManager.GetInstance().DeleteUserAccountInfo(deleteUserID);
            if (result != 0)
            {
                MessageBox.Show("사용자 정보 삭제 중에 오류가 발생하였습니다. ErrorCode=[" + result + "]", "사용자 정보 삭제", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            this.currentAccountList = DBManager.GetInstance().QueryUserAccountInfo();
            UpdateAccountList();
            ResetInputText();

            // 삭제가 실패한 경우, 삭제하려고 했던 아이템을 재선택 표시
        }

        /// <summary>
        /// [취소]
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.btnCancel.Visible = false;
            this.btnSave.Visible = false;
            this.pnlDetailBack.Enabled = false;

            this.btnRegist.Visible = true;
            this.btnModify.Visible = true;
            this.btnDelete.Visible = true;

            this.currentAccountList = DBManager.GetInstance().QueryUserAccountInfo();
            UpdateAccountList();
            ResetInputText();
        }
        /// <summary>
        /// [수정] 사용자 정보 등록 확정 및 변경 확정.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string title = "사용자 등록";
            if (updateMode == ProfileUpdateMode.Modify)
            {
                title = "사용자 정보 수정";
            }

            if (updateMode == ProfileUpdateMode.Regist)
            {
                if (string.IsNullOrEmpty(this.txtboxID.Text))
                {
                    MessageBox.Show("아이디를 입력해 주세요.", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtboxID.Focus();
                    return;
                }
                if (!isUniqueChecked)
                {
                    MessageBox.Show("아이디 중복 체크를 수행해 주세요.", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.txtboxID.Focus();
                    return;
                }

            }
            if (string.IsNullOrEmpty(this.txtboxPassowrd.Text))
            {
                MessageBox.Show("비밀번호를 입력해 주세요.", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtboxPassowrd.Focus();
                return;
            }
            if (string.IsNullOrEmpty(this.txtboxName.Text))
            {
                MessageBox.Show("이름을 입력해 주세요.", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.txtboxName.Focus();
                return;
            }

            UserAccount newAccount = new UserAccount();

            newAccount.ID = this.txtboxID.Text;
            newAccount.Password = this.txtboxPassowrd.Text;
            newAccount.Name = this.txtboxName.Text;
            newAccount.Departure = this.txtboxDepartment.Text;
            newAccount.Telephone = this.txtboxPhone.Text;
            newAccount.Description = this.txtboxDescription.Text;

            if (updateMode == ProfileUpdateMode.Modify)
            {
                int result = DBManager.GetInstance().UpdateUserAccountInfo(newAccount);
                if (result != 0)
                {
                    MessageBox.Show("사용자 정보 변경 중에 오류가 발생하였습니다. ErrorCode=[" + result + "]", "사용자 정보 수정", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else if (updateMode == ProfileUpdateMode.Regist)
            {
                int result = DBManager.GetInstance().RegistUserAccountInfo(newAccount);
                if (result != 0)
                {
                    MessageBox.Show("사용자 정보 등록 중에 오류가 발생하였습니다. ErrorCode=[" + result + "]", "사용자 정보 등록", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
            }

            this.btnCancel.Visible = false;
            this.btnSave.Visible = false;
            this.pnlDetailBack.Enabled = false;

            this.btnRegist.Visible = true;
            this.btnModify.Visible = true;
            this.btnDelete.Visible = true;

            this.currentAccountList = DBManager.GetInstance().QueryUserAccountInfo();
            UpdateAccountList();
            ResetInputText();
        }

        /// <summary>
        /// 아이디 입력 값 변경에 따라 [중복 체크] 버튼 제어.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtboxID_TextChanged(object sender, EventArgs e)
        {
            this.btnCheckUnique.Enabled = !string.IsNullOrEmpty(this.txtboxID.Text);

            if (string.IsNullOrEmpty(this.txtboxID.Text))
            {
                return;
            }

            // 바이트 수 체크
        }
        /// <summary>
        /// [중복 체크] 버튼 클릭 이벤트 핸들러.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckUnique_Click(object sender, EventArgs e)
        {
            this.isUniqueChecked = false;
            if (this.currentAccountList == null || this.currentAccountList.Count < 1)
            {
                this.isUniqueChecked = true;
                return;
            }

            this.txtboxID.Text = this.txtboxID.Text.Trim();
            if (string.IsNullOrEmpty(this.txtboxID.Text))
            {
                return;
            }

            bool isFound = false;
            foreach (UserAccount account in this.currentAccountList)
            {
                if (account.ID == this.txtboxID.Text)
                {
                    isFound = true;
                    break;
                }
            }

            if (isFound)
            {
                MessageBox.Show("이미 등록된 아이디입니다. 새로운 아이디를 입력해 주세요.", "아이디 중복 검사", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("사용 가능한 아이디입니다.", "아이디 중복 검사", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.isUniqueChecked = !isFound;
        }
    }
}
