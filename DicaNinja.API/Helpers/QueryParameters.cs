namespace DicaNinja.API.Helpers;

public record QueryParameters
{
    public QueryParameters()
    {
        Page = Page < 1 ? 1 : Page;
        PerPage = PerPage > 10 ? 10 : PerPage;
    }

    public int Page { get; } = 1;

    public int PerPage { get; } = 10;
}
