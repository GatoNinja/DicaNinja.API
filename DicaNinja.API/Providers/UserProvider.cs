
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

    public async Task<User?> GetByIdAsync(Guid id)
    {
        var query = from user in Context.Users
                    join person in Context.People on user.Id equals person.UserId
                    where user.Id == id
                    select new User(user.Id, user.Username, user.Email, person);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<User?> DoLoginAsync(string username, string password)
    {
        var query = from user in Context.Users
                    where user.Username == username || user.Email == username
                    select new User(user.Id, user.Username, user.Email, user.Password);

        var userFound = await query.FirstOrDefaultAsync();

        if (userFound is null)
        {
            return null;
        }

        var verifiedPassword = PasswordHasher.Check(userFound.Password, password);

        userFound.Password = null!;

        return verifiedPassword ? userFound : null;
    }

    public async Task<User?> InsertAsync(User user)
    {
        var existing = await Context.Users.AnyAsync(u => u.Username == user.Username || u.Email == user.Email);

        if (existing)
        {
            return null;
        }

        user.Password = PasswordHasher.Hash(user.Password);

        user.Person.Id = Guid.Empty;
        user.Person.UserId = Guid.Empty;
        user.Id = Guid.Empty;

        Context.Users.Add(user);
        await Context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await Context.Users.Select(user => new User(user.Id, user.Username, user.Email)).FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task ChangePassword(string email, string password)
    {
        var user = await GetByEmail(email);

        if (user is null)
        {
            return;
        }

        user.Password = PasswordHasher.Hash(password);

        await Context.SaveChangesAsync();
    }

    public EnumNewUserCheck ValidateNewUser(User user)
    {
        var hasUsername = Context.Users.Any(item => item.Username == user.Username);
        var hasEmail = Context.Users.Any(item => item.Email == user.Email);

        return (hasUsername, hasEmail) switch
        {
            (true, _) => EnumNewUserCheck.ExistingUsername,
            (_, true) => EnumNewUserCheck.ExistingEmail,
            _ => EnumNewUserCheck.Valid
        };
    }

    public async Task<IEnumerable<User>> GetFollowers(Guid userId, int page = 1, int pageSize = 10)
    {
        var query = from follower in Context.Followers
                    join user in Context.Users on follower.FollowedId equals user.Id
                    join person in Context.People on user.Id equals person.UserId
                    orderby person.FirstName, person.LastName
                    where follower.FollowedId == userId
                    select new User(user.Id, user.Username, person);

        return await query.Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetFollowing(Guid userId, int page = 1, int pageSize = 10)
    {
        var query = from follower in Context.Followers
                    join user in Context.Users on follower.FollowedId equals user.Id
                    join person in Context.People on user.Id equals person.UserId
                    orderby person.FirstName, person.LastName
                    where follower.UserId == userId
                    select new User(user.Id, user.Username, person);

        return await query.Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetFollowersCount(Guid userId)
    {
        return await Context.Followers.CountAsync(follower => follower.FollowedId == userId);
    }

    public async Task<int> GetFollowingCount(Guid userId)
    {
        return await Context.Followers.CountAsync(follower => follower.UserId == userId);
    }

    public async Task<int> GetBooksCount(Guid userId)
    {
        return await Context.Bookmarks.CountAsync(bookmark => bookmark.UserId == userId);
    }

    public async Task<int> GetAuthorsCount(Guid userId)
    {
        return await Context.Authors.Where(author => author.Books.Any(book => book.Bookmarks.Any(bookmark => bookmark.UserId == userId))).Distinct().CountAsync();
    }

    public async Task<int> GetCategoriesCount(Guid userId)
    {
        return await Context.Categories.Where(category => category.Books.Any(book => book.Bookmarks.Any(bookmark => bookmark.UserId == userId))).Distinct().CountAsync();
    }

    public async Task<User?> GetByUsernameOrEmail(string parameter)
    {
        _ = Guid.TryParse(parameter, out var id);

        var query = from user in Context.Users
                    join person in Context.People on user.Id equals person.UserId
                    where user.Email == parameter || user.Username == parameter || user.Id == id
                    select new User(user.Id, user.Username, user.Email, person);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<int> GetCount()
    {
        return await Context.Users.CountAsync();
    }
}
