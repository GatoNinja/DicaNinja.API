using BookSearch.API.DDD.SignUp;

namespace BookSearch.API.DDD.User
{
    public interface IUserRepository
    {
        Task<UserModel?> InsertAsync(UserModel user);
        Task<UserModel?> GetByIdAsync(Guid id);
        Task<UserModel?> DoLoginAsync(string username, string password);

        Task<UserModel?> GetByEmail(string email);

        Task ChangePassword(string email, string password);

        EnumNewUserCheck ValidateNewUser(UserModel userModel);
    }
}