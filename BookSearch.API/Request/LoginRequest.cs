
using System.ComponentModel.DataAnnotations;

namespace BookSearch.API.Request;

public record LoginRequest(
    [Required, MinLength(4)] string Username,
    [Required] string Password
);