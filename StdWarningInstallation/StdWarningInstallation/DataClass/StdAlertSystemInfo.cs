using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using GEPlugin;

namespace StdWarningInstallation.DataClass
{
    /// <summary>
    /// 표준경보시스템 정보
    /// </summary>
    public class SASInfo
    {
        /// <summary>
        /// 시스템 아이콘 URL
        /// </summary>
        private string iconURL = string.Empty;
        public string IconURL
        {
            get { return iconURL; }
            set { iconURL = value; }
        }
        /// <summary>
        /// 시스템 아이콘 Placemark Object
        /// </summary>
        private IKmlPlacemark placemark;
        public IKmlPlacemark Placemark
        {
            get { return placemark; }
            set { placemark = value; }
        }
        /// <summary>
        /// 시스템 프로필 정보
        /// </summary>
        private SASProfile profile;
        public SASProfile Profile
        {
            get { return profile; }
            set { profile = value; }
        }
    }

    /// <summary>
    /// 표준경보시스템 프로필 정보
    /// </summary>
    public class SASProfile
    {
        #region Fields
        /// <summary>
        /// 시스템 아이디(= CAP의Sender)
        /// </summary>
        private string identifier = string.Empty;
        public string ID
        {
            get { return identifier; }
            set { identifier = value; }
        }
        /// <summary>
        /// 시스템 이름
        /// </summary>
        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// 인증 코드
        /// </summary>
        private string authCode = string.Empty;
        public string AuthCode
        {
            get { return authCode; }
            set { authCode = value; }
        }
        /// <summary>
        /// 시스템 종류
        /// </summary>
        private string kindCode = string.Empty;
        public string KindCode
        {
            get { return kindCode; }
            set { kindCode = value; }
        }
        /// <summary>
        /// 아이피 종류(IPv4/IPv6)
        /// </summary>
        private IPType ipType;
        public IPType IpType
        {
            get { return ipType; }
            set { ipType = value; }
        }
        /// <summary>
        /// 아이피 주소
        /// </summary>
        private string ipAddress = string.Empty;
        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }
        /// <summary>
        /// 좌표(위도)
        /// </summary>
        private string latitude = string.Empty;
        public string Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }
        /// <summary>
        /// 좌표(경도)
        /// </summary>
        private string longitude = string.Empty;
        public string Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
        /// <summary>
        /// 표준경보시스템이 등록되어 있는 통합경보게이트웨이의 지역 코드
        /// </summary>
        private string assignedIAGWRegionCode = string.Empty;
        public string AssignedIAGWRegionCode
        {
            get { return assignedIAGWRegionCode; }
            set { assignedIAGWRegionCode = value; }
        }
        /// <summary>
        /// 표준경보시스템이 설치된 주소
        /// </summary>
        private string address = string.Empty;
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        /// <summary>
        /// 시스템 관리자 이름
        /// </summary>
        private string managerName = string.Empty;
        public string ManagerName
        {
            get { return managerName; }
            set { managerName = value; }
        }
        /// <summary>
        /// 시스템 관리자 소속
        /// </summary>
        private string managerDepartment = string.Empty;
        public string ManagerDepartment
        {
            get { return managerDepartment; }
            set { managerDepartment = value; }
        }
        /// <summary>
        /// 시스템 관리자 연락처
        /// </summary>
        private string managerPhone = string.Empty;
        public string ManagerPhone
        {
            get { return managerPhone; }
            set { managerPhone = value; }
        }
        #endregion

        #region Functions
        public bool DeepCopyFrom(SASProfile src)
        {
            if (src == null)
            {
                return false;
            }
            this.identifier = src.identifier;
            this.name = src.name;
            this.authCode = src.authCode;
            this.kindCode = src.kindCode;
            this.ipType = src.ipType;
            this.ipAddress = src.ipAddress;
            this.latitude = src.latitude;
            this.longitude = src.longitude;
            this.assignedIAGWRegionCode = src.assignedIAGWRegionCode;
            this.address = src.address;
            this.managerName = src.managerName;
            this.managerDepartment = src.managerDepartment;
            this.managerPhone = src.managerPhone;

            return true;
        }

        public override string ToString()
        {
            return this.name;
        }
        /// <summary>
        /// 해쉬키 산출
        /// </summary>
        /// <returns></returns>
        public byte[] ComputeHashKey()
        {
            System.Diagnostics.Debug.Assert(this != null);

            StringBuilder builder = new StringBuilder();
            builder.Append(this.identifier);
            builder.Append(this.name);
            builder.Append(this.authCode);
            builder.Append(this.kindCode);
            builder.Append(((int)this.ipType).ToString());
            builder.Append(this.ipAddress);
            builder.Append(this.latitude);
            builder.Append(this.longitude);
            builder.Append(this.assignedIAGWRegionCode);
            builder.Append(this.address);
            builder.Append(this.managerName);
            builder.Append(this.managerDepartment);
            builder.Append(this.managerPhone);

            byte[] data = Encoding.Default.GetBytes(builder.ToString());
            SHA1CryptoServiceProvider sh1 = new SHA1CryptoServiceProvider();
            byte[] hash = sh1.ComputeHash(data);

            return hash;
        }
        #endregion
    }

    public class SASKind
    {
        private string code;
        public string Code
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
        public void DeepCopyFrom(SASKind src)
        {
            if (src == null)
            {
                return;
            }
            this.code = src.code;
            this.name = src.name;
        }

        public SASKind()
        {
        }
        public SASKind(string code, string name)
        {
            this.code = code;
            this.name = name;
        }
    }

}
