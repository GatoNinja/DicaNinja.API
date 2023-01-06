
using DicaNinja.API.Contexts;

using DicaNinja.API.Models;

using DicaNinja.API.Providers.Interfaces;

using Microsoft.EntityFrameworkCore;

using System.Security.Cryptography;

namespace DicaNinja.API.Providers;

public sealed class RefreshTokenProvider : IRefreshTokenProvider
{
    public RefreshTokenProvider(BaseContext context, IUserProvider userProvider)
    {
        Context = context;
        UserProvider = userProvider;
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

    public async Task<RefreshToken?> GetRefreshTokenAsync(string username, string value, CancellationToken cancellationToken)
    {
        var query = from refreshToken in Context.RefreshTokens
                    join user in Context.Users on refreshToken.UserId equals user.Id
                    let selectedUser = new User(user.Id, user.Username, user.Email)
                    where user.Username == username
                          && refreshToken.IsActive
                          && refreshToken.Value == value
                    select new RefreshToken(refreshToken.Id, refreshToken.Value, refreshToken.RefreshTokenExpiryTime,
                        selectedUser);

        return await query.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task SaveRefreshTokenAsync(Guid userId, string refreshTokenValue, CancellationToken cancellationToken)
    {
        var user = await UserProvider.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

        if (user is null)
        {
            throw new KeyNotFoundException("Usuário não encontrado");
        }

        var refreshToken = new RefreshToken(refreshTokenValue, userId, true);

        Context.RefreshTokens.Add(refreshToken);
        await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task InvalidateAsync(string value, CancellationToken cancellationToken)
    {
        var refreshTokenFound =
            await Context.RefreshTokens.FirstOrDefaultAsync(refreshToken => refreshToken.Value == value, cancellationToken).ConfigureAwait(false);

        if (refreshTokenFound is null)
        {
            return;
        }

        refreshTokenFound.IsActive = false;

        Context.Entry(refreshTokenFound).State = EntityState.Modified;
        await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
