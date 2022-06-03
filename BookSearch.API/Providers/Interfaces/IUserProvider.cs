using BookSearch.API.Enums;
using BookSearch.API.Models;

namespace BookSearch.API.Providers.Interfaces;

public interface IUserProvider
{
    Task ChangePassword(string email, string password);
    Task<User?> DoLoginAsync(string username, string password);
    Task<User?> GetByEmail(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> InsertAsync(User user);
    EnumNewUserCheck ValidateNewUser(User user);
    Task<IEnumerable<User>> GetFollowers(Guid userId, int page = 1, int pageSize = 10);
    Task<int> GetFollowersCount(Guid userId);
    Task<int> GetBooksCount(Guid userId);
    Task<IEnumerable<User>> GetFollowing(Guid userId, int page = 1, int pageSize = 10);
    Task<int> GetAuthorsCount(Guid userId);
    Task<int> GetCategoriesCount(Guid userId);
    Task<int> GetFollowingCount(Guid userId);
}
