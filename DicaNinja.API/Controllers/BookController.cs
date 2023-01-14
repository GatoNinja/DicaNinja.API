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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<List<BookResponse>>> GetAsync([FromQuery] QueryParametersWithFilter request, CancellationToken cancellation)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var cacheKey = $"googlebook_{request.Query}-{request.Page}-{request.PerPage}";

        var cache = CacheService.GetData<List<BookResponse>>(cacheKey);

        if (cache is not null)
        {
            return Ok(cache);
        }

        List<BookResponse> books;

        try
        {
            books = await Service.QueryBooksAsync(request.Query, cancellation, request.Page, request.PerPage, request.Lang).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);

            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao chamar a API do Google Books");
        }

        await BookProvider.PopulateWithBookmarksAsync(books, GetUserId(), cancellation).ConfigureAwait(false);

        CacheService.SetData(cacheKey, books, DateTimeOffset.Now.AddMinutes(5));

        return Ok(books);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    [AllowAnonymous]
    public async Task<ActionResult<List<BookResponse>>> GetByAsync([FromRoute] Guid id, CancellationToken cancellation)
    {
        try
        {
            var book = await BookProvider.GetByIdAsync(id, cancellation);

            if (book is null)
            {
                return NotFound();
            }

            var mapped = Mapper.Map<BookResponse>(book);

            if (IsAuthenticated())
            {
                mapped.IsBookMarked = await BookmarkProvider.IsBookMarkedAsync(GetUserId(), book.Id);
            }

            return Ok(mapped);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);

            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao chamar a API do Google Books");
        }
    }

    [HttpGet("bookmark")]
    public async Task<ActionResult<List<BookResponse>>> GetBookmarksAsync([FromQuery] QueryParametersWithFilter query, CancellationToken cancellation)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        var books = await BookProvider.GetBookmarksAsync(GetUserId(), cancellation, query.Page, query.PerPage).ConfigureAwait(false);
        var totalBookmarks = await BookmarkProvider.GetBookmarkCountAsync(GetUserId(), cancellation).ConfigureAwait(false);
        var mapped = Mapper.Map<List<BookResponse>>(books);
        var paginated = PaginationHelper.CreatePagedResponse(mapped, query, totalBookmarks);

        foreach (var book in paginated.Data)
        {
            book.IsBookMarked = true;
        }

        return Ok(paginated);
    }

    [HttpGet("isbn/{isbn}/type/{type}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<BookResponse>> GetBookAsync([FromRoute] string isbn, [FromRoute] string type, CancellationToken cancellation)
    {
        var book = await BookProvider.GetByIsbnAsync(isbn, type, cancellation).ConfigureAwait(false);

        if (book == null)
        {
            return NotFound();
        }

        var mapped = Mapper.Map<BookResponse>(book);
        var internalRating = await BookProvider.AverageRatingAsync(book.Id, cancellation).ConfigureAwait(false);

        mapped.InternalRating = internalRating;

        return Ok(mapped);
    }

    [HttpGet("{bookId:guid}/author")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Author>>> GetAuthorsAsync([FromRoute] Guid bookId, CancellationToken cancellation)
    {
        var authors = await AuthorProvider.GetByBookAsync(bookId, cancellation).ConfigureAwait(false);

        return Ok(authors);
    }

    [HttpGet("{bookId:guid}/identifier")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Identifier>>> GetIdentifiersAsync([FromRoute] Guid bookId, CancellationToken cancellation)
    {
        var identifiers = await IdentifierProvider.GetByBookAsync(bookId, cancellation).ConfigureAwait(false);

        return Ok(identifiers);
    }

    [HttpGet("{bookId:guid}/category")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Category>>> GetCategoriesAsync([FromRoute] Guid bookId, CancellationToken cancellation)
    {
        var categories = await CategoryProvider.GetByBookAsync(bookId, cancellation).ConfigureAwait(false);

        return Ok(categories);
    }

    [HttpGet("{bookId:guid}/review")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Review>>> GetReviewsAsync([FromRoute] Guid bookId, [FromQuery] QueryParametersWithFilter query, CancellationToken cancellation)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        var reviews = await BookProvider.GetReviewsAsync(bookId, cancellation, query.Page, query.PerPage).ConfigureAwait(false);

        return Ok(reviews);
    }
}
