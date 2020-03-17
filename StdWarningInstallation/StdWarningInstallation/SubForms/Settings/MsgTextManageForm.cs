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
    public partial class MsgTextManageForm : Form
    {
        public MsgTextManageForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            bool isLoaded = true;
            isLoaded = InitializeDisasterCmbbox();
            if (!isLoaded)
            {
                MessageBox.Show("재난 종류 정보를 설정하는 중에 문제가 발생하였습니다. \n데이터를 확인 후 문안 관리 기능을 사용하십시오.", "문안 관리 데이터 오류",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            isLoaded = InitializeLanguageKindListBox();
            if (!isLoaded)
            {
                MessageBox.Show("언어 종류 정보를 설정하는 중에 문제가 발생하였습니다. \n데이터를 확인 후 문안 관리 기능을 사용하십시오.", "문안 관리 데이터 오류",
                                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            isLoaded = InitializeCityTypeListBox();

            UpdateTextData();
        }


        #region 컨트롤초기설정
        /// <summary>
        /// 재난 카테고리/종류 콤보박스 설정.
        /// </summary>
        private bool InitializeDisasterCmbbox()
        {
            this.cmbboxDisasterCategory.Items.Clear();
            this.lstboxDisasterKind.Items.Clear();

            this.cmbboxDisasterCategory.Enabled = false;
            this.lstboxDisasterKind.Enabled = false;

            if (BasisData.Disasters == null || BasisData.Disasters.Count <= 0)
            {
                this.cmbboxDisasterCategory.Enabled = false;
                this.lstboxDisasterKind.Enabled = false;

                return false;
            }

            this.cmbboxDisasterCategory.Enabled = true;

            DisasterInfo initDisaster = null;
            foreach (DisasterInfo info in BasisData.Disasters)
            {
                if (info == null || info.Category == null)
                {
                    continue;
                }
                DisasterInfo copy = new DisasterInfo();
                copy.DeepCopyFrom(info);

                this.cmbboxDisasterCategory.Items.Add(copy);

                if (info.Category.Name == "기상")
                {
                    initDisaster = copy;
                }
            }
            if (initDisaster == null || initDisaster.KindList == null)
            {
                return true;
            }
            this.cmbboxDisasterCategory.SelectedText = "기상";
            this.lstboxDisasterKind.Enabled = true;

            foreach (DisasterKind kindInfo in initDisaster.KindList)
            {
                this.lstboxDisasterKind.Items.Add(kindInfo);
            }

            return true;
        }
        /// <summary>
        /// [언어 종류] 초기 설정.
        /// </summary>
        /// <returns></returns>
        private bool InitializeLanguageKindListBox()
        {
            this.lstboxLanguageKind.Items.Clear();
            if (BasisData.MsgTextLanguageKind == null || BasisData.MsgTextLanguageKind.Count < 1)
            {
                return false;
            }

            foreach (MsgTextDisplayLanguageKind language in BasisData.MsgTextLanguageKind)
            {
                MsgTextDisplayLanguageKind copy = new MsgTextDisplayLanguageKind();
                copy.DeepCopyFrom(language);

                this.lstboxLanguageKind.Items.Add(copy);
            }

            return true;
        }
        /// <summary>
        /// [도시 유형 종류] 초기 설정.
        /// </summary>
        /// <returns></returns>
        private bool InitializeCityTypeListBox()
        {
            this.lstboxCityType.Items.Clear();
            if (BasisData.MsgTextCityType == null || BasisData.MsgTextCityType.Count < 1)
            {
                return false;
            }

            foreach (MsgTextCityType city in BasisData.MsgTextCityType)
            {
                MsgTextCityType copy = new MsgTextCityType();
                copy.DeepCopyFrom(city);

                this.lstboxCityType.Items.Add(copy);
            }

            return true;
        }
        #endregion

        #region 데이터 표시 갱신
        private void UpdateTextData()
        {
            List<MsgText> msgInfo = GetMsgTextByCurrentConditions();
            if (msgInfo == null)
            {
                return;
            }
            foreach (MsgText txt in msgInfo)
            {
                MsgTextDisplayMediaType mediaType = BasisData.FindMediaTypeInfoByID(txt.MediaTypeID);
                if (mediaType == null)
                {
                    continue;
                }
                string mediaTypeCode = mediaType.Code.ToUpper();
                if (mediaTypeCode == "TTS")
                {
                    this.txtboxTTS.Text = txt.Text;
                }
                else if (mediaTypeCode == "CBS")
                {
                    this.txtboxCBS.Text = txt.Text;
                }
                else if (mediaTypeCode == "BOARD")
                {
                    this.txtboxBoard.Text = txt.Text;
                }
                else if (mediaTypeCode == "DMB")
                {
                    this.txtboxDMB.Text = txt.Text;
                }
                else
                {
                }
            }
        }
        private List<MsgText> GetMsgTextByCurrentConditions()
        {
            if (this.lstboxDisasterKind.SelectedIndex < 0)
            {
                return null;
            }
            if (this.lstboxLanguageKind.SelectedIndex < 0)
            {
                return null;
            }
            if (this.lstboxCityType.SelectedIndex < 0)
            {
                return null;
            }

            DisasterKind kindInfo = lstboxDisasterKind.SelectedItem as DisasterKind;
            MsgTextDisplayLanguageKind selectedLangKind = this.lstboxLanguageKind.SelectedItem as MsgTextDisplayLanguageKind;
            MsgTextCityType selectedCityType = this.lstboxCityType.SelectedItem as MsgTextCityType;
            if (kindInfo == null || selectedLangKind == null || selectedCityType == null)
            {
                return null;
            }

            List<MsgText> msgInfo = BasisData.FindMsgTextInfoByDisasterCode(kindInfo.Code);
            if (msgInfo == null)
            {
                return null;
            }

            List<MsgText> result = null;
            foreach (MsgText txt in msgInfo)
            {
                if (txt.LanguageKindID != selectedLangKind.ID)
                {
                    continue;
                }
                if (txt.CityTypeID != selectedCityType.ID)
                {
                    continue;
                }

                if (result == null)
                {
                    result = new List<MsgText>();
                }
                MsgText copy = new MsgText();
                copy.DeepCopyFrom(txt);

                result.Add(copy);
            }

            return result;
        }
        #endregion

        private void cmbboxDisasterCategory_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cmb = sender as ComboBox;

            // 타이밍?

        }
        private void cmbboxDisasterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lstboxDisasterKind.Items.Clear();
            this.txtboxBoard.Clear();
            this.txtboxCBS.Clear();
            this.txtboxTTS.Clear();
            this.txtboxDMB.Clear();

            this.lstboxDisasterKind.SelectedIndex = -1;
            this.lstboxLanguageKind.SelectedIndex = -1;
            this.lstboxCityType.SelectedIndex = -1;

            ComboBox cmb = sender as ComboBox;
            if (cmb.SelectedIndex < 0)
            {
                return;
            }

            DisasterInfo info = cmb.SelectedItem as DisasterInfo;
            if (info == null || info.Category == null || info.KindList == null)
            {
                return;
            }

            foreach (DisasterKind kind in info.KindList)
            {
                this.lstboxDisasterKind.Items.Add(kind);
            }
        }

        private void lstboxDisasterKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.lstboxDisasterKind.Focused)
            {
                return;
            }

            this.txtboxBoard.Clear();
            this.txtboxCBS.Clear();
            this.txtboxTTS.Clear();
            this.txtboxDMB.Clear();

            this.lstboxLanguageKind.SelectedIndex = 0;
            this.lstboxCityType.SelectedIndex = 0;

            UpdateTextData();
        }

        private void lstboxLanguageKind_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.lstboxLanguageKind.Focused)
            {
                return;
            }
            this.txtboxBoard.Clear();
            this.txtboxCBS.Clear();
            this.txtboxTTS.Clear();
            this.txtboxDMB.Clear();

            this.lstboxCityType.SelectedIndex = 0;

            UpdateTextData();

        }

        private void lstboxCityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this.lstboxCityType.Focused)
            {
                return;
            }

            UpdateTextData();
        }

        private void txtboxCBS_TextChanged(object sender, EventArgs e)
        {
            string limit = "(" + this.txtboxCBS.TextLength + "/" + this.txtboxCBS.MaxLength + ")";
            this.lblLimitLetterOfCBS.Text = limit;
        }
        private void txtboxBoard_TextChanged(object sender, EventArgs e)
        {
            string limit = "(" + this.txtboxBoard.TextLength + "/" + this.txtboxBoard.MaxLength + ")";
            this.lblLimitLetterOfBoard.Text = limit;
        }
        private void txtboxTTS_TextChanged(object sender, EventArgs e)
        {
            string limit = "(" + this.txtboxTTS.TextLength + "/" + this.txtboxTTS.MaxLength + ")";
            this.lblLimitLetterOfTTS.Text = limit;
        }
        private void txtboxDMB_TextChanged(object sender, EventArgs e)
        {
            string limit = "(" + this.txtboxDMB.TextLength + "/" + this.txtboxDMB.MaxLength + ")";
            this.lblLimitLetterOfDMB.Text = limit;
        }
        /// <summary>
        /// [문안 복원] 버튼 클릭 이벤트 핸들러.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRestoreOne_Click(object sender, EventArgs e)
        {
            List<MsgText> currentMsg = GetMsgTextByCurrentConditions();
            if (currentMsg == null)
            {
                return;
            }
            if (currentMsg.Count < 1)
            {
                MessageBox.Show("문안 정보 복원 중 오류가 발생하여 요청을 수행할 수 없습니다. ErrorCode=[0]", "문안 복원 실패", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (BasisData.BasicMsgTextInfo == null)
            {
                MessageBox.Show("문안 정보 복원 중 오류가 발생하여 요청을 수행할 수 없습니다. ErrorCode=[1]", "문안 복원 실패", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            foreach (MsgText msg in currentMsg)
            {
                if (BasisData.BasicMsgTextInfo.ContainsKey(msg.ID))
                {
                    DisasterMsgText basic = BasisData.BasicMsgTextInfo[msg.ID];
                    msg.DeepCopyFrom(basic.MsgTxt);
                }
            }
            int result = DBManager.GetInstance().UpdateTransmitMsgText(currentMsg);
            if (result == 0)
            {
                foreach (MsgText newTxt in currentMsg)
                {
                    if (BasisData.TransmitMsgTextInfo.ContainsKey(newTxt.ID))
                    {
                        MsgText trans = BasisData.TransmitMsgTextInfo[newTxt.ID].MsgTxt;
                        trans.DeepCopyFrom(newTxt);
                    }
                }

                UpdateTextData();
                MessageBox.Show("문안 정보를 복원하였습니다.", "문안 복원", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("문안 정보 복원이 실패하였습니다.. ErrorCode=[" + result + "]", "문안 복원 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// [변경 저장] 버튼 클릭 이벤트 핸들러.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            List<MsgText> currentMsg = GetMsgTextByCurrentConditions();
            if (currentMsg == null)
            {
                return;
            }
            if (currentMsg.Count < 1)
            {
                MessageBox.Show("문안 정보 저장 중 오류가 발생하여 요청을 수행할 수 없습니다. ErrorCode=[01]", "문안 저장", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<MsgText> saveTarget = new List<MsgText>();
            foreach (MsgText txt in currentMsg)
            {
                bool found = false;
                MsgTextDisplayMediaType mediaType = BasisData.FindMediaTypeInfoByID(txt.MediaTypeID);
                if (mediaType == null)
                {
                    continue;
                }
                string mediaTypeCode = mediaType.Code.ToUpper();
                if (mediaTypeCode == "TTS")
                {
                    if (txt.Text == this.txtboxTTS.Text)
                    {
                        continue;
                    }
                    found = true;
                    txt.Text = this.txtboxTTS.Text;
                }
                else if (mediaTypeCode == "CBS")
                {
                    if (txt.Text == this.txtboxCBS.Text)
                    {
                        continue;
                    }
                    found = true;
                    txt.Text = this.txtboxCBS.Text;
                }
                else if (mediaTypeCode == "BOARD")
                {
                    if (txt.Text == this.txtboxBoard.Text)
                    {
                        continue;
                    }
                    found = true;
                    txt.Text = this.txtboxBoard.Text;
                }
                else if (mediaTypeCode == "DMB")
                {
                    if (txt.Text == this.txtboxDMB.Text)
                    {
                        continue;
                    }
                    found = true;
                    txt.Text = this.txtboxDMB.Text;
                }
                else
                {
                }

                if (found)
                {
                    MsgText copy = new MsgText();
                    copy.DeepCopyFrom(txt);

                    saveTarget.Add(copy);
                }
            }

            int result = DBManager.GetInstance().UpdateTransmitMsgText(saveTarget);
            if (result == 0)
            {
                foreach (MsgText newTxt in saveTarget)
                {
                    if (BasisData.TransmitMsgTextInfo.ContainsKey(newTxt.ID))
                    {
                        MsgText trans = BasisData.TransmitMsgTextInfo[newTxt.ID].MsgTxt;
                        trans.DeepCopyFrom(newTxt);
                    }
                }

                MessageBox.Show("문안 정보를 저장하였습니다.", "문안 저장", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("문안 정보 저장 중 오류가 발생하여 요청을 수행할 수 없습니다. ErrorCode=[" + result + "]", "문안 저장", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// [전체 복원] 버튼 클릭 이벤트 핸들러.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRestoreAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (BasisData.BasicMsgTextInfo == null || BasisData.BasicMsgTextInfo.Values == null)
                {
                    MessageBox.Show("기본 문안 정보가 존재하지 않습니다. 요청을 진행할 수 없습니다.",
                                                            "전체 문안 복원 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DialogResult answer = MessageBox.Show("등록된 모든 문안의 설정이 시스템 초기 상태로 복원되며, 시스템에 따라 수 분 정도 소요됩니다. \n초기화 하시겠습니까?",
                                                        "전체 문안 복원", MessageBoxButtons.YesNo);
                if (answer != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                List<MsgText> basicMsgs = new List<MsgText>();
                foreach (DisasterMsgText basicMsg in BasisData.BasicMsgTextInfo.Values)
                {
                    basicMsgs.Add(basicMsg.MsgTxt);
                }
                int result = DBManager.GetInstance().UpdateTransmitMsgText(basicMsgs);
                if (result == 0)
                {
                    BasisData.TransmitMsgTextInfo = DBManager.GetInstance().QueryTransmitMsgTextInfo(null);

                    UpdateTextData();

                    MessageBox.Show("모든 문안 정보를 시스템 초기 상태로 복원하였습니다.", "문안 복원", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("문안 복원 중에 오류가 발생하여 복원에 실패하였습니다. ErrorCode=[" + result + "]",
                                                            "전체 문안 복원 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                FileLogManager.GetInstance().WriteLog("[MsgTextManaerForm] 전체 문안 초기화 오류( Exception=[" + ex.ToString() + "] )");
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
