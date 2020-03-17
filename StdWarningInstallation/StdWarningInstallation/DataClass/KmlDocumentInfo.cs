using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GEPlugin;

namespace StdWarningInstallation.DataClass
{
    /// <summary>
    /// GroundOverlay 정보
    /// </summary>
    public class KmlDocumentInfo
    {
        #region Fields
        private string identifier = "";
        private string name = string.Empty;
        private double latitude = double.MinValue;
        private double longitude = double.MinValue;
        private double altitude = double.MinValue;
        private double tilt = double.MinValue;
        private double range = double.MinValue;
        private IKmlObject kmlObject = null;
        #endregion

        #region Properties
        /// <summary>
        /// 식별자
        /// </summary>
        public string Id
        {
            get { return identifier; }
            set { identifier = value; }
        }
        /// <summary>
        /// 명칭
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
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
        public double Altitude
        {
            get { return altitude; }
            set { altitude = value; }
        }
        public double Tilt
        {
            get { return tilt; }
            set { tilt = value; }
        }
        public double Range
        {
            get { return range; }
            set { range = value; }
        }
        /// <summary>
        /// KML Object Instance
        /// </summary>
        public IKmlObject KmlObject
        {
            get { return kmlObject; }
            set { kmlObject = value; }
        }
        #endregion

        #region Delegate
        #endregion

        #region Event
        #endregion

        public KmlDocumentInfo()
        {
        }
        public KmlDocumentInfo(string id, string name)
        {
            this.identifier = id;
            this.name = name;
        }
    }
}
