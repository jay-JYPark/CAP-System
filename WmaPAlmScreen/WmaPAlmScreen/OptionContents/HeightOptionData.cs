using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCASBIZ.NCasEnv;
using NCASBIZ.NCasUtility;
using NCASFND.NCasLogging;
using NCasAppCommon.Define;
using NCasMsgCommon.Std;
using NCasContentsModule.StoMsg;

namespace WmaPAlmScreen
{
    public class HeightOptionData
    {
        private bool useTime = false;
        private int firstTime = 0;
        private int secondTime = 1;
        private bool useAuto = false;
        private int maxValue = 0;
        private int maxValue2 = 0;
        private int maxValue3 = 0;
        private int maxValue4 = 0;
        private int autoTime = 0;
        private StoredMessageText msg = new StoredMessageText();
        private StoredMessageText msg2 = new StoredMessageText();
        private StoredMessageText msg3 = new StoredMessageText();
        private StoredMessageText msg4 = new StoredMessageText();
        private bool testOrder = false;

        public bool UseTime
        {
            get { return useTime; }
            set { useTime = value; }
        }

        public int FirstTime
        {
            get { return firstTime; }
            set { firstTime = value; }
        }

        public int SecondTime
        {
            get { return secondTime; }
            set { secondTime = value; }
        }

        public bool UseAuto
        {
            get { return useAuto; }
            set { useAuto = value; }
        }

        public int MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }

        public int MaxValue2
        {
            get { return maxValue2; }
            set { maxValue2 = value; }
        }

        public int MaxValue3
        {
            get { return maxValue3; }
            set { maxValue3 = value; }
        }

        public int MaxValue4
        {
            get { return maxValue4; }
            set { maxValue4 = value; }
        }

        public StoredMessageText Msg
        {
            get { return msg; }
            set { msg = value; }
        }

        public StoredMessageText Msg2
        {
            get { return msg2; }
            set { msg2 = value; }
        }

        public StoredMessageText Msg3
        {
            get { return msg3; }
            set { msg3 = value; }
        }

        public StoredMessageText Msg4
        {
            get { return msg4; }
            set { msg4 = value; }
        }

        public int AutoTime
        {
            get { return autoTime; }
            set { autoTime = value; }
        }

        public bool TestOrder
        {
            get { return testOrder; }
            set { testOrder = value; }
        }
    }
}