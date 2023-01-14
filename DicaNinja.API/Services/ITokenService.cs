

using DicaNinja.API.Models;

using DicaNinja.API.Response;

using System.Security.Claims;

namespace DicaNinja.API.Services;

public interface ITokenService
{
    Task<TokenResponse> GenerateTokenAsync(User user, CancellationToken cancellation);

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

    string GenerateAccessToken(IEnumerable<Claim> claims);
}
