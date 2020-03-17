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
    public class WeatherKindData
    {
        public enum WeatherKind
        {
            None = 0,
            galeWatch = 1,
            galeAlarm = 2,
            downpourWatch = 3,
            downpourAlarm = 4,
            coldwaveWatch = 5,
            coldwaveAlarm = 6,
            dryWatch = 7,
            dryAlarm = 8,
            tsunamiWatch = 9,
            tsunamiAlarm = 10,
            heavyseasWatch = 11,
            heavyseasAlarm = 12,
            stormWatch = 13,
            stormAlarm = 14,
            heavysnowWatch = 15,
            heavysnowAlarm = 16,
            sandstormWatch = 17,
            sandstormAlarm = 18
        }

        private WeatherKind eWeatherKind = WeatherKind.None;
        private StoredMessageText stoMsg = new StoredMessageText();
        private DateTime lastTime = new DateTime();

        public WeatherKind EWeatherKind
        {
            get { return this.eWeatherKind; }
            set { this.eWeatherKind = value; }
        }

        public StoredMessageText StoMsg
        {
            get { return this.stoMsg; }
            set { this.stoMsg = value; }
        }

        public DateTime LastTime
        {
            get { return this.lastTime; }
            set { this.lastTime = value; }
        }
    }
}