namespace BookSearch.API.Helpers;

public record MessageResponse(string Message, DateTimeOffset Timestamp)
{
    public MessageResponse(string message) : this(message, DateTimeOffset.Now)
    {
    }
}