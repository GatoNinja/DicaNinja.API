using BookSearch.API.Helpers;

using Microsoft.AspNetCore.Mvc;

namespace BookSearch.API.Abstracts
{
    public class ControllerHelper : ControllerBase
    {
        public Guid UserId
        {
            get
            {
                var claim = User.Claims.FirstOrDefault(clainToSearch => clainToSearch.Type == "Id");

                if (claim is null)
                {
                    throw new BadHttpRequestException(TextConstant.ForbiddenUser);
                }

                return Guid.Parse(claim.Value);
            }
        }
    }
}
