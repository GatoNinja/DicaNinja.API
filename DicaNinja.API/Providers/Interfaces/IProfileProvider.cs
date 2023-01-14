
using DicaNinja.API.Response;

namespace DicaNinja.API.Providers.Interfaces;

public interface IProfileProvider
{
    Task<UserProfileResponse?> GetUserProfileAsync(Guid userId, CancellationToken cancellation);

    Task<UserProfileResponse?> GetUserProfileAsync(string parameter, CancellationToken cancellation);

}
