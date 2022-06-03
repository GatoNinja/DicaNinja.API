using AutoMapper;

using DicaNinja.API.Contexts;
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Response;

using Microsoft.EntityFrameworkCore;

namespace DicaNinja.API.Providers;

public class BookProvider : IBookProvider
{
    public BookProvider(BaseContext context, IMapper mapper, IIdentifierProvider identifierProvider, IAuthorProvider authorProvider, ICategoryProvider categoryProvider)
    {
        this.Context = context;
        this.Mapper = mapper;
        this.IdentifierProvider = identifierProvider;
        this.AuthorProvider = authorProvider;
        this.CategoryProvider = categoryProvider;
    }

    private BaseContext Context { get; }
    private IMapper Mapper { get; }
    private IIdentifierProvider IdentifierProvider { get; }
    private IAuthorProvider AuthorProvider { get; }
    private ICategoryProvider CategoryProvider { get; }

    public async Task<Book?> CreateFromResponse(BookResponse response)
    {
        var book = this.Mapper.Map<Book>(response);

        book.Identifiers.RemoveAll(identifier => identifier.Id == Guid.Empty);
        book.Authors.RemoveAll(identifier => identifier.Id == Guid.Empty);
        book.Categories.RemoveAll(identifier => identifier.Id == Guid.Empty);

        foreach (var identifier in response.Identifiers)
        {
            var bookIdentifier = await this.IdentifierProvider.GetOrCreate(identifier);

            if (bookIdentifier is null)
            {
                continue;
            }

            book.Identifiers.Add(bookIdentifier);
        }

        foreach (var author in response.Authors)
        {
            var authorEntity = await this.AuthorProvider.GetOrCreate(author);

            if (authorEntity is null)
            {
                continue;
            }

            book.Authors.Add(authorEntity);
        }

        foreach (var category in response.Categories)
        {
            var categoryEntity = await this.CategoryProvider.GetOrCreate(category);

            if (categoryEntity is null)
            {
                continue;
            }

            book.Categories.Add(categoryEntity);
        }

        this.Context.Books.Add(book);

        await this.Context.SaveChangesAsync();

        return book;
    }

    public async Task<Book?> GetByIdentifier(string identifier, string type)
    {
        return await this.Context.Books.FirstOrDefaultAsync(book => book.Identifiers.Any(i => i.Type == type && i.Isbn == identifier));
    }

    public async Task<List<Book>> GetBookmarks(Guid userId, int page, int perPage)
    {
        var query = from book in this.Context.Books
                    join bookmark in this.Context.Bookmarks on book.Id equals bookmark.BookId
                    where bookmark.UserId == userId
                    select book;

        return await query
            .OrderBy(book => book.Title)
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToListAsync();
    }

    public async Task PopulateWithBookmarks(IEnumerable<BookResponse> books, Guid userId)
    {
        foreach (var book in books)
        {
            if (book?.Identifiers is null)
            {
                continue;
            }

            foreach (var identifier in book.Identifiers)
            {
                book.IsBookmarked = await this.IsBookmark(userId, identifier.Isbn, identifier.Type);

                if (book.IsBookmarked)
                {
                    continue;
                }
            }
        }
    }

    public async Task<IEnumerable<Review>> GetReviews(Guid bookId, int page = 1, int perPage = 10)
    {
        var query = this.Context.Reviews.Where(review => review.BookId == bookId);

        return await query
            .OrderByDescending(review => review.Created)
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToListAsync();
    }

    public async Task<double> AverageRating(Guid bookId)
    {
        return await this.Context.Reviews
            .Where(review => review.BookId == bookId)
            .Select(review => review.Rating)
            .DefaultIfEmpty()
            .AverageAsync(rating => rating);
    }

    public async Task<Book?> GetById(Guid bookId)
    {
        return await this.Context.Books.FirstOrDefaultAsync(book => book.Id == bookId);
    }

    private async Task<bool> IsBookmark(Guid userId, string identifier, string type)
    {
        var query = from bookmark in this.Context.Bookmarks
                    join book in this.Context.Books on bookmark.BookId equals book.Id
                    where bookmark.UserId == userId && book.Identifiers.Any(i => i.Isbn == identifier && i.Type == type)
                    select bookmark;

        return await query.AnyAsync();
    }

}
