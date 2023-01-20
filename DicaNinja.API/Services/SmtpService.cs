using SendGrid.Helpers.Mail;
using SendGrid;

namespace DicaNinja.API.Services;

public class SmtpService
{
    public async Task<SendGrid.Response> SendRecoveryEmailAsync(string to, string code)
    {
        const string templateId = "d-ad5d2ea662d641328cc2abfd8212108b";

        var client = new SendGridClient("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");

        var substitutions = new Dictionary<string, string>
        {
            {"email", to },
            { "code", code }
        };

        var msg = new SendGridMessage();
        msg.SetFrom(new EmailAddress("ninja@dicaninja.com.br", "Dica Ninja"));
        msg.AddTo(new EmailAddress(to));
        msg.SetTemplateId(templateId);
        msg.AddSubstitutions(substitutions);

        var response = await client.SendEmailAsync(msg);

        return response;
    }
}
