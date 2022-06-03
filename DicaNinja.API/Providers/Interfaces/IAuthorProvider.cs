
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface IAuthorProvider
{
    Task<Author?> GetOrCreate(string authorName);

    Task<List<Author>> GetByBook(Guid bookId);
}
