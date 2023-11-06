using Core.Entities.Anodes.Models.DB.AnodesD20;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Anodes.Models.DTO.AnodesD20;

public partial class DTOAnodeD20 : DTOAnode, IDTO<AnodeD20, DTOAnodeD20>
{
	public DTOAnodeD20(AnodeD20 anodeD20) : base(anodeD20)
	{
	}
}