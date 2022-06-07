namespace DicaNinja.API.Providers.Interfaces;

public interface IFollowerProvider
{
    Task<bool> FollowAsync(Guid userId, Guid followerId);
    Task<bool> UnfollowAsync(Guid userId, Guid followerId);
    Task<bool> IsFollowingAsync(Guid userId, Guid followerId);
}
