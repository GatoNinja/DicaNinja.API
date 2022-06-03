using BookSearch.API.Enums;
using BookSearch.API.Helpers;
using BookSearch.API.Models;
using BookSearch.API.Providers.Interfaces;
using BookSearch.API.Request;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.Controllers;

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

        var person = new Person(request.Firstname, request.Lastname);
        var user = new User(request.Username, request.Password, request.Email, person);
        var validateNewUser = UserProvider.ValidateNewUser(user);

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

        var insertedUser = await UserProvider.InsertAsync(user);

        if (insertedUser is null)
        {
            return new BadRequestObjectResult(new MessageResponse(TextConstant.ProblemToSaveRecord));
        }

        return new CreatedResult("token", insertedUser.Id);
    }
}
