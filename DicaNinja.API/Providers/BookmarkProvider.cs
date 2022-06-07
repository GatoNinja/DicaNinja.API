
using DicaNinja.API.Contexts;
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Services;

using Microsoft.EntityFrameworkCore;

namespace DicaNinja.API.Providers;

public class BookmarkProvider : IBookmarkProvider
{
    public BookmarkProvider(BaseContext context, IBookProvider bookProvider, BookGoogleService bookGoogleService)
    {
        Context = context;
        BookProvider = bookProvider;
        BookGoogleService = bookGoogleService;
    }

    private BaseContext Context { get; }
    private IBookProvider BookProvider { get; }
    private BookGoogleService BookGoogleService { get; }

    public async Task<int?> BookmarkAsync(Guid userId, string identifier, string type)
    {
        var existingBookmark = await FilterByUser(userId, identifier, type).FirstOrDefaultAsync();

        if (existingBookmark is not null)
        {
            Context.Remove(existingBookmark);
            await Context.SaveChangesAsync();
        }
        else
        {
            var book = await BookProvider.GetByIdentifierAsync(identifier, type);

            if (book is null)
            {
                book = await BookGoogleService.CreateBookFromGoogle(identifier);

                if (book is null)
                {
                    return null;
                }
            }

            var bookmark = new Bookmark(userId, book.Id);
            Context.Add(bookmark);
            await Context.SaveChangesAsync();
        }

        return await GetBookmarkCountAsync(userId);
    }

    public async Task<int> GetBookmarkCountAsync(Guid userId)
    {
        return await Context.Bookmarks.CountAsync(bookmark => bookmark.UserId == userId);
    }

    public async Task<bool> IsBookmarkedAsync(Guid userId, string identifier, string type)
    {
        return await FilterByUser(userId, identifier, type).AnyAsync();
    }

    private IQueryable<Bookmark> FilterByUser(Guid userId, string identifier, string type)
    {
        var query = from bookmark in Context.Bookmarks
                    join book in Context.Books on bookmark.BookId equals book.Id
                    where bookmark.UserId == userId && book.Identifiers.Any(i => i.Isbn == identifier && i.Type == type)
                    select bookmark;

        return query;
    }

}
