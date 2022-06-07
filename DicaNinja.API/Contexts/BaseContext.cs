
using DicaNinja.API.Abstracts;
using DicaNinja.API.Models;

using Microsoft.EntityFrameworkCore;

namespace DicaNinja.API.Contexts;

public class BaseContext : DbContext
{
    public BaseContext(DbContextOptions<BaseContext> options) : base(options)
    {
    }

    public DbSet<Follower> Followers { get; set; } = default!;

    public DbSet<Bookmark> Bookmarks { get; set; } = default!;

    public DbSet<User> Users { get; set; } = default!;

    public DbSet<Person> People { get; set; } = default!;

    public DbSet<PasswordRecovery> PasswordRecoveries { get; set; } = default!;

    public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

    public DbSet<Book> Books { get; set; } = default!;

    public DbSet<Author> Authors { get; set; } = default!;

    public DbSet<Category> Categories { get; set; } = default!;

    public DbSet<Identifier> Identifiers { get; set; } = default!;

    public DbSet<Review> Reviews { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Follower>()
            .HasIndex(f => new { f.UserId, f.FollowedId })
            .IsUnique();

        builder.Entity<User>()
            .HasMany(u => u.Following)
            .WithOne(f => f.FollowedUser)
            .HasForeignKey(f => f.UserId);

        builder.Entity<User>()
            .HasMany(u => u.Followers)
            .WithOne(f => f.User)
            .HasForeignKey(f => f.FollowedId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var item in ChangeTracker.Entries())
        {
            if (item.Entity is BaseModel model)
            {
                model.Created = DateTime.UtcNow;

                if (item.State == EntityState.Modified)
                {
                    model.Updated = DateTime.UtcNow;
                }
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
