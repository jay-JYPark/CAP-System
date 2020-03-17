namespace StdWarningInstallation.Ctrl
{
    partial class SWRConditionPart
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.chkboxWatch = new System.Windows.Forms.CheckBox();
            this.chkboxWarning = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblTitle.Font = new System.Drawing.Font("맑은 고딕", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(7)))), ((int)(((byte)(51)))), ((int)(((byte)(115)))));
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(60, 27);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "특보";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkboxWatch
            // 
            this.chkboxWatch.AutoSize = true;
            this.chkboxWatch.BackColor = System.Drawing.Color.Transparent;
            this.chkboxWatch.Location = new System.Drawing.Point(85, 4);
            this.chkboxWatch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkboxWatch.Name = "chkboxWatch";
            this.chkboxWatch.Size = new System.Drawing.Size(62, 19);
            this.chkboxWatch.TabIndex = 1;
            this.chkboxWatch.Text = "주의보";
            this.chkboxWatch.UseVisualStyleBackColor = false;
            // 
            // chkboxWarning
            // 
            this.chkboxWarning.AutoSize = true;
            this.chkboxWarning.BackColor = System.Drawing.Color.Transparent;
            this.chkboxWarning.Location = new System.Drawing.Point(160, 4);
            this.chkboxWarning.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkboxWarning.Name = "chkboxWarning";
            this.chkboxWarning.Size = new System.Drawing.Size(50, 19);
            this.chkboxWarning.TabIndex = 1;
            this.chkboxWarning.Text = "경보";
            this.chkboxWarning.UseVisualStyleBackColor = false;
            // 
            // SWRConditionPart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::StdWarningInstallation.Properties.Resources.bgWeatherTableLabelOne;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.Controls.Add(this.chkboxWarning);
            this.Controls.Add(this.chkboxWatch);
            this.Controls.Add(this.lblTitle);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SWRConditionPart";
            this.Size = new System.Drawing.Size(239, 27);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.CheckBox chkboxWatch;
        private System.Windows.Forms.CheckBox chkboxWarning;
    }
}
