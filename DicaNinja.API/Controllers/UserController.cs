using AutoMapper;

using DicaNinja.API.Abstracts;
using DicaNinja.API.Helpers;
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;
using DicaNinja.API.Request;
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

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UserRequest request, CancellationToken cancellationToken)
    {
        var user = Mapper.Map<User>(request);
        var updatedUser = await UserProvider.UpdateUserAsync(UserId, user, cancellationToken);

        return updatedUser is null ? NotFound() : Ok(Mapper.Map<DicaNinja.API.Response.UserResponse>(updatedUser));
    }

    [HttpGet()]
    public async Task<IActionResult> Search([FromQuery] string query, CancellationToken cancellationToken)
    {
        var users = await UserProvider.SearchAsync(UserId, query, cancellationToken);

        return Ok(users);

    }

    [HttpGet("followers")]
    public async Task<ActionResult<IEnumerable<User>>> GetFollowers([FromQuery] QueryParameters query, CancellationToken cancellationToken)
    {
        var followers = await UserProvider.GetFollowersAsync(UserId, cancellationToken, query.Page, query.PerPage);
        var total = await UserProvider.GetFollowersCountAsync(UserId, cancellationToken);
        var mapped = Mapper.Map<IEnumerable<UserResponse>>(followers);
        var paginated = PaginationHelper.CreatePagedResponse(mapped, query, total);

        return Ok(paginated);
    }

    [HttpGet("following")]
    public async Task<ActionResult<IEnumerable<User>>> GetFollowing([FromQuery] QueryParameters query, CancellationToken cancellationToken)
    {
        var following = await UserProvider.GetFollowingAsync(UserId, cancellationToken, query.Page, query.PerPage);
        var total = await UserProvider.GetFollowingCountAsync(UserId, cancellationToken);
        var mapped = Mapper.Map<IEnumerable<UserResponse>>(following);
        var paginated = PaginationHelper.CreatePagedResponse(mapped, query, total);

        return Ok(paginated);
    }
}
