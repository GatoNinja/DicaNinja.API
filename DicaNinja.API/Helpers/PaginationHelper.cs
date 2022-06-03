namespace DicaNinja.API.Helpers;

public static class PaginationHelper
{
    public static PagedResponse<IEnumerable<T>> CreatePagedResponse<T>(IEnumerable<T> pagedData,
        QueryParameters validQueryString, int totalRecords)
    {
        var response = new PagedResponse<IEnumerable<T>>(pagedData, validQueryString.Page, validQueryString.PerPage);
        var totalPages = totalRecords / (double)validQueryString.Page;
        var roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

        response.TotalPages = roundedTotalPages;
        response.TotalRecords = totalRecords;

        return response;
    }
}
