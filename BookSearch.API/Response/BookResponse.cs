namespace BookSearch.API.Response;

public class BookResponse
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int PageCount { get; set; }
    public string Publisher { get; set; } = string.Empty;
    public string PublicationDate { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public double AverageRating { get; set; }
    public IEnumerable<string> Categories { get; set; } = default!;
    public IEnumerable<string> Authors { get; set; } = default!;
    public List<IdentifierDTO> Identifiers { get; set; } = default!;
    public bool IsBookmarked { get; set; } = false;

    public BookResponse()
    {

    }
}
