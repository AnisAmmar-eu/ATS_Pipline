using Core.Entities.StationCycles.Models.DTO.LoadableCycles;

namespace Core.Entities.StationCycles.Models.DB.LoadableCycles;

public abstract partial class LoadableCycle
{
	protected LoadableCycle()
	{
	}

	protected LoadableCycle(DTOLoadableCycle dtoLoadableCycle) : base(dtoLoadableCycle)
	{
	}

	public override DTOLoadableCycle ToDTO()
	{
		return new(this);
	}
}