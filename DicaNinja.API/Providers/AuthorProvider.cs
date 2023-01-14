
using DicaNinja.API.Contexts;
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace DicaNinja.API.Providers;

public class AuthorProvider : IAuthorProvider
{
    public AuthorProvider(BaseContext context)
    {
        Context = context;
    }

    private BaseContext Context { get; }

    public async Task<IEnumerable<Author>> GetByBookAsync(Guid bookId, CancellationToken cancellation)
    {
        return await Context.Authors
            .Where(author => author.Books.Any(book => book.Id == bookId))
            .OrderBy(author => author.Name)
            .ToListAsync(cancellation).ConfigureAwait(false);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellation)
    {
        return await Context.Authors.CountAsync(cancellation).ConfigureAwait(false);
    }

    public async Task<Author?> GetOrCreateAsync(string authorName, CancellationToken cancellation)
    {
        var author = await Context.Authors.FirstOrDefaultAsync(a => a.Name == authorName, cancellation).ConfigureAwait(false);

        if (author is not null)
        {
            return author;
        }

        author = new Author(authorName);

        await Context.Authors.AddAsync(author, cancellation).ConfigureAwait(false);
        await Context.SaveChangesAsync(cancellation).ConfigureAwait(false);

        return author;
    }
}
