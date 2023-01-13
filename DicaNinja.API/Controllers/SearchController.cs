using DicaNinja.API.Abstracts;
using DicaNinja.API.Cache;
using DicaNinja.API.Helpers;
using DicaNinja.API.Response;
using DicaNinja.API.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Models;

namespace DicaNinja.API.Controllers;

[Route("[controller]")]
[ApiController]
public class SearchController : ControllerHelper
{
    public SearchController(ICacheService cacheService, BookGoogleService service, IBookProvider bookProvider, IUserProvider userProvider)
    {
        CacheService = cacheService;
        Service = service;
        BookProvider = bookProvider;
        UserProvider = userProvider;
    }

    private ICacheService CacheService { get; }
    private BookGoogleService Service { get; }
    private IBookProvider BookProvider { get; }
    private IUserProvider UserProvider { get; }

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<dynamic> DoSearch([FromQuery] QueryParametersWithFilter request, CancellationToken cancellationToken)
    {
        if (request is null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        //var cacheKey = $"googlebooksearch_{request.Query}-{request.Page}-{request.PerPage}";

        //var cache = CacheService.GetData<List<dynamic>>(cacheKey);

        //if (cache is not null)
        //{
        //    return Ok(cache);
        //}
        List<BookResponse> books;
        IEnumerable<User> users;

        try
        {
            books = await Service.QueryBooksAsync(request.Query, cancellationToken, request.Page, request.PerPage, request.Lang).ConfigureAwait(false);
            users = await UserProvider.SearchAsync(request.Query, cancellationToken, request.Page, request.PerPage);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);

            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao chamar a API do Google Books");
        }

        if (IsAuthenticated())
        {
            await BookProvider.PopulateWithBookmarksAsync(books, GetUserId(), cancellationToken).ConfigureAwait(false);
        }

        var response = new
        {
            books,
            users
        };

        //CacheService.SetData(cacheKey, response, DateTimeOffset.Now.AddMinutes(5));

        return Ok(response);
    }
}
