using BookSearch.API.Models;

namespace BookSearch.API.Repository.Interfaces;

public interface IAuthorRepository
{
    Task<Author?> GetOrCreate(string authorName);

    Task<List<Author>> GetByBook(Guid bookId);
}
