namespace BookSearch.API.DDD.Author;

public interface IAuthorRepository
{
    Task<Author?> GetOrCreate(string authorName);

    Task<List<Author>> GetByBook(Guid bookId);
}
