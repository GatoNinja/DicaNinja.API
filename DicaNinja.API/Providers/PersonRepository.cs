
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;

namespace DicaNinja.API.Providers;

public sealed class PersonProvider : IPersonProvider
{
    public Person(DefaultContext

    public async Task<Person> UpdatePersonAsync(Person person)
    {
        var currentPerson = await Context
    }
}
