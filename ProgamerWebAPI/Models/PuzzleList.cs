using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgamerWebAPI.Models
{
    public class PuzzleList
    {
        public List<Puzzle> puzzle_list { get; set; }

        public PuzzleList()
        {
            puzzle_list = new List<Puzzle>();
        }

        public bool response_valid { get; set; }
        public string response_message { get; set; }
    }
}