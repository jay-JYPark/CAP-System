using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WmaPAlmScreen
{
    public partial class WrongOperationForm : Form
    {
        public WrongOperationForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="text"></param>
        public WrongOperationForm(string text)
            : this()
        {
            this.textLabel.Text = text;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}