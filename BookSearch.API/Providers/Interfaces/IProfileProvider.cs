using BookSearch.API.Response;

namespace BookSearch.API.Providers.Interfaces;

public interface IProfileProvider
{
    Task<UserProfileResponse?> GetUserProfileAsync(Guid userId);
}
