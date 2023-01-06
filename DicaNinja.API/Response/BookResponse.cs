namespace DicaNinja.API.Response;

public class BookResponse
{
    public Guid Id { get; private set; } = Guid.Empty;
    public string Title { get; private set; } = string.Empty;
    public string Subtitle { get; private set; } = string.Empty;
    public string Language { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int PageCount { get; set; }
    public string Publisher { get; private set; } = string.Empty;
    public string PublicationDate { get; private set; } = string.Empty;
    public string Image { get; private set; } = string.Empty;
    public double AverageRating { get; set; }
    public IEnumerable<string> Categories { get; private set; } = default!;
    public IEnumerable<string> Authors { get; private set; } = default!;
    public List<IdentifierResponse> Identifiers { get; private set; } = new();
    public bool IsBookMarked { get; set; }
    public double InternalRating { get; set; }

    public string PreviewLink { get; set; } = string.Empty;

    public BookResponse()
    {

    }
}
