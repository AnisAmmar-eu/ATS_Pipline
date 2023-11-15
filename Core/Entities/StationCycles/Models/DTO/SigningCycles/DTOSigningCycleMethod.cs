using Core.Entities.StationCycles.Models.DB.SigningCycles;

namespace Core.Entities.StationCycles.Models.DTO.SigningCycles;

public partial class DTOSigningCycle
{
	public DTOSigningCycle()
	{
	}

	public DTOSigningCycle(SigningCycle cycle) : base(cycle)
	{
	}
}