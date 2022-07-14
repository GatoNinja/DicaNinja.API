
using DicaNinja.API.Contexts;
using DicaNinja.API.Enums;
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Services;

using Microsoft.EntityFrameworkCore;

namespace DicaNinja.API.Providers;

public sealed class UserProvider : IUserProvider
{
    public UserProvider(BaseContext context, IPasswordHasher passwordHasher)
    {
        Context = context;
        PasswordHasher = passwordHasher;
    }

    private BaseContext Context { get; }
    private IPasswordHasher PasswordHasher { get; }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var query = from user in Context.Users
                    where user.Id == id
                    select new User(user.Id, user.Username, user.Email, user.FirstName, user.LastName, user.Image);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> DoLoginAsync(string username, string password, CancellationToken cancellationToken)
    {
        var query = from user in Context.Users
                    where user.Username == username || user.Email == username
                    select new User(user.Id, user.Username, user.Email, user.Password);

        var userFound = await query.FirstOrDefaultAsync(cancellationToken);

        if (userFound is null)
        {
            return null;
        }

        var verifiedPassword = PasswordHasher.Check(userFound.Password, password);

        userFound.Password = null!;

        return verifiedPassword ? userFound : null;
    }

    public async Task<User?> InsertAsync(User user, CancellationToken cancellationToken)
    {
        var existing = await Context.Users.AnyAsync(u => u.Username == user.Username || u.Email == user.Email, cancellationToken);

        if (existing)
        {
            return null;
        }

        user.Password = PasswordHasher.Hash(user.Password);

        user.Id = Guid.Empty;

        Context.Users.Add(user);
        await Context.SaveChangesAsync(cancellationToken);

        return user;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await Context.Users.Select(user => new User(user.Id, user.Username, user.Email)).FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
    }

    public async Task ChangePasswordAsync(string email, string password, CancellationToken cancellationToken)
    {
        var user = await GetByEmailAsync(email, cancellationToken);

        if (user is null)
        {
            return;
        }

        user.Password = PasswordHasher.Hash(password);

        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task<EnumNewUserCheck> ValidateNewUserAsync(User user, CancellationToken cancellationToken)
    {
        var hasUsername = await Context.Users.AnyAsync(item => item.Username == user.Username, cancellationToken);
        var hasEmail = await Context.Users.AnyAsync(item => item.Email == user.Email, cancellationToken);

        return (hasUsername, hasEmail) switch
        {
            (true, _) => EnumNewUserCheck.ExistingUsername,
            (_, true) => EnumNewUserCheck.ExistingEmail,
            _ => EnumNewUserCheck.Valid
        };
    }

    public async Task<IEnumerable<User>> GetFollowersAsync(Guid userId, CancellationToken cancellationToken, int page = 1, int pageSize = 10)
    {
        var query = from follower in Context.Followers
                    join user in Context.Users on follower.FollowedId equals user.Id
                    orderby user.FirstName, user.LastName
                    where follower.FollowedId == userId
                    select new User(user.Id, user.Username, user.Email, user.FirstName, user.LastName, user.Image);

        return await query.Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetFollowingAsync(Guid userId, CancellationToken cancellationToken, int page = 1, int pageSize = 10)
    {
        var query = from follower in Context.Followers
                    join user in Context.Users on follower.FollowedId equals user.Id
                    orderby user.FirstName, user.LastName
                    where follower.UserId == userId
                    select new User(user.Id, user.Username, user.Email, user.FirstName, user.LastName, user.Image);

        return await query.Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetFollowersCountAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await Context.Followers.CountAsync(follower => follower.FollowedId == userId, cancellationToken);
    }

    public async Task<int> GetFollowingCountAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await Context.Followers.CountAsync(follower => follower.UserId == userId, cancellationToken);
    }

    public async Task<int> GetBooksCountAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await Context.Bookmarks.CountAsync(bookmark => bookmark.UserId == userId, cancellationToken);
    }

    public async Task<int> GetAuthorsCountAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await Context.Authors.Where(author => author.Books.Any(book => book.Bookmarks.Any(bookmark => bookmark.UserId == userId))).Distinct().CountAsync(cancellationToken);
    }

    public async Task<int> GetCategoriesCountAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await Context.Categories.Where(category => category.Books.Any(book => book.Bookmarks.Any(bookmark => bookmark.UserId == userId))).Distinct().CountAsync(cancellationToken);
    }

    public async Task<User?> GetByUsernameOrEmailAsync(string parameter, CancellationToken cancellationToken)
    {
        _ = Guid.TryParse(parameter, out var id);

        var query = from user in Context.Users
                    where user.Email == parameter || user.Username == parameter || user.Id == id
                    select new User(user.Id, user.Username, user.Email, user.FirstName, user.LastName, user.Image);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken)
    {
        return await Context.Users.CountAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> SearchAsync(Guid userId, string searchTerm, CancellationToken cancellationToken)
    {
        searchTerm = searchTerm.Trim().ToLower();

        if (searchTerm.Contains('@'))
        {
            var query = from user in Context.Users
                        where user.Email == searchTerm
                        select new User(user.Id, user.Username, user.Email, user.FirstName, user.LastName, user.Image);

            return await query.ToListAsync(cancellationToken);
        }

        var exactQuery = from user in Context.Users
                         where user.Username.ToLower().Contains(searchTerm)
                             || user.Email.ToLower().Contains(searchTerm)
                             || user.FirstName.ToLower().Contains(searchTerm)
                             || user.LastName.ToLower().Contains(searchTerm)
                             || string.Concat(user.FirstName, " ", user.LastName).ToLower().Contains(searchTerm)
                         select new User(user.Id, user.Username, user.Email, user.FirstName, user.LastName, user.Image);

        return await exactQuery.Take(20).ToListAsync(cancellationToken);
    }

    public async Task<User?> UpdateUserAsync(Guid userId, User user, CancellationToken cancellationToken)
    {
        var userToUpdate = await Context.Users
                                 .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

        if (userToUpdate == null)
        {
            return null;
        }

        userToUpdate.FirstName = user.FirstName ?? userToUpdate.FirstName;
        userToUpdate.LastName = user.LastName ?? userToUpdate.LastName;
        userToUpdate.Image = user.Image ?? userToUpdate.Image;
        userToUpdate.Description = user.Description ?? userToUpdate.Description;

        await Context.SaveChangesAsync(cancellationToken);

        return userToUpdate;
    }
}
