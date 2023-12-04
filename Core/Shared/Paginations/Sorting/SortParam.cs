namespace Core.Shared.Paginations.Sorting;

/// <summary>
/// A single sortParam is needed per pagination. If LastValue is empty it defaults to the first values,
/// if ColumnName or SortOptionName is empty, it defaults to the default descending ID orderBy.
/// </summary>
public class SortParam
{
	public string LastValue { get; set; } = string.Empty;
	public string ColumnName { get; set; } = string.Empty;
	public string SortOptionName { get; set; } = string.Empty;
}