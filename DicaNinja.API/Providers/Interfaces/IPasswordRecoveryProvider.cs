
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface IPasswordRecoveryProvider
{
    Task<PasswordRecovery?> GetByEmailAndCodeAsync(string email, string code, CancellationToken cancellationToken);

    Task UseRecoveryCodeAsync(Guid recoverId, CancellationToken cancellationToken);

    Task<PasswordRecovery> InsertAsync(PasswordRecovery user, CancellationToken cancellationToken);
}
