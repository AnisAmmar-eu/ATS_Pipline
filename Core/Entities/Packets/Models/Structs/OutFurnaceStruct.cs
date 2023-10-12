using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

public struct OutFurnaceStruct : IBaseADS<Packet, OutFurnaceStruct>
{
	public RIDStruct StationCycleRID;
	public uint AnnounceID;

	public uint FTAPickUp;
	public TimestampStruct TSPickUp;
	public TimestampStruct TSDeposit;
	public uint InvalidPacket;

	public Packet ToModel()
	{
		return new OutFurnace(this);
	}
}