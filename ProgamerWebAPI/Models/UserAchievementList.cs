using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgamerWebAPI.Models
{
    public class UserAchievementList
    {
        public List<UserAchievement> userachievement_list { get; set; }

        public UserAchievementList()
        {
            userachievement_list = new List<UserAchievement>();
        }

        public bool response_valid { get; set; }
        public string response_message { get; set; }
    }
}