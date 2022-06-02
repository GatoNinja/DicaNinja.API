namespace BookSearch.API.Repository.Interfaces;

public interface IBookmarkRepository
{
    Task<int> GetBookmarkCount(Guid userId);

    Task<int?> Bookmark(Guid userId, string identifier, string type);

    Task<bool> IsBookmarked(Guid userId, string identifier, string type);
}
