namespace Core.Entities.Packets.Models.Structs;

public struct ShootingStruct // TODO Not in list variables
{
	public uint CycleStationRID;
	public uint AnodeIDKey;
	public string GlobalStationStatus; // TODO Dictionary?
	public uint ProcedurePerformance; // TODO int?
	public string LedStatus; // TODO Dictionary?
	public uint ShootingTS;
}