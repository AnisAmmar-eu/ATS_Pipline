using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S3S4Cycles;
using Mapster;

namespace Core.Entities.StationCycles.Models.DB.MatchableCycles.S3S4Cycles;

public partial class S3S4Cycle
{
	public S3S4Cycle()
	{
	}

	public override DTOS3S4Cycle ToDTO() => this.Adapt<DTOS3S4Cycle>();
}