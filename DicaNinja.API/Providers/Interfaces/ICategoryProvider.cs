
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface ICategoryProvider
{
    Task<Category?> GetOrCreateAsync(string categoryName, CancellationToken cancellation);
    Task<IEnumerable<Category>> GetByBookAsync(Guid bookId, CancellationToken cancellation);
    Task<int> GetCountAsync(CancellationToken cancellation);
}
