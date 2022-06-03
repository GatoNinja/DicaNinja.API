namespace DicaNinja.API.Providers.Interfaces;

public interface IBookmarkProvider
{
    Task<int> GetBookmarkCount(Guid userId);

    Task<int?> Bookmark(Guid userId, string identifier, string type);

    Task<bool> IsBookmarked(Guid userId, string identifier, string type);
}
