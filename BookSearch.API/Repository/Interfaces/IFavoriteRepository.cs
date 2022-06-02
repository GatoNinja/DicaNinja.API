namespace BookSearch.API.Repository.Interfaces;

public interface IFavoriteRepository
{
    Task<int> GetFavoritesCount(Guid userId);

    Task<int?> Favorite(Guid userId, string identifier, string type);

    Task<bool> IsFavorite(Guid userId, string identifier, string type);
}
