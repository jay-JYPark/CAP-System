using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WmaPAlmScreen
{
    public class HeightPointContent
    {
        private List<HeightPointData> lstHeightPointData = new List<HeightPointData>();
        private string title = string.Empty;
        private DateTime lastTime = new DateTime();

        /// <summary>
        /// 관측소 정보 리스트 프로퍼티
        /// </summary>
        public List<HeightPointData> LstHeightPointData
        {
            get { return this.lstHeightPointData; }
            set { this.lstHeightPointData = value; }
        }

        /// <summary>
        /// 관측소명 리스트 프로퍼티
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        public DateTime LastTime
        {
            get { return this.lastTime; }
            set { this.lastTime = value; }
        }

        public void AddHeightPointData(HeightPointData HeightPointData)
        {
        }
    }
}