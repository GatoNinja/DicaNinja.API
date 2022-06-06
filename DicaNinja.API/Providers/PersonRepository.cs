
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;

namespace DicaNinja.API.Providers;

public sealed class PersonProvider : IPersonProvider
{
    public async Task<Person> UpdatePersonAsync(Person person)
    {
        throw new NotImplementedException();
    }
}
