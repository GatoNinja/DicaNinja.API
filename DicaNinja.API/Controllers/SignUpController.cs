
using DicaNinja.API.Enums;

using DicaNinja.API.Helpers;
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Request;

using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Controllers;

[Route("[controller]")]
public class SignUpController : ControllerBase
{
    public SignUpController(IUserProvider userProvider)
    {
        UserProvider = userProvider;
    }

    private IUserProvider UserProvider { get; }

    [HttpPost]
    public async Task<ActionResult<Guid>> PostNewUserAsync([FromBody] NewUserRequest request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            var messageResponse = new MessageResponse(TextConstant.PasswordDoesntMatch);

            return new BadRequestObjectResult(messageResponse);
        }

        var user = new User(request.Username, request.Firstname, request.Lastname, request.Email, request.Password);
        var cancellationToken = new CancellationToken();
        var validateNewUser = await UserProvider.ValidateNewUserAsync(user, cancellationToken);

        if (validateNewUser == EnumNewUserCheck.ExistingEmail)
        {
            var messageResponse = new MessageResponse(TextConstant.ExistingEmail);

            return new BadRequestObjectResult(messageResponse);
        }

        if (validateNewUser == EnumNewUserCheck.ExistingUsername)
        {
            var messageResponse = new MessageResponse(TextConstant.ExistingUsername);

            return new BadRequestObjectResult(messageResponse);
        }

        var insertedUser = await UserProvider.InsertAsync(user, cancellationToken);

        return insertedUser is null
            ? (ActionResult<Guid>)new BadRequestObjectResult(new MessageResponse(TextConstant.ProblemToSaveRecord))
            : (ActionResult<Guid>)new CreatedResult("token", insertedUser.Id);
    }
}
