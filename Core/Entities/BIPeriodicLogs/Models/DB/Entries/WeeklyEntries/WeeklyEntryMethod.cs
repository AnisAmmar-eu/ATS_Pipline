using Core.Entities.BIPeriodicLogs.Models.DTO;
using Core.Entities.BIPeriodicLogs.Models.DTO.Entries.WeeklyEntries;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DB.Entries.WeeklyEntries;

public partial class WeeklyEntry : BIPeriodicLog, IBaseEntity<WeeklyEntry, DTOWeeklyEntry>
{
	public override DTOWeeklyEntry ToDTO()
	{
		return new DTOWeeklyEntry(this);
	}

	public WeeklyEntry(DTOBIPeriodicLog dtoBIPeriodicLog) : base(dtoBIPeriodicLog)
	{
	}
}