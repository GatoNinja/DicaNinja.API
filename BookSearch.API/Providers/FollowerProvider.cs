using BookSearch.API.Contexts;
using BookSearch.API.Models;
using BookSearch.API.Providers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.Providers;

public class FollowerProvider : IFollowerProvider
{
    public FollowerProvider(BaseContext context)
    {
        Context = context;
    }

    private BaseContext Context { get; }

    public async Task<bool> Follow(Guid userId, Guid followedId)
    {
        var existingFollowing = await Context.Followers.AnyAsync(f => f.UserId == userId && f.FollowedId == followedId);

        if (existingFollowing)
        {
            return false;
        }

        var follower = new Follower(userId, followedId);

        Context.Followers.Add(follower);
        await Context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> IsFollowing(Guid userId, Guid followerId)
    {
        return await Context.Followers.AnyAsync(f => f.UserId == userId && f.FollowedId == followerId);
    }

    public async Task<bool> UnFollow(Guid userId, Guid followerId)
    {
        var existingFollowing = await Context.Followers.AnyAsync(f => f.UserId == userId && f.FollowedId == followerId);

        if (!existingFollowing)
        {
            return false;
        }

        var follower = await Context.Followers.FirstAsync(f => f.UserId == userId && f.FollowedId == followerId);

        Context.Followers.Remove(follower);
        await Context.SaveChangesAsync();

        return true;
    }
}
