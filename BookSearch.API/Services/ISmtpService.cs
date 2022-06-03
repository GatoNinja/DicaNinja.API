namespace BookSearch.API.Services;

public interface ISmtpService
{
    void SendEmail(string to, string subject, string body);
}