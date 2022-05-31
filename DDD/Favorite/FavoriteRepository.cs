using BookSearch.API.Contexts;

using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.DDD.Favorite;

public class FavoriteRepository : IFavoriteRepository
{
    public FavoriteRepository(DefaultContext context)
    {
        Context = context;
    }

    private DefaultContext Context { get; }
    
    public async Task<int> Favorite(Guid userId, string identifier, string type)
    {
        var existingFavorite = await Context.Favorites.FirstOrDefaultAsync(f => f.UserId == userId && f.Identifier == identifier && f.Type == type);

        if (existingFavorite is not null)
        {
            Context.Remove(existingFavorite);
            await Context.SaveChangesAsync();
        }
        else
        {
            var favorite = new FavoriteModel(userId, identifier, type);
            Context.Add(favorite);
            await Context.SaveChangesAsync();
        }

        return await GetFavoritesCount(userId);
    }

    public async Task<List<FavoriteModel>> GetFavoriteByUser(Guid userId, int page = 1, int pageSize = 10)
    {
        return await Context.Favorites.Where(favorite => favorite.UserId == userId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
    }

    public async Task<int> GetFavoritesCount(Guid userId)
    {
        return await Context.Favorites.CountAsync(favorite => favorite.UserId == userId);
    }
}
