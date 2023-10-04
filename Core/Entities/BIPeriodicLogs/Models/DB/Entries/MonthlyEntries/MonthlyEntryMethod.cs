using Core.Entities.BIPeriodicLogs.Models.DTO.Entries.MonthlyEntries;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DB.Entries.MonthlyEntries;

public partial class MonthlyEntry : BIPeriodicLog, IBaseEntity<MonthlyEntry, DTOMonthlyEntry>
{
	public override DTOMonthlyEntry ToDTO()
	{
		return new DTOMonthlyEntry(this);
	}
}