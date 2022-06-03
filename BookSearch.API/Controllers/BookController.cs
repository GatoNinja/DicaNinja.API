using AutoMapper;

using BookSearch.API.Abstracts;
using BookSearch.API.Helpers;
using BookSearch.API.Models;
using BookSearch.API.Repository.Interfaces;
using BookSearch.API.Response;
using BookSearch.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class BookController : ControllerHelper
{

    public BookController(BookGoogleService service, IBookRepository bookRepository, IMapper mapper, IAuthorRepository authorRepository, ICategoryRepository categoryRepository, IIdentifierRepository identifierRepository, IBookmarkRepository bookmarkRepository)
    {
        Service = service;
        BookRepository = bookRepository;
        Mapper = mapper;
        AuthorRepository = authorRepository;
        CategoryRepository = categoryRepository;
        IdentifierRepository = identifierRepository;
        BookmarkRepository = bookmarkRepository;
    }

    private BookGoogleService Service { get; }
    private IBookRepository BookRepository { get; }
    private IMapper Mapper { get; }
    private IAuthorRepository AuthorRepository { get; }
    private ICategoryRepository CategoryRepository { get; }
    private IIdentifierRepository IdentifierRepository { get; }
    private IBookmarkRepository BookmarkRepository { get; }

    [HttpGet]
    public async Task<ActionResult<List<BookResponse>>> GetAsync([FromQuery] string query)
    {
        var books = await Service.QueryBooks(query);

        await BookRepository.PopulateWithBookmarks(books, UserId);

        return Ok(books);
    }

    [HttpGet("bookmark")]
    public async Task<ActionResult<List<BookResponse>>> GetBookmarks([FromQuery] Helpers.QueryParameters query)
    {
        var books = await BookRepository.GetBookmarks(UserId, query.Page, query.PerPage);
        var totalBookmarks = await BookmarkRepository.GetBookmarkCount(UserId);
        var mapped = Mapper.Map<List<BookResponse>>(books);
        var paginated = PaginationHelper.CreatePagedResponse<BookResponse>(mapped, query, totalBookmarks);

        return Ok(paginated);
    }

    [HttpGet("{bookId:guid}")]
    public async Task<ActionResult<BookResponse>> GetBook([FromRoute] Guid bookId)
    {
        var book = await BookRepository.GetById(bookId);

        if (book == null)
        {
            return NotFound();
        }

        var mapped = Mapper.Map<BookResponse>(book);
        var internalRating = await BookRepository.AverageRating(bookId);

        mapped.InternalRating = internalRating;


        return Ok(mapped);
    }

    [HttpGet("{bookId:guid}/author")]
    public async Task<ActionResult<List<Author>>> GetAuthors([FromRoute] Guid bookId)
    {
        var authors = await AuthorRepository.GetByBook(bookId);

        return Ok(authors);
    }

    [HttpGet("{bookId:guid}/identifier")]
    public async Task<ActionResult<List<Identifier>>> GetIdentifiers([FromRoute] Guid bookId)
    {
        var identifiers = await IdentifierRepository.GetByBook(bookId);

        return Ok(identifiers);
    }

    [HttpGet("{bookId:guid}/category")]
    public async Task<ActionResult<List<Category>>> GetCategories([FromRoute] Guid bookId)
    {
        var categories = await CategoryRepository.GetByBook(bookId);

        return Ok(categories);
    }

    [HttpGet("{bookId:guid}/review")]
    public async Task<ActionResult<List<Category>>> GetReviews([FromRoute] Guid bookId, [FromQuery]QueryParameters query)
    {
        var reviews = await BookRepository.GetReviews(bookId, query.Page, query.PerPage);

        return Ok(reviews);
    }

}