using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgamerWebAPI.Models
{
    public class Puzzle
    {
        public int puzzle_id { get; set; }
        public int puzzle_level_id { get; set; }
        public string puzzle_type { get; set; }
        public string puzzle_instructions { get; set; }
        public string puzzle_expected_outcome { get; set; }
        public string puzzle_data { get; set; }
    }
}