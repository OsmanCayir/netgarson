using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace netgarson.Entities
{
    public class CategoryMenuRel
    {
        public int ID { get; set; }

        public int Category_ID { get; set; }

        public int Menu_ID { get; set; }
    }
}