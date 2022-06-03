namespace BookSearch.API.Helpers;

public static class PaginationHelper
{
    public static PagedResponse<List<T>> CreatePagedResponse<T>(List<T> pagedData,
        QueryParameters validQueryString, int totalRecords)
    {
        var response = new PagedResponse<List<T>>(pagedData, validQueryString.Page, validQueryString.PerPage);
        var totalPages = totalRecords / (double)validQueryString.Page;
        var roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

        response.TotalPages = roundedTotalPages;
        response.TotalRecords = totalRecords;

        return response;
    }
}