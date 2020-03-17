using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using StdWarningInstallation.DataClass;

namespace StdWarningInstallation
{
    public partial class SWRWarningItemDetailForm : Form
    {
        private SWRProfile currentProfile = new SWRProfile();
        public SWRWarningItemDetailForm(SWRProfile profile)
        {
            InitializeComponent();

            this.currentProfile.DeepCopyFrom(profile);

            InitializeOrderDetailList();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (this.currentProfile == null)
            {
                return;
            }

            UpdateHeadline();
            UpdateItemDetailList();
        }

        /// <summary>
        /// 기상특보통보문 상세 리스트 초기화.
        /// </summary>
        private void InitializeOrderDetailList()
        {
            this.dgvItemDetail.Rows.Clear();
            this.dgvItemDetail.Columns.Clear();

            DataGridViewColumn column = new DataGridViewColumn();
            column.Name = "항목";
            column.Width = 80;
            column.CellTemplate = new DataGridViewTextBoxCell();
            this.dgvItemDetail.Columns.Add(column);
            column = new DataGridViewColumn();
            column.Name = "내용";
            column.Width = 300;
            column.CellTemplate = new DataGridViewTextBoxCell();
            this.dgvItemDetail.Columns.Add(column);
        }
        /// <summary>
        /// 기상특보통보문 상세 리스트 갱신.
        /// </summary>
        private void UpdateItemDetailList()
        {
            this.dgvItemDetail.Rows.Clear();

            if (this.dgvItemDetail.Columns == null)
            {
                return;
            }
            if (this.currentProfile == null)
            {
                MessageBox.Show("데이터가 존재하지 않습니다.", "기상특보상세", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            SWRWarningItemProfile profile = this.currentProfile.GetWarningItemProfile();
            if (profile == null)
            {
                MessageBox.Show("데이터 변환 중 오류가 발생하였습니다.", "기상특보상세", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.dgvItemDetail.Rows.Add("지점코드", profile.StationID);
            if (profile.AnnounceTime.Ticks > 0)
            {
                this.dgvItemDetail.Rows.Add("발표시각", profile.AnnounceTime.ToString());
            }
            else
            {
                this.dgvItemDetail.Rows.Add("발표시각", "-");
            }
            this.dgvItemDetail.Rows.Add("발표번호(월별)", profile.SequenceNo.ToString());
            this.dgvItemDetail.Rows.Add("특보발표코드", profile.CommandCode);
            this.dgvItemDetail.Rows.Add("제목", profile.Title);
            this.dgvItemDetail.Rows.Add("해당구역", profile.TargetAreas);
            this.dgvItemDetail.Rows.Add("발효시각", profile.EffectStartInfo);
            this.dgvItemDetail.Rows.Add("내용", profile.Contents);
            if (profile.PresentConditionTime.Ticks > 0)
            {
                this.dgvItemDetail.Rows.Add("특보발효현황시각", profile.PresentConditionTime.ToString());
            }
            else
            {
                this.dgvItemDetail.Rows.Add("특보발효현황시각", "-");
            }
            this.dgvItemDetail.Rows.Add("특보발효현황내용", profile.PresentConditionContents.ToString());
            this.dgvItemDetail.Rows.Add("예비특보발효현황", profile.PreliminaryConditionContents);
            this.dgvItemDetail.Rows.Add("참고사항", profile.Other);
        }
        private void UpdateHeadline()
        {
            SWRWarningItemProfile profile = this.currentProfile.GetWarningItemProfile();
            if (profile == null)
            {
                MessageBox.Show("데이터 변환 중 오류가 발생하였습니다.", "기상특보상세", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<string> elements = new List<string>();

            elements.Add(profile.AnnounceTime.ToString());
            elements.Add(this.currentProfile.GetTargetAreaNames());
            string swrKind = BasisData.FindSWRKindStringByKindCode(this.currentProfile.WarnKindCode);
            if (string.IsNullOrEmpty(swrKind))
            {
                elements.Add("Unknown(" + this.currentProfile.WarnKindCode + ")");
            }
            else
            {
                elements.Add(" " + swrKind);
            }
            string swrStress = BasisData.FindSWRStressStringByStressCode(this.currentProfile.WarnStressCode);
            if (string.IsNullOrEmpty(swrStress))
            {
                elements.Add("Unknown(" + this.currentProfile.WarnStressCode + ")");
            }
            else
            {
                elements.Add(" " + swrStress);
            }
            string swrCommand = BasisData.FindSWRCommandStringByCommandCode(this.currentProfile.CommandCode);
            if (string.IsNullOrEmpty(swrCommand))
            {
                elements.Add("Unknown(" + this.currentProfile.CommandCode + ")");
            }
            else
            {
                elements.Add(" " + swrCommand);
            }

            this.lblReportHeadline.Text = string.Join(" ", elements);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
