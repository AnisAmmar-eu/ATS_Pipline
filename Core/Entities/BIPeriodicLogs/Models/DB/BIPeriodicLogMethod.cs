using Core.Entities.BIPeriodicLogs.Models.DTO;
using Core.Shared.Models.DB.Kernel;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DB;

// BI = Business Intelligence
public partial class BIPeriodicLog : BaseEntity, IBaseEntity<BIPeriodicLog, DTOBIPeriodicLog>
{
	public override DTOBIPeriodicLog ToDTO()
	{
		return new DTOBIPeriodicLog(this);
	}
}