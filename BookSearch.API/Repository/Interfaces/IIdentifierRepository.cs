using BookSearch.API.Models;
using BookSearch.API.Response;

namespace BookSearch.API.Repository.Interfaces;

public interface IIdentifierRepository
{
    Task<Identifier?> GetOrCreate(IdentifierDTO identifier);

    Task<List<Identifier>> GetByBook(Guid bookId);
}
