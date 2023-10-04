using Core.Entities.BIPeriodicLogs.Models.DB.RT.MonthlyRTs;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DTO.RT.MonthlyRTs;

public partial class DTOMonthlyRT : DTOBIPeriodicLog, IDTO<MonthlyRT, DTOMonthlyRT>
{
}