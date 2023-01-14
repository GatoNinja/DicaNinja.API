using DicaNinja.API.Contexts;
using DicaNinja.API.Enums;
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace DicaNinja.API.Providers;

public class HintProvider : IHintProvider
{

    public HintProvider(BaseContext context)
    {
        Context = context;
    }

    private BaseContext Context { get; }

    public Task<Book?>? GetHintAsync(Guid userId, CancellationToken cancellation)
    {
        var query = from book in Context.Books
                    where !Context.Bookmarks.Any(bookmark => bookmark.UserId == userId && bookmark.BookId == book.Id)
                    && !Context.Hints.Any(hint => hint.UserId == userId && !hint.Liked && hint.BookId == book.Id)
                    orderby Guid.NewGuid()
                    select book;

        var bookFromCategory = GetRecommendedByCategories(userId, query, cancellation);

        if (bookFromCategory is not null)
        {
            return bookFromCategory;
        }

        //var bookFromAuthor = GetRecommendedByAuthor(userId, query);

        //if (bookFromAuthor is not null)
        //{
        //    return bookFromAuthor;
        //}

        //var bookFromFollowing = GetRecommendedByFollowing(userId, query);

        //if (bookFromFollowing is not null)
        //{
        //    return bookFromFollowing;
        //}

        return null;
    }

    private async Task<Book?> GetRecommendedByCategories(Guid userId, IOrderedQueryable<Book> query, CancellationToken cancellation)
    {
        var userCategories = from category in Context.Categories
                             join book in Context.Books on category.BookId equals book.Id
                             join bookmark in Context.Bookmarks on book.Id equals bookmark.BookId
                             where bookmark.UserId == userId
                             select category;

        var queryWithCategories = from book in query
                                  join category in Context.Categories on book.Id equals category.BookId
                                  where userCategories.Any(userCategory => book.Categories.Any(bookCategory => bookCategory.Id == userCategory.Id))
                                  select book;

        return await queryWithCategories.FirstOrDefaultAsync(cancellation);
    }

    public async Task<IEnumerable<Book>> GetHintsLikedAsync(Guid userId, CancellationToken cancellation, int page = 1, int perPage = 10)
    {
        var query = from hint in Context.Hints
                    where hint.Liked
                    && hint.UserId == userId
                    select hint.Book;

        return await query.Take(perPage).Skip((page - 1) * perPage).ToListAsync(cancellation);
    }

    public async Task<bool> SetHintAccepted(Guid userId, Guid bookId, EnumHintStatus hintStatus, CancellationToken cancellation)
    {
        try
        {
            var hint = new Hint(userId, bookId, hintStatus);

            Context.Hints.Add(hint);
            await Context.SaveChangesAsync(cancellation);

            return true;
        }
        catch
        {
            throw;
        }
    }

    public async Task<int> TotalLikedAsync(Guid userId, CancellationToken cancellation)
    {
        return await Context.Hints.CountAsync(hint => hint.UserId == userId, cancellation);
    }
}
