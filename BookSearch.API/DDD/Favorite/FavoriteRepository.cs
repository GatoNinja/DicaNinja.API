using BookSearch.API.Contexts;
using BookSearch.API.DDD.Book;

using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.DDD.Favorite
{
    public class FavoriteRepository : IFavoriteRepository
    {
        public FavoriteRepository(DefaultContext context, IBookRepository bookRepository, BookGoogleService bookGoogleService)
        {
            Context = context;
            BookRepository = bookRepository;
            BookGoogleService = bookGoogleService;
        }

        private DefaultContext Context { get; }
        private IBookRepository BookRepository { get; }
        private BookGoogleService BookGoogleService { get; }
        
        public async Task<int?> Favorite(Guid userId, string identifier, string type)
        {
            var existingFavorite = await FilterByUser(userId, identifier, type).FirstOrDefaultAsync();
            
            if (existingFavorite is not null)
            {
                Context.Remove(existingFavorite);
                await Context.SaveChangesAsync();
            }
            else
            {
                var book = await BookRepository.GetByIdentifier(identifier, type);

                if (book is null)
                {
                    book = await BookGoogleService.CreateBookFromGoogle(identifier);

                    if (book is null)
                    {
                        return null;
                    }
                }

                var favorite = new Favorite(userId, book.Id);
                Context.Add(favorite);
                await Context.SaveChangesAsync();
            }

            return await GetFavoritesCount(userId);
        }

        public async Task<int> GetFavoritesCount(Guid userId)
        {
            return await Context.Favorites.CountAsync(favorite => favorite.UserId == userId);
        }

        public async Task<bool> IsFavorite(Guid userId, string identifier, string type)
        {
            return await FilterByUser(userId, identifier, type).AnyAsync();
        }

        private IQueryable<Favorite> FilterByUser(Guid userId, string identifier, string type)
        {
            var query = from favorite in Context.Favorites
                        join book in Context.Books on favorite.BookId equals book.Id
                        where favorite.UserId == userId && book.Identifiers.Any(i => i.Isbn == identifier && i.Type == type)
                        select favorite;

            return query;
        }
        
    }
}