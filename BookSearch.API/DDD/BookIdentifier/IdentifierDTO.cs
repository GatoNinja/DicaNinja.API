namespace BookSearch.API.DDD.Identifier
{
    public class IdentifierDTO
    {

        public IdentifierDTO(string identifier, string type)
        {
            Isbn = identifier;
            Type = type;
        }

        public string Isbn { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
