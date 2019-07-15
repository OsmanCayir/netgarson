using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace netgarson.Entities
{
    public class Menu
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public decimal Price { get; set; }

        public string ImagePath { get; set; }

        public bool ShowComment { get; set; }

        public bool Active { get; set; }

        public int User_ID { get; set; }
    }

}