using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DicaNinja.API.Request;

[ValidateNever]
public class PersonRequest
{
    public Guid UserId { get; set; }

    public Guid PersonId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Username { get; set; }

    public string Image { get; set; }

    public string? Description { get; set; }
}
