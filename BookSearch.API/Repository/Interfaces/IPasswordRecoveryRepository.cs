using BookSearch.API.Models;

namespace BookSearch.API.Repository.Interfaces;

public interface IPasswordRecoveryRepository
{
    Task<PasswordRecovery?> GetByEmailAndCode(string email, string code);

    Task UseRecoveryCode(Guid recoverId);

    Task<PasswordRecovery> InsertAsync(PasswordRecovery model);
}