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
    public partial class OrderConfirmForm : Form
    {
        /// <summary>
        /// 비밀번호가 일치하면 발생하는 이벤트
        /// </summary>
        public event EventHandler PasswordConfirmEvent;

        /// <summary>
        /// 생성자
        /// </summary>
        public OrderConfirmForm()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }

        /// <summary>
        /// 확인 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (PasswordMng.Password.ToLower() != this.passwordConfirmTextbox.Text.ToLower())
            {
                this.passwordConfirmTextbox.Text = string.Empty;
                this.errorLabel.Visible = true;
                return;
            }

            if (this.PasswordConfirmEvent != null)
            {
                this.PasswordConfirmEvent(this, new EventArgs());
            }

            this.Close();
        }

        /// <summary>
        /// 취소 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 비밀번호 입력 텍스트박스 KeyDown 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void passwordConfirmTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btnOk_Click(sender, e);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                this.btnCancel_Click(sender, e);
            }
        }

        /// <summary>
        /// 비밀번호 입력 텍스트박스 TextChanged 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void passwordConfirmTextbox_TextChanged(object sender, EventArgs e)
        {
            this.errorLabel.Visible = false;
        }
    }
}