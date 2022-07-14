using AutoMapper;

using DicaNinja.API.Abstracts;
using DicaNinja.API.Cache;
using DicaNinja.API.Helpers;
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Response;
using DicaNinja.API.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class BookController : ControllerHelper
{

    public BookController(BookGoogleService service, IBookProvider bookProvider, IMapper mapper, IAuthorProvider authorProvider, ICategoryProvider categoryProvider, IIdentifierProvider identifierProvider, IBookmarkProvider bookmarkProvider, ICacheService cacheService)
    {
        Service = service;
        BookProvider = bookProvider;
        Mapper = mapper;
        AuthorProvider = authorProvider;
        CategoryProvider = categoryProvider;
        IdentifierProvider = identifierProvider;
        BookmarkProvider = bookmarkProvider;
        CacheService = cacheService;
    }

    private BookGoogleService Service { get; }
    private IBookProvider BookProvider { get; }
    private IMapper Mapper { get; }
    private IAuthorProvider AuthorProvider { get; }
    private ICategoryProvider CategoryProvider { get; }
    private IIdentifierProvider IdentifierProvider { get; }
    private IBookmarkProvider BookmarkProvider { get; }
    private ICacheService CacheService { get; }

    [HttpGet]
    public async Task<ActionResult<List<BookResponse>>> GetAsync([FromQuery] QueryParametersWithFilter queryString, CancellationToken cancellationToken)
    {
        var cacheKey = $"googlebook_{queryString.Filter}-{queryString.Page}-{queryString.PerPage}";

        var cache = CacheService.GetData<List<BookResponse>>(cacheKey);

        if (cache is not null)
        {
            return Ok(cache);
        }

        var books = await Service.QueryBooksAsync(queryString.Filter, cancellationToken, queryString.Page, queryString.PerPage);

        await BookProvider.PopulateWithBookmarksAsync(books, UserId, cancellationToken);

        CacheService.SetData(cacheKey, books, DateTimeOffset.Now.AddMinutes(5));

        return Ok(books);
    }

    [HttpGet("bookmark")]
    public async Task<ActionResult<List<BookResponse>>> GetBookmarksAsync([FromQuery] QueryParameters query, CancellationToken cancellationToken)
    {
        var books = await BookProvider.GetBookmarksAsync(UserId, cancellationToken, query.Page, query.PerPage);
        var totalBookmarks = await BookmarkProvider.GetBookmarkCountAsync(UserId, cancellationToken);
        var mapped = Mapper.Map<List<BookResponse>>(books);
        var paginated = PaginationHelper.CreatePagedResponse(mapped, query, totalBookmarks);

        foreach (var book in paginated.Data)
        {
            book.IsBookMarked = true;
        }

        return Ok(paginated);
    }

    [HttpGet("isbn/{isbn}/type/{type}")]
    public async Task<ActionResult<BookResponse>> GetBookAsync([FromRoute] string isbn, [FromRoute] string type, CancellationToken cancellationToken)
    {
        var book = await BookProvider.GetByIsbnAsync(isbn, type, cancellationToken);

        if (book == null)
        {
            return NotFound();
        }

        var mapped = Mapper.Map<BookResponse>(book);
        var internalRating = await BookProvider.AverageRatingAsync(book.Id, cancellationToken);

        mapped.InternalRating = internalRating;

        return Ok(mapped);
    }

    [HttpGet("{bookId:guid}/author")]
    public async Task<ActionResult<List<Author>>> GetAuthorsAsync([FromRoute] Guid bookId, CancellationToken cancellationToken)
    {
        var authors = await AuthorProvider.GetByBookAsync(bookId, cancellationToken);

        return Ok(authors);
    }

    [HttpGet("{bookId:guid}/identifier")]
    public async Task<ActionResult<List<Identifier>>> GetIdentifiersAsync([FromRoute] Guid bookId, CancellationToken cancellationToken)
    {
        var identifiers = await IdentifierProvider.GetByBookAsync(bookId, cancellationToken);

        return Ok(identifiers);
    }

    [HttpGet("{bookId:guid}/category")]
    public async Task<ActionResult<List<Category>>> GetCategoriesAsync([FromRoute] Guid bookId, CancellationToken cancellationToken)
    {
        var categories = await CategoryProvider.GetByBookAsync(bookId, cancellationToken);

        return Ok(categories);
    }

    [HttpGet("{bookId:guid}/review")]
    public async Task<ActionResult<List<Category>>> GetReviewsAsync([FromRoute] Guid bookId, [FromQuery] QueryParameters query, CancellationToken cancellationToken)
    {
        var reviews = await BookProvider.GetReviewsAsync(bookId, cancellationToken, query.Page, query.PerPage);

        return Ok(reviews);
    }
}
