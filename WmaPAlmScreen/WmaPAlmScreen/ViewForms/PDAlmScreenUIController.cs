using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using NCASFND;
using NCASFND.NCasCtrl;
using NCASFND.NCasLogging;
using NCASBIZ.NCasData;
using NCASBIZ.NCasEnv;
using NCASBIZ.NCasDefine;
using NCASBIZ.NCasProtocol;
using NCASBIZ.NCasPlcProtocol;
using NCASBIZ.NCasUtility;
using NCasAppCommon.Define;
using NCasAppCommon.Type;
using NCasMsgCommon.Std;
using NCasMsgCommon.Tts;
using NCasContentsModule.StoMsg;
using NCasContentsModule.TTS;

namespace WmaPAlmScreen
{
    public partial class PDAlmScreenUIController : ViewBase
    {
        #region 생성자
        /// <summary>
        /// 생성자
        /// </summary>
        public PDAlmScreenUIController()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="main"></param>
        public PDAlmScreenUIController(MainForm main)
        {
            this.main = main;
        }
        #endregion
    }
}