using BookSearch.API.Enums;
using BookSearch.API.Providers;
using BookSearch.API.Tests.Abstracts;

namespace BookSearch.API.Tests.Repositores;

public class UserProviderTest : BaseTest
{
    public UserProviderTest() : base()
    {
        this.UserProvider = new UserProvider(Context, PasswordHasher);
    }

    public UserProvider UserProvider { get; }

    [Test]
    public async Task GetByEmailTest()
    {
        var mock = Users.First();
        var user = await UserProvider.GetByEmail(mock.Email);

        Assert.IsNotNull(user);
        Assert.IsNotNull(user?.Id);
        Assert.AreEqual(user?.Username, mock.Username);
        Assert.AreEqual(user?.Email, mock.Email);
        Assert.IsNull(user?.Password);
        Assert.IsNull(user?.Person);

        user = await UserProvider.GetByEmail("test@gatoninja.com.br");

        Assert.IsNull(user);
    }

    [Test]
    public async Task GetByIdTest()
    {
        var mock = Users.First();
        var user = await UserProvider.GetByIdAsync(mock.Id);

        Assert.IsNotNull(user);
        Assert.IsNotNull(user?.Id);
        Assert.IsNull(user?.Password);
        Assert.AreEqual(user?.Username, mock.Username);
        Assert.AreEqual(user?.Email, mock.Email);
        Assert.AreEqual(user?.Person.FirstName, mock.Person.FirstName);
        Assert.AreEqual(user?.Person.LastName, mock.Person.LastName);

        user = await UserProvider.GetByIdAsync(Guid.NewGuid());

        Assert.IsNull(user);
    }

    [Test]
    public async Task DoLoginAsyncTest()
    {
        var mock = Users.First();
        var user = await UserProvider.DoLoginAsync(mock.Username, "ninja");

        Assert.IsNotNull(user);
        Assert.IsNotNull(user?.Id);
        Assert.IsNull(user?.Password);
        Assert.AreEqual(user?.Username, mock.Username);
        Assert.AreEqual(user?.Email, mock.Email);
        Assert.IsNull(user?.Person);

        user = await UserProvider.DoLoginAsync("test", "test");

        Assert.IsNull(user);
    }

    [Test]
    public async Task InsertAsyncTest()
    {
        var mock = Users.First();
        mock.Id = Guid.Empty;
        var user = await UserProvider.InsertAsync(mock);

        Assert.IsNull(user);

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
        var mock = Users.First();

        Assert.DoesNotThrowAsync(async () =>
        {
            await UserProvider.ChangePassword(mock.Email, "ninjanovo");
        });
    }

    [Test]
    public void ValidateNewUserTest()
    {
        var mock = Users.First();
        var existingUsername = UserProvider.ValidateNewUser(mock);

        Assert.AreEqual(existingUsername, EnumNewUserCheck.ExistingUsername);

        mock.Username = "novousername";
        var existingEmail = UserProvider.ValidateNewUser(mock);

        Assert.AreEqual(existingEmail, EnumNewUserCheck.ExistingEmail);

        mock.Email = "novo@gatoninja.com.br";

        var valid = UserProvider.ValidateNewUser(mock);

        Assert.AreEqual(valid, EnumNewUserCheck.Valid);
    }
}
