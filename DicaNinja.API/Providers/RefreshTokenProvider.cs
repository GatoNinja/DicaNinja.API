using System.Security.Cryptography;

using DicaNinja.API.Models;

using DicaNinja.API.Contexts;
using DicaNinja.API.Providers.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace DicaNinja.API.Providers;

public sealed class RefreshTokenProvider : IRefreshTokenProvider
{
    public RefreshTokenProvider(BaseContext context, IUserProvider userProvider)
    {
        this.Context = context;
        this.UserProvider = userProvider;
    }

    private BaseContext Context { get; }
    private IUserProvider UserProvider { get; }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string username, string value)
    {
        var query = from refreshToken in this.Context.RefreshTokens
                    join user in this.Context.Users on refreshToken.UserId equals user.Id
                    let selectedUser = new User(user.Id, user.Username, user.Email)
                    where user.Username == username
                          && refreshToken.IsActive
                          && refreshToken.Value == value
                    select new RefreshToken(refreshToken.Id, refreshToken.Value, refreshToken.RefreshTokenExpiryTime,
                        selectedUser);

        return await query.FirstOrDefaultAsync();
    }

    public async Task SaveRefreshTokenAsync(Guid userId, string refreshTokenValue)
    {
        var user = await this.UserProvider.GetByIdAsync(userId);

        if (user is null)
        {
            throw new NullReferenceException("Usuário não encontrado");
        }

        var refreshToken = new RefreshToken(refreshTokenValue, userId, true);

        this.Context.RefreshTokens.Add(refreshToken);
        await this.Context.SaveChangesAsync();
    }

    public async Task InvalidateAsync(string value)
    {
        var refreshTokenFound =
            await this.Context.RefreshTokens.FirstOrDefaultAsync(refreshToken => refreshToken.Value == value);

        if (refreshTokenFound is null)
        {
            return;
        }

        refreshTokenFound.IsActive = false;

        this.Context.Entry(refreshTokenFound).State = EntityState.Modified;
        await this.Context.SaveChangesAsync();
    }
}