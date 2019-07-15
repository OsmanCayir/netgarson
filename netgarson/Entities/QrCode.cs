using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace netgarson.Entities
{
    public class QrCode
    {
        public int ID { get; set; }

        public int TableNo { get; set; }

        public int ScanCount { get; set; }

        public int CallCount { get; set; }

        public bool Active { get; set; }

        public int User_ID { get; set; }
    }
}