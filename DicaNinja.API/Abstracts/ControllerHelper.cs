
using DicaNinja.API.Helpers;

using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Abstracts;

public class ControllerHelper : ControllerBase
{
    protected Guid UserId
    {
        get
        {
            var claim = User.Claims.FirstOrDefault(claimToSearch => string.Compare(claimToSearch.Type, "Id", StringComparison.Ordinal) == 0);

            return claim is not null ? Guid.Parse(claim.Value) : throw new BadHttpRequestException(TextConstant.ForbiddenUser);
        }
    }
}
