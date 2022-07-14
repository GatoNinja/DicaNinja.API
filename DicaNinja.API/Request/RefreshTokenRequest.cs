
using System.ComponentModel.DataAnnotations;

namespace DicaNinja.API.Request;

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; }

    public RefreshTokenRequest(string refreshToken)
    {
        RefreshToken = refreshToken;
    }
}
