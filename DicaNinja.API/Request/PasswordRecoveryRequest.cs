
using System.ComponentModel.DataAnnotations;

namespace DicaNinja.API.Request;

public record PasswordRecoveryRequest(
    [Required,MinLength(4),MaxLength(7)]
    string Code,
    [Required, EmailAddress]
    string Email,
    [Required,MinLength(4),MaxLength(48)]
    string NewPassword
);