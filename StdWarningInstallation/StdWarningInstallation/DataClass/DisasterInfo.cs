using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StdWarningInstallation.DataClass
{
    public class DisasterInfo
    {
        #region Proeprties
        private DisasterCategory category;
        public DisasterCategory Category
        {
          get { return category; }
          set { category = value; }
        }
        private List<DisasterKind> kindList;
        public List<DisasterKind> KindList
        {
          get { return kindList; }
          set { kindList = value; }
        }
        #endregion

        public DisasterInfo()
        {
            this.category = new DisasterCategory();
            this.kindList = new List<DisasterKind>();
        }
        public DisasterInfo(DisasterCategory category, List<DisasterKind> kindList)
        {
            this.category = category;
            this.kindList = kindList;
        }

        public override string ToString()
        {
            return this.category.Name;
        }
        public void DeepCopyFrom(DisasterInfo src)
        {
            this.category = null;
            this.kindList = null;

            if (src == null)
            {
                return;
            }
            if (src.category != null)
            {
                this.category = new DisasterCategory();
                this.category.DeepCopyFrom(src.category);
            }
            if (src.kindList == null)
            {
                this.kindList = null;
                return;
            }
            this.kindList = new List<DisasterKind>();
            foreach (DisasterKind kind in src.kindList)
            {
                DisasterKind copy = new DisasterKind();
                copy.DeepCopyFrom(kind);

                this.kindList.Add(copy);
            }
        }
        public string GetCodeName(Enum code)
        {
            string name = null;
            for (int index = 0; index < this.kindList.Count; index++)
            {
                if (kindList[index].Code.Equals(code))
                {
                    name = kindList[index].Name;
                    break;
                }
            }
            return name;
        }
    }
    public class Disaster
    {
        #region Proeprties
        private DisasterCategory category;
        public DisasterCategory Category
        {
            get { return category; }
            set { category = value; }
        }
        private DisasterKind kind;
        public DisasterKind Kind
        {
            get { return kind; }
            set { kind = value; }
        }
        #endregion

        public void DeepCopyFrom(Disaster src)
        {
            this.category.DeepCopyFrom(src.category);
            this.kind.DeepCopyFrom(src.kind);
        }

        public bool Initialize()
        {
            if (this == null)
            {
                return false;
            }

            if (this.Category == null)
            {
                this.Category = new DisasterCategory();
            }
            if (this.Kind == null)
            {
                this.Kind = new DisasterKind();
            }

            return true;
        }

        public override string ToString()
        {
            return this.kind.Name;
        }

        public Disaster()
        {
        }
        public Disaster(DisasterCategory category, DisasterKind kind)
        {
            this.category = category;
            this.kind = kind;
        }
    }

    public class DisasterCategory
    {
        private int identifier = 0;
        public int ID
        {
            get { return identifier; }
            set { identifier = value; }
        }

        private string code = string.Empty;
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public void DeepCopyFrom(DisasterCategory src)
        {
            this.identifier = src.identifier;
            this.code = src.code;
            this.name = src.name;
        }

        public bool SetDefault()
        {
            if (this == null)
            {
                return false;
            }
            this.identifier = 0;
            this.code = string.Empty;
            this.name = string.Empty;

            return true;
        }

        public override string ToString()
        {
            return name;
        }
        public DisasterCategory()
        {
        }
        public DisasterCategory(int inID, string inCode, string inName)
        {
            this.identifier = inID;
            this.code = inCode;
            this.name = inName;
        }
    }

    public class DisasterKind
    {
        private int categoryID = 0;
        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }

        private string code = string.Empty;
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        private string name = string.Empty;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public void DeepCopyFrom(DisasterKind src)
        {
            this.categoryID = src.categoryID;
            this.code = src.code;
            this.name = src.name;
        }

        public override string ToString()
        {
            return name;
        }
        public DisasterKind()
        {
        }
        public DisasterKind(int inCategoryID, string inCode, string inName)
        {
            this.categoryID = inCategoryID;
            this.code = inCode;
            this.name = inName;
        }
    }
}