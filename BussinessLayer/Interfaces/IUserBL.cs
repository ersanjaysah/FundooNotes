using DataBaseLayer.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace BussinessLayer.Interfaces
{
    public interface IUserBL
    {
        public void AddUser(UserModel user);//Add method
        public string LoginUser(string email, string password);//login method

        public bool ForgotPassword(string email); //This method used for Forgot Password

        public bool ChangePassword(string Email,ChangePasswardModel changePassward); // method for change password 
    }
}
