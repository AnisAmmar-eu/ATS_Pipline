using Core.Entities.KPI.KPIEntries.Models.DB;
using Core.Entities.KPI.KPIEntries.Models.DTO;
using Core.Entities.KPI.KPIEntries.Repositories;
using Core.Shared.Services.Kernel;
using Core.Shared.UnitOfWork.Interfaces;

namespace Core.Entities.KPI.KPIEntries.Services;

public class KPIEntryService : ServiceBaseEntity<IKPIEntryRepository, KPIEntry, DTOKPIEntry>
{
	protected KPIEntryService(IAlarmUOW alarmUOW) : base(alarmUOW)
	{
	}
}