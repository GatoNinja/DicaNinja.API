using AutoMapper;
using BookSearch.API.Abstracts;
using BookSearch.API.Helpers;
using BookSearch.API.Models;
using BookSearch.API.Providers.Interfaces;
using BookSearch.API.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.Controllers;

[Route("[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerHelper
{
    public UserController(IUserProvider userProvider, IMapper mapper)
    {
        this.UserProvider = userProvider;
        this.Mapper = mapper;
    }

    private IUserProvider UserProvider { get; }
    private IMapper Mapper { get; }

    [HttpGet("followers")]
    public async Task<ActionResult<IEnumerable<User>>> GetFollowers([FromQuery] QueryParameters query)
    {
        var followers = await this.UserProvider.GetFollowers(this.UserId, query.Page, query.PerPage);
        var total = await this.UserProvider.GetFollowersCount(this.UserId);
        var mapped = this.Mapper.Map<IEnumerable<UserResponse>>(followers);
        var paginated = PaginationHelper.CreatePagedResponse<UserResponse>(mapped, query, total);

        return this.Ok(paginated);
    }

    [HttpGet("following")]
    public async Task<ActionResult<IEnumerable<User>>> GetFollowing([FromQuery] QueryParameters query)
    {
        var following = await this.UserProvider.GetFollowing(this.UserId, query.Page, query.PerPage);
        var total = await this.UserProvider.GetFollowingCount(this.UserId);
        var mapped = this.Mapper.Map<IEnumerable<UserResponse>>(following);
        var paginated = PaginationHelper.CreatePagedResponse<UserResponse>(mapped, query, total);

        return this.Ok(paginated);
    }
}
