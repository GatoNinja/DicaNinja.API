
using DicaNinja.API.Contexts;

using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace DicaNinja.API.Providers;

public class PasswordRecoveryProvider : IPasswordRecoveryProvider
{
    public PasswordRecoveryProvider(BaseContext context)
    {
        Context = context;
    }

    private BaseContext Context { get; }

    public async Task<PasswordRecovery?> GetByEmailAndCodeAsync(string email, string code, CancellationToken cancellation)
    {
        var query = from passwordRecovery in Context.PasswordRecoveries
                    join user in Context.Users on passwordRecovery.UserId equals user.Id
                    where user.Email == email
                          && passwordRecovery.Code == code
                          && passwordRecovery.IsActive
                          && passwordRecovery.ExpireDate >= DateTimeOffset.Now
                    select passwordRecovery;

        return await query.FirstOrDefaultAsync(cancellation).ConfigureAwait(false);
    }

    public async Task<PasswordRecovery> InsertAsync(PasswordRecovery passwordRecovery, CancellationToken cancellation)
    {
        if (passwordRecovery is null)
        {
            throw new ArgumentNullException(nameof(passwordRecovery));
        }

        var random = new Random();
        passwordRecovery.Code = random.Next(100000, 999999).ToString();

        await Context.PasswordRecoveries.AddAsync(passwordRecovery, cancellation).ConfigureAwait(false);
        await Context.SaveChangesAsync(cancellation).ConfigureAwait(false);

        return passwordRecovery;
    }


    public async Task UseRecoveryCodeAsync(Guid recoverId, CancellationToken cancellation)
    {
        var recover = await Context.PasswordRecoveries.FirstOrDefaultAsync(x => x.Id == recoverId, cancellation).ConfigureAwait(false);

        if (recover is null)
        {
            return;
        }

        recover.IsActive = false;

        Context.Entry(recover).State = EntityState.Modified;

        await Context.SaveChangesAsync(cancellation).ConfigureAwait(false);
    }
}
