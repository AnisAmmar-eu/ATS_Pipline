using System.Runtime.InteropServices;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
public struct InFurnaceStruct : IBaseADS<Packet>
{
	public RIDStruct CycleRID;
	public int Status;
	public TimestampStruct TS;
	public InFurnaceRWStruct MD;

	public Packet ToModel()
	{
		return new InFurnace(this);
	}
}

[StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
public struct InFurnaceRWStruct
{
	public RIDStruct AnnounceID;

	public int OriginID;
	public int PackPosition;
	public int PalletSide;
	public int PitNumber;
	public int PitSectionNumber;
	public int PitHeight;
	public int PitFTA;
	public int GreenConvPos;
	public TimestampStruct PitLoadTS;
}