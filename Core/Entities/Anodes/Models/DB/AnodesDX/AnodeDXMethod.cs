using Core.Entities.Anodes.Models.DTO.AnodesDX;

namespace Core.Entities.Anodes.Models.DB.AnodesDX;

public partial class AnodeDX
{
	public override DTOAnodeDX ToDTO()
	{
		return new DTOAnodeDX(this);
	}
}