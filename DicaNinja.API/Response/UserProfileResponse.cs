namespace DicaNinja.API.Response;

public class UserProfileResponse
{
    public UserProfileResponse(Guid Id, string Username, string Email, string FirstName, string LastName, int TotalBooks, int TotalAuthors, int TotalCategories, int TotalFollowers, int TotalFollowing)
    {
        this.Id = Id;
        this.Username = Username;
        this.Email = Email;
        this.FirstName = FirstName;
        this.LastName = LastName;
        this.TotalBooks = TotalBooks;
        this.TotalAuthors = TotalAuthors;
        this.TotalCategories = TotalCategories;
        this.TotalFollowers = TotalFollowers;
        this.TotalFollowing = TotalFollowing;
    }

    public Guid Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int TotalBooks { get; set; }

    public int TotalAuthors { get; set; }

    public int TotalCategories { get; set; }

    public int TotalFollowers { get; set; }

    public int TotalFollowing { get; set; }
}
