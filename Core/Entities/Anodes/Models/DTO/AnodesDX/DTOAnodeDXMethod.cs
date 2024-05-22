using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Anodes.Models.DB.AnodesDX;
using Mapster;

namespace Core.Entities.Anodes.Models.DTO.AnodesDX;

public partial class DTOAnodeDX
{
	public DTOAnodeDX()
	{
		AnodeType = AnodeTypes.DX;
	}

	public override AnodeDX ToModel()
	{
		AnodeDX anodeDX = this.Adapt<AnodeDX>();
		AnodeType = AnodeTypes.DX;
		anodeDX.S5Cycle = S5Cycle?.ToModel();
		return anodeDX;
	}
}