
using DicaNinja.API.Models;
using DicaNinja.API.Response;

namespace DicaNinja.API.Providers.Interfaces;

public interface IIdentifierProvider
{
    Task<Identifier?> GetOrCreateAsync(IdentifierResponse bookIdentifier, CancellationToken cancellationToken);

    Task<IEnumerable<Identifier>> GetByBookAsync(Guid bookId, CancellationToken cancellationToken);
}
