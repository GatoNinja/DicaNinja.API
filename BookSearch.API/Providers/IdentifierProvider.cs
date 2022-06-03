using BookSearch.API.Contexts;
using BookSearch.API.Models;
using BookSearch.API.Providers.Interfaces;
using BookSearch.API.Response;
using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.Providers;

public class IdentifierProvider : IIdentifierProvider
{
    public IdentifierProvider(BaseContext context)
    {
        Context = context;
    }

    private BaseContext Context { get; }

    public async Task<List<Identifier>> GetByBook(Guid bookId)
    {
        return await Context.Identifiers.Where(identifier => identifier.BookId == bookId).ToListAsync();
    }

    public async Task<Identifier?> GetOrCreate(IdentifierResponse bookIdentifier)
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
