using RestSharp;
using RestSharp.Authenticators;

namespace DicaNinja.API.Services;

public class SmtpService
{
    public RestResponse SendEmail(string to, string subject, string body)
    {
        using var client = new RestClient()
        {
            Authenticator = new HttpBasicAuthenticator("api",
                "6e2e57a23e28b9da38ef0ff8326b4e74-27a562f9-bcfc585c")
        };

        var request = new RestRequest()
        {
            Resource = "https://api.mailgun.net/v3/sandbox50fc8eab321a4b5fa6e2e83dd353ac74.mailgun.org/messages"
        };
        request.AddParameter("domain", "https://api.mailgun.net/v3/sandbox50fc8eab321a4b5fa6e2e83dd353ac74.mailgun.org", ParameterType.UrlSegment);
        request.AddParameter("from", "Dica Ninja <mailgun@sandbox50fc8eab321a4b5fa6e2e83dd353ac74.mailgun.org>");
        request.AddParameter("to", to);
        request.AddParameter("subject", "Olá!");
        request.AddParameter("text", "Seja bem vindo à Dica Ninja!");
        request.Method = Method.Post;

        return client.Execute(request);
    }
}
