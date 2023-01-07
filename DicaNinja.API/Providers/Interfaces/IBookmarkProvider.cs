namespace DicaNinja.API.Providers.Interfaces;

public interface IBookmarkProvider
{
    Task<int> GetBookmarkCountAsync(Guid userId, CancellationToken cancellationToken);

    Task<bool?> BookmarkAsync(Guid userId, string identifier, string type, CancellationToken cancellationToken);

    Task<bool> IsBookMarkedAsync(Guid userId, string identifier, string type, CancellationToken cancellationToken);

    Task<bool> IsBookMarkedAsync(Guid userId, Guid bookId);
}
