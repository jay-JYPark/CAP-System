using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Adeng.Framework.Ctrl;
using CAPLib;
using StdWarningInstallation.DataClass;

namespace StdWarningInstallation
{
    public partial class OrderModeForm : Form
    {
        public event EventHandler<OrderModeEventArgs> NotifyOrderModeChanged;

        // [2016-03-31] 기본 발령 정보를 시험으로 생성 - by Gonzi
        //public CAPLib.StatusType originalMode = CAPLib.StatusType.Actual;
        //public CAPLib.StatusType currentMode = CAPLib.StatusType.Actual;
        public CAPLib.StatusType originalMode = CAPLib.StatusType.Test;
        public CAPLib.StatusType currentMode = CAPLib.StatusType.Test;

        public OrderModeForm()
        {
            InitializeComponent();
        }
        public OrderModeForm(CAPLib.StatusType mode)
        {
            InitializeComponent();

            this.originalMode = mode;
        }

        private void LoadForm(object sender, EventArgs e)
        {
            if (originalMode == CAPLib.StatusType.Test)
            {
                this.btnOrderModeTest.ChkValue = true;
            }
            else if (originalMode == CAPLib.StatusType.Exercise)
            {
                this.btnOrderModeExcercise.ChkValue = true;
            }
            else
            {
                this.btnOrderModeActual.ChkValue = true;
            }
        }

        private void btnOrderMode_Click(object sender, EventArgs e)
        {
            AdengImgButtonEx button = sender as AdengImgButtonEx;

            this.btnOrderModeActual.ChkValue = false;
            this.btnOrderModeExcercise.ChkValue = false;
            this.btnOrderModeTest.ChkValue = false;

            if (button.Text == "실제")
            {
                this.currentMode = CAPLib.StatusType.Actual;
                this.btnOrderModeActual.ChkValue = true;
            }
            else if (button.Text == "훈련")
            {
                this.currentMode = CAPLib.StatusType.Exercise;
                this.btnOrderModeExcercise.ChkValue = true;
            }
            else
            {
                this.currentMode = CAPLib.StatusType.Test;
                this.btnOrderModeTest.ChkValue = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.originalMode != this.currentMode)
            {
                if (this.NotifyOrderModeChanged != null)
                {
                    this.NotifyOrderModeChanged(this, new OrderModeEventArgs(this.currentMode));
                }
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    /// <summary>
    /// 발령 준비 정보 전달 이벤트 아규먼트 클래스
    /// </summary>
    public class OrderModeEventArgs : EventArgs
    {
        private CAPLib.StatusType mode = CAPLib.StatusType.Actual;
        public CAPLib.StatusType Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        public OrderModeEventArgs(CAPLib.StatusType mode)
        {
            this.mode = mode;
        }
    }
}
