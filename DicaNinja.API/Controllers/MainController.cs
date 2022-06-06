
using AutoMapper;

using DicaNinja.API.Helpers;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Response;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Controllers;

[Route("8[controller]")]
[ApiController]
[AllowAnonymous]
public class MainController : ControllerBase
{
    public MainController(IMapper mapper, IBookProvider bookProvider, IUserProvider userProvider, IAuthorProvider authorProvider, ICategoryProvider categoriProvider)
    {
        this.Mapper = mapper;
        this.BookProvider = bookProvider;
        this.AuthorProvider = authorProvider;
        this.CategoriProvider = categoriProvider;
        this.UserProvider = userProvider;
    }

    private IMapper Mapper { get; }
    private IBookProvider BookProvider { get; }
    private IAuthorProvider AuthorProvider { get; }
    private ICategoryProvider CategoriProvider { get; }
    private IUserProvider UserProvider { get; }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<IEnumerable<BookResponse>>>> Get([FromQuery] QueryParameters query)
    {
        var books = await this.BookProvider.GetBooks();
        var totalBooks = await this.BookProvider.GetCount();
        var totalAuthors = await this.AuthorProvider.GetCount();
        var totalCategories = await this.CategoriProvider.GetCount();
        var totalUsers = await this.UserProvider.GetCount();
        var mapped = this.Mapper.Map<IEnumerable<BookResponse>>(books);
        var paginated = PaginationHelper.CreatePagedResponse(mapped, query, totalBooks);
        var response = new MainResponse(paginated, totalBooks, totalAuthors, totalCategories, totalUsers);
        
        return this.Ok(response);
    }
}
