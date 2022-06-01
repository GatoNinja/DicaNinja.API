
using System.ComponentModel.DataAnnotations;

namespace BookSearch.API.DDD.Token
{
    public record LoginPayload(
        [Required, MinLength(4)] string Username,
        [Required] string Password
    );
}