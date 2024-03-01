namespace Core.Entities.StationCycles.Models.Structs;

/// <summary>
/// Lighter version a station cycle for the front.
/// </summary>
public struct DTOReducedStationCycle
{
	public int ID { get; set; }
	public string RID { get; set; }
	public int? AnodeSize { get; set; }
	public string AnodeType { get; set; }
	public DateTimeOffset? ShootingTS { get; set; }
}