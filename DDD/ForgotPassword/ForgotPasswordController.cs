
using BookSearch.API.Abstracts;
using BookSearch.API.DDD.External;
using BookSearch.API.DDD.PasswordRecovery;
using BookSearch.API.DDD.User;
using BookSearch.API.Helpers;

using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.DDD.ForgotPassword;

[Route("[controller]")]
public class ForgotPasswordController : ControllerHelper
{
    public ForgotPasswordController(ISmtpService smtpService, IPasswordRecoveryRepository passwordRecovery, IUserRepository userRepository)
    {
        SmtpService = smtpService;
        PasswordRecovery = passwordRecovery;
        UserRepository = userRepository;
    }

    private ISmtpService SmtpService { get; }

    private IPasswordRecoveryRepository PasswordRecovery { get; }

    private IUserRepository UserRepository { get; }

    [HttpPost]
    public async Task<ActionResult> PostForgotPasswordAsync([FromBody] ForgotPasswordPayload request)
    {
        var user = await UserRepository.GetByEmail(request.Email);

        if (user is null)
        {
            var messageResponse = new MessageResponse(TextConstant.UserNotFound);

            return new NotFoundObjectResult(messageResponse);
        }

        var passwordRecovery = new PasswordRecoveryModel(user);
        var inserted = await PasswordRecovery.InsertAsync(passwordRecovery);
        var code = inserted.Code;
        var bodyMessage = @$"Seu código de recuperação para o login é {code}";
        SmtpService.SendEmail("ygor@ygorlazaro.com", "Recupere sua senha", bodyMessage);

        return Ok(new MessageResponse("Seu e-mail de recuperação foi enviado"));
    }
}