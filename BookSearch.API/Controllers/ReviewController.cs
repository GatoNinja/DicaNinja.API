using BookSearch.API.Abstracts;
using BookSearch.API.Models;
using BookSearch.API.Providers.Interfaces;
using BookSearch.API.Request;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ReviewController : ControllerHelper
{
    public ReviewController(IReviewProvider reviewProvider)
    {
        this.ReviewProvider = reviewProvider;
    }

    private IReviewProvider ReviewProvider { get; }

    [HttpPost]
    public async Task<Guid> CreateReview([FromBody] ReviewRequest request)
    {
        var (bookId, text, rating) = request;
        var review = new Review(this.UserId, bookId, text, rating);

        return await this.ReviewProvider.CreateReview(review);
    }
}
