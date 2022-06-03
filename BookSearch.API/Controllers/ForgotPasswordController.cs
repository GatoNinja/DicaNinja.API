using BookSearch.API.Abstracts;
using BookSearch.API.Helpers;
using BookSearch.API.Models;
using BookSearch.API.Providers.Interfaces;
using BookSearch.API.Request;
using BookSearch.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.Controllers;

[Route("[controller]")]
public class ForgotPasswordController : ControllerHelper
{
    public ForgotPasswordController(ISmtpService smtpService, IPasswordRecoveryProvider passwordRecoveryProvider, IUserProvider userProvider)
    {
        SmtpService = smtpService;
        PasswordRecoveryProvider = passwordRecoveryProvider;
        UserProvider = userProvider;
    }

    private ISmtpService SmtpService { get; }

    private IPasswordRecoveryProvider PasswordRecoveryProvider { get; }

    private IUserProvider UserProvider { get; }

    [HttpPost]
    public async Task<ActionResult> PostForgotPasswordAsync([FromBody] ForgotPasswordRequest request)
    {
        var user = await UserProvider.GetByEmail(request.Email);

        if (user is null)
        {
            var messageResponse = new MessageResponse(TextConstant.UserNotFound);

            return new NotFoundObjectResult(messageResponse);
        }

        var passwordRecovery = new PasswordRecovery(user);
        var inserted = await PasswordRecoveryProvider.InsertAsync(passwordRecovery);
        var code = inserted.Code;
        var bodyMessage = @$"Seu código de recuperação para o login é {code}";
        SmtpService.SendEmail("ygor@ygorlazaro.com", "Recupere sua senha", bodyMessage);

        return Ok(new MessageResponse("Seu e-mail de recuperação foi enviado"));
    }
}
