using DicaNinja.API.Abstracts;
using DicaNinja.API.Providers.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using DicaNinja.API.Response;

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
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var book = await HintProvider.GetHintAsync(GetUserId(), cancellationToken);

        if (book is null)
        {
            return NotFound();
        }

        var mapped = Mapper.Map<BookResponse>(book);

        return Ok(mapped);
    }
}
