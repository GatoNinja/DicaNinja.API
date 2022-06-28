
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

    public async Task<List<Author>> GetByBookAsync(Guid bookId, CancellationToken cancellationToken)
    {
        return await Context.Authors
            .Where(author => author.Books.Any(book => book.Id == bookId))
            .OrderBy(author => author.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken)
    {
        return await Context.Authors.CountAsync(cancellationToken);
    }

    public async Task<Author?> GetOrCreateAsync(string authorName, CancellationToken cancellationToken)
    {
        var author = await Context.Authors.FirstOrDefaultAsync(a => a.Name == authorName, cancellationToken);

        if (author is not null)
        {
            return author;
        }

        author = new Author(authorName);

        await Context.Authors.AddAsync(author, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);

        return author;
    }
}
