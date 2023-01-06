
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface IRefreshTokenProvider
{
    string GenerateRefreshToken();

    Task<RefreshToken?> GetRefreshTokenAsync(string username, string value, CancellationToken cancellationToken);

    Task SaveRefreshTokenAsync(Guid userId, string refreshTokenValue, CancellationToken cancellationToken);

    Task InvalidateAsync(string value, CancellationToken cancellationToken);
}
