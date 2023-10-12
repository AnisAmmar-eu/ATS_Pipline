using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

public struct ShootingStruct : IBaseADS<Packet, ShootingStruct> // TODO Not in list variables
{
	public RIDStruct StationCycleRID;
	public uint AnodeIDKey;
	public string GlobalStationStatus; // TODO Dictionary?
	public uint ProcedurePerformance; // TODO int?
	public string LedStatus; // TODO Dictionary?
	public uint ShootingTS;

	public Packet ToModel()
	{
		return new Shooting(this);
	}
}