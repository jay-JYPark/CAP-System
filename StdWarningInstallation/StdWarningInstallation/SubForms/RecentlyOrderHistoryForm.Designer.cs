namespace StdWarningInstallation
{
    partial class RecentlyOrderHistoryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecentlyOrderHistoryForm));
            this.btnClose = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTopDescription = new System.Windows.Forms.Label();
            this.lvOrderList = new Adeng.Framework.Ctrl.AdengListView();
            this.btnOrderCancel = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.ChkValue = false;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
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
            this.btnClose.Location = new System.Drawing.Point(660, 642);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 36);
            this.btnClose.TabIndex = 12;
            this.btnClose.Text = "닫기";
            this.btnClose.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnClose.UseChecked = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlTop.BackgroundImage")));
            this.pnlTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlTop.Controls.Add(this.lblTopDescription);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(758, 40);
            this.pnlTop.TabIndex = 11;
            // 
            // lblTopDescription
            // 
            this.lblTopDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblTopDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTopDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblTopDescription.Image = ((System.Drawing.Image)(resources.GetObject("lblTopDescription.Image")));
            this.lblTopDescription.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTopDescription.Location = new System.Drawing.Point(0, 0);
            this.lblTopDescription.Margin = new System.Windows.Forms.Padding(0);
            this.lblTopDescription.Name = "lblTopDescription";
            this.lblTopDescription.Padding = new System.Windows.Forms.Padding(14, 0, 7, 0);
            this.lblTopDescription.Size = new System.Drawing.Size(758, 40);
            this.lblTopDescription.TabIndex = 0;
            this.lblTopDescription.Text = "최근 발령 이력 30건 까지 표시되며 이력을 선택하여 발령 정보를 확인할 수 있습니다.";
            this.lblTopDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvOrderList
            // 
            this.lvOrderList.AntiAlias = false;
            this.lvOrderList.AutoFit = false;
            this.lvOrderList.BackColor = System.Drawing.Color.White;
            this.lvOrderList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvOrderList.ColumnHeight = 24;
            this.lvOrderList.ColumnOffset = new System.Windows.Forms.Padding(2, 2, 0, 1);
            this.lvOrderList.ColumnStyle = Adeng.Framework.Ctrl.AdengListViewColumnStyle.DEF_STYLE;
            this.lvOrderList.FocusedItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(185)))), ((int)(((byte)(230)))));
            this.lvOrderList.FrozenColumnIndex = -1;
            this.lvOrderList.FullRowSelect = true;
            this.lvOrderList.GridDashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.lvOrderList.GridLineStyle = Adeng.Framework.Ctrl.AdengListViewGridLine.NONE;
            this.lvOrderList.HideSelection = false;
            this.lvOrderList.HoverSelection = false;
            this.lvOrderList.IconOffset = new System.Drawing.Point(1, 0);
            this.lvOrderList.InteraceColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.lvOrderList.InteraceColor2 = System.Drawing.Color.White;
            this.lvOrderList.ItemHeight = 18;
            this.lvOrderList.ItemOffset = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lvOrderList.Location = new System.Drawing.Point(7, 48);
            this.lvOrderList.MultiSelect = false;
            this.lvOrderList.Name = "lvOrderList";
            this.lvOrderList.ScrollType = Adeng.Framework.Ctrl.AdengListViewScrollType.BOTH;
            this.lvOrderList.SelectioinItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(232)))), ((int)(((byte)(250)))));
            this.lvOrderList.Size = new System.Drawing.Size(744, 583);
            this.lvOrderList.TabIndex = 13;
            this.lvOrderList.UseInteraceColor = true;
            this.lvOrderList.UseSelFocusedBar = false;
            this.lvOrderList.ItemSelectionChanged += new Adeng.Framework.Ctrl.AdengListViewItemSelectionChangedEventHandler(this.lvOrderList_ItemSelectedChanged);
            this.lvOrderList.DoubleClick += new System.EventHandler(this.lvOrderList_DoubleClick);
            // 
            // btnOrderCancel
            // 
            this.btnOrderCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOrderCancel.ChkValue = false;
            this.btnOrderCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOrderCancel.Enabled = false;
            this.btnOrderCancel.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOrderCancel.ForeColor = System.Drawing.Color.White;
            this.btnOrderCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnOrderCancel.Image")));
            this.btnOrderCancel.ImgDisable = null;
            this.btnOrderCancel.ImgHover = null;
            this.btnOrderCancel.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnOrderCancel.ImgSelect")));
            this.btnOrderCancel.ImgStatusEvent = null;
            this.btnOrderCancel.ImgStatusNormal = null;
            this.btnOrderCancel.ImgStatusOffsetX = 2;
            this.btnOrderCancel.ImgStatusOffsetY = 0;
            this.btnOrderCancel.ImgStretch = false;
            this.btnOrderCancel.IsImgStatusNormal = true;
            this.btnOrderCancel.Location = new System.Drawing.Point(563, 642);
            this.btnOrderCancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnOrderCancel.Name = "btnOrderCancel";
            this.btnOrderCancel.Size = new System.Drawing.Size(90, 36);
            this.btnOrderCancel.TabIndex = 14;
            this.btnOrderCancel.Text = "발령 취소";
            this.btnOrderCancel.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnOrderCancel.UseChecked = true;
            this.btnOrderCancel.Click += new System.EventHandler(this.btnOrderCancel_Click);
            // 
            // RecentlyOrderHistoryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 686);
            this.ControlBox = false;
            this.Controls.Add(this.btnOrderCancel);
            this.Controls.Add(this.lvOrderList);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RecentlyOrderHistoryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "최근 발령 이력";
            this.Load += new System.EventHandler(this.LoadForm);
            this.pnlTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        private Adeng.Framework.Ctrl.AdengImgButtonEx btnClose;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTopDescription;
        private Adeng.Framework.Ctrl.AdengListView lvOrderList;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnOrderCancel;
    }
}