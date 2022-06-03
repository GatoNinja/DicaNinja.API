using BookSearch.API.Contexts;
using BookSearch.API.Models;
using BookSearch.API.Repository.Interfaces;
using BookSearch.API.Response;
using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.Repository;

public class IdentifierRepository : IIdentifierRepository
{
    public IdentifierRepository(BaseContext context)
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