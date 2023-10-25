namespace Core.Entities.StationCycles.Models.Structs;

public struct ReducedStationCycle
{
	public int ID { get; set; }
	public string RID { get; set; }
	public int? AnodeSize { get; set; }
	public string AnodeType { get; set; }
	public DateTimeOffset? ShootingTS { get; set; }
}