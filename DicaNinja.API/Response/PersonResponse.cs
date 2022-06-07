namespace DicaNinja.API.Request;

public record PersonResponse(Guid UserId, Guid PersonId, string FirstName, string LastName, string Username, string Image, string? Description);
