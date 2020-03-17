namespace StdWarningInstallation
{
    partial class WaitToOrderSWRForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WaitToOrderSWRForm));
            this.lvWaitToOrderSWRList = new Adeng.Framework.Ctrl.AdengListView();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblButtonDescription = new System.Windows.Forms.Label();
            this.btnClose = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.lblListTitle = new System.Windows.Forms.Label();
            this.pnlBodyBackground = new System.Windows.Forms.Panel();
            this.btnDelete = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlListBack = new System.Windows.Forms.Panel();
            this.btnOrder = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlTop.SuspendLayout();
            this.pnlBodyBackground.SuspendLayout();
            this.pnlListBack.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvWaitToOrderSWRList
            // 
            this.lvWaitToOrderSWRList.AntiAlias = false;
            this.lvWaitToOrderSWRList.AutoFit = false;
            this.lvWaitToOrderSWRList.BackColor = System.Drawing.Color.White;
            this.lvWaitToOrderSWRList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvWaitToOrderSWRList.ColumnHeight = 24;
            this.lvWaitToOrderSWRList.ColumnOffset = new System.Windows.Forms.Padding(2, 2, 0, 1);
            this.lvWaitToOrderSWRList.ColumnStyle = Adeng.Framework.Ctrl.AdengListViewColumnStyle.DEF_STYLE;
            this.lvWaitToOrderSWRList.FocusedItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(185)))), ((int)(((byte)(230)))));
            this.lvWaitToOrderSWRList.FrozenColumnIndex = -1;
            this.lvWaitToOrderSWRList.FullRowSelect = true;
            this.lvWaitToOrderSWRList.GridDashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.lvWaitToOrderSWRList.GridLineStyle = Adeng.Framework.Ctrl.AdengListViewGridLine.NONE;
            this.lvWaitToOrderSWRList.HideSelection = false;
            this.lvWaitToOrderSWRList.HoverSelection = false;
            this.lvWaitToOrderSWRList.IconOffset = new System.Drawing.Point(1, 0);
            this.lvWaitToOrderSWRList.InteraceColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.lvWaitToOrderSWRList.InteraceColor2 = System.Drawing.Color.White;
            this.lvWaitToOrderSWRList.ItemHeight = 18;
            this.lvWaitToOrderSWRList.ItemOffset = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lvWaitToOrderSWRList.Location = new System.Drawing.Point(6, 35);
            this.lvWaitToOrderSWRList.MultiSelect = false;
            this.lvWaitToOrderSWRList.Name = "lvWaitToOrderSWRList";
            this.lvWaitToOrderSWRList.ScrollType = Adeng.Framework.Ctrl.AdengListViewScrollType.BOTH;
            this.lvWaitToOrderSWRList.SelectioinItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(232)))), ((int)(((byte)(250)))));
            this.lvWaitToOrderSWRList.Size = new System.Drawing.Size(628, 153);
            this.lvWaitToOrderSWRList.TabIndex = 14;
            this.lvWaitToOrderSWRList.UseInteraceColor = true;
            this.lvWaitToOrderSWRList.UseSelFocusedBar = false;
            this.lvWaitToOrderSWRList.SelectedIndexChanged += new Adeng.Framework.Ctrl.AdengEventHandler(this.lvWaitToOrderSWRList_SelectedIndexChanged);
            this.lvWaitToOrderSWRList.DoubleClick += new System.EventHandler(this.lvWaitToOrderSWRList_DoubleClick);
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
            this.pnlTop.Size = new System.Drawing.Size(666, 40);
            this.pnlTop.TabIndex = 5;
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
            this.lblDescription.Size = new System.Drawing.Size(666, 40);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "미발령 기상 특보를 확인 후 경보를 발령합니다.";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblButtonDescription
            // 
            this.lblButtonDescription.AutoSize = true;
            this.lblButtonDescription.ForeColor = System.Drawing.Color.Gray;
            this.lblButtonDescription.Location = new System.Drawing.Point(157, 225);
            this.lblButtonDescription.Name = "lblButtonDescription";
            this.lblButtonDescription.Size = new System.Drawing.Size(313, 12);
            this.lblButtonDescription.TabIndex = 5;
            this.lblButtonDescription.Text = "경보 발령이 필요 없는 항목을 관리 목록에서 삭제합니다.";
            this.lblButtonDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.ChkValue = false;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImgDisable = null;
            this.btnClose.ImgHover = null;
            this.btnClose.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnClose.ImgSelect")));
            this.btnClose.ImgStatusEvent = null;
            this.btnClose.ImgStatusNormal = null;
            this.btnClose.ImgStatusOffsetX = 2;
            this.btnClose.ImgStatusOffsetY = 0;
            this.btnClose.ImgStretch = false;
            this.btnClose.IsImgStatusNormal = true;
            this.btnClose.Location = new System.Drawing.Point(569, 308);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 36);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "닫기";
            this.btnClose.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnClose.UseChecked = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblListTitle
            // 
            this.lblListTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblListTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblListTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblListTitle.Location = new System.Drawing.Point(0, 0);
            this.lblListTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblListTitle.Name = "lblListTitle";
            this.lblListTitle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblListTitle.Size = new System.Drawing.Size(640, 32);
            this.lblListTitle.TabIndex = 0;
            this.lblListTitle.Text = "미발령 기상 특보 목록";
            this.lblListTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBodyBackground
            // 
            this.pnlBodyBackground.BackColor = System.Drawing.Color.White;
            this.pnlBodyBackground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBodyBackground.Controls.Add(this.lblButtonDescription);
            this.pnlBodyBackground.Controls.Add(this.btnDelete);
            this.pnlBodyBackground.Controls.Add(this.pnlListBack);
            this.pnlBodyBackground.Location = new System.Drawing.Point(6, 48);
            this.pnlBodyBackground.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBodyBackground.Name = "pnlBodyBackground";
            this.pnlBodyBackground.Size = new System.Drawing.Size(654, 250);
            this.pnlBodyBackground.TabIndex = 7;
            // 
            // btnDelete
            // 
            this.btnDelete.ChkValue = false;
            this.btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(18)))), ((int)(((byte)(47)))));
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImgDisable = null;
            this.btnDelete.ImgHover = null;
            this.btnDelete.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnDelete.ImgSelect")));
            this.btnDelete.ImgStatusEvent = null;
            this.btnDelete.ImgStatusNormal = null;
            this.btnDelete.ImgStatusOffsetX = 2;
            this.btnDelete.ImgStatusOffsetY = 0;
            this.btnDelete.ImgStretch = true;
            this.btnDelete.IsImgStatusNormal = true;
            this.btnDelete.Location = new System.Drawing.Point(6, 206);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(140, 36);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "선택 항목 연계 제외";
            this.btnDelete.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnDelete.UseChecked = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // pnlListBack
            // 
            this.pnlListBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.pnlListBack.Controls.Add(this.lvWaitToOrderSWRList);
            this.pnlListBack.Controls.Add(this.lblListTitle);
            this.pnlListBack.Location = new System.Drawing.Point(6, 6);
            this.pnlListBack.Name = "pnlListBack";
            this.pnlListBack.Size = new System.Drawing.Size(640, 194);
            this.pnlListBack.TabIndex = 0;
            // 
            // btnOrder
            // 
            this.btnOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOrder.ChkValue = false;
            this.btnOrder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOrder.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOrder.Enabled = false;
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
            this.btnOrder.Location = new System.Drawing.Point(452, 308);
            this.btnOrder.Margin = new System.Windows.Forms.Padding(0);
            this.btnOrder.Name = "btnOrder";
            this.btnOrder.Size = new System.Drawing.Size(110, 36);
            this.btnOrder.TabIndex = 4;
            this.btnOrder.Text = "경보 발령";
            this.btnOrder.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnOrder.UseChecked = true;
            this.btnOrder.Click += new System.EventHandler(this.btnOrder_Click);
            // 
            // WaitToOrderSWRForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 351);
            this.ControlBox = false;
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlBodyBackground);
            this.Controls.Add(this.btnOrder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WaitToOrderSWRForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "기상 특보 연계 발령 관리";
            this.Load += new System.EventHandler(this.LoadForm);
            this.pnlTop.ResumeLayout(false);
            this.pnlBodyBackground.ResumeLayout(false);
            this.pnlBodyBackground.PerformLayout();
            this.pnlListBack.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Adeng.Framework.Ctrl.AdengListView lvWaitToOrderSWRList;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblButtonDescription;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnClose;
        private System.Windows.Forms.Label lblListTitle;
        private System.Windows.Forms.Panel pnlBodyBackground;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnDelete;
        private System.Windows.Forms.Panel pnlListBack;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnOrder;
    }
}