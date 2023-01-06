
using DicaNinja.API.Abstracts;

using DicaNinja.API.Helpers;

using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Request;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace DicaNinja.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class BookmarkController : ControllerHelper
{
    public BookmarkController(IBookmarkProvider bookmarkProvider)
    {
        BookmarkProvider = bookmarkProvider;
    }

    private IBookmarkProvider BookmarkProvider { get; }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status400BadRequest)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> CreateBookmarkAsync([FromBody] BookmarkRequest request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var count = await BookmarkProvider.BookmarkAsync(GetUserId(), request.Isbn, request.Type, cancellationToken).ConfigureAwait(false);

        if (count is not null)
        {
            return Ok(count);
        }

        var messageResponse = new MessageResponse("Ocorreu um problema salvando o livro");

        return new BadRequestObjectResult(messageResponse);

    }

    [HttpGet("count")]
    public async Task<int> GetCountAsync(CancellationToken cancellationToken)
    {
        return await BookmarkProvider.GetBookmarkCountAsync(GetUserId(), cancellationToken).ConfigureAwait(false);
    }
}
