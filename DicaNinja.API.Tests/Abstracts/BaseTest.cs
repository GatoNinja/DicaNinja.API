using DicaNinja.API.Startup;

using DicaNinja.API.Contexts;
using DicaNinja.API.Models;
using DicaNinja.API.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace DicaNinja.API.Tests.Abstracts;

public abstract class BaseTest
{
    protected BaseContext Context { get; }

    protected PasswordHasher PasswordHasher { get; }

    protected List<User> Users = new()
    {
        new User("gato", "ninja", "ninja@gatoninja.com.br", new Person("Gato", "Ninja")) { Reviews = new()},
        new User("guest", "guest", "guest@gatoninja.com.br", new Person("Guest", "User"))
    };

    protected List<Author> Authors = new() { new Author("Douglas Adams") };

    protected List<Category> Categories = new()
    {
        new Category("Aventura")
    };

    protected List<Book> Books = new()
    {
        new Book("A vida, o universo e tudo o mais", "O guia do mochileiro das galáxias", "pt", "Lindo livro", 100, "A Brasileia", DateTime.Now.ToShortDateString(), "imagem.png", 4.9)
    };

    protected List<Identifier> Identifiers = new()
    {
        new Identifier{ Type = "ISBN_13", Isbn = "123434534566"},
        new Identifier { Type = "ISBN_10", Isbn = "123434224"}
    };

    protected List<Review> Reviews = new()
    {
        new Review()
        {
            Rating = 4,
            Text = "Meu review lindo",
        }
    };

    protected BaseTest()
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
                .UseInMemoryDatabase("DicaNinja")
                .Options;

        Context = new BaseContext(contextOptions);

        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();

        var firstUser = Users.First();

        foreach (var book in Books)
        {
            book.Authors = new();
            book.Bookmarks = new();
            book.Categories = new();
            book.Identifiers = new();
            book.Reviews = new();

            book.Authors.AddRange(Authors);
            book.Bookmarks.Add(new Bookmark
            {
                User = firstUser
            });
            book.Categories.AddRange(Categories);
            book.Identifiers.AddRange(Identifiers);
            book.Reviews.AddRange(Reviews);
        }

        firstUser.Reviews = new();
        firstUser.Reviews.AddRange(Reviews);

        Context.Users.AddRange(Users);
        Context.Books.AddRange(Books);
        Context.SaveChanges();
    }
}
