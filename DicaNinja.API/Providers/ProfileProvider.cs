
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

    public async Task<UserProfileResponse?> GetUserProfileAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await UserProvider.GetByIdAsync(userId, cancellationToken);

        return user is null ? null : await LoadProfile(user, cancellationToken);
    }

    public async Task<UserProfileResponse?> GetUserProfileAsync(string parameter, CancellationToken cancellationToken)
    {
        var user = await UserProvider.GetByUsernameOrEmailAsync(parameter, cancellationToken);

        return user is null ? null : await LoadProfile(user, cancellationToken);
    }

    private async Task<UserProfileResponse> LoadProfile(User user, CancellationToken cancellationToken)
    {
        var userId = user.Id;
        var totalBooks = await UserProvider.GetBooksCountAsync(userId, cancellationToken);
        var totalAuthors = await UserProvider.GetAuthorsCountAsync(userId, cancellationToken);
        var totalCategories = await UserProvider.GetCategoriesCountAsync(userId, cancellationToken);
        var totalFollowers = await UserProvider.GetFollowersCountAsync(userId, cancellationToken);
        var totalFollowing = await UserProvider.GetFollowingCountAsync(userId, cancellationToken);

        return new UserProfileResponse(user.Id, user.Username, user.Email, user.FirstName, user.LastName, totalBooks, totalAuthors, totalCategories, totalFollowers, totalFollowing);
    }

}
