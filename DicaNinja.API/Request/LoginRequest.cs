
using System.ComponentModel.DataAnnotations;

namespace DicaNinja.API.Request;

public class LoginRequest
{
    [Required, MinLength(4)]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }

    public LoginRequest(string username, string password)
    {
        Username = username;
        Password = password;
    }
}
