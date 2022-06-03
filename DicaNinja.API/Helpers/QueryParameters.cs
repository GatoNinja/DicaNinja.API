namespace DicaNinja.API.Helpers;

public record QueryParameters
{
    public QueryParameters()
    {
        this.Page = this.Page < 1 ? 1 : this.Page;
        this.PerPage = this.PerPage > 10 ? 10 : this.PerPage;
    }

    public int Page { get; } = 1;

    public int PerPage { get; } = 10;
}
