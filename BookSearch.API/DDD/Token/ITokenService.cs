
using System.Security.Claims;

namespace BookSearch.API.DDD.Token
{
    public interface ITokenService
    {
        Task<TokenResponse> GenerateTokenAsync(User.User userModel);

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

        string GenerateAccessToken(IEnumerable<Claim> claims);
    }
}