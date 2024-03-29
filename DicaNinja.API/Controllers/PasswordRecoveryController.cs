
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
    public async Task<ActionResult> PostPasswordRecovery([FromBody] PasswordRecoveryRequest request, CancellationToken cancellation)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var recoveryCode = await PasswordRecoveryProvider.GetByEmailAndCodeAsync(request.Email, request.Code, cancellation).ConfigureAwait(false);

        if (recoveryCode is null)
        {
            var messageResponse = new MessageResponse(TextConstant.PasswordRecoveryInvalidCode);

            return new BadRequestObjectResult(messageResponse);
        }

        await UserProvider.ChangePasswordAsync(request.Email, request.NewPassword, cancellation).ConfigureAwait(false);
        await PasswordRecoveryProvider.UseRecoveryCodeAsync(recoveryCode.Id, cancellation).ConfigureAwait(false);

        return Ok();

    }
}
