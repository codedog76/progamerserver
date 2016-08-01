using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgamerWebAPI.Models
{
    public class Achievement
    {
        public int achievement_id { get; set; }
        public string achievement_title { get; set; }
        public string achievement_description { get; set; }
        public int achievement_total { get; set; }
    }
}