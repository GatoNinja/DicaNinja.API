
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

    public async Task<bool> FollowAsync(Guid userId, Guid followerId, CancellationToken cancellationToken)
    {
        var existingFollowing = await Context.Followers.AnyAsync(f => f.UserId == userId && f.FollowedId == followerId, cancellationToken).ConfigureAwait(false);

        if (existingFollowing)
        {
            return false;
        }

        var follower = new Follower(userId, followerId);

        await Context.Followers.AddAsync(follower, cancellationToken).ConfigureAwait(false);
        await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return true;
    }

    public async Task<bool> IsFollowingAsync(Guid userId, Guid followerId, CancellationToken cancellationToken)
    {
        return await Context.Followers.AnyAsync(f => f.UserId == userId && f.FollowedId == followerId, cancellationToken).ConfigureAwait(false);
    }

    public async Task<bool> UnFollowAsync(Guid userId, Guid followerId, CancellationToken cancellationToken)
    {
        var existingFollowing = await Context.Followers.AnyAsync(f => f.UserId == userId && f.FollowedId == followerId, cancellationToken).ConfigureAwait(false);

        if (!existingFollowing)
        {
            return false;
        }

        var follower = await Context.Followers.FirstAsync(f => f.UserId == userId && f.FollowedId == followerId, cancellationToken).ConfigureAwait(false);

        Context.Followers.Remove(follower);
        await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return true;
    }
}
