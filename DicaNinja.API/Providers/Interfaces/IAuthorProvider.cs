
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface IAuthorProvider
{
    Task<Author?> GetOrCreateAsync(string authorName, CancellationToken cancellationToken);

    Task<IEnumerable<Author>> GetByBookAsync(Guid bookId, CancellationToken cancellationToken);

    Task<int> GetCountAsync(CancellationToken cancellationToken);
}
