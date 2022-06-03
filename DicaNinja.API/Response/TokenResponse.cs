
using DicaNinja.API.Models;

namespace DicaNinja.API.Response;

public sealed record TokenResponse(string Token, string RefreshToken, User User);