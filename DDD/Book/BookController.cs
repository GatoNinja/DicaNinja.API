using BookSearch.API.Contexts;

using Google.Apis.Books.v1;
using Google.Apis.Services;

using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.DDD.Book;

[Route("[controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private const string apiKey = "AIzaSyBKobq7aC-ajuflWLdnrjGlFnz-Eem3Fhw";

    public BookController(DefaultContext context)
    {
        var service = new BooksService(new BaseClientService.Initializer()
        {
            ApiKey = apiKey,
            ApplicationName = "Fenicia",
        });

        Service = service;
        Context = context;
    }

    public DefaultContext Context { get; }
    private BooksService Service { get; }

    [HttpGet]
    public async Task<ActionResult<List<BookResponse>>> GetAsync([FromQuery] string query)
    {
        var request = Service.Volumes.List(query);
        var response = await request.ExecuteAsync();
        var output = response.Items.Select(item => new BookResponse(
            item.VolumeInfo.Title,
            item.VolumeInfo.Subtitle,
            item.VolumeInfo.IndustryIdentifiers.Select(identifier => new BookIdentifier(identifier.Identifier, identifier.Type)),
            item.VolumeInfo.Language,
            item.VolumeInfo.Description,
            item.VolumeInfo.Categories,
            item.VolumeInfo.PageCount ?? 0,
            item.VolumeInfo.Publisher,
            item.VolumeInfo.PublishedDate ?? "",
            item.VolumeInfo.ImageLinks?.Thumbnail ?? string.Empty,
            item.VolumeInfo.Authors,
            item.VolumeInfo.AverageRating ?? 0
            ));

        return Ok(output);
    }
}
