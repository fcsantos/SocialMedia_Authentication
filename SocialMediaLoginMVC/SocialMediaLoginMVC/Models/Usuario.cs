using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMediaLoginMVC.Models
{
    public class Usuario
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public bool verified
        {
            get;
            set;      
        }        
    }
}