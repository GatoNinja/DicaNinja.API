
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
        var booksTask = BookProvider.GetBooksAsync(cancellationToken);
        var totalBooksTask = BookProvider.GetCountAsync(cancellationToken);
        var totalAuthorsTask = AuthorProvider.GetCountAsync(cancellationToken);
        var totalCategoriesTask = CategoriProvider.GetCountAsync(cancellationToken);
        var totalUsersTask = UserProvider.GetCountAsync(cancellationToken);

        await Task.WhenAll(booksTask, totalBooksTask, totalAuthorsTask, totalCategoriesTask, totalUsersTask).ConfigureAwait(false);

        var books = await booksTask.ConfigureAwait(false);
        var totalBooks = await totalBooksTask.ConfigureAwait(false);
        var totalAuthors = await totalAuthorsTask.ConfigureAwait(false);
        var totalCategories = await totalCategoriesTask.ConfigureAwait(false);
        var totalUsers = await totalUsersTask.ConfigureAwait(false);

        var mapped = Mapper.Map<IEnumerable<BookResponse>>(books);
        var paginated = PaginationHelper.CreatePagedResponse(mapped, query, totalBooks);
        var response = new MainResponse(paginated, totalBooks, totalAuthors, totalCategories, totalUsers);

        return Ok(response);
    }


}
