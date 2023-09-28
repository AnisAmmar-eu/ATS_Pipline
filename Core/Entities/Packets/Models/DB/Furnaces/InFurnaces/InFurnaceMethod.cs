using Core.Entities.Packets.Models.DTO.Furnaces.InFurnaces;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Packets.Models.DB.Furnaces.InFurnaces;

public partial class InFurnace : Furnace, IBaseEntity<InFurnace, DTOInFurnace>
{
	public override DTOInFurnace ToDTO(string? languageRID = null)
	{
		return new DTOInFurnace(this);
	}
}