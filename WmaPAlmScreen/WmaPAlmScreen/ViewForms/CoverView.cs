using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NCASBIZ.NCasData;
using NCASBIZ.NCasUtility;

namespace WmaPAlmScreen
{
    public partial class CoverView : ViewBase
    {
        /// <summary>
        /// 생성자
        /// </summary>
        public CoverView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="main">MainForm에서 넘겨주는 참조</param>
        public CoverView(MainForm main)
            : this()
        {
            this.main = main;
            //this.verLabel.Text = NCasUtilityMng.INCasEtcUtility.GetVersionInfo();
        }

        private void verLabel_DoubleClick(object sender, EventArgs e)
        {
        }
    }
}