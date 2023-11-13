using Core.Entities.StationCycles.Dictionaries;
using Core.Entities.StationCycles.Models.DB.S3S4Cycles;

namespace Core.Entities.StationCycles.Models.DTO.S3S4Cycles;

public partial class DTOS3S4Cycle
{
	public DTOS3S4Cycle()
	{
		CycleType = CycleTypes.S3S4;
	}

	public DTOS3S4Cycle(S3S4Cycle s3S4Cycle) : base(s3S4Cycle)
	{
		CycleType = CycleTypes.S3S4;
		AnnounceID = s3S4Cycle.AnnounceID;
		MatchingCamera1 = s3S4Cycle.MatchingCamera1;
		MatchingCamera2 = s3S4Cycle.MatchingCamera2;

		InFurnaceStatus = s3S4Cycle.InFurnaceStatus;
		InFurnaceID = s3S4Cycle.InFurnaceID;
		InFurnacePacket = s3S4Cycle.InFurnacePacket?.ToDTO();

		OutFurnaceStatus = s3S4Cycle.OutFurnaceStatus;
		OutFurnaceID = s3S4Cycle.OutFurnaceID;
		OutFurnacePacket = s3S4Cycle.OutFurnacePacket?.ToDTO();
	}

	public override S3S4Cycle ToModel()
	{
		return new S3S4Cycle(this);
	}
}