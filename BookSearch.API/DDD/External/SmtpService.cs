using System.Net;
using System.Net.Mail;

namespace BookSearch.API.DDD.External;

public class SmtpService : ISmtpService
{
    public void SendEmail(string to, string subject, string body)
    {
        const string fromEmail = "ygor@ygorlazaro.com";
        var message = new MailMessage(fromEmail, to)
        {
            Subject = subject,
            Body = body
        };
        var client = new SmtpClient("smtp.zoho.com", 587)
        {
            Credentials = new NetworkCredential("ygor@ygorlazaro.com", "biX3tCqU8dVF"),
            Timeout = 10000,
            EnableSsl = true
        };

        try
        {
            client.Send(message);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception caught in CreateTestMessage2(): {0}",
                ex);
        }
    }
}