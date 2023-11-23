using Core.Entities.KPI.KPIEntries.Models.DB.KPILogs;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Models.DTO.KPILogs;

public partial class DTOKPILog : DTOKPIEntry, IDTO<KPILog, DTOKPILog>;