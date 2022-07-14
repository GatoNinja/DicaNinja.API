
using System.ComponentModel.DataAnnotations;

namespace DicaNinja.API.Request;

public class PasswordRecoveryRequest
{
    [Required,MinLength(4),MaxLength(7)]
    public string Code { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required,MinLength(4),MaxLength(48)]
    public string NewPassword { get; set; }

    public PasswordRecoveryRequest(string code, string email, string newPassword)
    {
        Code = code;
        Email = email;
        NewPassword = newPassword;
    }
}
