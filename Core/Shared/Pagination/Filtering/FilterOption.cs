namespace Core.Shared.Pagination.Filtering;

public static class FilterOptionMap
{
	private static readonly Dictionary<string, FilterOption> Map = new()
	{
		{ string.Empty, FilterOption.Nothing },
		{ "Contains", FilterOption.Contains },
		{ "Greater", FilterOption.IsGreaterThan },
		{ "GreaterEqual", FilterOption.IsGreaterThanOrEqualTo },
		{ "Less", FilterOption.IsLessThan },
		{ "LessEqual", FilterOption.IsLessThanOrEqualTo },
		{ "Equal", FilterOption.IsEqualTo },
		{ "NotEqual", FilterOption.IsNotEqualTo }
	};

	public static FilterOption Get(string key)
	{
		return Map[key];
	}
}

public enum FilterOption
{
	Nothing,
	Contains,
	IsGreaterThan,
	IsGreaterThanOrEqualTo,
	IsLessThan,
	IsLessThanOrEqualTo,
	IsEqualTo,
	IsNotEqualTo
}