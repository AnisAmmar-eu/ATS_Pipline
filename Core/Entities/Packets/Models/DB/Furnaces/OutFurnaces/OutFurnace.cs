using Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;

public partial class OutFurnace : Furnace, IBaseEntity<OutFurnace, DTOOutFurnace>
{
	public DateTimeOffset? TSUnpackPIT; // TSUnpackingOfPIT
	public DateTimeOffset? TSCentralConveyor; // TSWhenAnodeInOutletCentralConveyor
}