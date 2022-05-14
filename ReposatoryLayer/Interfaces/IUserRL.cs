using DataBaseLayer.Users;
using ReposatoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReposatoryLayer.Interfaces
{
    public interface IUserRL
    {
        public void AddUser(UserModel user);
        public string LoginUser(string email, string password);
    }
}
