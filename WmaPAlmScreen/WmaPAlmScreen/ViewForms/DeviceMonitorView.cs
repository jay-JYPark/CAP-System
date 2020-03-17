using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NCASFND;
using NCASFND.NCasCtrl;
using NCASBIZ.NCasData;
using NCASBIZ.NCasUtility;
using NCASBIZ.NCasDefine;

namespace WmaPAlmScreen
{
    public partial class DeviceMonitorView : ViewBase
    {
        private ProvInfo provInfo = null;

        /// <summary>
        /// 생성자
        /// </summary>
        public DeviceMonitorView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="main">MainForm에서 넘겨주는 참조</param>
        public DeviceMonitorView(MainForm main)
            : this()
        {
            this.main = main;
            this.provInfo = main.ProvInfo;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.InitListView();
            this.SetServerListView();
            this.SetTermListView();
            this.main.AddTimerMember(this);
        }

        public override void OnTimer()
        {
            base.OnTimer();
            this.setServerStatusUpdate();
            this.setTermStatusUpdate();
        }

        #region ListView 초기화
        /// <summary>
        /// 장비감시 ListView 초기화
        /// </summary>
        public void InitListView()
        {
            this.serverStatusListView.GridLineStyle = NCasListViewGridLine.GridBoth;
            this.serverStatusListView.GridDashStyle = DashStyle.Dot;
            this.serverStatusListView.ScrollType = NCasListViewScrollType.ScrollBoth;
            this.serverStatusListView.Font = new Font(WmaPAlmScreenRsc.FontName, 11.0f);
            this.serverStatusListView.ColumnHeight = 35;
            this.serverStatusListView.ItemHeight = 33;

            NCasColumnHeader col = new NCasColumnHeader();
            col.Text = "...";
            col.Width = 35;
            col.SortType = NCasListViewSortType.SortIcon;
            col.TextAlign = HorizontalAlignment.Center;
            col.ColumnLock = true;
            this.serverStatusListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "장비명";
            col.Width = 300;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Left;
            this.serverStatusListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "IP";
            col.Width = 180;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Center;
            this.serverStatusListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "상태";
            col.Width = 120;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Center;
            this.serverStatusListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "발생시각";
            col.Width = 280;
            col.SortType = NCasListViewSortType.SortDateTime;
            col.TextAlign = HorizontalAlignment.Center;
            this.serverStatusListView.Columns.Add(col);

            this.termStatusListView.GridLineStyle = NCasListViewGridLine.GridBoth;
            this.termStatusListView.GridDashStyle = DashStyle.Dot;
            this.termStatusListView.ScrollType = NCasListViewScrollType.ScrollBoth;
            this.termStatusListView.Font = new Font(WmaPAlmScreenRsc.FontName, 11.0f);
            this.termStatusListView.ColumnHeight = 35;
            this.termStatusListView.ItemHeight = 33;

            col = new NCasColumnHeader();
            col.Text = "...";
            col.Width = 35;
            col.SortType = NCasListViewSortType.SortIcon;
            col.TextAlign = HorizontalAlignment.Center;
            col.ColumnLock = true;
            this.termStatusListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "장비명";
            col.Width = 300;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Left;
            this.termStatusListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "IP";
            col.Width = 180;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Center;
            this.termStatusListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "상태";
            col.Width = 120;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Center;
            this.termStatusListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "발생시각";
            col.Width = 280;
            col.SortType = NCasListViewSortType.SortDateTime;
            col.TextAlign = HorizontalAlignment.Center;
            this.termStatusListView.Columns.Add(col);
        }
        #endregion

        #region ListView 셋팅
        /// <summary>
        /// 통제소 장비 ListView 셋팅
        /// </summary>
        private void SetServerListView()
        {
            NCasListViewItem lvi = null;

            //시도
            foreach (PDevInfo eachPDevInfo in this.provInfo.LstDevs)
            {
                if (eachPDevInfo.UseFlag != NCasDefineUseStatus.Use)
                    continue;

                if (eachPDevInfo.IpAddrToString == NCasUtilityMng.INCasEtcUtility.GetIPv4())
                    continue;

                if (eachPDevInfo.DevId != NCasDefineDeviceKind.MutilMainSys)
                    continue;

                lvi = new NCasListViewItem();
                lvi.Name = eachPDevInfo.IpAddrToString;
                lvi.ImageIndex = 0;

                NCasListViewItem.NCasListViewSubItem sub = new NCasListViewItem.NCasListViewSubItem();
                sub.Text = eachPDevInfo.Name;
                sub.TextAlign = HorizontalAlignment.Left;
                lvi.SubItems.Add(sub);

                sub = new NCasListViewItem.NCasListViewSubItem();
                sub.Text = eachPDevInfo.IpAddrToString;
                sub.TextAlign = HorizontalAlignment.Center;
                lvi.SubItems.Add(sub);

                sub = new NCasListViewItem.NCasListViewSubItem();
                sub.Text = string.Empty;
                sub.TextAlign = HorizontalAlignment.Center;
                lvi.SubItems.Add(sub);

                sub = new NCasListViewItem.NCasListViewSubItem();
                sub.Text = string.Empty;
                sub.TextAlign = HorizontalAlignment.Center;
                lvi.SubItems.Add(sub);

                this.serverStatusListView.Items.Add(lvi);
            }
        }

        /// <summary>
        /// 경보단말 ListView 셋팅
        /// </summary>
        private void SetTermListView()
        {
            NCasListViewItem lvi = null;

            foreach (TermInfo eachTermInfo in this.provInfo.LstTerms)
            {
                if (eachTermInfo.UseFlag != NCasDefineUseStatus.Use)
                    continue;

                if (eachTermInfo.TermFlag != NCasDefineTermKind.TermMutil)
                    continue;

                lvi = new NCasListViewItem();
                lvi.Name = eachTermInfo.IpAddrToSring;
                lvi.ImageIndex = 0;

                NCasListViewItem.NCasListViewSubItem sub = new NCasListViewItem.NCasListViewSubItem();
                sub.Text = eachTermInfo.Name;
                sub.TextAlign = HorizontalAlignment.Left;
                lvi.SubItems.Add(sub);

                sub = new NCasListViewItem.NCasListViewSubItem();
                sub.Text = eachTermInfo.IpAddrToSring;
                sub.TextAlign = HorizontalAlignment.Center;
                lvi.SubItems.Add(sub);

                sub = new NCasListViewItem.NCasListViewSubItem();
                sub.Text = string.Empty;
                sub.TextAlign = HorizontalAlignment.Center;
                lvi.SubItems.Add(sub);

                sub = new NCasListViewItem.NCasListViewSubItem();
                sub.Text = string.Empty;
                sub.TextAlign = HorizontalAlignment.Center;
                lvi.SubItems.Add(sub);

                this.termStatusListView.Items.Add(lvi);
            }
        }
        #endregion

        #region ListView 업데이트
        /// <summary>
        /// 통제소 장비 ListView 업데이트
        /// </summary>
        private void setServerStatusUpdate()
        {
            foreach (NCasListViewItem listViewItem in this.serverStatusListView.Items)
            {
                if (listViewItem == null)
                    continue;

                if (listViewItem.Name == string.Empty)
                    continue;

                PDevInfo pDevInfo = this.main.MmfMng.GetPDevInfoByIp(listViewItem.Name);

                if (pDevInfo.DevStsInfo.Status == NCasDefineNormalStatus.Noraml)
                {
                    listViewItem.ImageIndex = 1;
                }
                else
                {
                    listViewItem.ImageIndex = 0;
                }

                listViewItem.SubItems[3].Text = (pDevInfo.DevStsInfo.Status == NCasDefineNormalStatus.None) ? "없음" :
                    (pDevInfo.DevStsInfo.Status == NCasDefineNormalStatus.NoAnswer) ? "무응답" :
                    (pDevInfo.DevStsInfo.Status == NCasDefineNormalStatus.Abnormal) ? "이상" : "정상";

                if ((pDevInfo.DevStsInfo.Status == NCasDefineNormalStatus.Abnormal) || (pDevInfo.DevStsInfo.Status == NCasDefineNormalStatus.NoAnswer))
                {
                    listViewItem.SubItems[4].Text = NCasUtilityMng.INCasCommUtility.MakeDateTimeFormatForCasSystem(pDevInfo.DevStsInfo.OccurTimeToDateTime);
                }
                else if (pDevInfo.DevStsInfo.Status == NCasDefineNormalStatus.Noraml || pDevInfo.DevStsInfo.Status == NCasDefineNormalStatus.None)
                {
                    listViewItem.SubItems[4].Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 경보단말 ListView 업데이트
        /// </summary>
        private void setTermStatusUpdate()
        {
            foreach (NCasListViewItem listViewItem in this.termStatusListView.Items)
            {
                if (listViewItem == null)
                    continue;

                if (listViewItem.Name == string.Empty)
                    continue;

                TermInfo pTermInfo = this.main.MmfMng.GetTermInfoByIp(listViewItem.Name);

                if (pTermInfo.DevStsInfo.Status == NCasDefineNormalStatus.Noraml)
                {
                    listViewItem.ImageIndex = 1;
                }
                else
                {
                    listViewItem.ImageIndex = 0;
                }

                listViewItem.SubItems[3].Text = (pTermInfo.DevStsInfo.Status == NCasDefineNormalStatus.None) ? "없음" :
                    (pTermInfo.DevStsInfo.Status == NCasDefineNormalStatus.NoAnswer) ? "무응답" :
                    (pTermInfo.DevStsInfo.Status == NCasDefineNormalStatus.Abnormal) ? "이상" : "정상";

                if ((pTermInfo.DevStsInfo.Status == NCasDefineNormalStatus.Abnormal) || (pTermInfo.DevStsInfo.Status == NCasDefineNormalStatus.NoAnswer))
                {
                    listViewItem.SubItems[4].Text = NCasUtilityMng.INCasCommUtility.MakeDateTimeFormatForCasSystem(pTermInfo.DevStsInfo.OccurTimeToDateTime);
                }
                else if (pTermInfo.DevStsInfo.Status == NCasDefineNormalStatus.Noraml || pTermInfo.DevStsInfo.Status == NCasDefineNormalStatus.None)
                {
                    listViewItem.SubItems[4].Text = string.Empty;
                }
            }
        }
        #endregion
    }
}