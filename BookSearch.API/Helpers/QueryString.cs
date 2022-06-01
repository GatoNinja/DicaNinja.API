namespace BookSearch.API.Helpers
{
    public record QueryString
    {
        public QueryString()
        {
            Page = Page < 1 ? 1 : Page;
            PerPage = PerPage > 10 ? 10 : PerPage;
        }

        public int Page { get; set; } = 1;

        public int PerPage { get; set; } = 10;
    }
}