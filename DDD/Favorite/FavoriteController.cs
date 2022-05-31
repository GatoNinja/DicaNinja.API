using BookSearch.API.Abstracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.DDD.Favorite;

public record FavoritePayload(string isbn, string type);

[Route("[controller]")]
[ApiController]
public class FavoriteController: ControllerHelper
{
    public FavoriteController(IFavoriteRepository favoriteRepository)
    {
        FavoriteRepository = favoriteRepository;
    }

    private IFavoriteRepository FavoriteRepository { get; }

    [HttpPost]
    public async Task<int> CreateFavorite([FromBody] FavoritePayload payload)
    {
        return await FavoriteRepository.Favorite(UserId, payload.isbn, payload.type);
    }

    [HttpGet("count")]
    public async Task<int> GetCount()
    {
        return await FavoriteRepository.GetFavoritesCount(UserId);
    }

    [HttpGet()]
    public async Task<List<FavoriteModel>> GetAll([FromQuery] Helpers.QueryString queryString)
    {
        return await FavoriteRepository.GetFavoriteByUser(UserId, queryString.Page, queryString.PerPage);
    }
}