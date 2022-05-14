using DataBaseLayer.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReposatoryLayer.DBContext;
using ReposatoryLayer.Entities;
using ReposatoryLayer.Interfaces;
using System;
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
        public IConfiguration Configuration { get; set; }
        public UserRL(FundooContext fundoo, IConfiguration configuration)
        {
            this.fundoo = fundoo;
            this.Configuration = configuration;
        }
        public void AddUser(UserModel user)
        {
            try
            {
                User userdata = new User();
                userdata.FirstName = user.FirstName;
                userdata.Lastname = user.Lastname;
                userdata.CreatedDate = DateTime.Now;
                userdata.Email = user.Email;
                userdata.Password = user.Password;
                userdata.Address = user.Address;
                fundoo.Add(userdata);
                fundoo.SaveChanges();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string LoginUser(string email, string password)
        {

            try
            {
                var result = fundoo.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
                if (result == null)
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

        private string GetJWTToken(string email, object userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes("THIS_IS_MY_KEY_TO_GENERATE_TOKEN");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("email", email),
                    new Claim("Userid",userId.ToString())
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
    }
}
    

