using System.Runtime.InteropServices;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
public struct OutFurnaceStruct : IBaseADS<Packet>
{
	public RIDStruct StationCycleRID;
	public RIDStruct AnnounceID;

	public ushort FTAPickUp;
	public TimestampStruct PickUpTS;
	public TimestampStruct DepositTS;
	public ushort InvalidPacket;

	public Packet ToModel()
	{
		return new OutFurnace(this);
	}
}