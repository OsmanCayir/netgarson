﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace netgarson.Entities
{
    public class Set
    {
        public int ID { get; set; }

        public bool ShowScanNotification { get; set; }

        public bool ShowCallNotification { get; set; }

        public bool ShowCafeCommentNotification { get; set; }

        public bool ShowMenuCommentNotification { get; set; }

        public int User_ID { get; set; }
    }
}