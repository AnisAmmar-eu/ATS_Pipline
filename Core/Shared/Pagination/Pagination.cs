using Core.Shared.Pagination.Filtering;
using Core.Shared.Pagination.Sorting;

namespace Core.Shared.Pagination;

public class Pagination
{
	public SortParam SortParam { get; set; } = new();
	public List<FilterParam> FilterParams { get; set; } = new();
}