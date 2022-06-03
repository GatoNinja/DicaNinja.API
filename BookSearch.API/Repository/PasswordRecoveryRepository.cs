using System.Globalization;

using BookSearch.API.Contexts;
using BookSearch.API.Models;
using BookSearch.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.Repository;

public class PasswordRecoveryRepository : IPasswordRecoveryRepository
{
    public PasswordRecoveryRepository(BaseContext context)
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

    public async Task<PasswordRecovery> InsertAsync(PasswordRecovery model)
    {
        var random = new Random((int)DateTimeOffset.Now.Ticks);
        var code = Math.Ceiling(random.NextDouble() * 1000000).ToString(CultureInfo.InvariantCulture);
        model.Code = code.PadLeft(code.Length - 7, '0');

        await Context.PasswordRecoveries.AddAsync(model);
        await Context.SaveChangesAsync();

        return model;
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