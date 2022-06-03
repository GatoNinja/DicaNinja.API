
using System.Security.Claims;
using BookSearch.API.Models;
using BookSearch.API.Response;

namespace BookSearch.API.Services;

public interface ITokenService
{
    Task<TokenResponse> GenerateTokenAsync(User user);

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

    string GenerateAccessToken(IEnumerable<Claim> claims);
}