
using BookSearch.API.DDD.Person;
using BookSearch.API.DDD.User;
using BookSearch.API.Helpers;

using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.DDD.SignUp;

[Route("[controller]")]
public class SignUpController : ControllerBase
{
    public SignUpController(IUserRepository userRepository)
    {
        UserRepository = userRepository;
    }

    private IUserRepository UserRepository { get; }

    [HttpPost]
    public async Task<ActionResult<UserModel>> PostNewUserAsync([FromBody] NewUserPayload request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            var messageResponse = new MessageResponse(TextConstant.PasswordDoesntMatch);

            return new BadRequestObjectResult(messageResponse);
        }

        var person = new PersonModel(request.Firstname, request.Lastname);
        var user = new UserModel(request.Username, request.Password, request.Email, person);
        var validateNewUser = UserRepository.ValidateNewUser(user);

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

        var insertedUser = await UserRepository.InsertAsync(user);

        return new CreatedResult("token", insertedUser);
    }
}