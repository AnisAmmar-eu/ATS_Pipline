using Core.Entities.Packets.Models.DB.Furnaces.OutFurnaces;
using Core.Shared.Models.DTOs.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DTO.Furnaces.OutFurnaces;

public partial class DTOOutFurnace: DTOFurnace, IDTO<OutFurnace, DTOOutFurnace>
{
	public DateTimeOffset? TSUnpackPIT; // TSUnpackingOfPIT
	public DateTimeOffset? TSCentralConveyor; // TSWhenAnodeInOutletCentralConveyor
}