using BookSearch.API.Models;
using BookSearch.API.Response;

namespace BookSearch.API.Repository.Interfaces;

public interface IBookRepository
{
    Task<Book?> GetByIdentifier(string identifier, string type);

    Task<Book?> CreateFromResponse(BookResponse response);

    Task<List<Book>> GetBookmarks(Guid userId, int page, int perPage);

    Task PopulateWithBookmarks(IEnumerable<BookResponse> books, Guid userId);
}
