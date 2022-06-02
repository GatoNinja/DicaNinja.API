using BookSearch.API.Contexts;
using BookSearch.API.Models;
using BookSearch.API.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookSearch.API.Repository;

public class CategoryRepository : ICategoryRepository
{
    public CategoryRepository(DefaultContext context)
    {
        Context = context;
    }

    private DefaultContext Context { get; }

    public async Task<List<Category>> GetByBook(Guid bookId)
    {
        return await Context.Categories.Where(category => category.Books.Any(book => book.Id == bookId)).ToListAsync();
    }

    public async Task<Category?> GetOrCreate(string categoryName)
    {
        var category = await Context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);

        if (category is not null)
        {
            return category;
        }

        category = new Category(categoryName);

        Context.Categories.Add(category);

        return category;
    }
}
