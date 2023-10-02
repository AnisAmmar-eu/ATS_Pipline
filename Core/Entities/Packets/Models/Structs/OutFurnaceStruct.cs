using Core.Entities.Packets.Models.DB;
using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

public struct OutFurnaceStruct : IBaseADS<Packet, OutFurnaceStruct>
{
	public uint CycleStationRID;
	public uint TSUnpackPIT; // TSUnpackingOfPIT
	public uint TSCentralConveyor; // TSWhenAnodeInOutletCentralConveyor
	public uint FTAPickUp; // FTAPickUpFromPIT
	public Packet ToModel()
	{
		return new OutFurnace(this);
	}
}