using System;
using System.Collections.Generic;
using System.Text;

namespace DataBaseLayer.Users
{
    public class UserModel
    {
        public int Userid { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
       

    }
}
