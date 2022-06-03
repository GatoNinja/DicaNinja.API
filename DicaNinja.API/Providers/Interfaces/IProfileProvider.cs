
using DicaNinja.API.Response;

namespace DicaNinja.API.Providers.Interfaces;

public interface IProfileProvider
{
    Task<UserProfileResponse?> GetUserProfileAsync(Guid userId);

    Task<UserProfileResponse?> GetUserProfileAsync(string parameter);
    
}
