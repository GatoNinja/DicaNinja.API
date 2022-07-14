namespace DicaNinja.API.Helpers;

public class MessageResponse
{
    public string Message { get; }
    public DateTimeOffset Timestamp { get; }

    public MessageResponse(string message) : this(message, DateTimeOffset.Now)
    {
    }

    public MessageResponse(string message, DateTimeOffset timestamp)
    {
        Message = message;
        Timestamp = timestamp;
    }
}
