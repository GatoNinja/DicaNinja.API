
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
    public async Task<ActionResult<UserProfileResponse>> GetUserProfileAsync(CancellationToken cancellationToken)
    {
        var userProfile = await ProfileProvider.GetUserProfileAsync(GetUserId(), cancellationToken).ConfigureAwait(false);

        return userProfile is null ? NotFound() : Ok(userProfile);
    }

    [HttpGet("{parameter}")]
    [AllowAnonymous]
    public async Task<ActionResult<UserProfileResponse>> GetUserProfileAsync([FromRoute] string parameter, CancellationToken cancellationToken)
    {
        var userProfile = await ProfileProvider.GetUserProfileAsync(parameter, cancellationToken).ConfigureAwait(false);

        return userProfile is null ? NotFound() : Ok(userProfile);
    }
}
