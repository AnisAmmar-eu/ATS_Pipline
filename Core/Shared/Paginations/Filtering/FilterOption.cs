namespace Core.Shared.Paginations.Filtering;

public static class FilterOptionMap
{
	private static readonly Dictionary<string, FilterOption> Map = new() {
		{ string.Empty, FilterOption.Nothing },
		{ "Contains", FilterOption.Contains },
		{ "Greater", FilterOption.IsGreaterThan },
		{ "GreaterEqual", FilterOption.IsGreaterThanOrEqualTo },
		{ "Less", FilterOption.IsLessThan },
		{ "LessEqual", FilterOption.IsLessThanOrEqualTo },
		{ "Equal", FilterOption.IsEqualTo },
		{ "NotEqual", FilterOption.IsNotEqualTo },
	};

	public static FilterOption Get(string key)
	{
		return Map[key];
	}
}

public enum FilterOption
{
	Nothing = 0,
	Contains = 1,
	IsGreaterThan = 2,
	IsGreaterThanOrEqualTo = 3,
	IsLessThan = 4,
	IsLessThanOrEqualTo = 5,
	IsEqualTo = 6,
	IsNotEqualTo = 7,
}