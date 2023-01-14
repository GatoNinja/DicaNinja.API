
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Response;

using DicaNinja.API.Startup;

using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using System.Text;

namespace DicaNinja.API.Services;

public sealed class TokenService : ITokenService
{
    public TokenService(IRefreshTokenProvider refreshTokenProvider, ConfigurationReader config)
    {
        RefreshTokenProvider = refreshTokenProvider;
        Config = config;
    }

    private IRefreshTokenProvider RefreshTokenProvider { get; }

    private ConfigurationReader Config { get; }

    public async Task<TokenResponse> GenerateTokenAsync(User user, CancellationToken cancellation)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var claims = new List<Claim>
        {
            new("Id", user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, Config.Security.DefaultUserRole),
            new(ClaimTypes.Email, user.Email)
        };

        var accessToken = GenerateAccessToken(claims);
        var refreshToken = RefreshTokenProvider.GenerateRefreshToken();

        await RefreshTokenProvider.SaveRefreshTokenAsync(user.Id, refreshToken, cancellation).ConfigureAwait(false);

        return new TokenResponse(accessToken, refreshToken, user);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.Security.TokenSecurity)),
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal =
            tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        return securityToken switch
        {
            JwtSecurityToken jwtSecurityToken when jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.OrdinalIgnoreCase) => principal,
            _ => throw new SecurityTokenException("Invalid token")
        };

    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.Security.TokenSecurity));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokenExpiryInMinutes = Convert.ToInt32(Config.Security.TokenExpiryInMinutes);
        var tokeOptions = new JwtSecurityToken(Config.Info.Site, Config.Info.Site,
            claims,
            expires: DateTime.Now.AddMinutes(tokenExpiryInMinutes),
            signingCredentials: signinCredentials
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

        return tokenString;
    }
}
