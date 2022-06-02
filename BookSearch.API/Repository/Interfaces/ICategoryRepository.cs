using BookSearch.API.Models;

namespace BookSearch.API.Repository.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetOrCreate(string categoryName);

    Task<List<Category>> GetByBook(Guid bookId);
}
