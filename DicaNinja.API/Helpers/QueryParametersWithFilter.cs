using Microsoft.AspNetCore.Mvc;

namespace DicaNinja.API.Helpers;

public class QueryParametersWithFilter
{
    private int _perPage = 10;

    public int Page { get; set; } = 1;

    public int PerPage
    {
        get => _perPage;
        set => _perPage = value > 10 ? 10 : value;
    }

    public string Query { get; set; } = string.Empty;

    public string? Lang { get; set; } = null;
}
