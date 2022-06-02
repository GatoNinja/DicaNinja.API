
using System.ComponentModel.DataAnnotations;

namespace BookSearch.API.Request;

public record PasswordRecoveryRequest(
    [Required,MinLength(4),MaxLength(7)]
string Code,
    [Required, EmailAddress]
string Email,
    [Required,MinLength(4),MaxLength(48)]
string NewPassword
);
