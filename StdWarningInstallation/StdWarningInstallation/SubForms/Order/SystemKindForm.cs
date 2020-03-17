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
    public partial class SystemKindForm : Form
    {
        public event EventHandler<SASKindEventArgs> NotifySASKindChanged;

        private List<SASKind> originalSelList = null;
        private List<SASKind> currentSelList = null;

        public SystemKindForm()
        {
            InitializeComponent();

            this.originalSelList = new List<SASKind>();
            this.currentSelList = new List<SASKind>();
        }
        public SystemKindForm(List<SASKind> selList)
        {
            InitializeComponent();

            this.originalSelList = selList;
            this.currentSelList = new List<SASKind>();
        }

        private void LoadForm(object sender, EventArgs e)
        {
            if (BasisData.SASKindInfo == null ||
                BasisData.SASKindInfo.Values == null)
            {
                return;
            }

            foreach (SASKind kind in BasisData.SASKindInfo.Values)
            {
                SASKind copy = new SASKind();
                copy.DeepCopyFrom(kind);

                int itemIndex = this.chkListBoxStdAlertSystemKind.Items.Add(copy);
                if (this.originalSelList.Count == 0)
                {
                    this.chkListBoxStdAlertSystemKind.SetItemCheckState(itemIndex, CheckState.Checked);
                }
            }

            for (int i = 0; i < this.originalSelList.Count; i++)
            {
                for (int index = 0; index < this.chkListBoxStdAlertSystemKind.Items.Count; index++)
                {
                    SASKind ctrlKind = this.chkListBoxStdAlertSystemKind.Items[index] as SASKind;
                    if (ctrlKind == null)
                    {
                        System.Console.WriteLine("[SelectionStdAlertSystemKind] 체크 리스트 => 오브젝트 변환 실패");
                        continue;
                    }

                    if (this.originalSelList[i].Code == ctrlKind.Code)
                    {
                        this.chkListBoxStdAlertSystemKind.SetItemChecked(index, true);
                        break;
                    }
                }
            }
        }

        private void chkListBoxStdAlertSystemKind_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox ctrl = sender as CheckedListBox;


        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.None;

            bool isAllSelected = false;
            this.currentSelList.Clear();

            if (this.chkListBoxStdAlertSystemKind.CheckedItems == null || this.chkListBoxStdAlertSystemKind.CheckedItems.Count == 0)
            {
                MessageBox.Show("하나 이상의 표준경보시스템 종류를 선택해야 합니다.");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }
            else
            {
                for (int index = 0; index < this.chkListBoxStdAlertSystemKind.CheckedItems.Count; index++)
                {
                    SASKind kindInfo = this.chkListBoxStdAlertSystemKind.CheckedItems[index] as SASKind;
                    if (kindInfo == null)
                    {
                        continue;
                    }
                    this.currentSelList.Add(kindInfo);
                }
                if (this.currentSelList.Count == this.chkListBoxStdAlertSystemKind.Items.Count)
                {
                    isAllSelected = true;
                }
            }

            if (this.NotifySASKindChanged != null)
            {
                this.NotifySASKindChanged(this, new SASKindEventArgs(isAllSelected, this.currentSelList));
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
    /// <summary>
    /// 표준경보시스템 종류 선택 변경  정보 전달 이벤트 아규먼트 클래스
    /// </summary>
    public class SASKindEventArgs : EventArgs
    {
        private bool isAllSelected = false;
        public bool IsAllSelected
        {
            get { return isAllSelected; }
            set { isAllSelected = value; }
        }
        private List<SASKind> selectedSystemKinds;
        public List<SASKind> SelectedSystemKinds
        {
            get { return selectedSystemKinds; }
            set { selectedSystemKinds = value; }
        }

        public SASKindEventArgs()
        {
            this.selectedSystemKinds = new List<SASKind>();
        }
        public SASKindEventArgs(bool isAll, List<SASKind> selectedKindList)
        {
            this.isAllSelected = isAll;
            this.selectedSystemKinds = selectedKindList;
        }
    }
}
