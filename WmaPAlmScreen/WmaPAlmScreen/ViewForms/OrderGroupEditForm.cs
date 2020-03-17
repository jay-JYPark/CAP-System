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
using NCASFND.NCasLogging;
using NCASBIZ;
using NCASBIZ.NCasBiz;
using NCASBIZ.NCasData;
using NCASBIZ.NCasDefine;

namespace WmaPAlmScreen
{
    public partial class OrderGroupEditForm : Form
    {
        private ProvInfo provInfo = null;
        private bool isTerm = true;
        private NCasButton ncasButton = null;

        /// <summary>
        /// 생성자
        /// </summary>
        public OrderGroupEditForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="orderview"></param>
        /// <param name="type">1-단말그룹, 2-시군그룹</param>
        public OrderGroupEditForm(OrderView19201080 orderview, byte type, NCasButton button)
            : this()
        {
            if (type == 2)
            {
                this.isTerm = false;
            }

            this.provInfo = orderview.ProvInfo;
            this.ncasButton = button;
            this.Init();
        }

        /// <summary>
        /// 저장 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveBtn_Click(object sender, EventArgs e)
        {
            try
            {
                switch (this.ncasButton.Name)
                {
                    case "termGroupBtn1":
                        GroupContentMng.LstGroupContent[0].LstGroupData.Clear();
                        GroupContentMng.LstGroupContent[0].Title = this.groupNameTextBox.Text;
                        GroupData groupData = null;

                        for (int i = 0; i < this.groupListView.Items.Count; i++)
                        {
                            if (this.groupListView.Items[i].Checked)
                            {
                                groupData = new GroupData();
                                groupData.IpAddr = this.groupListView.Items[i].Name;
                                groupData.IsDist = false;
                                groupData.Title = this.groupListView.Items[i].SubItems[1].Text;
                                GroupContentMng.LstGroupContent[0].LstGroupData.Add(groupData);
                            }
                        }

                        GroupContentMng.SaveGroupContent();
                        break;

                    case "termGroupBtn2":
                        GroupContentMng.LstGroupContent[1].LstGroupData.Clear();
                        GroupContentMng.LstGroupContent[1].Title = this.groupNameTextBox.Text;

                        for (int i = 0; i < this.groupListView.Items.Count; i++)
                        {
                            if (this.groupListView.Items[i].Checked)
                            {
                                groupData = new GroupData();
                                groupData.IpAddr = this.groupListView.Items[i].Name;
                                groupData.IsDist = false;
                                groupData.Title = this.groupListView.Items[i].SubItems[1].Text;
                                GroupContentMng.LstGroupContent[1].LstGroupData.Add(groupData);
                            }
                        }

                        GroupContentMng.SaveGroupContent();
                        break;

                    case "termGroupBtn3":
                        GroupContentMng.LstGroupContent[2].LstGroupData.Clear();
                        GroupContentMng.LstGroupContent[2].Title = this.groupNameTextBox.Text;

                        for (int i = 0; i < this.groupListView.Items.Count; i++)
                        {
                            if (this.groupListView.Items[i].Checked)
                            {
                                groupData = new GroupData();
                                groupData.IpAddr = this.groupListView.Items[i].Name;
                                groupData.IsDist = false;
                                groupData.Title = this.groupListView.Items[i].SubItems[1].Text;
                                GroupContentMng.LstGroupContent[2].LstGroupData.Add(groupData);
                            }
                        }

                        GroupContentMng.SaveGroupContent();
                        break;

                    case "termGroupBtn4":
                        GroupContentMng.LstGroupContent[3].LstGroupData.Clear();
                        GroupContentMng.LstGroupContent[3].Title = this.groupNameTextBox.Text;

                        for (int i = 0; i < this.groupListView.Items.Count; i++)
                        {
                            if (this.groupListView.Items[i].Checked)
                            {
                                groupData = new GroupData();
                                groupData.IpAddr = this.groupListView.Items[i].Name;
                                groupData.IsDist = false;
                                groupData.Title = this.groupListView.Items[i].SubItems[1].Text;
                                GroupContentMng.LstGroupContent[3].LstGroupData.Add(groupData);
                            }
                        }

                        GroupContentMng.SaveGroupContent();
                        break;

                    case "termGroupBtn5":
                        GroupContentMng.LstGroupContent[4].LstGroupData.Clear();
                        GroupContentMng.LstGroupContent[4].Title = this.groupNameTextBox.Text;

                        for (int i = 0; i < this.groupListView.Items.Count; i++)
                        {
                            if (this.groupListView.Items[i].Checked)
                            {
                                groupData = new GroupData();
                                groupData.IpAddr = this.groupListView.Items[i].Name;
                                groupData.IsDist = false;
                                groupData.Title = this.groupListView.Items[i].SubItems[1].Text;
                                GroupContentMng.LstGroupContent[4].LstGroupData.Add(groupData);
                            }
                        }

                        GroupContentMng.SaveGroupContent();
                        break;

                    case "termGroupBtn6":
                        GroupContentMng.LstGroupContent[5].LstGroupData.Clear();
                        GroupContentMng.LstGroupContent[5].Title = this.groupNameTextBox.Text;

                        for (int i = 0; i < this.groupListView.Items.Count; i++)
                        {
                            if (this.groupListView.Items[i].Checked)
                            {
                                groupData = new GroupData();
                                groupData.IpAddr = this.groupListView.Items[i].Name;
                                groupData.IsDist = false;
                                groupData.Title = this.groupListView.Items[i].SubItems[1].Text;
                                GroupContentMng.LstGroupContent[5].LstGroupData.Add(groupData);
                            }
                        }

                        GroupContentMng.SaveGroupContent();
                        break;

                    case "termGroupBtn7":
                        GroupContentMng.LstGroupContent[6].LstGroupData.Clear();
                        GroupContentMng.LstGroupContent[6].Title = this.groupNameTextBox.Text;

                        for (int i = 0; i < this.groupListView.Items.Count; i++)
                        {
                            if (this.groupListView.Items[i].Checked)
                            {
                                groupData = new GroupData();
                                groupData.IpAddr = this.groupListView.Items[i].Name;
                                groupData.IsDist = false;
                                groupData.Title = this.groupListView.Items[i].SubItems[1].Text;
                                GroupContentMng.LstGroupContent[6].LstGroupData.Add(groupData);
                            }
                        }

                        GroupContentMng.SaveGroupContent();
                        break;

                    case "termGroupBtn8":
                        GroupContentMng.LstGroupContent[7].LstGroupData.Clear();
                        GroupContentMng.LstGroupContent[7].Title = this.groupNameTextBox.Text;

                        for (int i = 0; i < this.groupListView.Items.Count; i++)
                        {
                            if (this.groupListView.Items[i].Checked)
                            {
                                groupData = new GroupData();
                                groupData.IpAddr = this.groupListView.Items[i].Name;
                                groupData.IsDist = false;
                                groupData.Title = this.groupListView.Items[i].SubItems[1].Text;
                                GroupContentMng.LstGroupContent[7].LstGroupData.Add(groupData);
                            }
                        }

                        GroupContentMng.SaveGroupContent();
                        break;

                    case "termGroupBtn9":
                        GroupContentMng.LstGroupContent[8].LstGroupData.Clear();
                        GroupContentMng.LstGroupContent[8].Title = this.groupNameTextBox.Text;

                        for (int i = 0; i < this.groupListView.Items.Count; i++)
                        {
                            if (this.groupListView.Items[i].Checked)
                            {
                                groupData = new GroupData();
                                groupData.IpAddr = this.groupListView.Items[i].Name;
                                groupData.IsDist = false;
                                groupData.Title = this.groupListView.Items[i].SubItems[1].Text;
                                GroupContentMng.LstGroupContent[8].LstGroupData.Add(groupData);
                            }
                        }

                        GroupContentMng.SaveGroupContent();
                        break;

                    case "termGroupBtn10":
                        GroupContentMng.LstGroupContent[9].LstGroupData.Clear();
                        GroupContentMng.LstGroupContent[9].Title = this.groupNameTextBox.Text;

                        for (int i = 0; i < this.groupListView.Items.Count; i++)
                        {
                            if (this.groupListView.Items[i].Checked)
                            {
                                groupData = new GroupData();
                                groupData.IpAddr = this.groupListView.Items[i].Name;
                                groupData.IsDist = false;
                                groupData.Title = this.groupListView.Items[i].SubItems[1].Text;
                                GroupContentMng.LstGroupContent[9].LstGroupData.Add(groupData);
                            }
                        }

                        GroupContentMng.SaveGroupContent();
                        break;

                    case "distGroupBtn1":
                        GroupContentMng.LstGroupContent[10].LstGroupData.Clear();
                        GroupContentMng.LstGroupContent[10].Title = this.groupNameTextBox.Text;

                        for (int i = 0; i < this.groupListView.Items.Count; i++)
                        {
                            if (this.groupListView.Items[i].Checked)
                            {
                                groupData = new GroupData();
                                groupData.IpAddr = this.groupListView.Items[i].Name;
                                groupData.IsDist = false;
                                groupData.Title = this.groupListView.Items[i].SubItems[1].Text;
                                GroupContentMng.LstGroupContent[10].LstGroupData.Add(groupData);
                            }
                        }

                        GroupContentMng.SaveGroupContent();
                        break;

                    case "distGroupBtn2":
                        GroupContentMng.LstGroupContent[11].LstGroupData.Clear();
                        GroupContentMng.LstGroupContent[11].Title = this.groupNameTextBox.Text;

                        for (int i = 0; i < this.groupListView.Items.Count; i++)
                        {
                            if (this.groupListView.Items[i].Checked)
                            {
                                groupData = new GroupData();
                                groupData.IpAddr = this.groupListView.Items[i].Name;
                                groupData.IsDist = false;
                                groupData.Title = this.groupListView.Items[i].SubItems[1].Text;
                                GroupContentMng.LstGroupContent[11].LstGroupData.Add(groupData);
                            }
                        }

                        GroupContentMng.SaveGroupContent();
                        break;
                }

                MessageBox.Show("그룹 정보가 저장되었습니다.", "그룹 편집", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                NCasLoggingMng.ILoggingException.WriteException("OrderGroupEditForm", "OrderGroupEditForm.saveBtn_Click Method", ex);
                MessageBox.Show("그룹 정보 저장에 실패했습니다.\n다시 시도하세요.", "그룹 편집", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 닫기 버튼 클릭 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 기초데이터 초기화
        /// </summary>
        private void Init()
        {
            #region ListView 초기화
            this.groupListView.GridLineStyle = NCasListViewGridLine.GridBoth;
            this.groupListView.GridDashStyle = DashStyle.Dot;
            this.groupListView.ScrollType = NCasListViewScrollType.ScrollBoth;
            this.groupListView.Font = new Font(WmaPAlmScreenRsc.FontName, 10.0f);
            this.groupListView.ColumnHeight = 29;
            this.groupListView.ItemHeight = 26;
            this.groupListView.HideColumnCheckBox = false;

            NCasColumnHeader col = new NCasColumnHeader();
            col.Text = string.Empty;
            col.Width = 30;
            col.SortType = NCasListViewSortType.SortChecked;
            col.TextAlign = HorizontalAlignment.Center;
            col.ColumnLock = true;
            col.ColumnHide = false;
            col.CheckBoxes = true;
            this.groupListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "이름";
            col.Width = 140;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Left;
            this.groupListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "IP";
            col.Width = 90;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Center;
            this.groupListView.Columns.Add(col);

            col = new NCasColumnHeader();
            col.Text = "소속";
            col.Width = 60;
            col.SortType = NCasListViewSortType.SortText;
            col.TextAlign = HorizontalAlignment.Center;
            this.groupListView.Columns.Add(col);
            #endregion

            #region ListView Items 셋팅
            if (this.isTerm)
            {
                foreach (TermInfo termInfo in this.provInfo.LstTerms)
                {
                    if (termInfo.UseFlag != NCasDefineUseStatus.Use)
                        continue;

                    if (termInfo.TermFlag != NCasDefineTermKind.TermMutil)
                        continue;

                    NCasListViewItem lvi = new NCasListViewItem();
                    lvi.Name = termInfo.IpAddrToSring;

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

                    this.groupListView.Items.Add(lvi);
                }
            }
            else
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

                    NCasListViewItem lvi = new NCasListViewItem();
                    lvi.Name = distInfo.NetIdToString;

                    NCasListViewItem.NCasListViewSubItem sub = new NCasListViewItem.NCasListViewSubItem();
                    sub.Text = distInfo.Name;
                    sub.TextAlign = HorizontalAlignment.Left;
                    lvi.SubItems.Add(sub);

                    sub = new NCasListViewItem.NCasListViewSubItem();
                    sub.Text = distInfo.NetIdToString;
                    sub.TextAlign = HorizontalAlignment.Center;
                    lvi.SubItems.Add(sub);

                    sub = new NCasListViewItem.NCasListViewSubItem();
                    sub.Text = this.provInfo.Name;
                    sub.TextAlign = HorizontalAlignment.Center;
                    lvi.SubItems.Add(sub);

                    this.groupListView.Items.Add(lvi);
                }
            }
            #endregion

            #region ListView Checked 셋팅
            switch (this.ncasButton.Name)
            {
                case "termGroupBtn1":
                    this.groupNameTextBox.Text = GroupContentMng.LstGroupContent[0].Title;

                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[0].LstGroupData)
                    {
                        foreach (NCasListViewItem eachLvi in this.groupListView.Items)
                        {
                            if (eachGroupData.IpAddr == eachLvi.Name)
                            {
                                eachLvi.Checked = true;
                                break;
                            }
                        }
                    }
                    break;

                case "termGroupBtn2":
                    this.groupNameTextBox.Text = GroupContentMng.LstGroupContent[1].Title;

                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[1].LstGroupData)
                    {
                        foreach (NCasListViewItem eachLvi in this.groupListView.Items)
                        {
                            if (eachGroupData.IpAddr == eachLvi.Name)
                            {
                                eachLvi.Checked = true;
                                break;
                            }
                        }
                    }
                    break;

                case "termGroupBtn3":
                    this.groupNameTextBox.Text = GroupContentMng.LstGroupContent[2].Title;

                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[2].LstGroupData)
                    {
                        foreach (NCasListViewItem eachLvi in this.groupListView.Items)
                        {
                            if (eachGroupData.IpAddr == eachLvi.Name)
                            {
                                eachLvi.Checked = true;
                                break;
                            }
                        }
                    }
                    break;

                case "termGroupBtn4":
                    this.groupNameTextBox.Text = GroupContentMng.LstGroupContent[3].Title;

                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[3].LstGroupData)
                    {
                        foreach (NCasListViewItem eachLvi in this.groupListView.Items)
                        {
                            if (eachGroupData.IpAddr == eachLvi.Name)
                            {
                                eachLvi.Checked = true;
                                break;
                            }
                        }
                    }
                    break;

                case "termGroupBtn5":
                    this.groupNameTextBox.Text = GroupContentMng.LstGroupContent[4].Title;

                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[4].LstGroupData)
                    {
                        foreach (NCasListViewItem eachLvi in this.groupListView.Items)
                        {
                            if (eachGroupData.IpAddr == eachLvi.Name)
                            {
                                eachLvi.Checked = true;
                                break;
                            }
                        }
                    }
                    break;

                case "termGroupBtn6":
                    this.groupNameTextBox.Text = GroupContentMng.LstGroupContent[5].Title;

                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[5].LstGroupData)
                    {
                        foreach (NCasListViewItem eachLvi in this.groupListView.Items)
                        {
                            if (eachGroupData.IpAddr == eachLvi.Name)
                            {
                                eachLvi.Checked = true;
                                break;
                            }
                        }
                    }
                    break;

                case "termGroupBtn7":
                    this.groupNameTextBox.Text = GroupContentMng.LstGroupContent[6].Title;

                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[6].LstGroupData)
                    {
                        foreach (NCasListViewItem eachLvi in this.groupListView.Items)
                        {
                            if (eachGroupData.IpAddr == eachLvi.Name)
                            {
                                eachLvi.Checked = true;
                                break;
                            }
                        }
                    }
                    break;

                case "termGroupBtn8":
                    this.groupNameTextBox.Text = GroupContentMng.LstGroupContent[7].Title;

                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[7].LstGroupData)
                    {
                        foreach (NCasListViewItem eachLvi in this.groupListView.Items)
                        {
                            if (eachGroupData.IpAddr == eachLvi.Name)
                            {
                                eachLvi.Checked = true;
                                break;
                            }
                        }
                    }
                    break;

                case "termGroupBtn9":
                    this.groupNameTextBox.Text = GroupContentMng.LstGroupContent[8].Title;

                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[8].LstGroupData)
                    {
                        foreach (NCasListViewItem eachLvi in this.groupListView.Items)
                        {
                            if (eachGroupData.IpAddr == eachLvi.Name)
                            {
                                eachLvi.Checked = true;
                                break;
                            }
                        }
                    }
                    break;

                case "termGroupBtn10":
                    this.groupNameTextBox.Text = GroupContentMng.LstGroupContent[9].Title;

                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[9].LstGroupData)
                    {
                        foreach (NCasListViewItem eachLvi in this.groupListView.Items)
                        {
                            if (eachGroupData.IpAddr == eachLvi.Name)
                            {
                                eachLvi.Checked = true;
                                break;
                            }
                        }
                    }
                    break;

                case "distGroupBtn1":
                    this.groupNameTextBox.Text = GroupContentMng.LstGroupContent[10].Title;

                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[10].LstGroupData)
                    {
                        foreach (NCasListViewItem eachLvi in this.groupListView.Items)
                        {
                            if (eachGroupData.IpAddr == eachLvi.Name)
                            {
                                eachLvi.Checked = true;
                                break;
                            }
                        }
                    }
                    break;

                case "distGroupBtn2":
                    this.groupNameTextBox.Text = GroupContentMng.LstGroupContent[11].Title;

                    foreach (GroupData eachGroupData in GroupContentMng.LstGroupContent[11].LstGroupData)
                    {
                        foreach (NCasListViewItem eachLvi in this.groupListView.Items)
                        {
                            if (eachGroupData.IpAddr == eachLvi.Name)
                            {
                                eachLvi.Checked = true;
                                break;
                            }
                        }
                    }
                    break;
            }
            #endregion
        }
    }
}