namespace BookSearch.API.Request;

public record ReviewRequest(Guid BookId, string Text, int Rating);