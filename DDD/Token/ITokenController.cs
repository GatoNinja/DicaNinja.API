
using BookSearch.API.DDD.User;

using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.DDD.Token;

public interface ITokenController
{
    IUserRepository UserRepository { get; }
    ITokenService TokenService { get; }
    Task<ActionResult<TokenResponse>> PostTokenAsync([FromBody] LoginPayload request);
}