namespace DicaNinja.API.Response;

public class IdentifierResponse
{
    public IdentifierResponse(string isbn, string type)
    {
        Isbn = isbn;
        Type = type;
    }

    public string Isbn { get; set; }
    public string Type { get; set; }
}
