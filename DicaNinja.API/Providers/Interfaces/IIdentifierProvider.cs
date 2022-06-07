
using DicaNinja.API.Models;
using DicaNinja.API.Response;

namespace DicaNinja.API.Providers.Interfaces;

public interface IIdentifierProvider
{
    Task<Identifier?> GetOrCreateAsync(IdentifierResponse identifier);

    Task<List<Identifier>> GetByBookAsync(Guid bookId);
}
