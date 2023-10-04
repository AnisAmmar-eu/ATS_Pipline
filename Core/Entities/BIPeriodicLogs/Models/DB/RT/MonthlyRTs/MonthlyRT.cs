using Core.Entities.BIPeriodicLogs.Models.DTO.RT.MonthlyRTs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DB.RT.MonthlyRTs;

public partial class MonthlyRT : BIPeriodicLog, IBaseEntity<MonthlyRT, DTOMonthlyRT>
{
}