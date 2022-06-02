
using System.ComponentModel.DataAnnotations;

namespace BookSearch.API.Request;

public record RefreshTokenRequest([Required] string RefreshToken);
