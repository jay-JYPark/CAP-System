namespace WmaPAlmScreen
{
    partial class OrderStoredViewForm
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
            this.perLabel = new System.Windows.Forms.Label();
            this.timeProgressBar = new System.Windows.Forms.ProgressBar();
            this.sirenKindLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // perLabel
            // 
            this.perLabel.BackColor = System.Drawing.Color.Transparent;
            this.perLabel.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.perLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
            this.perLabel.Location = new System.Drawing.Point(140, 47);
            this.perLabel.Name = "perLabel";
            this.perLabel.Size = new System.Drawing.Size(80, 25);
            this.perLabel.TabIndex = 8;
            this.perLabel.Text = "0 %";
            this.perLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timeProgressBar
            // 
            this.timeProgressBar.BackColor = System.Drawing.Color.White;
            this.timeProgressBar.Location = new System.Drawing.Point(38, 78);
            this.timeProgressBar.Name = "timeProgressBar";
            this.timeProgressBar.Size = new System.Drawing.Size(284, 24);
            this.timeProgressBar.Step = 1;
            this.timeProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.timeProgressBar.TabIndex = 7;
            // 
            // sirenKindLabel
            // 
            this.sirenKindLabel.BackColor = System.Drawing.Color.Transparent;
            this.sirenKindLabel.Font = new System.Drawing.Font("맑은 고딕", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.sirenKindLabel.ForeColor = System.Drawing.Color.White;
            this.sirenKindLabel.Location = new System.Drawing.Point(40, 8);
            this.sirenKindLabel.Name = "sirenKindLabel";
            this.sirenKindLabel.Size = new System.Drawing.Size(281, 36);
            this.sirenKindLabel.TabIndex = 6;
            this.sirenKindLabel.Text = "저장메시지 방송 중";
            this.sirenKindLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OrderStoredViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::WmaPAlmScreen.WmaPAlmScreenRsc.bgLoading;
            this.ClientSize = new System.Drawing.Size(360, 154);
            this.ControlBox = false;
            this.Controls.Add(this.perLabel);
            this.Controls.Add(this.timeProgressBar);
            this.Controls.Add(this.sirenKindLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderStoredViewForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OrderStoredViewForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label perLabel;
        private System.Windows.Forms.ProgressBar timeProgressBar;
        private System.Windows.Forms.Label sirenKindLabel;
    }
}