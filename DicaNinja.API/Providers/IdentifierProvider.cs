
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

    public async Task<IEnumerable<Identifier>> GetByBookAsync(Guid bookId, CancellationToken cancellation)
    {
        return await Context.Identifiers.Where(identifier => identifier.BookId == bookId).ToListAsync(cancellation).ConfigureAwait(false);
    }

    public async Task<Identifier?> GetOrCreateAsync(IdentifierResponse bookIdentifier, CancellationToken cancellation)
    {
        if (bookIdentifier is null)
        {
            throw new ArgumentNullException(nameof(bookIdentifier));
        }

        var identifier = await Context.Identifiers
            .FirstOrDefaultAsync(identifier => identifier.Isbn == bookIdentifier.Isbn && identifier.Type == bookIdentifier.Type, cancellation).ConfigureAwait(false);

        if (identifier is not null)
        {
            return identifier;
        }

        identifier = new Identifier(bookIdentifier.Isbn, bookIdentifier.Type);

        await Context.Identifiers.AddAsync(identifier, cancellation).ConfigureAwait(false);
        await Context.SaveChangesAsync(cancellation).ConfigureAwait(false);

        return identifier;
    }
}
