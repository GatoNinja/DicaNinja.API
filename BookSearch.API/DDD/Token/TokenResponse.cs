namespace BookSearch.API.DDD.Token
{
    public sealed record TokenResponse(string Token, string RefreshToken, User.User User);
}