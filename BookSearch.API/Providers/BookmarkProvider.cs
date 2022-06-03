using BookSearch.API.Contexts;
using BookSearch.API.Models;
using BookSearch.API.Providers.Interfaces;
using BookSearch.API.Services;
using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.Providers;

public class BookmarkProvider : IBookmarkProvider
{
    public BookmarkProvider(BaseContext context, IBookProvider bookProvider, BookGoogleService bookGoogleService)
    {
        this.Context = context;
        this.BookProvider = bookProvider;
        this.BookGoogleService = bookGoogleService;
    }

    private BaseContext Context { get; }
    private IBookProvider BookProvider { get; }
    private BookGoogleService BookGoogleService { get; }

    public async Task<int?> Bookmark(Guid userId, string identifier, string type)
    {
        var existingBookmark = await this.FilterByUser(userId, identifier, type).FirstOrDefaultAsync();

        if (existingBookmark is not null)
        {
            this.Context.Remove(existingBookmark);
            await this.Context.SaveChangesAsync();
        }
        else
        {
            var book = await this.BookProvider.GetByIdentifier(identifier, type);

            if (book is null)
            {
                book = await this.BookGoogleService.CreateBookFromGoogle(identifier);

                if (book is null)
                {
                    return null;
                }
            }

            var bookmark = new Bookmark(userId, book.Id);
            this.Context.Add(bookmark);
            await this.Context.SaveChangesAsync();
        }

        return await this.GetBookmarkCount(userId);
    }

    public async Task<int> GetBookmarkCount(Guid userId)
    {
        return await this.Context.Bookmarks.CountAsync(bookmark => bookmark.UserId == userId);
    }

    public async Task<bool> IsBookmarked(Guid userId, string identifier, string type)
    {
        return await this.FilterByUser(userId, identifier, type).AnyAsync();
    }

    private IQueryable<Bookmark> FilterByUser(Guid userId, string identifier, string type)
    {
        var query = from bookmark in this.Context.Bookmarks
                    join book in this.Context.Books on bookmark.BookId equals book.Id
                    where bookmark.UserId == userId && book.Identifiers.Any(i => i.Isbn == identifier && i.Type == type)
                    select bookmark;

        return query;
    }

}
