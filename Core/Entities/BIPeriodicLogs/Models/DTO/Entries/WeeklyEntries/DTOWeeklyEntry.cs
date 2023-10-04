using Core.Entities.BIPeriodicLogs.Models.DB.Entries.WeeklyEntries;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DTO.Entries.WeeklyEntries;

public partial class DTOWeeklyEntry : DTOBIPeriodicLog, IDTO<WeeklyEntry, DTOWeeklyEntry>
{
}