using BookSearch.API.Contexts;
using BookSearch.API.Models;
using BookSearch.API.Repository.Interfaces;
using BookSearch.API.Services;
using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.Repository;

public class BookmarkRepository : IBookmarkRepository
{
    public BookmarkRepository(DefaultContext context, IBookRepository bookRepository, BookGoogleService bookGoogleService)
    {
        Context = context;
        BookRepository = bookRepository;
        BookGoogleService = bookGoogleService;
    }

    private DefaultContext Context { get; }
    private IBookRepository BookRepository { get; }
    private BookGoogleService BookGoogleService { get; }

    public async Task<int?> Bookmark(Guid userId, string identifier, string type)
    {
        var existingBookmark = await FilterByUser(userId, identifier, type).FirstOrDefaultAsync();

        if (existingBookmark is not null)
        {
            Context.Remove(existingBookmark);
            await Context.SaveChangesAsync();
        }
        else
        {
            var book = await BookRepository.GetByIdentifier(identifier, type);

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

        return await GetBookmarkCount(userId);
    }

    public async Task<int> GetBookmarkCount(Guid userId)
    {
        return await Context.Bookmarks.CountAsync(bookmark => bookmark.UserId == userId);
    }

    public async Task<bool> IsBookmarked(Guid userId, string identifier, string type)
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
