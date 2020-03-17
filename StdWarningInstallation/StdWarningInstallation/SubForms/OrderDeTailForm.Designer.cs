namespace StdWarningInstallation
{
    partial class OrderDeTailForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrderDeTailForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnClose = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.pnlSelectedOrderBack = new System.Windows.Forms.Panel();
            this.lvSelectedOrder = new Adeng.Framework.Ctrl.AdengListView();
            this.lblSelectedOrderTitle = new System.Windows.Forms.Label();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblDescription = new System.Windows.Forms.Label();
            this.pnlBodyBackground = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lvOrderResponse = new Adeng.Framework.Ctrl.AdengListView();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlOrderDetailBack = new System.Windows.Forms.Panel();
            this.btnXml = new Adeng.Framework.Ctrl.AdengImgButtonEx();
            this.dgvOrderDetail = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlSelectedOrderBack.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlBodyBackground.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlOrderDetailBack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderDetail)).BeginInit();
            this.SuspendLayout();
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
            this.btnClose.ImgStretch = true;
            this.btnClose.IsImgStatusNormal = true;
            this.btnClose.Location = new System.Drawing.Point(937, 659);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 36);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "닫기";
            this.btnClose.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnClose.UseChecked = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlSelectedOrderBack
            // 
            this.pnlSelectedOrderBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.pnlSelectedOrderBack.Controls.Add(this.lvSelectedOrder);
            this.pnlSelectedOrderBack.Controls.Add(this.lblSelectedOrderTitle);
            this.pnlSelectedOrderBack.Location = new System.Drawing.Point(7, 7);
            this.pnlSelectedOrderBack.Name = "pnlSelectedOrderBack";
            this.pnlSelectedOrderBack.Size = new System.Drawing.Size(1014, 93);
            this.pnlSelectedOrderBack.TabIndex = 0;
            // 
            // lvSelectedOrder
            // 
            this.lvSelectedOrder.AntiAlias = false;
            this.lvSelectedOrder.AutoFit = false;
            this.lvSelectedOrder.BackColor = System.Drawing.Color.White;
            this.lvSelectedOrder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvSelectedOrder.ColumnHeight = 24;
            this.lvSelectedOrder.ColumnOffset = new System.Windows.Forms.Padding(2, 2, 0, 1);
            this.lvSelectedOrder.ColumnStyle = Adeng.Framework.Ctrl.AdengListViewColumnStyle.DEF_STYLE;
            this.lvSelectedOrder.FocusedItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(185)))), ((int)(((byte)(230)))));
            this.lvSelectedOrder.FrozenColumnIndex = -1;
            this.lvSelectedOrder.FullRowSelect = true;
            this.lvSelectedOrder.GridDashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.lvSelectedOrder.GridLineStyle = Adeng.Framework.Ctrl.AdengListViewGridLine.NONE;
            this.lvSelectedOrder.HideSelection = false;
            this.lvSelectedOrder.HoverSelection = false;
            this.lvSelectedOrder.IconOffset = new System.Drawing.Point(1, 0);
            this.lvSelectedOrder.InteraceColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.lvSelectedOrder.InteraceColor2 = System.Drawing.Color.White;
            this.lvSelectedOrder.ItemHeight = 18;
            this.lvSelectedOrder.ItemOffset = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lvSelectedOrder.Location = new System.Drawing.Point(9, 33);
            this.lvSelectedOrder.MultiSelect = false;
            this.lvSelectedOrder.Name = "lvSelectedOrder";
            this.lvSelectedOrder.ScrollType = Adeng.Framework.Ctrl.AdengListViewScrollType.BOTH;
            this.lvSelectedOrder.SelectioinItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(232)))), ((int)(((byte)(250)))));
            this.lvSelectedOrder.Size = new System.Drawing.Size(996, 51);
            this.lvSelectedOrder.TabIndex = 14;
            this.lvSelectedOrder.UseInteraceColor = true;
            this.lvSelectedOrder.UseSelFocusedBar = false;
            // 
            // lblSelectedOrderTitle
            // 
            this.lblSelectedOrderTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSelectedOrderTitle.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblSelectedOrderTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblSelectedOrderTitle.Location = new System.Drawing.Point(0, 0);
            this.lblSelectedOrderTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblSelectedOrderTitle.Name = "lblSelectedOrderTitle";
            this.lblSelectedOrderTitle.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblSelectedOrderTitle.Size = new System.Drawing.Size(1014, 32);
            this.lblSelectedOrderTitle.TabIndex = 0;
            this.lblSelectedOrderTitle.Text = "선택한 발령 요약 정보";
            this.lblSelectedOrderTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            this.pnlTop.Size = new System.Drawing.Size(1044, 40);
            this.pnlTop.TabIndex = 9;
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
            this.lblDescription.Size = new System.Drawing.Size(1044, 40);
            this.lblDescription.TabIndex = 0;
            this.lblDescription.Text = "선택한 발령의 상세 정보와 응답 결과를 확인합니다.";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlBodyBackground
            // 
            this.pnlBodyBackground.BackColor = System.Drawing.Color.White;
            this.pnlBodyBackground.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBodyBackground.Controls.Add(this.panel2);
            this.pnlBodyBackground.Controls.Add(this.pnlOrderDetailBack);
            this.pnlBodyBackground.Controls.Add(this.pnlSelectedOrderBack);
            this.pnlBodyBackground.Location = new System.Drawing.Point(7, 47);
            this.pnlBodyBackground.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBodyBackground.Name = "pnlBodyBackground";
            this.pnlBodyBackground.Size = new System.Drawing.Size(1030, 605);
            this.pnlBodyBackground.TabIndex = 11;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.panel2.Controls.Add(this.lvOrderResponse);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(410, 107);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(611, 489);
            this.panel2.TabIndex = 15;
            // 
            // lvOrderResponse
            // 
            this.lvOrderResponse.AntiAlias = false;
            this.lvOrderResponse.AutoFit = false;
            this.lvOrderResponse.BackColor = System.Drawing.Color.White;
            this.lvOrderResponse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvOrderResponse.ColumnHeight = 24;
            this.lvOrderResponse.ColumnOffset = new System.Windows.Forms.Padding(2, 2, 0, 1);
            this.lvOrderResponse.ColumnStyle = Adeng.Framework.Ctrl.AdengListViewColumnStyle.DEF_STYLE;
            this.lvOrderResponse.FocusedItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(185)))), ((int)(((byte)(230)))));
            this.lvOrderResponse.FrozenColumnIndex = -1;
            this.lvOrderResponse.FullRowSelect = true;
            this.lvOrderResponse.GridDashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.lvOrderResponse.GridLineStyle = Adeng.Framework.Ctrl.AdengListViewGridLine.NONE;
            this.lvOrderResponse.HideSelection = false;
            this.lvOrderResponse.HoverSelection = false;
            this.lvOrderResponse.IconOffset = new System.Drawing.Point(1, 0);
            this.lvOrderResponse.InteraceColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.lvOrderResponse.InteraceColor2 = System.Drawing.Color.White;
            this.lvOrderResponse.ItemHeight = 18;
            this.lvOrderResponse.ItemOffset = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.lvOrderResponse.Location = new System.Drawing.Point(9, 33);
            this.lvOrderResponse.MultiSelect = false;
            this.lvOrderResponse.Name = "lvOrderResponse";
            this.lvOrderResponse.ScrollType = Adeng.Framework.Ctrl.AdengListViewScrollType.BOTH;
            this.lvOrderResponse.SelectioinItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(232)))), ((int)(((byte)(250)))));
            this.lvOrderResponse.Size = new System.Drawing.Size(594, 447);
            this.lvOrderResponse.TabIndex = 15;
            this.lvOrderResponse.UseInteraceColor = true;
            this.lvOrderResponse.UseSelFocusedBar = false;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.label2.Size = new System.Drawing.Size(611, 32);
            this.label2.TabIndex = 0;
            this.label2.Text = "응답 결과";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlOrderDetailBack
            // 
            this.pnlOrderDetailBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(228)))), ((int)(((byte)(249)))));
            this.pnlOrderDetailBack.Controls.Add(this.btnXml);
            this.pnlOrderDetailBack.Controls.Add(this.dgvOrderDetail);
            this.pnlOrderDetailBack.Controls.Add(this.label1);
            this.pnlOrderDetailBack.Location = new System.Drawing.Point(7, 107);
            this.pnlOrderDetailBack.Name = "pnlOrderDetailBack";
            this.pnlOrderDetailBack.Size = new System.Drawing.Size(395, 489);
            this.pnlOrderDetailBack.TabIndex = 15;
            // 
            // btnXml
            // 
            this.btnXml.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXml.ChkValue = false;
            this.btnXml.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXml.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnXml.Font = new System.Drawing.Font("맑은 고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnXml.ForeColor = System.Drawing.Color.White;
            this.btnXml.Image = ((System.Drawing.Image)(resources.GetObject("btnXml.Image")));
            this.btnXml.ImgDisable = null;
            this.btnXml.ImgHover = null;
            this.btnXml.ImgSelect = ((System.Drawing.Image)(resources.GetObject("btnXml.ImgSelect")));
            this.btnXml.ImgStatusEvent = null;
            this.btnXml.ImgStatusNormal = null;
            this.btnXml.ImgStatusOffsetX = 2;
            this.btnXml.ImgStatusOffsetY = 0;
            this.btnXml.ImgStretch = true;
            this.btnXml.IsImgStatusNormal = true;
            this.btnXml.Location = new System.Drawing.Point(346, 5);
            this.btnXml.Margin = new System.Windows.Forms.Padding(0);
            this.btnXml.Name = "btnXml";
            this.btnXml.Size = new System.Drawing.Size(40, 24);
            this.btnXml.TabIndex = 12;
            this.btnXml.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnXml.UseChecked = false;
            this.btnXml.Visible = false;
            // 
            // dgvOrderDetail
            // 
            this.dgvOrderDetail.AllowUserToAddRows = false;
            this.dgvOrderDetail.AllowUserToDeleteRows = false;
            this.dgvOrderDetail.AllowUserToResizeColumns = false;
            this.dgvOrderDetail.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOrderDetail.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvOrderDetail.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            this.dgvOrderDetail.BackgroundColor = System.Drawing.Color.White;
            this.dgvOrderDetail.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvOrderDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvOrderDetail.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvOrderDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvOrderDetail.Location = new System.Drawing.Point(9, 33);
            this.dgvOrderDetail.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.dgvOrderDetail.MinimumSize = new System.Drawing.Size(367, 427);
            this.dgvOrderDetail.Name = "dgvOrderDetail";
            this.dgvOrderDetail.ReadOnly = true;
            this.dgvOrderDetail.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvOrderDetail.RowHeadersVisible = false;
            this.dgvOrderDetail.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvOrderDetail.RowTemplate.Height = 23;
            this.dgvOrderDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrderDetail.Size = new System.Drawing.Size(377, 447);
            this.dgvOrderDetail.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.label1.Size = new System.Drawing.Size(395, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "발령 상세 정보";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OrderDeTailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1044, 702);
            this.ControlBox = false;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlBodyBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderDeTailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "발령 이력 상세 정보";
            this.pnlSelectedOrderBack.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlBodyBackground.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pnlOrderDetailBack.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Adeng.Framework.Ctrl.AdengImgButtonEx btnClose;
        private System.Windows.Forms.Panel pnlSelectedOrderBack;
        private System.Windows.Forms.Label lblSelectedOrderTitle;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Panel pnlBodyBackground;
        private System.Windows.Forms.Panel pnlOrderDetailBack;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private Adeng.Framework.Ctrl.AdengListView lvSelectedOrder;
        private Adeng.Framework.Ctrl.AdengListView lvOrderResponse;
        private System.Windows.Forms.DataGridView dgvOrderDetail;
        private Adeng.Framework.Ctrl.AdengImgButtonEx btnXml;
    }
}