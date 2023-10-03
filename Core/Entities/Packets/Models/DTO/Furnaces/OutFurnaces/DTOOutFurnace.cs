using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;

public partial class DTOOutFurnace : DTOFurnace, IDTO<OutFurnace, DTOOutFurnace>
{
	public int FTAPickUp { get; set; } // FTAPickUpFromPIT
	public DateTimeOffset? TSCentralConveyor { get; set; } // TSWhenAnodeInOutletCentralConveyor
	public DateTimeOffset? TSUnpackPIT { get; set; } // TSUnpackingOfPIT
}