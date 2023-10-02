using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.Structs;

public struct OutFurnaceStruct : IBaseADS<OutFurnace, OutFurnaceStruct>
{
	public uint CycleStationRID;
	public uint TSUnpackPIT; // TSUnpackingOfPIT
	public uint TSCentralConveyor; // TSWhenAnodeInOutletCentralConveyor
	public uint FTAPickUp; // FTAPickUpFromPIT
	public OutFurnace ToModel()
	{
		return new OutFurnace(this);
	}
}