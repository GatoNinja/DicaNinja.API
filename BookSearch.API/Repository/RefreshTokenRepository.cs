using System.Security.Cryptography;

using BookSearch.API.Contexts;
using BookSearch.API.Models;
using BookSearch.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.Repository;

public sealed class RefreshTokenRepository : IRefreshTokenRepository
{
    public RefreshTokenRepository(BaseContext context, IUserRepository userRepository)
    {
        Context = context;
        UserRepository = userRepository;
    }

    private BaseContext Context { get; }
    private IUserRepository UserRepository { get; }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string username, string value)
    {
        var query = from refreshToken in Context.RefreshTokens
            join user in Context.Users on refreshToken.UserId equals user.Id
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
        var user = await UserRepository.GetByIdAsync(userId);

        if (user is null)
        {
            throw new NullReferenceException("Usuário não encontrado");
        }

        var refreshToken = new RefreshToken(refreshTokenValue, userId, true);

        Context.RefreshTokens.Add(refreshToken);
        await Context.SaveChangesAsync();
    }

    public async Task InvalidateAsync(string value)
    {
        var refreshTokenFound =
            await Context.RefreshTokens.FirstOrDefaultAsync(refreshToken => refreshToken.Value == value);

        if (refreshTokenFound is null)
        {
            return;
        }

        refreshTokenFound.IsActive = false;

        Context.Entry(refreshTokenFound).State = EntityState.Modified;
        await Context.SaveChangesAsync();
    }
}