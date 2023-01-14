
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface IAuthorProvider
{
    Task<Author?> GetOrCreateAsync(string authorName, CancellationToken cancellation);

    Task<IEnumerable<Author>> GetByBookAsync(Guid bookId, CancellationToken cancellation);

    Task<int> GetCountAsync(CancellationToken cancellation);
}
