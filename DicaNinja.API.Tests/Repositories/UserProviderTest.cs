
using DicaNinja.API.Enums;
using DicaNinja.API.Providers;
using DicaNinja.API.Tests.Abstracts;

namespace DicaNinja.API.Tests.Repositories;

public class UserProviderTest : BaseProviderTest
{
    public UserProviderTest() : base()
    {
        UserProvider = new UserProvider(Context, PasswordHasher);
    }

    public UserProvider UserProvider { get; }

    [Test]
    public async Task GetByEmailTest()
    {
        var cancellation = new CancellationToken();
        var mock = Users.First();
        var user = await UserProvider.GetByEmailAsync(mock.Email, cancellation);

        Assert.That(user, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(user?.Id, Is.Not.Null);
            Assert.That(mock.Username, Is.EqualTo(user?.Username));
            Assert.That(mock.Email, Is.EqualTo(user?.Email));
        });
        Assert.Multiple(() =>
        {
            Assert.That(user?.Password, Is.Null);
        });
        user = await UserProvider.GetByEmailAsync("test@gatoninja.com.br", cancellation);

        Assert.That(user, Is.Null);
    }

    [Test]
    public async Task GetByIdTest()
    {
        var cancellation = new CancellationToken();
        var mock = Users.First();
        var user = await UserProvider.GetByIdAsync(mock.Id, cancellation);

        Assert.That(user, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(user?.Id, Is.Not.Null);
            Assert.That(user?.Password, Is.Null);
            Assert.That(mock.Username, Is.EqualTo(user?.Username));
            Assert.That(mock.Email, Is.EqualTo(user?.Email));
            Assert.That(mock.FirstName, Is.EqualTo(user?.FirstName));
            Assert.That(mock.LastName, Is.EqualTo(user?.LastName));
        });

        user = await UserProvider.GetByIdAsync(Guid.NewGuid(), cancellation);

        Assert.That(user, Is.Null);
    }

    [Test]
    public async Task DoLoginAsyncTest()
    {
        var cancellation = new CancellationToken();
        var mock = Users.First();
        var user = await UserProvider.DoLoginAsync(mock.Username, "ninja", cancellation);

        Assert.That(user, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(user?.Id, Is.Not.Null);
            Assert.That(user?.Password, Is.Null);
            Assert.That(mock.Username, Is.EqualTo(user?.Username));
            Assert.That(mock.Email, Is.EqualTo(user?.Email));
        });
        user = await UserProvider.DoLoginAsync("test", "test", cancellation);

        Assert.That(user, Is.Null);
    }

    [Test]
    public async Task InsertAsyncTest()
    {
        var cancellation = new CancellationToken();
        var mock = Users.First();
        mock.Id = Guid.Empty;
        var user = await UserProvider.InsertAsync(mock, cancellation);

        Assert.That(user, Is.Null);
    }

    [Test]
    public void ChangePasswordAsyncTest()
    {
        var cancellation = new CancellationToken();
        var mock = Users.First();

        Assert.DoesNotThrowAsync(async () =>
        {
            await UserProvider.ChangePasswordAsync(mock.Email, "ninjanovo", cancellation);
        });
    }

    [Test]
    public async Task ValidateNewUserTestAsync()
    {
        var cancellation = new CancellationToken();
        var mock = Users.First();
        var existingUsername = await UserProvider.ValidateNewUserAsync(mock, cancellation);

        Assert.That(existingUsername, Is.EqualTo(EnumNewUserCheck.ExistingUsername));

        mock.Username = "novousername";
        var existingEmail = await UserProvider.ValidateNewUserAsync(mock, cancellation);

        Assert.That(existingEmail, Is.EqualTo(EnumNewUserCheck.ExistingEmail));

        mock.Email = "novo@gatoninja.com.br";

        var valid = await UserProvider.ValidateNewUserAsync(mock, cancellation);

        Assert.That(valid, Is.EqualTo(EnumNewUserCheck.Valid));
    }
}
