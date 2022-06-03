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
        this.Context = context;
    }

    private BaseContext Context { get; }

    public async Task<PasswordRecovery?> GetByEmailAndCode(string email, string code)
    {
        var query = from passwordRecovery in this.Context.PasswordRecoveries
                    join user in this.Context.Users on passwordRecovery.UserId equals user.Id
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

        await this.Context.PasswordRecoveries.AddAsync(passwordRecovery);
        await this.Context.SaveChangesAsync();

        return passwordRecovery;
    }


    public async Task UseRecoveryCode(Guid recoverId)
    {
        var recover = await this.Context.PasswordRecoveries.FirstOrDefaultAsync(x => x.Id == recoverId);

        if (recover is null)
        {
            return;
        }

        recover.IsActive = false;

        await this.Context.SaveChangesAsync();
    }
}
