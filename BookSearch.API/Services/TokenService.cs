using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookSearch.API.Models;
using BookSearch.API.Repository.Interfaces;
using BookSearch.API.Response;
using BookSearch.API.Startup;

using Microsoft.IdentityModel.Tokens;

namespace BookSearch.API.Services;

public sealed class TokenService : ITokenService
{
    public TokenService(IRefreshTokenRepository refreshTokenRepository, ConfigurationReader config)
    {
        RefreshTokenRepository = refreshTokenRepository;
        Config = config;
    }

    private IRefreshTokenRepository RefreshTokenRepository { get; }

    private ConfigurationReader Config { get; }

    public async Task<TokenResponse> GenerateTokenAsync(User user)
    {
        var claims = new List<Claim>
        {
            new("Id", user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, Config.Security.DefaultUserRole),
            new(ClaimTypes.Email, user.Email)
        };

        var accessToken = GenerateAccessToken(claims);
        var refreshToken = RefreshTokenRepository.GenerateRefreshToken();

        await RefreshTokenRepository.SaveRefreshTokenAsync(user.Id, refreshToken);

        return new TokenResponse(accessToken, refreshToken, user);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.Security.TokenSecurity)),
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal =
            tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase)) throw new SecurityTokenException("Invalid token");

        return principal;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config.Security.TokenSecurity));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokenExpiryInMinutes = Convert.ToInt32(Config.Security.TokenExpiryInMinutes);
        var tokeOptions = new JwtSecurityToken(
            Config.Info.Site,
            Config.Info.Site,
            claims,
            expires: DateTime.Now.AddMinutes(tokenExpiryInMinutes),
            signingCredentials: signinCredentials
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

        return tokenString;
    }
}