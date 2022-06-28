
using DicaNinja.API.Models;
using DicaNinja.API.Response;

namespace DicaNinja.API.Providers.Interfaces;

public interface IIdentifierProvider
{
    Task<Identifier?> GetOrCreateAsync(IdentifierResponse identifier, CancellationToken cancellationToken);

    Task<List<Identifier>> GetByBookAsync(Guid bookId, CancellationToken cancellationToken);
}
