namespace Core.Shared.Pagination.Filtering;

public class FilterParam
{
	public string ColumnName { get; set; } = string.Empty;
	public string FilterValue { get; set; } = string.Empty;
	public string FilterOptionName { get; set; } = string.Empty;
}