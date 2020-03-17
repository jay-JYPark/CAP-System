using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StdWarningInstallation.DataClass
{
    public class RegionInfo
    {
        private RegionCodeVersion version;
        public RegionCodeVersion Version
        {
            get { return version; }
            set { version = value; }
        }

        Dictionary<string, RegionProfile> lstRegion;
        public Dictionary<string, RegionProfile> LstRegion
        {
            get { return lstRegion; }
            set { lstRegion = value; }
        }

        public RegionInfo()
        {
            this.version = new RegionCodeVersion();
            this.lstRegion = new Dictionary<string, RegionProfile>();
        }
    }

    public class RegionProfile
    {
        #region Fields
        /// <summary>
        /// 지역 코드.
        /// </summary>
        private string regionCode;

        /// <summary>
        /// 지역 명칭.
        /// </summary>
        private string regionName;

        /// <summary>
        /// 지역의 상대적인 수준.
        /// </summary>
        private RelativeRegionLevel regionLevel;

        /// <summary>
        /// KML 문자열 데이터. 데이터베이스에 저장된 데이터를 읽어들여 설정.
        /// </summary>
        private string kmlText;

        /// <summary>
        /// KML 클래스 형 데이터. 위의 kmlText 데이터를 변환/편집한 결과.
        /// </summary>
        private KmlDocumentInfo kmlInfo;

        /// <summary>
        /// 하위 지역 정보.
        /// </summary>
        private Dictionary<string, RegionProfile> lstSubRegion;
        #endregion

        #region Properties
        public string Code
        {
            get { return regionCode; }
            set { regionCode = value; }
        }
        public string CodePart1
        {
            get { return regionCode.Substring(0, 2); }
        }
        public string CodePart2
        {
            get { return regionCode.Substring(2, 3); }
        }
        public string CodePart3
        {
            get { return regionCode.Substring(5, 5); }
        }

        public string Name
        {
            get { return regionName; }
            set { regionName = value; }
        }

        public RelativeRegionLevel RegionLevel
        {
            get { return regionLevel; }
            set { regionLevel = value; }
        }

        public string KmlText
        {
            get { return kmlText; }
            set { kmlText = value; }
        }
        public KmlDocumentInfo KmlInfo
        {
          get { return kmlInfo; }
          set { kmlInfo = value; }
        }
        #endregion

        public Dictionary<string, RegionProfile> LstSubRegion
        {
            get { return lstSubRegion; }
            set { lstSubRegion = value; }
        }

        public override string ToString()
        {
            return this.Name;
        }
        public RegionProfile()
        {
            this.regionCode = string.Empty;
            this.regionName = string.Empty;
            //kmlInfo
            this.lstSubRegion = new Dictionary<string, RegionProfile>();
        }
    }

    public class RegionCode
    {
        private string level1;
        public string Level1
        {
            get { return level1; }
            set { level1 = value; }
        }
        private string level2;
        public string Level2
        {
            get { return level2; }
            set { level2 = value; }
        }
        private string level3;
        public string Level3
        {
            get { return level3; }
            set { level3 = value; }
        }

        public override string ToString()
        {
            return level1 + level2 + level3;
        }
        public RegionCode()
        {
        }
        public RegionCode(string fullCode)
        {
            level1 = fullCode.Substring(0, 2);
            level2 = fullCode.Substring(2, 3);
            level3 = fullCode.Substring(5, 5);
        }
    }

    public class RegionDefinition
    {
        private RegionCode code;
        public RegionCode Code
        {
            get { return code; }
            set { code = value; }
        }
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public override string ToString()
        {
            return name;
        }

        public RegionDefinition()
        {
        }
        public RegionDefinition(RegionCode code, string name)
        {
            this.code = code;
            this.name = name;
        }
        public RegionDefinition(string regionCode, string name)
        {
            this.code = new RegionCode(regionCode);
            this.name = name;
        }
    }


    public class RegionCodeVersion
    {
        private string identifier = string.Empty;
        public string ID
        {
            get { return identifier; }
            set { identifier = value; }
        }
        private string info = string.Empty;
        public string Information
        {
            get { return info; }
            set { info = value; }
        }

        public RegionCodeVersion()
        {
        }
    }
}
