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

    public Task<Book?> GetHintAsync(Guid userId, CancellationToken cancellationToken)
    {
        var query = from book in Context.Books
                    where !Context.Bookmarks.Any(bookmark => bookmark.UserId == userId)
                    && !Context.Hints.Any(hint => hint.UserId == userId && !hint.Liked)
                    select book;

        return query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> SetHintAccepted(Guid userId, Guid bookId, EnumHintStatus hintStatus, CancellationToken cancellationToken)
    {
        try
        {
            var hint = new Hint(userId, bookId, hintStatus);

            Context.Hints.Add(hint);
            await Context.SaveChangesAsync(cancellationToken);

            return true;
        }
        catch
        {
            throw;
        }
    }
}
