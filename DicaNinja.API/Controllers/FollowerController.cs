
using DicaNinja.API.Abstracts;
using DicaNinja.API.Enums;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Response;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class FollowerController : ControllerHelper
{
    public FollowerController(IFollowerProvider followerProvider)
    {
        FollowerProvider = followerProvider;
    }

    private IFollowerProvider FollowerProvider { get; }

    [HttpPost()]
    public async Task<ActionResult<bool>> ChangeFollowStatusAsync([FromBody] FollowInfo request, CancellationToken cancellationToken)
    {
        return request is null
            ? throw new ArgumentNullException(nameof(request))
            : (ActionResult<bool>)(request.Status switch
        {
            EnumStatusFollow.Follow => await FollowerProvider.FollowAsync(GetUserId(), request.FollowerId, cancellationToken).ConfigureAwait(false),
            EnumStatusFollow.UnFollow => await FollowerProvider.UnFollowAsync(GetUserId(), request.FollowerId, cancellationToken).ConfigureAwait(false),
            _ => throw new NotImplementedException()
        });
    }
}
