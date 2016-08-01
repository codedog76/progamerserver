using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgamerWebAPI.Models
{
    public class AchievementList
    {
        public List<Achievement> achievement_list { get; set; }

        public AchievementList()
        {
            achievement_list = new List<Achievement>();
        }

        public bool response_valid { get; set; }
        public string response_message { get; set; }
    }
}