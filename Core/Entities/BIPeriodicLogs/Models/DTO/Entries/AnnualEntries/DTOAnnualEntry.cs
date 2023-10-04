using Core.Entities.BIPeriodicLogs.Models.DB.Entries.AnnualEntries;
using Core.Shared.Models.DTO.Kernel.Interfaces;

namespace Core.Entities.BIPeriodicLogs.Models.DTO.Entries.AnnualEntries;

public partial class DTOAnnualEntry : DTOBIPeriodicLog, IDTO<AnnualEntry, DTOAnnualEntry>
{
	
}