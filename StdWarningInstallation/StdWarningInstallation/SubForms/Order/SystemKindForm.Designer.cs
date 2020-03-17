namespace StdWarningInstallation
{
    partial class SystemKindForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemKindForm));
            this.btnCancel = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnOK = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTopDescription = new System.Windows.Forms.Label();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.chkListBoxStdAlertSystemKind = new System.Windows.Forms.CheckedListBox();
            this.pnlTop.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.ChkValue = false;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImgDisable = null;
            this.btnCancel.ImgHover = null;
            this.btnCancel.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImgSelect")));
            this.btnCancel.ImgStatusEvent = null;
            this.btnCancel.ImgStatusNormal = null;
            this.btnCancel.ImgStatusOffsetX = 2;
            this.btnCancel.ImgStatusOffsetY = 0;
            this.btnCancel.ImgStretch = false;
            this.btnCancel.IsImgStatusNormal = true;
            this.btnCancel.Location = new System.Drawing.Point(188, 292);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 36);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "닫기";
            this.btnCancel.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnCancel.UseChecked = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.ChkValue = false;
            this.btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Image = ((System.Drawing.Image)(resources.GetObject("btnOK.Image")));
            this.btnOK.ImgDisable = null;
            this.btnOK.ImgHover = null;
            this.btnOK.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnOK.ImgSelect")));
            this.btnOK.ImgStatusEvent = null;
            this.btnOK.ImgStatusNormal = null;
            this.btnOK.ImgStatusOffsetX = 2;
            this.btnOK.ImgStatusOffsetY = 0;
            this.btnOK.ImgStretch = false;
            this.btnOK.IsImgStatusNormal = true;
            this.btnOK.Location = new System.Drawing.Point(92, 292);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 36);
            this.btnOK.TabIndex = 10;
            this.btnOK.Text = "확인";
            this.btnOK.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnOK.UseChecked = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlTop.BackgroundImage")));
            this.pnlTop.Controls.Add(this.lblTopDescription);
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(285, 40);
            this.pnlTop.TabIndex = 8;
            // 
            // lblTopDescription
            // 
            this.lblTopDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblTopDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblTopDescription.Image = ((System.Drawing.Image)(resources.GetObject("lblTopDescription.Image")));
            this.lblTopDescription.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTopDescription.Location = new System.Drawing.Point(0, 0);
            this.lblTopDescription.Margin = new System.Windows.Forms.Padding(0);
            this.lblTopDescription.Name = "lblTopDescription";
            this.lblTopDescription.Padding = new System.Windows.Forms.Padding(14, 0, 7, 0);
            this.lblTopDescription.Size = new System.Drawing.Size(285, 40);
            this.lblTopDescription.TabIndex = 0;
            this.lblTopDescription.Text = "시스템 종류를 선택합니다.";
            this.lblTopDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.Color.White;
            this.pnlBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBody.Controls.Add(this.chkListBoxStdAlertSystemKind);
            this.pnlBody.Location = new System.Drawing.Point(7, 47);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(271, 233);
            this.pnlBody.TabIndex = 13;
            // 
            // chkListBoxStdAlertSystemKind
            // 
            this.chkListBoxStdAlertSystemKind.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chkListBoxStdAlertSystemKind.CheckOnClick = true;
            this.chkListBoxStdAlertSystemKind.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.chkListBoxStdAlertSystemKind.FormattingEnabled = true;
            this.chkListBoxStdAlertSystemKind.HorizontalScrollbar = true;
            this.chkListBoxStdAlertSystemKind.Location = new System.Drawing.Point(7, 7);
            this.chkListBoxStdAlertSystemKind.Margin = new System.Windows.Forms.Padding(0);
            this.chkListBoxStdAlertSystemKind.Name = "chkListBoxStdAlertSystemKind";
            this.chkListBoxStdAlertSystemKind.Size = new System.Drawing.Size(257, 220);
            this.chkListBoxStdAlertSystemKind.TabIndex = 13;
            // 
            // SystemKindForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 335);
            this.ControlBox = false;
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pnlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SystemKindForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "표준경보시스템 종류 설정";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.LoadForm);
            this.pnlTop.ResumeLayout(false);
            this.pnlBody.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Adeng.Framework.Ctrl.AdengImgButtonEx btnCancel;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnOK;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTopDescription;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.CheckedListBox chkListBoxStdAlertSystemKind;
    }
}