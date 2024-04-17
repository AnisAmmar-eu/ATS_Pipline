using Core.Entities.StationCycles.Models.DTO.LoadableCycles.S1S2Cycles;

namespace Core.Entities.StationCycles.Models.DB.LoadableCycles.S1S2Cycles;

public partial class S1S2Cycle
{
	public S1S2Cycle()
	{
	}

	public S1S2Cycle(DTOS1S2Cycle dtoS1S2Cycle) : base(dtoS1S2Cycle)
	{
    }

	public override DTOS1S2Cycle ToDTO() => new(this);
}