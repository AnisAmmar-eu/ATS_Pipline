using System.Linq.Expressions;
using System.Reflection;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Shared.Paginations.TextSearches;

public static class TextSearch
{
	/// <summary>
    /// Apply a text search to an <see cref="IQueryable{T}"/> source from its pagination.
    /// It chains every <see cref="TextSearchParam"/> in pagination with OR boolean operators.
    /// Text searches are then applied to the query.
	/// </summary>
    /// <param name="source">Query to search</param>
    /// <param name="pagination">Pagination parameters used to search</param>
    /// <typeparam name="T">BaseEntity from which rows will be searched upon</typeparam>
    /// <typeparam name="TDTO"></typeparam>
    /// <returns>A filtered query</returns>
	public static IQueryable<T> TextSearchFromPagination<T, TDTO>(this IQueryable<T> source, Pagination pagination)
		where T : class, IBaseEntity<T, TDTO>
		where TDTO : class, IDTO<T, TDTO>
	{
		return source.Where(TextSearchesToWhereClause<T>(pagination.TextSearchParams));
	}

	private static Expression<Func<T, bool>> TextSearchesToWhereClause<T>(IEnumerable<TextSearchParam>? textSearchParams)
	{
		if (textSearchParams is null)
			return _ => true;

		ParameterExpression param = Expression.Parameter(typeof(T));

		// Maps the filterParams into a list of Expression<Func<T, bool>> with the FilterToExpression list.
		List<BinaryExpression> textSearches
			= textSearchParams.Select(textSearchParam => TextSearchToExpression<T>(textSearchParam, param)).ToList();

		if (textSearches.Count == 0)
			return _ => true;

		// Aggregates all the expressions into one by combining their bodies and creating a new expression with the same
		// parameters than the first one.
		Expression expr = textSearches.Aggregate(Expression.OrElse);
		return Expression.Lambda<Func<T, bool>>(expr, param);
	}

	private static BinaryExpression TextSearchToExpression<T>(TextSearchParam textSearchParam, ParameterExpression param)
	{
		// Gets the property of the class from its column name.
		string[] names = textSearchParam.ColumnName.Split('.');
		PropertyInfo filterColumn = GetColumnProperty<T>(names);
		return Expression.Equal(
			Expression.Call(
				Expression.Call(
					Expression.Property(param, filterColumn.Name),
					Array.Find(filterColumn.PropertyType.GetMethods(), method => method.Name == "ToString")!),
				Array.Find(typeof(string).GetMethods(), method => method.Name == "Contains")!,
				Expression.Constant(textSearchParam.FilterValue, typeof(string))),
			Expression.Constant(true));
	}

	private static PropertyInfo GetColumnProperty<T>(string[] names)
	{
		PropertyInfo? propertyInfo = null;
		Type type = typeof(T);
		foreach (string name in names)
		{
			propertyInfo = type.GetProperty(
				name,
				BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
			if (propertyInfo is null)
				throw new InvalidDataException("FilterParam Column name is invalid.");

			type = propertyInfo.PropertyType;
		}

		if (propertyInfo is null)
			throw new InvalidDataException("FilterParam Column name is invalid.");

		return propertyInfo;
	}
}