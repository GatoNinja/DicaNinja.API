using DicaNinja.API.Response;

using DicaNinja.API.Providers;
using DicaNinja.API.Tests.Abstracts;

namespace DicaNinja.API.Tests.Repositories;

public class IdentifierProviderTest : BaseTest
{
    public IdentifierProviderTest() : base()
    {
        IdentifierProvider = new IdentifierProvider(Context);
    }

    public IdentifierProvider IdentifierProvider { get; }

    [Test]
    public async Task GetByBookTest()
    {
        var mock = Books.First();
        var identifiers = await IdentifierProvider.GetByBook(mock.Id);

        Assert.That(identifiers, Is.Not.Null);
        CollectionAssert.AllItemsAreNotNull(identifiers);
        CollectionAssert.IsNotEmpty(identifiers);

        foreach (var identifier in identifiers)
        {
            Assert.That(identifier.Isbn, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(identifier.Isbn, Has.Length);

                Assert.That(identifier.Type, Is.Not.Null);
            });
            Assert.That(identifier.Type, Has.Length);
        }
    }

    [Test]
    public async Task GetOrCreateTest()
    {
        var mock = Identifiers.First();
        var identifier = await IdentifierProvider.GetOrCreate(new IdentifierResponse
        {
            Isbn = mock.Isbn,
            Type = mock.Type
        });

        Assert.That(identifier, Is.Not.Null);
        Assert.That(identifier, Is.EqualTo(mock));

        var newMock = new IdentifierResponse
        {
            Isbn = "21341213123",
            Type = "ISBN-13"
        };
        identifier = await IdentifierProvider.GetOrCreate(newMock);

        Assert.That(identifier, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(identifier.Id, Is.Not.Empty);
            Assert.That(identifier.Isbn, Is.EqualTo(newMock.Isbn));
            Assert.That(identifier.Type, Is.EqualTo(newMock.Type));
        });
    }
}
