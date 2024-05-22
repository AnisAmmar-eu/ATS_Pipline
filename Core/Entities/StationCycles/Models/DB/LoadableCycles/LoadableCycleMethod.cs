using Core.Entities.StationCycles.Models.DTO.LoadableCycles;
using Mapster;

namespace Core.Entities.StationCycles.Models.DB.LoadableCycles;

public abstract partial class LoadableCycle
{
	protected LoadableCycle()
	{
	}

	public override DTOLoadableCycle ToDTO() => this.Adapt<DTOLoadableCycle>();
}