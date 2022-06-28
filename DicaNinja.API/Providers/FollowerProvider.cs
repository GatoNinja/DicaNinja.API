
using DicaNinja.API.Contexts;

using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace DicaNinja.API.Providers;

public class FollowerProvider : IFollowerProvider
{
    public FollowerProvider(BaseContext context)
    {
        Context = context;
    }

    private BaseContext Context { get; }

    public async Task<bool> FollowAsync(Guid userId, Guid followedId, CancellationToken cancellationToken)
    {
        var existingFollowing = await Context.Followers.AnyAsync(f => f.UserId == userId && f.FollowedId == followedId, cancellationToken);

        if (existingFollowing)
        {
            return false;
        }

        var follower = new Follower(userId, followedId);

        await Context.Followers.AddAsync(follower, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> IsFollowingAsync(Guid userId, Guid followerId, CancellationToken cancellationToken)
    {
        return await Context.Followers.AnyAsync(f => f.UserId == userId && f.FollowedId == followerId, cancellationToken);
    }

    public async Task<bool> UnFollowAsync(Guid userId, Guid followerId, CancellationToken cancellationToken)
    {
        var existingFollowing = await Context.Followers.AnyAsync(f => f.UserId == userId && f.FollowedId == followerId, cancellationToken);

        if (!existingFollowing)
        {
            return false;
        }

        var follower = await Context.Followers.FirstAsync(f => f.UserId == userId && f.FollowedId == followerId, cancellationToken);

        Context.Followers.Remove(follower);
        await Context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
