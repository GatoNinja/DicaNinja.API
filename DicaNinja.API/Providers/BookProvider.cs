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

    public async Task<Book?> GetByIdentifier(string identifier, string type)
    {
        return await Context.Books.FirstOrDefaultAsync(book => book.Identifiers.Any(i => i.Type == type && i.Isbn == identifier));
    }

    public async Task<List<Book>> GetBookmarks(Guid userId, int page, int perPage)
    {
        var query = from book in Context.Books
                    join bookmark in Context.Bookmarks on book.Id equals bookmark.BookId
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
                book.IsBookmarked = await IsBookmark(userId, identifier.Isbn, identifier.Type);

                if (book.IsBookmarked)
                {
                    continue;
                }
            }
        }
    }

    public async Task<IEnumerable<Review>> GetReviews(Guid bookId, int page = 1, int perPage = 10)
    {
        var query = Context.Reviews.Where(review => review.BookId == bookId);

        return await query
            .OrderByDescending(review => review.Created)
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToListAsync();
    }

    public async Task<double> AverageRating(Guid bookId)
    {
        return await Context.Reviews
            .Where(review => review.BookId == bookId)
            .Select(review => review.Rating)
            .DefaultIfEmpty()
            .AverageAsync(rating => rating);
    }

    public async Task<Book?> GetById(Guid bookId)
    {
        return await Context.Books.FirstOrDefaultAsync(book => book.Id == bookId);
    }

    private async Task<bool> IsBookmark(Guid userId, string identifier, string type)
    {
        var query = from bookmark in Context.Bookmarks
                    join book in Context.Books on bookmark.BookId equals book.Id
                    where bookmark.UserId == userId && book.Identifiers.Any(i => i.Isbn == identifier && i.Type == type)
                    select bookmark;

        return await query.AnyAsync();
    }

    public async Task<IEnumerable<Book>> GetBooks(int page = 1, int perPage = 10)
    {
        return await Context.Books
            .OrderBy(book => Guid.NewGuid())
            .Skip((page - 1) * perPage)
            .Take(perPage)
            .ToListAsync();
    }

    public async Task<int> GetCount()
    {
        return await Context.Books.CountAsync();
    }

    public async Task<BookResponse?> GetByIsbn(string isbn, string type)
    {
        var book = await Context.Books.FirstOrDefaultAsync(book => book.Identifiers.Any(i => i.Isbn == isbn && i.Type == type));

        if (book is null)
        {
            var bookGoogle = await BookGoogleService.GetFromIdentifier(isbn);

            if (bookGoogle is null)
            {
                return null;
            }

            var newBook = await BookGoogleService.CreateFromResponse(bookGoogle);

            return Mapper.Map<BookResponse>(newBook);
        }

        return Mapper.Map<BookResponse>(book);
    }

}
