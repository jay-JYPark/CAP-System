using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using StdWarningInstallation.Ctrl;
using StdWarningInstallation.DataClass;

namespace StdWarningInstallation
{
    public partial class SWRConditionSettingForm : Form
    {
        public event EventHandler<SWRConditionUpdateEventArgs> NotifySWRAssociationConditionUpdate;

        private ConfigSWRData currentSetting = null;
        private List<SWRAssociationCondition> currentCondition = null;

        public SWRConditionSettingForm(ConfigSWRData setting, List<SWRAssociationCondition> conditions)
        {
            InitializeComponent();

            this.currentSetting = setting;
            this.currentCondition = conditions;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.chkboxUseAssociation.Checked = this.currentSetting.UseService;

            if (!this.currentSetting.UseService)
            {
                UpdateChkStateAll(false);
                EnableElement(false);
            }
            else
            {
                if (this.currentCondition == null)
                {
                    return;
                }

                EnableElement(true);

                foreach (SWRAssociationCondition condition in this.currentCondition)
                {
                    switch (condition.WarnKindCode)
                    {
                        case 1:
                            {
                                // 강풍
                                if (condition.WarnStressCode == 0)
                                {
                                    this.swrConditionPartHighWind.Watch = condition.IsUse;
                                }
                                else
                                {
                                    this.swrConditionPartHighWind.Warning = condition.IsUse;
                                }
                            }
                            break;
                        case 2:
                            {
                                // 호우
                                if (condition.WarnStressCode == 0)
                                {
                                    this.swrConditionPartHeavyRain.Watch = condition.IsUse;
                                }
                                else
                                {
                                    this.swrConditionPartHeavyRain.Warning = condition.IsUse;
                                }
                            }
                            break;
                        case 3:
                            {
                                // 한파
                                if (condition.WarnStressCode == 0)
                                {
                                    this.swrConditionPartColdWave.Watch = condition.IsUse;
                                }
                                else
                                {
                                    this.swrConditionPartColdWave.Warning = condition.IsUse;
                                }
                            }
                            break;
                        case 4:
                            {
                                // 건조
                                if (condition.WarnStressCode == 0)
                                {
                                    this.swrConditionPartHeavyArid.Watch = condition.IsUse;
                                }
                                else
                                {
                                    this.swrConditionPartHeavyArid.Warning = condition.IsUse;
                                }
                            }
                            break;
                        case 5:
                            {
                                // 해일
                                if (condition.WarnStressCode == 0)
                                {
                                    this.swrConditionPartStormSurge.Watch = condition.IsUse;
                                }
                                else
                                {
                                    this.swrConditionPartStormSurge.Warning = condition.IsUse;
                                }
                            }
                            break;
                        case 6:
                            {
                                // 풍랑
                                if (condition.WarnStressCode == 0)
                                {
                                    this.swrConditionPartWindAndWaves.Watch = condition.IsUse;
                                }
                                else
                                {
                                    this.swrConditionPartWindAndWaves.Warning = condition.IsUse;
                                }
                            }
                            break;
                        case 7:
                            {
                                // 태풍
                                if (condition.WarnStressCode == 0)
                                {
                                    this.swrConditionPartHurricane.Watch = condition.IsUse;
                                }
                                else
                                {
                                    this.swrConditionPartHurricane.Warning = condition.IsUse;
                                }
                            }
                            break;
                        case 8:
                            {
                                // 대설
                                if (condition.WarnStressCode == 0)
                                {
                                    this.swrConditionPartHeavySnow.Watch = condition.IsUse;
                                }
                                else
                                {
                                    this.swrConditionPartHeavySnow.Warning = condition.IsUse;
                                }
                            }
                            break;
                        case 9:
                            {
                                // 황사
                                if (condition.WarnStressCode == 0)
                                {
                                    this.swrConditionPartYellowSand.Watch = condition.IsUse;
                                }
                                else
                                {
                                    this.swrConditionPartYellowSand.Warning = condition.IsUse;
                                }
                            }
                            break;
                        case 12:
                            {
                                // 폭염
                                if (condition.WarnStressCode == 0)
                                {
                                    this.swrConditionPartHeatWaveSpecial.Watch = condition.IsUse;
                                }
                                else
                                {
                                    this.swrConditionPartHeatWaveSpecial.Warning = condition.IsUse;
                                }
                            }
                            break;
                        default:
                            {
                            }
                            break;
                    }
                }
            }
        }

        private void EnableElement(bool enable)
        {
            this.swrConditionPartHighWind.Enabled = enable;
            this.swrConditionPartHeavyRain.Enabled = enable;
            this.swrConditionPartColdWave.Enabled = enable;
            this.swrConditionPartHeavyArid.Enabled = enable;
            this.swrConditionPartStormSurge.Enabled = enable;
            this.swrConditionPartWindAndWaves.Enabled = enable;
            this.swrConditionPartHurricane.Enabled = enable;
            this.swrConditionPartHeavySnow.Enabled = enable;
            this.swrConditionPartYellowSand.Enabled = enable;
            this.swrConditionPartHeatWaveSpecial.Enabled = enable;
        }

        private void UpdateChkStateAll(bool chkState)
        {
            // 강풍
            this.swrConditionPartHighWind.Watch = chkState;
            this.swrConditionPartHighWind.Warning = chkState;

            // 호우
            this.swrConditionPartHeavyRain.Watch = chkState;
            this.swrConditionPartHeavyRain.Warning = chkState;

            // 태풍
            this.swrConditionPartColdWave.Watch = chkState;
            this.swrConditionPartColdWave.Warning = chkState;

            // 한파
            this.swrConditionPartHeavyArid.Watch = chkState;
            this.swrConditionPartHeavyArid.Warning = chkState;

            // 건조
            this.swrConditionPartStormSurge.Watch = chkState;
            this.swrConditionPartStormSurge.Warning = chkState;

            // 대설
            this.swrConditionPartWindAndWaves.Watch = chkState;
            this.swrConditionPartWindAndWaves.Warning = chkState;

            // 황사
            this.swrConditionPartHurricane.Watch = chkState;
            this.swrConditionPartHurricane.Warning = chkState;

            // 풍랑
            this.swrConditionPartHeavySnow.Watch = chkState;
            this.swrConditionPartHeavySnow.Warning = chkState;

            // 해일
            this.swrConditionPartYellowSand.Watch = chkState;
            this.swrConditionPartYellowSand.Warning = chkState;

            // 폭염
            this.swrConditionPartHeatWaveSpecial.Watch = chkState;
            this.swrConditionPartHeatWaveSpecial.Warning = chkState;
        }

        /// <summary>
        /// [기상 특보 연계 유무] 체크 이벤트 핸들러.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkboxUseAssociation_CheckedChanged(object sender, EventArgs e)
        {
            this.currentSetting.UseService = this.chkboxUseAssociation.Checked;

            if (this.currentSetting.UseService)
            {
                this.chkboxUseAssociation.Font = new Font("굴림", 9.0f, FontStyle.Bold);

                UpdateChkStateAll(true);
                EnableElement(true);
            }
            else
            {
                this.chkboxUseAssociation.Font = new Font("굴림", 9.0f, FontStyle.Regular);

                UpdateChkStateAll(false);
                EnableElement(false);
            }
        }

        /// <summary>
        /// [주의보/경보] 체크 이벤트 핸들러.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkboxCondition_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            List<SWRAssociationCondition> conditions = null;

            if (this.chkboxUseAssociation.Checked)
            {
                conditions = new List<SWRAssociationCondition>();

                // 강풍
                //conditionInfo.HighWind.Watch = this.chkboxHWA.Checked;
                //conditionInfo.HighWind.Warning = this.chkboxHWW.Checked;
                SWRAssociationCondition condition = new SWRAssociationCondition();
                condition.WarnKindCode = 1;
                condition.WarnStressCode = 0;
                condition.IsUse = this.swrConditionPartHighWind.Watch;
                conditions.Add(condition);
                condition = new SWRAssociationCondition();
                condition.WarnKindCode = 1;
                condition.WarnStressCode = 1;
                condition.IsUse = this.swrConditionPartHighWind.Warning;
                conditions.Add(condition);

                // 호우
                //conditionInfo.HeavyRain.Watch = this.chkboxHRA.Checked;
                //conditionInfo.HeavyRain.Warning = this.chkboxHRW.Checked;
                condition = new SWRAssociationCondition();
                condition.WarnKindCode = 2;
                condition.WarnStressCode = 0;
                condition.IsUse = this.swrConditionPartHeavyRain.Watch;
                conditions.Add(condition);
                condition = new SWRAssociationCondition();
                condition.WarnKindCode = 2;
                condition.WarnStressCode = 1;
                condition.IsUse = this.swrConditionPartHeavyRain.Warning;
                conditions.Add(condition);

                // 태풍
                //conditionInfo.Hurricane.Watch = this.chkboxHUA.Checked;
                //conditionInfo.Hurricane.Warning = this.chkboxHUW.Checked;
                condition = new SWRAssociationCondition();
                condition.WarnKindCode = 7;
                condition.WarnStressCode = 0;
                condition.IsUse = this.swrConditionPartHurricane.Watch;
                conditions.Add(condition);
                condition = new SWRAssociationCondition();
                condition.WarnKindCode = 7;
                condition.WarnStressCode = 1;
                condition.IsUse = this.swrConditionPartHurricane.Warning;
                conditions.Add(condition);

                // 한파
                //conditionInfo.ColdWave.Watch = this.chkboxCWA.Checked;
                //conditionInfo.ColdWave.Warning = this.chkboxCWW.Checked;
                condition = new SWRAssociationCondition();
                condition.WarnKindCode = 3;
                condition.WarnStressCode = 0;
                condition.IsUse = this.swrConditionPartColdWave.Watch;
                conditions.Add(condition);
                condition = new SWRAssociationCondition();
                condition.WarnKindCode = 3;
                condition.WarnStressCode = 1;
                condition.IsUse = this.swrConditionPartColdWave.Warning;
                conditions.Add(condition);

                // 건조
                //conditionInfo.HeavyArid.Watch = this.chkboxHAA.Checked;
                //conditionInfo.HeavyArid.Warning = this.chkboxHAW.Checked;
                condition = new SWRAssociationCondition();
                condition.WarnKindCode = 4;
                condition.WarnStressCode = 0;
                condition.IsUse = this.swrConditionPartHeavyArid.Watch;
                conditions.Add(condition);
                condition = new SWRAssociationCondition();
                condition.WarnKindCode = 4;
                condition.WarnStressCode = 1;
                condition.IsUse = this.swrConditionPartHeavyArid.Warning;
                conditions.Add(condition);

                // 대설
                //conditionInfo.HeavySnow.Watch = this.chkboxHSW.Checked;
                //conditionInfo.HeavySnow.Warning = this.chkboxHAS.Checked;
                condition = new SWRAssociationCondition();
                condition.WarnKindCode = 8;
                condition.WarnStressCode = 0;
                condition.IsUse = this.swrConditionPartHeavySnow.Watch;
                conditions.Add(condition);
                condition = new SWRAssociationCondition();
                condition.WarnKindCode = 8;
                condition.WarnStressCode = 1;
                condition.IsUse = this.swrConditionPartHeavySnow.Warning;
                conditions.Add(condition);

                // 황사
                //conditionInfo.YellowSand.Watch = this.chkboxYSA.Checked;
                //conditionInfo.YellowSand.Warning = this.chkboxYSW.Checked;
                condition = new SWRAssociationCondition();
                condition.WarnKindCode = 9;
                condition.WarnStressCode = 0;
                condition.IsUse = this.swrConditionPartYellowSand.Watch;
                conditions.Add(condition);
                condition = new SWRAssociationCondition();
                condition.WarnKindCode = 9;
                condition.WarnStressCode = 1;
                condition.IsUse = this.swrConditionPartYellowSand.Warning;
                conditions.Add(condition);

                // 풍랑
                //conditionInfo.WindAndWaves.Watch = this.chkboxWWA.Checked;
                //conditionInfo.WindAndWaves.Warning = this.chkboxWWW.Checked;
                condition = new SWRAssociationCondition();
                condition.WarnKindCode = 6;
                condition.WarnStressCode = 0;
                condition.IsUse = this.swrConditionPartWindAndWaves.Watch;
                conditions.Add(condition);
                condition = new SWRAssociationCondition();
                condition.WarnKindCode = 6;
                condition.WarnStressCode = 1;
                condition.IsUse = this.swrConditionPartWindAndWaves.Warning;
                conditions.Add(condition);

                // 해일
                //conditionInfo.StormSurge.Watch = this.chkboxSSA.Checked;
                //conditionInfo.StormSurge.Warning = this.chkboxSSW.Checked;
                condition = new SWRAssociationCondition();
                condition.WarnKindCode = 5;
                condition.WarnStressCode = 0;
                condition.IsUse = this.swrConditionPartStormSurge.Watch;
                conditions.Add(condition);
                condition = new SWRAssociationCondition();
                condition.WarnKindCode = 5;
                condition.WarnStressCode = 1;
                condition.IsUse = this.swrConditionPartStormSurge.Warning;
                conditions.Add(condition);

                // 폭염
                //conditionInfo.HeatWaveSpecial.Watch = this.chkboxSSA.Checked;
                //conditionInfo.HeatWaveSpecial.Warning = this.chkboxSSW.Checked;
                condition = new SWRAssociationCondition();
                condition.WarnKindCode = 12;
                condition.WarnStressCode = 0;
                condition.IsUse = this.swrConditionPartHeatWaveSpecial.Watch;
                conditions.Add(condition);
                condition = new SWRAssociationCondition();
                condition.WarnKindCode = 12;
                condition.WarnStressCode = 1;
                condition.IsUse = this.swrConditionPartHeatWaveSpecial.Warning;
                conditions.Add(condition);
            }

            if (this.NotifySWRAssociationConditionUpdate != null)
            {
                this.NotifySWRAssociationConditionUpdate(this, new SWRConditionUpdateEventArgs(this.chkboxUseAssociation.Checked, conditions));
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

    public class SWRConditionUpdateEventArgs : EventArgs
    {
        private bool useAssociation = false;
        public bool UseAssociation
        {
            get { return useAssociation; }
            set { useAssociation = value; }
        }
        private List<SWRAssociationCondition> conditionList = new List<SWRAssociationCondition>();
        public List<SWRAssociationCondition> ConditionList
        {
            get { return conditionList; }
            set { conditionList = value; }
        }

        public SWRConditionUpdateEventArgs(bool useAssociation, List<SWRAssociationCondition> inConditionList)
        {
            this.useAssociation = useAssociation;

            if (inConditionList != null)
            {
                foreach (SWRAssociationCondition condition in inConditionList)
                {
                    SWRAssociationCondition copy = new SWRAssociationCondition();
                    copy.DeepCopyFrom(condition);

                    this.conditionList.Add(copy);
                }
            }
        }
    }

}
