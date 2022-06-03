using BookSearch.API.Abstracts;
using BookSearch.API.Helpers;
using BookSearch.API.Providers.Interfaces;
using BookSearch.API.Request;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.Controllers;

[Route("[controller]")]
public class PasswordRecoveryController : ControllerHelper
{
    public PasswordRecoveryController(IPasswordRecoveryProvider passwordRecoveryProvider, IUserProvider userProvider)
    {
        this.PasswordRecoveryProvider = passwordRecoveryProvider;
        this.UserProvider = userProvider;
    }

    private IPasswordRecoveryProvider PasswordRecoveryProvider { get; }

    private IUserProvider UserProvider { get; }

    [HttpPost]
    public async Task<ActionResult> PostPasswordRecovery([FromBody] PasswordRecoveryRequest requst)
    {
        var recoveryCode = await this.PasswordRecoveryProvider.GetByEmailAndCode(requst.Email, requst.Code);

        if (recoveryCode is null)
        {
            var messageResponse = new MessageResponse(TextConstant.PasswordRecoveryInvalidCode);

            return new BadRequestObjectResult(messageResponse);
        }

        await this.UserProvider.ChangePassword(requst.Email, requst.NewPassword);
        await this.PasswordRecoveryProvider.UseRecoveryCode(recoveryCode.Id);

        return this.Ok();

    }
}
