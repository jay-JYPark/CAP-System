using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StdWarningInstallation.DataClass
{
    class InquiryConditions
    {
    }

    public class OrderInquiryCondition
    {
        private bool usePeriod = false;
        public bool UsePeriod
        {
            get { return usePeriod; }
            set { usePeriod = value; }
        }
        private DateTime startTime;
        public DateTime StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        private DateTime endTime;
        public DateTime EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }
        private int count = 0;
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        private OrderLocationKindInfo orderLocationKind;
        public OrderLocationKindInfo OrderLocationKind
        {
            get { return orderLocationKind; }
            set { orderLocationKind = value; }
        }
        private OrderMode orderMode;
        public OrderMode OrderMode
        {
            get { return orderMode; }
            set { orderMode = value; }
        }
        private Disaster disaster;
        public Disaster Disaster
        {
            get { return disaster; }
            set { disaster = value; }
        }
    }
}
