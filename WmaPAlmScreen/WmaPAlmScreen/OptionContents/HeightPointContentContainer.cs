using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WmaPAlmScreen
{
    [Serializable]
    public class HeightPointContentContainer
    {
        private List<HeightPointContent> lstHeightPointContent = new List<HeightPointContent>();

        public List<HeightPointContent> LstHeightPointContent
        {
            get { return this.lstHeightPointContent; }
            set { this.lstHeightPointContent = value; }
        }
    }
}