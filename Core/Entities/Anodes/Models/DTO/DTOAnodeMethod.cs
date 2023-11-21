using Core.Entities.Anodes.Models.DB;
using Core.Entities.Anodes.Models.DTO.AnodesD20;
using Core.Entities.Anodes.Models.DTO.AnodesDX;

namespace Core.Entities.Anodes.Models.DTO;

public abstract partial class DTOAnode
{
	public DTOAnode(Anode anode)
	{
		S1S2CycleRID = anode.S1S2CycleRID;
		Status = anode.Status;
		ClosedTS = anode.ClosedTS;

		S1S2CycleID = anode.S1S2CycleID;
		S1S2CycleStationID = anode.S1S2CycleStationID;
		S1S2CycleTS = anode.S1S2CycleTS;
		S1S2Cycle = anode.S1S2Cycle.ToDTO();

		S3S4CycleID = anode.S3S4CycleID;
		S3S4CycleStationID = anode.S3S4CycleStationID;
		S3S4CycleTS = anode.S3S4CycleTS;
		S3S4Cycle = anode.S3S4Cycle?.ToDTO();
	}

	public override Anode ToModel()
	{
		return this switch
		{
			DTOAnodeD20 anodeD20 => anodeD20.ToModel(),
			DTOAnodeDX anodeDX => anodeDX.ToModel(),
			_ => throw new InvalidCastException("Trying to convert an abstract class to model")
		};
	}
}