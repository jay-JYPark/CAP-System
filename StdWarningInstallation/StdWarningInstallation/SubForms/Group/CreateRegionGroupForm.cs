using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using StdWarningInstallation.Ctrl;

namespace StdWarningInstallation
{
    public partial class CreateRegionGroupForm : RegionGroupBaseForm
    {
        public CreateRegionGroupForm()
        {
            InitializeComponent();

            base.SetProfileUpdateMode(DataClass.ProfileUpdateMode.Regist);
        }

        public override void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.IsProfileChanged())
            {
                string msg = "입력하신 그룹 정보가 저장되지 않았습니다. 현재 창을 닫으시겠습니까?";
                DialogResult answer = MessageBox.Show(msg, "그룹 등록", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (answer == System.Windows.Forms.DialogResult.Yes)
                {
                    this.Close();
                    return;
                }

                this.DialogResult = System.Windows.Forms.DialogResult.None;
            }
            else
            {
                this.Close();
            }
        }
    }
}
