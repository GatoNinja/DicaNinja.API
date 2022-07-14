namespace DicaNinja.API.Response;

public class RefreshTokenResponse
{
    public RefreshTokenResponse(string accessToken, string refreshToken)
    {
        AccessToken = accessToken;
        RefreshToken = refreshToken;
    }

    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
