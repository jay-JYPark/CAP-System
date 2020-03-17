using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCASBIZ.NCasUtility;

namespace WmaPAlmScreen
{
    public class HeightPointData
    {
        private string ipAddr = string.Empty;
        private string title = string.Empty;

        /// <summary>
        /// 단말 IP 프로퍼티
        /// </summary>
        public string IpAddr
        {
            get { return this.ipAddr; }
            set { this.ipAddr = value; }
        }

        /// <summary>
        /// 단말 IP 주소를 uint 타입으로 리턴
        /// </summary>
        public uint IpAddrToUint
        {
            get { return NCasUtilityMng.INCasCommUtility.StringIP2UintIP(this.ipAddr); }
        }

        /// <summary>
        /// 장비명 프로퍼티
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }
    }
}