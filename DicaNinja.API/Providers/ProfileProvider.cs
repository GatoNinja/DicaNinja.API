
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

    public async Task<UserProfileResponse?> GetUserProfileAsync(Guid userId, CancellationToken cancellation)
    {
        var user = await UserProvider.GetByIdAsync(userId, cancellation).ConfigureAwait(false);

        return user is null ? null : await LoadProfile(user, cancellation).ConfigureAwait(false);
    }

    public async Task<UserProfileResponse?> GetUserProfileAsync(string parameter, CancellationToken cancellation)
    {
        var user = await UserProvider.GetByUsernameOrEmailAsync(parameter, cancellation).ConfigureAwait(false);

        return user is null ? null : await LoadProfile(user, cancellation).ConfigureAwait(false);
    }

    private async Task<UserProfileResponse> LoadProfile(User user, CancellationToken cancellation)
    {
        var userId = user.Id;
        var tasks = new Task<int>[]
        {
            UserProvider.GetBooksCountAsync(userId, cancellation),
            UserProvider.GetAuthorsCountAsync(userId, cancellation),
            UserProvider.GetCategoriesCountAsync(userId, cancellation),
            UserProvider.GetFollowersCountAsync(userId, cancellation),
            UserProvider.GetFollowingCountAsync(userId, cancellation)
        };
        var counts = await Task.WhenAll(tasks).ConfigureAwait(false);

        return new UserProfileResponse(user.Id, user.Username, user.Email, user.FirstName, user.LastName, TotalBooks: counts[0], TotalAuthors: counts[1], TotalCategories: counts[2], TotalFollowers: counts[3], TotalFollowing: counts[4]);
    }


}
