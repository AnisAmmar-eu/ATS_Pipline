using Core.Entities.StationCycles.Models.DTO.MatchingCycles.S3S4Cycles;

namespace Core.Entities.StationCycles.Models.DB.MatchingCycles.S3S4Cycles;

public partial class S3S4Cycle
{
	public S3S4Cycle()
	{
	}

	public S3S4Cycle(DTOS3S4Cycle dtoS3S4Cycle) : base(dtoS3S4Cycle)
	{
		AnnounceID = dtoS3S4Cycle.AnnounceID;
		MatchingCamera1 = dtoS3S4Cycle.MatchingCamera1;
		MatchingCamera2 = dtoS3S4Cycle.MatchingCamera2;

		InFurnaceStatus = dtoS3S4Cycle.InFurnaceStatus;
		InFurnaceID = dtoS3S4Cycle.InFurnaceID;
		InFurnacePacket = dtoS3S4Cycle.InFurnacePacket?.ToModel();

		OutFurnaceStatus = dtoS3S4Cycle.OutFurnaceStatus;
		OutFurnaceID = dtoS3S4Cycle.OutFurnaceID;
		OutFurnacePacket = dtoS3S4Cycle.OutFurnacePacket?.ToModel();
	}

	public override DTOS3S4Cycle ToDTO()
	{
		return new DTOS3S4Cycle(this);
	}
}