
using DicaNinja.API.Contexts;
using DicaNinja.API.Models;

using DicaNinja.API.Providers.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace DicaNinja.API.Providers;

public class ReviewProvider : IReviewProvider
{
    public ReviewProvider(BaseContext context)
    {
        Context = context;
    }

    private BaseContext Context { get; }

    public async Task<Guid> CreateReviewAsync(Review review, CancellationToken cancellation)
    {
        if (review is null)
        {
            throw new ArgumentNullException(nameof(review));
        }

        var existingBook = await Context.Books.AnyAsync(book => book.Id == review.BookId, cancellation).ConfigureAwait(false);

        if (!existingBook)
        {
            throw new KeyNotFoundException("O livro informado não foi encontrado");
        }

        var existingUser = await Context.Users.AnyAsync(user => user.Id == review.UserId, cancellation).ConfigureAwait(false);

        if (!existingUser)
        {
            throw new KeyNotFoundException("O usuário informado não foi encontrado");
        }

        var existingReview = await Context.Reviews.AnyAsync(r => r.UserId == review.UserId && r.BookId == review.BookId, cancellation).ConfigureAwait(false);

        if (existingReview)
        {
            throw new KeyNotFoundException("Você já avaliou este livro");
        }

        await Context.Reviews.AddAsync(review, cancellation).ConfigureAwait(false);
        await Context.SaveChangesAsync(cancellation).ConfigureAwait(false);

        return review.Id;
    }
}
