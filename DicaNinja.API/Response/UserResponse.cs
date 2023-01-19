namespace DicaNinja.API.Response;

public class UserResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Image { get; set; }
    public string? Description { get; set; }

    public UserResponse()
    {
        Id = Guid.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        Username = string.Empty;
        Image = string.Empty;
        Description = string.Empty;
    }

    public UserResponse(Guid id, string firstName, string lastName, string username, string image, string? description)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Image = image;
        Description = description;
    }
}
