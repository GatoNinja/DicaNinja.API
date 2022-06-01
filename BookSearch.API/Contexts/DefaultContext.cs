
using BookSearch.API.DDD.Author;
using BookSearch.API.DDD.Book;
using BookSearch.API.DDD.BookCategory;
using BookSearch.API.DDD.BookIdentifier;
using BookSearch.API.DDD.Favorite;
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

        public DbSet<BookCategory> BookCategories { get; set; } = default!;

        public DbSet<BookIdentifier> BookIdentifiers { get; set; } = default!;
    }

}