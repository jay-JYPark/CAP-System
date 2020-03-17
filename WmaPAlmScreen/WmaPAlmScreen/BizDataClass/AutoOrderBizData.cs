using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCASBIZ.NCasType;
using NCASBIZ.NCasProtocol;
using NCASBIZ.NCasDefine;
using NCASFND.NCasLogging;
using NCasMsgCommon.Tts;
using NCasMsgCommon.Std;
using NCasContentsModule.TTS;
using NCasContentsModule.StoMsg;

namespace WmaPAlmScreen
{
    public class AutoOrderBizData : NCasObject
    {
        private NCasProtocolTc171 almProtocol = null;
        private byte[] sendBuff = null;

        /// <summary>
        /// 경보발령 TC 프로퍼티
        /// </summary>
        public NCasProtocolTc171 AlmProtocol
        {
            get { return this.almProtocol; }
            set { this.almProtocol = value; }
        }

        /// <summary>
        /// 데이터
        /// </summary>
        public byte[] SendBuff
        {
            get { return this.sendBuff; }
            set { this.sendBuff = value; }
        }

        public override void CloneFrom(NCasObject obj)
        {
            AutoOrderBizData newObject = obj as AutoOrderBizData;
            this.almProtocol = newObject.almProtocol;
            this.sendBuff = newObject.sendBuff;
        }

        public override NCasObject CloneTo()
        {
            AutoOrderBizData newObject = new AutoOrderBizData();
            newObject.almProtocol = this.almProtocol;
            newObject.sendBuff = this.sendBuff;

            return newObject;
        }
    }
}