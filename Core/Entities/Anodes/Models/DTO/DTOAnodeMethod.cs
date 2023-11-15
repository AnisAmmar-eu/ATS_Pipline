using Core.Entities.Anodes.Models.DB;

namespace Core.Entities.Anodes.Models.DTO;

public partial class DTOAnode
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
}