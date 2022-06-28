
using DicaNinja.API.Models;
using DicaNinja.API.Response;

namespace DicaNinja.API.Providers.Interfaces;

public interface IBookProvider
{
    Task<Book?> GetByIdentifierAsync(string identifier, string type, CancellationToken cancellationToken);

    Task<List<Book>> GetBookmarksAsync(Guid userId, CancellationToken cancellationToken, int page = 1, int perPage = 10);

    Task PopulateWithBookmarksAsync(IEnumerable<BookResponse> books, Guid userId, CancellationToken cancellationToken);

    Task<IEnumerable<Review>> GetReviewsAsync(Guid bookId, CancellationToken cancellationToken, int page = 1, int perPage = 10);
    Task<double> AverageRatingAsync(Guid bookId, CancellationToken cancellationToken);

    Task<Book?> GetByIdAsync(Guid bookId, CancellationToken cancellationToken);
    Task<IEnumerable<Book>> GetBooksAsync(CancellationToken cancellationToken, int page = 1, int perPage = 10);
    Task<int> GetCountAsync(CancellationToken cancellationToken);
    Task<BookResponse?> GetByIsbnAsync(string isbn, string type, CancellationToken cancellationToken);
}
