
using DicaNinja.API.Abstracts;

using DicaNinja.API.Models;
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
    public async Task<Guid> CreateReview([FromBody] ReviewRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var review = new Review(GetUserId(), request.BookId, request.Text, request.Rating);

        return await ReviewProvider.CreateReviewAsync(review, cancellationToken).ConfigureAwait(false);
    }
}
