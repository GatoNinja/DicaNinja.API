
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface ICategoryProvider
{
    Task<Category?> GetOrCreateAsync(string categoryName, CancellationToken cancellationToken);
    Task<IEnumerable<Category>> GetByBookAsync(Guid bookId, CancellationToken cancellationToken);
    Task<int> GetCountAsync(CancellationToken cancellationToken);
}
