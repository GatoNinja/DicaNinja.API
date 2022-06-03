namespace BookSearch.API.Providers.Interfaces;

public interface IFollowerProvider
{
    Task<bool> Follow(Guid userId, Guid followerId);
    Task<bool> UnFollow(Guid userId, Guid followerId);
    Task<bool> IsFollowing(Guid userId, Guid followerId);
}
