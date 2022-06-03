
using DicaNinja.API.Contexts;
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace DicaNinja.API.Providers;

public class CategoryProvider : ICategoryProvider
{
    public CategoryProvider(BaseContext context)
    {
        this.Context = context;
    }

    private BaseContext Context { get; }

    public async Task<List<Category>> GetByBook(Guid bookId)
    {
        return await this.Context.Categories.Where(category => category.Books.Any(book => book.Id == bookId)).ToListAsync();
    }

    public async Task<Category?> GetOrCreate(string categoryName)
    {
        var category = await this.Context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName);

        if (category is not null)
        {
            return category;
        }

        category = new Category(categoryName);

        this.Context.Categories.Add(category);

        return category;
    }
}
