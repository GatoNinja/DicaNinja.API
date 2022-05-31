using BookSearch.API.DDD.User;

namespace BookSearch.API.DDD.Token;

public sealed record TokenResponse(string Token, string RefreshToken, UserModel User);