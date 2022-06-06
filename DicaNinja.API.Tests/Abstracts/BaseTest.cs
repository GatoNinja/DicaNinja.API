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
        this.PasswordHasher = new PasswordHasher(configurationReader);

        foreach (var user in this.Users)
        {
            user.Password = this.PasswordHasher.Hash(user.Password);
        }

        var contextOptions = new DbContextOptionsBuilder<BaseContext>()
                .UseInMemoryDatabase("DicaNinja")
                .Options;

        this.Context = new BaseContext(contextOptions);

        this.Context.Database.EnsureDeleted();
        this.Context.Database.EnsureCreated();

        var firstUser = this.Users.First();

        foreach (var book in this.Books)
        {
            book.Authors = new();
            book.Bookmarks = new();
            book.Categories = new();
            book.Identifiers = new();
            book.Reviews = new();

            book.Authors.AddRange(this.Authors);
            book.Bookmarks.Add(new Bookmark
            {
                User = firstUser
            });
            book.Categories.AddRange(this.Categories);
            book.Identifiers.AddRange(this.Identifiers);
            book.Reviews.AddRange(this.Reviews);
        }

        firstUser.Reviews = new();
        firstUser.Reviews.AddRange(this.Reviews);

        this.Context.Users.AddRange(this.Users);
        this.Context.Books.AddRange(this.Books);
        this.Context.SaveChanges();
    }
}
