
using BookSearch.API.DDD.Favorite;
using BookSearch.API.DDD.PasswordRecovery;
using BookSearch.API.DDD.Person;
using BookSearch.API.DDD.RefreshToken;
using BookSearch.API.DDD.User;

using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.Contexts;

public class DefaultContext: DbContext
{
    public DefaultContext(DbContextOptions<DefaultContext> options) : base(options)
    {
    }

    public DbSet<FavoriteModel> Favorites { get; set; } = default!;
    
    public DbSet<UserModel> Users { get; set; } = default!;

    public DbSet<PersonModel> People { get; set; } = default!;

    public DbSet<PasswordRecoveryModel> PasswordRecoveries { get; set; } = default!;

    public DbSet<RefreshTokenModel> RefreshTokens { get; set; } = default!;
}
