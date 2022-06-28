
using AutoMapper;

using DicaNinja.API.Helpers;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Response;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Controllers;

[Route("[controller]")]
[ApiController]
[AllowAnonymous]
public class MainController : ControllerBase
{
    public MainController(IMapper mapper, IBookProvider bookProvider, IUserProvider userProvider, IAuthorProvider authorProvider, ICategoryProvider categoriProvider)
    {
        Mapper = mapper;
        BookProvider = bookProvider;
        AuthorProvider = authorProvider;
        CategoriProvider = categoriProvider;
        UserProvider = userProvider;
    }

    private IMapper Mapper { get; }
    private IBookProvider BookProvider { get; }
    private IAuthorProvider AuthorProvider { get; }
    private ICategoryProvider CategoriProvider { get; }
    private IUserProvider UserProvider { get; }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<IEnumerable<BookResponse>>>> Get([FromQuery] QueryParameters query, CancellationToken cancellationToken)
    {
        var books = await BookProvider.GetBooksAsync(cancellationToken);
        var totalBooks = await BookProvider.GetCountAsync(cancellationToken);
        var totalAuthors = await AuthorProvider.GetCountAsync(cancellationToken);
        var totalCategories = await CategoriProvider.GetCountAsync(cancellationToken);
        var totalUsers = await UserProvider.GetCountAsync(cancellationToken);
        var mapped = Mapper.Map<IEnumerable<BookResponse>>(books);
        var paginated = PaginationHelper.CreatePagedResponse(mapped, query, totalBooks);
        var response = new MainResponse(paginated, totalBooks, totalAuthors, totalCategories, totalUsers);

        return Ok(response);
    }
}
