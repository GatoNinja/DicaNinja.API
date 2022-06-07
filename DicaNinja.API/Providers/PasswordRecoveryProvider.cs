using System.Globalization;

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

    public async Task<PasswordRecovery?> GetByEmailAndCode(string email, string code)
    {
        var query = from passwordRecovery in Context.PasswordRecoveries
                    join user in Context.Users on passwordRecovery.UserId equals user.Id
                    where user.Email == email
                          && passwordRecovery.Code == code
                          && passwordRecovery.IsActive
                          && passwordRecovery.ExpireDate >= DateTimeOffset.Now
                    select passwordRecovery;

        return await query.FirstOrDefaultAsync();
    }

    public async Task<PasswordRecovery> InsertAsync(PasswordRecovery passwordRecovery)
    {
        var random = new Random((int)DateTimeOffset.Now.Ticks);
        var code = Math.Ceiling(random.NextDouble() * 1000000).ToString(CultureInfo.InvariantCulture);
        passwordRecovery.Code = code.PadLeft(code.Length - 7, '0');

        await Context.PasswordRecoveries.AddAsync(passwordRecovery);
        await Context.SaveChangesAsync();

        return passwordRecovery;
    }


    public async Task UseRecoveryCode(Guid recoverId)
    {
        var recover = await Context.PasswordRecoveries.FirstOrDefaultAsync(x => x.Id == recoverId);

        if (recover is null)
        {
            return;
        }

        recover.IsActive = false;

        await Context.SaveChangesAsync();
    }
}
