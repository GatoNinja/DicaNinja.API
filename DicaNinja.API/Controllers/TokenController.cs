
using DicaNinja.API.Abstracts;
using DicaNinja.API.Helpers;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Request;
using DicaNinja.API.Response;
using DicaNinja.API.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DicaNinja.API.Controllers;

[Route("token")]
[EnableRateLimiting("token")]
public class TokenController : ControllerHelper
{
    public TokenController(IUserProvider userProvider, ITokenService tokenService)
    {
        UserProvider = userProvider;
        TokenService = tokenService;
    }

    private IUserProvider UserProvider { get; }

    private ITokenService TokenService { get; }

    [HttpPost, ProducesResponseType(StatusCodes.Status201Created), ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TokenResponse>> PostTokenAsync([FromBody] LoginRequest request, CancellationToken cancellation)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var user = await UserProvider.DoLoginAsync(request.Username, request.Password, cancellation).ConfigureAwait(false);

        if (user is null)
        {
            var messageResponse = new MessageResponse(TextConstant.WrongUserOrInvalidPassword);

            return NotFound(messageResponse);
        }

        var token = await TokenService.GenerateTokenAsync(user, cancellation).ConfigureAwait(false);

        return new CreatedResult("token", token);
    }
}
