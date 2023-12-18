namespace Core.Shared.Paginations.TextSearches;

/// <summary>
/// This class describes a text search to be applied to the query.
/// A text search is the "contains" operator which only works on string.
/// We filter for rows where the column value (obtained fromm <see cref="ColumnName"/> contains the <see cref="FilterValue"/>
/// e.g. columnValue.Contains(<see cref="FilterValue"/>)?
/// </summary>
public class TextSearchParam
{
	/// <summary>
	/// The name of the entity property (or SQL table column name) to text search upon.
	/// </summary>
	public string ColumnName { get; set; } = string.Empty;

	/// <summary>
	/// The value to which the entity value will be search upon.
	/// </summary>
	public string FilterValue { get; set; } = string.Empty;
}