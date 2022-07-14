namespace DicaNinja.API.Response;

public class PersonResponse
{
    public Guid UserId { get; set; }
    public Guid PersonId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string Image { get; set; }
    public string? Description { get; set; }

    public PersonResponse()
    {
        UserId = Guid.Empty;
        PersonId = Guid.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        Username = string.Empty;
        Image = string.Empty;
        Description = string.Empty;
    }

    public PersonResponse(Guid userId, Guid personId, string firstName, string lastName, string username, string image, string? description)
    {
        UserId = userId;
        PersonId = personId;
        FirstName = firstName;
        LastName = lastName;
        Username = username;
        Image = image;
        Description = description;
    }
}
