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
    public partial class InformationForm : Form
    {
        public InformationForm(string _version)
        {
            InitializeComponent();
            this.MainLB.Text = _version;
        }

        /// <summary>
        /// 폼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InformationForm_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}