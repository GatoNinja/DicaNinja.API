
using System.ComponentModel.DataAnnotations;

namespace BookSearch.API.DDD.RefreshToken;

public record RefreshTokenPayload([Required] string RefreshToken);