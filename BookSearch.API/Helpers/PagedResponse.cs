namespace BookSearch.API.Helpers;

public sealed record PagedResponse<T> : Response<T>
{
    internal PagedResponse(T data, int page, int perPage) : base(data)
    {
        Page = page;
        PerPage = perPage;
    }

    public int Page { get; }

    public int PerPage { get; }

    public int TotalPages { get; set; }

    public int TotalRecords { get; set; }
}