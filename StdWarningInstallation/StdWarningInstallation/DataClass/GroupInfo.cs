using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StdWarningInstallation.DataClass
{
    class GroupInfo
    {
    }

    public class GroupProfile
    {
        private string groupID;
        public string GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        }
        private string groupName;
        public string Name
        {
            get { return groupName; }
            set { groupName = value; }
        }
        private GroupTypeCodes groupType;
        public GroupTypeCodes GroupType
        {
            get { return groupType; }
            set { groupType = value; }
        }
        private int disasterCategoryID;
        public int DisasterCategoryID
        {
            get { return disasterCategoryID; }
            set { disasterCategoryID = value; }
        }
        private string disasterKindCode;
        public string DisasterKindCode
        {
            get { return disasterKindCode; }
            set { disasterKindCode = value; }
        }
        /// <summary>
        /// 발령 대상 정보.<br></br>
        /// 1. 그룹 종류가 지역그룹인 경우에는, 행정구역코드 목록
        /// 2. 그룹 종류가 시스템그룹인 경우에는, 표준경보시스템 아이디 목록
        /// </summary>
        private List<string> targets;
        public List<string> Targets
        {
            get { return targets; }
            set { targets = value; }
        }
        private List<string> targetSystemKinds;
        public List<string> TargetSystemKinds
        {
            get { return targetSystemKinds; }
            set { targetSystemKinds = value; }
        }

        public void DeepCopyFrom(GroupProfile src)
        {
            this.groupID = src.groupID;
            this.groupName = src.groupName;
            this.groupType = src.groupType;
            this.disasterCategoryID = src.disasterCategoryID;
            this.disasterKindCode = src.disasterKindCode;
            this.targets = null;
            if (src.targets != null)
            {
                this.targets = new List<string>();
                foreach (string target in src.targets)
                {
                    string copy = target;
                    this.targets.Add(copy);
                }
            }
            this.targetSystemKinds = null;
            if (src.targetSystemKinds != null)
            {
                this.targetSystemKinds = new List<string>();
                foreach (string kind in src.targetSystemKinds)
                {
                    string copy = kind;
                    this.targetSystemKinds.Add(copy);
                }
            }
        }
        public string GetTargetsString()
        {
            string targetStr = ListToString(this.targets);
            return targetStr;
        }
        public string GetTargetSystemKindsString()
        {
            string kindStr = ListToString(this.targetSystemKinds);
            return kindStr;
        }
        private string ListToString(List<string> list)
        {
            if (list == null || list.Count <= 0)
            {
                return string.Empty;
            }

            string result = string.Join(",", list.ToArray());
            return result;
        }
    }

    public class SASFilter
    {
        private string region1Code;
        public string Region1Code
        {
            get { return region1Code; }
            set { region1Code = value; }
        }
        private string region2Code;
        public string Region2Code
        {
            get { return region2Code; }
            set { region2Code = value; }
        }
        private string systemKindCode;
        public string SystemKindCode
        {
            get { return systemKindCode; }
            set { systemKindCode = value; }
        }

        private void DeepCopyFrom(SASFilter src)
        {
            if (src == null)
            {
                return;
            }

            this.region1Code = src.region1Code;
            this.region2Code = src.region2Code;
            this.systemKindCode = src.systemKindCode;
        }
    }

}
