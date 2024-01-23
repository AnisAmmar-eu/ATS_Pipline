using System.Runtime.InteropServices;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
public struct OutFurnaceStruct : IBaseADS<Packet>
{
	public RIDStruct CycleRID;
	public int Status;
	public TimestampStruct TS;
	public OutFurnaceRWStruct MD;

	public Packet ToModel()
	{
		return new OutFurnace(this);
	}
}

[StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
public struct OutFurnaceRWStruct
{
	public RIDStruct AnnounceID;
	public int PitFTA;
	public TimestampStruct PitPickup;
	public TimestampStruct PitDeposit;
	public int InvalidPacket;
	public int BackedConvPos;
}