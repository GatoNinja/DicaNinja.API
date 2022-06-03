using BookSearch.API.Helpers;

using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.Abstracts;

public class ControllerHelper : ControllerBase
{
    protected Guid UserId
    {
        get
        {
            var claim = this.User.Claims.FirstOrDefault(claimToSearch => claimToSearch.Type == "Id");

            if (claim is null)
            {
                throw new BadHttpRequestException(TextConstant.ForbiddenUser);
            }

            return Guid.Parse(claim.Value);
        }
    }
}
