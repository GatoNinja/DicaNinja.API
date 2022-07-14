namespace DicaNinja.API.Response;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public UserResponse()
    {
        Id = Guid.Empty;
        Username = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
    }

    public UserResponse(Guid id, string username, string firstName, string lastName)
    {
        Id = id;
        Username = username;
        FirstName = firstName;
        LastName = lastName;
    }
}
