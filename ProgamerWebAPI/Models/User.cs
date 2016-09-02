using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgamerWebAPI.Models
{
    public class User
    {
        public string user_student_number { get; set; }
        public string user_nickname { get; set; }
        public string user_password { get; set; }
        public string user_type { get; set; }
        public int user_levels_completed { get; set; }
        public int user_avatar { get; set; }
        public int user_is_private { get; set; }
        public int user_total_score { get; set; }
        public int user_total_attempts { get; set; }
        public int user_total_time { get; set; }
        public double user_average_score { get; set; }
        public double user_average_attempts { get; set; }
        public double user_average_time { get; set; }

        public bool response_valid { get; set; }
        public string response_message { get; set; }
    }
}