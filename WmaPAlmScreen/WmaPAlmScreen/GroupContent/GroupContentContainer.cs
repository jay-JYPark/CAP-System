using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WmaPAlmScreen
{
    [Serializable]
    public class GroupContentContainer
    {
        private List<GroupContent> lstGroupContent = new List<GroupContent>();

        public List<GroupContent> LstGroupContent
        {
            get { return this.lstGroupContent; }
            set { this.lstGroupContent = value; }
        }
    }
}