
using System.ComponentModel.DataAnnotations;

namespace DicaNinja.API.Request;

public record LoginRequest(
    [Required, MinLength(4)] string Username,
    [Required] string Password
);