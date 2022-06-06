
using System.ComponentModel.DataAnnotations;

namespace DicaNinja.API.Request;

public record NewUserRequest(
    [Required, MinLength(3), MaxLength(48)]
    string Username,
    [Required, MinLength(4), MaxLength(48)]
    string Password,
    [Required, MinLength(4), MaxLength(48)]
    string ConfirmPassword,
    [Required, EmailAddress(ErrorMessage = "O endereço de e-mail é inválido")]
    string Email,
    [Required, MinLength(2), MaxLength(48)]
    string Firstname,
    [Required, MinLength(2), MaxLength(48)]
    string Lastname);
