
using DicaNinja.API.Abstracts;
using DicaNinja.API.Helpers;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Request;

using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Controllers;

[Route("[controller]")]
public class PasswordRecoveryController : ControllerHelper
{
    public PasswordRecoveryController(IPasswordRecoveryProvider passwordRecoveryProvider, IUserProvider userProvider)
    {
        PasswordRecoveryProvider = passwordRecoveryProvider;
        UserProvider = userProvider;
    }

    private IPasswordRecoveryProvider PasswordRecoveryProvider { get; }

    private IUserProvider UserProvider { get; }

    [HttpPost]
    public async Task<ActionResult> PostPasswordRecovery([FromBody] PasswordRecoveryRequest requst)
    {
        var recoveryCode = await PasswordRecoveryProvider.GetByEmailAndCode(requst.Email, requst.Code);

        if (recoveryCode is null)
        {
            var messageResponse = new MessageResponse(TextConstant.PasswordRecoveryInvalidCode);

            return new BadRequestObjectResult(messageResponse);
        }

        await UserProvider.ChangePassword(requst.Email, requst.NewPassword);
        await PasswordRecoveryProvider.UseRecoveryCode(recoveryCode.Id);

        return Ok();

    }
}
