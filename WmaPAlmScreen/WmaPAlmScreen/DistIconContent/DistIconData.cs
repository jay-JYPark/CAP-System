using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WmaPAlmScreen
{
    public class DistIconData
    {
        private int code = 0;
        private int pointX = 0;
        private int pointY = 0;

        /// <summary>
        /// 장비 코드 프로퍼티
        /// </summary>
        public int Code
        {
            get { return this.code; }
            set { this.code = value; }
        }

        /// <summary>
        /// 시군 X 좌표
        /// </summary>
        public int X
        {
            get { return this.pointX; }
            set { this.pointX = value; }
        }

        /// <summary>
        /// 시군 Y 좌표
        /// </summary>
        public int Y
        {
            get { return this.pointY; }
            set { this.pointY = value; }
        }
    }
}