using BookSearch.API.Models;

namespace BookSearch.API.Response.Token
{
    public sealed record TokenResponse(string Token, string RefreshToken, User User);
}
