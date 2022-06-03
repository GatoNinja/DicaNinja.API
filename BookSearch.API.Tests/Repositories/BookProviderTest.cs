using BookSearch.API.Providers;
using BookSearch.API.Tests.Abstracts;

namespace BookSearch.API.Tests.Repositories;

public class CategoryProviderTest : BaseTest
{
    public CategoryProviderTest() : base()
    {
        this.CategoryProvider = new CategoryProvider(this.Context);
    }

    public CategoryProvider CategoryProvider { get; }

    [Test]
    public async Task GetByBookTest()
    {
        var mock = this.Books.First();
        var categories = await this.CategoryProvider.GetByBook(mock.Id);

        Assert.That(categories, Is.Not.Null);
        CollectionAssert.AllItemsAreNotNull(categories);
        CollectionAssert.IsNotEmpty(categories);

        foreach (var author in categories)
        {
            Assert.That(author.Name, Is.Not.Null);
            Assert.That(author.Name, Has.Length);
        }
    }

    [Test]
    public async Task GetOrCreateTest()
    {
        var mock = this.Categories.First();
        var category = await this.CategoryProvider.GetOrCreate(mock.Name);

        Assert.That(category, Is.Not.Null);
        Assert.That(category.Name, Is.EqualTo(mock.Name));

        category = await this.CategoryProvider.GetOrCreate("Categoria inexistente");

        Assert.That(category, Is.Not.Null);
        Assert.That(category.Name, Is.EqualTo("Categoria inexistente"));
    }
}
