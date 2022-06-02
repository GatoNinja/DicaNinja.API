using BookSearch.API.Contexts;

using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.DDD.Identifier;

public class IdentifierRepository : IIdentifierRepository
{
    public IdentifierRepository(DefaultContext context)
    {
        Context = context;
    }

    private DefaultContext Context { get; }

    public async Task<List<Identifier>> GetByBook(Guid bookId)
    {
        return await Context.Identifiers.Where(identifier => identifier.BookId == bookId).ToListAsync();
    }

    public async Task<Identifier?> GetOrCreate(IdentifierDTO bookIdentifier)
    {
        var identifier = await Context.Identifiers.FirstOrDefaultAsync(identifier => identifier.Isbn == bookIdentifier.Isbn && identifier.Type == bookIdentifier.Type);
        
        if (identifier is not null)
        {
            return identifier;
        }

        identifier = new Identifier(bookIdentifier.Isbn, bookIdentifier.Type);

        Context.Identifiers.Add(identifier);

        return identifier;
    }
}
