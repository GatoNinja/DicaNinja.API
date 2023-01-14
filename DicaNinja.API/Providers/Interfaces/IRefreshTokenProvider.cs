
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface IRefreshTokenProvider
{
    string GenerateRefreshToken();

    Task<RefreshToken?> GetRefreshTokenAsync(string username, string value, CancellationToken cancellation);

    Task SaveRefreshTokenAsync(Guid userId, string refreshTokenValue, CancellationToken cancellation);

    Task InvalidateAsync(string value, CancellationToken cancellation);
}
