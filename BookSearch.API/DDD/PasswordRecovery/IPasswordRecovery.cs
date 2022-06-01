namespace BookSearch.API.DDD.PasswordRecovery
{
    public interface IPasswordRecoveryRepository
    {
        Task<PasswordRecovery?> GetByEmailAndCode(string email, string code);

        Task UseRecoveryCode(Guid recoverId);

        Task<PasswordRecovery> InsertAsync(PasswordRecovery model);
    }
}