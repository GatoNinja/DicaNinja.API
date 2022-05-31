using BookSearch.API.DDD.User;

using System.Security.Claims;

namespace BookSearch.API.DDD.Token;

public interface ITokenService
{
    Task<TokenResponse> GenerateTokenAsync(UserModel userModel);

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

    string GenerateAccessToken(IEnumerable<Claim> claims);
}