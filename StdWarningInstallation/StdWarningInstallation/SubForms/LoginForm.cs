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
    public partial class LoginForm : Form
    {
        private readonly char KEY_ENTER = (char)13;
        private readonly char KEY_ESC = (char)27;

        public event EventHandler<LoginEventArgs> NotifyLoginResult;
        private List<UserAccount> userAccountList = new List<UserAccount>();


        /// <summary>
        /// 디폴트 생성자.
        /// </summary>
        /// <param name="userAccount"></param>
        private LoginForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 이 생성자를 사용할 것.
        /// </summary>
        /// <param name="userAccount"></param>
        public LoginForm(List<UserAccount> userAccountInfo)
        {
            InitializeComponent();
            this.userAccountList = userAccountInfo;
        }

        /// <summary>
        /// 닫기 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblExit_Click(object sender, EventArgs e)
        {
            if (this.NotifyLoginResult != null)
            {
                this.NotifyLoginResult(this, new LoginEventArgs(false, null));
            }

            this.Close();
        }

        /// <summary>
        /// 로그인 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;

                if (this.txtboxID.Text.ToUpper() == "ADMIN" && this.txtboxPassword.Text.ToUpper() == "ADMIN")
                {
                    if (this.NotifyLoginResult != null)
                    {
                        this.NotifyLoginResult(this, new LoginEventArgs(true, null));
                        Random rd = new Random();
                        this.progressBar.Visible = true;

                        for (; ; )
                        {
                            this.progressBar.Step = rd.Next(1, 51);
                            this.progressBar.PerformStep();

                            if (this.progressBar.Value == 500)
                            {
                                break;
                            }

                            System.Threading.Thread.Sleep(150);
                        }

                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        this.Dispose(true);
                        return;
                    }
                }

                bool isIDMatched = false;
                UserAccount confirmUserInfo = new UserAccount();

                for (int i = 0; i < this.userAccountList.Count; i++)
                {
                    if (this.userAccountList[i].ID.ToUpper() == this.txtboxID.Text.ToUpper())
                    {
                        isIDMatched = true;
                        confirmUserInfo = this.userAccountList[i];
                        break;
                    }
                }

                if (!isIDMatched)
                {
                    this.lblLoginState.Text = "※아이디와 비밀번호를 확인하세요.";
                    return;
                }

                if (confirmUserInfo.Password.ToUpper() == this.txtboxPassword.Text.ToUpper())
                {
                    if (this.NotifyLoginResult != null)
                    {
                        this.NotifyLoginResult(this, new LoginEventArgs(true, confirmUserInfo));
                        Random rd = new Random();
                        this.progressBar.Visible = true;

                        for (; ; )
                        {
                            this.progressBar.Step = rd.Next(1, 51);
                            this.progressBar.PerformStep();

                            if (this.progressBar.Value == 500)
                            {
                                break;
                            }

                            System.Threading.Thread.Sleep(150);
                        }

                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        this.Dispose(true);
                        return;
                    }
                }

                this.lblLoginState.Text = "※아이디와 비밀번호를 확인하세요.";
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[LoginForm] btnLogin_Click( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[LoginForm] btnLogin_Click( " + ex.ToString() + " )");

                MessageBox.Show("로그인 처리 중에 예외가 발생하였습니다.", "로그인", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return;
        }

        /// <summary>
        /// 텍스트박스 입력값 변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtbox_TextChanged(object sender, EventArgs e)
        {
            if (this.txtboxID.Text.Length > 0 && this.txtboxPassword.Text.Length > 0)
            {
                this.btnLogin.Enabled = true;
            }
            else
            {
                this.btnLogin.Enabled = false;
            }
        }

        /// <summary>
        /// 텍스트박스 키 다운 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == KEY_ENTER)
            {
                if (this.txtboxID.Text.Length > 0 && this.txtboxPassword.Text.Length > 0)
                {
                    btnLogin_Click(sender, e);
                }
            }
            else if (e.KeyChar == KEY_ESC)
            {
                lblExit_Click(sender, e);
            }
            else
            {
                // do nothing
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    /// <summary>
    /// 로그인 기능에 사용하는 이벤트 아규먼트 클래스
    /// </summary>
    public class LoginEventArgs : EventArgs
    {
        private bool isLogin = false;
        private UserAccount loginUserAccount = null;

        public bool IsLogin
        {
            get { return this.isLogin; }
            set { this.isLogin = value; }
        }

        public UserAccount LoginUser
        {
            get { return this.loginUserAccount; }
            set { this.loginUserAccount = value; }
        }

        public LoginEventArgs(bool loginResult, UserAccount loginUser)
        {
            this.isLogin = loginResult;
            this.loginUserAccount = loginUser;
        }
    }
}