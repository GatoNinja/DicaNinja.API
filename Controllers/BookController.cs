using Google.Apis.Books.v1;
using Google.Apis.Services;

using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private const string apiKey = "AIzaSyBKobq7aC-ajuflWLdnrjGlFnz-Eem3Fhw";

    public BookController()
    {
        var service = new BooksService(new BaseClientService.Initializer()
        {
            ApiKey = apiKey,
            ApplicationName = "Fenicia",
        });

        Service = service;
    }

    private BooksService Service { get; }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] string query)
    {
        var request = Service.Volumes.List(query);
        var response = await request.ExecuteAsync();

        return Ok(response.Items.Select(item => item.VolumeInfo));
    }
}
