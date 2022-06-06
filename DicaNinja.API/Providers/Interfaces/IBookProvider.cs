
using DicaNinja.API.Models;
using DicaNinja.API.Response;

namespace DicaNinja.API.Providers.Interfaces;

public interface IBookProvider
{
    Task<Book?> GetByIdentifier(string identifier, string type);

    Task<Book?> CreateFromResponse(BookResponse response);

    Task<List<Book>> GetBookmarks(Guid userId, int page = 1, int perPage = 10);

    Task PopulateWithBookmarks(IEnumerable<BookResponse> books, Guid userId);

    Task<IEnumerable<Review>> GetReviews(Guid bookId, int page = 1, int perPage = 10);
    Task<double> AverageRating(Guid bookId);

    Task<Book?> GetById(Guid bookId);
    Task<IEnumerable<Book>> GetBooks(int page = 1, int perPage = 10);
    Task<int> GetCount();
}
