using BookSearch.API.Contexts;
using BookSearch.API.Models;
using BookSearch.API.Services;
using BookSearch.API.Startup;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace BookSearch.API.Tests.Abstracts;

public abstract class BaseTest
{
    protected BaseContext Context { get; }

    protected PasswordHasher PasswordHasher { get; }

    protected List<User> Users = new List<User>
    {
        new User("gato", "ninja", "ninja@gatoninja.com.br", new Person("Gato", "Ninja"))
    };

    public BaseTest()
    {
        var inMemorySettings = new Dictionary<string, string> {
                                {"HashIterations", "10000"}
                            };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
        var configurationReader = new ConfigurationReader(configuration);
        PasswordHasher = new PasswordHasher(configurationReader);

        foreach (var user in Users)
        {
            user.Password = PasswordHasher.Hash(user.Password);
        }

        var contextOptions = new DbContextOptionsBuilder<BaseContext>()
                .UseInMemoryDatabase("BookSearch")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
        Context = new BaseContext(contextOptions);

        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();

        Context.Users.AddRange(Users);
        Context.SaveChanges();
    }
}
