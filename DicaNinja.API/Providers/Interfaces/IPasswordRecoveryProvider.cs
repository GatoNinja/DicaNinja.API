
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface IPasswordRecoveryProvider
{
    Task<PasswordRecovery?> GetByEmailAndCode(string email, string code);

    Task UseRecoveryCode(Guid recoverId);

    Task<PasswordRecovery> InsertAsync(PasswordRecovery user);
}
