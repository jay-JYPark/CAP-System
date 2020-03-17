using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCasAppCommon.Type;

namespace WmaPAlmScreen
{
    [Serializable]
    public class KeyDataContainer
    {
        private List<NCasKeyData> lstKeyData = new List<NCasKeyData>();

        public List<NCasKeyData> LstKeyData
        {
            get { return lstKeyData; }
        }
    }
}