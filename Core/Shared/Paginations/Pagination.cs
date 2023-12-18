using Core.Entities.Alarms.AlarmsC.Models.DB;
using Core.Entities.Alarms.AlarmsLog.Models.DB;
using Core.Shared.Paginations.Filtering;
using Core.Shared.Paginations.Sorting;
using Core.Shared.Paginations.TextSearches;

namespace Core.Shared.Paginations;

/// <summary>
///	This class is used as an argument for paginated request along with filtering, text search and sorting.
/// </summary>
public class Pagination
{
	/// <summary>
	/// All foreign classes which should be included in the query. Presence of mandatory relations is NOT assured by the program.
	/// e.g. <see cref="AlarmLog"/> foreign relation with <see cref="AlarmC"/> is mandatory and should be given as <c>[ "Alarm" ]</c>
	/// as it is the name of the property.
	/// </summary>
	public string[] Includes { get; set; } = Array.Empty<string>();

	/// <summary>
	/// A list of <see cref="FilterParam"/> to apply to the query. Filter params are the first ones to be applied.
	/// Filtering also handles the <see cref="SortParam.LastValue"/>-based pagination.
	/// <seealso cref="Filter"/>
	/// </summary>
    public List<FilterParam> FilterParams { get; set; } = [];

	/// <summary>
	/// A list of <see cref="TextSearchParam"/> to apply to the query. Text search params are the second ones to be applied.
	/// <seealso cref="TextSearch"/>
	/// </summary>
    public List<TextSearchParam> TextSearchParams { get; set; } = [];

	/// <summary>
	/// A <see cref="SortParam"/> to apply to the query. Sorting is the third and last one to be applied.
	/// <seealso cref="Sort"/>
	/// </summary>
    public SortParam SortParam { get; set; } = new();
}