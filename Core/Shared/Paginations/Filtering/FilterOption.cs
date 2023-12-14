namespace Core.Shared.Paginations.Filtering;

public static class FilterOptionMap
{
	private static readonly Dictionary<string, FilterOption> Map = new() {
		{ string.Empty, FilterOption.Nothing },
		{ "Greater", FilterOption.IsGreaterThan },
		{ "GreaterEqual", FilterOption.IsGreaterThanOrEqualTo },
		{ "Less", FilterOption.IsLessThan },
		{ "LessEqual", FilterOption.IsLessThanOrEqualTo },
		{ "Equal", FilterOption.IsEqualTo },
		{ "NotEqual", FilterOption.IsNotEqualTo },
		{ "IsType", FilterOption.IsType },
	};

	public static FilterOption Get(string key)
	{
		return Map[key];
	}
}

public enum FilterOption
{
	Nothing = 0,
	IsGreaterThan = 1,
	IsGreaterThanOrEqualTo = 2,
	IsLessThan = 3,
	IsLessThanOrEqualTo = 4,
	IsEqualTo = 5,
	IsNotEqualTo = 6,
	IsType = 7,
}