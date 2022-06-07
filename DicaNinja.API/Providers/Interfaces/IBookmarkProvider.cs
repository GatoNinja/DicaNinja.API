namespace DicaNinja.API.Providers.Interfaces;

public interface IBookmarkProvider
{
    Task<int> GetBookmarkCountAsync(Guid userId);

    Task<int?> BookmarkAsync(Guid userId, string identifier, string type);

    Task<bool> IsBookmarkedAsync(Guid userId, string identifier, string type);
}
