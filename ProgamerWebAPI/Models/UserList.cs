using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProgamerWebAPI.Models
{
    public class UserList
    {
        public List<User> user_list { get; set; }

        public UserList()
        {
            user_list = new List<User>();
        }

        public bool response_valid { get; set; }
        public string response_message { get; set; }
    }
}