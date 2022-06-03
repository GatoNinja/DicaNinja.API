namespace DicaNinja.API.Response;

public record UserProfileResponse(Guid Id, string Username, string Email, string FirstName, string LastName, int TotalBooks, int TotalAuthors, int TotalCategories, int TotalFollowers, int TotalFollowing);
