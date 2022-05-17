using DataBaseLayer.Users;
using ReposatoryLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReposatoryLayer.Interfaces
{
    public interface IUserRL
    {
        public void AddUser(UserModel user); //method used for Adduser
        public string LoginUser(string email, string password); //This Method Used For Login user
        public bool ForgotPassword(string email); //This method used for Forgot Password
        public bool ChangePassword(string Email,ChangePasswardModel changePassward); // method for change password    
    }
}

