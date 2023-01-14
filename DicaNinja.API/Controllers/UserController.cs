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
    public async Task<IActionResult> UpdateUser([FromBody] UserRequest request, CancellationToken cancellation)
    {
        var user = Mapper.Map<User>(request);
        var updatedUser = await UserProvider.UpdateUserAsync(GetUserId(), user, cancellation).ConfigureAwait(false);

        return updatedUser is null ? NotFound() : Ok(Mapper.Map<DicaNinja.API.Response.UserResponse>(updatedUser));
    }

    [HttpGet()]
    public async Task<IActionResult> Search([FromQuery] string query, CancellationToken cancellation)
    {
        var users = await UserProvider.SearchAsync(query, cancellation).ConfigureAwait(false);

        return Ok(users);

    }

    [HttpGet("followers")]
    public async Task<ActionResult<IEnumerable<User>>> GetFollowers([FromQuery] QueryParametersWithFilter query, CancellationToken cancellation)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        var followers = await UserProvider.GetFollowersAsync(GetUserId(), cancellation, query.Page, query.PerPage).ConfigureAwait(false);
        var total = await UserProvider.GetFollowersCountAsync(GetUserId(), cancellation).ConfigureAwait(false);
        var mapped = Mapper.Map<IEnumerable<UserResponse>>(followers);
        var paginated = PaginationHelper.CreatePagedResponse(mapped, query, total);

        return Ok(paginated);
    }

    [HttpGet("following")]
    public async Task<ActionResult<IEnumerable<User>>> GetFollowing([FromQuery] QueryParametersWithFilter query, CancellationToken cancellation)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        var following = await UserProvider.GetFollowingAsync(GetUserId(), cancellation, query.Page, query.PerPage).ConfigureAwait(false);
        var total = await UserProvider.GetFollowingCountAsync(GetUserId(), cancellation).ConfigureAwait(false);
        var mapped = Mapper.Map<IEnumerable<UserResponse>>(following);
        var paginated = PaginationHelper.CreatePagedResponse(mapped, query, total);

        return Ok(paginated);
    }
}
