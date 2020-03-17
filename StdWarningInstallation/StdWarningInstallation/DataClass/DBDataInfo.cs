using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StdWarningInstallation.DataClass
{
    class DBDataInfo
    {
    }

    public class QueryResultDataInfo
    {
        private List<RowData> dataList = new List<RowData>();
        public List<RowData> DataList
        {
            get { return dataList; }
            set { dataList = value; }
        }
        public int DataCnt
        {
            get { return dataList.Count; }
        }
        private int affectedRecordCnt;
        public int AffectedRecordCnt
        {
            get { return affectedRecordCnt; }
            set { affectedRecordCnt = value; }
        }
    }

    public class RowData
    {
        private List<string> fieldDataList = new List<string>();
        public List<string> FieldDataList
        {
            get { return fieldDataList; }
            set { fieldDataList = value; }
        }
        public int FieldCnt
        {
            get { return fieldDataList.Count; }
        }
    }
    public class ColumnData
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string value;
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public ColumnData()
        {
        }
        public ColumnData(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
