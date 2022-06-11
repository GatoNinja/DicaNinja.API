using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DicaNinja.API.Request;

[ValidateNever]
public class PersonRequest
{
    public Guid UserId { get; set; }

    public Guid PersonId { get; set; }

    public string FirstName { get; private set; } = default!;

    public string LastName { get; private set; } = default!;

    public string Username { get; private set; } = default!;

    public string Image { get; private set; } = default!;

    public string? Description { get; set; }
}
