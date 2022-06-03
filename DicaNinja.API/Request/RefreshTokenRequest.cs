
using System.ComponentModel.DataAnnotations;

namespace DicaNinja.API.Request;

public record RefreshTokenRequest([Required] string RefreshToken);