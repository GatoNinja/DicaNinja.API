using System.Globalization;

using BookSearch.API.Contexts;

using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.DDD.PasswordRecovery;

public class PasswordRecoveryRepository : IPasswordRecoveryRepository
{
    public PasswordRecoveryRepository(DefaultContext context)
    {
        Context = context;
    }

    private DefaultContext Context { get; }

    public async Task<PasswordRecoveryModel?> GetByEmailAndCode(string email, string code)
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

    public async Task<PasswordRecoveryModel> InsertAsync(PasswordRecoveryModel model)
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