using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace netgarson.Entities
{
    public class Call
    {
        public int ID { get; set; }

        public int TableNo { get; set; }

        public DateTime RecordDate { get; set; }

        public bool IsNew { get; set; }

        public int User_ID { get; set; }

        public string HelperDateTimeRelative { get; set; }
    }
}