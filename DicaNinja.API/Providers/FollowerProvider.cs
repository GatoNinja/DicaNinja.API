using DicaNinja.API.Models;

using DicaNinja.API.Contexts;
using DicaNinja.API.Providers.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace DicaNinja.API.Providers;

public class FollowerProvider : IFollowerProvider
{
    public FollowerProvider(BaseContext context)
    {
        this.Context = context;
    }

    private BaseContext Context { get; }

    public async Task<bool> Follow(Guid userId, Guid followedId)
    {
        var existingFollowing = await this.Context.Followers.AnyAsync(f => f.UserId == userId && f.FollowedId == followedId);

        if (existingFollowing)
        {
            return false;
        }

        var follower = new Follower(userId, followedId);

        this.Context.Followers.Add(follower);
        await this.Context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> IsFollowing(Guid userId, Guid followerId)
    {
        return await this.Context.Followers.AnyAsync(f => f.UserId == userId && f.FollowedId == followerId);
    }

    public async Task<bool> Unfollow(Guid userId, Guid followerId)
    {
        var existingFollowing = await this.Context.Followers.AnyAsync(f => f.UserId == userId && f.FollowedId == followerId);

        if (!existingFollowing)
        {
            return false;
        }

        var follower = await this.Context.Followers.FirstAsync(f => f.UserId == userId && f.FollowedId == followerId);

        this.Context.Followers.Remove(follower);
        await this.Context.SaveChangesAsync();

        return true;
    }
}
