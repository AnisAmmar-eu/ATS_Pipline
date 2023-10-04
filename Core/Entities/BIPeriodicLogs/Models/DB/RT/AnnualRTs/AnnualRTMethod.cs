using Core.Entities.BIPeriodicLogs.Models.DTO;
using Core.Entities.BIPeriodicLogs.Models.DTO.RT.AnnualRTs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DB.RT.AnnualRTs;

public partial class AnnualRT : BIPeriodicLog, IBaseEntity<AnnualRT, DTOAnnualRT>
{
	public override DTOAnnualRT ToDTO()
	{
		return new DTOAnnualRT(this);
	}

	public AnnualRT(DTOBIPeriodicLog dtoBIPeriodicLog) : base(dtoBIPeriodicLog)
	{
	}
}