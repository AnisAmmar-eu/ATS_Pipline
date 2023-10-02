using Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

public struct InFurnaceStruct : IBaseADS<InFurnace, InFurnaceStruct>
{
	public uint CycleStationRID;
	public uint OriginID;
	public uint AnodePosition; // AnodePositionInThePackOf7
	public uint PalletSide;
	public uint PITNumber;
	public uint PITSectionNumber;
	public uint PITHeight;
	public uint FTAinPIT;
	public uint GreenPosition;
	public uint BakedPosition;
	public uint FTASuckPit;
	public uint TSLoad;
	public InFurnace ToModel()
	{
		return new InFurnace(this);
	}
}