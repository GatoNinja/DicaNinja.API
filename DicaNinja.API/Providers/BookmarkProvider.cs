
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

    public async Task<bool?> BookmarkAsync(Guid userId, string identifier, string type, CancellationToken cancellationToken)
    {
        var existingBookmark = await FilterByUser(userId, identifier, type).FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);

        if (existingBookmark is not null)
        {
            Context.Remove(existingBookmark);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return false;
        }
        else
        {
            var book = await BookProvider.GetByIdentifierAsync(identifier, type, cancellationToken).ConfigureAwait(false);

            if (book is null)
            {
                book = await BookGoogleService.CreateBookFromGoogleAsync(identifier, cancellationToken).ConfigureAwait(false);

                if (book is null)
                {
                    return null;
                }
            }

            var bookmark = new Bookmark(userId, book.Id);
            Context.Add(bookmark);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return true;
        }
    }

    public async Task<int> GetBookmarkCountAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await Context.Bookmarks.CountAsync(bookmark => bookmark.UserId == userId, cancellationToken).ConfigureAwait(false);
    }

    public async Task<bool> IsBookMarkedAsync(Guid userId, string identifier, string type, CancellationToken cancellationToken)
    {
        return await FilterByUser(userId, identifier, type).AnyAsync(cancellationToken).ConfigureAwait(false);
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
