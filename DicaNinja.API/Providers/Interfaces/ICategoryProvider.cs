
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface ICategoryProvider
{
    Task<Category?> GetOrCreateAsync(string categoryName);
    Task<List<Category>> GetByBookAsync(Guid bookId);
    Task<int> GetCountAsync();
}
