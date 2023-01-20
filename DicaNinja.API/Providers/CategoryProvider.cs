
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

    public async Task<IEnumerable<Category>> GetByBookAsync(Guid bookId, CancellationToken cancellation)
    {
        return await Context.Categories.Where(category => category.Books.Any(book => book.Id == bookId)).ToListAsync(cancellation).ConfigureAwait(false);
    }

    public async Task<Category?> GetByName(string name, CancellationToken cancellation)
    {
        return await Context.Categories.FirstOrDefaultAsync(category => category.Name == name, cancellation);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellation)
    {
        return await Context.Categories.CountAsync(cancellation).ConfigureAwait(false);
    }

    public async Task<Category?> GetOrCreateAsync(string categoryName, CancellationToken cancellation)
    {
        var category = await Context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName, cancellation).ConfigureAwait(false);

        if (category is not null)
        {
            return category;
        }

        category = new Category(categoryName);

        await Context.Categories.AddAsync(category, cancellation).ConfigureAwait(false);
        await Context.SaveChangesAsync(cancellation).ConfigureAwait(false);

        return category;
    }
}
