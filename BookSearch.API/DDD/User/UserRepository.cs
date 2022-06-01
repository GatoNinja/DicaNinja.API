
using BookSearch.API.Contexts;
using BookSearch.API.DDD.PasswordHasher;
using BookSearch.API.DDD.SignUp;

using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.DDD.User
{
    public sealed class UserRepository : IUserRepository
    {
        public UserRepository(DefaultContext context, IPasswordHasher passwordHasher)
        {
            Context = context;
            PasswordHasher = passwordHasher;
        }

        private DefaultContext Context { get; }
        private IPasswordHasher PasswordHasher { get; }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await Context.Users
                .Include(user => user.PersonModel)
                .FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User?> DoLoginAsync(string username, string password)
        {
            var query = from user in Context.Users
                        where user.Username == username
                        select new User(user.Id, user.Username, user.Email, user.Password);

            var userFound = await query.FirstOrDefaultAsync();

            if (userFound is null)
            {
                return null;
            }

            var (verifiedPassword, _) = PasswordHasher.Check(userFound.Password, password);

            return verifiedPassword ? userFound : null;
        }

        public async Task<User?> InsertAsync(User model)
        {
            model.Password = PasswordHasher.Hash(model.Password);

            Context.Users.Add(model);
            await Context.SaveChangesAsync();

            return model;
        }

        public async Task<User?> GetByEmail(string email)
        {
            return await Context.Users.FirstOrDefaultAsync(user => user.Email == email);
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

        public EnumNewUserCheck ValidateNewUser(User userModel)
        {
            var hasUsername = Context.Users.Any(item => item.Username == userModel.Username);
            var hasEmail = Context.Users.Any(item => item.Email == userModel.Email);

            return (hasUsername, hasEmail) switch
            {
                (true, _) => EnumNewUserCheck.ExistingUsername,
                (_, true) => EnumNewUserCheck.ExistingEmail,
                _ => EnumNewUserCheck.Valid
            };
        }
    }
}