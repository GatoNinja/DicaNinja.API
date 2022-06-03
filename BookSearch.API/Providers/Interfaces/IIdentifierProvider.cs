using BookSearch.API.Models;
using BookSearch.API.Response;

namespace BookSearch.API.Providers.Interfaces;

public interface IIdentifierProvider
{
    Task<Identifier?> GetOrCreate(IdentifierResponse identifier);

    Task<List<Identifier>> GetByBook(Guid bookId);
}
