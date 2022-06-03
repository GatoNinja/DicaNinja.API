using BookSearch.API.Abstracts;
using BookSearch.API.Enums;
using BookSearch.API.Providers.Interfaces;
using BookSearch.API.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.Controllers;

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
    public async Task<ActionResult<bool>> ChangeFollowStatus([FromBody] FollowInfo request)
    {
        if (request.Status == EnumStatusFollow.Follow)
        {
            return await FollowerProvider.Follow(UserId, request.FollowerId);
        }

        return await FollowerProvider.UnFollow(UserId, request.FollowerId);
    }
}
