using Core.Entities.BIPeriodicLogs.Models.DTO.RT.DailyRTs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DB.RT.DailyRTs;

public partial class DailyRT : BIPeriodicLog, IBaseEntity<DailyRT, DTODailyRT>
{
}