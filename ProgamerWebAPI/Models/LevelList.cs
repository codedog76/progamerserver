using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgamerWebAPI.Models
{
    public class LevelList
    {
        public List<Level> level_list { get; set; }

        public LevelList()
        {
            level_list = new List<Level>();
        }

        public bool response_valid { get; set; }
        public string response_message { get; set; }
    }
}