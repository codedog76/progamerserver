﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgamerWebAPI.Models
{
    public class Level
    {
        public string level_user_student_number_id { get; set; }
        public int level_id { get; set; }
        public int level_number { get; set; }
        public string level_title { get; set; }
        public string level_description { get; set; }
        public int level_completed { get; set; }
        public int level_score { get; set; }
        public int level_attempts { get; set; }
        public int level_time { get; set; }

        public List<Puzzle> puzzle_list { get; set; }

        public Level()
        {
            puzzle_list = new List<Puzzle>();
        }

        public bool response_valid { get; set; }
        public string response_message { get; set; }
    }
}