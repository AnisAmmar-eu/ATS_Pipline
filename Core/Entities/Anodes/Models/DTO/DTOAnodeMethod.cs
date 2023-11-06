using Core.Entities.Anodes.Models.DB;
using Core.Shared.Models.DTO.Kernel;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.Anodes.Models.DTO;

public partial class DTOAnode : DTOBaseEntity, IDTO<Anode, DTOAnode>
{
	public DTOAnode(Anode anode)
	{
		S1S2CycleRID = anode.S1S2CycleRID;
		Status = anode.Status;
		ClosedTS = anode.ClosedTS;

		S1S2CycleID = anode.S1S2CycleID;
		S3S4CycleID = anode.S3S4CycleID;

		S3S4Cycle = anode.S3S4Cycle?.ToDTO();

		S1S2Cycle = anode.S1S2Cycle.ToDTO();
	}
}