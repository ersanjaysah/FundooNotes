using BussinessLayer.Interfaces;
using DataBaseLayer.Users;
using Microsoft.AspNetCore.Mvc;
using ReposatoryLayer.DBContext;
using System;
using System.Linq;

namespace FundooNotes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        FundooContext fundooContext;
        IUserBL userBL;
        public UserController(FundooContext fundoo, IUserBL userBL)
        {
            this.fundooContext = fundoo;
            this.userBL = userBL;
        }
        [HttpPost("Register")]
        public IActionResult AddUser(UserModel user)
        {
            try
            {
                this.userBL.AddUser(user);
                return this.Ok(new { success = true, message = $"User Added Successfully" });
            }
            catch (SystemException)
            {
                throw;
            }
        }
        [HttpPost("login/{email}/{password}")]
        public IActionResult LoginUser(string email, string password)
        {
            try
            {
                var userdata = fundooContext.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
                if (userdata == null)
                {
                    return this.BadRequest(new { success = false, message = $"email and password is invalid" });

                }
                
                var result = this.userBL.LoginUser(email, password);
                return this.Ok(new { success = true, message = $"login successfull {result}" });

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}


