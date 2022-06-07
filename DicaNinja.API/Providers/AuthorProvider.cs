
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

    public async Task<List<Author>> GetByBook(Guid bookId)
    {
        return await Context.Authors
            .Where(author => author.Books.Any(book => book.Id == bookId))
            .OrderBy(author => author.Name)
            .ToListAsync();
    }

    public async Task<int> GetCount()
    {
        return await Context.Authors.CountAsync();
    }

    public async Task<Author?> GetOrCreate(string authorName)
    {
        var author = await Context.Authors.FirstOrDefaultAsync(a => a.Name == authorName);

        if (author is not null)
        {
            return author;
        }

        author = new Author(authorName);

        Context.Authors.Add(author);

        return author;
    }
}
