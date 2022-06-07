using AutoMapper;

using DicaNinja.API.Abstracts;

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

    public BookController(BookGoogleService service, IBookProvider bookProvider, IMapper mapper, IAuthorProvider authorProvider, ICategoryProvider categoryProvider, IIdentifierProvider identifierProvider, IBookmarkProvider bookmarkProvider)
    {
        Service = service;
        BookProvider = bookProvider;
        Mapper = mapper;
        AuthorProvider = authorProvider;
        CategoryProvider = categoryProvider;
        IdentifierProvider = identifierProvider;
        BookmarkProvider = bookmarkProvider;
    }

    private BookGoogleService Service { get; }
    private IBookProvider BookProvider { get; }
    private IMapper Mapper { get; }
    private IAuthorProvider AuthorProvider { get; }
    private ICategoryProvider CategoryProvider { get; }
    private IIdentifierProvider IdentifierProvider { get; }
    private IBookmarkProvider BookmarkProvider { get; }

    [HttpGet]
    public async Task<ActionResult<List<BookResponse>>> GetAsync([FromQuery] string query)
    {
        var books = await Service.QueryBooks(query);

        await BookProvider.PopulateWithBookmarksAsync(books, UserId);

        return Ok(books);
    }

    [HttpGet("bookmark")]
    public async Task<ActionResult<List<BookResponse>>> GetBookmarks([FromQuery] QueryParameters query)
    {
        var books = await BookProvider.GetBookmarksAsync(UserId, query.Page, query.PerPage);
        var totalBookmarks = await BookmarkProvider.GetBookmarkCountAsync(UserId);
        var mapped = Mapper.Map<List<BookResponse>>(books);
        var paginated = PaginationHelper.CreatePagedResponse(mapped, query, totalBookmarks);

        return Ok(paginated);
    }

    [HttpGet("isbn/{isbn}/type/{type}")]
    public async Task<ActionResult<BookResponse>> GetBook([FromRoute] string isbn, [FromRoute] string type)
    {
        var book = await BookProvider.GetByIsbnAsync(isbn, type);

        if (book == null)
        {
            return NotFound();
        }

        var mapped = Mapper.Map<BookResponse>(book);
        var internalRating = await BookProvider.AverageRatingAsync(book.Id);

        mapped.InternalRating = internalRating;


        return Ok(mapped);
    }

    [HttpGet("{bookId:guid}/author")]
    public async Task<ActionResult<List<Author>>> GetAuthors([FromRoute] Guid bookId)
    {
        var authors = await AuthorProvider.GetByBookAsync(bookId);

        return Ok(authors);
    }

    [HttpGet("{bookId:guid}/identifier")]
    public async Task<ActionResult<List<Identifier>>> GetIdentifiers([FromRoute] Guid bookId)
    {
        var identifiers = await IdentifierProvider.GetByBookAsync(bookId);

        return Ok(identifiers);
    }

    [HttpGet("{bookId:guid}/category")]
    public async Task<ActionResult<List<Category>>> GetCategories([FromRoute] Guid bookId)
    {
        var categories = await CategoryProvider.GetByBookAsync(bookId);

        return Ok(categories);
    }

    [HttpGet("{bookId:guid}/review")]
    public async Task<ActionResult<List<Category>>> GetReviews([FromRoute] Guid bookId, [FromQuery] QueryParameters query)
    {
        var reviews = await BookProvider.GetReviewsAsync(bookId, query.Page, query.PerPage);

        return Ok(reviews);
    }

}
