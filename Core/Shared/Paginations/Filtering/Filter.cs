using System.Linq.Expressions;
using System.Reflection;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;
using Core.Shared.Paginations.Sorting;

namespace Core.Shared.Paginations.Filtering;

public static class Filter
{
	public static IQueryable<T> FilterFromPagination<T, TDTO>(this IQueryable<T> source, Pagination pagination)
		where T : class, IBaseEntity<T, TDTO>
		where TDTO : class, IDTO<T, TDTO>
	{
		ParameterExpression param = Expression.Parameter(typeof(T));
		source = (pagination.SortParam.LastValue.Length == 0)
            ? source
			: source.Where(GetLastValueWhereClause<T>(pagination.SortParam, param));

		return source .Where(FiltersToWhereClause<T>(pagination.FilterParams, param));
	}

	private static Expression<Func<T, bool>> GetLastValueWhereClause<T>(SortParam sortParam, ParameterExpression param)
	{
		SortOption sortOption = SortOptionMap.Get(sortParam.SortOptionName);
		string[] names = sortParam.ColumnName.Split('.');
		PropertyInfo filterColumn = GetColumnProperty<T>(names);
		IComparable? lastValue = ParseComparable(filterColumn.PropertyType, sortParam.LastValue);
		if (lastValue is null)
			throw new ArgumentException($"Error happened during parsing of {nameof(lastValue)}");

		Expression left = GetPropertyExpression(param, names);
		Expression right = Expression.Constant(lastValue, lastValue.GetType());
		BinaryExpression comparison = (sortOption == SortOption.Ascending)
			? Expression.GreaterThan(left, right)
			: Expression.LessThan(left, right);
		return Expression.Lambda<Func<T, bool>>(comparison, param);
	}

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

	private static Expression FilterToExpression<T>(FilterParam filterParam, ParameterExpression param)
	{
		// Gets the property of the class from its column name.
		FilterOption filterOption = FilterOptionMap.Get(filterParam.FilterOptionName);
		string[] names = filterParam.ColumnName.Split('.');
		PropertyInfo filterColumn = GetColumnProperty<T>(names);
		if (filterOption == FilterOption.Nothing)
			return Expression.Constant(true);

		IComparable? refValue = ParseComparable(filterColumn.PropertyType, filterParam.FilterValue);
		if (refValue is null)
			throw new ArgumentException("Error happened during parsing of filterValue");

		return GetExpressionBody(
			filterOption,
			GetPropertyExpression(param, names),
			Expression.Constant(refValue, refValue.GetType()));
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

	private static Expression GetPropertyExpression(ParameterExpression param, string[] names)
	{
		Expression property = Expression.Property(param, names[0]);
		for (int i = 1; i < names.Length; ++i)
			property = Expression.Property(property, names[i]);

		return property;
	}

	private static IComparable? ParseComparable(Type type, string value)
	{
		if (type == typeof(string))
			return value;
		// Verifies if the type is parsable or not by using reflection.
		bool isParsable = type.GetInterfaces()
			.Any(c => c.IsGenericType && c.GetGenericTypeDefinition() == typeof(IParsable<>));
		if (!isParsable)
			throw new ArgumentException("Filter: Trying to parse a value which is not parsable.");

		// Then it gets the Parse method through reflection.
		// https://stackoverflow.com/questions/74501978/how-do-i-test-if-a-type-t-implements-iparsablet
		MethodInfo? parse = Array.Find(
			type.GetMethods(BindingFlags.Static | BindingFlags.Public),
			c =>
				c.Name == "Parse"
					&& c.GetParameters().Length == 2
					&& c.GetParameters()[0].ParameterType == typeof(string)
					&& c.GetParameters()[1].ParameterType == typeof(IFormatProvider));
		if (parse is null)
			throw new ArgumentException("Filter: Trying to parse a value which is not parsable.");

		// And it finally invokes it.
		return parse.Invoke(null, new object[] { value, null! }) as IComparable;
	}
}