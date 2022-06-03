
using DicaNinja.API.Providers;
using DicaNinja.API.Tests.Abstracts;

namespace DicaNinja.API.Tests.Repositories;

public class FollowerProviderTest : BaseTest
{
    public FollowerProviderTest() : base()
    {
        this.FollowerProvider = new FollowerProvider(this.Context);
        this.UserProvider = new UserProvider(this.Context, this.PasswordHasher);
    }

    public FollowerProvider FollowerProvider { get; }
    public UserProvider UserProvider { get; }

    [Test]
    public async Task IsFollowingTest()
    {
        var firstUser = this.Users[0];
        var secondUser = this.Users[1];

        var isFollowing = await this.FollowerProvider.IsFollowing(firstUser.Id, secondUser.Id);

        Assert.That(isFollowing, Is.False);

        await this.FollowerProvider.Follow(firstUser.Id, secondUser.Id);

        isFollowing = await this.FollowerProvider.IsFollowing(firstUser.Id, secondUser.Id);

        Assert.That(isFollowing, Is.True);

        await this.UserProvider.GetFollowers(secondUser.Id);
        await this.UserProvider.GetFollowing(secondUser.Id);

        await this.FollowerProvider.Unfollow(firstUser.Id, secondUser.Id);

        isFollowing = await this.FollowerProvider.IsFollowing(firstUser.Id, secondUser.Id);

        Assert.That(isFollowing, Is.False);
    }
}
