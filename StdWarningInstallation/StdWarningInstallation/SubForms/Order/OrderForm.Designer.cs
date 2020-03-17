namespace StdWarningInstallation
{
    partial class OrderForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderForm));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblDescription = new System.Windows.Forms.Label();
            this.pnlBody = new System.Windows.Forms.Panel();
            this.pnlSubMenuBackground = new System.Windows.Forms.Panel();
            this.btnMsgText = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnStdAlertSystemKind = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnOrderMode = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.lblSubMenuTitle = new System.Windows.Forms.Label();
            this.pnlDisasterBackground = new System.Windows.Forms.Panel();
            this.cmbboxDisasterKind = new System.Windows.Forms.ComboBox();
            this.cmbboxDisasterCategory = new System.Windows.Forms.ComboBox();
            this.lblDisasterTitle = new System.Windows.Forms.Label();
            this.lblOrderMode = new System.Windows.Forms.Label();
            this.btnCancel = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnOrder = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.lblHeadline = new System.Windows.Forms.Label();
            this.pnlTop.SuspendLayout();
            this.pnlBody.SuspendLayout();
            this.pnlSubMenuBackground.SuspendLayout();
            this.pnlDisasterBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlTop.BackgroundImage")));
            this.pnlTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlTop.Controls.Add(this.lblDescription);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(500, 40);
            this.pnlTop.TabIndex = 0;
            // 
            // lblDescription
            // 
            this.lblDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblDescription.Image = ((System.Drawing.Image)(resources.GetObject("lblDescription.Image")));
            this.lblDescription.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDescription.Location = new System.Drawing.Point(0, 0);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Padding = new System.Windows.Forms.Padding(14, 0, 7, 0);
            this.lblDescription.Size = new System.Drawing.Size(500, 40);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "발령 정보를 확인하거나 설정합니다.";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBody
            // 
            this.pnlBody.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBody.BackColor = System.Drawing.Color.White;
            this.pnlBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBody.Controls.Add(this.pnlSubMenuBackground);
            this.pnlBody.Controls.Add(this.pnlDisasterBackground);
            this.pnlBody.Controls.Add(this.lblOrderMode);
            this.pnlBody.Location = new System.Drawing.Point(7, 48);
            this.pnlBody.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBody.Name = "pnlBody";
            this.pnlBody.Size = new System.Drawing.Size(486, 158);
            this.pnlBody.TabIndex = 4;
            // 
            // pnlSubMenuBackground
            // 
            this.pnlSubMenuBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.pnlSubMenuBackground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSubMenuBackground.Controls.Add(this.btnMsgText);
            this.pnlSubMenuBackground.Controls.Add(this.btnStdAlertSystemKind);
            this.pnlSubMenuBackground.Controls.Add(this.btnOrderMode);
            this.pnlSubMenuBackground.Controls.Add(this.lblSubMenuTitle);
            this.pnlSubMenuBackground.Location = new System.Drawing.Point(7, 98);
            this.pnlSubMenuBackground.Name = "pnlSubMenuBackground";
            this.pnlSubMenuBackground.Size = new System.Drawing.Size(470, 50);
            this.pnlSubMenuBackground.TabIndex = 5;
            // 
            // btnMsgText
            // 
            this.btnMsgText.ChkValue = false;
            this.btnMsgText.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMsgText.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnMsgText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(47)))));
            this.btnMsgText.Image = ((System.Drawing.Image)(resources.GetObject("btnMsgText.Image")));
            this.btnMsgText.ImgDisable = null;
            this.btnMsgText.ImgHover = null;
            this.btnMsgText.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnMsgText.ImgSelect")));
            this.btnMsgText.ImgStatusEvent = null;
            this.btnMsgText.ImgStatusNormal = null;
            this.btnMsgText.ImgStatusOffsetX = 2;
            this.btnMsgText.ImgStatusOffsetY = 0;
            this.btnMsgText.ImgStretch = false;
            this.btnMsgText.IsImgStatusNormal = true;
            this.btnMsgText.Location = new System.Drawing.Point(324, 6);
            this.btnMsgText.Name = "btnMsgText";
            this.btnMsgText.Size = new System.Drawing.Size(100, 36);
            this.btnMsgText.TabIndex = 1;
            this.btnMsgText.Text = "문안";
            this.btnMsgText.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnMsgText.UseChecked = true;
            this.btnMsgText.Click += new System.EventHandler(this.btnMsgText_Click);
            // 
            // btnStdAlertSystemKind
            // 
            this.btnStdAlertSystemKind.ChkValue = false;
            this.btnStdAlertSystemKind.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStdAlertSystemKind.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnStdAlertSystemKind.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(47)))));
            this.btnStdAlertSystemKind.Image = ((System.Drawing.Image)(resources.GetObject("btnStdAlertSystemKind.Image")));
            this.btnStdAlertSystemKind.ImgDisable = null;
            this.btnStdAlertSystemKind.ImgHover = null;
            this.btnStdAlertSystemKind.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnStdAlertSystemKind.ImgSelect")));
            this.btnStdAlertSystemKind.ImgStatusEvent = null;
            this.btnStdAlertSystemKind.ImgStatusNormal = null;
            this.btnStdAlertSystemKind.ImgStatusOffsetX = 2;
            this.btnStdAlertSystemKind.ImgStatusOffsetY = 0;
            this.btnStdAlertSystemKind.ImgStretch = false;
            this.btnStdAlertSystemKind.IsImgStatusNormal = true;
            this.btnStdAlertSystemKind.Location = new System.Drawing.Point(218, 6);
            this.btnStdAlertSystemKind.Name = "btnStdAlertSystemKind";
            this.btnStdAlertSystemKind.Size = new System.Drawing.Size(100, 36);
            this.btnStdAlertSystemKind.TabIndex = 1;
            this.btnStdAlertSystemKind.Text = "시스템 종류";
            this.btnStdAlertSystemKind.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnStdAlertSystemKind.UseChecked = true;
            this.btnStdAlertSystemKind.Click += new System.EventHandler(this.btnStdAlertSystemKind_Click);
            // 
            // btnOrderMode
            // 
            this.btnOrderMode.ChkValue = false;
            this.btnOrderMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOrderMode.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOrderMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(47)))));
            this.btnOrderMode.Image = ((System.Drawing.Image)(resources.GetObject("btnOrderMode.Image")));
            this.btnOrderMode.ImgDisable = null;
            this.btnOrderMode.ImgHover = null;
            this.btnOrderMode.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnOrderMode.ImgSelect")));
            this.btnOrderMode.ImgStatusEvent = null;
            this.btnOrderMode.ImgStatusNormal = null;
            this.btnOrderMode.ImgStatusOffsetX = 2;
            this.btnOrderMode.ImgStatusOffsetY = 0;
            this.btnOrderMode.ImgStretch = false;
            this.btnOrderMode.IsImgStatusNormal = true;
            this.btnOrderMode.Location = new System.Drawing.Point(112, 6);
            this.btnOrderMode.Name = "btnOrderMode";
            this.btnOrderMode.Size = new System.Drawing.Size(100, 36);
            this.btnOrderMode.TabIndex = 1;
            this.btnOrderMode.Text = "모드";
            this.btnOrderMode.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnOrderMode.UseChecked = true;
            this.btnOrderMode.Click += new System.EventHandler(this.btnOrderMode_Click);
            // 
            // lblSubMenuTitle
            // 
            this.lblSubMenuTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSubMenuTitle.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSubMenuTitle.ForeColor = System.Drawing.Color.MidnightBlue;
            this.lblSubMenuTitle.Location = new System.Drawing.Point(0, 0);
            this.lblSubMenuTitle.Name = "lblSubMenuTitle";
            this.lblSubMenuTitle.Size = new System.Drawing.Size(82, 48);
            this.lblSubMenuTitle.TabIndex = 0;
            this.lblSubMenuTitle.Text = "정보 설정";
            this.lblSubMenuTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pnlDisasterBackground
            // 
            this.pnlDisasterBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.pnlDisasterBackground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDisasterBackground.Controls.Add(this.cmbboxDisasterKind);
            this.pnlDisasterBackground.Controls.Add(this.cmbboxDisasterCategory);
            this.pnlDisasterBackground.Controls.Add(this.lblDisasterTitle);
            this.pnlDisasterBackground.Location = new System.Drawing.Point(7, 44);
            this.pnlDisasterBackground.Name = "pnlDisasterBackground";
            this.pnlDisasterBackground.Size = new System.Drawing.Size(470, 47);
            this.pnlDisasterBackground.TabIndex = 5;
            // 
            // cmbboxDisasterKind
            // 
            this.cmbboxDisasterKind.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbboxDisasterKind.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbboxDisasterKind.FormattingEnabled = true;
            this.cmbboxDisasterKind.Location = new System.Drawing.Point(239, 11);
            this.cmbboxDisasterKind.Name = "cmbboxDisasterKind";
            this.cmbboxDisasterKind.Size = new System.Drawing.Size(185, 25);
            this.cmbboxDisasterKind.TabIndex = 1;
            this.cmbboxDisasterKind.SelectedIndexChanged += new System.EventHandler(this.cmbboxDisasterKind_SelectedIndexChanged);
            // 
            // cmbboxDisasterCategory
            // 
            this.cmbboxDisasterCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbboxDisasterCategory.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cmbboxDisasterCategory.FormattingEnabled = true;
            this.cmbboxDisasterCategory.ItemHeight = 17;
            this.cmbboxDisasterCategory.Location = new System.Drawing.Point(112, 11);
            this.cmbboxDisasterCategory.Name = "cmbboxDisasterCategory";
            this.cmbboxDisasterCategory.Size = new System.Drawing.Size(121, 25);
            this.cmbboxDisasterCategory.TabIndex = 1;
            this.cmbboxDisasterCategory.SelectedIndexChanged += new System.EventHandler(this.cmbboxDisasterCategory_SelectedIndexChanged);
            // 
            // lblDisasterTitle
            // 
            this.lblDisasterTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblDisasterTitle.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblDisasterTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblDisasterTitle.Location = new System.Drawing.Point(0, 0);
            this.lblDisasterTitle.Name = "lblDisasterTitle";
            this.lblDisasterTitle.Size = new System.Drawing.Size(82, 45);
            this.lblDisasterTitle.TabIndex = 0;
            this.lblDisasterTitle.Text = "재난 종류";
            this.lblDisasterTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblOrderMode
            // 
            this.lblOrderMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(149)))), ((int)(((byte)(30)))));
            this.lblOrderMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOrderMode.Font = new System.Drawing.Font("궁서", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblOrderMode.ForeColor = System.Drawing.Color.White;
            this.lblOrderMode.Location = new System.Drawing.Point(0, 0);
            this.lblOrderMode.Margin = new System.Windows.Forms.Padding(0);
            this.lblOrderMode.Name = "lblOrderMode";
            this.lblOrderMode.Size = new System.Drawing.Size(484, 35);
            this.lblOrderMode.TabIndex = 4;
            this.lblOrderMode.Text = "시험모드";
            this.lblOrderMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.btnCancel.Location = new System.Drawing.Point(401, 215);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 36);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "취소";
            this.btnCancel.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnCancel.UseChecked = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOrder
            // 
            this.btnOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOrder.ChkValue = false;
            this.btnOrder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOrder.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOrder.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOrder.ForeColor = System.Drawing.Color.White;
            this.btnOrder.Image = ((System.Drawing.Image)(resources.GetObject("btnOrder.Image")));
            this.btnOrder.ImgDisable = null;
            this.btnOrder.ImgHover = null;
            this.btnOrder.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnOrder.ImgSelect")));
            this.btnOrder.ImgStatusEvent = null;
            this.btnOrder.ImgStatusNormal = null;
            this.btnOrder.ImgStatusOffsetX = 2;
            this.btnOrder.ImgStatusOffsetY = 0;
            this.btnOrder.ImgStretch = false;
            this.btnOrder.IsImgStatusNormal = true;
            this.btnOrder.Location = new System.Drawing.Point(305, 215);
            this.btnOrder.Name = "btnOrder";
            this.btnOrder.Size = new System.Drawing.Size(90, 36);
            this.btnOrder.TabIndex = 1;
            this.btnOrder.Text = "발령";
            this.btnOrder.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnOrder.UseChecked = true;
            this.btnOrder.Click += new System.EventHandler(this.btnOrder_Click);
            // 
            // lblHeadline
            // 
            this.lblHeadline.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHeadline.AutoEllipsis = true;
            this.lblHeadline.BackColor = System.Drawing.Color.White;
            this.lblHeadline.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblHeadline.Font = new System.Drawing.Font("맑은 고딕", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblHeadline.ForeColor = System.Drawing.Color.Black;
            this.lblHeadline.Location = new System.Drawing.Point(7, 48);
            this.lblHeadline.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeadline.Name = "lblHeadline";
            this.lblHeadline.Padding = new System.Windows.Forms.Padding(1, 4, 1, 4);
            this.lblHeadline.Size = new System.Drawing.Size(486, 30);
            this.lblHeadline.TabIndex = 6;
            this.lblHeadline.Text = "Headline";
            this.lblHeadline.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHeadline.Visible = false;
            this.lblHeadline.Click += new System.EventHandler(this.lblHeadline_Click);
            // 
            // OrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(221)))), ((int)(((byte)(221)))));
            this.ClientSize = new System.Drawing.Size(500, 259);
            this.ControlBox = false;
            this.Controls.Add(this.lblHeadline);
            this.Controls.Add(this.pnlBody);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOrder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "발령 정보 설정";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.LoadForm);
            this.pnlTop.ResumeLayout(false);
            this.pnlBody.ResumeLayout(false);
            this.pnlSubMenuBackground.ResumeLayout(false);
            this.pnlDisasterBackground.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Panel pnlBody;
        private System.Windows.Forms.Label lblOrderMode;
        private System.Windows.Forms.Panel pnlDisasterBackground;
        private System.Windows.Forms.Label lblDisasterTitle;
        private System.Windows.Forms.ComboBox cmbboxDisasterKind;
        private System.Windows.Forms.ComboBox cmbboxDisasterCategory;
        private System.Windows.Forms.Panel pnlSubMenuBackground;
        private System.Windows.Forms.Label lblSubMenuTitle;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnOrderMode;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnStdAlertSystemKind;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnMsgText;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnCancel;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnOrder;
        private System.Windows.Forms.Label lblHeadline;
    }
}