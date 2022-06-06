using DicaNinja.API.Helpers;

namespace DicaNinja.API.Response;

public record MainResponse(PagedResponse<IEnumerable<BookResponse>> books, int totalBooks, int totalAuthors, int totalCategories, int totalUsers);
