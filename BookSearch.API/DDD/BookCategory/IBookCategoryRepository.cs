namespace BookSearch.API.DDD.BookCategory;

public interface IBookCategoryRepository
{
    Task<BookCategory?> GetOrCreate(string categoryName);
}
