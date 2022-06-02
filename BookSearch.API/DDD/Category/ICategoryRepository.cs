namespace BookSearch.API.DDD.Category;

public interface ICategoryRepository
{
    Task<Category?> GetOrCreate(string categoryName);

    Task<List<Category>> GetByBook(Guid bookId);
}
