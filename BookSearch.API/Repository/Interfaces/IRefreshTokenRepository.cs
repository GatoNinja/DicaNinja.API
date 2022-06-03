using BookSearch.API.Models;

namespace BookSearch.API.Repository.Interfaces;

public interface IRefreshTokenRepository
{
    string GenerateRefreshToken();

    Task<RefreshToken?> GetRefreshTokenAsync(string username, string value);

    Task SaveRefreshTokenAsync(Guid userId, string refreshToken);

    Task InvalidateAsync(string value);
}