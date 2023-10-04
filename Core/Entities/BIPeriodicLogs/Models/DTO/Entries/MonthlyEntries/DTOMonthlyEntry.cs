using Core.Entities.BIPeriodicLogs.Models.DB.Entries.MonthlyEntries;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DTO.Entries.MonthlyEntries;

public partial class DTOMonthlyEntry : DTOBIPeriodicLog, IDTO<MonthlyEntry, DTOMonthlyEntry>
{
	
}