
using System.ComponentModel.DataAnnotations;

namespace DicaNinja.API.Request;

public class ForgotPasswordRequest
{
    [Required, EmailAddress]
    public string Email { get; set; }

    public ForgotPasswordRequest(string email)
    {
        Email = email;
    }
}
