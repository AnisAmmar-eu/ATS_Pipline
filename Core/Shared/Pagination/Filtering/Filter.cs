using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Shared.Pagination.Filtering;

public class Filter<T>
{
	/*
	public static Expression<Func<T, bool>> FiltersToWhereClause(IEnumerable<FilterParam> filterParams)
	{
		List<Expression<Func<T, bool>>> filters = new();
		// TODO
		return filters.Aggregate((expr1, expr2) =>
		{
			BinaryExpression body = Expression.AndAlso(expr1.Body, expr2.Body);
			return Expression.Lambda<Func<T, bool>>(body, expr1.Parameters[0]);
		});
	}

	private static Expression<Func<T, bool>> FilterToExpression(FilterParam filterParam)
	{
		PropertyInfo? filterColumn = typeof(T).GetProperty(filterParam.ColumnName,
			BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
		if (filterColumn == null)
			throw new InvalidDataException("FilterParam Column name is invalid.");
		switch (filterParam.FilterOption)
		{
			case FilterOption.Nothing:
				return t => true;
			case FilterOption.Contains:
				return t => filterColumn.GetValue(t, null) != null && filterColumn.GetValue(t, null)!.ToString()!
					.ToLower().Contains(filterParam.FilterValue.ToLower());
			case FilterOption.IsGreaterThan:
				return t =>
				{
					object? value = filterColumn.GetValue(t, null);
					if (value == null || value is not IComparable comparable || filterParam.FilterValue is not IParsable parsable)
						return false;
				}	
		}
	}
	*/
}