using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WmaPAlmScreen
{
    public partial class OrderStoredViewForm : Form
    {
        public event EventHandler StoredMsgFinishEvent;
        private Timer timer = null;
        private int playTime = 0;
        private double perTime = 0;
        private double percentNumber = 0;

        /// <summary>
        /// 생성자
        /// </summary>
        public OrderStoredViewForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 재생시간 프로퍼티
        /// </summary>
        public int PlayTime
        {
            set { this.playTime = value; }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (this.timer != null)
            {
                this.timer.Tick -= new EventHandler(timer_Tick);
                this.timer.Stop();
            }
        }

        /// <summary>
        /// 재생 시간을 셋팅하고, 프로그래스바를 시작한다.
        /// </summary>
        /// <param name="playTime"></param>
        public void StartSirenForm(int playTime)
        {
            this.playTime = playTime;
            this.timeProgressBar.Maximum = playTime;
            this.perTime = 100.0 / playTime;

            this.timer = new Timer();
            this.timer.Interval = 1000;
            this.timer.Tick += new EventHandler(timer_Tick);
            this.timer.Start();
        }

        /// <summary>
        /// Timer Tick 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            this.timeProgressBar.PerformStep();
            this.perLabel.Text = ((int)this.percentNumber).ToString() + " %";
            this.percentNumber += this.perTime;

            if (this.playTime == 0)
            {
                this.perLabel.Text = "100 %";
            }
            else if (this.playTime == -1)
            {
                if (this.StoredMsgFinishEvent != null)
                {
                    this.StoredMsgFinishEvent(this, new EventArgs());
                }

                this.Close();
            }

            this.playTime--;
        }
    }
}