using DicaNinja.API.Models;

using DicaNinja.API.Abstracts;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Request;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ReviewController : ControllerHelper
{
    public ReviewController(IReviewProvider reviewProvider)
    {
        ReviewProvider = reviewProvider;
    }

    private IReviewProvider ReviewProvider { get; }

    [HttpPost]
    public async Task<Guid> CreateReview([FromBody] ReviewRequest request)
    {
        var (bookId, text, rating) = request;
        var review = new Review(UserId, bookId, text, rating);

        return await ReviewProvider.CreateReviewAsync(review);
    }
}
