namespace BookSearch.API.DDD.Favorite;

public interface IFavoriteRepository
{
    Task<List<FavoriteModel>> GetFavoriteByUser(Guid userId, int page, int pageSize);

    Task<int> GetFavoritesCount(Guid userId);

    Task<int> Favorite(Guid userId, string identifier, string type);

    Task<bool> IsFavorite(Guid userId, string identifier, string type);
}
