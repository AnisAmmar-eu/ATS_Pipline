using Core.Entities.StationCycles.Models.DTO.S3S4Cycles;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.StationCycles.Models.DB.S3S4Cycles;

public partial class S3S4Cycle : StationCycle, IBaseEntity<S3S4Cycle, DTOS3S4Cycle>
{
	public S3S4Cycle()
	{
		
	}
	public S3S4Cycle(DTOS3S4Cycle dtoS3S4Cycle) : base(dtoS3S4Cycle)
	{
		AnnounceID = dtoS3S4Cycle.AnnounceID;

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