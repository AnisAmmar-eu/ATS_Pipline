using System.Linq.Expressions;
using System.Reflection;

namespace Core.Shared.Pagination.Filtering;

public static class Filter<T>
{
	public static Expression<Func<T, bool>> FiltersToWhereClause(IEnumerable<FilterParam>? filterParams)
	{
		if (filterParams == null)
			return t => true;

		ParameterExpression param = Expression.Parameter(typeof(T));

		// Maps the filterParams into a list of Expression<Func<T, bool>> with the FilterToExpression list.
		List<Expression> filters = filterParams.Select(filterParam => FilterToExpression(filterParam, param)).ToList();

		if (!filters.Any())
			return t => true;

		// Aggregates all the expressions into one by combining their bodies and creating a new expression with the same
		// parameters than the first one.
		Expression expr = filters.Aggregate(Expression.AndAlso);
		return Expression.Lambda<Func<T, bool>>(expr, param);
	}

	private static Expression FilterToExpression(FilterParam filterParam, ParameterExpression param)
	{
		// Gets the property of the class from its column name.
		FilterOption filterOption = FilterOptionMap.Get(filterParam.FilterOptionName);
		PropertyInfo? filterColumn = typeof(T).GetProperty(filterParam.ColumnName,
			BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
		if (filterColumn == null)
			throw new InvalidDataException("FilterParam Column name is invalid.");
		switch (filterOption)
		{
			case FilterOption.Nothing:
				return Expression.Constant(true);
			case FilterOption.Contains:
				return Expression.Equal(
					Expression.Call(Expression.Property(param, filterParam.ColumnName),
						typeof(string).GetMethods().FirstOrDefault(method => method.Name == "Contains")!,
						Expression.Constant(filterParam.FilterValue, typeof(string))),
					Expression.Constant(true));
			case FilterOption.IsGreaterThan:
			case FilterOption.IsGreaterThanOrEqualTo:
			case FilterOption.IsLessThan:
			case FilterOption.IsLessThanOrEqualTo:
			case FilterOption.IsEqualTo:
			case FilterOption.IsNotEqualTo:
				IComparable? refValue = ParseComparable(filterColumn.PropertyType, filterParam.FilterValue);
				if (refValue == null)
					throw new ArgumentException("Error happened during parsing of filterValue");
				return GetExpressionBody(filterOption,
					Expression.Property(param, filterParam.ColumnName),
					Expression.Constant(refValue, refValue.GetType()));
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private static BinaryExpression GetExpressionBody(FilterOption filterOption, Expression left, Expression right)
	{
		return filterOption switch
		{
			FilterOption.IsGreaterThan => Expression.GreaterThan(left, right),
			FilterOption.IsGreaterThanOrEqualTo => Expression.GreaterThanOrEqual(left, right),
			FilterOption.IsLessThan => Expression.LessThan(left, right),
			FilterOption.IsLessThanOrEqualTo => Expression.LessThanOrEqual(left, right),
			FilterOption.IsEqualTo => Expression.Equal(left, right),
			FilterOption.IsNotEqualTo => Expression.NotEqual(left, right),
			_ => throw new ArgumentOutOfRangeException(nameof(filterOption), filterOption, null)
		};
	}

	private static IComparable? ParseComparable(Type type, string value)
	{
		// Verifies if the type is parsable or not by using reflection.
		bool isParsable = type.GetInterfaces()
			.Any(c => c.IsGenericType && c.GetGenericTypeDefinition() == typeof(IParsable<>));
		if (!isParsable)
			throw new ArgumentException("Filter: Trying to parse a value which is not parsable.");

		// Then it gets the Parse method through reflection still.
		// https://stackoverflow.com/questions/74501978/how-do-i-test-if-a-type-t-implements-iparsablet
		MethodInfo? parse = type.GetMethods(BindingFlags.Static | BindingFlags.Public).FirstOrDefault(c =>
			c.Name == "Parse" && c.GetParameters().Length == 2 &&
			c.GetParameters()[0].ParameterType == typeof(string) &&
			c.GetParameters()[1].ParameterType == typeof(IFormatProvider));
		if (parse == null)
			throw new ArgumentException("Filter: Trying to parse a value which is not parsable.");

		// And it finally invokes it.
		return parse.Invoke(null, new object[] { value, null! }) as IComparable;
	}
}