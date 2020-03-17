using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StdWarningInstallation.Ctrl
{
    public partial class SWRConditionPart : UserControl
    {
        /// <summary>
        /// 특보 종류
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                return this.lblTitle.Text;
            }
            set
            {
                this.lblTitle.Text = value;
            }
        }
        /// <summary>
        /// 주의보
        /// </summary>
        public bool Watch
        {
            get
            {
                return this.chkboxWatch.Checked;
            }
            set
            {
                this.chkboxWatch.Checked = value;
            }
        }
        /// <summary>
        /// 경보
        /// </summary>
        public bool Warning
        {
            get
            {
                return this.chkboxWarning.Checked;
            }
            set
            {
                this.chkboxWarning.Checked = value;
            }
        }
        public SWRConditionPart()
        {
            InitializeComponent();
        }
    }
}
