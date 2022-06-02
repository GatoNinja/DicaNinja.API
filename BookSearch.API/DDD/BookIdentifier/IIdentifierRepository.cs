namespace BookSearch.API.DDD.Identifier;

public interface IIdentifierRepository
{
    Task<Identifier?> GetOrCreate(IdentifierDTO identifier);

    Task<List<Identifier>> GetByBook(Guid bookId);
}
