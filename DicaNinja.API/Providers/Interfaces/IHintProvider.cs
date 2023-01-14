using DicaNinja.API.Enums;
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface IHintProvider
{
    Task<Book?> GetHintAsync(Guid userId, CancellationToken cancellation);

    Task<IEnumerable<Book>> GetHintsLikedAsync(Guid userId, CancellationToken cancellation, int page = 1, int perPage = 10);

    Task<bool> SetHintAccepted(Guid userId, Guid bookId, EnumHintStatus hintStatus, CancellationToken cancellation);

    Task<int> TotalLikedAsync(Guid userId, CancellationToken cancellation);
}
