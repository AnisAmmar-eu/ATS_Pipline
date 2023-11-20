using System.Linq.Expressions;
using Core.Shared.Models.DB.Kernel;

namespace Core.Shared.Pagination.Sorting;

public static class Sort
{
	public static IOrderedQueryable<T> SortFromPagination<T>(this IQueryable<T> source, Pagination pagination)
		where T: BaseEntity
	{
		SortParam sortParam = pagination.SortParam;
		SortOption sortOption = SortOptionMap.Get(sortParam.SortOptionName);
		ParameterExpression param = Expression.Parameter(typeof(T));
		return sortOption switch
		{
			SortOption.None => source.OrderByDescending(t => t.ID),
			SortOption.Ascending => source.OrderBy(GetOrderByExpression<T>(sortParam, param)),
			SortOption.Descending => source.OrderByDescending(GetOrderByExpression<T>(sortParam, param)),
			_ => throw new ArgumentOutOfRangeException()
		};
	}

	private static Expression<Func<T, object>> GetOrderByExpression<T>(SortParam sortParam, ParameterExpression param)
	{
		return Expression.Lambda<Func<T, object>>(
			Expression.Convert(Expression.Property(param, sortParam.ColumnName), typeof(object)), param);
	}
}