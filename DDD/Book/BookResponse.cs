namespace BookSearch.API.DDD.Book;
public record BookResponse(string Title, string Subtitle, IEnumerable<BookIdentifier> Identifiers, string Language, string Description, IEnumerable<string> Categories, int PageCount, string Publisher, string PublicationDate, string Image, IEnumerable<string> Authors, double AverageRating);

