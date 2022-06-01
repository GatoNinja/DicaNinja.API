using BookSearch.API.Contexts;

using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.DDD.BookIdentifier;

public class BookIdentifierRepository : IBookIdentifierRepository
{
    public BookIdentifierRepository(DefaultContext context)
    {
        Context = context;
    }

    private DefaultContext Context { get; }
    
    public async Task<BookIdentifier?> GetOrCreate(BookIdentifierDTO bookIdentifier)
    {
        var identifier = await Context.BookIdentifiers.FirstOrDefaultAsync(identifier => identifier.Isbn == bookIdentifier.Isbn && identifier.Type == bookIdentifier.Type);
        
        if (identifier is not null)
        {
            return identifier;
        }

        identifier = new BookIdentifier(bookIdentifier.Isbn, bookIdentifier.Type);

        Context.BookIdentifiers.Add(identifier);

        return identifier;
    }
}
