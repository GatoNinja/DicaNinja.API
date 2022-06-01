namespace BookSearch.API.DDD.Book;

public interface IBookRepository
{
    Task<Book?> GetByIdentifier(string identifier, string type);

    Task<Book?> CreateFromResponse(BookResponse response);
}
