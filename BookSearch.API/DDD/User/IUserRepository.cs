using BookSearch.API.DDD.SignUp;

namespace BookSearch.API.DDD.User
{
    public interface IUserRepository
    {
        Task ChangePassword(string email, string password);
        Task<User?> DoLoginAsync(string username, string password);
        Task<User?> GetByEmail(string email);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> InsertAsync(User model);
        EnumNewUserCheck ValidateNewUser(User userModel);
    }
}