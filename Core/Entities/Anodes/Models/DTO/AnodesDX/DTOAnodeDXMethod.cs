using Core.Entities.Anodes.Dictionaries;
using Core.Entities.Anodes.Models.DB.AnodesDX;

namespace Core.Entities.Anodes.Models.DTO.AnodesDX;

public partial class DTOAnodeDX
{
	public DTOAnodeDX()
	{
		AnodeType = AnodeTypes.DX;
	}

	public DTOAnodeDX(AnodeDX anodeDX) : base(anodeDX)
	{
		AnodeType = AnodeTypes.DX;
		S5CycleID = anodeDX.S5CycleID;
		S5CycleTS = anodeDX.S5CycleTS;
		S5Cycle = anodeDX.S5Cycle?.ToDTO();
	}

	public override AnodeDX ToModel()
	{
		return new(this);
	}
}