
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

    public async Task<bool> FollowAsync(Guid userId, Guid followerId, CancellationToken cancellation)
    {
        var existingFollowing = await Context.Followers.AnyAsync(f => f.UserId == userId && f.FollowedId == followerId, cancellation).ConfigureAwait(false);

        if (existingFollowing)
        {
            return false;
        }

        var follower = new Follower(userId, followerId);

        await Context.Followers.AddAsync(follower, cancellation).ConfigureAwait(false);
        await Context.SaveChangesAsync(cancellation).ConfigureAwait(false);

        return true;
    }

    public async Task<bool> IsFollowingAsync(Guid userId, Guid followerId, CancellationToken cancellation)
    {
        return await Context.Followers.AnyAsync(f => f.UserId == userId && f.FollowedId == followerId, cancellation).ConfigureAwait(false);
    }

    public async Task<bool> UnFollowAsync(Guid userId, Guid followerId, CancellationToken cancellation)
    {
        var existingFollowing = await Context.Followers.AnyAsync(f => f.UserId == userId && f.FollowedId == followerId, cancellation).ConfigureAwait(false);

        if (!existingFollowing)
        {
            return false;
        }

        var follower = await Context.Followers.FirstAsync(f => f.UserId == userId && f.FollowedId == followerId, cancellation).ConfigureAwait(false);

        Context.Followers.Remove(follower);
        await Context.SaveChangesAsync(cancellation).ConfigureAwait(false);

        return true;
    }
}
