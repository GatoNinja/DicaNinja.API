using AutoMapper;

using BookSearch.API.Abstracts;
using BookSearch.API.Helpers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.DDD.Favorite
{
    public record FavoritePayload(string isbn, string type);

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
            var count = await FavoriteRepository.Favorite(UserId, payload.isbn, payload.type);

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

        [HttpGet()]
        public async Task<PagedResponse<List<FavoriteDTO>>> GetAll([FromQuery] Helpers.QueryString queryString)
        {
            var favorites = await FavoriteRepository.GetFavoriteByUser(UserId, queryString.Page, queryString.PerPage);
            var totalRecords = await FavoriteRepository.GetFavoritesCount(UserId);
            var mapped = Mapper.Map<List<FavoriteDTO>>(favorites);

            return PaginationHelper.CreatePagedResponse<FavoriteDTO>(mapped, queryString, totalRecords);
        }
    }
}