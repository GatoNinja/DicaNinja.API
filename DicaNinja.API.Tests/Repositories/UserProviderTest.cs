
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
        var mock = Users.First();
        var user = await UserProvider.GetByEmailAsync(mock.Email);

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
            Assert.That(user?.Person, Is.Null);
        });
        user = await UserProvider.GetByEmailAsync("test@gatoninja.com.br");

        Assert.That(user, Is.Null);
    }

    [Test]
    public async Task GetByIdTest()
    {
        var mock = Users.First();
        var user = await UserProvider.GetByIdAsync(mock.Id);

        Assert.That(user, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(user?.Id, Is.Not.Null);
            Assert.That(user?.Password, Is.Null);
            Assert.That(mock.Username, Is.EqualTo(user?.Username));
            Assert.That(mock.Email, Is.EqualTo(user?.Email));
            Assert.That(mock.Person.FirstName, Is.EqualTo(user?.Person.FirstName));
            Assert.That(mock.Person.LastName, Is.EqualTo(user?.Person.LastName));
        });

        user = await UserProvider.GetByIdAsync(Guid.NewGuid());

        Assert.That(user, Is.Null);
    }

    [Test]
    public async Task DoLoginAsyncTest()
    {
        var mock = Users.First();
        var user = await UserProvider.DoLoginAsync(mock.Username, "ninja");

        Assert.That(user, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(user?.Id, Is.Not.Null);
            Assert.That(user?.Password, Is.Null);
            Assert.That(mock.Username, Is.EqualTo(user?.Username));
            Assert.That(mock.Email, Is.EqualTo(user?.Email));
            Assert.That(user?.Person, Is.Null);
        });
        user = await UserProvider.DoLoginAsync("test", "test");

        Assert.That(user, Is.Null);
    }

    [Test]
    public async Task InsertAsyncTest()
    {
        var mock = Users.First();
        mock.Id = Guid.Empty;
        var user = await UserProvider.InsertAsync(mock);

        Assert.That(user, Is.Null);
    }

    [Test]
    public void ChangePasswordAsyncTest()
    {
        var mock = Users.First();

        Assert.DoesNotThrowAsync(async () =>
        {
            await UserProvider.ChangePasswordAsync(mock.Email, "ninjanovo");
        });
    }

    [Test]
    public void ValidateNewUserTest()
    {
        var mock = Users.First();
        var existingUsername = UserProvider.ValidateNewUser(mock);

        Assert.That(existingUsername, Is.EqualTo(EnumNewUserCheck.ExistingUsername));

        mock.Username = "novousername";
        var existingEmail = UserProvider.ValidateNewUser(mock);

        Assert.That(existingEmail, Is.EqualTo(EnumNewUserCheck.ExistingEmail));

        mock.Email = "novo@gatoninja.com.br";

        var valid = UserProvider.ValidateNewUser(mock);

        Assert.That(valid, Is.EqualTo(EnumNewUserCheck.Valid));
    }
}
