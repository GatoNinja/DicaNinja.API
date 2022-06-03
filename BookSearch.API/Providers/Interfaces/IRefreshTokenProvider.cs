using BookSearch.API.Models;

namespace BookSearch.API.Providers.Interfaces;

public interface IRefreshTokenProvider
{
    string GenerateRefreshToken();

    Task<RefreshToken?> GetRefreshTokenAsync(string username, string value);

    Task SaveRefreshTokenAsync(Guid userId, string refreshToken);

    Task InvalidateAsync(string value);
}
