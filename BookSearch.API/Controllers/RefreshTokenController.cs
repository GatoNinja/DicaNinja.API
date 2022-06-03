using BookSearch.API.Abstracts;
using BookSearch.API.Helpers;
using BookSearch.API.Repository.Interfaces;
using BookSearch.API.Request;
using BookSearch.API.Response;
using BookSearch.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.Controllers;

[Route("[controller]")]
public class RefreshTokenController : ControllerHelper
{
    public RefreshTokenController(IRefreshTokenRepository refreshTokenRepository, ITokenService tokenService)
    {
        RefreshTokenRepository = refreshTokenRepository;
        TokenService = tokenService;
    }

    private IRefreshTokenRepository RefreshTokenRepository { get; }

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

        var refreshToken = await RefreshTokenRepository.GetRefreshTokenAsync(username, request.RefreshToken);

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
        await RefreshTokenRepository.InvalidateAsync(value);

        var newAccessToken = TokenService.GenerateAccessToken(User.Claims);
        var newRefreshToken = RefreshTokenRepository.GenerateRefreshToken();

        await RefreshTokenRepository.SaveRefreshTokenAsync(userId, newRefreshToken);

        return new RefreshTokenResponse(newAccessToken, newRefreshToken);
    }
}