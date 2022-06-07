
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface IPasswordRecoveryProvider
{
    Task<PasswordRecovery?> GetByEmailAndCodeAsync(string email, string code);

    Task UseRecoveryCodeAsync(Guid recoverId);

    Task<PasswordRecovery> InsertAsync(PasswordRecovery user);
}
