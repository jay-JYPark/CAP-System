using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WmaPAlmScreen
{
    [Serializable]
    public class DistIconDataContainer
    {
        private List<DistIconData> lstDistIconData = new List<DistIconData>();

        public List<DistIconData> LstDistIconData
        {
            get { return lstDistIconData; }
            set { this.lstDistIconData = value; }
        }
    }
}