
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

    public async Task<IEnumerable<Category>> GetByBookAsync(Guid bookId, CancellationToken cancellationToken)
    {
        return await Context.Categories.Where(category => category.Books.Any(book => book.Id == bookId)).ToListAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken)
    {
        return await Context.Categories.CountAsync(cancellationToken).ConfigureAwait(false);
    }

    public async Task<Category?> GetOrCreateAsync(string categoryName, CancellationToken cancellationToken)
    {
        var category = await Context.Categories.FirstOrDefaultAsync(c => c.Name == categoryName, cancellationToken).ConfigureAwait(false);

        if (category is not null)
        {
            return category;
        }

        category = new Category(categoryName);

        await Context.Categories.AddAsync(category, cancellationToken).ConfigureAwait(false);
        await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return category;
    }
}
