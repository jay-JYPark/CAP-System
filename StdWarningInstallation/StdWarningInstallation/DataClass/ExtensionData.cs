using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StdWarningInstallation.DataClass
{
    /// <summary>
    /// KmlExtendedData 정보
    /// </summary>
    public class KmlExtendedData
    {
        #region Fields
        private string dataName = string.Empty;
        private string data = string.Empty;
        #endregion

        #region Properties
        public string DataName
        {
            get { return dataName; }
            set { dataName = value; }
        }
        public string Data
        {
            get { return data; }
            set { data = value; }
        }
        #endregion

        public KmlExtendedData()
        {
        }
    }
}
