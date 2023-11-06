using Core.Entities.Anodes.Models.DTO.AnodesD20;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.Anodes.Models.DB.AnodesD20;

public partial class AnodeD20 : Anode, IBaseEntity<AnodeD20, DTOAnodeD20>
{
	public override DTOAnodeD20 ToDTO()
	{
		return new DTOAnodeD20(this);
	}
}