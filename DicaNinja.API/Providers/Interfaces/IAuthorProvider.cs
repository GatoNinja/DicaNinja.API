
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface IAuthorProvider
{
    Task<Author?> GetOrCreateAsync(string authorName);

    Task<List<Author>> GetByBookAsync(Guid bookId);

    Task<int> GetCountAsync();
}
