
using BookSearch.API.Abstracts;
using BookSearch.API.DDD.User;
using BookSearch.API.Helpers;

using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.DDD.PasswordRecovery;

[Route("[controller]")]
public class PasswordRecoveryController : ControllerHelper
{
    public PasswordRecoveryController(IPasswordRecoveryRepository passwordRecovery, IUserRepository userRepository)
    {
        PasswordRecovery = passwordRecovery;
        UserRepository = userRepository;
    }

    private IPasswordRecoveryRepository PasswordRecovery { get; }

    private IUserRepository UserRepository { get; }

    [HttpPost]
    public async Task<ActionResult> PostPasswordRecovery([FromBody] PasswordRecoveryPayload payload)
    {
        var recoveryCode = await PasswordRecovery.GetByEmailAndCode(payload.Email, payload.Code);

        if (recoveryCode is null)
        {
            var messageResponse = new MessageResponse(TextConstant.PasswordRecoveryInvalidCode);

            return new BadRequestObjectResult(messageResponse);
        }

        await UserRepository.ChangePassword(payload.Email, payload.NewPassword);
        await PasswordRecovery.UseRecoveryCode(recoveryCode.Id);

        return Ok();

    }
}