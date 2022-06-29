
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
        Context = context;
    }

    private BaseContext Context { get; }

    public async Task<List<Identifier>> GetByBookAsync(Guid bookId, CancellationToken cancellationToken)
    {
        return await Context.Identifiers.Where(identifier => identifier.BookId == bookId).ToListAsync(cancellationToken);
    }

    public async Task<Identifier?> GetOrCreateAsync(IdentifierResponse bookIdentifier, CancellationToken cancellationToken)
    {
        var identifier = await Context.Identifiers
            .FirstOrDefaultAsync(identifier => identifier.Isbn == bookIdentifier.Isbn && identifier.Type == bookIdentifier.Type, cancellationToken);

        if (identifier is not null)
        {
            return identifier;
        }

        identifier = new Identifier(bookIdentifier.Isbn, bookIdentifier.Type);

        await Context.Identifiers.AddAsync(identifier, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);

        return identifier;
    }
}
