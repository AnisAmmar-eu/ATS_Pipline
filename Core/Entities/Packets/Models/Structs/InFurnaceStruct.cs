namespace Core.Entities.Packets.Models.Structs;

public struct InFurnaceStruct
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
}