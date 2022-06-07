using AutoMapper;

using DicaNinja.API.Abstracts;
using DicaNinja.API.Helpers;
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Response;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerHelper
{
    public UserController(IUserProvider userProvider, IMapper mapper)
    {
        UserProvider = userProvider;
        Mapper = mapper;
    }

    private IUserProvider UserProvider { get; }
    private IMapper Mapper { get; }

    [HttpGet("followers")]
    public async Task<ActionResult<IEnumerable<User>>> GetFollowers([FromQuery] QueryParameters query)
    {
        var followers = await UserProvider.GetFollowers(UserId, query.Page, query.PerPage);
        var total = await UserProvider.GetFollowersCount(UserId);
        var mapped = Mapper.Map<IEnumerable<UserResponse>>(followers);
        var paginated = PaginationHelper.CreatePagedResponse(mapped, query, total);

        return Ok(paginated);
    }

    [HttpGet("following")]
    public async Task<ActionResult<IEnumerable<User>>> GetFollowing([FromQuery] QueryParameters query)
    {
        var following = await UserProvider.GetFollowing(UserId, query.Page, query.PerPage);
        var total = await UserProvider.GetFollowingCount(UserId);
        var mapped = Mapper.Map<IEnumerable<UserResponse>>(following);
        var paginated = PaginationHelper.CreatePagedResponse(mapped, query, total);

        return Ok(paginated);
    }
}
