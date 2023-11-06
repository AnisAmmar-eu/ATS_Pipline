using Core.Entities.Anodes.Models.DB.AnodesDX;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Anodes.Models.DTO.AnodesDX;

public partial class DTOAnodeDX : DTOAnode, IDTO<AnodeDX, DTOAnodeDX>
{
	public DTOAnodeDX(AnodeDX anodeDX) : base(anodeDX)
	{
		S5CycleID = anodeDX.S5CycleID;
		S5Cycle = anodeDX.S5Cycle?.ToDTO();
	}
}