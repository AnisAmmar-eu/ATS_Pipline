using Core.Entities.BIPeriodicLogs.Models.DTO.RT.WeeklyRTs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DB.RT.WeeklyRTs;

public partial class WeeklyRT : BIPeriodicLog, IBaseEntity<WeeklyRT, DTOWeeklyRT>
{
	
}