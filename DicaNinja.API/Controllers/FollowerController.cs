
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
        return request.Status switch
        {
            EnumStatusFollow.Follow => await FollowerProvider.FollowAsync(UserId, request.FollowerId, cancellationToken),
            EnumStatusFollow.UnFollow => await FollowerProvider.UnFollowAsync(UserId, request.FollowerId, cancellationToken),
            _ => throw new NotImplementedException()
        };
    }
}
