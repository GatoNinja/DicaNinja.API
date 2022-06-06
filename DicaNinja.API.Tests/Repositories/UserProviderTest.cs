
using DicaNinja.API.Enums;
using DicaNinja.API.Providers;
using DicaNinja.API.Tests.Abstracts;

namespace DicaNinja.API.Tests.Repositories;

public class UserProviderTest : BaseTest
{
    public UserProviderTest() : base()
    {
        this.UserProvider = new UserProvider(this.Context, this.PasswordHasher);
    }

    public UserProvider UserProvider { get; }

    [Test]
    public async Task GetByEmailTest()
    {
        var mock = this.Users.First();
        var user = await this.UserProvider.GetByEmail(mock.Email);

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
        user = await this.UserProvider.GetByEmail("test@gatoninja.com.br");

        Assert.That(user, Is.Null);
    }

    [Test]
    public async Task GetByIdTest()
    {
        var mock = this.Users.First();
        var user = await this.UserProvider.GetByIdAsync(mock.Id);

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

        user = await this.UserProvider.GetByIdAsync(Guid.NewGuid());

        Assert.That(user, Is.Null);
    }

    [Test]
    public async Task DoLoginAsyncTest()
    {
        var mock = this.Users.First();
        var user = await this.UserProvider.DoLoginAsync(mock.Username, "ninja");

        Assert.That(user, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(user?.Id, Is.Not.Null);
            Assert.That(user?.Password, Is.Null);
            Assert.That(mock.Username, Is.EqualTo(user?.Username));
            Assert.That(mock.Email, Is.EqualTo(user?.Email));
            Assert.That(user?.Person, Is.Null);
        });
        user = await this.UserProvider.DoLoginAsync("test", "test");

        Assert.That(user, Is.Null);
    }

    [Test]
    public async Task InsertAsyncTest()
    {
        var mock = this.Users.First();
        mock.Id = Guid.Empty;
        var user = await this.UserProvider.InsertAsync(mock);

        Assert.That(user, Is.Null);

        // var newUser = new User("admin", "admin", "admin@gatoninja.com.br", new Person("Admin", "Admin"));
        // var insertedUser = await UserProvider.InsertAsync(newUser);

        // Assert.IsNotNull(insertedUser);
        // Assert.IsNotNull(insertedUser?.Id);
        // Assert.IsNull(insertedUser?.Password);
        // Assert.AreEqual(insertedUser?.Username, mock.Username);
        // Assert.AreEqual(insertedUser?.Email, mock.Email);
        // Assert.IsNull(insertedUser?.Person);
    }

    [Test]
    public void ChangePasswordAsyncTest()
    {
        var mock = this.Users.First();

        Assert.DoesNotThrowAsync(async () =>
        {
            await this.UserProvider.ChangePassword(mock.Email, "ninjanovo");
        });
    }

    [Test]
    public void ValidateNewUserTest()
    {
        var mock = this.Users.First();
        var existingUsername = this.UserProvider.ValidateNewUser(mock);

        Assert.That(existingUsername, Is.EqualTo(EnumNewUserCheck.ExistingUsername));

        mock.Username = "novousername";
        var existingEmail = this.UserProvider.ValidateNewUser(mock);

        Assert.That(existingEmail, Is.EqualTo(EnumNewUserCheck.ExistingEmail));

        mock.Email = "novo@gatoninja.com.br";

        var valid = this.UserProvider.ValidateNewUser(mock);

        Assert.That(valid, Is.EqualTo(EnumNewUserCheck.Valid));
    }
}
