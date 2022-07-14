namespace DicaNinja.API.Response;

public class UserResponse
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Image { get; set; }
    public string? Description { get; set; }

    public UserResponse()
    {
        UserId = Guid.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        Username = string.Empty;
        Image = string.Empty;
        Description = string.Empty;
    }

    public UserResponse(Guid userId, string firstName, string lastName, string username, string image, string? description)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Image = image;
        Description = description;
    }
}
