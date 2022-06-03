using BookSearch.API.Models;

namespace BookSearch.API.Repository.Interfaces;

public interface IReviewRepository
{
    Task<Guid> CreateReview(Review review);
}