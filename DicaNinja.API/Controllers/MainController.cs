
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
    public MainController(IMapper mapper, IBookProvider bookProvider, IUserProvider userProvider, IAuthorProvider authorProvider, ICategoryProvider categoriProvider, ICacheService cacheService, IBookmarkProvider bookmarkProvider)
    {
        Mapper = mapper;
        BookProvider = bookProvider;
        AuthorProvider = authorProvider;
        CategoriProvider = categoriProvider;
        CacheService = cacheService;
        BookmarkProvider = bookmarkProvider;
        UserProvider = userProvider;
    }

    private IMapper Mapper { get; }
    private IBookProvider BookProvider { get; }
    private IAuthorProvider AuthorProvider { get; }
    private ICategoryProvider CategoriProvider { get; }
    private ICacheService CacheService { get; }
    private IBookmarkProvider BookmarkProvider { get; }
    private IUserProvider UserProvider { get; }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<IEnumerable<BookResponse>>>> Get([FromQuery] QueryParameters query, CancellationToken cancellationToken)
    {
        var cacheKey = "main";

        var cache = CacheService.GetData<MainResponse>(cacheKey);

        if (cache is not null && false)
        {
            Console.WriteLine("Getting main page from cache");

            return Ok(cache);
        }

        var books = await BookProvider.GetBooksAsync(cancellationToken);
        var totalBooks = await BookProvider.GetCountAsync(cancellationToken);
        var totalAuthors = await AuthorProvider.GetCountAsync(cancellationToken);
        var totalCategories = await CategoriProvider.GetCountAsync(cancellationToken);
        var totalUsers = await UserProvider.GetCountAsync(cancellationToken);

        var mapped = Mapper.Map<IEnumerable<BookResponse>>(books);

        if (IsAuthenticated())
        {
            foreach (var book in mapped)
            {
                book.IsBookMarked = await BookmarkProvider.IsBookMarkedAsync(GetUserId(), book.Id);
            }
        }

        var paginated = PaginationHelper.CreatePagedResponse(mapped, query, totalBooks);
        var response = new MainResponse(paginated, totalBooks, totalAuthors, totalCategories, totalUsers);

        CacheService.SetData(cacheKey, response, DateTimeOffset.Now.AddMinutes(10));

        return Ok(response);
    }
}
