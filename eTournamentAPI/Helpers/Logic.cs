using System.Net;
using System.Net.Mail;
using eTournamentAPI.Models;

namespace eTournamentAPI.Helpers;

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
        var Body = string.Empty;

        foreach (var orderItem in OrderItems)
            Body += "Order items : <br/>" +
                    "Match Name : " + orderItem.Match.Name +
                    "<br/>";

        Body += "Sub total :" + ShoppingCartTotal;

        var message = new MailMessage();
        message.From = new MailAddress("info@etournament.com");
        message.To.Add(ToMail);
        message.Subject = "New order request";
        message.IsBodyHtml = true;
        message.Body = Body;

        var smtpClient = new SmtpClient();
        smtpClient.UseDefaultCredentials = false;

        smtpClient.Host = Host;
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(Username, Password);
        smtpClient.Send(message);
    }
}