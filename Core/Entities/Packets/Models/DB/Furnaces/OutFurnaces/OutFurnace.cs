using Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;

public partial class OutFurnace : Furnace, IBaseEntity<OutFurnace, DTOOutFurnace>
{
	public int FTAPickUp { get; set; } // FTAPickUpFromPIT
	public DateTimeOffset? TSCentralConveyor { get; set; } // TSWhenAnodeInOutletCentralConveyor
	public DateTimeOffset? TSUnpackPIT { get; set; } // TSUnpackingOfPIT
}