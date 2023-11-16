using Core.Entities.StationCycles.Models.DTO.SigningCycles;

namespace Core.Entities.StationCycles.Models.DB.SigningCycles;

public abstract partial class SigningCycle
{
	public SigningCycle()
	{
	}

	public SigningCycle(DTOSigningCycle dtoSigningCycle) : base(dtoSigningCycle)
	{
	}

	public override DTOSigningCycle ToDTO()
	{
		return new DTOSigningCycle(this);
	}
}