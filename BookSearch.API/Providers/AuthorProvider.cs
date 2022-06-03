using BookSearch.API.Contexts;
using BookSearch.API.Models;
using BookSearch.API.Providers.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.Providers;

public class AuthorProvider : IAuthorProvider
{
    public AuthorProvider(BaseContext context)
    {
        Context = context;
    }

    private BaseContext Context { get; }

    public async Task<List<Author>> GetByBook(Guid bookId)
    {
        return await Context.Authors.Where(author => author.Books.Any(book => book.Id == bookId)).ToListAsync();
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
