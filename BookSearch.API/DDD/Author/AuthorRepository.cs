﻿using BookSearch.API.Contexts;

using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.DDD.Author;

public class AuthorRepository : IAuthorRepository
{
    public AuthorRepository(DefaultContext context)
    {
        Context = context;
    }

    private DefaultContext Context { get; }

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
