using BookSearch.API.Models;

namespace BookSearch.API.Response;

public sealed record TokenResponse(string Token, string RefreshToken, User User);