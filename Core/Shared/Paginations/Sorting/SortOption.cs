namespace Core.Shared.Paginations.Sorting;

public static class SortOptionMap
{
	private static readonly Dictionary<string, SortOption> Map = new()
	{
		{ string.Empty, SortOption.None },
		{ "Ascending", SortOption.Ascending },
		{ "Descending", SortOption.Descending }
	};

	public static SortOption Get(string key)
	{
		return Map[key];
	}
}

public enum SortOption
{
	None,
	Ascending,
	Descending,
}