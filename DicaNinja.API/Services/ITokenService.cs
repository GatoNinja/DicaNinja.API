
using System.Security.Claims;

using DicaNinja.API.Models;
using DicaNinja.API.Response;

namespace DicaNinja.API.Services;

public interface ITokenService
{
    Task<TokenResponse> GenerateTokenAsync(User user);

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

    string GenerateAccessToken(IEnumerable<Claim> claims);
}