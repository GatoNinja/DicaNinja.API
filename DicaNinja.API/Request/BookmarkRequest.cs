namespace DicaNinja.API.Request;

public class BookmarkRequest
{
    public string Isbn { get; set; }
    public string Type { get; set; }

    public BookmarkRequest(string isbn, string type)
    {
        Isbn = isbn;
        Type = type;
    }
}
