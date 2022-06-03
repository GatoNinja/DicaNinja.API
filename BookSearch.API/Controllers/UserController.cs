using AutoMapper;
using BookSearch.API.Abstracts;
using BookSearch.API.Helpers;
using BookSearch.API.Models;
using BookSearch.API.Providers.Interfaces;
using BookSearch.API.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.Providers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerHelper
{
    public UserController(IUserProvider userProvider, IFollowerProvider followerProvider, IMapper mapper)
    {
        UserProvider = userProvider;
        FollowerProvider = followerProvider;
        Mapper = mapper;
    }

    private IUserProvider UserProvider { get; }
    private IFollowerProvider FollowerProvider { get; }
    private IMapper Mapper { get; }

    [HttpGet("followers")]
    public async Task<ActionResult<IEnumerable<User>>> GetFollowers([FromQuery] QueryParameters query)
    {
        var followers = await UserProvider.GetFollowers(UserId, query.Page, query.PerPage);
        var total = await UserProvider.GetFollowersCount(UserId);
        var mapped = Mapper.Map<IEnumerable<UserResponse>>(followers);
        var paginated = PaginationHelper.CreatePagedResponse<UserResponse>(mapped, query, total);

        return Ok(followers);
    }

    [HttpGet("following")]
    public async Task<ActionResult<IEnumerable<User>>> GetFollowing([FromQuery] QueryParameters query)
    {
        var following = await UserProvider.GetFollowing(UserId, query.Page, query.PerPage);
        var total = await UserProvider.GetFollowingCount(UserId);
        var mapped = Mapper.Map<IEnumerable<UserResponse>>(following);
        var paginated = PaginationHelper.CreatePagedResponse<UserResponse>(mapped, query, total);

        return Ok(following);
    }
}
