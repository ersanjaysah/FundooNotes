using DataBaseLayer.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Interfaces
{
    public interface IUserBL
    {
        public void AddUser(UserModel user); //method
        public string LoginUser(string email, string password);
    }
}
