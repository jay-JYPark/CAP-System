using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCASBIZ.NCasUtility;

namespace WmaPAlmScreen
{
    public class DeviceStatusData
    {
        private string ipAddr = string.Empty;
        private string name = string.Empty;

        /// <summary>
        /// 장비 IP 프로퍼티
        /// </summary>
        public string IpAddr
        {
            get { return this.ipAddr; }
            set { this.ipAddr = value; }
        }

        /// <summary>
        /// 장비 IP 주소를 uint 타입으로 리턴
        /// </summary>
        public uint IpAddrToUint
        {
            get { return NCasUtilityMng.INCasCommUtility.StringIP2UintIP(this.ipAddr); }
        }

        /// <summary>
        /// 장비명 프로퍼티
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
    }
}