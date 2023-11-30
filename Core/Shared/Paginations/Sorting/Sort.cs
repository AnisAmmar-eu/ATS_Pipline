using System.Linq.Expressions;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Shared.Paginations.Sorting;

public static class Sort
{
	public static IOrderedQueryable<T> SortFromPagination<T, TDTO>(this IQueryable<T> source, Pagination pagination)
		where T : class, IBaseEntity<T, TDTO>
		where TDTO : class, IDTO<T, TDTO>
	{
		SortParam? sortParam = pagination.SortParam;
		if (sortParam is null)
			return source.OrderByDescending(t => t.ID);

		SortOption sortOption = SortOptionMap.Get(sortParam.SortOptionName);
		ParameterExpression param = Expression.Parameter(typeof(T));
		return sortOption switch {
			SortOption.None => source.OrderByDescending(t => t.ID),
			SortOption.Ascending => source.OrderBy(GetOrderByExpression<T>(sortParam, param)),
			SortOption.Descending => source.OrderByDescending(GetOrderByExpression<T>(sortParam, param)),
			_ => throw new ArgumentOutOfRangeException(nameof(pagination)),
		};
	}

	private static Expression<Func<T, object>> GetOrderByExpression<T>(SortParam sortParam, ParameterExpression param)
	{
		return Expression.Lambda<Func<T, object>>(
			Expression.Convert(Expression.Property(param, sortParam.ColumnName), typeof(object)), param);
	}
}