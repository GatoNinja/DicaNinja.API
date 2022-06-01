
using System.ComponentModel.DataAnnotations;

namespace BookSearch.API.DDD.PasswordRecovery;

public record PasswordRecoveryPayload(
    [Required,MinLength(4),MaxLength(7)]
    string Code,
    [Required, EmailAddress]
    string Email,
    [Required,MinLength(4),MaxLength(48)]
    string NewPassword
);