using BookSearch.API.Abstracts;
using BookSearch.API.Contexts;
using BookSearch.API.DDD.Favorite;
using BookSearch.API.Startup;

using Google.Apis.Books.v1;
using Google.Apis.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.DDD.Book;

[Route("[controller]")]
[ApiController]
[Authorize]
public class BookController : ControllerHelper
{

    public BookController(BookGoogleService service)
    {
        Service = service;
    }

    private BookGoogleService Service { get; }

    [HttpGet]
    public async Task<ActionResult<List<BookResponse>>> GetAsync([FromQuery] string query)
    {
        var books = await Service.QueryBooks(query);

        return Ok(books);
    }
}
