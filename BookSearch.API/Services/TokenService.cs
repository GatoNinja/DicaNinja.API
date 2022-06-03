using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookSearch.API.Models;
using BookSearch.API.Providers.Interfaces;
using BookSearch.API.Response;
using BookSearch.API.Startup;

using Microsoft.IdentityModel.Tokens;

namespace BookSearch.API.Services;

public sealed class TokenService : ITokenService
{
    public TokenService(IRefreshTokenProvider refreshTokenProvider, ConfigurationReader config)
    {
        this.RefreshTokenProvider = refreshTokenProvider;
        this.Config = config;
    }

    private IRefreshTokenProvider RefreshTokenProvider { get; }

    private ConfigurationReader Config { get; }

    public async Task<TokenResponse> GenerateTokenAsync(User user)
    {
        var claims = new List<Claim>
        {
            new("Id", user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, this.Config.Security.DefaultUserRole),
            new(ClaimTypes.Email, user.Email)
        };

        var accessToken = this.GenerateAccessToken(claims);
        var refreshToken = this.RefreshTokenProvider.GenerateRefreshToken();

        await this.RefreshTokenProvider.SaveRefreshTokenAsync(user.Id, refreshToken);

        return new TokenResponse(accessToken, refreshToken, user);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Config.Security.TokenSecurity)),
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal =
            tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        return securityToken switch
        {
            JwtSecurityToken jwtSecurityToken when jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase) => principal,
            _ => throw new SecurityTokenException("Invalid token")
        };

    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Config.Security.TokenSecurity));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokenExpiryInMinutes = Convert.ToInt32(this.Config.Security.TokenExpiryInMinutes);
        var tokeOptions = new JwtSecurityToken(this.Config.Info.Site, this.Config.Info.Site,
            claims,
            expires: DateTime.Now.AddMinutes(tokenExpiryInMinutes),
            signingCredentials: signinCredentials
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

        return tokenString;
    }
}
