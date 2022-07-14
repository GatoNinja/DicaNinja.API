namespace DicaNinja.API.Helpers;

public sealed class PagedResponse<T> : Response<T>
{
    public int Page { get; }
    public int PerPage { get; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }
    public PagedResponse(T data, int page, int perPage) : base(data)
    {
        Page = page;
        PerPage = perPage;
    }
}
