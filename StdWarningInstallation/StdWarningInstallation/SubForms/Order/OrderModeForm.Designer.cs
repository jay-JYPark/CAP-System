namespace StdWarningInstallation
{
    partial class OrderModeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderModeForm));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTopDescription = new System.Windows.Forms.Label();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.btnOrderModeTest = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnOrderModeExcercise = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnOrderModeActual = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnCancel = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnOK = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlTop.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlTop.BackgroundImage")));
            this.pnlTop.Controls.Add(this.lblTopDescription);
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(349, 40);
            this.pnlTop.TabIndex = 2;
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
            this.lblTopDescription.Size = new System.Drawing.Size(349, 40);
            this.lblTopDescription.TabIndex = 0;
            this.lblTopDescription.Text = "발령 모드를 선택합니다.";
            this.lblTopDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBody
            // 
            this.pnlBody.BackColor = System.Drawing.Color.White;
            this.pnlBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBody.Controls.Add(this.btnOrderModeTest);
            this.pnlBody.Controls.Add(this.btnOrderModeExcercise);
            this.pnlBody.Controls.Add(this.btnOrderModeActual);
            this.pnlBody.Location = new System.Drawing.Point(7, 49);
            this.pnlBody.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(332, 67);
            this.pnlBody.TabIndex = 5;
            // 
            // btnOrderModeTest
            // 
            this.btnOrderModeTest.ChkValue = false;
            this.btnOrderModeTest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOrderModeTest.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOrderModeTest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(47)))));
            this.btnOrderModeTest.Image = ((System.Drawing.Image)(resources.GetObject("btnOrderModeTest.Image")));
            this.btnOrderModeTest.ImgDisable = null;
            this.btnOrderModeTest.ImgHover = null;
            this.btnOrderModeTest.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnOrderModeTest.ImgSelect")));
            this.btnOrderModeTest.ImgStatusEvent = null;
            this.btnOrderModeTest.ImgStatusNormal = null;
            this.btnOrderModeTest.ImgStatusOffsetX = 2;
            this.btnOrderModeTest.ImgStatusOffsetY = 0;
            this.btnOrderModeTest.ImgStretch = false;
            this.btnOrderModeTest.IsImgStatusNormal = true;
            this.btnOrderModeTest.Location = new System.Drawing.Point(221, 15);
            this.btnOrderModeTest.Name = "btnOrderModeTest";
            this.btnOrderModeTest.Size = new System.Drawing.Size(100, 36);
            this.btnOrderModeTest.TabIndex = 4;
            this.btnOrderModeTest.Text = "시험";
            this.btnOrderModeTest.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnOrderModeTest.UseChecked = true;
            this.btnOrderModeTest.Click += new System.EventHandler(this.btnOrderMode_Click);
            // 
            // btnOrderModeExcercise
            // 
            this.btnOrderModeExcercise.ChkValue = false;
            this.btnOrderModeExcercise.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOrderModeExcercise.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOrderModeExcercise.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(47)))));
            this.btnOrderModeExcercise.Image = ((System.Drawing.Image)(resources.GetObject("btnOrderModeExcercise.Image")));
            this.btnOrderModeExcercise.ImgDisable = null;
            this.btnOrderModeExcercise.ImgHover = null;
            this.btnOrderModeExcercise.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnOrderModeExcercise.ImgSelect")));
            this.btnOrderModeExcercise.ImgStatusEvent = null;
            this.btnOrderModeExcercise.ImgStatusNormal = null;
            this.btnOrderModeExcercise.ImgStatusOffsetX = 2;
            this.btnOrderModeExcercise.ImgStatusOffsetY = 0;
            this.btnOrderModeExcercise.ImgStretch = false;
            this.btnOrderModeExcercise.IsImgStatusNormal = true;
            this.btnOrderModeExcercise.Location = new System.Drawing.Point(115, 15);
            this.btnOrderModeExcercise.Name = "btnOrderModeExcercise";
            this.btnOrderModeExcercise.Size = new System.Drawing.Size(100, 36);
            this.btnOrderModeExcercise.TabIndex = 3;
            this.btnOrderModeExcercise.Text = "훈련";
            this.btnOrderModeExcercise.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnOrderModeExcercise.UseChecked = true;
            this.btnOrderModeExcercise.Click += new System.EventHandler(this.btnOrderMode_Click);
            // 
            // btnOrderModeActual
            // 
            this.btnOrderModeActual.ChkValue = false;
            this.btnOrderModeActual.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOrderModeActual.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOrderModeActual.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(47)))));
            this.btnOrderModeActual.Image = ((System.Drawing.Image)(resources.GetObject("btnOrderModeActual.Image")));
            this.btnOrderModeActual.ImgDisable = null;
            this.btnOrderModeActual.ImgHover = null;
            this.btnOrderModeActual.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnOrderModeActual.ImgSelect")));
            this.btnOrderModeActual.ImgStatusEvent = null;
            this.btnOrderModeActual.ImgStatusNormal = null;
            this.btnOrderModeActual.ImgStatusOffsetX = 2;
            this.btnOrderModeActual.ImgStatusOffsetY = 0;
            this.btnOrderModeActual.ImgStretch = false;
            this.btnOrderModeActual.IsImgStatusNormal = true;
            this.btnOrderModeActual.Location = new System.Drawing.Point(9, 15);
            this.btnOrderModeActual.Name = "btnOrderModeActual";
            this.btnOrderModeActual.Size = new System.Drawing.Size(100, 36);
            this.btnOrderModeActual.TabIndex = 2;
            this.btnOrderModeActual.Text = "실제";
            this.btnOrderModeActual.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnOrderModeActual.UseChecked = true;
            this.btnOrderModeActual.Click += new System.EventHandler(this.btnOrderMode_Click);
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
            this.btnCancel.Location = new System.Drawing.Point(249, 123);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 36);
            this.btnCancel.TabIndex = 7;
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
            this.btnOK.Location = new System.Drawing.Point(153, 123);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 36);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "확인";
            this.btnOK.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnOK.UseChecked = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // OrderModeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(346, 165);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderModeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "발령 모드 변경";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.LoadForm);
            this.pnlTop.ResumeLayout(false);
            this.pnlBody.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTopDescription;
        private System.Windows.Forms.Panel pnlBody;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnOrderModeTest;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnOrderModeExcercise;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnOrderModeActual;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnCancel;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnOK;

    }
}