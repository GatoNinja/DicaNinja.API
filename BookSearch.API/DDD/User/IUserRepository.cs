using BookSearch.API.DDD.SignUp;

namespace BookSearch.API.DDD.User
{
    public interface IUserRepository
    {
        Task ChangePassword(string email, string password);
        Task<UserModel?> DoLoginAsync(string username, string password);
        Task<UserModel?> GetByEmail(string email);
        Task<UserModel?> GetByIdAsync(Guid id);
        Task<UserModel?> InsertAsync(UserModel model);
        EnumNewUserCheck ValidateNewUser(UserModel userModel);
    }
}