
using DicaNinja.API.Helpers;

using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Abstracts;

public class ControllerHelper : ControllerBase
{
    protected Guid GetUserId()
    {
        var claim = User.Claims.FirstOrDefault(claimToSearch => string.Equals(claimToSearch.Type, "Id", StringComparison.Ordinal));

        return claim is null ? throw new BadHttpRequestException(TextConstant.ForbiddenUser) : Guid.Parse(claim.Value);
    }

    protected bool IsAuthenticated()
    {
        return User.Claims.Any();
    }
}
