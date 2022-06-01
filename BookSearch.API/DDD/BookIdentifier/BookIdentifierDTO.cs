namespace BookSearch.API.DDD.BookIdentifier
{
    public class BookIdentifierDTO
    {

        public BookIdentifierDTO(string identifier, string type)
        {
            Isbn = identifier;
            Type = type;
        }

        public string Isbn { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}