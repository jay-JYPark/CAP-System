using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCASBIZ.NCasType;
using NCASBIZ.NCasProtocol;
using NCasAppCommon.Type;

namespace WmaPAlmScreen
{
    public class LedBizData : NCasObject
    {
        private NCasProtocolTc80 protocolTc80 = null;
        private byte[] sendBuff = null;

        public NCasProtocolTc80 ProtocolTc80
        {
            get { return this.protocolTc80; }
            set { this.protocolTc80 = value; }
        }

        public byte[] SendBuff
        {
            get { return this.sendBuff; }
            set { this.sendBuff = value; }
        }

        public override void CloneFrom(NCasObject obj)
        {
            LedBizData newObject = obj as LedBizData;
            this.protocolTc80 = newObject.protocolTc80;
            this.sendBuff = newObject.sendBuff;
        }

        public override NCasObject CloneTo()
        {
            LedBizData newObject = new LedBizData();
            newObject.protocolTc80 = this.protocolTc80;
            newObject.sendBuff = this.sendBuff;
            return newObject;
        }
    }
}