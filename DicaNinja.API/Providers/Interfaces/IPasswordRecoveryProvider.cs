
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface IPasswordRecoveryProvider
{
    Task<PasswordRecovery?> GetByEmailAndCodeAsync(string email, string code, CancellationToken cancellation);

    Task UseRecoveryCodeAsync(Guid recoverId, CancellationToken cancellation);

    Task<PasswordRecovery> InsertAsync(PasswordRecovery passwordRecovery, CancellationToken cancellation);
}
