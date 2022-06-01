using BookSearch.API.Abstracts;
using BookSearch.API.DDD.User;
using BookSearch.API.Helpers;

using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.DDD.Token;

[Route("token")]
public class TokenController : ControllerHelper
{
    public TokenController(IUserRepository userRepository, ITokenService tokenService)
    {
        UserRepository = userRepository;
        TokenService = tokenService;
    }

    public IUserRepository UserRepository { get; }

    public ITokenService TokenService { get; }

    [HttpPost, ProducesResponseType(StatusCodes.Status201Created), ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TokenResponse>> PostTokenAsync([FromBody] LoginPayload request)
    {
        var (username, password) = request;
        var user = await UserRepository.DoLoginAsync(username, password);

        if (user is null)
        {
            var messageResponse = new MessageResponse(TextConstant.WrongUserOrInvalidPassword);

            return NotFound(messageResponse);
        }

        var token = await TokenService.GenerateTokenAsync(user);

        return new CreatedResult("token", token);
    }
}