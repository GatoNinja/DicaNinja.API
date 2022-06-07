
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Response;

namespace DicaNinja.API.Providers;

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

        return user is null ? null : await LoadProfile(user);
    }

    public async Task<UserProfileResponse?> GetUserProfileAsync(string parameter)
    {
        var user = await UserProvider.GetByUsernameOrEmailAsync(parameter);

        return user is null ? null : await LoadProfile(user);
    }

    private async Task<UserProfileResponse> LoadProfile(User user)
    {
        var userId = user.Id;
        var totalBooks = await UserProvider.GetBooksCountAsync(userId);
        var totalAuthors = await UserProvider.GetAuthorsCountAsync(userId);
        var totalCategories = await UserProvider.GetCategoriesCountAsync(userId);
        var totalFollowers = await UserProvider.GetFollowersCountAsync(userId);
        var totalFollowing = await UserProvider.GetFollowingCountAsync(userId);

        return new UserProfileResponse(user.Id, user.Username, user.Email, user.Person.FirstName, user.Person.LastName, totalBooks, totalAuthors, totalCategories, totalFollowers, totalFollowing);
    }

}
