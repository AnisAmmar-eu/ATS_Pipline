using Core.Entities.StationCycles.Models.DTO.SigningCycles;

namespace Core.Entities.StationCycles.Models.DB.LoadableCycles;

public abstract partial class LoadableCycle
{
	protected LoadableCycle()
	{
	}

	protected LoadableCycle(DTOSigningCycle dtoSigningCycle) : base(dtoSigningCycle)
	{
	}

	public override DTOSigningCycle ToDTO()
	{
		return new(this);
	}
}