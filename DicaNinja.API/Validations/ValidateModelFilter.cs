using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DicaNinja.API.Validations;

public class ValidateModelFilter : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context is null || context.ModelState.IsValid)
        {
            return;
        }
        context.Result = new BadRequestObjectResult(context.ModelState);
    }
}
