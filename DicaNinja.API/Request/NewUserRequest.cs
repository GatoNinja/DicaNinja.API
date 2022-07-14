
using System.ComponentModel.DataAnnotations;

namespace DicaNinja.API.Request;

public class NewUserRequest
{
    [Required, MinLength(3), MaxLength(48)]
    public string Username { get; set; }

    [Required, MinLength(4), MaxLength(48)]
    public string Password { get; set; }

    [Required, MinLength(4), MaxLength(48)]
    public string ConfirmPassword { get; set; }

    [Required, EmailAddress(ErrorMessage = "O endereço de e-mail é inválido")]
    public string Email { get; set; }

    [Required, MinLength(2), MaxLength(48)]
    public string Firstname { get; set; }

    [Required, MinLength(2), MaxLength(48)]
    public string Lastname { get; set; }

    public NewUserRequest(string username, string password, string confirmPassword, string email, string firstname, string lastname)
    {
        Username = username;
        Password = password;
        ConfirmPassword = confirmPassword;
        Email = email;
        Firstname = firstname;
        Lastname = lastname;
    }
}
