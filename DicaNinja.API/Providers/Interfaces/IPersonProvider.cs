using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface IPersonProvider
{
    Task<Person?> UpdatePersonAsync(Guid userId, Person person, CancellationToken cancellationToken);
}
