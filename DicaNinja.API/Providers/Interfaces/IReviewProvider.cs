
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface IReviewProvider
{
    Task<Guid> CreateReview(Review review);
}
