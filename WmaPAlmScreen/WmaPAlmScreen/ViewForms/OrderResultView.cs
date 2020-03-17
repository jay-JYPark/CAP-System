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
using NCASBIZ.NCasDefine;
using NCASBIZ.NCasUtility;

namespace WmaPAlmScreen
{
    public partial class OrderResultView : ViewBase
    {
        private ProvInfo provInfo = null;

        /// <summary>
        /// 생성자
        /// </summary>
        public OrderResultView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="main">MainForm에서 넘겨주는 참조</param>
        public OrderResultView(MainForm main)
            : this()
        {
            this.main = main;
            this.provInfo = main.ProvInfo;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.InitListView();
            this.SetTermListView();
            this.main.AddTimerMember(this);
        }

        public override void OnTimer()
        {
            base.OnTimer();
            this.setTermStatusUpdate();
        }

        #region ListView 초기화
        /// <summary>
        /// 발령결과 ListView 초기화
        /// </summary>
        public void InitListView()
        {
            this.orderResultListView.GridLineStyle = NCasListViewGridLine.GridBoth;
            this.orderResultListView.GridDashStyle = DashStyle.Dot;
            this.orderResultListView.ScrollType = NCasListViewScrollType.ScrollBoth;
            this.orderResultListView.Font = new Font(WmaPAlmScreenRsc.FontName, 11.0f);
            this.orderResultListView.ColumnHeight = 32;
            this.orderResultListView.ItemHeight = 29;

            NCasColumnHeader col = new NCasColumnHeader();
            col.Text = "...";
            col.Width = 33;
            col.SortType = NCasListViewSortType.SortIcon;
            col.TextAlign = HorizontalAlignment.Center;
            col.ColumnLock = true;
            this.orderResultListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "장비명";
            col.Width = 180;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Left;
            this.orderResultListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "IP";
            col.Width = 150;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Center;
            this.orderResultListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "모드";
            col.Width = 120;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Center;
            this.orderResultListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "발령원";
            col.Width = 150;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Center;
            this.orderResultListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "경보종류";
            col.Width = 150;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Center;
            this.orderResultListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "발령매체";
            col.Width = 150;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Center;
            this.orderResultListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "발령시각";
            col.Width = 200;
            col.SortType = NCasListViewSortType.SortDateTime;
            col.TextAlign = HorizontalAlignment.Center;
            this.orderResultListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "응답시각";
            col.Width = 200;
            col.SortType = NCasListViewSortType.SortDateTime;
            col.TextAlign = HorizontalAlignment.Center;
            this.orderResultListView.Columns.Add(col);
        }
        #endregion

        #region ListView 셋팅
        /// <summary>
        /// 발령결과 ListView 셋팅
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

                sub = new NCasListViewItem.NCasListViewSubItem();
                sub.Text = string.Empty;
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

                sub = new NCasListViewItem.NCasListViewSubItem();
                sub.Text = string.Empty;
                sub.TextAlign = HorizontalAlignment.Center;
                lvi.SubItems.Add(sub);

                this.orderResultListView.Items.Add(lvi);
            }
        }
        #endregion

        #region ListView 업데이트
        /// <summary>
        /// 발령결과 ListView 업데이트
        /// </summary>
        private void setTermStatusUpdate()
        {
            foreach (NCasListViewItem listViewItem in this.orderResultListView.Items)
            {
                if (listViewItem == null)
                    continue;

                if (listViewItem.Name == string.Empty)
                    continue;

                TermInfo pTermInfo = this.main.MmfMng.GetTermInfoByIp(listViewItem.Name);

                if (pTermInfo.AlarmOrderInfo.OccurTimeToDateTime > pTermInfo.WwsAlarmOrderInfo.OccurTimeToDateTime)
                {
                    listViewItem.ImageIndex = (pTermInfo.AlarmOrderInfo.Kind == NCasDefineOrderKind.AlarmStandby) ? 0 : //예비
                        (pTermInfo.AlarmOrderInfo.Kind == NCasDefineOrderKind.AlarmWatch) ? 1 : //경계
                        (pTermInfo.AlarmOrderInfo.Kind == NCasDefineOrderKind.AlarmAttack) ? 2 : //공습
                        (pTermInfo.AlarmOrderInfo.Kind == NCasDefineOrderKind.AlarmBiochemist) ? 3 : //화생방
                        (pTermInfo.AlarmOrderInfo.Kind == NCasDefineOrderKind.AlarmCancel) ? 4 : //해제
                        (pTermInfo.AlarmOrderInfo.Kind == NCasDefineOrderKind.DisasterWatch) ? 5 : //재난위험(사이렌)
                        (pTermInfo.AlarmOrderInfo.Kind == NCasDefineOrderKind.DisasterBroadcast) ? 6 : 4; //재난경계(방송)

                    listViewItem.SubItems[3].Text = NCasUtilityMng.INCasCommUtility.NCasDefineOrderMode2String(pTermInfo.AlarmOrderInfo.Mode);
                    listViewItem.SubItems[4].Text = NCasUtilityMng.INCasCommUtility.NCasDefineOrderSource2String(pTermInfo.AlarmOrderInfo.Source);
                    listViewItem.SubItems[5].Text = NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(pTermInfo.AlarmOrderInfo.Kind);
                    listViewItem.SubItems[6].Text = NCasUtilityMng.INCasCommUtility.NCasDefineOrderMedia2String(pTermInfo.AlarmOrderInfo.Media);
                    listViewItem.SubItems[7].Text = NCasUtilityMng.INCasCommUtility.MakeDateTimeFormatForCasSystem(pTermInfo.AlarmOrderInfo.OccurTimeToDateTime);

                    if (pTermInfo.AlarmResponseInfo.AlarmResponse == NCasDefineResponse.None)
                    {
                        listViewItem.SubItems[8].Text = string.Empty;
                    }
                    else
                    {
                        listViewItem.SubItems[8].Text = NCasUtilityMng.INCasCommUtility.MakeDateTimeFormatForCasSystem(pTermInfo.AlarmResponseInfo.RespTimeToDateTime);
                    }
                }
                else
                {
                    listViewItem.ImageIndex = (pTermInfo.WwsAlarmOrderInfo.Kind == NCasDefineOrderKind.AlarmStandby) ? 0 : //예비
                        (pTermInfo.WwsAlarmOrderInfo.Kind == NCasDefineOrderKind.AlarmWatch) ? 1 : //경계
                        (pTermInfo.WwsAlarmOrderInfo.Kind == NCasDefineOrderKind.AlarmAttack) ? 2 : //공습
                        (pTermInfo.WwsAlarmOrderInfo.Kind == NCasDefineOrderKind.AlarmBiochemist) ? 3 : //화생방
                        (pTermInfo.WwsAlarmOrderInfo.Kind == NCasDefineOrderKind.AlarmCancel) ? 4 : //해제
                        (pTermInfo.WwsAlarmOrderInfo.Kind == NCasDefineOrderKind.DisasterWatch) ? 5 : //재난위험(사이렌)
                        (pTermInfo.WwsAlarmOrderInfo.Kind == NCasDefineOrderKind.WmaAutoAlarm) ? 6 : //재난위험(사이렌)
                        (pTermInfo.WwsAlarmOrderInfo.Kind == NCasDefineOrderKind.DisasterBroadcast) ? 6 : 4; //재난경계(방송)

                    listViewItem.SubItems[3].Text = NCasUtilityMng.INCasCommUtility.NCasDefineOrderMode2String(pTermInfo.WwsAlarmOrderInfo.Mode);
                    listViewItem.SubItems[4].Text = NCasUtilityMng.INCasCommUtility.NCasDefineOrderSource2String(pTermInfo.WwsAlarmOrderInfo.Source);
                    listViewItem.SubItems[5].Text = NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(pTermInfo.WwsAlarmOrderInfo.Kind);
                    listViewItem.SubItems[6].Text = NCasUtilityMng.INCasCommUtility.NCasDefineOrderMedia2String(pTermInfo.WwsAlarmOrderInfo.Media);
                    listViewItem.SubItems[7].Text = NCasUtilityMng.INCasCommUtility.MakeDateTimeFormatForCasSystem(pTermInfo.WwsAlarmOrderInfo.OccurTimeToDateTime);

                    if (pTermInfo.WwsAlarmResponseInfo.AlarmResponse == NCasDefineResponse.None)
                    {
                        listViewItem.SubItems[8].Text = string.Empty;
                    }
                    else
                    {
                        listViewItem.SubItems[8].Text = NCasUtilityMng.INCasCommUtility.MakeDateTimeFormatForCasSystem(pTermInfo.WwsAlarmResponseInfo.RespTimeToDateTime);
                    }
                }
            }
        }
        #endregion
    }
}