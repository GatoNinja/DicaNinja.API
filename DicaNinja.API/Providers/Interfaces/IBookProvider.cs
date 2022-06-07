
using DicaNinja.API.Models;
using DicaNinja.API.Response;

namespace DicaNinja.API.Providers.Interfaces;

public interface IBookProvider
{
    Task<Book?> GetByIdentifierAsync(string identifier, string type);

    Task<List<Book>> GetBookmarksAsync(Guid userId, int page = 1, int perPage = 10);

    Task PopulateWithBookmarksAsync(IEnumerable<BookResponse> books, Guid userId);

    Task<IEnumerable<Review>> GetReviewsAsync(Guid bookId, int page = 1, int perPage = 10);
    Task<double> AverageRatingAsync(Guid bookId);

    Task<Book?> GetByIdAsync(Guid bookId);
    Task<IEnumerable<Book>> GetBooksAsync(int page = 1, int perPage = 10);
    Task<int> GetCountAsync();
    Task<BookResponse?> GetByIsbnAsync(string isbn, string type);
}
