namespace StdWarningInstallation
{
    partial class InformationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InformationForm));
            this.MainLB = new Adeng.Framework.Ctrl.LabelEx();
            this.WarningLB = new Adeng.Framework.Ctrl.LabelEx();
            this.SuspendLayout();
            // 
            // MainLB
            // 
            this.MainLB.BackColor = System.Drawing.Color.Transparent;
            this.MainLB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.MainLB.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.MainLB.Location = new System.Drawing.Point(6, 111);
            this.MainLB.Name = "MainLB";
            this.MainLB.Size = new System.Drawing.Size(388, 79);
            this.MainLB.TabIndex = 0;
            this.MainLB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.MainLB.Click += new System.EventHandler(this.InformationForm_Click);
            // 
            // WarningLB
            // 
            this.WarningLB.BackColor = System.Drawing.Color.Transparent;
            this.WarningLB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.WarningLB.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.WarningLB.Location = new System.Drawing.Point(7, 232);
            this.WarningLB.Name = "WarningLB";
            this.WarningLB.Size = new System.Drawing.Size(386, 55);
            this.WarningLB.TabIndex = 1;
            this.WarningLB.Text = "경고 : 이 컴퓨터 프로그램은 저작권법과 국제 협약의 보호를 받습니다. 이 프로그램의 전부 또는 일부를 무단으로 복제, 배포하는 행위는 민사 및 " +
    "형사법에 의해 엄격히 규제되어 있으며, 기소 사유가 됩니다.";
            this.WarningLB.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.WarningLB.Click += new System.EventHandler(this.InformationForm_Click);
            // 
            // InformationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.ControlBox = false;
            this.Controls.Add(this.WarningLB);
            this.Controls.Add(this.MainLB);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InformationForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.Click += new System.EventHandler(this.InformationForm_Click);
            this.ResumeLayout(false);

        }

        #endregion

        private Adeng.Framework.Ctrl.LabelEx MainLB;
        private Adeng.Framework.Ctrl.LabelEx WarningLB;
    }
}