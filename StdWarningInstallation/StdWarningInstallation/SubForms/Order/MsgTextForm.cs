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
    public partial class MsgTextForm : Form
    {
        // Original: 발령창이 가지고 있는 정보로, [문안] 창을 열 때의 원본 데이터
        // Current:  Original의 복사데이터로 [적용] 버튼을 눌러질 떄마다 저장된 데이터
        // Temporary: Original의 복사데이터로 문안 창에서 사용자가 설정을 변경할 때마다 실시간으로 저장되는 데이터
        // [닫기]: Current -----> Original
        // [적용]: Temporary ---> Current
        // [초기화]: Original --> Temporary/Current
        private SendingMsgTextInfo originalSettingInfo = null;
        private SendingMsgTextInfo currentSettingInfo = new SendingMsgTextInfo();
        private SendingMsgTextInfo temporarySettingInfo = new SendingMsgTextInfo();

        MsgTextDisplayLanguageKind previousLanguageSelection = null;
        MsgModifyHistory modifyHistory = new MsgModifyHistory();

        private readonly string DEFAULT_HEADER_TIME = "○○일 ○○시";
        private readonly string DEFAULT_HEADER_REGION = "○○지역";

        public MsgTextForm(SendingMsgTextInfo info)
        {
            InitializeComponent();

            this.originalSettingInfo = info;
            this.currentSettingInfo.DeepCopyFrom(info);
            this.temporarySettingInfo.DeepCopyFrom(info);
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            // 동적으로 할당된 언어 종류 체크박스 이벤트핸들러 삭제
            if (BasisData.MsgTextLanguageKind != null)
            {
                foreach (MsgTextDisplayLanguageKind selectedLanguage in BasisData.MsgTextLanguageKind)
                {
                    if (!this.pnlLanguageSetting.Controls.ContainsKey(selectedLanguage.LanguageCode))
                    {
                        continue;
                    }
                    CheckBox control = this.pnlLanguageSetting.Controls[selectedLanguage.LanguageCode] as CheckBox;
                    if (control == null)
                    {
                        // 컨트롤 타입이 맞지 않음
                        continue;
                    }
                    control.CheckedChanged -= new System.EventHandler(this.chkboxLanguage_CheckedChanged);
                }
            }

            // 동적으로 할당된 도시 형태 라디오버튼 이벤트핸들러 삭제
            if (BasisData.MsgTextCityType != null)
            {
                foreach (MsgTextCityType selectedCityType in BasisData.MsgTextCityType)
                {
                    if (!this.pnlCityTypeSetting.Controls.ContainsKey(selectedCityType.TypeCode))
                    {
                        continue;
                    }
                    CheckBox control = this.pnlCityTypeSetting.Controls[selectedCityType.TypeCode] as CheckBox;
                    if (control == null)
                    {
                        // 컨트롤 타입이 맞지 않음
                        continue;
                    }
                    control.CheckedChanged -= new System.EventHandler(this.radioBtnCityType_CheckedChanged);
                }
            }

            base.OnClosing(e);
        }

        private void LoadForm(object sender, EventArgs e)
        {
            // 데이터 체크 및 초기설정
            InitializeData();

            // 언어 선택 체크박스 설정
            InitializeLanguageCheckBoxes();

            // 도시 유형 라디오버튼 설정
            InitializeCityTypeRadioButtons();

            // 문안 표시 용 언어 선택 콤보박스리스트 선택 상태
            if (this.cmbboxLanguage.Items != null && this.cmbboxLanguage.Items.Count > 0)
            {
                this.cmbboxLanguage.SelectedIndex = 0;
            }

            // 문안 텍스트박스 설정
            InitializeMsgTextBoxes();

        }


        #region 초기 설정
        private void InitializeData()
        {
            // 체크
            if (this.originalSettingInfo == null || this.currentSettingInfo == null || this.temporarySettingInfo == null)
            {
                MessageBox.Show("문안 정보에 오류가 있어 실행할 수 없습니다.\n 처리를 종료합니다.", "문안 변경 및 확인 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Dispose();
                return;
            }

            // 언어
            if (this.temporarySettingInfo.SelectedLanguages == null)
            {
                this.temporarySettingInfo.SelectedLanguages = new List<MsgTextDisplayLanguageKind>();
            }
            if (this.temporarySettingInfo.SelectedLanguages.Count < 1)
            {
                // 기본 언어 [한국어] 추가
                MsgTextDisplayLanguageKind defaultLanguage = BasisData.FindMsgTextLanguageInfoByCode(BasisData.DEFAULT_LANGUAGECODE);
                if (defaultLanguage == null)
                {
                    defaultLanguage = new MsgTextDisplayLanguageKind();
                    defaultLanguage.ID = 1;
                    defaultLanguage.LanguageCode = BasisData.DEFAULT_LANGUAGECODE;
                    defaultLanguage.LanguageName = "한국어";
                }
                this.temporarySettingInfo.SelectedLanguages.Add(defaultLanguage);
            }

            // 도시 형태
            if (this.temporarySettingInfo.SelectedCityType == null)
            {
                this.temporarySettingInfo.SelectedCityType = new MsgTextCityType();
            }
            if (string.IsNullOrEmpty(this.temporarySettingInfo.SelectedCityType.TypeCode))
            {
                // 일반(기본)을 선택상태로 초기화
                MsgTextCityType defaultType = BasisData.FindCityTypeInfoByCode(BasisData.DEFAULT_CITYTYPECODE);
                if (defaultType == null)
                {
                    defaultType = new MsgTextCityType();
                    defaultType.ID = 0;
                    defaultType.TypeCode = BasisData.DEFAULT_CITYTYPECODE;
                    defaultType.TypeName = "일반(기본)";
                }
                this.temporarySettingInfo.SelectedCityType = defaultType;
            }

            // 모든 조건에 대응하는 문안 영역을 확보
            foreach (MsgTextDisplayLanguageKind language in BasisData.MsgTextLanguageKind)
            {
                foreach (MsgTextCityType cityType in BasisData.MsgTextCityType)
                {
                    foreach (MsgTextDisplayMediaType mediaType in BasisData.MsgTextDisplayMediaType)
                    {
                        MsgText txt = SearchMsgTextFromTargetSettingInfo(this.temporarySettingInfo, language.LanguageCode, cityType.ID, (int)mediaType.ID);
                        if (txt == null)
                        {
                            txt = new MsgText();
                            txt.LanguageKindID = language.ID;
                            txt.CityTypeID = cityType.ID;
                            txt.MediaTypeID = (int)mediaType.ID;
                            txt.Text = string.Empty;

                            this.temporarySettingInfo.CurrentTransmitMsgText.Add(txt);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// [언어 설정] 언어 체크 박스 컨트롤 작성 및 상태 초기화
        /// </summary>
        private void InitializeLanguageCheckBoxes()
        {
            // 기본 언어 [한국어] 추가
            MsgTextDisplayLanguageKind defaultLanguage = BasisData.FindMsgTextLanguageInfoByCode(BasisData.DEFAULT_LANGUAGECODE);
            if (defaultLanguage == null)
            {
                defaultLanguage = new MsgTextDisplayLanguageKind();
                defaultLanguage.ID = 1;
                defaultLanguage.LanguageCode = BasisData.DEFAULT_LANGUAGECODE;
                defaultLanguage.LanguageName = "한국어";
            }
            CheckBox defaultLanguageChkbox = CreateLanguageCheckBox(0, defaultLanguage);
            defaultLanguageChkbox.Enabled = false;

            // 기본 언어 이외의 언어 정보 추가
            int controlIndex = 1;
            foreach (MsgTextDisplayLanguageKind language in BasisData.MsgTextLanguageKind)
            {
                if (language.LanguageCode == BasisData.DEFAULT_LANGUAGECODE)
                {
                    continue;
                }

                MsgTextDisplayLanguageKind copy = new MsgTextDisplayLanguageKind();
                copy.DeepCopyFrom(language);

                CheckBox control = CreateLanguageCheckBox(controlIndex, copy);
                controlIndex++;
            }

            // 현재 선택 언어에 체크 설정
            UpdateLanguageSelection();
        }
        private CheckBox CreateLanguageCheckBox(int controlIndex, MsgTextDisplayLanguageKind languageKindInfo)
        {
            // 컨트롤 위치
            Point location = new Point(17, 30);

            CheckBox chkbox = new CheckBox();
            this.pnlLanguageSetting.Controls.Add(chkbox);

            chkbox.AutoSize = true;
            chkbox.Checked = false;
            chkbox.CheckState = System.Windows.Forms.CheckState.Unchecked;
            chkbox.Enabled = true;
            chkbox.Location = new System.Drawing.Point(location.X + (100 * controlIndex), location.Y);
            chkbox.Name = languageKindInfo.LanguageCode;
            chkbox.Size = new System.Drawing.Size(100, 16);
            chkbox.TabIndex = controlIndex;
            chkbox.Text = languageKindInfo.LanguageName;
            chkbox.UseVisualStyleBackColor = true;
            chkbox.CheckedChanged += new System.EventHandler(this.chkboxLanguage_CheckedChanged);

            chkbox.Tag = languageKindInfo;

            return chkbox;
        }

        /// <summary>
        /// [도시 유형별 문안 설정] 
        /// </summary>
        private void InitializeCityTypeRadioButtons()
        {
            // 기본 도시 유형 [일반(기본)] 추가
            MsgTextCityType defaultCityType = BasisData.FindCityTypeInfoByCode(BasisData.DEFAULT_CITYTYPECODE);
            if (defaultCityType == null)
            {
                defaultCityType = new MsgTextCityType();
                defaultCityType.ID = 1;
                defaultCityType.TypeCode = BasisData.DEFAULT_CITYTYPECODE;
                defaultCityType.TypeName = "일반(기본)";
            }
            RadioButton defaultControl = CreateCityTypeRadioButton(0, defaultCityType);
            if (defaultControl == null)
            {
                System.Console.WriteLine("[MsgTextForm] 도시 유형 컨트롤 생성 오류");
                return;
            }

            // 그 외의 도시 유형 추가
            int controlIndex = 1;
            foreach (MsgTextCityType cityType in BasisData.MsgTextCityType)
            {
                if (cityType.TypeCode.ToUpper() == BasisData.DEFAULT_CITYTYPECODE.ToUpper())
                {
                    continue;
                }
                MsgTextCityType copy = new MsgTextCityType();
                copy.DeepCopyFrom(cityType);
                RadioButton newRadioBtn = CreateCityTypeRadioButton(controlIndex, copy);
                controlIndex++;
            }

            // 현재 설정에 맞게 도시유형 선택 상태 갱신
            UpdateCityTypeSelection();
        }
        private RadioButton CreateCityTypeRadioButton(int controlIndex, MsgTextCityType cityTypeInfo)
        {
            // 컨트롤 위치
            Point location = new Point(17, 30);

            RadioButton newRadioBtn = new RadioButton();
            this.pnlCityTypeSetting.Controls.Add(newRadioBtn);

            newRadioBtn.Checked = true;
            newRadioBtn.Location = new System.Drawing.Point(location.X + (100 * controlIndex), location.Y);
            newRadioBtn.Name = cityTypeInfo.TypeCode;
            newRadioBtn.Size = new System.Drawing.Size(100, 24);
            newRadioBtn.TabIndex = 1;
            newRadioBtn.TabStop = true;
            newRadioBtn.Text = cityTypeInfo.TypeName;
            newRadioBtn.UseVisualStyleBackColor = true;
            newRadioBtn.CheckedChanged += new System.EventHandler(this.radioBtnCityType_CheckedChanged);

            newRadioBtn.Tag = cityTypeInfo;

            return newRadioBtn;
        }

        /// <summary>
        /// [문안 설정] 문안 텍스트 박스 컨트롤 작성 및 상태 초기화
        /// </summary>
        private void InitializeMsgTextBoxes()
        {
            // 문안 표시 텍스트 박스에 문안표출형태 오브젝트 정보를 바인딩
            if (BasisData.MsgTextDisplayMediaType != null && BasisData.MsgTextDisplayMediaType.Count > 0)
            {
                foreach (MsgTextDisplayMediaType mediaInfo in BasisData.MsgTextDisplayMediaType)
                {
                    MsgTextDisplayMediaType copy = new MsgTextDisplayMediaType();
                    copy.DeepCopyFrom(mediaInfo);

                    if (mediaInfo.Code.ToUpper().Contains("TTS"))
                    {
                        this.txtboxTTSMsg.Tag = copy;
                        this.txtboxTTSMsg.Name = mediaInfo.Code;
                    }
                    else if (mediaInfo.Code.ToUpper().Contains("CBS"))
                    {
                        this.txtboxCBSMsg.Tag = copy;
                        this.txtboxCBSMsg.Name = mediaInfo.Code;
                    }
                    else if (mediaInfo.Code.ToUpper().Contains("BOARD"))
                    {
                        this.txtboxBoardMsg.Tag = copy;
                        this.txtboxBoardMsg.Name = mediaInfo.Code;
                    }
                    else if (mediaInfo.Code.ToUpper().Contains("DMB"))
                    {
                        this.txtboxDMBMsg.Tag = copy;
                        this.txtboxDMBMsg.Name = mediaInfo.Code;
                    }
                    else
                    {
                    }
                }
            }

            // 문안 표시 갱신
            UpdateMessageText();
        }
        #endregion


        #region 갱신
        /// <summary>
        /// [언어 설정] 선택 상태(체크 표시) 갱신.
        /// </summary>
        private void UpdateLanguageSelection()
        {
            // 언어 설정의 선택 상태 초기화
            foreach (MsgTextDisplayLanguageKind languageKind in BasisData.MsgTextLanguageKind)
            {
                CheckBox control = this.pnlLanguageSetting.Controls[languageKind.LanguageCode] as CheckBox;
                if (control != null)
                {
                    control.Checked = false;
                }
            }
            // 문안 표시부의 언어 콤보박스 선택 초기화
            this.cmbboxLanguage.ResetText();
            if (this.cmbboxLanguage.Items != null)
            {
                this.cmbboxLanguage.Items.Clear();
            }

            // 최신 정보로 설정 갱신
            bool isChecked = false;
            foreach (MsgTextDisplayLanguageKind selectedLanguage in this.temporarySettingInfo.SelectedLanguages)
            {
                CheckBox control = this.pnlLanguageSetting.Controls[selectedLanguage.LanguageCode] as CheckBox;
                if (control == null)
                {
                    // 컨트롤 타입이 맞지 않음
                    continue;
                }

                // 선택된 언어에 체크
                control.Checked = true;
                isChecked = true;

                // 문안 표시 용 콤보박스리스트에 추가
                this.cmbboxLanguage.Items.Add(control.Tag as MsgTextDisplayLanguageKind);
            }
            if (!isChecked)
            {
                // 디폴트에 체크
                CheckBox control = this.pnlLanguageSetting.Controls[BasisData.DEFAULT_LANGUAGECODE] as CheckBox;
                if (control != null)
                {
                    control.Checked = true;
                }
            }

            // 문안 표시 용 언어 선택 콤보박스리스트 선택 상태
            if (this.cmbboxLanguage.Items != null && this.cmbboxLanguage.Items.Count > 0)
            {
                this.cmbboxLanguage.SelectedIndex = 0;
                this.previousLanguageSelection = this.cmbboxLanguage.SelectedItem as MsgTextDisplayLanguageKind;
            }
        }
        /// <summary>
        /// [도시 유형별 문안 설정] 선택 상태(체크 표시) 갱신.
        /// </summary>
        private void UpdateCityTypeSelection()
        {
            RadioButton defaultControl = this.pnlCityTypeSetting.Controls[BasisData.DEFAULT_CITYTYPECODE] as RadioButton;

            // 현재 설정에 맞게 도시유형 선택 상태 갱신
            if (this.temporarySettingInfo == null || this.temporarySettingInfo.SelectedCityType == null)
            {
                if (defaultControl != null)
                {
                    defaultControl.Checked = true;
                }
                return;
            }
            if (!this.pnlCityTypeSetting.Controls.ContainsKey(this.temporarySettingInfo.SelectedCityType.TypeCode))
            {
                if (defaultControl != null)
                {
                    defaultControl.Checked = true;
                }
                return;
            }
            RadioButton control = this.pnlCityTypeSetting.Controls[this.temporarySettingInfo.SelectedCityType.TypeCode] as RadioButton;
            if (control == null)
            {
                System.Console.WriteLine("[MsgTextForm] 도시 유형 컨트롤 생성 - 타입 캐스팅 오류");

                if (defaultControl != null)
                {
                    defaultControl.Checked = true;
                }
                return;
            }
            control.Checked = true;
        }
        /// <summary>
        /// 각 텍스트박스의 문안을 갱신.
        /// </summary>
        private void UpdateMessageText()
        {
            // 1.초기화
            this.txtboxTTSMsg.ResetText();
            this.txtboxCBSMsg.ResetText();
            this.txtboxBoardMsg.ResetText();
            this.txtboxDMBMsg.ResetText();

            // 2.2 언어 선택 상태
            MsgTextDisplayLanguageKind selectedLanguage = this.cmbboxLanguage.SelectedItem as MsgTextDisplayLanguageKind;
            if (selectedLanguage == null)
            {
                System.Console.WriteLine("[MsgTextForm] UpdateMessageText( 언어 종류 선택 상태를 얻을 수 없음 )");
                return;
            }

            // 3. 텍스트 설정
            foreach (MsgText msgTextInfo in this.temporarySettingInfo.CurrentTransmitMsgText)
            {
                // 언어
                if (selectedLanguage.ID != msgTextInfo.LanguageKindID)
                {
                    continue;
                }
                // 도시 형태
                if (msgTextInfo.CityTypeID != this.temporarySettingInfo.SelectedCityType.ID)
                {
                    continue;
                }
                // 표출매체 유형
                MsgTextDisplayMediaType mediaType = BasisData.FindMediaTypeInfoByID(msgTextInfo.MediaTypeID);
                if (mediaType == null)
                {
                    continue;
                }

                if (pnlMsgTextSetting.Controls.ContainsKey(mediaType.Code))
                {
                    TextBox control = pnlMsgTextSetting.Controls[mediaType.Code] as TextBox;
                    if (control == null)
                    {
                        continue;
                    }
                    control.Text = msgTextInfo.Text;
                }
            }
        }
        #endregion


        #region 언어지원
        /// <summary>
        /// [다국어] 언어 선택 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkboxLanguage_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox ctrl = sender as CheckBox;
            if (ctrl == null)
            {
                return;
            }
            if (!ctrl.Focused)
            {
                // 소프트웨어적인 처리인 경우에는, 데이터를 추가할 필요 없음.
                return;
            }

            MsgTextDisplayLanguageKind currentLanguage = ctrl.Tag as MsgTextDisplayLanguageKind;
            if (currentLanguage == null)
            {
                return;
            }

            if (ctrl.Checked)
            {
                // 로컬 설정 정보에 추가
                if (this.temporarySettingInfo.SelectedLanguages == null)
                {
                    this.temporarySettingInfo.SelectedLanguages = new List<MsgTextDisplayLanguageKind>();
                }
                this.temporarySettingInfo.SelectedLanguages.Add(currentLanguage);

                // 문안 표시 용 언어 선택 콤보박스리스트에 언어 추가
                this.cmbboxLanguage.Items.Add(currentLanguage);
            }
            else
            {
                // 현재 체크 해제된 아이템
                MsgTextDisplayLanguageKind uncheckedLanguage = ctrl.Tag as MsgTextDisplayLanguageKind;
                if (uncheckedLanguage == null)
                {
                    return;
                }

                // 콤보박스리스트의 현재 상태 백업
                MsgTextDisplayLanguageKind currentCmbBoxItem = this.cmbboxLanguage.SelectedItem as MsgTextDisplayLanguageKind;
                if (currentCmbBoxItem.LanguageCode == uncheckedLanguage.LanguageCode)
                {
                    // 현재 표시 중인 언어를 삭제하는 경우

                    // 현재 선택한 언어 정보를 임시설정데이터에서 삭제
                     for (int index = 0; index < this.temporarySettingInfo.SelectedLanguages.Count; index++)
                    {
                        if (this.temporarySettingInfo.SelectedLanguages[index].ID == uncheckedLanguage.ID)
                        {
                            this.temporarySettingInfo.SelectedLanguages.RemoveAt(index);
                        }
                    }
                    // 도시 유형별 설정을 초기화
                    this.temporarySettingInfo.SelectedCityType = BasisData.FindCityTypeInfoByCode(BasisData.DEFAULT_CITYTYPECODE);
                    // 콤보박스리스트에서 해당언어를 삭제하고 선택 상태를 초기화
                    this.cmbboxLanguage.Items.Remove(uncheckedLanguage);
                    this.cmbboxLanguage.ResetText();
                    this.cmbboxLanguage.SelectedIndex = 0;

                    // 수동변경 후에는 머리글 취소 불가
                    this.btnRevert.Enabled = false;
                    this.modifyHistory.Clear();

                    // 전체 표시 갱신
                    UpdateLanguageSelection();
                    UpdateCityTypeSelection();
                    UpdateMessageText();
                }
                else
                {
                    // 현재 표시 중이 아닌 언어를 삭제하는 경우
                    string targetLanguageCode = uncheckedLanguage.LanguageCode;
                    int selectedCityTypeID = this.temporarySettingInfo.SelectedCityType.ID;

                    // 현재 선택 해제한 언어 정보를 임시설정데이터에서 삭제
                    for (int index = 0; index < this.temporarySettingInfo.SelectedLanguages.Count; index++)
                    {
                        if (this.temporarySettingInfo.SelectedLanguages[index].ID == uncheckedLanguage.ID)
                        {
                            this.temporarySettingInfo.SelectedLanguages.RemoveAt(index);
                        }
                    }

                    // 현재 선택 해제한 문안 정보를 임시설정데이터에서 삭제하고 CurrentSettingInfo로 복원
                    foreach (MsgTextDisplayMediaType mediaType in BasisData.MsgTextDisplayMediaType)
                    {
                        MsgText src = SearchMsgTextFromTargetSettingInfo(this.currentSettingInfo, targetLanguageCode, selectedCityTypeID, (int)mediaType.ID);
                        MsgText dst = SearchMsgTextFromTargetSettingInfo(this.temporarySettingInfo, targetLanguageCode, selectedCityTypeID, (int)mediaType.ID);

                        if (dst == null)
                        {
                            dst = new MsgText();
                        }
                        if (src == null)
                        {
                            dst.ID = string.Empty;
                            dst.LanguageKindID = -1;
                            dst.CityTypeID = -1;
                            dst.MediaTypeID = -1;
                            dst.Text = string.Empty;
                        }
                        else
                        {
                            dst.DeepCopyFrom(src);
                        }
                    }

                    // 콤보박스리스트에서 해당언어를 삭제하고 인덱스가 변경되었을 수 있으므로 재선택 설정
                    this.cmbboxLanguage.Items.Remove(uncheckedLanguage);
                    this.cmbboxLanguage.SelectedItem = currentCmbBoxItem;

                    // 표시 갱신
                    UpdateLanguageSelection();
                    UpdateCityTypeSelection();
                }
            }
        }
        #endregion

        #region 도시유형별문안지원
        /// <summary>
        /// [도시 유형별 문안 선택] 라디오버튼 클릭 이벤트.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioBtnCityType_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton btn = sender as RadioButton;
            if (btn == null)
            {
                return;
            }
            if (!btn.Focused)
            {
                return;
            }
            // 수동변경 후에는 머리글 취소 불가
            this.btnRevert.Enabled = false;
            this.modifyHistory.Clear();

            MsgTextCityType cityTypeInfo = btn.Tag as MsgTextCityType;
            if (cityTypeInfo == null)
            {
                return;
            }

            if (btn.Checked)
            {
                if (this.temporarySettingInfo.SelectedCityType == null)
                {
                    this.temporarySettingInfo.SelectedCityType = new MsgTextCityType();
                }
                MsgTextCityType copy = new MsgTextCityType();
                copy.DeepCopyFrom(cityTypeInfo);
                this.temporarySettingInfo.SelectedCityType = copy;

                // 문안 표시 갱신
                UpdateMessageText();
            }
        }
        #endregion

        #region 머리글추가/제거
        /// <summary>
        /// [머리글/추가] 문안에 포함된 ○○일○○시○○지역 에 대한 정보를 일괄 변환한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReplace_Click(object sender, EventArgs e)
        {
            string senderName = "[국민안전처]";

            if ((string.IsNullOrEmpty(this.txtboxHeaderTime.Text) && (string.IsNullOrEmpty(this.txtboxHeaderRegion.Text))) ||
                ((this.txtboxHeaderTime.Text == DEFAULT_HEADER_TIME) && (this.txtboxHeaderRegion.Text == DEFAULT_HEADER_REGION)))
            {
                return;
            }

            string headerTime = this.txtboxHeaderTime.Text;
            string headerRegion = this.txtboxHeaderRegion.Text;
            string ttsText = this.txtboxTTSMsg.Text;
            string cbsText = this.txtboxCBSMsg.Text;
            string boardText = this.txtboxBoardMsg.Text;
            string dmbText = this.txtboxDMBMsg.Text;

            if (!string.IsNullOrEmpty(modifyHistory.headerTime) || !string.IsNullOrEmpty(modifyHistory.headerRegion))
            {
                // 1회 이후의 시간/지역 정보 설정
                this.txtboxTTSMsg.Text = modifyHistory.lastTTS.Replace(modifyHistory.headerTime, headerTime);
                this.txtboxTTSMsg.Text = this.txtboxTTSMsg.Text.Replace(modifyHistory.headerRegion, headerRegion);

                this.txtboxCBSMsg.Text = modifyHistory.lastCBS.Replace(modifyHistory.headerTime, headerTime);
                this.txtboxCBSMsg.Text = this.txtboxCBSMsg.Text.Replace(modifyHistory.headerRegion, headerRegion);

                this.txtboxBoardMsg.Text = modifyHistory.lastBoard.Replace(modifyHistory.headerTime, headerTime);
                this.txtboxBoardMsg.Text = this.txtboxBoardMsg.Text.Replace(modifyHistory.headerRegion, headerRegion);

                this.txtboxDMBMsg.Text = modifyHistory.lastDMB.Replace(modifyHistory.headerTime, headerTime);
                this.txtboxDMBMsg.Text = this.txtboxDMBMsg.Text.Replace(modifyHistory.headerRegion, headerRegion);
            }
            else
            {
                // 최초의 시간/지역 정보 설정

                // 머리말 수정 이력 백업(변환 전 원본 정보)
                this.modifyHistory.originalTTS = this.txtboxTTSMsg.Text;
                this.modifyHistory.originalCBS = this.txtboxCBSMsg.Text;
                this.modifyHistory.originalBoard = this.txtboxBoardMsg.Text;
                this.modifyHistory.originalDMB = this.txtboxDMBMsg.Text;

                // TTS
                bool replaceTime = false;
                int startIndex = 0;
                string keyword = string.Empty;
                foreach (string key in BasisData.KEYWORD_TIMES)
                {
                    startIndex = this.txtboxTTSMsg.Text.IndexOf(key);
                    if (startIndex >= 0)
                    {
                        replaceTime = true;
                        this.txtboxTTSMsg.Text = this.txtboxTTSMsg.Text.Replace(key, this.txtboxHeaderTime.Text);
                        break;
                    }
                }
                bool replaceRegion = false;
                startIndex = 0;
                keyword = string.Empty;
                foreach (string key in BasisData.KEYWORD_REGIONS)
                {
                    startIndex = this.txtboxTTSMsg.Text.IndexOf(key);
                    if (startIndex >= 0)
                    {
                        replaceRegion = true;
                        this.txtboxTTSMsg.Text = this.txtboxTTSMsg.Text.Replace(key, this.txtboxHeaderRegion.Text);
                        break;
                    }
                }
                if (!replaceTime && !replaceRegion)
                {
                    string timeAndRegion = string.Empty;
                    if (!string.IsNullOrEmpty(this.txtboxHeaderTime.Text) && this.txtboxHeaderTime.Text != DEFAULT_HEADER_TIME)
                    {
                        timeAndRegion += this.txtboxHeaderTime.Text;
                    }
                    if (!string.IsNullOrEmpty(this.txtboxHeaderRegion.Text) && this.txtboxHeaderRegion.Text != DEFAULT_HEADER_REGION)
                    {
                        if (!string.IsNullOrEmpty(timeAndRegion))
                        {
                            timeAndRegion += ", ";
                        }
                        timeAndRegion += this.txtboxHeaderRegion.Text;
                    }
                    if (this.txtboxTTSMsg.Text.StartsWith(senderName))
                    {
                        this.txtboxTTSMsg.Text = this.txtboxTTSMsg.Text.Insert(senderName.Length, timeAndRegion);
                    }
                    else
                    {
                        this.txtboxTTSMsg.Text = timeAndRegion + this.txtboxTTSMsg.Text;
                    }
                }

                // CBS
                replaceTime = false;
                startIndex = 0;
                keyword = string.Empty;
                foreach (string key in BasisData.KEYWORD_TIMES)
                {
                    startIndex = this.txtboxCBSMsg.Text.IndexOf(key);
                    if (startIndex >= 0)
                    {
                        replaceTime = true;
                        this.txtboxCBSMsg.Text = this.txtboxCBSMsg.Text.Replace(key, this.txtboxHeaderTime.Text);
                        break;
                    }
                }
                replaceRegion = false;
                startIndex = 0;
                keyword = string.Empty;
                foreach (string key in BasisData.KEYWORD_REGIONS)
                {
                    startIndex = this.txtboxCBSMsg.Text.IndexOf(key);
                    if (startIndex >= 0)
                    {
                        replaceRegion = true;
                        this.txtboxCBSMsg.Text = this.txtboxCBSMsg.Text.Replace(key, this.txtboxHeaderRegion.Text);
                        break;
                    }
                }
                if (!replaceTime && !replaceRegion)
                {
                    string timeAndRegion = string.Empty;
                    if (!string.IsNullOrEmpty(this.txtboxHeaderTime.Text) && this.txtboxHeaderTime.Text != DEFAULT_HEADER_TIME)
                    {
                        timeAndRegion += this.txtboxHeaderTime.Text;
                    }
                    if (!string.IsNullOrEmpty(this.txtboxHeaderRegion.Text) && this.txtboxHeaderRegion.Text != DEFAULT_HEADER_REGION)
                    {
                        if (!string.IsNullOrEmpty(timeAndRegion))
                        {
                            timeAndRegion += ", ";
                        }
                        timeAndRegion += this.txtboxHeaderRegion.Text;
                    }
                    if (this.txtboxCBSMsg.Text.StartsWith(senderName))
                    {
                        this.txtboxCBSMsg.Text = this.txtboxCBSMsg.Text.Insert(senderName.Length, timeAndRegion);
                    }
                    else
                    {
                        this.txtboxCBSMsg.Text = timeAndRegion + this.txtboxCBSMsg.Text;
                    }
                }

                // Board
                replaceTime = false;
                startIndex = 0;
                keyword = string.Empty;
                foreach (string key in BasisData.KEYWORD_TIMES)
                {
                    startIndex = this.txtboxBoardMsg.Text.IndexOf(key);
                    if (startIndex >= 0)
                    {
                        replaceTime = true;
                        this.txtboxBoardMsg.Text = this.txtboxBoardMsg.Text.Replace(key, this.txtboxHeaderTime.Text);
                        break;
                    }
                }
                replaceRegion = false;
                startIndex = 0;
                keyword = string.Empty;
                foreach (string key in BasisData.KEYWORD_REGIONS)
                {
                    startIndex = this.txtboxBoardMsg.Text.IndexOf(key);
                    if (startIndex >= 0)
                    {
                        replaceRegion = true;
                        this.txtboxBoardMsg.Text = this.txtboxBoardMsg.Text.Replace(key, this.txtboxHeaderRegion.Text);
                        break;
                    }
                }
                if (!replaceTime && !replaceRegion)
                {
                    string timeAndRegion = string.Empty;
                    if (!string.IsNullOrEmpty(this.txtboxHeaderTime.Text) && this.txtboxHeaderTime.Text != DEFAULT_HEADER_TIME)
                    {
                        timeAndRegion += this.txtboxHeaderTime.Text;
                    }
                    if (!string.IsNullOrEmpty(this.txtboxHeaderRegion.Text) && this.txtboxHeaderRegion.Text != DEFAULT_HEADER_REGION)
                    {
                        if (!string.IsNullOrEmpty(timeAndRegion))
                        {
                            timeAndRegion += ", ";
                        }
                        timeAndRegion += this.txtboxHeaderRegion.Text;
                    }
                    if (this.txtboxBoardMsg.Text.StartsWith(senderName))
                    {
                        this.txtboxBoardMsg.Text = this.txtboxBoardMsg.Text.Insert(senderName.Length, timeAndRegion);
                    }
                    else
                    {
                        this.txtboxBoardMsg.Text = timeAndRegion + this.txtboxBoardMsg.Text;
                    }
                }

                // DMB
               replaceTime = false;
                startIndex = 0;
                keyword = string.Empty;
                foreach (string key in BasisData.KEYWORD_TIMES)
                {
                    startIndex = this.txtboxDMBMsg.Text.IndexOf(key);
                    if (startIndex >= 0)
                    {
                        replaceTime = true;
                        this.txtboxDMBMsg.Text = this.txtboxDMBMsg.Text.Replace(key, this.txtboxHeaderTime.Text);
                        break;
                    }
                }
                replaceRegion = false;
                startIndex = 0;
                keyword = string.Empty;
                foreach (string key in BasisData.KEYWORD_REGIONS)
                {
                    startIndex = this.txtboxDMBMsg.Text.IndexOf(key);
                    if (startIndex >= 0)
                    {
                        replaceRegion = true;
                        this.txtboxDMBMsg.Text = this.txtboxDMBMsg.Text.Replace(key, this.txtboxHeaderRegion.Text);
                        break;
                    }
                }
                if (!replaceTime && !replaceRegion)
                {
                    string timeAndRegion = string.Empty;
                    if (!string.IsNullOrEmpty(this.txtboxHeaderTime.Text) && this.txtboxHeaderTime.Text != DEFAULT_HEADER_TIME)
                    {
                        timeAndRegion += this.txtboxHeaderTime.Text;
                    }
                    if (!string.IsNullOrEmpty(this.txtboxHeaderRegion.Text) && this.txtboxHeaderRegion.Text != DEFAULT_HEADER_REGION)
                    {
                        if (!string.IsNullOrEmpty(timeAndRegion))
                        {
                            timeAndRegion += ", ";
                        }
                        timeAndRegion += this.txtboxHeaderRegion.Text;
                    }
                    if (this.txtboxDMBMsg.Text.StartsWith(senderName))
                    {
                        this.txtboxDMBMsg.Text = this.txtboxDMBMsg.Text.Insert(senderName.Length, timeAndRegion);
                    }
                    else
                    {
                        this.txtboxDMBMsg.Text = timeAndRegion + this.txtboxDMBMsg.Text;
                    }
                }
            }

            // 머리말 수정 이력 백업(변환 후 정보)
            modifyHistory.headerTime = headerTime;
            modifyHistory.headerRegion = headerRegion;
            modifyHistory.lastTTS = this.txtboxTTSMsg.Text;
            modifyHistory.lastCBS = this.txtboxCBSMsg.Text;
            modifyHistory.lastBoard = this.txtboxBoardMsg.Text;
            modifyHistory.lastDMB = this.txtboxDMBMsg.Text;

            this.btnRevert.Enabled = true;

            // Temporary관리정보에 적용
            TextToTemporarySettingInfo();
        }
        /// <summary>
        /// [시간/지역 정보] 삽입했던 머리말 취소.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevert_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(modifyHistory.headerTime) || !string.IsNullOrEmpty(modifyHistory.headerRegion))
            {
                this.txtboxTTSMsg.Text = modifyHistory.originalTTS;
                this.txtboxCBSMsg.Text = modifyHistory.originalCBS;
                this.txtboxBoardMsg.Text = modifyHistory.originalBoard;
                this.txtboxDMBMsg.Text = modifyHistory.originalDMB;

                TextToTemporarySettingInfo();

                modifyHistory.Clear();
            }

            UpdateMessageText();

            // 머리글 취소 불가
            this.btnRevert.Enabled = false;
            this.modifyHistory.Clear();
        }
        private void TextToTemporarySettingInfo()
        {
            MsgTextDisplayLanguageKind selectedLanguage = this.cmbboxLanguage.SelectedItem as MsgTextDisplayLanguageKind;
            if (selectedLanguage == null)
            {
                System.Console.WriteLine("[MsgTextForm] UpdateMessageText( 언어 종류 선택 상태를 얻을 수 없음 )");
                return;
            }

            foreach (MsgText msgText in this.temporarySettingInfo.CurrentTransmitMsgText)
            {
                // 언어
                if (msgText.LanguageKindID != selectedLanguage.ID)
                {
                    continue;
                }
                // 도시 형태
                if (msgText.CityTypeID != this.temporarySettingInfo.SelectedCityType.ID)
                {
                    continue;
                }
                // 표출매체 유형
                MsgTextDisplayMediaType mediaType = BasisData.FindMediaTypeInfoByID(msgText.MediaTypeID);
                if (mediaType == null)
                {
                    continue;
                }

                if (pnlMsgTextSetting.Controls.ContainsKey(mediaType.Code))
                {
                    TextBox control = pnlMsgTextSetting.Controls[mediaType.Code] as TextBox;
                    if (control == null)
                    {
                        continue;
                    }
                    msgText.Text = control.Text;
                }
            }
        }
        #endregion

        private MsgText SearchMsgTextFromTargetSettingInfo(SendingMsgTextInfo target, string languageCode, int cityTypeID, int mediaTypeID)
        {
            MsgText result = null;

            foreach (MsgText msg in target.CurrentTransmitMsgText)
            {
                MsgTextDisplayLanguageKind language = BasisData.FindMsgTextLanguageInfoByID(msg.LanguageKindID);
                if (language == null)
                {
                    continue;
                }
                if (language.LanguageCode != languageCode)
                {
                    continue;
                }
                if (msg.CityTypeID != cityTypeID)
                {
                    continue;
                }
                if (msg.MediaTypeID != mediaTypeID)
                {
                    continue;
                }

                result = msg;
                break;
            }

            return result;
        }


        #region 문안표시/편집
        /// <summary>
        /// 문안을 표시할 언어 종류 변경.
        /// ★★★ 검토필요: 도시유형별의 종류가 [일반]이 아닌 경우에도 언어 선택 가능하게 할 것인가?
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbboxLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox ctrl = sender as ComboBox;
            if (ctrl == null)
            {
                return;
            }
            if (!ctrl.Focused)
            {
                return;
            }
            // 수동변경 후에는 머리글 취소 불가
            this.btnRevert.Enabled = false;
            this.modifyHistory.Clear();

            // 현재 문안을 Temporary 관리데이터에 저장
            foreach (MsgText temporary in this.temporarySettingInfo.CurrentTransmitMsgText)
            {
                // 언어  설정
                if (previousLanguageSelection.ID != temporary.LanguageKindID)
                {
                    continue;
                }
                // 도시 형태
                if (this.temporarySettingInfo.SelectedCityType.ID != temporary.CityTypeID)
                {
                    continue;
                }
                // 표출매체 유형
                MsgTextDisplayMediaType mediaType = BasisData.FindMediaTypeInfoByID(temporary.MediaTypeID);
                if (mediaType == null)
                {
                    continue;
                }

                if (mediaType.Code == this.txtboxTTSMsg.Name)
                {
                    temporary.Text = this.txtboxTTSMsg.Text;
                }
                else if (mediaType.Code == this.txtboxCBSMsg.Name)
                {
                    temporary.Text = this.txtboxCBSMsg.Text;
                }
                else if (mediaType.Code == this.txtboxBoardMsg.Name)
                {
                    temporary.Text = this.txtboxBoardMsg.Text;
                }
                else if (mediaType.Code == this.txtboxDMBMsg.Name)
                {
                    temporary.Text = this.txtboxDMBMsg.Text;
                }
                else
                {
                    // invalid type
                }
            }

            // 초기화
            this.txtboxTTSMsg.ResetText();
            this.txtboxCBSMsg.ResetText();
            this.txtboxBoardMsg.ResetText();
            this.txtboxDMBMsg.ResetText();

            // 변경된 아이템에 맞는 문안으로 갱신
            UpdateMessageText();

            // 현재의 문안을 Temporary에 저장
            MsgTextDisplayLanguageKind selectedLanguage = ctrl.SelectedItem as MsgTextDisplayLanguageKind;
            this.previousLanguageSelection = selectedLanguage;
        }
        /// <summary>
        /// 문안 내용 편집
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtboxMsgTxt_TextChanged(object sender, EventArgs e)
        {
            TextBox ctrl = sender as TextBox;
            if (ctrl == null)
            {
                return;
            }

            // 입력 문자 수
            string limitCnt = "(" + ctrl.Text.Length.ToString() + "/" + ctrl.MaxLength.ToString() + ")";
            if (ctrl.Name.ToUpper().Contains("TTS"))
            {
                this.lblTTSLetterCount.Text = limitCnt;
            }
            else if (ctrl.Name.ToUpper().Contains("CBS"))
            {
                this.lblCBSLetterCount.Text = limitCnt;
            }
            else if (ctrl.Name.ToUpper().Contains("BOARD"))
            {
                this.lblBoardLetterCount.Text = limitCnt;
            }
            else if (ctrl.Name.ToUpper().Contains("DMB"))
            {
                this.lblDMBLetterCount.Text = limitCnt;
            }
            else
            {
            }

            if (this.cmbboxLanguage.SelectedItem == null)
            {
                return;
            }

            if (ctrl.Focused)
            {
                // 수동변경 후에는 머리글 취소 불가
                this.btnRevert.Enabled = false;
                this.modifyHistory.Clear();

                MsgTextDisplayLanguageKind langKind = this.cmbboxLanguage.SelectedItem as MsgTextDisplayLanguageKind;
                if (langKind == null)
                {
                    return;
                }

                MsgTextDisplayMediaType mediaType = ctrl.Tag as MsgTextDisplayMediaType;

                MsgText msg = SearchMsgTextFromTargetSettingInfo(this.temporarySettingInfo, langKind.LanguageCode, this.temporarySettingInfo.SelectedCityType.ID, (int)mediaType.ID);
                if (msg != null)
                {
                    msg.Text = ctrl.Text;
                }
            }
        }
        #endregion


        #region 문안편집내용_저장/취소(닫기)/초기화
        /// <summary>
        /// [초기화] 버튼 클릭 이벤트 핸들러.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            if (IsLanguageSettingChanged(this.originalSettingInfo) || IsCityTypeSettingChanged(this.originalSettingInfo) || IsMsgTextSettingChanged(this.originalSettingInfo))
            {
                DialogResult answer = MessageBox.Show("문안 내용을 편집 이전의 상태로 초기화합니다. 계속하시겠습니까?", "문안 편집 초기화",
                                                        MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (answer != System.Windows.Forms.DialogResult.Yes)
                {
                    return;
                }

                this.currentSettingInfo.DeepCopyFrom(originalSettingInfo);
                this.temporarySettingInfo.DeepCopyFrom(currentSettingInfo);
            }

            UpdateLanguageSelection();
            UpdateCityTypeSelection();
            UpdateMessageText();

            // 머리글 취소 불가
            this.btnRevert.Enabled = false;
            this.modifyHistory.Clear();

            MessageBox.Show("편집 상태를 초기화하였습니다.", "문안 편집 초기화", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// [닫기] 버튼 클릭 이벤트 핸들러.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            CheckSaveMsgText();

            if (!CheckMsgValidation())
            {
                return;
            }

            this.Close();
        }
        /// <summary>
        /// [저장] 버튼 클릭 이벤트 핸들러.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.currentSettingInfo.DeepCopyFrom(this.temporarySettingInfo);

            MessageBox.Show("저장하였습니다.", "전송 문안 저장", MessageBoxButtons.OK);
        }
        #endregion


        private bool CheckMsgValidation()
        {
            bool isNullExist = false;
            string invalidMsgLanguage = string.Empty;
            string invalidMsgMediaType = string.Empty;

            foreach (MsgTextDisplayLanguageKind language in this.currentSettingInfo.SelectedLanguages)
            {
                int selectedCityTypeID = this.currentSettingInfo.SelectedCityType.ID;
                foreach (MsgTextDisplayMediaType mediaType in BasisData.MsgTextDisplayMediaType)
                {
                    MsgText txt = SearchMsgTextFromTargetSettingInfo(this.currentSettingInfo, language.LanguageCode, selectedCityTypeID, (int)mediaType.ID);
                    if (txt == null || string.IsNullOrEmpty(txt.Text))
                    {
                        isNullExist = true;
                        invalidMsgLanguage = language.LanguageName;
                        invalidMsgMediaType = mediaType.TypeName;
                        break;
                    }
                }
                if (isNullExist)
                {
                    break;
                }

                if (isNullExist)
                {
                    break;
                }
            }

            if (isNullExist)
            {
                MessageBox.Show("문안이 입력되지 않은 항목이 있습니다. \n문안을 입력해 주세요. [" + invalidMsgLanguage + "/" + invalidMsgMediaType + "]",
                                "문안 항목 오류", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            return !isNullExist;
        }
        private void CheckSaveMsgText()
        {
            if (IsLanguageSettingChanged(this.currentSettingInfo) || IsCityTypeSettingChanged(this.currentSettingInfo) || IsMsgTextSettingChanged(this.currentSettingInfo))
            {
                DialogResult answer = MessageBox.Show("저장되지 않은 변경 사항이 있습니다.\n이 창을 닫기 전에 변경 내용을 저장하시겠습니까?",
                    "문안 변경 저장", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (answer == System.Windows.Forms.DialogResult.Yes)
                {
                    this.currentSettingInfo.DeepCopyFrom(this.temporarySettingInfo);
                }
            }

            this.originalSettingInfo.DeepCopyFrom(this.currentSettingInfo);
        }
        /// <summary>
        /// CurrentSettingInfo 를 기준으로 TemporarySettingInfo 의 (언어 설정) 변경 유무 체크
        /// </summary>
        /// <returns></returns>
        private bool IsLanguageSettingChanged(SendingMsgTextInfo baseInfo)
        {
            bool isChanged = false;

            if (baseInfo == null)
            {
                return false;
            }

            if (baseInfo.SelectedLanguages == null)
            {
                if (this.temporarySettingInfo.SelectedLanguages == null)
                {
                }
                else if (this.temporarySettingInfo.SelectedLanguages.Count <= 0)
                {
                }
                else
                {
                    isChanged = true;
                }
            }
            else if (baseInfo.SelectedLanguages.Count == 0)
            {
                if (this.temporarySettingInfo.SelectedLanguages.Count <= 1)
                {
                    // 동일(디폴트 설정)
                }
                else
                {
                    isChanged = true;
                }
            }
            else
            {
                if (baseInfo.SelectedLanguages.Count != this.temporarySettingInfo.SelectedLanguages.Count)
                {
                    isChanged = true;
                }
                else
                {
                    for (int index = 0; index < baseInfo.SelectedLanguages.Count; index++)
                    {
                        MsgTextDisplayLanguageKind currentLanguage = baseInfo.SelectedLanguages[index];
                        MsgTextDisplayLanguageKind temporaryLanguage = this.temporarySettingInfo.SelectedLanguages[index];
                        if (currentLanguage.LanguageCode != temporaryLanguage.LanguageCode)
                        {
                            isChanged = true;
                            break;
                        }
                    }
                }
            }

            return isChanged;
        }
        /// <summary>
        /// CurrentSettingInfo 를 기준으로 TemporarySettingInfo 의 (도시 유형별) 변경 유무 체크
        /// </summary>
        /// <returns></returns>
        private bool IsCityTypeSettingChanged(SendingMsgTextInfo baseInfo)
        {
            bool isChanged = false;

            if (baseInfo.SelectedCityType == null)
            {
                if (this.temporarySettingInfo.SelectedCityType == null)
                {
                }
                else if (this.temporarySettingInfo.SelectedCityType.TypeCode == BasisData.DEFAULT_CITYTYPECODE)
                {
                }
                else
                {
                    isChanged = true;
                }
            }
            else if (this.temporarySettingInfo.SelectedCityType == null)
            {
                if (baseInfo.SelectedCityType.TypeCode != BasisData.DEFAULT_CITYTYPECODE)
                {
                    isChanged = true;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(baseInfo.SelectedCityType.TypeCode))
                {
                    if (string.IsNullOrEmpty(this.temporarySettingInfo.SelectedCityType.TypeCode) ||
                        (this.temporarySettingInfo.SelectedCityType.TypeCode == BasisData.DEFAULT_CITYTYPECODE))
                    {
                        // default setting
                    }
                    else
                    {
                        isChanged = true;
                    }
                }
                else if (!string.IsNullOrEmpty(this.temporarySettingInfo.SelectedCityType.TypeCode) &&
                         (baseInfo.SelectedCityType.TypeCode != this.temporarySettingInfo.SelectedCityType.TypeCode))
                {
                    isChanged = true;
                }
                else
                {
                }
            }

            return isChanged;
        }
        /// <summary>
        /// CurrentSettingInfo 를 기준으로 TemporarySettingInfo 의 (문안 내용) 변경 유무 체크
        /// </summary>
        /// <returns></returns>
        private bool IsMsgTextSettingChanged(SendingMsgTextInfo baseInfo)
        {
            bool isChanged = false;

            foreach (MsgTextDisplayLanguageKind language in BasisData.MsgTextLanguageKind)
            {
                foreach (MsgTextCityType cityType in BasisData.MsgTextCityType)
                {
                    foreach (MsgTextDisplayMediaType mediaType in BasisData.MsgTextDisplayMediaType)
                    {
                        MsgText current = SearchMsgTextFromTargetSettingInfo(this.currentSettingInfo, language.LanguageCode, cityType.ID, (int)mediaType.ID);
                        MsgText temporary = SearchMsgTextFromTargetSettingInfo(this.temporarySettingInfo, language.LanguageCode, cityType.ID, (int)mediaType.ID);

                        if (current == null && (temporary != null && !string.IsNullOrEmpty(temporary.Text)))
                        {
                            isChanged = true;
                            break;
                        }
                        else if ((current != null && temporary != null) && (current.Text != temporary.Text))
                        {
                            isChanged = true;
                            break;
                        }
                        else
                        {
                        }
                    }
                }
            }

            return isChanged;
        }

    }

    public class MsgModifyHistory
    {
        public string originalTTS = string.Empty;
        public string originalCBS = string.Empty;
        public string originalBoard = string.Empty;
        public string originalDMB = string.Empty;
        public string headerTime = string.Empty;
        public string headerRegion = string.Empty;
        public string lastTTS = string.Empty;
        public string lastCBS = string.Empty;
        public string lastBoard = string.Empty;
        public string lastDMB = string.Empty;

        public void Clear()
        {
            this.originalTTS = string.Empty;
            this.originalCBS = string.Empty;
            this.originalBoard = string.Empty;
            this.originalDMB = string.Empty;
            this.headerTime = string.Empty;
            this.headerRegion = string.Empty;
            this.lastTTS = string.Empty;
            this.lastCBS = string.Empty;
            this.lastBoard = string.Empty;
            this.lastDMB = string.Empty;
        }
    }
}