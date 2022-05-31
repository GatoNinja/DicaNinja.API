using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using BookSearch.API.DDD.RefreshToken;
using BookSearch.API.DDD.User;

using Microsoft.IdentityModel.Tokens;

namespace BookSearch.API.DDD.Token;

public sealed class TokenService : ITokenService
{
    public TokenService(IRefreshTokenRepository refreshTokenRepository, IConfiguration configuration)
    {
        RefreshTokenRepository = refreshTokenRepository;
        Configuration = configuration;
    }

    private IRefreshTokenRepository RefreshTokenRepository { get; }

    private IConfiguration Configuration { get; }

    public async Task<TokenResponse> GenerateTokenAsync(UserModel userModel)
    {
        var claims = new List<Claim>
        {
            new("Id", userModel.Id.ToString()),
            new(ClaimTypes.Name, userModel.Username),
            new(ClaimTypes.Role, Configuration["DefaultUserRole"]),
            new(ClaimTypes.Email, userModel.Email)
        };

        var accessToken = GenerateAccessToken(claims);
        var refreshToken = RefreshTokenRepository.GenerateRefreshToken();

        await RefreshTokenRepository.SaveRefreshTokenAsync(userModel.Id, refreshToken);

        return new TokenResponse(accessToken, refreshToken, userModel);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenSecurity"])),
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
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenSecurity"]));
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokenExpiryInMinutes = Convert.ToInt32(Configuration["TokenExpiryInMinutes"]);
        var tokeOptions = new JwtSecurityToken(
            Configuration["Info:Site"],
            Configuration["Info:Site"],
            claims,
            expires: DateTime.Now.AddMinutes(tokenExpiryInMinutes),
            signingCredentials: signinCredentials
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

        return tokenString;
    }
}