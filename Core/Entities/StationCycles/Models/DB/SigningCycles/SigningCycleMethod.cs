using Core.Entities.StationCycles.Models.DTO.SigningCycles;

namespace Core.Entities.StationCycles.Models.DB.SigningCycles;

public abstract partial class SigningCycle
{
	protected SigningCycle()
	{
	}

	protected SigningCycle(DTOSigningCycle dtoSigningCycle) : base(dtoSigningCycle)
	{
	}

	public override DTOSigningCycle ToDTO()
	{
		return new(this);
	}
}