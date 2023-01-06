namespace DicaNinja.API.Helpers;

public static class PaginationHelper
{
    public static PagedResponse<IEnumerable<T>> CreatePagedResponse<T>(IEnumerable<T> pagedData,
        QueryParameters queryString, int totalRecords)
    {
        if (queryString == null)
        {
            throw new ArgumentNullException(nameof(queryString));
        }

        var response = new PagedResponse<IEnumerable<T>>(pagedData, queryString.Page, queryString.PerPage);
        var totalPages = totalRecords / (double)queryString.PerPage;
        var roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

        response.TotalPages = roundedTotalPages;
        response.TotalRecords = totalRecords;

        return response;
    }
}
