using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StdWarningInstallation.DataClass
{
    /// <summary>
    /// Icon 생성시 필요한 정보를 가지는 Class
    /// </summary>
    public class IconInfo
    {
        #region Fields
        private string placemarkID = string.Empty;
        private string iconName = string.Empty;
        private string iconURL = string.Empty;
        private double latitude = 0.0;
        private double longitude = 0.0;
        private List<KmlExtendedData> lstExtensionData = new List<KmlExtendedData>();
        #endregion

        #region Properties
        public string PlacemarkID
        {
            get { return placemarkID; }
            set { placemarkID = value; }
        }
        public string IconName
        {
            get { return iconName; }
            set { iconName = value; }
        }
        public string IconURL
        {
            get { return iconURL; }
            set { iconURL = value; }
        }
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
        public List<KmlExtendedData> LstExtendedData
        {
            get { return lstExtensionData; }
            set { lstExtensionData = value; }
        }
        #endregion

        public IconInfo()
        {
        }
    }
}
