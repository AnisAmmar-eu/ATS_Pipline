namespace Core.Entities.Packets.Models.Structs;

public struct OutFurnace
{
	public uint CycleStationRID;
	public uint TSUnpackPIT; // TSUnpackingOfPIT
	public uint TSCentralConveyor; // TSWhenAnodeInOutletCentralConveyor
	public uint FTAPickUp; // FTAPickUpFromPIT
}