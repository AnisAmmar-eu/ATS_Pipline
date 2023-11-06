using Core.Entities.Anodes.Models.DTO.AnodesDX;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Anodes.Models.DB.AnodesDX;

public partial class AnodeDX : Anode, IBaseEntity<AnodeDX, DTOAnodeDX>
{
	public override DTOAnodeDX ToDTO()
	{
		return new DTOAnodeDX(this);
	}
}