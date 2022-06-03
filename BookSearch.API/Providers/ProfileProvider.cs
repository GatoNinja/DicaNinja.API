using BookSearch.API.Providers.Interfaces;
using BookSearch.API.Response;

namespace BookSearch.API.Providers;

public class ProfileProvider : IProfileProvider
{
    public ProfileProvider(IUserProvider userProvider)
    {
        UserProvider = userProvider;
    }

    private IUserProvider UserProvider { get; }

    public async Task<UserProfileResponse?> GetUserProfileAsync(Guid userId)
    {
        var user = await UserProvider.GetByIdAsync(userId);

        if (user is null)
        {
            return null;
        }

        var totalBooks = await UserProvider.GetBooksCount(userId);
        var totalAuthors = await UserProvider.GetAuthorsCount(userId);
        var totalCategories = await UserProvider.GetCategoriesCount(userId);
        var totalFollowers = await UserProvider.GetFollowersCount(userId);
        var totalFollowing = await UserProvider.GetFollowingCount(userId);

        return new UserProfileResponse(user.Id, user.Username, user.Email, user.Person.FirstName, user.Person.LastName, totalBooks, totalAuthors, totalCategories, totalFollowers, totalFollowing);
    }
}
