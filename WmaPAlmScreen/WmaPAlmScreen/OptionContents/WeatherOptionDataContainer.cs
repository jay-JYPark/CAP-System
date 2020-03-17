using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WmaPAlmScreen
{
    [Serializable]
    public class WeatherOptionDataContainer
    {
        private List<WeatherOptionData> lstWeatherOptionData = new List<WeatherOptionData>();

        public List<WeatherOptionData> LstWeatherOptionData
        {
            get { return lstWeatherOptionData; }
            set { this.lstWeatherOptionData = value; }
        }
    }
}