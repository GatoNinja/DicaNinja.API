
using System.ComponentModel.DataAnnotations;

namespace BookSearch.API.DDD.SignUp
{
    public record NewUserPayload(
        [Required, MinLength(3), MaxLength(48)]
        string Username,
        [Required, MinLength(4), MaxLength(48)]
        string Password,
        [Required, MinLength(4), MaxLength(48)]
        string ConfirmPassword,
        [Required, EmailAddress]
        string Email,
        [Required, MinLength(2), MaxLength(48)]
        string Firstname,
        [Required, MinLength(2), MaxLength(48)]
        string Lastname);
}