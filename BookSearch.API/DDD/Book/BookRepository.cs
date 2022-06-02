﻿using AutoMapper;

using BookSearch.API.Contexts;
using BookSearch.API.DDD.Author;
using BookSearch.API.DDD.Identifier;
using BookSearch.API.DDD.Category;

using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.DDD.Book;

public class BookRepository : IBookRepository
{
    public BookRepository(DefaultContext context, IMapper mapper, IIdentifierRepository identifierRepository, IAuthorRepository authorRepository, ICategoryRepository categoryRepository)
    {
        Context = context;
        Mapper = mapper;
        IdentifierRepository = identifierRepository;
        AuthorRepository = authorRepository;
        CategoryRepository = categoryRepository;
    }

    private DefaultContext Context { get; }
    private IMapper Mapper { get; }
    private IIdentifierRepository IdentifierRepository { get; }
    private IAuthorRepository AuthorRepository { get; }
    private ICategoryRepository CategoryRepository { get; }

    public async Task<Book?> CreateFromResponse(BookResponse response)
    {
        var book = Mapper.Map<Book>(response);
        
        book.Identifiers.RemoveAll(identifier => identifier.Id == Guid.Empty);
        book.Authors.RemoveAll(identifier => identifier.Id == Guid.Empty);
        book.Categories.RemoveAll(identifier => identifier.Id == Guid.Empty);
        
        foreach (var identifier in response.Identifiers)
        {
            var bookIdentifier = await IdentifierRepository.GetOrCreate(identifier);

            if (bookIdentifier is null)
            {
                continue;
            }

            book.Identifiers.Add(bookIdentifier);
        }

        foreach (var author in response.Authors)
        {
            var authorEntity = await AuthorRepository.GetOrCreate(author);

            if (authorEntity is null)
            {
                continue;
            }

            book.Authors.Add(authorEntity);
        }

        foreach (var category in response.Categories)
        {
            var categoryEntity = await CategoryRepository.GetOrCreate(category);

            if (categoryEntity is null)
            {
                continue;
            }

            book.Categories.Add(categoryEntity);
        }

        Context.Books.Add(book);
        
        await Context.SaveChangesAsync();

        return book;
    }

    public async Task<Book?> GetByIdentifier(string identifier, string type)
    {
        return await Context.Books.FirstOrDefaultAsync(book => book.Identifiers.Any(i => i.Type == type && i.Isbn == identifier));
    }

    public async Task<List<Book>> GetFavorites(Guid userId, int page, int perPage)
    {
        var query = from book in Context.Books
                    join favorite in Context.Favorites on book.Id equals favorite.BookId
                    where favorite.UserId == userId
                    select book;

        return await query
               .OrderBy(book => book.Title)
               .Skip((page - 1) * perPage)
               .Take(perPage)
               .ToListAsync();
    }
}
