namespace Core.Shared.Pagination.Filtering;

public class FilterParam
{
	public string ColumnName { get; set; } = string.Empty;
	public string FilterValue { get; set; } = string.Empty;
	public FilterOption FilterOption { get; set; } = FilterOption.Nothing;
}