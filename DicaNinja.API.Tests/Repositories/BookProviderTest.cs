
using DicaNinja.API.Providers;
using DicaNinja.API.Tests.Abstracts;

namespace DicaNinja.API.Tests.Repositories;

public class CategoryProviderTest : BaseProviderTest
{
    public CategoryProviderTest() : base()
    {
        CategoryProvider = new CategoryProvider(Context);
    }

    public CategoryProvider CategoryProvider { get; }

    [Test]
    public async Task GetByBookTest()
    {
        var cancellation = new CancellationToken();
        var mock = Books.First();
        var categories = await CategoryProvider.GetByBookAsync(mock.Id, cancellation);

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
        var cancellation = new CancellationToken();
        var mock = Categories.First();
        var category = await CategoryProvider.GetOrCreateAsync(mock.Name, cancellation);

        Assert.That(category, Is.Not.Null);
        Assert.That(category?.Name, Is.EqualTo(mock.Name));

        category = await CategoryProvider.GetOrCreateAsync("Categoria inexistente", cancellation);

        Assert.That(category, Is.Not.Null);
        Assert.That(category?.Name, Is.EqualTo("Categoria inexistente"));
    }
}
