namespace DicaNinja.API.Request;

public class ReviewRequest
{
    public Guid BookId { get; set; }
    public string Text { get; set; }
    public int Rating { get; set; }

    public ReviewRequest(Guid bookId, string text, int rating)
    {
        BookId = bookId;
        Text = text;
        Rating = rating;
    }
}
