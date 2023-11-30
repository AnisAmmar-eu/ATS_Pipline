using Core.Entities.Anodes.Models.DB.AnodesDX;

namespace Core.Entities.Anodes.Models.DTO.AnodesDX;

public partial class DTOAnodeDX
{
	public DTOAnodeDX(AnodeDX anodeDX) : base(anodeDX)
	{
		S5CycleID = anodeDX.S5CycleID;
		S5CycleTS = anodeDX.S5CycleTS;
		S5Cycle = anodeDX.S5Cycle?.ToDTO();
	}

	public override AnodeDX ToModel()
	{
		return new(this);
	}
}