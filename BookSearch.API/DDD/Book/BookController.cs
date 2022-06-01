using BookSearch.API.Abstracts;
using BookSearch.API.Contexts;
using BookSearch.API.DDD.Favorite;

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
    private const string apiKey = "AIzaSyBKobq7aC-ajuflWLdnrjGlFnz-Eem3Fhw";

    public BookController(DefaultContext context, IFavoriteRepository favoriteRepository)
    {
        var service = new BooksService(new BaseClientService.Initializer()
        {
            ApiKey = apiKey,
            ApplicationName = "Fenicia",
        });

        Service = service;
        Context = context;
        FavoriteRepository = favoriteRepository;
    }

    private DefaultContext Context { get; }
    private IFavoriteRepository FavoriteRepository { get; }
    private BooksService Service { get; }

    [HttpGet]
    public async Task<ActionResult<List<BookResponse>>> GetAsync([FromQuery] string query)
    {
        var request = Service.Volumes.List(query);
        var response = await request.ExecuteAsync();
        var books = response.Items.Select(item => new BookResponse(
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

        //foreach (var book in books)
        //{
        //    if (book.Title.Contains("direito"))
        //    {
        //        book.IsFavorite = await IsBookFavorite(UserId, book.Identifiers);
        //    }
        //}

        return Ok(books);
    }

    //private async Task<bool> IsBookFavorite(Guid userId, IEnumerable<BookIdentifier> identifiers)
    //{
    //    foreach (var identifier in identifiers)
    //    {
    //        if (await FavoriteRepository.IsFavorite(userId, identifier.Isbn, identifier.Type))
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}
}
