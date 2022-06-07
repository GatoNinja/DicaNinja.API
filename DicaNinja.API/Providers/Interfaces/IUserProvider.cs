
using DicaNinja.API.Enums;
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface IUserProvider
{
    Task ChangePasswordAsync(string email, string password);
    Task<User?> DoLoginAsync(string username, string password);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> InsertAsync(User user);
    EnumNewUserCheck ValidateNewUser(User user);
    Task<IEnumerable<User>> GetFollowersAsync(Guid userId, int page = 1, int pageSize = 10);
    Task<int> GetFollowersCountAsync(Guid userId);
    Task<int> GetBooksCountAsync(Guid userId);
    Task<IEnumerable<User>> GetFollowingAsync(Guid userId, int page = 1, int pageSize = 10);
    Task<int> GetAuthorsCountAsync(Guid userId);
    Task<int> GetCategoriesCountAsync(Guid userId);
    Task<int> GetFollowingCountAsync(Guid userId);
    Task<User?> GetByUsernameOrEmailAsync(string parameter);
    Task<int> GetCountAsync();
    Task<IEnumerable<User>> SearchAsync(Guid userId, string searchTerm);
    
}
