using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DicaNinja.API.Request;

[ValidateNever]
public class UserRequest
{
    public Guid UserId { get; set; }

    public string FirstName { get; private set; } = default!;

    public string LastName { get; private set; } = default!;

    public string Username { get; private set; } = default!;

    public string Image { get; private set; } = default!;

    public string? Description { get; set; }
}
