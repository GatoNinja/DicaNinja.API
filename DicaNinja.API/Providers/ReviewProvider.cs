
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

    public async Task<Guid> CreateReviewAsync(Review review)
    {
        var existingBook = await Context.Books.AnyAsync(book => book.Id == review.BookId);

        if (!existingBook)
        {
            throw new Exception("O livro informado não foi encontrado");
        }

        var existingUser = await Context.Users.AnyAsync(user => user.Id == review.UserId);

        if (!existingUser)
        {
            throw new Exception("O usuário informado não foi encontrado");
        }

        var existingReview = await Context.Reviews.AnyAsync(r => r.UserId == review.UserId && r.BookId == review.BookId);

        if (existingReview)
        {
            throw new Exception("Você já avaliou este livro");
        }

        await Context.Reviews.AddAsync(review);
        await Context.SaveChangesAsync();

        return review.Id;
    }
}
