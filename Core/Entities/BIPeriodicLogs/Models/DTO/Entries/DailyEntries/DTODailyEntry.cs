using Core.Entities.BIPeriodicLogs.Models.DB.Entries.DailyEntries;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DTO.Entries.DailyEntries;

public partial class DTODailyEntry : DTOBIPeriodicLog, IDTO<DailyEntry, DTODailyEntry>
{
}