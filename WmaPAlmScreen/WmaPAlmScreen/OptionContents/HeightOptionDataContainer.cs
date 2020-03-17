using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WmaPAlmScreen
{
    [Serializable]
    public class HeightOptionDataContainer
    {
        private List<HeightOptionData> lstHeightOptionData = new List<HeightOptionData>();

        public List<HeightOptionData> LstHeightOptionData
        {
            get { return lstHeightOptionData; }
            set { this.lstHeightOptionData = value; }
        }
    }
}