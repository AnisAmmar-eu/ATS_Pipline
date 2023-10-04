using Core.Entities.BIPeriodicLogs.Models.DTO;
using Core.Entities.BIPeriodicLogs.Models.DTO.Entries.AnnualEntries;
using Core.Shared.Models.DB.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DB.Entries.AnnualEntries;

public partial class AnnualEntry : BIPeriodicLog, IBaseEntity<AnnualEntry, DTOAnnualEntry>
{
	public override DTOAnnualEntry ToDTO()
	{
		return new DTOAnnualEntry(this);
	}

	public AnnualEntry(DTOBIPeriodicLog dtoBIPeriodicLog) : base(dtoBIPeriodicLog)
	{
	}
}