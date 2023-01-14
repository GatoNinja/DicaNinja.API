using DicaNinja.API.Abstracts;
using DicaNinja.API.Providers.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DicaNinja.API.Response;
using DicaNinja.API.Request;
using DicaNinja.API.Helpers;

namespace DicaNinja.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class HintController : ControllerHelper
{
    public HintController(IHintProvider hintProvider, IMapper mapper)
    {
        HintProvider = hintProvider;
        Mapper = mapper;
    }

    public IHintProvider HintProvider { get; }
    public IMapper Mapper { get; }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Get(CancellationToken cancellation)
    {
        var book = await HintProvider.GetHintAsync(GetUserId(), cancellation);

        if (book is null)
        {
            return NotFound();
        }

        var mapped = Mapper.Map<BookResponse>(book);

        return Ok(mapped);
    }

    [HttpGet("liked")]
    public async Task<IActionResult> GetLiked([FromQuery] QueryParametersWithFilter query, CancellationToken cancellation)
    {
        var books = await HintProvider.GetHintsLikedAsync(GetUserId(), cancellation, query.Page, query.PerPage);
        var totalLiked = await HintProvider.TotalLikedAsync(GetUserId(), cancellation);
        var mapped = Mapper.Map<BookResponse[]>(books);
        var paginated = PaginationHelper.CreatePagedResponse(mapped, query, totalLiked);

        return Ok(paginated);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] HintRequest payload, CancellationToken cancellation)
    {
        var response = await HintProvider.SetHintAccepted(GetUserId(), payload.Book, payload.Status, cancellation);

        return Ok(response);
    }
}
