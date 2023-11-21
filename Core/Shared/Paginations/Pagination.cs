using Core.Shared.Paginations.Filtering;
using Core.Shared.Paginations.Sorting;

namespace Core.Shared.Paginations;

public class Pagination
{
	public string[] Includes { get; set; } = { };
	public SortParam SortParam { get; set; } = new();
	public List<FilterParam> FilterParams { get; set; } = new();
}