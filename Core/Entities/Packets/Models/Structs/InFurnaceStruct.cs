using System.Runtime.InteropServices;
using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 0, CharSet = CharSet.Ansi)]
public struct InFurnaceStruct : IBaseADS<Packet, InFurnaceStruct>
{
	public RIDStruct StationCycleRID;
	public RIDStruct AnnounceID;

	public ushort OriginID;
	public ushort PackPosition; // AnodePositionInThePackOf7
	public ushort PalletSide;
	public ushort PitNumber;
	public ushort PitSectionNumber;
	public ushort PitHeight;

	public ushort FTAPlace;
	public ushort FTASuck;

	public ushort GreenConvPos;
	public ushort BakedConvPos;
	public TimestampStruct PitLoadTS;

	public Packet ToModel()
	{
		return new InFurnace(this);
	}
}