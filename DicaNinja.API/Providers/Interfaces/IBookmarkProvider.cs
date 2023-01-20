namespace DicaNinja.API.Providers.Interfaces;

public interface IBookmarkProvider
{
    Task<int> GetBookmarkCountAsync(Guid userId, CancellationToken cancellation);

    Task<bool?> BookmarkAsync(Guid userId, string identifier, string type, CancellationToken cancellation);

    Task<bool> IsBookMarkedAsync(Guid userId, string identifier, string type, CancellationToken cancellation);

    Task<bool> IsBookMarkedAsync(Guid userId, Guid bookId);

    Task<bool> HasBookmarkAsync(Guid userId, CancellationToken cancellation);
}
