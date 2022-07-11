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
using eTournamentAPI.Models;
using Newtonsoft.Json;

namespace eTournamentAPI.Helpers
{
    public class Logic
    {
        public void SendCompletedOrderEmail(
            double ShoppingCartTotal,
            string ToMail,
            string Host,
            string Username,
            string Password,
            List<OrderItem> OrderItems)
        {
            string Body = string.Empty;

            foreach (var orderItem in OrderItems)
            {
                Body += "Order items : <br/>" +
                        "Match Name : " + orderItem.Match.Name +
                        "<br/>";


            }

            Body += "Sub total :" + ShoppingCartTotal;

            MailMessage message = new MailMessage();
            message.From = new MailAddress("info@etournament.com");
            message.To.Add(ToMail);
            message.Subject = "New order request";
            message.IsBodyHtml = true;
            message.Body = Body;

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.UseDefaultCredentials = false;

            smtpClient.Host = Host;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(Username, Password);
            smtpClient.Send(message);
        }
    }
}