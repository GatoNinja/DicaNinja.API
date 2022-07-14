
using DicaNinja.API.Models;

namespace DicaNinja.API.Response;

public sealed class TokenResponse
{
    public string Token { get; init; }
    public string RefreshToken { get; init; }
    public User User { get; init; }

    public TokenResponse(string token, string refreshToken, User user)
    {
        Token = token;
        RefreshToken = refreshToken;
        User = user;
    }
}
