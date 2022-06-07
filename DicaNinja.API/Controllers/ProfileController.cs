
using DicaNinja.API.Abstracts;

using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Response;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Controllers;

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

        return userProfile is null ? this.NotFound() : this.Ok(userProfile);
    }

    [HttpGet("{parameter}")]
    [AllowAnonymous]
    public async Task<ActionResult<UserProfileResponse>> GetUserProfileAsync([FromRoute] string parameter)
    {
        var userProfile = await ProfileProvider.GetUserProfileAsync(parameter);

        return userProfile is null ? this.NotFound() : this.Ok(userProfile);
    }
}
