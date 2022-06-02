
using BookSearch.API.Abstracts;
using BookSearch.API.Helpers;
using BookSearch.API.Repository.Interfaces;
using BookSearch.API.Request;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.Controller;

[Route("[controller]")]
public class PasswordRecoveryController : ControllerHelper
{
    public PasswordRecoveryController(IPasswordRecoveryRepository passwordRecovery, IUserRepository userRepository)
    {
        PasswordRecoveryRepository = passwordRecovery;
        UserRepository = userRepository;
    }

    private IPasswordRecoveryRepository PasswordRecoveryRepository { get; }

    private IUserRepository UserRepository { get; }

    [HttpPost]
    public async Task<ActionResult> PostPasswordRecovery([FromBody] PasswordRecoveryPayload payload)
    {
        var recoveryCode = await PasswordRecoveryRepository.GetByEmailAndCode(payload.Email, payload.Code);

        if (recoveryCode is null)
        {
            var messageResponse = new MessageResponse(TextConstant.PasswordRecoveryInvalidCode);

            return new BadRequestObjectResult(messageResponse);
        }

        await UserRepository.ChangePassword(payload.Email, payload.NewPassword);
        await PasswordRecoveryRepository.UseRecoveryCode(recoveryCode.Id);

        return Ok();

    }
}
