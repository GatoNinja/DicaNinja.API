using BookSearch.API.Models;

namespace BookSearch.API.Providers.Interfaces;

public interface IReviewProvider
{
    Task<Guid> CreateReview(Review review);
}
