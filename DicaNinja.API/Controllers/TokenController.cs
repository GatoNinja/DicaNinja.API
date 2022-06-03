
using DicaNinja.API.Abstracts;
using DicaNinja.API.Helpers;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Request;
using DicaNinja.API.Response;
using DicaNinja.API.Services;

using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Controllers;

[Route("token")]
public class TokenController : ControllerHelper
{
    public TokenController(IUserProvider userProvider, ITokenService tokenService)
    {
        this.UserProvider = userProvider;
        this.TokenService = tokenService;
    }

    private IUserProvider UserProvider { get; }

    private ITokenService TokenService { get; }

    [HttpPost, ProducesResponseType(StatusCodes.Status201Created), ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TokenResponse>> PostTokenAsync([FromBody] LoginRequest request)
    {
        var (username, password) = request;
        var user = await this.UserProvider.DoLoginAsync(username, password);

        if (user is null)
        {
            var messageResponse = new MessageResponse(TextConstant.WrongUserOrInvalidPassword);

            return this.NotFound(messageResponse);
        }

        var token = await this.TokenService.GenerateTokenAsync(user);

        return new CreatedResult("token", token);
    }
}
