
using DicaNinja.API.Contexts;
using DicaNinja.API.Models;
using DicaNinja.API.Providers.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace DicaNinja.API.Providers;

public class CategoryProvider : ICategoryProvider
{
    public CategoryProvider(BaseContext context)
    {
        Context = context;
    }

    private BaseContext Context { get; }

    public async Task<List<Category>> GetByBookAsync(Guid bookId)
    {
        return await Context.Categories.Where(category => category.Books.Any(book => book.Id == bookId)).ToListAsync();
    }

    public async Task<int> GetCountAsync()
    {
        return await Context.Categories.CountAsync();
    }

    public async Task<Category?> GetOrCreateAsync(string categoryName)
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
