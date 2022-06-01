namespace BookSearch.API.DDD.External
{
    public interface ISmtpService
    {
        void SendEmail(string to, string subject, string body);
    }
}