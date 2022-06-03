
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface ICategoryProvider
{
    Task<Category?> GetOrCreate(string categoryName);

    Task<List<Category>> GetByBook(Guid bookId);
}
