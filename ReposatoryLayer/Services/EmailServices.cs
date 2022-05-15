using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ReposatoryLayer.Services
{
    public class EmailServices
    {
        public static void SendMail(string email, string token)
        {
            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.EnableSsl = true;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = true;
                client.Credentials = new NetworkCredential("san1997sah@gmail.com", "sanjay12345@");
                MailMessage msgObj = new MailMessage();
                msgObj.To.Add(email);
                msgObj.From = new MailAddress("san1997sah@gmail.com");
                msgObj.Subject = "Password Reset Link";
                msgObj.Body = $"www.fundooNotes.com/reset-password/{token}";
                client.Send(msgObj);
            }
        }
    }
}   
