using System.Linq.Expressions;
using System.Reflection;

namespace BookSearch.API.Helpers
{
    public static class QueryableExtension
    {
        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string? orderByProperty,
            EnumOrderBy orderBy = EnumOrderBy.Ascending)
        {

            if (orderByProperty is null)
            {
                return source;
            }

            var command = orderBy == EnumOrderBy.Descending ? "OrderByDescending" : "OrderBy";
            var type = typeof(TEntity);
            var property = type.GetProperty(orderByProperty, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (property is null)
            {
                Console.WriteLine($"A propriedade {property} não existe na entidade");

                return source;
            }

            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new[] { type, property.PropertyType },
                source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }
    }
}