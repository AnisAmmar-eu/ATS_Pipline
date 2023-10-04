using Core.Entities.BIPeriodicLogs.Models.DB.RT.DailyRTs;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DTO.RT.DailyRTs;

public partial class DTODailyRT : DTOBIPeriodicLog, IDTO<DailyRT, DTODailyRT>
{
}