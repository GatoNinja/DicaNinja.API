
using DicaNinja.API.Contexts;

using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;

using Microsoft.EntityFrameworkCore;

using System.Globalization;

namespace DicaNinja.API.Providers;

public class PasswordRecoveryProvider : IPasswordRecoveryProvider
{
    public PasswordRecoveryProvider(BaseContext context)
    {
        Context = context;
    }

    private BaseContext Context { get; }

    public async Task<PasswordRecovery?> GetByEmailAndCodeAsync(string email, string code, CancellationToken cancellationToken)
    {
        var query = from passwordRecovery in Context.PasswordRecoveries
                    join user in Context.Users on passwordRecovery.UserId equals user.Id
                    where user.Email == email
                          && passwordRecovery.Code == code
                          && passwordRecovery.IsActive
                          && passwordRecovery.ExpireDate >= DateTimeOffset.Now
                    select passwordRecovery;

        return await query.FirstOrDefaultAsync(cancellationToken).ConfigureAwait(false);
    }

    private static readonly Random Random = new Random();

    public async Task<PasswordRecovery> InsertAsync(PasswordRecovery passwordRecovery, CancellationToken cancellationToken)
    {
        if (passwordRecovery is null)
        {
            throw new ArgumentNullException(nameof(passwordRecovery));
        }

        var code = Math.Ceiling(Random.NextDouble() * 1000000).ToString(CultureInfo.InvariantCulture);
        passwordRecovery.Code = code.PadLeft(code.Length - 7, '0');

        await Context.PasswordRecoveries.AddAsync(passwordRecovery, cancellationToken).ConfigureAwait(false);
        await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return passwordRecovery;
    }


    public async Task UseRecoveryCodeAsync(Guid recoverId, CancellationToken cancellationToken)
    {
        var recover = await Context.PasswordRecoveries.FirstOrDefaultAsync(x => x.Id == recoverId, cancellationToken).ConfigureAwait(false);

        if (recover is null)
        {
            return;
        }

        recover.IsActive = false;

        await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
