using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NCASFND;
using NCASFND.NCasCtrl;
using NCASBIZ;
using NCASBIZ.NCasData;
using NCASBIZ.NCasDefine;

namespace WmaPAlmScreen
{
    public partial class OrderDistTermForm : Form
    {
        private delegate void TermSelectEventArgsHandler(object sender, TermSelectEventArgs tsea);
        public event EventHandler TermAllSelectEvent; //단말전체 선택 이벤트
        public event EventHandler TermAllCancelEvent; //단말전체 해제 이벤트
        public event EventHandler TermDestinationCancelEvent; //개별발령 취소 이벤트
        public event EventHandler<TermSelectEventArgs> AddTermDestinationEvent; //단말개별 발령에 단말 추가
        public event EventHandler<TermSelectEventArgs> RemoveTermDestinationEvent; //단말개별 발령에 단말 삭제
        private DistInfo distInfo = null;

        /// <summary>
        /// 생성자
        /// </summary>
        public OrderDistTermForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="distInfo"></param>
        public OrderDistTermForm(DistInfo distInfo)
            : this()
        {
            this.distInfo = distInfo;
            this.InitListView();
            this.SetListView();
            this.distNameTextBox.Text = this.distInfo.Name;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (this.TermDestinationCancelEvent != null)
            {
                this.TermDestinationCancelEvent(this, new EventArgs());
            }
        }

        #region ListView 초기화
        /// <summary>
        /// 리스트뷰를 초기화한다.
        /// </summary>
        private void InitListView()
        {
            this.termListView.GridLineStyle = NCasListViewGridLine.GridBoth;
            this.termListView.GridDashStyle = DashStyle.Dot;
            this.termListView.ScrollType = NCasListViewScrollType.ScrollBoth;
            this.termListView.Font = new Font(WmaPAlmScreenRsc.FontName, 10.0f);
            this.termListView.ColumnHeight = 27;
            this.termListView.ItemHeight = 25;
            this.termListView.HideColumnCheckBox = false;

            NCasColumnHeader col = new NCasColumnHeader();
            col.Text = string.Empty;
            col.Width = 30;
            col.SortType = NCasListViewSortType.SortChecked;
            col.TextAlign = HorizontalAlignment.Center;
            col.ColumnLock = true;
            col.ColumnHide = false;
            col.CheckBoxes = true;
            this.termListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "이름";
            col.Width = 135;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Left;
            this.termListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "IP";
            col.Width = 95;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Center;
            this.termListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "소속";
            col.Width = 85;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Center;
            this.termListView.Columns.Add(col);
        }
        #endregion

        #region ListView 셋팅
        /// <summary>
        /// 리스트뷰를 셋팅한다.
        /// </summary>
        private void SetListView()
        {
            foreach (TermInfo termInfo in this.distInfo.LstTerms)
            {
                if (termInfo.UseFlag != NCasDefineUseStatus.Use)
                    continue;

                if (termInfo.TermFlag != NCasDefineTermKind.TermMutil)
                    continue;

                NCasListViewItem lvi = new NCasListViewItem();
                lvi.Name = termInfo.IpAddrToSring;
                lvi.Tag = termInfo;

                NCasListViewItem.NCasListViewSubItem sub = new NCasListViewItem.NCasListViewSubItem();
                sub.Text = termInfo.Name;
                sub.TextAlign = HorizontalAlignment.Left;
                lvi.SubItems.Add(sub);

                sub = new NCasListViewItem.NCasListViewSubItem();
                sub.Text = termInfo.IpAddrToSring;
                sub.TextAlign = HorizontalAlignment.Center;
                lvi.SubItems.Add(sub);

                sub = new NCasListViewItem.NCasListViewSubItem();
                sub.Text = termInfo.DistInfo.Name;
                sub.TextAlign = HorizontalAlignment.Center;
                lvi.SubItems.Add(sub);

                this.termListView.Items.Add(lvi);
            }
        }
        #endregion

        /// <summary>
        /// 취소 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            if (this.TermDestinationCancelEvent != null)
            {
                this.TermDestinationCancelEvent(this, new EventArgs());
            }

            this.Close();
        }

        private void termListView_ColumnChecked(object sender, NCasColumnCheckedEventArgs e)
        {
            if (e.Column.Checked)
            {
                if (this.TermAllSelectEvent != null)
                {
                    this.TermAllSelectEvent(this, new EventArgs());
                }
            }
            else
            {
                if (this.TermAllCancelEvent != null)
                {
                    this.TermAllCancelEvent(this, new EventArgs());
                }
            }
        }

        private void termListView_ItemChecked(object sender, NCasItemCheckedEventArgs e)
        {
            TermInfo ti = null;

            for (int i = 0; i < this.termListView.Items.Count; i++)
            {
                if (e.Item.Name == this.termListView.Items[i].Name)
                {
                    ti = this.termListView.Items[i].Tag as TermInfo;
                }
            }

            if (e.Item.Checked)
            {
                if (this.AddTermDestinationEvent != null)
                {
                    this.AddTermDestinationEvent(this, new TermSelectEventArgs(ti));
                }
            }
            else
            {
                if (this.RemoveTermDestinationEvent != null)
                {
                    this.RemoveTermDestinationEvent(this, new TermSelectEventArgs(ti));
                }
            }

            if (this.termListView.Items.Count == this.termListView.CheckedItems.Count)
            {
                if (this.TermAllSelectEvent != null)
                {
                    this.TermAllSelectEvent(this, new EventArgs());
                }
            }

            if (this.termListView.CheckedItems.Count == 0)
            {
                if (this.TermAllCancelEvent != null)
                {
                    this.TermAllCancelEvent(this, new EventArgs());
                }
            }
        }
    }
}