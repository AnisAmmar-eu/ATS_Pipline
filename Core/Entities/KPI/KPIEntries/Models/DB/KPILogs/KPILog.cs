using Core.Entities.KPI.KPIEntries.Models.DTO.KPILogs;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Models.DB.KPILogs;

public partial class KPILog : KPIEntry, IBaseEntity<KPILog, DTOKPILog>
{
}