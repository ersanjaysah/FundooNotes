using BussinessLayer.Interfaces;
using DataBaseLayer.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReposatoryLayer.DBContext;
using ReposatoryLayer.Services;
using System;
using System.Linq;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        FundooContext fundooContext;
        IUserBL userBL;
        StringCipher stringCipher;
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
                
                var userdata = fundooContext.Users.FirstOrDefault(u => u.Email == email);
               string Password = StringCipher.DecodeFrom64(userdata.Password); 
                if (userdata == null)
                {
                    return this.BadRequest(new { success = false, message = $"email and password is invalid" });

                }
               
                var userdata1 = fundooContext.Users.FirstOrDefault(u => u.Email == email && Password==password);

                if (userdata1 == null)
                {
                    return this.BadRequest(new { success = false, message = $"password is invalid" });

                }

                string result = this.userBL.LoginUser(email, password);
                return this.Ok(new { success = true, message = "login successfull",Token=result });

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost("ForgotPassword/{email}")]
        public ActionResult ForgotPassword(string email)
        {
            try
            {
                var Result = this.userBL.ForgotPassword(email);
                if (Result != false)
                {
                    return this.Ok(new

                    {
                        success = true,
                        message = $"mail sent sucessfully" + $"token: {Result}"
                    });
                }
                
                return this.BadRequest(new { success = false, message = $"mail not sent" });
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [Authorize]
        [HttpPut("ChangePassword")]

        public ActionResult ChangePassword(ChangePasswardModel changePassward)
        {
            try
            {
                var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userid", StringComparison.InvariantCultureIgnoreCase));
                int UserID = Int32.Parse(userid.Value);
                var result = fundooContext.Users.Where(u => u.Userid == UserID).FirstOrDefault();
                string Email = result.Email.ToString();

                bool res = userBL.ChangePassword(Email, changePassward);//email.changepass
                if (res == false)
                {
                    return this.BadRequest(new { success = false, message = "Enter Valid Password" });
                }
                return this.Ok(new { success = true, message = "Password changed Successfully" });

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}

    



