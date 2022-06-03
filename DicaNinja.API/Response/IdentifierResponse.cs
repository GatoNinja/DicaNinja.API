namespace DicaNinja.API.Response;

public record IdentifierResponse(string Isbn, string Type)
{
    public IdentifierResponse() : this(string.Empty, string.Empty)
    {

    }
}
