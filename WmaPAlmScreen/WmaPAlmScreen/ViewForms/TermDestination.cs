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
    public partial class TermDestination : Form
    {
        private delegate void TermSelectEventArgsHandler(object sender, TermSelectEventArgs tsea);
        public event EventHandler TermAllSelectEvent; //단말전체 선택 이벤트
        public event EventHandler TermAllCancelEvent; //단말전체 해제 이벤트
        public event EventHandler TermDestinationCancelEvent; //개별발령 취소 이벤트
        public event EventHandler<TermSelectEventArgs> AddTermDestinationEvent; //단말개별 발령에 단말 추가
        public event EventHandler<TermSelectEventArgs> RemoveTermDestinationEvent; //단말개별 발령에 단말 삭제
        private ProvInfo provInfo = null;

        /// <summary>
        /// 생성자
        /// </summary>
        public TermDestination()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="provInfo"></param>
        public TermDestination(ProvInfo provInfo)
            : this()
        {
            this.provInfo = provInfo;
            this.InitTreeView();
            this.InitListView();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (this.TermDestinationCancelEvent != null)
            {
                this.TermDestinationCancelEvent(this, new EventArgs());
            }
        }

        /// <summary>
        /// 리스트뷰에 전체 단말을 셋팅한다.
        /// </summary>
        private void SetListViewAllTerm()
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
                lvi.Text = eachTermInfo.Name;
                lvi.TextAlign = HorizontalAlignment.Left;
                lvi.Tag = eachTermInfo;

                NCasListViewItem.NCasListViewSubItem sub = new NCasListViewItem.NCasListViewSubItem();
                sub.Text = eachTermInfo.IpAddrToSring;
                sub.TextAlign = HorizontalAlignment.Center;
                lvi.SubItems.Add(sub);

                sub = new NCasListViewItem.NCasListViewSubItem();
                sub.Text = eachTermInfo.DistInfo.Name;
                sub.TextAlign = HorizontalAlignment.Center;
                lvi.SubItems.Add(sub);

                this.selectTermListView.Items.Add(lvi);
            }
        }

        /// <summary>
        /// 리스트뷰를 초기화한다.
        /// </summary>
        private void InitListView()
        {
            this.selectTermListView.GridLineStyle = NCasListViewGridLine.GridBoth;
            this.selectTermListView.GridDashStyle = DashStyle.Dot;
            this.selectTermListView.ScrollType = NCasListViewScrollType.ScrollBoth;
            this.selectTermListView.ColumnHeight = 27;
            this.selectTermListView.ItemHeight = 25;
            this.selectTermListView.HideColumnCheckBox = true;

            NCasColumnHeader col = new NCasColumnHeader();
            col.Text = "이름";
            col.Width = 180;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Left;
            this.selectTermListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "IP";
            col.Width = 115;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Center;
            this.selectTermListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "소속";
            col.Width = 55;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Center;
            this.selectTermListView.Columns.Add(col);
        }

        /// <summary>
        /// 트리뷰에 시군 및 단말을 초기화한다.
        /// </summary>
        private void InitTreeView()
        {
            foreach (DistInfo distInfo in this.provInfo.LstDists)
            {
                bool isTermMulti = false;

                foreach (TermInfo eachTermInfo in distInfo.LstTerms)
                {
                    if (eachTermInfo.TermFlag == NCasDefineTermKind.TermMutil)
                    {
                        isTermMulti = true;
                        break;
                    }
                }

                if (isTermMulti == false)
                {
                    continue;
                }

                TreeNode treeNode = new TreeNode();
                treeNode.Name = distInfo.NetIdToString;
                treeNode.Text = distInfo.Name + " (" + distInfo.NetIdToString + ")";
                treeNode.Tag = distInfo;
                this.termListTreeView.Nodes.Add(treeNode);

                foreach (TermInfo termInfo in distInfo.LstTerms)
                {
                    if (termInfo.UseFlag != NCasDefineUseStatus.Use)
                        continue;

                    if (termInfo.TermFlag != NCasDefineTermKind.TermMutil)
                        continue;

                    TreeNode termTreeNode = new TreeNode();
                    termTreeNode.Name = termInfo.IpAddrToSring;
                    termTreeNode.Text = termInfo.Name + " (" + termInfo.IpAddrToSring + ")";
                    termTreeNode.Tag = termInfo;
                    this.termListTreeView.Nodes[distInfo.NetIdToString].Nodes.Add(termTreeNode);
                }
            }

            this.termListTreeView.ExpandAll();
        }

        /// <summary>
        /// 전체선택 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void allSelectBtn_Click(object sender, EventArgs e)
        {
            foreach (TreeNode distTreeNode in this.termListTreeView.Nodes)
            {
                distTreeNode.Checked = true;

                foreach (TreeNode termTreeNode in distTreeNode.Nodes)
                {
                    termTreeNode.Checked = true;
                }
            }

            this.selectTermListView.Items.Clear();
            this.SetListViewAllTerm();

            if (this.TermAllSelectEvent != null)
            {
                this.TermAllSelectEvent(this, new EventArgs());
            }
        }

        /// <summary>
        /// 선택취소 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectCancelBtn_Click(object sender, EventArgs e)
        {
            foreach (TreeNode distTreeNode in this.termListTreeView.Nodes)
            {
                distTreeNode.Checked = false;

                foreach (TreeNode termTreeNode in distTreeNode.Nodes)
                {
                    termTreeNode.Checked = false;
                }
            }

            this.selectTermListView.Items.Clear();

            if (this.TermAllCancelEvent != null)
            {
                this.TermAllCancelEvent(this, new EventArgs());
            }
        }

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

        /// <summary>
        /// 추가 화살표 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addBtn_Click(object sender, EventArgs e)
        {
            if (this.termListTreeView.SelectedNode == null)
            {
                MessageBox.Show("발령대상 단말을 선택하세요.", "개별 단말 선택", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (this.termListTreeView.SelectedNode.Tag is DistInfo)
            {
                this.termListTreeView.SelectedNode.Checked = true;

                foreach (TreeNode treeNode in this.termListTreeView.SelectedNode.Nodes)
                {
                    treeNode.Checked = true;
                    TermInfo ti = treeNode.Tag as TermInfo;

                    if (this.AddTermDestinationEvent != null)
                    {
                        this.AddTermDestinationEvent(this, new TermSelectEventArgs(ti));
                    }

                    this.AddTermListView(ti);
                }
            }
            else if (this.termListTreeView.SelectedNode.Tag is TermInfo)
            {
                TermInfo ti = this.termListTreeView.SelectedNode.Tag as TermInfo;
                this.termListTreeView.SelectedNode.Checked = true;

                if (this.AddTermDestinationEvent != null)
                {
                    this.AddTermDestinationEvent(this, new TermSelectEventArgs(ti));
                }

                this.AddTermListView(ti);
                bool isAllCheck = true;

                foreach (TreeNode treeNode in this.termListTreeView.SelectedNode.Parent.Nodes)
                {
                    if (treeNode.Checked == false)
                    {
                        isAllCheck = false;
                        break;
                    }
                }

                if (isAllCheck)
                    this.termListTreeView.SelectedNode.Parent.Checked = true;
            }
        }

        /// <summary>
        /// 제거 화살표 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeBtn_Click(object sender, EventArgs e)
        {
            if (this.selectTermListView.SelectedItems.Count == 0)
            {
                MessageBox.Show("취소할 발령대상 단말을 선택하세요.", "개별 단말 선택", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            TermInfo termInfo = this.selectTermListView.SelectedItems[0].Tag as TermInfo;

            if (this.RemoveTermDestinationEvent != null)
            {
                this.RemoveTermDestinationEvent(this, new TermSelectEventArgs(termInfo));
            }

            this.RemoveTermListView(termInfo);

            foreach (TreeNode treeNode in this.termListTreeView.Nodes)
            {
                if (treeNode.Name == termInfo.IpAddrToSring)
                {
                    treeNode.Checked = false;
                    break;
                }

                foreach (TreeNode subTreeNode in treeNode.Nodes)
                {
                    if (subTreeNode.Name == termInfo.IpAddrToSring)
                    {
                        subTreeNode.Checked = false;

                        if (treeNode.Checked == true)
                        {
                            treeNode.Checked = false;
                        }

                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 트리뷰 체크 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void termListTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Checked)
                {
                    if (e.Node.Tag is DistInfo)
                    {
                        foreach (TreeNode treeNode in e.Node.Nodes)
                        {
                            treeNode.Checked = true;
                            TermInfo ti = treeNode.Tag as TermInfo;

                            if (this.AddTermDestinationEvent != null)
                            {
                                this.AddTermDestinationEvent(this, new TermSelectEventArgs(ti));
                            }

                            this.AddTermListView(ti);
                        }
                    }
                    else if (e.Node.Tag is TermInfo)
                    {
                        TermInfo ti = e.Node.Tag as TermInfo;

                        if (this.AddTermDestinationEvent != null)
                        {
                            this.AddTermDestinationEvent(this, new TermSelectEventArgs(ti));
                        }

                        this.AddTermListView(ti);
                        bool isAllCheck = true;

                        foreach (TreeNode treeNode in e.Node.Parent.Nodes)
                        {
                            if (treeNode.Checked == false)
                            {
                                isAllCheck = false;
                                break;
                            }
                        }

                        if (isAllCheck)
                            e.Node.Parent.Checked = true;
                    }
                }
                else //체크 해제
                {
                    if (e.Node.Tag is DistInfo)
                    {
                        foreach (TreeNode treeNode in e.Node.Nodes)
                        {
                            treeNode.Checked = false;
                            TermInfo ti = treeNode.Tag as TermInfo;

                            if (this.RemoveTermDestinationEvent != null)
                            {
                                this.RemoveTermDestinationEvent(this, new TermSelectEventArgs(ti));
                            }

                            this.RemoveTermListView(ti);
                        }
                    }
                    else if (e.Node.Tag is TermInfo)
                    {
                        TermInfo ti = e.Node.Tag as TermInfo;

                        if (this.RemoveTermDestinationEvent != null)
                        {
                            this.RemoveTermDestinationEvent(this, new TermSelectEventArgs(ti));
                        }

                        this.RemoveTermListView(ti);

                        if (e.Node.Parent.Checked == true)
                            e.Node.Parent.Checked = false;
                    }
                }
            }
        }

        /// <summary>
        /// 단말을 리스트뷰에 추가한다.
        /// </summary>
        /// <param name="termInfo"></param>
        private void AddTermListView(TermInfo eachTermInfo)
        {
            NCasListViewItem lvi = null;

            foreach (NCasListViewItem eachLvi in this.selectTermListView.Items)
            {
                if (eachLvi.Name == eachTermInfo.IpAddrToSring)
                {
                    lvi = eachLvi;
                    break;
                }
            }

            if (lvi != null)
                return;

            lvi = null;

            if (eachTermInfo.UseFlag != NCasDefineUseStatus.Use)
                return;

            if (eachTermInfo.TermFlag != NCasDefineTermKind.TermMutil)
                return;

            lvi = new NCasListViewItem();
            lvi.Name = eachTermInfo.IpAddrToSring;
            lvi.Text = eachTermInfo.Name;
            lvi.TextAlign = HorizontalAlignment.Left;
            lvi.Tag = eachTermInfo;

            NCasListViewItem.NCasListViewSubItem sub = new NCasListViewItem.NCasListViewSubItem();
            sub.Text = eachTermInfo.IpAddrToSring;
            sub.TextAlign = HorizontalAlignment.Center;
            lvi.SubItems.Add(sub);

            sub = new NCasListViewItem.NCasListViewSubItem();
            sub.Text = eachTermInfo.DistInfo.Name;
            sub.TextAlign = HorizontalAlignment.Center;
            lvi.SubItems.Add(sub);

            this.selectTermListView.Items.Add(lvi);

            if (this.selectTermListView.Items.Count == this.provInfo.GetUsableAlarmTermCnt())
            {
                if (this.TermAllSelectEvent != null)
                {
                    this.TermAllSelectEvent(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// 단말을 리스트뷰에서 제거한다.
        /// </summary>
        /// <param name="termInfo"></param>
        private void RemoveTermListView(TermInfo termInfo)
        {
            NCasListViewItem lvi = null;

            foreach (NCasListViewItem eachLvi in this.selectTermListView.Items)
            {
                if (eachLvi.Name == termInfo.IpAddrToSring)
                {
                    lvi = eachLvi;
                    break;
                }
            }

            if (lvi != null)
            {
                this.selectTermListView.Items.Remove(lvi);
            }
        }

        /// <summary>
        /// 트리뷰에서 노드를 더블클릭할 때 발생하는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void termListTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Checked == true)
            {
                if (e.Node.Tag is TermInfo)
                {
                    e.Node.Checked = false;
                    TermInfo ti = e.Node.Tag as TermInfo;
                    
                    if (this.RemoveTermDestinationEvent != null)
                    {
                        this.RemoveTermDestinationEvent(this, new TermSelectEventArgs(ti));
                    }

                    this.RemoveTermListView(ti);

                    if (e.Node.Parent.Checked == true)
                        e.Node.Parent.Checked = false;
                }
            }
            else
            {
                if (e.Node.Tag is TermInfo)
                {
                    e.Node.Checked = true;
                    TermInfo ti = e.Node.Tag as TermInfo;
                    
                    if (this.AddTermDestinationEvent != null)
                    {
                        this.AddTermDestinationEvent(this, new TermSelectEventArgs(ti));
                    }

                    this.AddTermListView(ti);
                    bool isAllCheck = true;

                    foreach (TreeNode treeNode in e.Node.Parent.Nodes)
                    {
                        if (treeNode.Checked == false)
                        {
                            isAllCheck = false;
                            break;
                        }
                    }

                    if (isAllCheck)
                        e.Node.Parent.Checked = true;
                }
            }
        }

        /// <summary>
        /// 리스트뷰를 더블클릭할 때 발생하는 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectTermListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.selectTermListView.SelectedItems.Count == 0)
                return;

            TermInfo termInfo = this.selectTermListView.SelectedItems[0].Tag as TermInfo;
            
            if (this.RemoveTermDestinationEvent != null)
            {
                this.RemoveTermDestinationEvent(this, new TermSelectEventArgs(termInfo));
            }

            this.RemoveTermListView(termInfo);

            foreach (TreeNode treeNode in this.termListTreeView.Nodes)
            {
                if (treeNode.Name == termInfo.IpAddrToSring)
                {
                    treeNode.Checked = false;
                    break;
                }

                foreach (TreeNode subTreeNode in treeNode.Nodes)
                {
                    if (subTreeNode.Name == termInfo.IpAddrToSring)
                    {
                        subTreeNode.Checked = false;

                        if (treeNode.Checked == true)
                        {
                            treeNode.Checked = false;
                        }

                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 단말 추가/삭제를 위한 이벤트 아규먼트 클래스
    /// </summary>
    public class TermSelectEventArgs : EventArgs
    {
        private TermInfo termInfo = null;

        public TermInfo TermInfo
        {
            get { return this.termInfo; }
            set { this.termInfo = value; }
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="termInfo"></param>
        public TermSelectEventArgs(TermInfo termInfo)
        {
            this.termInfo = termInfo;
        }
    }
}