using BookSearch.API.Contexts;
using BookSearch.API.Enums;
using BookSearch.API.Models;
using BookSearch.API.Providers.Interfaces;
using BookSearch.API.Services;
using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.Providers;

public sealed class UserProvider : IUserProvider
{
    public UserProvider(BaseContext context, IPasswordHasher passwordHasher)
    {
        this.Context = context;
        this.PasswordHasher = passwordHasher;
    }

    private BaseContext Context { get; }
    private IPasswordHasher PasswordHasher { get; }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        var query = from user in this.Context.Users
                    join person in this.Context.People on user.Id equals person.UserId
                    where user.Id == id
                    select new User(user.Id, user.Username, user.Email, person);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<User?> DoLoginAsync(string username, string password)
    {
        var query = from user in this.Context.Users
                    where user.Username == username
                    select new User(user.Id, user.Username, user.Email, user.Password);

        var userFound = await query.FirstOrDefaultAsync();

        if (userFound is null)
        {
            return null;
        }

        var verifiedPassword = this.PasswordHasher.Check(userFound.Password, password);

        userFound.Password = null!;

        return verifiedPassword ? userFound : null;
    }

    public async Task<User?> InsertAsync(User user)
    {
        var existing = await this.Context.Users.AnyAsync(u => u.Username == user.Username || u.Email == user.Email);

        if (existing)
        {
            return null;
        }

        user.Password = this.PasswordHasher.Hash(user.Password);

        user.Id = Guid.Empty;

        this.Context.Users.Add(user);
        await this.Context.SaveChangesAsync();

        return user;
    }

    public async Task<User?> GetByEmail(string email)
    {
        return await this.Context.Users.Select(user => new User(user.Id, user.Username, user.Email))
                                  .FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task ChangePassword(string email, string password)
    {
        var user = await this.GetByEmail(email);

        if (user is null)
        {
            return;
        }

        user.Password = this.PasswordHasher.Hash(password);

        await this.Context.SaveChangesAsync();
    }

    public EnumNewUserCheck ValidateNewUser(User user)
    {
        var hasUsername = this.Context.Users.Any(item => item.Username == user.Username);
        var hasEmail = this.Context.Users.Any(item => item.Email == user.Email);

        return (hasUsername, hasEmail) switch
        {
            (true, _) => EnumNewUserCheck.ExistingUsername,
            (_, true) => EnumNewUserCheck.ExistingEmail,
            _ => EnumNewUserCheck.Valid
        };
    }

    public async Task<IEnumerable<User>> GetFollowers(Guid userId, int page = 1, int pageSize = 10)
    {
        var query = from follower in this.Context.Followers
                    join user in this.Context.Users on follower.FollowedId equals user.Id
                    join person in this.Context.People on user.Id equals person.UserId
                    where follower.UserId == userId
                    select new User(user.Id, user.Username, person);

        return await query.Skip((page - 1) * pageSize)
            .OrderBy(user => user.Person.FirstName)
            .ThenBy(user => user.Person.LastName)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<User>> GetFollowing(Guid userId, int page = 1, int pageSize = 10)
    {
        var query = from follower in this.Context.Followers
                    join user in this.Context.Users on follower.UserId equals user.Id
                    join person in this.Context.People on user.Id equals person.UserId
                    where follower.FollowedId == userId
                    select new User(user.Id, user.Username, person);

        return await query.Skip((page - 1) * pageSize)
            .OrderBy(user => user.Person.FirstName)
            .ThenBy(user => user.Person.LastName)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetFollowersCount(Guid userId)
    {
        return await this.Context.Followers.CountAsync(follower => follower.FollowedId == userId);
    }

    public async Task<int> GetFollowingCount(Guid userId)
    {
        return await this.Context.Followers.CountAsync(follower => follower.UserId == userId);
    }

    public async Task<int> GetBooksCount(Guid userId)
    {
        return await this.Context.Bookmarks.CountAsync(bookmark => bookmark.UserId == userId);
    }

    public async Task<int> GetAuthorsCount(Guid userId)
    {
        return await this.Context.Authors.Where(author => author.Books.Any(book => book.Bookmarks.Any(bookmark => bookmark.UserId == userId))).Distinct().CountAsync();
    }

    public async Task<int> GetCategoriesCount(Guid userId)
    {
        return await this.Context.Categories.Where(category => category.Books.Any(book => book.Bookmarks.Any(bookmark => bookmark.UserId == userId))).Distinct().CountAsync();
    }
}
