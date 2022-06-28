
using DicaNinja.API.Abstracts;

using DicaNinja.API.Helpers;

using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Request;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<ActionResult> CreateBookmarkAsync([FromBody] BookmarkRequest request, CancellationToken cancellationToken)
    {
        var count = await BookmarkProvider.BookmarkAsync(UserId, request.Isbn, request.Type, cancellationToken);

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
        return await BookmarkProvider.GetBookmarkCountAsync(UserId, cancellationToken);
    }
}
