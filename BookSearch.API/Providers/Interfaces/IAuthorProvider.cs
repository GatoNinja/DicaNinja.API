using BookSearch.API.Models;

namespace BookSearch.API.Providers.Interfaces;

public interface IAuthorProvider
{
    Task<Author?> GetOrCreate(string authorName);

    Task<List<Author>> GetByBook(Guid bookId);
}
