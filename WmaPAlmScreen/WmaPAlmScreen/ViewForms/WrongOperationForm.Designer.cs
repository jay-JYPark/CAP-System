namespace WmaPAlmScreen
{
    partial class WrongOperationForm
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
            this.textLabel = new System.Windows.Forms.Label();
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
            this.btnOk.Location = new System.Drawing.Point(156, 101);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(89, 39);
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "확인";
            this.btnOk.TxtAlignment = System.Drawing.StringAlignment.Center;
            this.btnOk.UseAutoWrap = false;
            this.btnOk.UseCheck = false;
            this.btnOk.UseImgStretch = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // textLabel
            // 
            this.textLabel.BackColor = System.Drawing.Color.Transparent;
            this.textLabel.Font = new System.Drawing.Font("맑은 고딕", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.textLabel.ForeColor = System.Drawing.Color.White;
            this.textLabel.Location = new System.Drawing.Point(97, 44);
            this.textLabel.Name = "textLabel";
            this.textLabel.Size = new System.Drawing.Size(290, 23);
            this.textLabel.TabIndex = 12;
            this.textLabel.Text = "발령대상 선택 후 진행하세요";
            this.textLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // WrongOperationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WmaPAlmScreen.WmaPAlmScreenRsc.bgErrorPop;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(400, 157);
            this.ControlBox = false;
            this.Controls.Add(this.textLabel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WrongOperationForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private NCASFND.NCasCtrl.NCasButton btnOk;
        private System.Windows.Forms.Label textLabel;

    }
}