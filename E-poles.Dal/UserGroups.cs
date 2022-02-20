using System;
using System.Collections.Generic;
using System.Text;

namespace E_poles.Dal
{
    public class UserGroups
    {
        public int UserId { get; set; }
        public User Users { get; set; }
        public int GroupsId { get; set; }
        public Groups Groups { get; set; }
    }
}
