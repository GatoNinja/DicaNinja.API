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
        this.Service = service;
        this.BookProvider = bookProvider;
        this.Mapper = mapper;
        this.AuthorProvider = authorProvider;
        this.CategoryProvider = categoryProvider;
        this.IdentifierProvider = identifierProvider;
        this.BookmarkProvider = bookmarkProvider;
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
        var books = await this.Service.QueryBooks(query);

        await this.BookProvider.PopulateWithBookmarks(books, this.UserId);

        return this.Ok(books);
    }

    [HttpGet("bookmark")]
    public async Task<ActionResult<List<BookResponse>>> GetBookmarks([FromQuery] QueryParameters query)
    {
        var books = await this.BookProvider.GetBookmarks(this.UserId, query.Page, query.PerPage);
        var totalBookmarks = await this.BookmarkProvider.GetBookmarkCount(this.UserId);
        var mapped = this.Mapper.Map<List<BookResponse>>(books);
        var paginated = PaginationHelper.CreatePagedResponse(mapped, query, totalBookmarks);

        return this.Ok(paginated);
    }

    [HttpGet("{bookId:guid}")]
    public async Task<ActionResult<BookResponse>> GetBook([FromRoute] Guid bookId)
    {
        var book = await this.BookProvider.GetById(bookId);

        if (book == null)
        {
            return this.NotFound();
        }

        var mapped = this.Mapper.Map<BookResponse>(book);
        var internalRating = await this.BookProvider.AverageRating(bookId);

        mapped.InternalRating = internalRating;


        return this.Ok(mapped);
    }

    [HttpGet("{bookId:guid}/author")]
    public async Task<ActionResult<List<Author>>> GetAuthors([FromRoute] Guid bookId)
    {
        var authors = await this.AuthorProvider.GetByBook(bookId);

        return this.Ok(authors);
    }

    [HttpGet("{bookId:guid}/identifier")]
    public async Task<ActionResult<List<Identifier>>> GetIdentifiers([FromRoute] Guid bookId)
    {
        var identifiers = await this.IdentifierProvider.GetByBook(bookId);

        return this.Ok(identifiers);
    }

    [HttpGet("{bookId:guid}/category")]
    public async Task<ActionResult<List<Category>>> GetCategories([FromRoute] Guid bookId)
    {
        var categories = await this.CategoryProvider.GetByBook(bookId);

        return this.Ok(categories);
    }

    [HttpGet("{bookId:guid}/review")]
    public async Task<ActionResult<List<Category>>> GetReviews([FromRoute] Guid bookId, [FromQuery] QueryParameters query)
    {
        var reviews = await this.BookProvider.GetReviews(bookId, query.Page, query.PerPage);

        return this.Ok(reviews);
    }

}
