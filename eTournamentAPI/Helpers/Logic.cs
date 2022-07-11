using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using eTournamentAPI.Data.Enums;
using Newtonsoft.Json;

namespace eTournamentAPI.Helpers
{
    public class Logic
    {
        public void SendCompletedOrderEmail(
            string Body,
            string ToMail,
            int Port,
            string Host,
            string Username,
            string Password)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("info@etournament.com");
            message.To.Add(ToMail);
            message.Subject = "New order request";
            message.IsBodyHtml = true;
            message.Body = Body;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.UseDefaultCredentials = true;

            smtpClient.Host = Host;
            smtpClient.Port = Port;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new System.Net.NetworkCredential(Username, Password);
            smtpClient.Send(message);
        }
    }
}