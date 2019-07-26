using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace netgarson.Entities
{
    public class Set
    {
        public int ID { get; set; }//sil

        public bool ShowScanNotification { get; set; }

        public bool ShowCallNotification { get; set; }

        public bool ShowCafeCommentNotification { get; set; }

        public bool ShowMenuCommentNotification { get; set; }

        public string ScanChartType { get; set; }

        public int ScanChartMonth { get; set; }

        public int ScanChartYear { get; set; }

        public string CallChartType { get; set; }

        public int CallChartMonth { get; set; }

        public int CallChartYear { get; set; }

        public int User_ID { get; set; }
    }
}