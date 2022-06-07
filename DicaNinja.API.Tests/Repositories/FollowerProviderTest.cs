
using DicaNinja.API.Providers;
using DicaNinja.API.Tests.Abstracts;

namespace DicaNinja.API.Tests.Repositories;

public class FollowerProviderTest : BaseTest
{
    public FollowerProviderTest() : base()
    {
        FollowerProvider = new FollowerProvider(Context);
        UserProvider = new UserProvider(Context, PasswordHasher);
    }

    public FollowerProvider FollowerProvider { get; }
    public UserProvider UserProvider { get; }

    [Test]
    public async Task IsFollowingTest()
    {
        var firstUser = Users[0];
        var secondUser = Users[1];

        var isFollowing = await FollowerProvider.IsFollowing(firstUser.Id, secondUser.Id);

        Assert.That(isFollowing, Is.False);

        await FollowerProvider.Follow(firstUser.Id, secondUser.Id);

        isFollowing = await FollowerProvider.IsFollowing(firstUser.Id, secondUser.Id);

        Assert.That(isFollowing, Is.True);

        await UserProvider.GetFollowers(secondUser.Id);
        await UserProvider.GetFollowing(secondUser.Id);

        await FollowerProvider.Unfollow(firstUser.Id, secondUser.Id);

        isFollowing = await FollowerProvider.IsFollowing(firstUser.Id, secondUser.Id);

        Assert.That(isFollowing, Is.False);
    }
}
