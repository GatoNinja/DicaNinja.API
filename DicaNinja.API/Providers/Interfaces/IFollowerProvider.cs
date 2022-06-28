namespace DicaNinja.API.Providers.Interfaces;

public interface IFollowerProvider
{
    Task<bool> FollowAsync(Guid userId, Guid followerId, CancellationToken cancellationToken);
    Task<bool> UnFollowAsync(Guid userId, Guid followerId, CancellationToken cancellationToken);
    Task<bool> IsFollowingAsync(Guid userId, Guid followerId, CancellationToken cancellationToken);
}
