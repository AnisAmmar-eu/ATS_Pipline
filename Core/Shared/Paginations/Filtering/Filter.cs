using System.Linq.Expressions;
using System.Reflection;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Paginations.Sorting;

namespace Core.Shared.Paginations.Filtering;

public static class Filter
{
	/// <summary>
	/// Apply filters to an <see cref="IQueryable{T}"/> source from its pagination.
	/// The last value from the <see cref="SortParam"/> is first used to remove previously queried rows. If none is given, no rows are removed.
	/// Then, it chains every <see cref="FilterParam"/> in pagination with AND boolean operators.
	/// Filters are then applied to the query.
	/// </summary>
	/// <param name="source">Query to filter</param>
	/// <param name="pagination">Pagination parameters used to filter</param>
	/// <typeparam name="T">BaseEntity from which rows will be filtered</typeparam>
	/// <typeparam name="TDTO"></typeparam>
	/// <returns>A filtered query</returns>
	public static IQueryable<T> FilterFromPagination<T, TDTO>(this IQueryable<T> source, Pagination pagination)
		where T : class, IBaseEntity<T, TDTO>
		where TDTO : class, IDTO<T, TDTO>
	{
		ParameterExpression param = Expression.Parameter(typeof(T));
		source = (pagination.SortParam.LastValue.Length == 0)
			? source
			: source.Where(GetLastValueWhereClause<T>(pagination.SortParam, param));

		return source.Where(FiltersToWhereClause<T>(pagination.FilterParams, param));
	}

	/// <summary>
	/// Returns the expression filter of LastValue. The LastValue comparison is applied on the column on which the result will be sorted.
	/// If there is no column or sort method specified, it defaults to ID Descending.
	/// Otherwise, remove every row which are inferior (resp. superior) to it if in ascending (resp. descending) sort.
	/// </summary>
	/// <param name="sortParam"></param>
	/// <param name="param"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	private static Expression<Func<T, bool>> GetLastValueWhereClause<T>(SortParam sortParam, ParameterExpression param)
	{
		SortOption sortOption = SortOptionMap.Get(sortParam.SortOptionName);
		string[] names = sortParam.ColumnName.Split('.');
		PropertyInfo filterColumn = GetColumnProperty<T>(names);
		IComparable? lastValue = ParseAsComparable(filterColumn.PropertyType, sortParam.LastValue);
		if (lastValue is null)
			throw new ArgumentException($"Error happened during parsing of {nameof(lastValue)}");

		Expression left = GetExpressionProperty(param, names);
		Expression right = Expression.Constant(lastValue, lastValue.GetType());
		BinaryExpression comparison = (sortOption == SortOption.Ascending)
			? Expression.GreaterThan(left, right)
			: Expression.LessThan(left, right);
		return Expression.Lambda<Func<T, bool>>(comparison, param);
	}

	/// <summary>
	/// Chains all filterParams into a single Expression.
	/// Filters are chained with the AND boolean operator.
	/// </summary>
	/// <param name="filterParams"></param>
	/// <param name="param"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	private static Expression<Func<T, bool>> FiltersToWhereClause<T>(
		IEnumerable<FilterParam>? filterParams,
		ParameterExpression param)
	{
		if (filterParams is null)
			return _ => true;

		// Maps the filterParams into a list of Expression<Func<T, bool>> with the FilterToExpression list.
		List<Expression> filters
			= filterParams.Select(filterParam => FilterToExpression<T>(filterParam, param)).ToList();

		if (filters.Count == 0)
			return _ => true;

		// Aggregates all the expressions into one by combining their bodies and creating a new expression with the same
		// parameters than the first one.
		Expression expr = filters.Aggregate(Expression.AndAlso);
		return Expression.Lambda<Func<T, bool>>(expr, param);
	}

	/// <summary>
	/// Converts a filterParam into a compilable LINQ to Entities compatible expression so it can be translated to SQL.
	/// </summary>
	/// <param name="filterParam"></param>
	/// <param name="param"></param>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	private static Expression FilterToExpression<T>(FilterParam filterParam, ParameterExpression param)
	{
		// Gets the property of the class from its column name.
		FilterOption filterOption = FilterOptionMap.Get(filterParam.FilterOptionName);
		if (filterOption == FilterOption.IsType)
		{
			// Gets all possible types in the Assembly (running instance) and find the one with the same name.
			Type? type = Assembly.GetAssembly(typeof(T))?.GetTypes().ToList()
				.Find(t => t.Name == filterParam.FilterValue[0]);
			return (type is null) ? Expression.Constant(false) : Expression.TypeIs(param, type);
		}

		string[] names = filterParam.ColumnName.Split('.');
		PropertyInfo filterColumn = GetColumnProperty<T>(names);
		if (filterOption == FilterOption.Nothing)
			return Expression.Constant(true);

		List<Expression> expressions = filterParam.FilterValue
			.ConvertAll(value => {
				IComparable refValue = ParseAsComparable(filterColumn.PropertyType, value)
					?? throw new ArgumentException("Error happened during parsing of filterValue");
				return (Expression)GetExpressionBody(
					filterOption,
					GetExpressionProperty(param, names),
					Expression.Constant(refValue, refValue.GetType()));
			});

		return (expressions.Count == 0) ? Expression.Constant(true) : expressions.Aggregate(Expression.OrElse);
	}

	private static BinaryExpression GetExpressionBody(FilterOption filterOption, Expression left, Expression right)
	{
		return filterOption switch {
			FilterOption.IsGreaterThan => Expression.GreaterThan(left, right),
			FilterOption.IsGreaterThanOrEqualTo => Expression.GreaterThanOrEqual(left, right),
			FilterOption.IsLessThan => Expression.LessThan(left, right),
			FilterOption.IsLessThanOrEqualTo => Expression.LessThanOrEqual(left, right),
			FilterOption.IsEqualTo => Expression.Equal(left, right),
			FilterOption.IsNotEqualTo => Expression.NotEqual(left, right),
			_ => throw new ArgumentOutOfRangeException(nameof(filterOption), filterOption, null),
		};
	}

	/// <summary>
	///	Returns the PropertyInfo queried by its name & path to it. If given [ "Bar", "ID" ],
	/// it will return the PropertyInfo of ID if there's an object with this column in "Bar".
	/// eg:
	/// <code>
	/// public class Foo
	/// {
	///		public BarClass Bar { get; set; }
	///		public int Value { get; set; }
	/// }
	/// public class BarClass
	/// {
	///		public int ID { get; set; }
	/// }
	/// </code>
	///	ID column is accessed with [ "Bar", "ID" ] from T = Foo.
	/// Value column is accessed with [ "Value" ] from T = Foo.
	/// </summary>
	/// <param name="names">An array of strings for nested parameters</param>
	/// <typeparam name="T">Type from which column is queried</typeparam>
	/// <returns>The property info of the (possibly nested) queried column</returns>
	/// <exception cref="InvalidDataException">Thrown if no PropertyInfo is found due to invalid name</exception>
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

		return propertyInfo ?? throw new InvalidDataException("FilterParam Column name is invalid.");
	}

	/// <summary>
	/// Similar to <see cref="GetColumnProperty{T}"/>,
	/// except it returns an Expression accessing this (potentially nested) property instead of its PropertyInfo
	/// </summary>
	/// <param name="param">Parameter expression on which to access property</param>
	/// <param name="names">An array of strings for nested parameters</param>
	/// <returns>An expression accessing this property from param</returns>
	private static Expression GetExpressionProperty(ParameterExpression param, string[] names)
	{
		Expression property = Expression.Property(param, names[0]);
		for (int i = 1; i < names.Length; ++i)
			property = Expression.Property(property, names[i]);

		return property;
	}

	/// <summary>
	/// Will parse a string into an IComparable object if its <paramref name="type"/> implements IParsable and IComparable.
	/// This function uses System.Reflection.
	/// </summary>
	/// <param name="type">Type of the unparsed value</param>
	/// <param name="value">Value to be parsed as comparable</param>
	/// <returns>The parsed value as an IComparable</returns>
	/// <exception cref="ArgumentException">Thrown if type is either not Comparable or not Parsable</exception>
	private static IComparable? ParseAsComparable(Type type, string value)
	{
		if (type == typeof(string))
			return value;

		if (type.GetInterfaces().All(c => c != typeof(IComparable)))
			throw new ArgumentException($"Filter: {type} is not parsable as IComparable.");

		// Verifies if the type is parsable or not by using reflection.
		if (!type.GetInterfaces().Any(c => c.IsGenericType && c.GetGenericTypeDefinition() == typeof(IParsable<>)))
			throw new ArgumentException("Filter: Trying to parse a value which is not parsable.");

		// Then it gets the Parse method through reflection.
		// https://stackoverflow.com/questions/74501978/how-do-i-test-if-a-type-t-implements-iparsablet
		MethodInfo? parse = Array.Find(
			type.GetMethods(BindingFlags.Static | BindingFlags.Public),
			c =>
				c.Name == "Parse"
					&& c.GetParameters().Length == 1
					&& c.GetParameters()[0].ParameterType == typeof(string))
			?? throw new ArgumentException(
				"Filter: Trying to parse a value which is not parsable.");

		// And it finally invokes it.
		return parse.Invoke(null, [value]) as IComparable;
	}
}