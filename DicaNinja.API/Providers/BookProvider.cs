using AutoMapper;

using DicaNinja.API.Contexts;
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Response;
using DicaNinja.API.Services;

using Microsoft.EntityFrameworkCore;

namespace DicaNinja.API.Providers;

public class BookProvider : IBookProvider
{
    public BookProvider(BaseContext context, IMapper mapper, BookGoogleService bookGoogleService)
    {
        Context = context;
        Mapper = mapper;
        BookGoogleService = bookGoogleService;
    }

    private BaseContext Context { get; }
    private IMapper Mapper { get; }
    private BookGoogleService BookGoogleService { get; }

    public async Task<Book?> GetByIdentifierAsync(string identifier, string type, CancellationToken cancellation)
    {
        return await Context.Books.FirstOrDefaultAsync(book => book.Identifiers.Any(i => i.Type == type && i.Isbn == identifier), cancellation).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Book>> GetBookmarksAsync(Guid userId, CancellationToken cancellation, int page, int perPage)
    {
        var query = from book in Context.Books
                    join bookmark in Context.Bookmarks on book.Id equals bookmark.BookId
                    where bookmark.UserId == userId
                    select book;

        var books = await query
            .OrderBy(book => book.Title)
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .Include(book => book.Identifiers)
            .Include(book => book.Authors)
            .ToListAsync(cancellation).ConfigureAwait(false);

        return books;
    }

    public async Task PopulateWithBookmarksAsync(IEnumerable<BookResponse> books, Guid userId, CancellationToken cancellation)
    {
        if (books is null)
        {
            return;
        }

        foreach (var book in books)
        {
            if (book?.Identifiers is null)
            {
                continue;
            }

            foreach (var identifier in book.Identifiers)
            {
                book.IsBookMarked = await IsBookmark(userId, identifier.Isbn, identifier.Type, cancellation).ConfigureAwait(false);

                if (book.IsBookMarked)
                {
                    continue;
                }
            }
        }
    }

    public async Task<IEnumerable<Review>> GetReviewsAsync(Guid bookId, CancellationToken cancellation, int page = 1, int perPage = 10)
    {
        var query = Context.Reviews.Where(review => review.BookId == bookId);

        return await query
            .OrderByDescending(review => review.Created)
            .Include(review => review.User)
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToListAsync(cancellation).ConfigureAwait(false);
    }

    public async Task<double> AverageRatingAsync(Guid bookId, CancellationToken cancellation)
    {
        return await Context.Reviews
            .Where(review => review.BookId == bookId)
            .Select(review => review.Rating)
            .DefaultIfEmpty()
            .AverageAsync(rating => rating, cancellation).ConfigureAwait(false);
    }

    public async Task<Book?> GetByIdAsync(Guid bookId, CancellationToken cancellation)
    {
        return await Context.Books.FirstOrDefaultAsync(book => book.Id == bookId, cancellation).ConfigureAwait(false);
    }

    private async Task<bool> IsBookmark(Guid userId, string identifier, string type, CancellationToken cancellation)
    {
        var query = from bookmark in Context.Bookmarks
                    join book in Context.Books on bookmark.BookId equals book.Id
                    where bookmark.UserId == userId && book.Identifiers.Any(i => i.Isbn == identifier && i.Type == type)
                    select bookmark;

        return await query.AnyAsync(cancellation).ConfigureAwait(false);
    }

    public async Task<IEnumerable<Book>> GetBooksAsync(CancellationToken cancellation, int page = 1, int perPage = 10)
    {
        return await Context.Books
            .OrderBy(book => Guid.NewGuid())
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .Include(book => book.Identifiers)
            .ToListAsync(cancellation).ConfigureAwait(false);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellation)
    {
        return await Context.Books.CountAsync(cancellation).ConfigureAwait(false);
    }

    public async Task<BookResponse?> GetByIsbnAsync(string isbn, string type, CancellationToken cancellation)
    {
        var book = await Context.Books.FirstOrDefaultAsync(book => book.Identifiers.Any(i => i.Isbn == isbn && i.Type == type), cancellation).ConfigureAwait(false);

        return book is null ? await CreateFromGoogleAsync(isbn, cancellation) : Mapper.Map<BookResponse>(book);
    }

    public async Task<BookResponse?> CreateFromGoogleAsync(string isbn, CancellationToken cancellation)
    {
        var bookGoogle = await BookGoogleService.GetFromIdentifierAsync(isbn, cancellation).ConfigureAwait(false);

        if (bookGoogle is null)
        {
            return null;
        }

        var newBook = await BookGoogleService.CreateFromResponse(bookGoogle, cancellation).ConfigureAwait(false);

        return Mapper.Map<BookResponse>(newBook);
    }

    public async Task<bool> GetExistingByIsbnAsync(string isbn, string type, CancellationToken cancellation)
    {
        return await Context.Identifiers.AnyAsync(i => i.Isbn == isbn && i.Type == type);
    }
}
