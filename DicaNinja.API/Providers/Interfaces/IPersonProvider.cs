using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface IPersonProvider
{
    Task<Person> UpdatePersonAsync(Person person);
}
