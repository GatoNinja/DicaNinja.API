
using DicaNinja.API.Helpers;

using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Abstracts;

public class ControllerHelper : ControllerBase
{
    protected Guid UserId
    {
        get
        {
            var claim = User.Claims.FirstOrDefault(claimToSearch => claimToSearch.Type == "Id");

            if (claim is null)
            {
                throw new BadHttpRequestException(TextConstant.ForbiddenUser);
            }

            return Guid.Parse(claim.Value);
        }
    }
}
