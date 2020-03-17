using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WmaPAlmScreen
{
    public partial class ViewBase : UserControl
    {
        protected int interval = 1000;
        protected MainForm main = null;
        protected MainForm.ViewKind viewKind = MainForm.ViewKind.None;

        /// <summary>
        /// 지연시간 멤버 프로퍼티
        /// </summary>
        public int Interval
        {
            get { return this.interval; }
            set { this.interval = value; }
        }

        /// <summary>
        /// 화면 종류 프로퍼티
        /// </summary>
        public MainForm.ViewKind ViewKind
        {
            get { return this.viewKind; }
            set { this.viewKind = value; }
        }
        
        /// <summary>
        /// 생성자
        /// </summary>
        public ViewBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="main">MainForm에서 넘겨주는 참조</param>
        public ViewBase(MainForm main)
            : this()
        {
            this.main = main;
        }

        /// <summary>
        /// View를 생성하는 Factory method
        /// </summary>
        /// <param name="viewKind">ViewKind 종류</param>
        /// <param name="main">MainForm</param>
        /// <returns></returns>
        public static ViewBase CreateView(MainForm.ViewKind viewKind, MainForm main)
        {
            switch (viewKind)
            {
                case MainForm.ViewKind.None:
                    CoverView cover = new CoverView(main);
                    return cover;

                case MainForm.ViewKind.OrderView19201080:
                    OrderView19201080 order = new OrderView19201080(main);
                    return order;

                case MainForm.ViewKind.ResultView:
                    OrderResultView result = new OrderResultView(main);
                    return result;

                case MainForm.ViewKind.DevMonView:
                    DeviceMonitorView deviceMonitor = new DeviceMonitorView(main);
                    return deviceMonitor;

                default:
                    return new ViewBase(main);
            }
        }

        /// <summary>
        /// Timer 처리 메소드
        /// </summary>
        virtual public void OnTimer()
        {
        }
    }
}