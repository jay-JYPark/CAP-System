namespace WmaPAlmScreen
{
    partial class DistIconUserControl
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.distIconBtn = new NCASFND.NCasCtrl.NCasButton();
            this.distName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // distIconBtn
            // 
            this.distIconBtn.AnimationInterval = 300;
            this.distIconBtn.CheckedValue = false;
            this.distIconBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.distIconBtn.Dock = System.Windows.Forms.DockStyle.Top;
            this.distIconBtn.Image = global::WmaPAlmScreen.WmaPAlmScreenRsc.iconMap;
            this.distIconBtn.ImgDisable = null;
            this.distIconBtn.ImgHover = null;
            this.distIconBtn.ImgSelect = global::WmaPAlmScreen.WmaPAlmScreenRsc.iconMapSelected;
            this.distIconBtn.ImgStatusEvent = null;
            this.distIconBtn.ImgStatusNormal = null;
            this.distIconBtn.ImgStatusOffsetX = 2;
            this.distIconBtn.ImgStatusOffsetY = 0;
            this.distIconBtn.IsStatusNormal = true;
            this.distIconBtn.Location = new System.Drawing.Point(0, 0);
            this.distIconBtn.Name = "distIconBtn";
            this.distIconBtn.Size = new System.Drawing.Size(54, 47);
            this.distIconBtn.TabIndex = 0;
            this.distIconBtn.TxtAlignment = System.Drawing.StringAlignment.Near;
            this.distIconBtn.UseAutoWrap = true;
            this.distIconBtn.UseCheck = true;
            this.distIconBtn.UseImgStretch = true;
            // 
            // distName
            // 
            this.distName.BackColor = System.Drawing.Color.Transparent;
            this.distName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.distName.Font = new System.Drawing.Font("나눔고딕 ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.distName.ForeColor = System.Drawing.Color.White;
            this.distName.Location = new System.Drawing.Point(0, 47);
            this.distName.Name = "distName";
            this.distName.Size = new System.Drawing.Size(54, 20);
            this.distName.TabIndex = 1;
            this.distName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // DistIconUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.distName);
            this.Controls.Add(this.distIconBtn);
            this.Name = "DistIconUserControl";
            this.Size = new System.Drawing.Size(54, 67);
            this.ResumeLayout(false);

        }

        #endregion

        private NCASFND.NCasCtrl.NCasButton distIconBtn;
        private System.Windows.Forms.Label distName;
    }
}
