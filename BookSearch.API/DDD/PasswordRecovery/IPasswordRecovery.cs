namespace BookSearch.API.DDD.PasswordRecovery;

public interface IPasswordRecoveryRepository
{
    Task<PasswordRecoveryModel?> GetByEmailAndCode(string email, string code);

    Task UseRecoveryCode(Guid recoverId);

    Task<PasswordRecoveryModel> InsertAsync(PasswordRecoveryModel model);
}