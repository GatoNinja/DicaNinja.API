
using DicaNinja.API.Models;
using DicaNinja.API.Response;

namespace DicaNinja.API.Providers.Interfaces;

public interface IBookProvider
{
    Task<Book?> GetByIdentifierAsync(string identifier, string type, CancellationToken cancellation);

    Task<IEnumerable<Book>> GetBookmarksAsync(Guid userId, CancellationToken cancellation, int page = 1, int perPage = 10);

    Task PopulateWithBookmarksAsync(IEnumerable<BookResponse> books, Guid userId, CancellationToken cancellation);

    Task<IEnumerable<Review>> GetReviewsAsync(Guid bookId, CancellationToken cancellation, int page = 1, int perPage = 10);

    Task<double> AverageRatingAsync(Guid bookId, CancellationToken cancellation);

    Task<Book?> GetByIdAsync(Guid bookId, CancellationToken cancellation);

    Task<IEnumerable<Book>> GetBooksAsync(CancellationToken cancellation, int page = 1, int perPage = 10);

    Task<int> GetCountAsync(CancellationToken cancellation);

    Task<BookResponse?> GetByIsbnAsync(string isbn, string type, CancellationToken cancellation);

    Task<bool> GetExistingByIsbnAsync(string isbn, string type, CancellationToken cancellation);

    Task<BookResponse?> CreateFromGoogleAsync(string isbn, CancellationToken cancellation);
}
