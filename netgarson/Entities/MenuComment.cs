using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace netgarson.Entities
{
    public class MenuComment
    {
        public int ID { get; set; }

        public string CommentText { get; set; }

        public int Plus { get; set; }

        public int Cons { get; set; }

        public DateTime RecordDate { get; set; }

        public bool IsNew { get; set; }

        public bool Active { get; set; }

        public int Menu_ID { get; set; }

        public int User_ID { get; set; }

        public string HelperMenuName { get; set; }

        public string HelperDateTimeRelative { get; set; }
    }
}