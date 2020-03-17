namespace StdWarningInstallation
{
    partial class ClearAlertWaitingListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClearAlertWaitingListForm));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblDescription = new System.Windows.Forms.Label();
            this.pnlBodyBackground = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDelete = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlClearWaitingList = new System.Windows.Forms.Panel();
            this.lvClearWaitingList = new Adeng.Framework.Ctrl.AdengListView();
            this.lblClearWaitingListTitle = new System.Windows.Forms.Label();
            this.btnClose = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.btnClearAlert = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlTop.SuspendLayout();
            this.pnlBodyBackground.SuspendLayout();
            this.pnlClearWaitingList.SuspendLayout();
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
            this.pnlTop.Size = new System.Drawing.Size(666, 40);
            this.pnlTop.TabIndex = 2;
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
            this.lblDescription.Text = "경보 발령 후 경보 상황을 해제하지 않은 발령 목록을 확인하고 해제합니다.";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBodyBackground
            // 
            this.pnlBodyBackground.BackColor = System.Drawing.Color.White;
            this.pnlBodyBackground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBodyBackground.Controls.Add(this.label1);
            this.pnlBodyBackground.Controls.Add(this.btnDelete);
            this.pnlBodyBackground.Controls.Add(this.pnlClearWaitingList);
            this.pnlBodyBackground.Location = new System.Drawing.Point(6, 48);
            this.pnlBodyBackground.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBodyBackground.Name = "pnlBodyBackground";
            this.pnlBodyBackground.Size = new System.Drawing.Size(654, 250);
            this.pnlBodyBackground.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(152, 226);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(309, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "경보 해제가 필요없는 항목을 관리 목록에서 삭제합니다.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.btnDelete.ImgStretch = false;
            this.btnDelete.IsImgStatusNormal = true;
            this.btnDelete.Location = new System.Drawing.Point(6, 206);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(140, 36);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "선택 항목 삭제";
            this.btnDelete.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnDelete.UseChecked = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // pnlClearWaitingList
            // 
            this.pnlClearWaitingList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.pnlClearWaitingList.Controls.Add(this.lvClearWaitingList);
            this.pnlClearWaitingList.Controls.Add(this.lblClearWaitingListTitle);
            this.pnlClearWaitingList.Location = new System.Drawing.Point(6, 6);
            this.pnlClearWaitingList.Name = "pnlClearWaitingList";
            this.pnlClearWaitingList.Size = new System.Drawing.Size(640, 194);
            this.pnlClearWaitingList.TabIndex = 0;
            // 
            // lvClearWaitingList
            // 
            this.lvClearWaitingList.AntiAlias = false;
            this.lvClearWaitingList.AutoFit = false;
            this.lvClearWaitingList.BackColor = System.Drawing.Color.White;
            this.lvClearWaitingList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvClearWaitingList.ColumnHeight = 24;
            this.lvClearWaitingList.ColumnOffset = new System.Windows.Forms.Padding(2, 2, 0, 1);
            this.lvClearWaitingList.ColumnStyle = Adeng.Framework.Ctrl.AdengListViewColumnStyle.DEF_STYLE;
            this.lvClearWaitingList.FocusedItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(185)))), ((int)(((byte)(230)))));
            this.lvClearWaitingList.FrozenColumnIndex = -1;
            this.lvClearWaitingList.FullRowSelect = true;
            this.lvClearWaitingList.GridDashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.lvClearWaitingList.GridLineStyle = Adeng.Framework.Ctrl.AdengListViewGridLine.NONE;
            this.lvClearWaitingList.HideSelection = false;
            this.lvClearWaitingList.HoverSelection = false;
            this.lvClearWaitingList.IconOffset = new System.Drawing.Point(1, 0);
            this.lvClearWaitingList.InteraceColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.lvClearWaitingList.InteraceColor2 = System.Drawing.Color.White;
            this.lvClearWaitingList.ItemHeight = 18;
            this.lvClearWaitingList.ItemOffset = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lvClearWaitingList.Location = new System.Drawing.Point(6, 35);
            this.lvClearWaitingList.MultiSelect = false;
            this.lvClearWaitingList.Name = "lvClearWaitingList";
            this.lvClearWaitingList.ScrollType = Adeng.Framework.Ctrl.AdengListViewScrollType.BOTH;
            this.lvClearWaitingList.SelectioinItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(232)))), ((int)(((byte)(250)))));
            this.lvClearWaitingList.Size = new System.Drawing.Size(628, 153);
            this.lvClearWaitingList.TabIndex = 14;
            this.lvClearWaitingList.UseInteraceColor = true;
            this.lvClearWaitingList.UseSelFocusedBar = false;
            this.lvClearWaitingList.ItemSelectionChanged += new Adeng.Framework.Ctrl.AdengListViewItemSelectionChangedEventHandler(this.lvClearWaitingList_ItemSelectionChanged);
            this.lvClearWaitingList.DoubleClick += new System.EventHandler(this.lvClearWaitingList_DoubleClick);
            // 
            // lblClearWaitingListTitle
            // 
            this.lblClearWaitingListTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblClearWaitingListTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblClearWaitingListTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblClearWaitingListTitle.Location = new System.Drawing.Point(0, 0);
            this.lblClearWaitingListTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblClearWaitingListTitle.Name = "lblClearWaitingListTitle";
            this.lblClearWaitingListTitle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblClearWaitingListTitle.Size = new System.Drawing.Size(640, 32);
            this.lblClearWaitingListTitle.TabIndex = 0;
            this.lblClearWaitingListTitle.Text = "경보 해제 대기 기발령 목록";
            this.lblClearWaitingListTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "닫기";
            this.btnClose.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnClose.UseChecked = false;
            this.btnClose.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnClearAlert
            // 
            this.btnClearAlert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearAlert.ChkValue = false;
            this.btnClearAlert.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClearAlert.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnClearAlert.Enabled = false;
            this.btnClearAlert.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnClearAlert.ForeColor = System.Drawing.Color.White;
            this.btnClearAlert.Image = ((System.Drawing.Image)(resources.GetObject("btnClearAlert.Image")));
            this.btnClearAlert.ImgDisable = null;
            this.btnClearAlert.ImgHover = null;
            this.btnClearAlert.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnClearAlert.ImgSelect")));
            this.btnClearAlert.ImgStatusEvent = null;
            this.btnClearAlert.ImgStatusNormal = null;
            this.btnClearAlert.ImgStatusOffsetX = 2;
            this.btnClearAlert.ImgStatusOffsetY = 0;
            this.btnClearAlert.ImgStretch = false;
            this.btnClearAlert.IsImgStatusNormal = true;
            this.btnClearAlert.Location = new System.Drawing.Point(452, 308);
            this.btnClearAlert.Margin = new System.Windows.Forms.Padding(0);
            this.btnClearAlert.Name = "btnClearAlert";
            this.btnClearAlert.Size = new System.Drawing.Size(110, 36);
            this.btnClearAlert.TabIndex = 2;
            this.btnClearAlert.Text = "해제 경보";
            this.btnClearAlert.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnClearAlert.UseChecked = true;
            this.btnClearAlert.Click += new System.EventHandler(this.btnClearAlert_Click);
            // 
            // ClearAlertWaitingListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 351);
            this.ControlBox = false;
            this.Controls.Add(this.pnlBodyBackground);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnClearAlert);
            this.Controls.Add(this.pnlTop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClearAlertWaitingListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "경보 상황 해제 관리";
            this.Load += new System.EventHandler(this.LoadForm);
            this.pnlTop.ResumeLayout(false);
            this.pnlBodyBackground.ResumeLayout(false);
            this.pnlBodyBackground.PerformLayout();
            this.pnlClearWaitingList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Panel pnlBodyBackground;
        private System.Windows.Forms.Panel pnlClearWaitingList;
        private System.Windows.Forms.Label lblClearWaitingListTitle;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnDelete;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnClose;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnClearAlert;
        private Adeng.Framework.Ctrl.AdengListView lvClearWaitingList;
        private System.Windows.Forms.Label label1;
    }
}