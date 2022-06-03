using BookSearch.API.Models;

namespace BookSearch.API.Providers.Interfaces;

public interface ICategoryProvider
{
    Task<Category?> GetOrCreate(string categoryName);

    Task<List<Category>> GetByBook(Guid bookId);
}
