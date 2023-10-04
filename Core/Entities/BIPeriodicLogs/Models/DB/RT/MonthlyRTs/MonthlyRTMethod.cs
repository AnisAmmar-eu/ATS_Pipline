using Core.Entities.BIPeriodicLogs.Models.DTO;
using Core.Entities.BIPeriodicLogs.Models.DTO.RT.MonthlyRTs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DB.RT.MonthlyRTs;

public partial class MonthlyRT : BIPeriodicLog, IBaseEntity<MonthlyRT, DTOMonthlyRT>
{
	public override DTOMonthlyRT ToDTO()
	{
		return new DTOMonthlyRT(this);
	}

	public MonthlyRT(DTOBIPeriodicLog dtoBIPeriodicLog) : base(dtoBIPeriodicLog)
	{
	}
}