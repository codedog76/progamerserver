using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgamerWebAPI.Models
{
    public class UserAchievement
    {
        public int userachievement_id { get; set; }
        public string user_student_number { get; set; }
        public int achievement_id { get; set; }
        public int userachievement_progress { get; set; }
        public int userachievement_completed { get; set; }
        public int userachievement_notified { get; set; }
        public string userachievement_date_completed { get; set; }
    }
}