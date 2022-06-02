
using System.ComponentModel.DataAnnotations;

namespace BookSearch.API.Request;

public record RefreshTokenPayload([Required] string RefreshToken);
