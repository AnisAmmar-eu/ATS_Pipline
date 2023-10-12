using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

public struct InFurnaceStruct : IBaseADS<Packet, InFurnaceStruct>
{
	public RIDStruct StationCycleRID;
	public uint AnnounceID;
	
	public uint IdOrigin; // TODO OriginID?
	public uint PackPosition; // AnodePositionInThePackOf7
	public uint PalletSide;
	public uint PitNumber;
	public uint PitSectionNumber;
	public uint PitHeight;
	
	public uint FTAPlace;
	public uint FTASuck;
	
	public uint GreenConvPos;
	public uint BakedConvPos;
	public TimestampStruct PitLoadTS;

	public Packet ToModel()
	{
		return new InFurnace(this);
	}
}