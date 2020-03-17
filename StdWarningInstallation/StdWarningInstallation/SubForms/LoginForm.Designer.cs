namespace StdWarningInstallation
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.lblID = new Adeng.Framework.Ctrl.LabelEx();
            this.lblPassword = new Adeng.Framework.Ctrl.LabelEx();
            this.txtboxID = new Adeng.Framework.Ctrl.TextBoxEx();
            this.txtboxPassword = new Adeng.Framework.Ctrl.TextBoxEx();
            this.btnLogin = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.lblTitle = new Adeng.Framework.Ctrl.LabelEx();
            this.lblExit = new Adeng.Framework.Ctrl.LabelEx();
            this.lblLoginState = new Adeng.Framework.Ctrl.LabelEx();
            this.progressBar = new Adeng.Framework.Ctrl.ProgressBarEx();
            this.SuspendLayout();
            // 
            // lblID
            // 
            this.lblID.BackColor = System.Drawing.Color.Transparent;
            this.lblID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblID.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(93)))), ((int)(((byte)(159)))));
            this.lblID.Location = new System.Drawing.Point(4, 151);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(114, 23);
            this.lblID.TabIndex = 0;
            this.lblID.Text = "아이디";
            this.lblID.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPassword
            // 
            this.lblPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblPassword.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblPassword.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(93)))), ((int)(((byte)(159)))));
            this.lblPassword.Location = new System.Drawing.Point(4, 185);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(114, 23);
            this.lblPassword.TabIndex = 0;
            this.lblPassword.Text = "비밀번호";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtboxID
            // 
            this.txtboxID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtboxID.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxID.Location = new System.Drawing.Point(128, 146);
            this.txtboxID.MaxLength = 15;
            this.txtboxID.Name = "txtboxID";
            this.txtboxID.Size = new System.Drawing.Size(182, 32);
            this.txtboxID.TabIndex = 1;
            this.txtboxID.TextChanged += new System.EventHandler(this.txtbox_TextChanged);
            this.txtboxID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbox_KeyPress);
            // 
            // txtboxPassword
            // 
            this.txtboxPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtboxPassword.Font = new System.Drawing.Font("굴림", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txtboxPassword.Location = new System.Drawing.Point(128, 181);
            this.txtboxPassword.MaxLength = 15;
            this.txtboxPassword.Name = "txtboxPassword";
            this.txtboxPassword.Size = new System.Drawing.Size(182, 32);
            this.txtboxPassword.TabIndex = 2;
            this.txtboxPassword.UseSystemPasswordChar = true;
            this.txtboxPassword.TextChanged += new System.EventHandler(this.txtbox_TextChanged);
            this.txtboxPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbox_KeyPress);
            // 
            // btnLogin
            // 
            this.btnLogin.ChkValue = false;
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnLogin.Enabled = false;
            this.btnLogin.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.Image = ((System.Drawing.Image)(resources.GetObject("btnLogin.Image")));
            this.btnLogin.ImgDisable = null;
            this.btnLogin.ImgHover = ((System.Drawing.Image)(resources.GetObject("btnLogin.ImgHover")));
            this.btnLogin.ImgSelect = null;
            this.btnLogin.ImgStatusEvent = null;
            this.btnLogin.ImgStatusNormal = null;
            this.btnLogin.ImgStatusOffsetX = 2;
            this.btnLogin.ImgStatusOffsetY = 0;
            this.btnLogin.ImgStretch = false;
            this.btnLogin.IsImgStatusNormal = true;
            this.btnLogin.Location = new System.Drawing.Point(316, 146);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(60, 63);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "로그인";
            this.btnLogin.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnLogin.UseChecked = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoEllipsis = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblTitle.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(93)))), ((int)(((byte)(159)))));
            this.lblTitle.Location = new System.Drawing.Point(2, 47);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(417, 44);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "통합경보시스템 표준발령대";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblExit
            // 
            this.lblExit.BackColor = System.Drawing.Color.Transparent;
            this.lblExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblExit.Image = ((System.Drawing.Image)(resources.GetObject("lblExit.Image")));
            this.lblExit.Location = new System.Drawing.Point(391, 7);
            this.lblExit.Name = "lblExit";
            this.lblExit.Size = new System.Drawing.Size(24, 24);
            this.lblExit.TabIndex = 4;
            this.lblExit.Click += new System.EventHandler(this.lblExit_Click);
            // 
            // lblLoginState
            // 
            this.lblLoginState.BackColor = System.Drawing.Color.Transparent;
            this.lblLoginState.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblLoginState.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblLoginState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(93)))), ((int)(((byte)(159)))));
            this.lblLoginState.Location = new System.Drawing.Point(130, 220);
            this.lblLoginState.Name = "lblLoginState";
            this.lblLoginState.Size = new System.Drawing.Size(246, 31);
            this.lblLoginState.TabIndex = 0;
            this.lblLoginState.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(77, 98);
            this.progressBar.Maximum = 500;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(268, 22);
            this.progressBar.TabIndex = 5;
            this.progressBar.Visible = false;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(422, 282);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblLoginState);
            this.Controls.Add(this.lblExit);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtboxPassword);
            this.Controls.Add(this.txtboxID);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.lblID);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Adeng.Framework.Ctrl.LabelEx lblID;
        private Adeng.Framework.Ctrl.LabelEx lblPassword;
        private Adeng.Framework.Ctrl.TextBoxEx txtboxID;
        private Adeng.Framework.Ctrl.TextBoxEx txtboxPassword;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnLogin;
        private Adeng.Framework.Ctrl.LabelEx lblTitle;
        private Adeng.Framework.Ctrl.LabelEx lblExit;
        private Adeng.Framework.Ctrl.LabelEx lblLoginState;
        private Adeng.Framework.Ctrl.ProgressBarEx progressBar;
    }
}