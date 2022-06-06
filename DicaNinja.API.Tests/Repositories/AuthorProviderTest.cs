
using DicaNinja.API.Providers;
using DicaNinja.API.Tests.Abstracts;

namespace DicaNinja.API.Tests.Repositories;

public class AuthorProviderTest : BaseTest
{
    public AuthorProviderTest() : base()
    {
        this.AuthorProvider = new AuthorProvider(this.Context);
    }

    public AuthorProvider AuthorProvider { get; }

    [Test]
    public async Task GetByBookTest()
    {
        var mock = this.Books.First();
        var authors = await this.AuthorProvider.GetByBook(mock.Id);

        Assert.That(authors, Is.Not.Null);
        CollectionAssert.AllItemsAreNotNull(authors);
        CollectionAssert.IsNotEmpty(authors);

        foreach (var author in authors)
        {
            Assert.That(author.Name, Is.Not.Null);
            Assert.That(author.Name, Has.Length);
        }
    }

    [Test]
    public async Task GetOrCreateTest()
    {
        var mock = this.Authors.First();
        var author = await this.AuthorProvider.GetOrCreate(mock.Name);

        Assert.That(author, Is.Not.Null);
        Assert.That(author.Name, Is.EqualTo(mock.Name));

        author = await this.AuthorProvider.GetOrCreate("Author inexistente");

        Assert.That(author, Is.Not.Null);
        Assert.That(author.Name, Is.EqualTo("Author inexistente"));
    }
}