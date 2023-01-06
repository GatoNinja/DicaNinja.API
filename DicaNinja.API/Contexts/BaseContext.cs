
using DicaNinja.API.Abstracts;
using DicaNinja.API.Models;

using Microsoft.EntityFrameworkCore;

namespace DicaNinja.API.Contexts;

public class BaseContext : DbContext
{
    public BaseContext(DbContextOptions<BaseContext> options) : base(options)
    {
    }

    public DbSet<Follower> Followers { get; private set; } = default!;

    public DbSet<Bookmark> Bookmarks { get; private set; } = default!;

    public DbSet<User> Users { get; private set; } = default!;

    public DbSet<PasswordRecovery> PasswordRecoveries { get; private set; } = default!;

    public DbSet<RefreshToken> RefreshTokens { get; private set; } = default!;

    public DbSet<Book> Books { get; private set; } = default!;

    public DbSet<Author> Authors { get; private set; } = default!;

    public DbSet<Category> Categories { get; private set; } = default!;

    public DbSet<Identifier> Identifiers { get; private set; } = default!;

    public DbSet<Review> Reviews { get; private set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (modelBuilder is null)
        {
            throw new ArgumentNullException(nameof(modelBuilder), "ModelBuilder couldn't be created");
        }

        modelBuilder.Entity<Follower>()
            .HasIndex(f => new { f.UserId, f.FollowedId })
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasMany(u => u.Following)
            .WithOne(f => f.FollowedUser)
            .HasForeignKey(f => f.UserId);

        modelBuilder.Entity<User>()
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
