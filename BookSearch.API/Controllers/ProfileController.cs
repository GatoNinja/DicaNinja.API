using BookSearch.API.Abstracts;
using BookSearch.API.Providers.Interfaces;
using BookSearch.API.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ProfileController : ControllerHelper
{
    public ProfileController(IProfileProvider profileProvider)
    {
        ProfileProvider = profileProvider;
    }

    private IProfileProvider ProfileProvider { get; }

    [HttpGet]
    public async Task<ActionResult<UserProfileResponse>> GetUserProfileAsync()
    {
        var userProfile = await ProfileProvider.GetUserProfileAsync(UserId);

        if (userProfile is null)
        {
            return NotFound();
        }

        return Ok(userProfile);
    }
}
