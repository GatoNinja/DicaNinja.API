using BookSearch.API.Providers;
using BookSearch.API.Tests.Abstracts;

namespace BookSearch.API.Tests.Repositories;

public class FollowerProviderTest: BaseTest
{
    public FollowerProviderTest() : base()
    {
        this.FollowerProvider = new FollowerProvider(this.Context);
    }

    public FollowerProvider FollowerProvider { get; }

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

        await this.FollowerProvider.Unfollow(firstUser.Id, secondUser.Id);

        isFollowing = await this.FollowerProvider.IsFollowing(firstUser.Id, secondUser.Id);

        Assert.That(isFollowing, Is.False);
    }
}
