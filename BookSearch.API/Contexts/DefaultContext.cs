
using BookSearch.API.DDD.Author;
using BookSearch.API.DDD.Book;
using BookSearch.API.DDD.Category;
using BookSearch.API.DDD.Favorite;
using BookSearch.API.DDD.Identifier;
using BookSearch.API.DDD.PasswordRecovery;
using BookSearch.API.DDD.Person;
using BookSearch.API.DDD.RefreshToken;
using BookSearch.API.DDD.User;

using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.Contexts
{
    public class DefaultContext : DbContext
    {
        public DefaultContext(DbContextOptions<DefaultContext> options) : base(options)
        {
        }

        public DbSet<Favorite> Favorites { get; set; } = default!;

        public DbSet<User> Users { get; set; } = default!;

        public DbSet<Person> People { get; set; } = default!;

        public DbSet<PasswordRecovery> PasswordRecoveries { get; set; } = default!;

        public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

        public DbSet<Book> Books { get; set; } = default!;

        public DbSet<Author> Authors { get; set; } = default!;

        public DbSet<Category> Categories { get; set; } = default!;

        public DbSet<Identifier> Identifiers { get; set; } = default!;
    }

}
