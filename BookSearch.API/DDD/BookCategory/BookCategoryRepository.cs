using BookSearch.API.Contexts;

using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.DDD.BookCategory;

public class BookCategoryRepository : IBookCategoryRepository
{
    public BookCategoryRepository(DefaultContext context)
    {
        Context = context;
    }

    private DefaultContext Context { get; }

    public async Task<BookCategory?> GetOrCreate(string categoryName)
    {
        var category = await Context.BookCategories.FirstOrDefaultAsync(c => c.Name == categoryName);

        if (category is not null)
        {
            return category;
        }

        category = new BookCategory(categoryName);

        Context.BookCategories.Add(category);

        return category;
    }
}
