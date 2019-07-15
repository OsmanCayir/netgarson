using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace netgarson.Entities
{
    public class MailQueue
    {
        public int ID { get; set; }

        public string Mail { get; set; }

        public int MailState { get; set; }

        public string MailStateDescription { get; set; }

        public int ContentType { get; set; }

        public string ContentTypeDescription { get; set; }

        public DateTime RecordDate { get; set; }

        public DateTime? SendDate { get; set; }
    }
}