namespace BookSearch.API.DDD.Book;

public class BookResponse
{
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public IEnumerable<BookIdentifier> Identifiers { get; set; } = default!;
    public string Language { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public IEnumerable<string> Categories { get; set; } = default!;
    public int PageCount { get; set; }
    public string Publisher { get; set; } = string.Empty;
    public string PublicationDate { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public IEnumerable<string> Authors { get; set; } = default!;
    public double AverageRating { get; set; }
    public bool IsFavorite { get; set; }

    public BookResponse()
    {
        
    }
}
