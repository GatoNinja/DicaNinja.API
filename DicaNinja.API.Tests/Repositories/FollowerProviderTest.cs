
using DicaNinja.API.Providers;
using DicaNinja.API.Tests.Abstracts;

namespace DicaNinja.API.Tests.Repositories;

public class FollowerProviderTest : BaseProviderTest
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
        var cancellation = new CancellationToken();
        var firstUser = Users[0];
        var secondUser = Users[1];

        var isFollowing = await FollowerProvider.IsFollowingAsync(firstUser.Id, secondUser.Id, cancellation);

        Assert.That(isFollowing, Is.False);

        await FollowerProvider.FollowAsync(firstUser.Id, secondUser.Id, cancellation);

        isFollowing = await FollowerProvider.IsFollowingAsync(firstUser.Id, secondUser.Id, cancellation);

        Assert.That(isFollowing, Is.True);

        await UserProvider.GetFollowersAsync(secondUser.Id, cancellation);
        await UserProvider.GetFollowingAsync(secondUser.Id, cancellation);

        await FollowerProvider.UnFollowAsync(firstUser.Id, secondUser.Id, cancellation);

        isFollowing = await FollowerProvider.IsFollowingAsync(firstUser.Id, secondUser.Id, cancellation);

        Assert.That(isFollowing, Is.False);
    }
}
