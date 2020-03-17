using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StdWarningInstallation
{
    public partial class ProgramInformationForm : Form
    {
        private string currentProgramName = "통합경보시스템 표준발령대(SWI)";
        private string currentVersionInfo = "Version 2.0.0 (2015.01.16)";

        public ProgramInformationForm()
        {
            InitializeComponent();

        }
        public ProgramInformationForm(string versionInfo)
        {
            InitializeComponent();

            this.currentVersionInfo = versionInfo;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.lblSystemName.Text = this.currentProgramName;
            this.lblVersion.Text = this.currentVersionInfo;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
