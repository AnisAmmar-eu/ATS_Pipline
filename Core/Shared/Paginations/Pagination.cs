using Core.Shared.Paginations.Filtering;
using Core.Shared.Paginations.Sorting;
using Core.Shared.Paginations.TextSearches;

namespace Core.Shared.Paginations;

/// <summary>
///	Pagination first filters with AND, then text searches with OR and finally orderBy with SortParam
/// </summary>
public class Pagination
{
	public string[] Includes { get; set; } = Array.Empty<string>();
    public List<FilterParam> FilterParams { get; set; } = [];
    public List<TextSearchParam> TextSearchParams { get; set; } = [];
    public SortParam SortParam { get; set; } = new();
}