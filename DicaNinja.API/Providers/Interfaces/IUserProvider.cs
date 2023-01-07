
using DicaNinja.API.Enums;
using DicaNinja.API.Models;

namespace DicaNinja.API.Providers.Interfaces;

public interface IUserProvider
{
    Task ChangePasswordAsync(string email, string password, CancellationToken cancellationToken);
    Task<User?> DoLoginAsync(string username, string password, CancellationToken cancellationToken);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User?> InsertAsync(User user, CancellationToken cancellationToken);
    Task<EnumNewUserCheck> ValidateNewUserAsync(User user, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetFollowersAsync(Guid userId, CancellationToken cancellationToken, int page = 1, int pageSize = 10);
    Task<int> GetFollowersCountAsync(Guid userId, CancellationToken cancellationToken);
    Task<int> GetBooksCountAsync(Guid userId, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetFollowingAsync(Guid userId, CancellationToken cancellationToken, int page = 1, int pageSize = 10);
    Task<int> GetAuthorsCountAsync(Guid userId, CancellationToken cancellationToken);
    Task<int> GetCategoriesCountAsync(Guid userId, CancellationToken cancellationToken);
    Task<int> GetFollowingCountAsync(Guid userId, CancellationToken cancellationToken);
    Task<User?> GetByUsernameOrEmailAsync(string parameter, CancellationToken cancellationToken);
    Task<int> GetCountAsync(CancellationToken cancellationToken);
    Task<IEnumerable<User>> SearchAsync(string searchTerm, CancellationToken cancellationToken, int page = 1, int pageSize = 10);
    Task<User?> UpdateUserAsync(Guid userId, User user, CancellationToken cancellationToken);

}
