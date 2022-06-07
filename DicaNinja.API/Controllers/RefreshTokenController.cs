
using DicaNinja.API.Abstracts;
using DicaNinja.API.Helpers;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Request;
using DicaNinja.API.Response;
using DicaNinja.API.Services;

using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Controllers;

[Route("[controller]")]
public class RefreshTokenController : ControllerHelper
{
    public RefreshTokenController(IRefreshTokenProvider refreshTokenProvider, ITokenService tokenService)
    {
        RefreshTokenProvider = refreshTokenProvider;
        TokenService = tokenService;
    }

    private IRefreshTokenProvider RefreshTokenProvider { get; }

    private ITokenService TokenService { get; }

    [HttpPost]
    public async Task<ActionResult<RefreshTokenResponse>> RefreshAsync([FromBody] RefreshTokenRequest request)
    {
        if (User.Identity is null)
        {
            var messageResponse = new MessageResponse(TextConstant.RefreshTokenNull);

            return new UnauthorizedObjectResult(messageResponse);
        }

        var username = User.Identity.Name;

        if (username is null)
        {
            var messageResponse = new MessageResponse(TextConstant.UsernameNull);

            return new NotFoundObjectResult(messageResponse);
        }

        var refreshToken = await RefreshTokenProvider.GetRefreshTokenAsync(username, request.RefreshToken);

        if (refreshToken is null)
        {
            var messageResponse = new MessageResponse(TextConstant.RefreshTokenNotFound);

            return new NotFoundObjectResult(messageResponse);
        }

        if (refreshToken.RefreshTokenExpiryTime <= DateTime.Now || refreshToken.User is null)
        {
            var messageResponse = new MessageResponse(TextConstant.RefreshTokenInvalid);

            return new BadRequestObjectResult(messageResponse);
        }

        var refreshTokenResponse = await CreateRefreshTokenResponse(UserId, refreshToken.Value);

        return new OkObjectResult(refreshTokenResponse);
    }

    private async Task<RefreshTokenResponse> CreateRefreshTokenResponse(Guid userId, string value)
    {
        await RefreshTokenProvider.InvalidateAsync(value);

        var newAccessToken = TokenService.GenerateAccessToken(User.Claims);
        var newRefreshToken = RefreshTokenProvider.GenerateRefreshToken();

        await RefreshTokenProvider.SaveRefreshTokenAsync(userId, newRefreshToken);

        return new RefreshTokenResponse(newAccessToken, newRefreshToken);
    }
}
