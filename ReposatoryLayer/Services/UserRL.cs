using DataBaseLayer.Users;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MSMQ.Messaging;
using ReposatoryLayer.DBContext;
using ReposatoryLayer.Entities;
using ReposatoryLayer.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;




namespace ReposatoryLayer.Services
{
    public class UserRL : IUserRL
    {
        FundooContext fundoo;
        public IConfiguration Configuration { get; }


        public UserRL(FundooContext fundoo, IConfiguration configuration)
        {
            this.fundoo = fundoo;
            this.Configuration = configuration;
        }
        /// <summary>
        /// Used method of Add user
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(UserModel user)
        {
            try
            {
                User userdata = new User(); //Created instance of User class
                userdata.FirstName = user.FirstName;
                userdata.Lastname = user.Lastname;
                userdata.Email = user.Email;
                userdata.Password = StringCipher.EncodePasswordToBase64(user.Password);
                userdata.Address = user.Address;
                userdata.CreatedDate = DateTime.Now;
                fundoo.Add(userdata);
                fundoo.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// used method of Login user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string LoginUser(string email, string password)
         {

            try
            { 

                var result = fundoo.Users.Where(u => u.Email == email).FirstOrDefault();
                string pass= StringCipher.DecodeFrom64(result.Password);

                if (pass != password)
                {
                    return null;
                }
                return GetJWTToken(email, result.Userid);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetJWTToken(string email, int userID)
        {
            // generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("email", email),
                    new Claim("Userid",userID.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials =
                               new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ForgotPassword(string email)
        {
            try
            {
                var userdata = fundoo.Users.FirstOrDefault(u => u.Email == email);
                if (userdata == null)
                {
                    return false;
                }

                MessageQueue queue;
                //Add message to queue
                if (MessageQueue.Exists(@".\Private$\FundooQueue"))
                {
                    queue = new MessageQueue(@".\Private$\FundooQueue");
                }

                else
                {
                    queue = MessageQueue.Create(@".\Private$\FundooQueue");
                }

                Message message = new Message();
                message.Formatter = new BinaryMessageFormatter();
                message.Body = GetJWTToken(email, userdata.Userid);
                message.Label = "Forgot password Email";
                queue.Send(message);

                Message msg = queue.Receive();
                msg.Formatter = new BinaryMessageFormatter();
                EmailServices.SendMail(email, message.Body.ToString());
                queue.ReceiveCompleted += new ReceiveCompletedEventHandler(msmqQueue_ReceiveCompleted);

                queue.BeginReceive();
                queue.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void msmqQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {
            try
            {
                MessageQueue queue = (MessageQueue)sender;
                Message msg = queue.EndReceive(e.AsyncResult);
                EmailServices.SendMail(e.Message.ToString(), GenerateToken(e.Message.ToString()));
                queue.BeginReceive();
            }
            catch (MessageQueueException ex)
            {
                if (ex.MessageQueueErrorCode == MessageQueueErrorCode.AccessDenied)
                {
                    Console.WriteLine("Access is denied. " +
                        "Queue might be a system queue.");
                }
            }
        }

        public string GenerateToken(string email)
        {
            if (email == null)
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Email", email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),

                SigningCredentials =
                new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        //method to encode the password

        public static string EncryptPassword(string password)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                {
                    return null;

                }
                else
                {
                    byte[] b = Encoding.ASCII.GetBytes(password);
                    string encrypted = Convert.ToBase64String(b);
                    return encrypted;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public bool ChangePassword(string Email, ChangePasswardModel changePassward)
        {
            try
            {
                if (changePassward.Password.Equals(changePassward.ConfirmPassword))
                {
                    var user = fundoo.Users.Where(x => x.Email == Email).FirstOrDefault();
                    user.Password=StringCipher.EncodePasswordToBase64(changePassward.ConfirmPassword);
                    fundoo.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}




