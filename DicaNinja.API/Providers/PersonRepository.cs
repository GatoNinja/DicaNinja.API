
using DicaNinja.API.Contexts;
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace DicaNinja.API.Providers;

public sealed class PersonProvider : IPersonProvider
{
    public PersonProvider(BaseContext context)
    {
        Context = context;
    }

    private BaseContext Context { get; }


    public async Task<Person?> UpdatePersonAsync(Guid userId, Person person, CancellationToken cancellationToken)
    {
        var personToUpdate = await Context.People.Include(person => person.User)
            .FirstOrDefaultAsync(p => p.UserId == userId, cancellationToken);

        if (personToUpdate == null)
        {
            return null;
        }

        personToUpdate.FirstName = person.FirstName ?? personToUpdate.FirstName;
        personToUpdate.LastName = person.LastName ?? personToUpdate.LastName;
        personToUpdate.Image = person.Image ?? personToUpdate.Image;
        personToUpdate.Description = person.Description ?? personToUpdate.Description;

        await Context.SaveChangesAsync(cancellationToken);

        return personToUpdate;
    }
}
