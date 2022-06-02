using AutoMapper;

using BookSearch.API.Abstracts;
using BookSearch.API.Helpers;
using BookSearch.API.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.Request;

[Route("[controller]")]
[ApiController]
[Authorize]
public class BookmarkController : ControllerHelper
{
    public BookmarkController(IBookmarkRepository bookmarkRepository, IMapper mapper)
    {
        BookmarkRepository = bookmarkRepository;
        Mapper = mapper;
    }

    private IBookmarkRepository BookmarkRepository { get; }
    private IMapper Mapper { get; }

    [HttpPost]
    public async Task<ActionResult> CreateBookmark([FromBody] BookmarkRequest request)
    {
        var count = await BookmarkRepository.Bookmark(UserId, request.Isbn, request.Type);

        if (count is null)
        {
            var messageResponse = new MessageResponse("Ocorreu um problema salvando o livro");

            return new BadRequestObjectResult(messageResponse);
        }

        return Ok(count);
    }

    [HttpGet("count")]
    public async Task<int> GetCount()
    {
        return await BookmarkRepository.GetBookmarkCount(UserId);
    }
}
