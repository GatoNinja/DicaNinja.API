
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
        var user = await UserProvider.GetByIdAsync(userId, cancellationToken).ConfigureAwait(false);

        return user is null ? null : await LoadProfile(user, cancellationToken).ConfigureAwait(false);
    }

    public async Task<UserProfileResponse?> GetUserProfileAsync(string parameter, CancellationToken cancellationToken)
    {
        var user = await UserProvider.GetByUsernameOrEmailAsync(parameter, cancellationToken).ConfigureAwait(false);

        return user is null ? null : await LoadProfile(user, cancellationToken).ConfigureAwait(false);
    }

    private async Task<UserProfileResponse> LoadProfile(User user, CancellationToken cancellationToken)
    {
        var userId = user.Id;
        var tasks = new Task<int>[]
        {
            UserProvider.GetBooksCountAsync(userId, cancellationToken),
            UserProvider.GetAuthorsCountAsync(userId, cancellationToken),
            UserProvider.GetCategoriesCountAsync(userId, cancellationToken),
            UserProvider.GetFollowersCountAsync(userId, cancellationToken),
            UserProvider.GetFollowingCountAsync(userId, cancellationToken)
        };
        var counts = await Task.WhenAll(tasks).ConfigureAwait(false);

        return new UserProfileResponse(user.Id, user.Username, user.Email, user.FirstName, user.LastName, TotalBooks: counts[0], TotalAuthors: counts[1], TotalCategories: counts[2], TotalFollowers: counts[3], TotalFollowing: counts[4]);
    }


}
