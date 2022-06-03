using BookSearch.API.Contexts;
using BookSearch.API.Models;
using BookSearch.API.Repository.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.Repository;

public class ReviewRepository : IReviewRepository
{
    public ReviewRepository(BaseContext context)
    {
        Context = context;
    }

    private BaseContext Context { get; }

    public async Task<Guid> CreateReview(Review review)
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