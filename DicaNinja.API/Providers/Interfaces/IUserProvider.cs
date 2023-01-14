
using DicaNinja.API.Enums;
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface IUserProvider
{
    Task ChangePasswordAsync(string email, string password, CancellationToken cancellation);
    Task<User?> DoLoginAsync(string username, string password, CancellationToken cancellation);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellation);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellation);
    Task<User?> InsertAsync(User user, CancellationToken cancellation);
    Task<EnumNewUserCheck> ValidateNewUserAsync(User user, CancellationToken cancellation);
    Task<IEnumerable<User>> GetFollowersAsync(Guid userId, CancellationToken cancellation, int page = 1, int pageSize = 10);
    Task<int> GetFollowersCountAsync(Guid userId, CancellationToken cancellation);
    Task<int> GetBooksCountAsync(Guid userId, CancellationToken cancellation);
    Task<IEnumerable<User>> GetFollowingAsync(Guid userId, CancellationToken cancellation, int page = 1, int pageSize = 10);
    Task<int> GetAuthorsCountAsync(Guid userId, CancellationToken cancellation);
    Task<int> GetCategoriesCountAsync(Guid userId, CancellationToken cancellation);
    Task<int> GetFollowingCountAsync(Guid userId, CancellationToken cancellation);
    Task<User?> GetByUsernameOrEmailAsync(string parameter, CancellationToken cancellation);
    Task<int> GetCountAsync(CancellationToken cancellation);
    Task<IEnumerable<User>> SearchAsync(string searchTerm, CancellationToken cancellation, int page = 1, int pageSize = 10);
    Task<User?> UpdateUserAsync(Guid userId, User user, CancellationToken cancellation);

}
