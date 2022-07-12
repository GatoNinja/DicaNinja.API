
using DicaNinja.API.Abstracts;

using DicaNinja.API.Helpers;
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Request;
using DicaNinja.API.Services;

using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Controllers;

[Route("[controller]")]
public class ForgotPasswordController : ControllerHelper
{
    public ForgotPasswordController(SmtpService smtpService, IPasswordRecoveryProvider passwordRecoveryProvider, IUserProvider userProvider)
    {
        SmtpService = smtpService;
        PasswordRecoveryProvider = passwordRecoveryProvider;
        UserProvider = userProvider;
    }

    private SmtpService SmtpService { get; }

    private IPasswordRecoveryProvider PasswordRecoveryProvider { get; }

    private IUserProvider UserProvider { get; }

    [HttpPost]
    public async Task<ActionResult> PostForgotPasswordAsync([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await UserProvider.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
        {
            var messageResponse = new MessageResponse(TextConstant.UserNotFound);

            return new NotFoundObjectResult(messageResponse);
        }

        var passwordRecovery = new PasswordRecovery(user);
        var inserted = await PasswordRecoveryProvider.InsertAsync(passwordRecovery, cancellationToken);
        var code = inserted.Code;
        var bodyMessage = @$"Seu código de recuperação para o login é {code}";
        SmtpService.SendEmail("ygor@ygorlazaro.com", "Recupere sua senha", bodyMessage);

        return Ok(new MessageResponse("Seu e-mail de recuperação foi enviado"));
    }
}
