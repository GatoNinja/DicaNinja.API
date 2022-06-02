
using System.ComponentModel.DataAnnotations;

namespace BookSearch.API.Request;

public record LoginPayload(
    [Required, MinLength(4)] string Username,
    [Required] string Password
);
