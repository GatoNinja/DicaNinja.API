
using DicaNinja.API.Contexts;
using DicaNinja.API.Models;

using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Response;

using Microsoft.EntityFrameworkCore;

namespace DicaNinja.API.Providers;

public class IdentifierProvider : IIdentifierProvider
{
    public IdentifierProvider(BaseContext context)
    {
        this.Context = context;
    }

    private BaseContext Context { get; }

    public async Task<List<Identifier>> GetByBook(Guid bookId)
    {
        return await this.Context.Identifiers.Where(identifier => identifier.BookId == bookId).ToListAsync();
    }

    public async Task<Identifier?> GetOrCreate(IdentifierResponse bookIdentifier)
    {
        var identifier = await this.Context.Identifiers.FirstOrDefaultAsync(identifier => identifier.Isbn == bookIdentifier.Isbn && identifier.Type == bookIdentifier.Type);

        if (identifier is not null)
        {
            return identifier;
        }

        identifier = new Identifier(bookIdentifier.Isbn, bookIdentifier.Type);

        this.Context.Identifiers.Add(identifier);

        return identifier;
    }
}
