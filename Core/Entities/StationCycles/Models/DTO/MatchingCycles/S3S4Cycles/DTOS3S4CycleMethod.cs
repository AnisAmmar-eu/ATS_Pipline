using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.MatchableCycles.S3S4Cycles;
using Mapster;

namespace Core.Entities.StationCycles.Models.DTO.MatchingCycles.S3S4Cycles;

public partial class DTOS3S4Cycle
{
	public DTOS3S4Cycle()
	{
		CycleType = CycleTypes.S3S4;
	}

	public override S3S4Cycle ToModel()
	{
		S3S4Cycle s3s4Cycle = this.Adapt<S3S4Cycle>();
		s3s4Cycle.InFurnacePacket = InFurnacePacket?.ToModel();
		s3s4Cycle.OutFurnacePacket = OutFurnacePacket?.ToModel();

		return s3s4Cycle;
	}
}