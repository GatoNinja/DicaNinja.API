using BookSearch.API.Abstracts;
using BookSearch.API.Helpers;
using BookSearch.API.Repository.Interfaces;
using BookSearch.API.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class BookmarkController : ControllerHelper
{
    public BookmarkController(IBookmarkRepository bookmarkRepository)
    {
        BookmarkRepository = bookmarkRepository;
    }

    private IBookmarkRepository BookmarkRepository { get; }

    [HttpPost]
    public async Task<ActionResult> CreateBookmark([FromBody] BookmarkRequest request)
    {
        var count = await BookmarkRepository.Bookmark(UserId, request.Isbn, request.Type);

        if (count is not null)
        {
            return Ok(count);
        }

        var messageResponse = new MessageResponse("Ocorreu um problema salvando o livro");

        return new BadRequestObjectResult(messageResponse);

    }

    [HttpGet("count")]
    public async Task<int> GetCount()
    {
        return await BookmarkRepository.GetBookmarkCount(UserId);
    }
}