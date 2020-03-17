namespace WmaPAlmScreen
{
    partial class OrderConfirmForm
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
            this.components = new System.ComponentModel.Container();
            this.btnOk = new NCASFND.NCasCtrl.NCasButton();
            this.middlePanel = new System.Windows.Forms.Panel();
            this.errorLabel = new System.Windows.Forms.Label();
            this.passwordConfirmTextbox = new System.Windows.Forms.TextBox();
            this.middleLabel = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.pictureBoxIcon = new System.Windows.Forms.PictureBox();
            this.topPanel = new System.Windows.Forms.Panel();
            this.btnCancel = new NCASFND.NCasCtrl.NCasButton();
            this.middlePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).BeginInit();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.AnimationInterval = 300;
            this.btnOk.CheckedValue = false;
            this.btnOk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOk.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Image = global::WmaPAlmScreen.WmaPAlmScreenRsc.btnSmallNormal;
            this.btnOk.ImgDisable = null;
            this.btnOk.ImgHover = null;
            this.btnOk.ImgSelect = global::WmaPAlmScreen.WmaPAlmScreenRsc.btnSmallPress;
            this.btnOk.ImgStatusEvent = null;
            this.btnOk.ImgStatusNormal = null;
            this.btnOk.ImgStatusOffsetX = 2;
            this.btnOk.ImgStatusOffsetY = 0;
            this.btnOk.IsStatusNormal = true;
            this.btnOk.Location = new System.Drawing.Point(63, 161);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(89, 39);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "확인";
            this.btnOk.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnOk.UseAutoWrap = false;
            this.btnOk.UseCheck = false;
            this.btnOk.UseImgStretch = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // middlePanel
            // 
            this.middlePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.middlePanel.Controls.Add(this.errorLabel);
            this.middlePanel.Controls.Add(this.passwordConfirmTextbox);
            this.middlePanel.Controls.Add(this.middleLabel);
            this.middlePanel.Location = new System.Drawing.Point(3, 65);
            this.middlePanel.Name = "middlePanel";
            this.middlePanel.Size = new System.Drawing.Size(238, 92);
            this.middlePanel.TabIndex = 9;
            // 
            // errorLabel
            // 
            this.errorLabel.AutoSize = true;
            this.errorLabel.Font = new System.Drawing.Font("나눔바른고딕 옛한글", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.errorLabel.ForeColor = System.Drawing.Color.White;
            this.errorLabel.Location = new System.Drawing.Point(65, 68);
            this.errorLabel.Name = "errorLabel";
            this.errorLabel.Size = new System.Drawing.Size(109, 17);
            this.errorLabel.TabIndex = 2;
            this.errorLabel.Text = "다시 입력하세요!";
            this.errorLabel.Visible = false;
            // 
            // passwordConfirmTextbox
            // 
            this.passwordConfirmTextbox.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.passwordConfirmTextbox.Location = new System.Drawing.Point(14, 35);
            this.passwordConfirmTextbox.MaxLength = 12;
            this.passwordConfirmTextbox.Name = "passwordConfirmTextbox";
            this.passwordConfirmTextbox.PasswordChar = '*';
            this.passwordConfirmTextbox.Size = new System.Drawing.Size(211, 27);
            this.passwordConfirmTextbox.TabIndex = 1;
            this.passwordConfirmTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.passwordConfirmTextbox.TextChanged += new System.EventHandler(this.passwordConfirmTextbox_TextChanged);
            this.passwordConfirmTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.passwordConfirmTextbox_KeyDown);
            // 
            // middleLabel
            // 
            this.middleLabel.AutoSize = true;
            this.middleLabel.Font = new System.Drawing.Font("나눔바른고딕 옛한글", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.middleLabel.ForeColor = System.Drawing.Color.White;
            this.middleLabel.Location = new System.Drawing.Point(9, 10);
            this.middleLabel.Name = "middleLabel";
            this.middleLabel.Size = new System.Drawing.Size(60, 17);
            this.middleLabel.TabIndex = 0;
            this.middleLabel.Text = "비밀번호";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.BackColor = System.Drawing.Color.Transparent;
            this.labelTitle.Font = new System.Drawing.Font("나눔바른고딕", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(8, 21);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(163, 19);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "실제발령을 확인합니다";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBoxIcon
            // 
            this.pictureBoxIcon.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxIcon.Image = global::WmaPAlmScreen.WmaPAlmScreenRsc.popIconKey;
            this.pictureBoxIcon.Location = new System.Drawing.Point(206, 12);
            this.pictureBoxIcon.Name = "pictureBoxIcon";
            this.pictureBoxIcon.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxIcon.TabIndex = 1;
            this.pictureBoxIcon.TabStop = false;
            // 
            // topPanel
            // 
            this.topPanel.Controls.Add(this.pictureBoxIcon);
            this.topPanel.Controls.Add(this.labelTitle);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(245, 56);
            this.topPanel.TabIndex = 8;
            // 
            // btnCancel
            // 
            this.btnCancel.AnimationInterval = 300;
            this.btnCancel.CheckedValue = false;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.Font = new System.Drawing.Font("나눔바른고딕", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Image = global::WmaPAlmScreen.WmaPAlmScreenRsc.btnSmallNormal;
            this.btnCancel.ImgDisable = null;
            this.btnCancel.ImgHover = null;
            this.btnCancel.ImgSelect = global::WmaPAlmScreen.WmaPAlmScreenRsc.btnSmallPress;
            this.btnCancel.ImgStatusEvent = null;
            this.btnCancel.ImgStatusNormal = null;
            this.btnCancel.ImgStatusOffsetX = 2;
            this.btnCancel.ImgStatusOffsetY = 0;
            this.btnCancel.IsStatusNormal = true;
            this.btnCancel.Location = new System.Drawing.Point(152, 161);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(89, 39);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "취소";
            this.btnCancel.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnCancel.UseAutoWrap = false;
            this.btnCancel.UseCheck = false;
            this.btnCancel.UseImgStretch = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // OrderConfirmForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ClientSize = new System.Drawing.Size(245, 202);
            this.ControlBox = false;
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.middlePanel);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderConfirmForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "발령 확인";
            this.TopMost = true;
            this.middlePanel.ResumeLayout(false);
            this.middlePanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxIcon)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private NCASFND.NCasCtrl.NCasButton btnOk;
        private System.Windows.Forms.Panel middlePanel;
        private System.Windows.Forms.Label errorLabel;
        private System.Windows.Forms.TextBox passwordConfirmTextbox;
        private System.Windows.Forms.Label middleLabel;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.PictureBox pictureBoxIcon;
        private System.Windows.Forms.Panel topPanel;
        private NCASFND.NCasCtrl.NCasButton btnCancel;
    }
}