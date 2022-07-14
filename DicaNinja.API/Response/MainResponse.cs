using DicaNinja.API.Helpers;

namespace DicaNinja.API.Response;

public class MainResponse
{
    public PagedResponse<IEnumerable<BookResponse>> books { get; }
    public int totalBooks { get; }
    public int totalAuthors { get; }
    public int totalCategories { get; }
    public int totalUsers { get; }
    public MainResponse(PagedResponse<IEnumerable<BookResponse>> books, int totalBooks, int totalAuthors, int totalCategories, int totalUsers)
    {
        this.books = books;
        this.totalBooks = totalBooks;
        this.totalAuthors = totalAuthors;
        this.totalCategories = totalCategories;
        this.totalUsers = totalUsers;
    }
}
