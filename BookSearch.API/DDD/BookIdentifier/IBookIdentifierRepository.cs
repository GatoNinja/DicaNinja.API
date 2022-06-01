namespace BookSearch.API.DDD.BookIdentifier;

public interface IBookIdentifierRepository
{
    Task<BookIdentifier?> GetOrCreate(BookIdentifierDTO bookIdentifier);
}
