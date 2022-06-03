using BookSearch.API.Abstracts;
using BookSearch.API.Models;
using BookSearch.API.Repository.Interfaces;
using BookSearch.API.Request;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class ReviewController : ControllerHelper
{
    public ReviewController(IReviewRepository reviewRepository)
    {
        ReviewRepository = reviewRepository;
    }

    private IReviewRepository ReviewRepository { get; }

    [HttpPost]
    public async Task<Guid> CreateReview([FromBody] ReviewRequest request)
    {
        var (bookId, text, rating) = request;
        var review = new Review(UserId, bookId, text, rating);

        return await ReviewRepository.CreateReview(review);
    }
}