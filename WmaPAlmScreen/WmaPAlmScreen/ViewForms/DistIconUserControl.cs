using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NCASFND.NCasCtrl;

namespace WmaPAlmScreen
{
    public partial class DistIconUserControl : UserControl
    {
        /// <summary>
        /// 생성자
        /// </summary>
        public DistIconUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 시군 아이콘 버튼 프로퍼티
        /// </summary>
        public NCasButton DistIcon
        {
            get { return this.distIconBtn; }
            set { this.distIconBtn = value; }
        }

        /// <summary>
        /// 시군 아이콘 이름 프로퍼티
        /// </summary>
        public string DistName
        {
            get { return this.distName.Text; }
            set { this.distName.Text = value; }
        }
    }
}