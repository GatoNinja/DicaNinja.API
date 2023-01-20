
using AutoMapper;

using DicaNinja.API.Abstracts;
using DicaNinja.API.Cache;
using DicaNinja.API.Helpers;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Response;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Controllers;

[Route("[controller]")]
[ApiController]
[AllowAnonymous]
public class MainController : ControllerHelper
{
    public MainController(IMapper mapper, IBookProvider bookProvider, ICacheService cacheService, IBookmarkProvider bookmarkProvider)
    {
        Mapper = mapper;
        BookProvider = bookProvider;
        CacheService = cacheService;
        BookmarkProvider = bookmarkProvider;
    }

    private IMapper Mapper { get; }
    private IBookProvider BookProvider { get; }
    private ICacheService CacheService { get; }
    private IBookmarkProvider BookmarkProvider { get; }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<IEnumerable<BookResponse>>>> Get([FromQuery] QueryParametersWithFilter query, CancellationToken cancellation)
    {
        var cacheKey = "main";

        var cache = CacheService.GetData<PagedResponse<IEnumerable<BookResponse>>>(cacheKey);

        if (cache is not null && false)
        {
            Console.WriteLine("Getting main page from cache");

            return Ok(cache);
        }

        var books = await BookProvider.GetBooksAsync(cancellation);
        var totalBooks = await BookProvider.GetCountAsync(cancellation);

        var mapped = Mapper.Map<IEnumerable<BookResponse>>(books);

        if (IsAuthenticated())
        {
            foreach (var book in mapped)
            {
                book.IsBookMarked = await BookmarkProvider.IsBookMarkedAsync(GetUserId(), book.Id);
            }
        }

        var paginated = PaginationHelper.CreatePagedResponse(mapped, query, totalBooks);

        CacheService.SetData(cacheKey, paginated, DateTimeOffset.Now.AddMinutes(10));

        return Ok(paginated);
    }
}
