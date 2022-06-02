using AutoMapper;

using BookSearch.API.Abstracts;
using BookSearch.API.Helpers;
using BookSearch.API.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.Request;

public record FavoritePayload(string Isbn, string Type);

[Route("[controller]")]
[ApiController]
[Authorize]
public class FavoriteController : ControllerHelper
{
    public FavoriteController(IFavoriteRepository favoriteRepository, IMapper mapper)
    {
        FavoriteRepository = favoriteRepository;
        Mapper = mapper;
    }

    private IFavoriteRepository FavoriteRepository { get; }
    private IMapper Mapper { get; }

    [HttpPost]
    public async Task<ActionResult> CreateFavorite([FromBody] FavoritePayload payload)
    {
        var count = await FavoriteRepository.Favorite(UserId, payload.Isbn, payload.Type);

        if (count is null)
        {
            var messageResponse = new MessageResponse("Ocorreu um problema salvando o livro");

            return new BadRequestObjectResult(messageResponse);
        }

        return Ok(count);
    }

    [HttpGet("count")]
    public async Task<int> GetCount()
    {
        return await FavoriteRepository.GetFavoritesCount(UserId);
    }
}
