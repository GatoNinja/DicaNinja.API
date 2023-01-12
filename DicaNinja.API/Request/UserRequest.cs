using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DicaNinja.API.Request;

[ValidateNever]
public class UserRequest
{
    public Guid UserId { get; set; }

    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Image { get; set; } = default!;

    public string Password { get; set; } = default!;

    public string? Description { get; set; }
}
