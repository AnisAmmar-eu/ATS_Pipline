using Core.Entities.Anodes.Models.DTO.AnodesD20;

namespace Core.Entities.Anodes.Models.DB.AnodesD20;

public partial class AnodeD20
{
	public override DTOAnodeD20 ToDTO()
	{
		return new DTOAnodeD20(this);
	}
}