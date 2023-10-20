using System.Runtime.InteropServices;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Shootings;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
public struct ShootingStruct : IBaseADS<Packet, ShootingStruct>
{
	public RIDStruct StationCycleRID;
	public ushort AnodeIDKey;
	public string GlobalStationStatus; // TODO Dictionary?
	public ushort ProcedurePerformance; // TODO int?
	public string LedStatus; // TODO Dictionary?
	public TimestampStruct ShootingTS;

	public Packet ToModel()
	{
		return new Shooting(this);
	}
}