using Core.Entities.BIPeriodicLogs.Models.DTO.Entries.DailyEntries;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DB.Entries.DailyEntries;

public partial class DailyEntry : BIPeriodicLog, IBaseEntity<DailyEntry, DTODailyEntry>
{
}