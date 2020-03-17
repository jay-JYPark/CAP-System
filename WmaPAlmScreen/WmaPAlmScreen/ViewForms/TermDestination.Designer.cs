namespace WmaPAlmScreen
{
    partial class TermDestination
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
            this.topPanel = new NCASFND.NCasCtrl.NCasPanel();
            this.mainPictureBox = new System.Windows.Forms.PictureBox();
            this.topLabel = new System.Windows.Forms.Label();
            this.middlePanel = new NCASFND.NCasCtrl.NCasPanel();
            this.termListTreeView = new System.Windows.Forms.TreeView();
            this.addBtn = new NCASFND.NCasCtrl.NCasButton();
            this.selectCancelBtn = new NCASFND.NCasCtrl.NCasButton();
            this.allSelectBtn = new NCASFND.NCasCtrl.NCasButton();
            this.removeBtn = new NCASFND.NCasCtrl.NCasButton();
            this.selectTermListView = new NCASFND.NCasCtrl.NCasListView();
            this.rightLabel = new System.Windows.Forms.Label();
            this.leftLabel = new System.Windows.Forms.Label();
            this.cancelBtn = new NCASFND.NCasCtrl.NCasButton();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
            this.middlePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.BackgroundImage = global::WmaPAlmScreen.WmaPAlmScreenRsc.popBgTop;
            this.topPanel.Controls.Add(this.mainPictureBox);
            this.topPanel.Controls.Add(this.topLabel);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(607, 56);
            this.topPanel.TabIndex = 0;
            // 
            // mainPictureBox
            // 
            this.mainPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPictureBox.BackColor = System.Drawing.Color.Transparent;
            this.mainPictureBox.Image = global::WmaPAlmScreen.WmaPAlmScreenRsc.popTermSelect;
            this.mainPictureBox.Location = new System.Drawing.Point(565, 12);
            this.mainPictureBox.Name = "mainPictureBox";
            this.mainPictureBox.Size = new System.Drawing.Size(32, 32);
            this.mainPictureBox.TabIndex = 4;
            this.mainPictureBox.TabStop = false;
            // 
            // topLabel
            // 
            this.topLabel.AutoSize = true;
            this.topLabel.BackColor = System.Drawing.Color.Transparent;
            this.topLabel.Font = new System.Drawing.Font("나눔바른고딕 옛한글", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.topLabel.ForeColor = System.Drawing.Color.White;
            this.topLabel.Location = new System.Drawing.Point(13, 20);
            this.topLabel.Name = "topLabel";
            this.topLabel.Size = new System.Drawing.Size(156, 17);
            this.topLabel.TabIndex = 3;
            this.topLabel.Text = "개별 단말을 선택합니다";
            // 
            // middlePanel
            // 
            this.middlePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.middlePanel.Controls.Add(this.termListTreeView);
            this.middlePanel.Controls.Add(this.addBtn);
            this.middlePanel.Controls.Add(this.selectCancelBtn);
            this.middlePanel.Controls.Add(this.allSelectBtn);
            this.middlePanel.Controls.Add(this.removeBtn);
            this.middlePanel.Controls.Add(this.selectTermListView);
            this.middlePanel.Controls.Add(this.rightLabel);
            this.middlePanel.Controls.Add(this.leftLabel);
            this.middlePanel.Location = new System.Drawing.Point(20, 73);
            this.middlePanel.Name = "middlePanel";
            this.middlePanel.Size = new System.Drawing.Size(569, 478);
            this.middlePanel.TabIndex = 1;
            // 
            // termListTreeView
            // 
            this.termListTreeView.CheckBoxes = true;
            this.termListTreeView.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.termListTreeView.FullRowSelect = true;
            this.termListTreeView.HideSelection = false;
            this.termListTreeView.Location = new System.Drawing.Point(9, 35);
            this.termListTreeView.Name = "termListTreeView";
            this.termListTreeView.Size = new System.Drawing.Size(245, 391);
            this.termListTreeView.TabIndex = 8;
            this.termListTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.termListTreeView_AfterCheck);
            this.termListTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.termListTreeView_NodeMouseDoubleClick);
            // 
            // addBtn
            // 
            this.addBtn.AnimationInterval = 300;
            this.addBtn.CheckedValue = false;
            this.addBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.addBtn.Font = new System.Drawing.Font("나눔바른고딕", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.addBtn.ForeColor = System.Drawing.Color.White;
            this.addBtn.Image = global::WmaPAlmScreen.WmaPAlmScreenRsc.btnArrRightNormal;
            this.addBtn.ImgDisable = null;
            this.addBtn.ImgHover = null;
            this.addBtn.ImgSelect = global::WmaPAlmScreen.WmaPAlmScreenRsc.btnArrRightPress;
            this.addBtn.ImgStatusEvent = null;
            this.addBtn.ImgStatusNormal = null;
            this.addBtn.ImgStatusOffsetX = 2;
            this.addBtn.ImgStatusOffsetY = 0;
            this.addBtn.IsStatusNormal = true;
            this.addBtn.Location = new System.Drawing.Point(262, 168);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(34, 55);
            this.addBtn.TabIndex = 6;
            this.addBtn.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.addBtn.UseAutoWrap = true;
            this.addBtn.UseCheck = false;
            this.addBtn.UseImgStretch = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // selectCancelBtn
            // 
            this.selectCancelBtn.AnimationInterval = 300;
            this.selectCancelBtn.CheckedValue = false;
            this.selectCancelBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.selectCancelBtn.Font = new System.Drawing.Font("나눔바른고딕", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.selectCancelBtn.ForeColor = System.Drawing.Color.White;
            this.selectCancelBtn.Image = global::WmaPAlmScreen.WmaPAlmScreenRsc.btnSmallNormal;
            this.selectCancelBtn.ImgDisable = null;
            this.selectCancelBtn.ImgHover = null;
            this.selectCancelBtn.ImgSelect = global::WmaPAlmScreen.WmaPAlmScreenRsc.btnSmallPress;
            this.selectCancelBtn.ImgStatusEvent = null;
            this.selectCancelBtn.ImgStatusNormal = null;
            this.selectCancelBtn.ImgStatusOffsetX = 2;
            this.selectCancelBtn.ImgStatusOffsetY = 0;
            this.selectCancelBtn.IsStatusNormal = true;
            this.selectCancelBtn.Location = new System.Drawing.Point(469, 433);
            this.selectCancelBtn.Name = "selectCancelBtn";
            this.selectCancelBtn.Size = new System.Drawing.Size(89, 39);
            this.selectCancelBtn.TabIndex = 4;
            this.selectCancelBtn.Text = "선택취소";
            this.selectCancelBtn.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.selectCancelBtn.UseAutoWrap = true;
            this.selectCancelBtn.UseCheck = false;
            this.selectCancelBtn.UseImgStretch = true;
            this.selectCancelBtn.Click += new System.EventHandler(this.selectCancelBtn_Click);
            // 
            // allSelectBtn
            // 
            this.allSelectBtn.AnimationInterval = 300;
            this.allSelectBtn.CheckedValue = false;
            this.allSelectBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.allSelectBtn.Font = new System.Drawing.Font("나눔바른고딕", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.allSelectBtn.ForeColor = System.Drawing.Color.White;
            this.allSelectBtn.Image = global::WmaPAlmScreen.WmaPAlmScreenRsc.btnSmallNormal;
            this.allSelectBtn.ImgDisable = null;
            this.allSelectBtn.ImgHover = null;
            this.allSelectBtn.ImgSelect = global::WmaPAlmScreen.WmaPAlmScreenRsc.btnSmallPress;
            this.allSelectBtn.ImgStatusEvent = null;
            this.allSelectBtn.ImgStatusNormal = null;
            this.allSelectBtn.ImgStatusOffsetX = 2;
            this.allSelectBtn.ImgStatusOffsetY = 0;
            this.allSelectBtn.IsStatusNormal = true;
            this.allSelectBtn.Location = new System.Drawing.Point(374, 433);
            this.allSelectBtn.Name = "allSelectBtn";
            this.allSelectBtn.Size = new System.Drawing.Size(89, 39);
            this.allSelectBtn.TabIndex = 3;
            this.allSelectBtn.Text = "전체선택";
            this.allSelectBtn.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.allSelectBtn.UseAutoWrap = true;
            this.allSelectBtn.UseCheck = false;
            this.allSelectBtn.UseImgStretch = true;
            this.allSelectBtn.Click += new System.EventHandler(this.allSelectBtn_Click);
            // 
            // removeBtn
            // 
            this.removeBtn.AnimationInterval = 300;
            this.removeBtn.CheckedValue = false;
            this.removeBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.removeBtn.Font = new System.Drawing.Font("나눔바른고딕", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.removeBtn.ForeColor = System.Drawing.Color.White;
            this.removeBtn.Image = global::WmaPAlmScreen.WmaPAlmScreenRsc.btnArrLeftNormal;
            this.removeBtn.ImgDisable = null;
            this.removeBtn.ImgHover = null;
            this.removeBtn.ImgSelect = global::WmaPAlmScreen.WmaPAlmScreenRsc.btnArrLeftPress;
            this.removeBtn.ImgStatusEvent = null;
            this.removeBtn.ImgStatusNormal = null;
            this.removeBtn.ImgStatusOffsetX = 2;
            this.removeBtn.ImgStatusOffsetY = 0;
            this.removeBtn.IsStatusNormal = true;
            this.removeBtn.Location = new System.Drawing.Point(262, 240);
            this.removeBtn.Name = "removeBtn";
            this.removeBtn.Size = new System.Drawing.Size(34, 55);
            this.removeBtn.TabIndex = 7;
            this.removeBtn.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.removeBtn.UseAutoWrap = true;
            this.removeBtn.UseCheck = false;
            this.removeBtn.UseImgStretch = true;
            this.removeBtn.Click += new System.EventHandler(this.removeBtn_Click);
            // 
            // selectTermListView
            // 
            this.selectTermListView.AntiAlias = false;
            this.selectTermListView.AutoFit = false;
            this.selectTermListView.BackColor = System.Drawing.Color.White;
            this.selectTermListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.selectTermListView.ColumnHeight = 24;
            this.selectTermListView.ColumnOffset = new System.Windows.Forms.Padding(2, 2, 0, 1);
            this.selectTermListView.ColumnStyle = NCASFND.NCasListViewColumnStyle.DefaultStyle;
            this.selectTermListView.FocusedItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(82)))), ((int)(((byte)(185)))), ((int)(((byte)(230)))));
            this.selectTermListView.Font = new System.Drawing.Font("나눔바른고딕", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.selectTermListView.FrozenColumnIndex = -1;
            this.selectTermListView.FullRowSelect = true;
            this.selectTermListView.GridDashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            this.selectTermListView.GridLineStyle = NCASFND.NCasListViewGridLine.GridNone;
            this.selectTermListView.HideColumnCheckBox = false;
            this.selectTermListView.HideSelection = false;
            this.selectTermListView.HoverSelection = false;
            this.selectTermListView.IconOffset = new System.Drawing.Point(1, 0);
            this.selectTermListView.InteraceColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.selectTermListView.InteraceColor2 = System.Drawing.Color.White;
            this.selectTermListView.ItemHeight = 18;
            this.selectTermListView.ItemOffset = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.selectTermListView.Location = new System.Drawing.Point(303, 35);
            this.selectTermListView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.selectTermListView.MultiSelect = false;
            this.selectTermListView.Name = "selectTermListView";
            this.selectTermListView.ScrollType = NCASFND.NCasListViewScrollType.ScrollBoth;
            this.selectTermListView.SelectioinItemBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(232)))), ((int)(((byte)(250)))));
            this.selectTermListView.Size = new System.Drawing.Size(255, 391);
            this.selectTermListView.TabIndex = 5;
            this.selectTermListView.UseFocusBar = true;
            this.selectTermListView.UseInteraceColor = true;
            this.selectTermListView.UseItemFitBox = false;
            this.selectTermListView.UseSelFocusedBar = false;
            this.selectTermListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.selectTermListView_MouseDoubleClick);
            // 
            // rightLabel
            // 
            this.rightLabel.AutoSize = true;
            this.rightLabel.BackColor = System.Drawing.Color.Transparent;
            this.rightLabel.Font = new System.Drawing.Font("나눔바른고딕 옛한글", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.rightLabel.ForeColor = System.Drawing.Color.White;
            this.rightLabel.Location = new System.Drawing.Point(302, 13);
            this.rightLabel.Name = "rightLabel";
            this.rightLabel.Size = new System.Drawing.Size(82, 17);
            this.rightLabel.TabIndex = 3;
            this.rightLabel.Text = "선택한 단말";
            // 
            // leftLabel
            // 
            this.leftLabel.AutoSize = true;
            this.leftLabel.BackColor = System.Drawing.Color.Transparent;
            this.leftLabel.Font = new System.Drawing.Font("나눔바른고딕 옛한글", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.leftLabel.ForeColor = System.Drawing.Color.White;
            this.leftLabel.Location = new System.Drawing.Point(9, 13);
            this.leftLabel.Name = "leftLabel";
            this.leftLabel.Size = new System.Drawing.Size(68, 17);
            this.leftLabel.TabIndex = 2;
            this.leftLabel.Text = "단말 목록";
            // 
            // cancelBtn
            // 
            this.cancelBtn.AnimationInterval = 300;
            this.cancelBtn.CheckedValue = false;
            this.cancelBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancelBtn.Font = new System.Drawing.Font("나눔바른고딕", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.cancelBtn.ForeColor = System.Drawing.Color.White;
            this.cancelBtn.Image = global::WmaPAlmScreen.WmaPAlmScreenRsc.btnSaveNormal;
            this.cancelBtn.ImgDisable = null;
            this.cancelBtn.ImgHover = null;
            this.cancelBtn.ImgSelect = global::WmaPAlmScreen.WmaPAlmScreenRsc.btnSavePress;
            this.cancelBtn.ImgStatusEvent = null;
            this.cancelBtn.ImgStatusNormal = null;
            this.cancelBtn.ImgStatusOffsetX = 2;
            this.cancelBtn.ImgStatusOffsetY = 0;
            this.cancelBtn.IsStatusNormal = true;
            this.cancelBtn.Location = new System.Drawing.Point(474, 557);
            this.cancelBtn.Name = "cancelBtn";
            this.cancelBtn.Size = new System.Drawing.Size(115, 43);
            this.cancelBtn.TabIndex = 5;
            this.cancelBtn.Text = "닫기";
            this.cancelBtn.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.cancelBtn.UseAutoWrap = true;
            this.cancelBtn.UseCheck = false;
            this.cancelBtn.UseImgStretch = true;
            this.cancelBtn.Click += new System.EventHandler(this.cancelBtn_Click);
            // 
            // TermDestination
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(36)))), ((int)(((byte)(36)))));
            this.ClientSize = new System.Drawing.Size(607, 605);
            this.ControlBox = false;
            this.Controls.Add(this.cancelBtn);
            this.Controls.Add(this.middlePanel);
            this.Controls.Add(this.topPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TermDestination";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "개별 단말 선택";
            this.TopMost = true;
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
            this.middlePanel.ResumeLayout(false);
            this.middlePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private NCASFND.NCasCtrl.NCasPanel topPanel;
        private System.Windows.Forms.Label topLabel;
        private System.Windows.Forms.PictureBox mainPictureBox;
        private NCASFND.NCasCtrl.NCasPanel middlePanel;
        private System.Windows.Forms.TreeView termListTreeView;
        private NCASFND.NCasCtrl.NCasButton addBtn;
        private NCASFND.NCasCtrl.NCasButton selectCancelBtn;
        private NCASFND.NCasCtrl.NCasButton allSelectBtn;
        private NCASFND.NCasCtrl.NCasButton removeBtn;
        private NCASFND.NCasCtrl.NCasListView selectTermListView;
        private System.Windows.Forms.Label rightLabel;
        private System.Windows.Forms.Label leftLabel;
        private NCASFND.NCasCtrl.NCasButton cancelBtn;
    }
}