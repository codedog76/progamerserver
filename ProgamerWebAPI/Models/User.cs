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
        public int user_avatar { get; set; }
        public int user_is_private { get; set; }
        public int user_overall_score { get; set; }
        public int user_overall_attempts { get; set; }
        public int user_overall_time { get; set; }

        public bool response_valid { get; set; }
        public string response_message { get; set; }
    }
}