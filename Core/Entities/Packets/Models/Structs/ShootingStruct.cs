using Core.Entities.Packets.Models.DB.Shootings;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

public struct ShootingStruct : IBaseADS<Shooting, ShootingStruct> // TODO Not in list variables
{
	public uint CycleStationRID;
	public uint AnodeIDKey;
	public string GlobalStationStatus; // TODO Dictionary?
	public uint ProcedurePerformance; // TODO int?
	public string LedStatus; // TODO Dictionary?
	public uint ShootingTS;
	public Shooting ToModel()
	{
		return new Shooting(this);
	}
}