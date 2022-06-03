
using DicaNinja.API.Models;
using DicaNinja.API.Response;

namespace DicaNinja.API.Providers.Interfaces;

public interface IIdentifierProvider
{
    Task<Identifier?> GetOrCreate(IdentifierResponse identifier);

    Task<List<Identifier>> GetByBook(Guid bookId);
}
