using AutoMapper;

using BookSearch.API.Contexts;
using BookSearch.API.DDD.Author;
using BookSearch.API.DDD.BookCategory;
using BookSearch.API.DDD.BookIdentifier;

using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.DDD.Book;

public class BookRepository : IBookRepository
{
    public BookRepository(DefaultContext context, IMapper mapper, IBookIdentifierRepository bookIdentifierRepository, IAuthorRepository authorRepository, IBookCategoryRepository categoryRepository)
    {
        Context = context;
        Mapper = mapper;
        BookIdentifierRepository = bookIdentifierRepository;
        AuthorRepository = authorRepository;
        CategoryRepository = categoryRepository;
    }

    private DefaultContext Context { get; }
    private IMapper Mapper { get; }
    private IBookIdentifierRepository BookIdentifierRepository { get; }
    private IAuthorRepository AuthorRepository { get; }
    private IBookCategoryRepository CategoryRepository { get; }

    public async Task<Book?> CreateFromResponse(BookResponse response)
    {
        var book = Mapper.Map<Book>(response);
        
        book.Identifiers.RemoveAll(identifier => identifier.Id == Guid.Empty);
        book.Authors.RemoveAll(identifier => identifier.Id == Guid.Empty);
        book.Categories.RemoveAll(identifier => identifier.Id == Guid.Empty);
        
        foreach (var identifier in response.Identifiers)
        {
            var bookIdentifier = await BookIdentifierRepository.GetOrCreate(identifier);

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
}
