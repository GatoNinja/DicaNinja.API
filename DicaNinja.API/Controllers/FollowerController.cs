
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
        this.FollowerProvider = followerProvider;
    }

    private IFollowerProvider FollowerProvider { get; }

    [HttpPost()]
    public async Task<ActionResult<bool>> ChangeFollowStatus([FromBody] FollowInfo request)
    {
        return request.Status switch
        {
            EnumStatusFollow.Follow => await this.FollowerProvider.Follow(this.UserId, request.FollowerId),
            EnumStatusFollow.Unfollow => await this.FollowerProvider.Unfollow(this.UserId, request.FollowerId),
            _ => throw new NotImplementedException()
        };
    }
}
