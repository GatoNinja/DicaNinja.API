
using System.Web.Http.Results;

using DicaNinja.API.Abstracts;

using DicaNinja.API.Helpers;
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Request;
using DicaNinja.API.Services;

using Microsoft.AspNetCore.Http.HttpResults;
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
    public async Task<ActionResult> PostForgotPasswordAsync([FromBody] ForgotPasswordRequest request, CancellationToken cancellation)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var user = await UserProvider.GetByEmailAsync(request.Email, cancellation).ConfigureAwait(false);

        if (user is null)
        {
            var messageResponse = new MessageResponse(TextConstant.UserNotFound);

            return new NotFoundObjectResult(messageResponse);
        }

        var passwordRecovery = new PasswordRecovery(user.Id);
        var inserted = await PasswordRecoveryProvider.InsertAsync(passwordRecovery, cancellation).ConfigureAwait(false);
        var code = inserted.Code;
        var response = await SmtpService.SendRecoveryEmailAsync(user.Email, code);
        var errorMessage = "Ocorreu um problema ao enviar o e-mail. Tente mais tarde, se o problema persistir entre em contato em ninja@dicaninja.com.br";
        var successMessage = "Seu e-mail de recuperação foi enviado";

        return response.IsSuccessStatusCode
            ? Ok(new MessageResponse(successMessage))
            : StatusCode(StatusCodes.Status500InternalServerError, errorMessage);
    }
}
