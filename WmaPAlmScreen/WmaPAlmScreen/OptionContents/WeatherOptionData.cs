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
    public class WeatherOptionData
    {
        private bool useTime = false;
        private int firstTime = 0;
        private int secondTime = 1;
        private bool useAuto = false;
        private int autoTime = 0;
        private List<WeatherKindData> weatherKindMsg = new List<WeatherKindData>();
        private List<string> areaCode = new List<string>();
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

        public List<WeatherKindData> WeatherKindMsg
        {
            get { return weatherKindMsg; }
            set { weatherKindMsg = value; }
        }

        public int AutoTime
        {
            get { return autoTime; }
            set { autoTime = value; }
        }

        public List<string> LstAreaCode
        {
            get { return areaCode; }
            set { areaCode = value; }
        }

        public bool TestOrder
        {
            get { return testOrder; }
            set { testOrder = value; }
        }
    }
}