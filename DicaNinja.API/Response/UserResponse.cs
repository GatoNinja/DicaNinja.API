namespace DicaNinja.API.Response;

public record UserResponse(Guid Id, string Username, string FirstName, string LastName)
{
    public UserResponse() : this(Guid.Empty, string.Empty, string.Empty, string.Empty)
    {

    }
}
