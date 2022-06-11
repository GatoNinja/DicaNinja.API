using DicaNinja.API.Startup;

using DicaNinja.API.Contexts;
using DicaNinja.API.Models;
using DicaNinja.API.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DicaNinja.API.Tests.Abstracts;

public abstract class BaseProviderTest
{
    protected BaseContext Context { get; }

    protected PasswordHasher PasswordHasher { get; }

    protected List<User> Users = new()
    {
        new User("gato", "ninja", "ninja@gatoninja.com.br", new Person("Gato", "Ninja")) ,
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
        new Identifier("9788576055487", "ISBN-13"),
        new Identifier("9776055487", "ISBN-10")
    };

    protected List<Review> Reviews = new()
    {
        new Review("Mew review lindo", 4)
    };

    protected BaseProviderTest()
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
            user.SetPassword(PasswordHasher.Hash(user.Password));
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
            book.Authors.AddRange(Authors);
            book.Bookmarks.Add(new Bookmark(firstUser));
            book.Categories.AddRange(Categories);
            book.Identifiers.AddRange(Identifiers);
            book.Reviews.AddRange(Reviews);
        }

        firstUser.Reviews.AddRange(Reviews);

        Context.Users.AddRange(Users);
        Context.Books.AddRange(Books);
        Context.SaveChanges();
    }
}
