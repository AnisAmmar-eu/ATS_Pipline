using Core.Entities.BIPeriodicLogs.Models.DB.RT.WeeklyRTs;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DTO.RT.WeeklyRTs;

public partial class DTOWeeklyRT : DTOBIPeriodicLog, IDTO<WeeklyRT, DTOWeeklyRT>
{
	public DTOWeeklyRT(WeeklyRT weeklyRT) : base(weeklyRT)
	{
	}
}