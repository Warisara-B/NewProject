using System;
using System.Linq.Expressions;
using Plexus.Utility.ViewModel;

namespace Plexus.Utility.Extensions
{
    public static class IQueryableExtensions
    {
        public static PagedViewModel<T> GetPagedViewModel<T>(this IQueryable<T> input, int page, int pageSize) where T : class
        {
            if (input is null || page < 1)
            {
                return new PagedViewModel<T>
                {
                    TotalPage = 0,
                    Page = page,
                    PageSize = pageSize,
                    Items = Enumerable.Empty<T>()
                };
            }

            var totalItem = input.Count();
            var totalPage = Convert.ToInt32(Math.Ceiling(totalItem / (double)pageSize));

            var skipCount = (page - 1) * pageSize;

            var response = new PagedViewModel<T>
            {
                TotalPage = totalPage,
                Page = page,
                PageSize = pageSize,
                TotalItem = totalItem,
                Items = input.Skip(skipCount)
                             .Take(pageSize)
                             .ToList()
            };

            return response;
        }

        /// <summary>
        /// Reference: https://stackoverflow.com/questions/66681123/linq-method-which-dynamically-ordering-data-by-column-name-i-send
        /// </summary>
        /// <param name="input"></param>
        /// <param name="columnName"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> input, string columnName, SortingOrder? orderBy = SortingOrder.ASC)
        {
            ParameterExpression parameter = Expression.Parameter(input.ElementType, string.Empty);

            MemberExpression property = Expression.Property(parameter, columnName);

            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = orderBy.HasValue && orderBy == SortingOrder.DESC ? "OrderByDescending" : "OrderBy";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), 
                                                              methodName,
                                                              new Type[] { input.ElementType, property.Type },
                                                              input.Expression, Expression.Quote(lambda));

            return input.Provider.CreateQuery<T>(methodCallExpression);
        }
    }
}

